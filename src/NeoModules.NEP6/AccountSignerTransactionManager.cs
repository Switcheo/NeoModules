using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using NeoModules.Core;
using NeoModules.JsonRpc.Client;
using NeoModules.KeyPairs;
using NeoModules.NEP6.Helpers;
using NeoModules.NEP6.Interfaces;
using NeoModules.NEP6.Transactions;
using NeoModules.Rest.Interfaces;
using NeoModules.Rest.Services;
using NeoModules.RPC;
using NeoModules.RPC.DTOs;
using NeoModules.RPC.Infrastructure;
using NeoModules.RPC.TransactionManagers;
using Org.BouncyCastle.Security;
using Helper = NeoModules.KeyPairs.Helper;
using Transaction = NeoModules.NEP6.Transactions.Transaction;
using TransactionOutput = NeoModules.NEP6.Transactions.TransactionOutput;
using Utils = NeoModules.NEP6.Helpers.Utils;

namespace NeoModules.NEP6
{
    public class AccountSignerTransactionManager : TransactionManagerBase, IRandomNumberGenerator
    {
        private static readonly SecureRandom Random = new SecureRandom();
        private readonly KeyPair _accountKey;
        private readonly INeoscanService _restService;
        public string Address => Helper.CreateSignatureRedeemScript(_accountKey.PublicKey).ToScriptHash().ToAddress();


        public AccountSignerTransactionManager(IClient rpcClient, INeoscanService restService, IAccount account)
        {
            Account = account ?? throw new ArgumentNullException(nameof(account));
            Client = rpcClient;
            _restService = restService;
            if (account.PrivateKey != null)
                _accountKey = new KeyPair(account.PrivateKey); //if account is watch only, it does not have private key
        }

        public byte[] GenerateNonce(int size)
        {
            var bytes = new byte[size];
            Random.NextBytes(bytes);
            return bytes;
        }

        /// <summary>
        /// Signs a Transaction object and sends a 'sendrawtransaction' RPC call to the connected node.
        /// </summary>
        /// <param name="txInput"></param>
        /// <returns></returns>
        private async Task<bool> SignAndSendTransaction(Transaction txInput)
        {
            if (txInput == null) return false;
            var serializedSignedTransaction = SignTransaction(txInput);
            return await SendTransactionAsync(serializedSignedTransaction.ToHexString());
        }

        /// <summary>
        /// Signs a Transaction object
        /// </summary>
        /// <param name="txInput"></param>
        public byte[] SignTransaction(Transaction txInput)
        {
            return txInput.Sign(_accountKey);
        }

        /// <summary>
        /// (Alternative) Signs a byte array that has the transaction data.
        /// </summary>
        /// <param name="transactionData"></param>
        /// <returns></returns>
        public override Task<string> SignTransactionAsync(byte[] transactionData) //todo refractor this to have more use
        {
            var signerTransaction = Utils.Sign(transactionData, _accountKey.PrivateKey);
            return Task.FromResult(signerTransaction.ToHexString());
        }


        /// <summary>
        /// Makes a 'getrawtransaction ' RPC call to the connected node.
        /// Only returns if the Transaction already has a block hash (indicates that is part of a block, therefore is confirmed)
        /// </summary>
        /// <param name="tx"></param>
        /// <returns></returns>
        public override async Task<RPC.DTOs.Transaction> WaitForTxConfirmation(string tx)
        {
            while (true)
            {
                var confirmedTx = await GetTransaction(tx);
                if (confirmedTx != null && !string.IsNullOrEmpty(confirmedTx.Blockhash)) return confirmedTx;
                await Task.Delay(10000);
            }
        }

        #region Contracts

        /// <summary>
        /// Makes a 'invokescript' RPC call to the connected node.
        /// Return the gas cost if the contract tx is "simulated" correctly
        /// </summary>
        /// <param name="scriptHash"></param>
        /// <param name="operation"></param>
        /// <param name="args"></param>
        /// <param name="attachSymbol"></param>
        /// <param name="attachTargets"></param>
        /// <returns></returns>
        //public async Task<double> EstimateGasContractCall(byte[] scriptHash, string operation,
        //    object[] args, string attachSymbol = null, IEnumerable<TransferOutput> attachTargets = null)
        //{
        //    var bytes = Utils.GenerateScript(scriptHash, operation, args);

        //    if (string.IsNullOrEmpty(attachSymbol)) attachSymbol = "GAS";

        //    if (attachTargets == null) attachTargets = new List<TransferOutput>();

        //    var (inputs, outputs) =
        //        await TransactionBuilderHelper.GenerateInputsOutputs(Address, attachSymbol, attachTargets, 0, _restService);

        //    if (inputs.Count == 0) throw new WalletException($"Not enough inputs for transaction");

        //    var tx = new InvocationTransaction()
        //    {
        //        Version = 0,
        //        Script = bytes,
        //        Gas = Fixed8.Zero,
        //        Inputs = inputs.ToArray(),
        //        Outputs = outputs.ToArray()
        //    };


        //    var serializedScriptHash = SignTransaction(tx);
        //    return await EstimateGasAsync(serializedScriptHash.ToHexString());
        //}

        /// <summary>
        /// Creates a 'InvocationTransaction' with the parameters passed, signs it and send a 'sendrawtransaction' RPC call to the connected node.
        /// </summary>
        /// <param name="contractScriptHash"></param>
        /// <param name="operation"></param>
        /// <param name="args"></param>
        /// <param name="attachSymbol"></param>
        /// <param name="attachTargets"></param>
        /// <returns></returns>
        //public async Task<Transaction> CallContract(byte[] contractScriptHash, string operation,
        //    object[] args, string attachSymbol = null, IEnumerable<TransferOutput> attachTargets = null)
        //{
        //    var bytes = Utils.GenerateScript(contractScriptHash, operation, args);
        //    return await CallContract(_accountKey, contractScriptHash, bytes, attachSymbol, attachTargets);
        //}

        /// <summary>
        /// (Alternative)
        /// Creates a 'InvocationTransaction' with the parameters passed, signs it and send a 'sendrawtransaction' RPC call to the connected node.
        /// But because there are no fees currently, you can execute contracts without assets, if there is no need for coinReference.
        /// </summary>
        /// <param name="scriptHash"></param>
        /// <param name="script"></param>
        /// <returns></returns>
        public async Task<Transaction> AssetlessContractCall(byte[] scriptHash, byte[] script)
        {
            var signatureScript = Helper.CreateSignatureRedeemScript(_accountKey.PublicKey).ToScriptHash();
            var tx = new InvocationTransaction()
            {
                Version = 0,
                Script = script,
                Gas = Fixed8.Zero,
                Inputs = new CoinReference[0],
                Outputs = new Transactions.TransactionOutput[0],
                Attributes = new[]
                {
                    new TransactionAttribute
                    {
                        Data = signatureScript.ToArray(),
                        Usage = TransactionAttributeUsage.Script
                    },
                    new TransactionAttribute //TODO: change this to use the same nonce in same tx to avoid duplicated tx
                    {
                        Data = GenerateNonce(4),
                        Usage = TransactionAttributeUsage.Remark
                    }
                }
            };
            var result = await SignAndSendTransaction(tx);
            return result ? tx : null;
        }

        //public async Task<Transaction> CallContract(KeyPair key, byte[] scriptHash, byte[] script,
        //    string attachSymbol = null, IEnumerable<TransferOutput> attachTargets = null, decimal fee = 0)
        //{
        //    if (string.IsNullOrEmpty(attachSymbol)) attachSymbol = "GAS";

        //    if (attachTargets == null) attachTargets = new List<TransferOutput>();

        //    var (inputs, outputs) =
        //        await TransactionBuilderHelper.GenerateInputsOutputs(Address, attachSymbol, attachTargets, fee, _restService);

        //    //Assetless contract call
        //    if (inputs.Count == 0 && outputs.Count == 0)
        //    {
        //        return await AssetlessContractCall(scriptHash, script);
        //    }

        //    var tx = new InvocationTransaction
        //    {
        //        Version = 0,
        //        Script = script,
        //        Gas = Fixed8.Zero,
        //        Inputs = inputs.ToArray(),
        //        Outputs = outputs.ToArray()
        //    };

        //    var result = await SignAndSendTransaction(tx);
        //    return result ? tx : null;
        //}

        #endregion

        #region Assets

        /// <summary>
        /// Sends a 'native' asset (Neo or Gas) to another address using 'sendrawtransaction'.
        /// </summary>
        /// <param name="toAddress"></param>
        /// <param name="symbol"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        //public async Task<Transaction> SendAsset(string toAddress, Dictionary<string, decimal> symbolsAndAmount, decimal fee = 0)
        //{
        //    var toScriptHash = toAddress.ToScriptHash().ToArray();
        //    var targets = TransactionBuilderHelper.BuildTransferOutputs(toAddress, symbolsAndAmount);
        //    //var fixedFee = fee == 0 ? Fixed8.Zero : Fixed8.FromDecimal(fee);
        //    return await SendAsset(_accountKey, targets, fee);
        //}

        /// <summary>
        /// Sends a 'native' asset (Neo or Gas) to another address using 'sendrawtransaction'.
        /// </summary>
        /// <param name="toAddress"></param>
        /// <param name="symbol"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        //public async Task<Transaction> SendAsset(byte[] toAddress, List<string> symbols, decimal amount, decimal fee = 0)
        //{
        //    var target = new TransferOutput { AddressHash = toAddress, Amount = amount };
        //    var targets = new List<TransferOutput> { target };
        //    var fixedFee = fee == 0 ? Fixed8.Zero : Fixed8.FromDecimal(fee);
        //    return await SendAsset(_accountKey, targets, fee);
        //}

        //public async Task<Transaction> SendAsset(KeyPair fromKey, IEnumerable<TransferOutput> targets, decimal fee)
        //{
        //    List<Transaction.CoinReference> finalInputs = new List<Transaction.CoinReference>();
        //    List<Transaction.TransactionOutput> finalOutputs = new List<Transaction.TransactionOutput>();

        //    foreach (var transferOutput in targets)
        //    {
        //        List<Transaction.CoinReference> inputs;
        //        List<Transaction.TransactionOutput> outputs;

        //        (inputs, outputs) =
        //            await TransactionBuilderHelper.GenerateInputsOutputs(Address,
        //                transferOutput.Symbol,
        //                new List<TransferOutput> { transferOutput },
        //                fee,
        //                _restService);
        //        finalInputs.AddRange(inputs);
        //        finalOutputs.AddRange(outputs);
        //    }

        //    var tx = new Transaction
        //    {
        //        Type = TransactionType.ContractTransaction,
        //        Version = 0,
        //        Script = null,
        //        Gas = -1,
        //        Inputs = finalInputs.ToArray(),
        //        Outputs = finalOutputs.ToArray()
        //    };

        //    var result = await SignAndSendTransaction(tx);
        //    return result ? tx : null;
        //}

        public async Task<ContractTransaction> NativeAssetTransaction(List<TransactionAttribute> attributes, IEnumerable<TransferOutput> outputs,
            UInt160 from = null, UInt160 change_address = null, Fixed8 fee = default(Fixed8)) //todo from change
        {
            ContractTransaction tx = new ContractTransaction();
            if (attributes == null) attributes = new List<TransactionAttribute>();
            tx.Attributes = attributes.ToArray();
            tx.Inputs = new CoinReference[0];
            tx.Outputs = outputs.Where(p => p.IsGlobalAsset).Select(p => p.ToTxOutput()).ToArray();
            tx.Witnesses = new Witness[0];
            tx = await MakeTransaction(tx, _accountKey.PublicKeyHash, change_address, fee);
            var success = await SignAndSendTransaction(tx);
            return success ? tx : null;
        }

        public async Task<T> MakeTransaction<T>(T tx, UInt160 from = null, UInt160 change_address = null, Fixed8 fee = default(Fixed8)) where T : Transaction
        {
            if (tx.Outputs == null) tx.Outputs = new TransactionOutput[0];
            if (tx.Attributes == null) tx.Attributes = new TransactionAttribute[0];
            fee += tx.SystemFee;
            var payTotal = tx.Outputs.GroupBy(p => p.AssetId, (k, g) => new
            {
                AssetId = k,
                Value = g.Sum(p => p.Value)
            }).ToDictionary(p => p.AssetId);
            if (fee > Fixed8.Zero)
            {
                if (payTotal.ContainsKey(Utils.GasToken))
                {
                    payTotal[Utils.GasToken] = new
                    {
                        AssetId = Utils.GasToken,
                        Value = payTotal[Utils.GasToken].Value + fee
                    };
                }
                else
                {
                    payTotal.Add(Utils.GasToken, new
                    {
                        AssetId = Utils.GasToken,
                        Value = fee
                    });
                }
            }
            var payCoins = payTotal.Select(async p => new
            {
                AssetId = p.Key,
                Unspents = await FindUnspentCoins(p.Key, p.Value.Value, from)
            }).Select(x => x.Result).ToDictionary(p => p.AssetId);

            if (payCoins.Any(p => p.Value.Unspents == null)) return null;

            var inputSum = payCoins.Values.ToDictionary(p => p.AssetId, p => new
            {
                p.AssetId,
                Value = p.Unspents.Sum(q => q.Output.Value)
            });
            if (change_address == null) change_address = from; //GetChangeAddress();
            List<TransactionOutput> outputsNew = new List<TransactionOutput>(tx.Outputs);
            foreach (UInt256 assetId in inputSum.Keys)
            {
                if (inputSum[assetId].Value > payTotal[assetId].Value)
                {
                    outputsNew.Add(new TransactionOutput
                    {
                        AssetId = assetId,
                        Value = inputSum[assetId].Value - payTotal[assetId].Value,
                        ScriptHash = change_address
                    });
                }
            }
            tx.Inputs = payCoins.Values.SelectMany(p => p.Unspents).Select(p => p.Reference).ToArray();
            tx.Outputs = outputsNew.ToArray();
            return tx;
        }

        private async Task<Coin[]> FindUnspentCoins(UInt256 assetId, Fixed8 amount, UInt160 from)
        {
            var address = Helper.CreateSignatureRedeemScript(_accountKey.PublicKey).ToScriptHash().ToAddress(); //todo
            var unspents = await TransactionBuilderHelper.GetUnspent(address, _restService);

            Coin[] unspents_asset = unspents.Where(p => p.Output.AssetId == assetId).ToArray();
            Fixed8 sum = unspents_asset.Sum(p => p.Output.Value);
            if (sum < amount) return null;
            if (sum == amount) return unspents_asset;
            Coin[] unspents_ordered = unspents_asset.OrderByDescending(p => p.Output.Value).ToArray();
            int i = 0;
            while (unspents_ordered[i].Output.Value <= amount)
                amount -= unspents_ordered[i++].Output.Value;
            if (amount == Fixed8.Zero)
                return unspents_ordered.Take(i).ToArray();
            else
                return unspents_ordered.Take(i).Concat(new[] { unspents_ordered.Last(p => p.Output.Value >= amount) }).ToArray();
        }


        /// <summary>
        /// Creates a 'ClaimTransaction', signs it and send a 'sendrawtransaction' RPC call to the connected node.
        /// </summary>
        /// <returns></returns>
        //public async Task<Transaction> ClaimGas()
        //{
        //    var address = Helper.CreateSignatureRedeemScript(_accountKey.PublicKey);
        //    var targetScriptHash = address.ToScriptHash();
        //    var (claimable, amount) =
        //        await TransactionBuilderHelper.GetClaimable(address.ToScriptHash().ToAddress(), _restService);

        //    var references = new List<Transaction.CoinReference>();
        //    foreach (var entry in claimable)
        //        references.Add(new Transaction.CoinReference
        //        {
        //            PrevHash = entry.Txid.HexToBytes().Reverse().ToArray(),
        //            PrevIndex = entry.N
        //        });

        //    if (amount <= 0) throw new WalletException("No GAS available to claim at this address");

        //    var outputs = new List<Transaction.TransactionOutput>
        //    {
        //        new Transaction.TransactionOutput
        //        {
        //            ScriptHash = targetScriptHash.ToArray(),
        //            AssetId = Utils.GasToken.HexToBytes().Reverse().ToArray(),
        //            Value = amount
        //        }
        //    };

        //    var tx = new Transaction
        //    {
        //        Type = TransactionType.ClaimTransaction,
        //        Version = 0,
        //        Script = null,
        //        Gas = -1,
        //        References = references.ToArray(),
        //        Inputs = new Transaction.CoinReference[0],
        //        Outputs = outputs.ToArray()
        //    };

        //    tx.Sign(_accountKey);

        //    var result = await SignAndSendTransaction(tx);
        //    return result ? tx : null;
        //}

        #endregion

        #region NEP5 Transfer

        /// <summary>
        /// Creates a 'InvocationTransaction' with the parameters passed, signs it and send a 'sendrawtransaction' RPC call to the connected node.
        /// Used the NEP5 standard 'tranfer' method.
        /// </summary>
        /// <param name="toAddress"></param>
        /// <param name="amount"></param>
        /// <param name="tokenScriptHash"></param>
        /// <param name="decimals"></param>
        ///// <returns></returns>
        //public async Task<Transaction> TransferNep5(string toAddress, decimal amount, byte[] tokenScriptHash,
        //    int decimals = 8)
        //{
        //    var toAddressScriptHash = toAddress.ToScriptHash().ToArray();
        //    return await TransferNep5(toAddressScriptHash, amount, tokenScriptHash, decimals);
        //}

        //public async Task<Transaction> TransferNep5(byte[] toAddress, decimal amount, byte[] tokenScriptHash,
        //    int decimals = 8)
        //{
        //    if (toAddress.Length != 20) throw new ArgumentException(nameof(toAddress));
        //    if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount));

        //    var keyAddress = Helper.CreateSignatureRedeemScript(_accountKey.PublicKey);
        //    var fromAddress = keyAddress.ToScriptHash().ToArray();
        //    var amountBigInteger = Utils.ConvertToBigInt(amount, decimals);

        //    var result = await CallContract(tokenScriptHash,
        //        Nep5Methods.transfer.ToString(),
        //        new object[] { fromAddress, toAddress, amountBigInteger });

        //    return result;
        //}
        #endregion
    }
}