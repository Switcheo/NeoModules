using System;
using System.IO;
using NeoModules.Core;
using NeoModules.Core.KeyPair;
using NeoModules.Core.NVM;
using NeoModules.NEP6.Helpers;

namespace NeoModules.NEP6.Transactions
{
    public class Witness : ISerializable
    {
        public byte[] InvocationScript;
        public byte[] VerificationScript;

        public int Size => InvocationScript.GetVarSize() + VerificationScript.GetVarSize();

        private UInt160 _scriptHash;
        public virtual UInt160 ScriptHash
        {
            get
            {
                if (_scriptHash == null)
                {
                    _scriptHash = VerificationScript.ToScriptHash();
                }
                return _scriptHash;
            }
        }

        void ISerializable.Deserialize(BinaryReader reader)
        {
            throw new NotImplementedException();
        }

        void ISerializable.Serialize(BinaryWriter writer)
        {
            writer.WriteVarBytes(InvocationScript);
            writer.WriteVarBytes(VerificationScript);
        }
    }
}