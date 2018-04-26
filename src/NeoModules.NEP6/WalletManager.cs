using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Threading.Tasks;
using NeoModules.Core;
using NeoModules.KeyPairs;
using NeoModules.NEP6.Models;
using NeoModules.NVM;
using NeoModules.Rest.DTOs;
using NeoModules.Rest.Services;
using NeoModules.RPC.TransactionManagers;
using Helper = NeoModules.KeyPairs.Helper;
using Transaction = NeoModules.NEP6.Models.Transaction;

namespace NeoModules.NEP6
{
    /// <summary>
    ///     Class responsible for basic wallet management.
    /// </summary>
    public class WalletManager
    {
        private static Dictionary<string, string> _systemAssets;
        private readonly INeoRestService _restService;
        private readonly ITransactionManager _transactionManager;
        private readonly Wallet _wallet;

        public WalletManager(Wallet wallet, ITransactionManager transactionManager = null,
            INeoRestService restService = null)
        {
            _wallet = wallet;
            _transactionManager = transactionManager;
            _restService = restService;
        }

        /// <summary>
        ///     Adds or Updates an Account to the Accounts List on the associated Wallet.
        /// </summary>
        /// <param name="account">Account to be added/updated</param>
        public void AddAccount(Account account)
        {
            lock (_wallet.Accounts)
            {
                var oldAccount = _wallet.Accounts.FirstOrDefault(a => a.Address == account.Address);
                if (oldAccount != null) // update account
                {
                    oldAccount.IsDefault = account.IsDefault;
                    oldAccount.Nep2Key = account.Nep2Key;
                    oldAccount.IsLock = account.IsLock;
                    oldAccount.Label = account.Label;
                    oldAccount.Extra = account.Extra;
                    if (account.Contract == null)
                    {
                        account.Contract = oldAccount.Contract;
                    }
                    else
                    {
                        var oldContract = oldAccount.Contract;
                        if (oldContract != null)
                        {
                            var contract = account.Contract;
                            contract.Parameters = oldContract.Parameters;
                            contract.Deployed = oldContract.Deployed;
                        }
                    }
                }
                else // add new account to list
                {
                    _wallet.Accounts.Add(account);
                }
            }
        }

        /// <summary>
        ///     Returns the first Account on the Wallet that has IsDefault set to true. Else, returns the first one with that has
        ///     an encrypted key.
        /// </summary>
        /// <returns></returns>
        public Account GetDefaultAccount()
        {
            if (_wallet.Accounts.Count == 0) throw new ArgumentNullException("No accounts available in this Wallet");
            var defaultAccount = _wallet.Accounts.FirstOrDefault(a => a.IsDefault);
            if (defaultAccount != null) return defaultAccount;
            defaultAccount = _wallet.Accounts.FirstOrDefault(a => !string.IsNullOrEmpty(a.Nep2Key));
            return defaultAccount ?? _wallet.Accounts[0];
        }

        /// <summary>
        ///     Decrypts and add the account to the Wallet Account List, using WIF.
        /// </summary>
        /// <param name="wif"></param>
        /// <param name="label"></param>
        /// <returns></returns>
        public Account ImportAccount(string wif, string label)
        {
            var key = new KeyPair(Wallet.GetPrivateKeyFromWif(wif));
            var contract = new Contract
            {
                Script = Helper.CreateSignatureRedeemScript(key.PublicKey),
                Parameters = new List<Parameter>
                {
                    new Parameter("signature", ParameterType.Signature)
                },
                Deployed = false
            };
            var account = new Account(contract.ScriptHash)
            {
                Contract = contract,
                Label = label
            };
            AddAccount(account);
            return account;
        }

        /// <summary>
        ///     Decrypts and add the account to the Wallet Account List, using NEP2.
        /// </summary>
        /// <param name="label"></param>
        /// <param name="encryptedPrivateKey"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<Account> ImportAccount(string encryptedPrivateKey, string password, string label)
        {
            var privateKey = await Nep2.Decrypt(encryptedPrivateKey, password, _wallet.Scrypt);
            var key = new KeyPair(privateKey);
            Array.Clear(privateKey, 0, privateKey.Length);

            var contract = new Contract
            {
                Script = Helper.CreateSignatureRedeemScript(key.PublicKey),
                Parameters = new List<Parameter>
                {
                    new Parameter("signature", ParameterType.Signature)
                },
                Deployed = false
            };

            var account = new Account(contract.ScriptHash, label)
            {
                Nep2Key = encryptedPrivateKey,
                Contract = contract,
                IsDefault = false
            };
            AddAccount(account);
            return account;
        }

        /// <summary>
        ///     Creates an Account and returns it.
        /// </summary>
        /// <returns></returns>
        public Account CreateAccount(string label = null)
        {
            var privateKey = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(privateKey);
            }

            var key = new KeyPair(privateKey);
            Array.Clear(privateKey, 0, privateKey.Length);
            var contract = new Contract
            {
                Script = Helper.CreateSignatureRedeemScript(key.PublicKey),
                Parameters = new List<Parameter>
                {
                    new Parameter("signature", ParameterType.Signature)
                },
                Deployed = false
            };
            var account = new Account(key.PublicKeyHash, label)
            {
                Contract = contract
            };
            AddAccount(account);
            return account;
        }

        /// <summary>
        ///     Deletes the Account from the wallet, passing an address as parameter. Returns True if deleted, or false if not.
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public bool DeleteAccount(string address)
        {
            var scriptHash = address.ToScriptHash();
            var account = _wallet.Accounts.FirstOrDefault(p => p.Address == scriptHash);
            return DeleteAccount(account);
        }

        /// <summary>
        ///     Deletes the Account from the wallet, passing an Account object as parameter. Returns True if deleted, or false if
        ///     not.
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public bool DeleteAccount(Account account)
        {
            if (account == null) return false;
            lock (_wallet.Accounts)
            {
                bool deleted;
                deleted = _wallet.Accounts.Remove(account);
                return deleted;
            }
        }

        /// <summary>
        ///     Returns an Account that corresponds with the address provided.
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public Account GetAccount(string address)
        {
            return GetAccount(address.ToScriptHash());
        }

        /// <summary>
        ///     Returns an Account that corresponds with the scriptHash provided.
        /// </summary>
        /// <param name="scriptHash"></param>
        /// <returns></returns>
        public Account GetAccount(UInt160 scriptHash)
        {
            lock (_wallet.Accounts)
            {
                return _wallet.Accounts.FirstOrDefault(p => p.Address == scriptHash);
            }
        }

        /// <summary>
        ///     Gets the KeyPair object using the encrypted key and password pair.
        /// </summary>
        /// <param name="nep2Key"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<KeyPair> GetKey(string nep2Key, string password)
        {
            if (nep2Key == null) return null;
            var decryptedKey = await Nep2.Decrypt(nep2Key, password, _wallet.Scrypt);
            var privKey = new KeyPair(decryptedKey);
            return privKey;
        }

        /// <summary>
        ///     Verifies if the provided encrypted key and password are correct.
        /// </summary>
        /// <param name="nep2Key"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<bool> VerifyPassword(string nep2Key, string password)
        {
            try
            {
                await Nep2.Decrypt(nep2Key, password, _wallet.Scrypt);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }


        //TODO: move this to separate class/service because of SRP


        public async Task<Transaction> CallContract(KeyPair key, byte[] scriptHash, object[] args)
        {
            var bytes = GenerateScript(scriptHash, args);
            return await CallAndSignContract(key, scriptHash, bytes); //TODO changes .Result
        }

        public async Task<Transaction> CallContract(KeyPair key, byte[] scriptHash, string operation, object[] args)
        {
            return await CallContract(key, scriptHash, new object[] { operation, args });
        }

        public async Task<Transaction> CallAndSignContract(KeyPair key, byte[] scriptHash, byte[] bytes)
        {
            var gasCost = 0;

            GenerateInputsOutputs(key, scriptHash, new Dictionary<string, decimal> { { "GAS", gasCost } }, out var inputs,
                out var outputs);

            var tx = new Transaction
            {
                Type = 0xd1,
                Version = 0,
                Script = bytes,
                Gas = gasCost,
                Inputs = inputs.ToArray(),
                Outputs = outputs.ToArray()
            };

            tx.Sign(key);

            var hexTx = tx.Serialize();

            var ok = await _transactionManager.SendRawTransactionAsync(hexTx);
            return ok ? tx : null;
        }

        public async Task<Transaction> SendAsset(KeyPair fromKey, byte[] scriptHash,
            Dictionary<string, decimal> amounts)
        {
            GenerateInputsOutputs(fromKey, scriptHash, amounts, out var inputs, out var outputs);

            var tx = new Transaction
            {
                Type = 128,
                Version = 0,
                Script = null,
                Gas = -1,
                Inputs = inputs.ToArray(),
                Outputs = outputs.ToArray()
            };

            tx.Sign(fromKey);

            var hexTx = tx.Serialize();

            var ok = await _transactionManager.SendRawTransactionAsync(hexTx);
            return ok ? tx : null;
        }

        private void GenerateInputsOutputs(KeyPair key, byte[] outputHash, Dictionary<string, decimal> ammounts,
            out List<Transaction.Input> inputs, out List<Transaction.Output> outputs)
        {
            if (ammounts == null || ammounts.Count == 0) throw new WalletException("Invalid amount list");

            var address = Helper.CreateSignatureRedeemScript(key.PublicKey);
            var unspent = GetUnspent(Wallet.ToAddress(address.ToScriptHash())).Result;

            // filter any asset lists with zero unspent inputs
            unspent = unspent.Where(pair => pair.Value.Count > 0).ToDictionary(pair => pair.Key, pair => pair.Value);

            inputs = new List<Transaction.Input>();
            outputs = new List<Transaction.Output>();

            foreach (var entry in ammounts)
            {
                var symbol = entry.Key;
                if (!unspent.ContainsKey(symbol))
                    throw new WalletException($"Not enough {symbol} in address {Wallet.ToAddress(key.PublicKeyHash)}");

                var cost = entry.Value;

                string assetId;

                var info = GetAssetsInfo();
                if (info.ContainsKey(symbol))
                    assetId = info[symbol];
                else
                    throw new WalletException($"{symbol} is not a valid blockchain asset.");

                var sources = unspent[symbol];

                decimal selected = 0;
                foreach (var src in sources)
                {
                    selected += src.Value;

                    var input = new Transaction.Input
                    {
                        PrevHash = src.TxId,
                        PrevIndex = src.N
                    };

                    inputs.Add(input);

                    if (selected >= cost) break;
                }

                if (selected < cost) throw new ArgumentOutOfRangeException($"Not enough {symbol}");

                if (cost > 0)
                {
                    var output = new Transaction.Output
                    {
                        AssetId = assetId,
                        ScriptHash = GetStringFromScriptHash(outputHash),
                        Value = cost
                    };
                    outputs.Add(output);
                }

                if (selected > cost || cost == 0)
                {
                    var left = selected - cost;
                    var signatureScript = Helper.CreateSignatureRedeemScript(key.PublicKey);
                    var signatureHash = signatureScript.ToScriptHash();
                    var scripthash = Utils.ReverseHex(signatureHash.ToArray().ByteToHex());
                    var change = new Transaction.Output
                    {
                        AssetId = assetId,
                        // ScriptHash = Utils.ReverseHex(key.signatureHash.ToArray().ByteToHex()),
                        ScriptHash = scripthash,
                        Value = left
                    };
                    outputs.Add(change);
                }
            }
        }

        public static string GetStringFromScriptHash(byte[] hash)
        {
            return Utils.ReverseHex(hash.ToHexString());
        }

        public async Task<Dictionary<string, List<Unspent>>> GetUnspent(string address)
        {
            var response = await _restService.GetBalanceAsync(address);
            var addressBalance = AddressBalance.FromJson(response);

            var result = new Dictionary<string, List<Unspent>>();
            foreach (var balanceEntry in addressBalance.Balance)
            {
                var child = balanceEntry.Unspent;
                if (child?.Count > 0)
                {
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

                    foreach (var data in balanceEntry.Unspent)
                    {
                        var input = new Unspent
                        {
                            TxId = data.TxId,
                            N = data.N,
                            Value = data.Value
                        };

                        list.Add(input);
                    }
                }
            }

            return result;
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

        public struct UnspentEntry // todo: replace this with Unspent class from REST
        {
            public string Txid;
            public uint Index;
            public decimal Value;
        }

        public static byte[] GenerateScript(byte[] scriptHash, object[] args)
        {
            using (var sb = new ScriptBuilder())
            {
                var items = new Stack<object>();

                if (args != null)
                {
                    foreach (var item in args)
                    {
                        items.Push(item);
                    }
                }

                while (items.Count > 0)
                {
                    var item = items.Pop();
                    EmitObject(sb, item);
                }

                sb.EmitAppCall(scriptHash, false);

                var timestamp = DateTime.UtcNow.ToTimestamp();
                var nonce = BitConverter.GetBytes(timestamp);

                //sb.Emit(OpCode.THROWIFNOT);
                sb.Emit(OpCode.RET);
                sb.EmitPush(nonce);

                var bytes = sb.ToArray();

                string hex = bytes.ToHexString();
                //System.IO.File.WriteAllBytes(@"D:\code\Crypto\neo-debugger-tools\ICO-Template\bin\Debug\inputs.avm", bytes);

                return bytes;
            }
        }



        private static void EmitObject(ScriptBuilder sb, object item)
        {
            if (item is IEnumerable<byte>)
            {
                var arr = ((IEnumerable<byte>)item).ToArray();

                sb.EmitPush(arr);
            }
            else
            if (item is IEnumerable<object>)
            {
                var arr = ((IEnumerable<object>)item).ToArray();

                for (int index = arr.Length - 1; index >= 0; index--)
                {
                    EmitObject(sb, arr[index]);
                }

                sb.EmitPush(arr.Length);
                sb.Emit(OpCode.PACK);
            }
            else
            if (item == null)
            {
                sb.EmitPush("");
            }
            else
            if (item is string)
            {
                sb.EmitPush((string)item);
            }
            else
            if (item is bool)
            {
                sb.EmitPush((bool)item);
            }
            else
            if (item is BigInteger)
            {
                sb.EmitPush((BigInteger)item);
            }
            else
            {
                throw new Exception("Unsupported contract parameter: " + item.ToString());
            }
        }
    }
}