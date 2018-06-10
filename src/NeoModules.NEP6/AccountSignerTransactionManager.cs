using System;
using System.Collections.Generic;
using System.Linq;
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
using NeoModules.RPC.TransactionManagers;
using Helper = NeoModules.KeyPairs.Helper;

namespace NeoModules.NEP6
{
    public class AccountSignerTransactionManager : TransactionManagerBase
    {
        private static Dictionary<string, string> _systemAssets;
        private readonly KeyPair _accountKey;
        private readonly INeoRestService _restService;

        public AccountSignerTransactionManager(IClient rpcClient, INeoRestService restService, IAccount account)
        {
            Account = account ?? throw new ArgumentNullException(nameof(account));
            Client = rpcClient;
            _restService = restService;
            if (account.PrivateKey != null)
                _accountKey = new KeyPair(account.PrivateKey); //if account is watch only, it does not have private key
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

        private async Task<(List<ClaimableElement>, decimal amount)> GetClaimable(string address)
        {
            var claimableJson = await _restService.GetClaimableAsync(address);
            var claimable = Claimable.FromJson(claimableJson);
            var amount = claimable.Unclaimed;

            return (claimable.ClaimableList.ToList(), (decimal)amount);
        }

        private void SignTransaction(SignedTransaction txInput)
        {
            txInput.Sign(_accountKey);
        }

        private async Task<bool> SignAndSendTransaction(SignedTransaction txInput)
        {
            if (txInput == null) return false;
            SignTransaction(txInput);
            var serializedTransaction = txInput.Serialize();
            return await SendTransactionAsync(serializedTransaction.ToHexString());
        }

        private async Task<(List<SignedTransaction.Input> inputs, List<SignedTransaction.Output> outputs)>
            GenerateInputsOutputs(KeyPair key, string symbol, IEnumerable<TransactionOutput> targets)
        {
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
            List<TransactionOutput> transactionOutputs = null;
            if (targets != null)
            {
                transactionOutputs = targets.ToList();
                foreach (var target in transactionOutputs)
                {
                    if (target.AddressHash.SequenceEqual(fromHash))
                        throw new WalletException("Target can't be same as input");

                    cost += target.Amount;
                }
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

            if (cost > 0 && targets != null)
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

        public override Task<string> SignTransactionAsync(byte[] transactionData) //todo refractor this to have more use
        {
            var signerTransaction = Utils.Sign(transactionData, _accountKey.PrivateKey);
            return Task.FromResult(signerTransaction.ToHexString());
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

            SignTransaction(tx);
            var serializedScriptHash = tx.Serialize();
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
            var toScriptHash = toAddress.ToScriptHash().ToArray();
            var target = new TransactionOutput { AddressHash = toScriptHash, Amount = amount };
            var targets = new List<TransactionOutput> { target };
            return await SendAsset(_accountKey, symbol, targets);
        }

        public async Task<SignedTransaction> SendAsset(byte[] toAddress, string symbol, decimal amount)
        {
            var target = new TransactionOutput { AddressHash = toAddress, Amount = amount };
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

        public async Task<SignedTransaction> ClaimGas()
        {
            var address = Helper.CreateSignatureRedeemScript(_accountKey.PublicKey);
            var targetScriptHash = address.ToScriptHash();
            var (claimable, amount) = await GetClaimable(Wallet.ToAddress(address.ToScriptHash()));

            var references = new List<SignedTransaction.Input>();
            foreach (var entry in claimable)
                references.Add(new SignedTransaction.Input
                {
                    PrevHash = entry.Txid.HexToBytes().Reverse().ToArray(),
                    PrevIndex = entry.N
                });

            if (amount <= 0) throw new WalletException("No GAS available to claim at this address");

            var outputs = new List<SignedTransaction.Output>
            {
                new SignedTransaction.Output
                {
                    ScriptHash = targetScriptHash.ToArray(),
                    AssetId = Utils.GasToken.HexToBytes().Reverse().ToArray(),
                    Value = amount
                }
            };

            var tx = new SignedTransaction
            {
                Type = TransactionType.ClaimTransaction,
                Version = 0,
                Script = null,
                Gas = -1,
                References = references.ToArray(),
                Inputs = new SignedTransaction.Input[0],
                Outputs = outputs.ToArray()
            };

            tx.Sign(_accountKey);

            var result = await SignAndSendTransaction(tx);
            return result ? tx : null;
        }

        #endregion

        #region NEP5 Transfer

        public async Task<SignedTransaction> TransferNep5(string toAddress, decimal amount, byte[] tokenScriptHash,
            int decimals = 8)
        {
            var toScriptHash = toAddress.ToScriptHash().ToArray();
            return await TransferNep5(toScriptHash, amount, tokenScriptHash, decimals);
        }

        public async Task<SignedTransaction> TransferNep5(byte[] toAddress, decimal amount, byte[] tokenScriptHash,
            int decimals = 8)
        {
            if (toAddress.Length != 20) throw new ArgumentException(nameof(toAddress));
            if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount));

            var keyAddress = Helper.CreateSignatureRedeemScript(_accountKey.PublicKey);
            var fromAddress = keyAddress.ToScriptHash().ToArray();
            var amountBigInteger = Utils.ConvertToBigInt(amount, decimals);

            var result = await CallContract(tokenScriptHash,
                Nep5Methods.transfer.ToString(),
                new object[] { fromAddress, toAddress, amountBigInteger });

            return result;
        }

        #endregion
    }
}