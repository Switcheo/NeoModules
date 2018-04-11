using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using NeoModules.Core;
using NeoModules.KeyPairs;
using NeoModules.NEP6.Models;
using Helper = NeoModules.KeyPairs.Helper;

namespace NeoModules.NEP6
{
    /// <summary>
    ///     Class responsible for basic wallet management.
    /// </summary>
    public class WalletManager
    {
        private readonly Wallet _wallet;

        public WalletManager(Wallet wallet)
        {
            _wallet = wallet;
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
        /// Decrypts and add the account to the Wallet Account List, using WIF.
        /// </summary>
        /// <param name="wif"></param>
        /// <param name="label"></param>
        /// <returns></returns>
        public Account ImportAccount(string wif, string label = null)
        {
            KeyPair key = new KeyPair(Wallet.GetPrivateKeyFromWif(wif));
            Contract contract = new Contract
            {
                Script = Helper.CreateSignatureRedeemScript(key.PublicKey),
                Parameters = new List<Parameter>
                {
                    new Parameter("signature", ParameterType.Signature)
                },
                Deployed = false
            };
            Account account = new Account(contract.ScriptHash)
            {
                Contract = contract,
                Label = label
            };
            AddAccount(account);
            return account;
        }

        /// <summary>
        /// Decrypts and add the account to the Wallet Account List, using NEP2.
        /// </summary>
        /// <param name="label"></param>
        /// <param name="encryptedPrivateKey"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<Account> ImportAccount(string encryptedPrivateKey, string password, string label = null)
        {
            var privateKey = await Nep2.Decrypt(encryptedPrivateKey, password, _wallet.Scrypt);
            var key = new KeyPair(privateKey);
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
            var scriptHash = Helper.ToScriptHash(address);
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
            return GetAccount(Helper.ToScriptHash(address));
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
    }
}