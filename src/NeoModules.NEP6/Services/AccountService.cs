using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using NeoModules.Core;
using NeoModules.KeyPairs;
using NeoModules.NEP6.Interfaces;
using NeoModules.NEP6.Models;

namespace NeoModules.NEP6.Services
{
    public class AccountService
    {
        private readonly Wallet _wallet;

        protected IRandomNumberGenerator RandomGenerator;

        public AccountService(Wallet wallet, IRandomNumberGenerator randomGenerator)
        {
            _wallet = wallet;
            RandomGenerator = randomGenerator;
        }

        //Account Creation
        public Account ImportAccount(string label, string encryptedPrivateKey, string password)
        {
            KeyPair key = new KeyPair(Wallet.GetPrivateKeyFromNep2(encryptedPrivateKey, password, _wallet.Scrypt.N, _wallet.Scrypt.R, _wallet.Scrypt.P));
            var contract = new Contract
            {
                Script = KeyPairs.Helper.CreateSignatureRedeemScript(key.PublicKey),
                Parameters = new List<Parameter>
                {
                    new Parameter("signature",ParameterType.Signature)
                },
                Deployed = false
            };

            Account account = new Account(key.PublicKeyHash, label)
            {
                Nep2Key = encryptedPrivateKey,
                Contract = contract,
                IsDefault = false,
            };
            _wallet.AddAccount(account);
            return account;
        }

        public Account CreateAccount()
        {
            byte[] privateKey = new byte[32];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(privateKey);
            }
            KeyPair key = new KeyPair(privateKey);
            Contract contract = new Contract
            {
                Script = KeyPairs.Helper.CreateSignatureRedeemScript(key.PublicKey),
                Parameters = new List<Parameter>
                {
                    new Parameter("signature", ParameterType.Signature)
                },
                Deployed = false
            };
            var account = new Account(key.PublicKeyHash)
            {
                Contract = contract
            };
            Array.Clear(privateKey, 0, privateKey.Length);
            //AddAccount(account);
            return account;
        }

        //public bool DeleteAccount(string address)
        //{
        //    var scriptHash = KeyPairs.Helper.ToScriptHash(address);
        //    var account = Accounts.FirstOrDefault(p => p.Address == scriptHash);
        //    return DeleteAccount(account);
        //}

        //public Account GetAccount(string address)
        //{
        //    return GetAccount(KeyPairs.Helper.ToScriptHash(address));
        //}

        public KeyPair GetKey(string nep2Key, string password)
        {
            if (nep2Key == null) return null;
            var privKey = new KeyPair(Wallet.GetPrivateKeyFromNep2(nep2Key, password, _wallet.Scrypt.N,
                _wallet.Scrypt.R,
                _wallet.Scrypt.P));
            return privKey;
        }

        public bool VerifyPassword(string nep2Key, string password)
        {
            try
            {
                Wallet.GetPrivateKeyFromNep2(nep2Key, password, _wallet.Scrypt.N, _wallet.Scrypt.R, _wallet.Scrypt.P);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        //Account Managment
    }
}