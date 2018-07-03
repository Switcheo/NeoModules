using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NeoModules.Core;
using NeoModules.JsonRpc.Client;
using NeoModules.KeyPairs;
using NeoModules.NEP6.Helpers;
using NeoModules.NEP6.Interfaces;
using NeoModules.NEP6.Transactions;
using NeoModules.Rest.Services;
using NeoModules.RPC;
using NeoModules.RPC.Infrastructure;
using NeoModules.RPC.TransactionManagers;
using Org.BouncyCastle.Security;
using Helper = NeoModules.KeyPairs.Helper;

namespace NeoModules.NEP6
{
    public class AccountSignerTransactionManager : TransactionManagerBase, IRandomNumberGenerator
    {
        private static readonly SecureRandom Random = new SecureRandom();
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

        public byte[] GenerateRandomBytes(int size)
        {
            var bytes = new byte[size];
            Random.NextBytes(bytes);
            return bytes;
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

            var (inputs, outputs) =
                await TransactionBuilderHelper.GenerateInputsOutputs(_accountKey, attachSymbol, attachTargets, _restService);

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

            var (inputs, outputs) =
                await TransactionBuilderHelper.GenerateInputsOutputs(key, attachSymbol, attachTargets, _restService);

            SignedTransaction tx;
            //Asset less contract call
            if (inputs.Count == 0 && outputs.Count == 0)
            {
                var signatureScript = Helper.CreateSignatureRedeemScript(key.PublicKey).ToScriptHash();
                tx = new SignedTransaction
                {
                    Type = TransactionType.InvocationTransaction,
                    Version = 0,
                    Script = bytes,
                    Gas = 0,
                    Inputs = new SignedTransaction.Input[0],
                    Outputs = new SignedTransaction.Output[0],
                    Attributes = new[]
                    {
                        new TransactionAttribute
                        {
                            Data = signatureScript.ToArray(),
                            Usage = TransactionAttributeUsage.Script
                        },
                        new TransactionAttribute
                        {
                            Data = GenerateRandomBytes(4),
                            Usage = TransactionAttributeUsage.Remark
                        }
                    }
                };
            }
            else
            {
                tx = new SignedTransaction
                {
                    Type = TransactionType.InvocationTransaction,
                    Version = 0,
                    Script = bytes,
                    Gas = 0,
                    Inputs = inputs.ToArray(),
                    Outputs = outputs.ToArray()
                };
            }

            var result = await SignAndSendTransaction(tx);
            return result ? tx : null;
        }

        #endregion

        #region Assets

        public async Task<SignedTransaction> SendAsset(string toAddress, string symbol, decimal amount)
        {
            var toScriptHash = toAddress.ToScriptHash().ToArray();
            var target = new TransactionOutput {AddressHash = toScriptHash, Amount = amount};
            var targets = new List<TransactionOutput> {target};
            return await SendAsset(_accountKey, symbol, targets);
        }

        public async Task<SignedTransaction> SendAsset(byte[] toAddress, string symbol, decimal amount)
        {
            var target = new TransactionOutput {AddressHash = toAddress, Amount = amount};
            var targets = new List<TransactionOutput> {target};
            return await SendAsset(_accountKey, symbol, targets);
        }

        public async Task<SignedTransaction> SendAsset(KeyPair fromKey, string symbol,
            IEnumerable<TransactionOutput> targets)
        {
            var (inputs, outputs) =
                await TransactionBuilderHelper.GenerateInputsOutputs(fromKey, symbol, targets, _restService);

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
            var (claimable, amount) =
                await TransactionBuilderHelper.GetClaimable(address.ToScriptHash().ToAddress(), _restService);

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
            var toAddressScriptHash = toAddress.ToScriptHash().ToArray();
            return await TransferNep5(toAddressScriptHash, amount, tokenScriptHash, decimals);
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
                new object[] {fromAddress, toAddress, amountBigInteger});

            return result;
        }

        #endregion
    }
}