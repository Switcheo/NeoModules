using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using NeoModules.Core;
using NeoModules.Core.KeyPair;
using NeoModules.Core.NVM;
using NeoModules.JsonRpc.Client;
using NeoModules.NEP6.Helpers;
using NeoModules.NEP6.Interfaces;
using NeoModules.NEP6.TransactionManagers;
using NeoModules.NEP6.Transactions;
using NeoModules.Rest.Interfaces;
using NeoModules.RPC;
using Org.BouncyCastle.Security;
using Helper = NeoModules.Core.KeyPair.Helper;
using IAccount = NeoModules.NEP6.Interfaces.IAccount;
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

        public UInt160 AddressScriptHash => Helper.CreateSignatureRedeemScript(_accountKey.PublicKey).ToScriptHash();

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
        /// <param name="signed"></param>
        public byte[] SignTransaction(Transaction txInput, bool signed = true)
        {
            return txInput.Sign(_accountKey, signed);
        }

        /// <summary>
        /// Signs a message using ECDSA algo, with NIST P-256 curve and SHA-256 hash function
        /// </summary>
        /// <param name="messageToSign"></param>
        /// <returns></returns>
        public override string SignMessage(string messageToSign)
        {
            return Utils.Sign(messageToSign.HexToBytes(), _accountKey.PrivateKey).ToHexString();
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

        /// <summary>
        /// Makes a 'invokescript' RPC call to the connected node.
        /// Return the gas cost if the contract tx is "simulated" correctly
        /// </summary>
        /// <param name="scriptHash"></param>
        /// <param name="operation"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public async Task<decimal> EstimateGasContractInvocation(byte[] scriptHash, string operation,
            object[] args)
        {
            var bytes = Utils.GenerateScript(scriptHash.ToScriptHash(), operation, args);
            return await EstimateGasAsync(bytes.ToHexString());
        }

        /// <summary>
        /// Creates a 'ClaimTransaction', signs it and send a 'sendrawtransaction' RPC call to the connected node.
        /// This method does not put gas into claimable state. Can only claim 'unclaimable' amount. 
        /// </summary>
        /// <returns></returns>
        public async Task<ClaimTransaction> ClaimGas(UInt160 changeAddress = null)
        {
            var (claimable, amount) =
                await TransactionBuilderHelper.GetClaimable(AddressScriptHash.ToAddress(), _restService);

            if (amount <= 0) throw new WalletException("No GAS available to claim at this address");

            var tx = new ClaimTransaction();

            var references = new List<CoinReference>();
            foreach (var entry in claimable)
            {
                references.Add(new CoinReference
                {
                    PrevHash = UInt256.Parse(entry.Txid),
                    PrevIndex = (ushort)entry.N,
                });
            }

            if (changeAddress == null) changeAddress = AddressScriptHash;
            var outputs = new List<TransactionOutput>
            {
                new TransactionOutput
                {
                    ScriptHash = changeAddress,
                    AssetId = Utils.GasToken,
                    Value = Fixed8.FromDecimal(amount),
                }
            };
            tx.Version = 0;
            tx.Claims = references.ToArray();
            tx.Inputs = new CoinReference[0];
            tx.Outputs = outputs.ToArray();
            tx.Attributes = new TransactionAttribute[0];

            var result = await SignAndSendTransaction(tx);
            return result ? tx : null;
        }

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
            var tx = new InvocationTransaction()
            {
                Version = 0,
                Script = script,
                Gas = Fixed8.Zero,
                Inputs = new CoinReference[0],
                Outputs = new TransactionOutput[0],
                Attributes = new[]
                {
                    new TransactionAttribute
                    {
                        Data = AddressScriptHash.ToArray(),
                        Usage = TransactionAttributeUsage.Script
                    },
                    new TransactionAttribute //Nonce used to prevent hash colision
                    {
                        Data = GenerateNonce(4),
                        Usage = TransactionAttributeUsage.Remark
                    }
                }
            };
            var result = await SignAndSendTransaction(tx);
            return result ? tx : null;
        }

        /// <summary>
        /// Creates an InvocationTransactions. Serves to invoke a contract on the blockchain.
        /// It need the contract script hash, operation and operation arguments.
        /// This can be used for the "mintTokens" NEP5 method for example.
        /// </summary>
        /// <param name="contractScriptHash"></param>
        /// <param name="operation"></param>
        /// <param name="args"></param>
        /// <param name="outputs"></param>
        /// <param name="fee"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public async Task<Transaction> CallContract(string contractScriptHash, string operation,
            object[] args, IEnumerable<TransferOutput> outputs = null,
            decimal fee = 0, List<TransactionAttribute> attributes = null)
        {
            if (string.IsNullOrEmpty(contractScriptHash)) throw new ArgumentNullException(nameof(contractScriptHash));
            if (string.IsNullOrEmpty(operation)) throw new ArgumentNullException(nameof(operation));

            var script = Utils.GenerateScript(contractScriptHash, operation, args);

            if (attributes == null) attributes = new List<TransactionAttribute>();
            attributes.Add(new TransactionAttribute
            {
                Usage = TransactionAttributeUsage.Script,
                Data = AddressScriptHash.ToArray()
            });

            var tx = new InvocationTransaction
            {
                Version = 1,
                Script = script,
                Attributes = attributes.ToArray(),
                Inputs = new CoinReference[0],
                Outputs = outputs == null ? new TransactionOutput[0] : outputs.Where(p => p.IsGlobalAsset).Select(p => p.ToTxOutput()).ToArray(),
                Witnesses = new Witness[0]
            };

            var gasConsumed = await EstimateGasAsync(tx.Script.ToHexString());
            tx.Gas = InvocationTransaction.GetGas(Fixed8.FromDecimal(gasConsumed));

            tx = MakeTransaction(tx, AddressScriptHash, null, Fixed8.FromDecimal(fee));
            var success = await SignAndSendTransaction(tx);
            return success ? tx : null;
        }

        /// <summary>
        /// Transfer NEP5 tokens.
        /// </summary>
        /// <param name="attributes"></param>
        /// <param name="outputs"></param>
        /// <param name="changeAddress"></param>
        /// <param name="fee"></param>
        /// <returns></returns>
        public async Task<Transaction> TransferNep5(List<TransactionAttribute> attributes,
            IEnumerable<TransferOutput> outputs,
            UInt160 changeAddress = null, decimal fee = 0)
        {
            InvocationTransaction tx;
            var cOutputs = outputs.Where(p => !p.IsGlobalAsset).GroupBy(p => new
            {
                AssetId = (UInt160)p.AssetId,
                Account = p.ScriptHash
            }, (k, g) => new
            {
                k.AssetId,
                Value = g.Aggregate(BigInteger.Zero, (x, y) => x + y.Value.Value),
                k.Account
            }).ToArray();
            if (cOutputs.Length == 0)
            {
                return null;
            }
            var nep5Balances = await TransactionBuilderHelper.GetNep5Balances(AddressScriptHash.ToAddress(), _restService);

            using (ScriptBuilder sb = new ScriptBuilder())
            {
                foreach (var output in cOutputs)
                {
                    var nep5Balance = nep5Balances.SingleOrDefault(x => x.AssetHash == output.AssetId.ToString().Remove(0, 2));
                    if (nep5Balance == null)
                    {
                        throw new WalletException($"Not enough balance of: {output.AssetId} ");
                    }
                    sb.EmitAppCall(output.AssetId, Nep5Methods.transfer.ToString(), AddressScriptHash, output.Account, output.Value);
                    sb.Emit(OpCode.THROWIFNOT);
                }

                byte[] nonce = GenerateNonce(8);
                sb.Emit(OpCode.RET, nonce);
                tx = new InvocationTransaction
                {
                    Version = 1,
                    Script = sb.ToArray()
                };
            }

            if (attributes == null) attributes = new List<TransactionAttribute>();
            attributes.Add(new TransactionAttribute
            {
                Usage = TransactionAttributeUsage.Script,
                Data = AddressScriptHash.ToArray()
            });

            tx.Attributes = attributes.ToArray();
            tx.Inputs = new CoinReference[0];
            tx.Outputs = outputs.Where(p => p.IsGlobalAsset).Select(p => p.ToTxOutput()).ToArray();
            tx.Witnesses = new Witness[0];

            var gasConsumed = await EstimateGasAsync(tx.Script.ToHexString()); //todo add gas limit 
            tx.Gas = InvocationTransaction.GetGas(Fixed8.FromDecimal(gasConsumed));

            tx = MakeTransaction(tx, AddressScriptHash, changeAddress, Fixed8.FromDecimal(fee));
            var success = await SignAndSendTransaction(tx);
            return success ? tx : null;
        }

        /// <summary>
        /// Creates a ContractTransaction. It can only send "native" assets, such as NEO or/and GAS.
        /// </summary>
        /// <param name="attributes"></param>
        /// <param name="outputs"></param>
        /// <param name="changeAddress"></param>
        /// <param name="fee"></param>
        /// <returns></returns>
        public async Task<ContractTransaction> SendNativeAsset(List<TransactionAttribute> attributes,
            IEnumerable<TransferOutput> outputs,
            UInt160 changeAddress = null,
            decimal fee = 0) //todo from change
        {
            ContractTransaction tx = new ContractTransaction();
            if (attributes == null) attributes = new List<TransactionAttribute>();
            tx.Attributes = attributes.ToArray();
            tx.Inputs = new CoinReference[0];
            tx.Outputs = outputs.Where(p => p.IsGlobalAsset).Select(p => p.ToTxOutput()).ToArray();
            tx.Witnesses = new Witness[0];
            tx = MakeTransaction(tx, AddressScriptHash, changeAddress, Fixed8.FromDecimal(fee));
            var success = await SignAndSendTransaction(tx);
            return success ? tx : null;
        }

        /// <summary>
        /// Method that SendNativeAsset, TransferNep5 and CallContract uses to create the inputs and outputs of the transaction.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tx"></param>
        /// <param name="from"></param>
        /// <param name="changeAddress"></param>
        /// <param name="fee"></param>
        /// <returns></returns>
        public T MakeTransaction<T>(T tx, UInt160 from = null, UInt160 changeAddress = null, Fixed8 fee = default(Fixed8)) where T : Transaction
        {
            if (tx.Outputs == null) tx.Outputs = new TransactionOutput[0];
            if (tx.Attributes == null) tx.Attributes = new TransactionAttribute[0];
            if (from == null) from = AddressScriptHash;
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
                Unspents = await TransactionBuilderHelper.FindUnspentCoins(p.Key, p.Value.Value, from, _restService)
            }).Select(x => x.Result).ToDictionary(p => p.AssetId);

            if (payCoins.Any(p => p.Value.Unspents == null)) return null;

            var inputSum = payCoins.Values.ToDictionary(p => p.AssetId, p => new
            {
                p.AssetId,
                Value = p.Unspents.Sum(q => q.Output.Value)
            });
            if (changeAddress == null) changeAddress = from;
            List<TransactionOutput> outputsNew = new List<TransactionOutput>(tx.Outputs);
            foreach (UInt256 assetId in inputSum.Keys)
            {
                if (inputSum[assetId].Value > payTotal[assetId].Value)
                {
                    outputsNew.Add(new TransactionOutput
                    {
                        AssetId = assetId,
                        Value = inputSum[assetId].Value - payTotal[assetId].Value,
                        ScriptHash = changeAddress
                    });
                }
            }
            tx.Inputs = payCoins.Values.SelectMany(p => p.Unspents).Select(p => p.Reference).ToArray();
            tx.Outputs = outputsNew.ToArray();
            return tx;
        }
    }
}