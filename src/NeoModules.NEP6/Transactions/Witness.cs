using System.IO;
using NeoModules.Core;
using NeoModules.KeyPairs;

namespace NeoModules.NEP6.Transactions
{
    public class Witness
    {
        public byte[] InvocationScript;
        public byte[] VerificationScript;

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

        public void Serialize(BinaryWriter writer)
        {
            writer.WriteVarBytes(InvocationScript);
            writer.WriteVarBytes(VerificationScript);
        }

        public static Witness Unserialize(BinaryReader reader)
        {
            var invocationScript = reader.ReadVarBytes(65536);
            var verificationScript = reader.ReadVarBytes(65536);
            return new Witness {InvocationScript = invocationScript, VerificationScript = verificationScript};
        }
    }
}