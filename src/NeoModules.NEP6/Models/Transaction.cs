using System;
using System.IO;
using System.Linq;
using NeoModules.Core;
using NeoModules.KeyPairs;
using NeoModules.KeyPairs.Cryptography.ECC;
using Helper = NeoModules.KeyPairs.Helper;

namespace NeoModules.NEP6.Models
{
    public enum AssetType : byte
    {
        CreditFlag = 0x40,
        DutyFlag = 0x80,

        GoverningToken = 0x00,
        UtilityToken = 0x01,
        Currency = 0x08,
        Share = DutyFlag | 0x10,
        Invoice = DutyFlag | 0x18,
        Token = CreditFlag | 0x20
    }


    public enum TransactionAttributeUsage
    {
        ContractHash = 0x00,
        ECDH02 = 0x02,
        ECDH03 = 0x03,
        Script = 0x20,
        Vote = 0x30,
        DescriptionUrl = 0x81,
        Description = 0x90,

        Hash1 = 0xa1,
        Hash2 = 0xa2,
        Hash3 = 0xa3,
        Hash4 = 0xa4,
        Hash5 = 0xa5,
        Hash6 = 0xa6,
        Hash7 = 0xa7,
        Hash8 = 0xa8,
        Hash9 = 0xa9,
        Hash10 = 0xaa,
        Hash11 = 0xab,
        Hash12 = 0xac,
        Hash13 = 0xad,
        Hash14 = 0xae,
        Hash15 = 0xaf,

        Remark = 0xf0,
        Remark1 = 0xf1,
        Remark2 = 0xf2,
        Remark3 = 0xf3,
        Remark4 = 0xf4,
        Remark5 = 0xf5,
        Remark6 = 0xf6,
        Remark7 = 0xf7,
        Remark8 = 0xf8,
        Remark9 = 0xf9,
        Remark10 = 0xfa,
        Remark11 = 0xfb,
        Remark12 = 0xfc,
        Remark13 = 0xfd,
        Remark14 = 0xfe,
        Remark15 = 0xff
    }

    public struct TransactionAttribute
    {
        public TransactionAttributeUsage Usage;
        public byte[] Data;

        public static TransactionAttribute Unserialize(BinaryReader reader)
        {
            var Usage = (TransactionAttributeUsage) reader.ReadByte();

            byte[] Data;

            if (Usage == TransactionAttributeUsage.ContractHash || Usage == TransactionAttributeUsage.Vote ||
                Usage >= TransactionAttributeUsage.Hash1 && Usage <= TransactionAttributeUsage.Hash15)
                Data = reader.ReadBytes(32);
            else if (Usage == TransactionAttributeUsage.ECDH02 || Usage == TransactionAttributeUsage.ECDH03)
                Data = new[] {(byte) Usage}.Concat(reader.ReadBytes(32)).ToArray();
            else if (Usage == TransactionAttributeUsage.Script)
                Data = reader.ReadBytes(20);
            else if (Usage == TransactionAttributeUsage.DescriptionUrl)
                Data = reader.ReadBytes(reader.ReadByte());
            else if (Usage == TransactionAttributeUsage.Description || Usage >= TransactionAttributeUsage.Remark)
                Data = reader.ReadVarBytes(ushort.MaxValue);
            else
                throw new NotImplementedException();

            return new TransactionAttribute {Usage = Usage, Data = Data};
        }
    }

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


    public enum TransactionType : byte
    {
        MinerTransaction = 0x00,
        IssueTransaction = 0x01,
        ClaimTransaction = 0x02,
        EnrollmentTransaction = 0x20,
        RegisterTransaction = 0x40,
        ContractTransaction = 0x80,
        StateTransaction = 0x90,
        PublishTransaction = 0xd0,
        InvocationTransaction = 0xd1
    }

    public class Transaction
    {
        private UInt256 _hash;
        public TransactionAttribute[] Attributes;
        public decimal Gas;

        public Input[] Inputs;
        public Output[] Outputs;

        public Input[] References;
        public byte[] Script;

        public TransactionType Type;
        public byte Version;
        public Witness[] Witnesses;


        /// <summary>
        ///     If using Xamarin, you should set this to true, because Xamarin does not support implementation for creating nist
        ///     P-256 ECDSA signatures
        /// </summary>
        public static bool SendFromMobile { get; set; } = false;

        public UInt256 Hash
        {
            get
            {
                if (_hash == null)
                {
                    var rawTx = Serialize(false);
                    _hash = new UInt256(Helper.Hash256(rawTx));
                }

                return _hash;
            }
        }

        public byte[] Serialize(bool signed = true)
        {
            using (var stream = new MemoryStream())
            {
                using (var writer = new BinaryWriter(stream))
                {
                    writer.Write((byte) Type);
                    writer.Write(Version);

                    // exclusive data
                    switch (Type)
                    {
                        case TransactionType.InvocationTransaction:
                        {
                            writer.WriteVarInt(Script.Length);
                            writer.Write(Script);
                            if (Version >= 1) writer.WriteFixed(Gas);

                            break;
                        }
                    }

                    // Don't need any attributes
                    writer.Write((byte) 0);

                    writer.WriteVarInt(Inputs.Length);
                    foreach (var input in Inputs) SerializeTransactionInput(writer, input);

                    writer.WriteVarInt(Outputs.Length);
                    foreach (var output in Outputs) SerializeTransactionOutput(writer, output);

                    if (signed && Witnesses != null)
                    {
                        writer.WriteVarInt(Witnesses.Length);
                        foreach (var witness in Witnesses) witness.Serialize(writer);
                    }
                }

                return stream.ToArray();
            }
        }


        public void Sign(KeyPair key)
        {
            var txdata = Serialize(false);

            var privkey = key.PrivateKey;
            var signature = Utils.Sign(txdata, privkey);

            var invocationScript = ("40" + signature.ToHexString()).HexToBytes();
            var verificationScript = Helper.CreateSignatureRedeemScript(key.PublicKey);
            Witnesses = new[]
                {new Witness {InvocationScript = invocationScript, VerificationScript = verificationScript}};
        }


        public static Transaction Unserialize(BinaryReader reader)
        {
            var tx = new Transaction();

            tx.Type = (TransactionType) reader.ReadByte();
            tx.Version = reader.ReadByte();

            switch (tx.Type)
            {
                case TransactionType.InvocationTransaction:
                {
                    var scriptLength = reader.ReadVarInt();
                    tx.Script = reader.ReadBytes((int) scriptLength);

                    if (tx.Version >= 1)
                        tx.Gas = reader.ReadFixed();
                    else
                        tx.Gas = 0;

                    break;
                }

                case TransactionType.MinerTransaction:
                {
                    var Nonce = reader.ReadUInt32();
                    break;
                }

                case TransactionType.ClaimTransaction:
                {
                    var len = (int) reader.ReadVarInt(0x10000000);
                    tx.References = new Input[len];
                    for (var i = 0; i < len; i++) tx.References[i] = UnserializeTransactionInput(reader);

                    break;
                }

                case TransactionType.ContractTransaction:
                {
                    break;
                }

                case TransactionType.PublishTransaction:
                {
                    var script = reader.ReadVarBytes();
                    var parameterList = reader.ReadVarBytes();
                    var returnType = reader.ReadByte();
                    bool NeedStorage;
                    if (tx.Version >= 1)
                        NeedStorage = reader.ReadBoolean();
                    else
                        NeedStorage = false;
                    var name = reader.ReadVarString();
                    var codeVersion = reader.ReadVarString();
                    var author = reader.ReadVarString();
                    var email = reader.ReadVarString();
                    var description = reader.ReadVarString();
                    break;
                }

                case TransactionType.EnrollmentTransaction:
                {
                    var publicKey = ECPoint.DeserializeFrom(reader, ECCurve.Secp256r1);
                    break;
                }

                case TransactionType.RegisterTransaction:
                {
                    var assetType = (AssetType) reader.ReadByte();
                    var name = reader.ReadVarString();
                    var amount = reader.ReadFixed();
                    var precision = reader.ReadByte();
                    var owner = ECPoint.DeserializeFrom(reader, ECCurve.Secp256r1);
                    if (owner.IsInfinity && assetType != AssetType.GoverningToken &&
                        assetType != AssetType.UtilityToken)
                        throw new FormatException();
                    var admin = reader.ReadBytes(20);
                    break;
                }

                case TransactionType.IssueTransaction:
                {
                    break;
                }

                default:
                {
                    throw new NotImplementedException();
                }
            }

            var attrCount = (int) reader.ReadVarInt(16);
            if (attrCount != 0)
            {
                tx.Attributes = new TransactionAttribute[attrCount];
                for (var i = 0; i < attrCount; i++) tx.Attributes[i] = TransactionAttribute.Unserialize(reader);
            }

            var inputCount = (int) reader.ReadVarInt();
            tx.Inputs = new Input[inputCount];
            for (var i = 0; i < inputCount; i++) tx.Inputs[i] = UnserializeTransactionInput(reader);

            var outputCount = (int) reader.ReadVarInt();
            tx.Outputs = new Output[outputCount];
            for (var i = 0; i < outputCount; i++) tx.Outputs[i] = UnserializeTransactionOutput(reader);

            var witnessCount = (int) reader.ReadVarInt();
            tx.Witnesses = new Witness[witnessCount];
            for (var i = 0; i < witnessCount; i++) tx.Witnesses[i] = Witness.Unserialize(reader);

            return tx;
        }

        public static Transaction Unserialize(byte[] bytes)
        {
            using (var stream = new MemoryStream(bytes))
            {
                using (var reader = new BinaryReader(stream))
                {
                    return Unserialize(reader);
                }
            }
        }

        public struct Input
        {
            public byte[] prevHash;
            public uint prevIndex;
        }

        public struct Output
        {
            public byte[] scriptHash;
            public byte[] assetID;
            public decimal value;
        }


        #region HELPERS

        protected static void SerializeTransactionInput(BinaryWriter writer, Input input)
        {
            writer.Write(input.prevHash);
            writer.Write((ushort) input.prevIndex);
        }

        protected static void SerializeTransactionOutput(BinaryWriter writer, Output output)
        {
            writer.Write(output.assetID);
            writer.WriteFixed(output.value);
            writer.Write(output.scriptHash);
        }

        protected static Input UnserializeTransactionInput(BinaryReader reader)
        {
            var prevHash = reader.ReadBytes(32);
            var prevIndex = reader.ReadUInt16();
            return new Input {prevHash = prevHash, prevIndex = prevIndex};
        }

        protected static Output UnserializeTransactionOutput(BinaryReader reader)
        {
            var assetID = reader.ReadBytes(32);
            var value = reader.ReadFixed();
            var scriptHash = reader.ReadBytes(20);
            return new Output {assetID = assetID, value = value, scriptHash = scriptHash};
        }

        #endregion
    }
}