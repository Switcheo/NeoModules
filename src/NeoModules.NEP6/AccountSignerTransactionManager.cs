using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using NeoModules.Core;
using NeoModules.JsonRpc.Client;
using NeoModules.KeyPairs;
using NeoModules.NEP6.Models;
using NeoModules.NEP6.Transactions;
using NeoModules.Rest.DTOs;
using NeoModules.Rest.Services;
using NeoModules.RPC;
using NeoModules.RPC.Infrastructure;
using NeoModules.RPC.Services;
using NeoModules.RPC.TransactionManagers;
using Helper = NeoModules.KeyPairs.Helper;

namespace NeoModules.NEP6
{
    public class AccountSignerTransactionManager : TransactionManagerBase
    {
        private static Dictionary<string, string> _systemAssets;
        private readonly KeyPair _accountKey;
        private readonly INeoRestService _restService;
        private NeoNep5Service _nep5Service;

        public AccountSignerTransactionManager(IClient rpcClient, INeoRestService restService, IAccount account)
        {
            Account = account ?? throw new ArgumentNullException(nameof(account));
            Client = rpcClient;
            _restService = restService;
            if (account.PrivateKey != null) _accountKey = new KeyPair(account.PrivateKey); //if account is watch only, it does not have private key
        }

        //if you want to use nep5 tranfer method, you need to init this
        public void InitializeNep5Service(string tokenScriptHash)
        {
            _nep5Service = tokenScriptHash.StartsWith("0x") ? new NeoNep5Service(Client, tokenScriptHash.Substring(2)) : new NeoNep5Service(Client, tokenScriptHash);
        }

        private async Task<Dictionary<string, List<Unspent>>> GetUnspent(string address)
        {
            if (_restService == null) throw new NullReferenceException("REST client not configured");
            var response = await _restService.GetBalanceAsync(address);
            var addressBalance = AddressBalance.FromJson(response);

            var result = new Dictionary<string, List<Unspent>>();
            foreach (var balanceEntry in addressBalance.Balance)
            {
                var child = balanceEntry.Unspent;
                if (!(child?.Count > 0)) continue;
                List<Unspent> list;
                if (result.ContainsKey(balanceEntry.Asset))
                {
                    list = result[balanceEntry.Asset];
                }
                else
                {
                    list = new List<Unspent>();
                    result[balanceEntry.Asset] = list;
                }

                list.AddRange(balanceEntry.Unspent.Select(data => new Unspent
                {
                    TxId = data.TxId,
                    N = data.N,
                    Value = data.Value
                }));
            }

            return result;
        }

        private static byte[] SignTransaction(KeyPair key, SignedTransaction txInput)
        {
            txInput.Sign(key);
            return txInput.Serialize();
        }

        private async Task<bool> SignAndSendTransaction(SignedTransaction txInput)
        {
            if (txInput == null) return false;
            var signedTransaction = SignTransaction(_accountKey, txInput);
            return await SendTransactionAsync(signedTransaction.ToHexString());
        }

        private async Task<(List<SignedTransaction.Input> inputs, List<SignedTransaction.Output> outputs)>
            GenerateInputsOutputs(KeyPair key, string symbol, IEnumerable<TransactionOutput> targets)
        {
            if (targets == null) throw new WalletException("Invalid amount list");

            var address = Helper.CreateSignatureRedeemScript(key.PublicKey);
            var unspent = await GetUnspent(Wallet.ToAddress(address.ToScriptHash()));

            // filter any asset lists with zero unspent inputs
            unspent = unspent.Where(pair => pair.Value.Count > 0).ToDictionary(pair => pair.Key, pair => pair.Value);

            var inputs = new List<SignedTransaction.Input>();
            var outputs = new List<SignedTransaction.Output>();

            string assetId;

            var info = GetAssetsInfo();
            if (info.ContainsKey(symbol))
                assetId = info[symbol];
            else
                throw new WalletException($"{symbol} is not a valid blockchain asset.");

            if (!unspent.ContainsKey(symbol)) throw new WalletException($"Not enough {symbol} in address {address}");

            decimal cost = 0;

            var fromHash = key.PublicKeyHash.ToArray();
            var transactionOutputs = targets.ToList();
            foreach (var target in transactionOutputs)
            {
                if (target.AddressHash.SequenceEqual(fromHash))
                    throw new WalletException("Target can't be same as input");

                cost += target.Amount;
            }

            var sources = unspent[symbol];
            decimal selected = 0;

            foreach (var src in sources)
            {
                selected += (decimal)src.Value;

                var input = new SignedTransaction.Input
                {
                    PrevHash = src.TxId.HexToBytes().Reverse().ToArray(),
                    PrevIndex = src.N
                };

                inputs.Add(input);

                if (selected >= cost) break;
            }

            if (selected < cost) throw new WalletException($"Not enough {symbol}");


            if (cost > 0)
                foreach (var target in transactionOutputs)
                {
                    var output = new SignedTransaction.Output
                    {
                        AssetId = assetId.HexToBytes().Reverse().ToArray(),
                        ScriptHash = target.AddressHash.ToArray(),
                        Value = target.Amount
                    };
                    outputs.Add(output);
                }


            if (selected > cost || cost == 0)
            {
                var left = selected - cost;
                var signatureScript = Helper.CreateSignatureRedeemScript(key.PublicKey).ToScriptHash();
                var change = new SignedTransaction.Output
                {
                    AssetId = assetId.HexToBytes().Reverse().ToArray(),
                    ScriptHash = signatureScript.ToArray(),
                    Value = left
                };
                outputs.Add(change);
            }

            return (inputs, outputs);
        }

        internal static Dictionary<string, string> GetAssetsInfo()
        {
            if (_systemAssets == null)
            {
                _systemAssets = new Dictionary<string, string>();
                AddAsset("NEO", "c56f33fc6ecfcd0c225c4ab356fee59390af8560be0e930faebe74a6daff7c9b");
                AddAsset("GAS", "602c79718b16e442de58778e148d0b1084e3b2dffd5de6b7b16cee7969282de7");
            }

            return _systemAssets;
        }

        private static void AddAsset(string symbol, string hash)
        {
            _systemAssets[symbol] = hash;
        }

        #region Contracts

        public async Task<double> EstimateGasContractCall(byte[] scriptHash, string operation,
            object[] args, string attachSymbol = null, IEnumerable<TransactionOutput> attachTargets = null)
        {
            var bytes = Utils.GenerateScript(scriptHash, operation, args);

            if (string.IsNullOrEmpty(attachSymbol)) attachSymbol = "GAS";

            if (attachTargets == null) attachTargets = new List<TransactionOutput>();

            var (inputs, outputs) = await GenerateInputsOutputs(_accountKey, attachSymbol, attachTargets);

            if (inputs.Count == 0) throw new WalletException($"Not enough inputs for transaction");

            var tx = new SignedTransaction
            {
                Type = TransactionType.InvocationTransaction,
                Version = 0,
                Script = bytes,
                Gas = 0,
                Inputs = inputs.ToArray(),
                Outputs = outputs.ToArray()
            };

            var serializedScriptHash = SignTransaction(_accountKey, tx);

            return await EstimateGasAsync(serializedScriptHash.ToHexString());
        }

        public async Task<SignedTransaction> CallContract(byte[] scriptHash, string operation,
            object[] args, string attachSymbol = null, IEnumerable<TransactionOutput> attachTargets = null)
        {
            var bytes = Utils.GenerateScript(scriptHash, operation, args);
            return await CallContract(_accountKey, scriptHash, bytes, attachSymbol, attachTargets);
        }

        public async Task<SignedTransaction> CallContract(KeyPair key, byte[] scriptHash, byte[] bytes,
            string attachSymbol = null, IEnumerable<TransactionOutput> attachTargets = null)
        {
            if (string.IsNullOrEmpty(attachSymbol)) attachSymbol = "GAS";

            if (attachTargets == null) attachTargets = new List<TransactionOutput>();

            var (inputs, outputs) = await GenerateInputsOutputs(key, attachSymbol, attachTargets);

            if (inputs.Count == 0) throw new WalletException($"Not enough inputs for transaction");

            var tx = new SignedTransaction
            {
                Type = TransactionType.InvocationTransaction,
                Version = 0,
                Script = bytes,
                Gas = 0,
                Inputs = inputs.ToArray(),
                Outputs = outputs.ToArray()
            };

            var result = await SignAndSendTransaction(tx);
            return result ? tx : null;
        }

        #endregion

        #region Assets

        public async Task<SignedTransaction> SendAsset(string toAddress, string symbol, decimal amount)
        {
            var toScriptHash = toAddress.GetScriptHashFromAddress();
            var target = new TransactionOutput { AddressHash = toScriptHash, Amount = amount };
            var targets = new List<TransactionOutput> { target };
            return await SendAsset(_accountKey, symbol, targets);
        }

        public async Task<SignedTransaction> SendAsset(KeyPair fromKey, string symbol,
            IEnumerable<TransactionOutput> targets)
        {
            var (inputs, outputs) = await GenerateInputsOutputs(fromKey, symbol, targets);

            var tx = new SignedTransaction
            {
                Type = TransactionType.ContractTransaction,
                Version = 0,
                Script = null,
                Gas = -1,
                Inputs = inputs.ToArray(),
                Outputs = outputs.ToArray()
            };

            var result = await SignAndSendTransaction(tx);
            return result ? tx : null;
        }

        #endregion


        //TODO move this to another class

        #region NEP5

        public void ChangeNep5ServiceToken(string tokenScriptHash)
        {
            _nep5Service.ChangeTokenScripHash(tokenScriptHash);
        }

        public async Task<string> GetNep5Name(string tokenScriptHash = null)
        {
            if (_nep5Service == null) throw new ArgumentNullException($"NEP5 service not configured");
            if (!string.IsNullOrEmpty(tokenScriptHash)) ChangeNep5ServiceToken(tokenScriptHash);

            var result = await _nep5Service.GetName();
            if (string.IsNullOrEmpty(result)) return string.Empty;

            return result.HexToString();
        }

        public async Task<string> GetNep5Symbol(string tokenScriptHash = null)
        {
            if (_nep5Service == null) throw new ArgumentNullException($"NEP5 service not configured");
            if (!string.IsNullOrEmpty(tokenScriptHash)) ChangeNep5ServiceToken(tokenScriptHash);

            var result = await _nep5Service.GetSymbol();
            if (string.IsNullOrEmpty(result)) return string.Empty;

            return result.HexToString();
        }

        public async Task<int> GetNep5Decimals(string tokenScriptHash = null)
        {
            if (_nep5Service == null) throw new ArgumentNullException($"NEP5 service not configured");
            if (!string.IsNullOrEmpty(tokenScriptHash)) ChangeNep5ServiceToken(tokenScriptHash);

            var result = await _nep5Service.GetDecimals();
            if (string.IsNullOrEmpty(result) || string.Equals(result, "[]")) return 0;

            return int.Parse(result);
        }

        public async Task<double> GetNep5TotalSupply(int decimals, string tokenScriptHash = null) //todo support decimal points
        {
            if (_nep5Service == null) throw new ArgumentNullException($"NEP5 service not configured");
            if (!string.IsNullOrEmpty(tokenScriptHash)) ChangeNep5ServiceToken(tokenScriptHash);

            var result = await _nep5Service.GetTotalSupply();
            if (string.IsNullOrEmpty(result)) return 0;

            var supplyBigInteger = new BigInteger(result.HexToBytes());

            var totalSupply = supplyBigInteger / Utils.DecimalToBigInteger(decimals);
            return (double)totalSupply;
        }

        public async Task<decimal> GetNep5BalanceOf(int decimals, string tokenScriptHash = null) //todo support decimal points
        {
            if (_nep5Service == null) throw new ArgumentNullException($"NEP5 service not configured");
            if (!string.IsNullOrEmpty(tokenScriptHash)) ChangeNep5ServiceToken(tokenScriptHash);

            var result = await _nep5Service.GetBalance(Account.Address);
            if (string.IsNullOrEmpty(result)) return 0;

            var balanceBigInteger = new BigInteger(result.HexToBytes());
            var balance = balanceBigInteger / Utils.DecimalToBigInteger(decimals);

            return (decimal)balance;
        }

        public async Task<string> TransferNep5(byte[] toAddress, decimal amount, byte[] tokenScriptHash = null)
        {
            if (toAddress.Length != 20) throw new ArgumentException(nameof(toAddress));
            if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount));

            var fromAddress = _accountKey.PublicKeyHash.ToArray();

            var result = await CallContract(tokenScriptHash,
                Nep5Methods.transfer.ToString(),
                new object[] { fromAddress, toAddress, amount });

            if (result == null) return string.Empty;
            return result.Hash.ToString();
        }

        #endregion
    }
}