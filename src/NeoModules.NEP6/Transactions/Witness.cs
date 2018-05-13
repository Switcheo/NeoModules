using System.IO;

namespace NeoModules.NEP6.Transactions
{
    public struct Witness
    {
        public byte[] InvocationScript;
        public byte[] VerificationScript;

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