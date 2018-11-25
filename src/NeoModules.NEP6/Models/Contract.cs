using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using NeoModules.Core;
using NeoModules.Core.KeyPair;
using NeoModules.Core.NVM;
using NeoModules.NEP6.Converters;

namespace NeoModules.NEP6.Models
{
    public class Contract
    {
        /// <summary>
        /// The script code of the contract. 
        /// This field can be null if the contract has been deployed to the blockchain. 
        /// </summary>
        [JsonProperty("script")]
        [JsonConverter(typeof(StringToByteArrayConverter))]
        public byte[] Script { get; set; }

        /// <summary>
        /// An array of Parameter objects which describe the details of each parameter in the contract function.
        /// </summary>
        [JsonProperty("parameters")]
        public List<Parameter> Parameters { get; set; }

        /// <summary>
        /// An array of Parameter objects which describe the details of each parameter in the contract function.
        /// For more information about Parameter object, see the descriptions in NEP-3: <see href="https://github.com/neo-project/proposals/pull/12">NeoContract ABI</see>
        /// </summary>
        [JsonProperty("deployed")]
        public bool Deployed { get; set; }

        [JsonIgnore]
        public UInt160 ScriptHash => Script.ToScriptHash();

        [JsonConstructor]
        public Contract(List<Parameter> parameters, bool deployed, byte[] script = null)
        {
            Parameters = parameters;
            Deployed = deployed;
            Script = script;
        }

        public Contract() { }

        [JsonIgnore]
        public virtual bool IsStandard
        {
            get
            {
                if (Script.Length != 35) return false;
                if (Script[0] != 33 || Script[34] != (byte)OpCode.CHECKSIG)
                    return false;
                return true;
            }
        }

        public static Contract FromJson(string json) => JsonConvert.DeserializeObject<Contract>(json);

        public static string ToJson(Contract self) => JsonConvert.SerializeObject(self);
    }

    #region Enums
    public enum ContractParameterType : byte
    {
        Signature = 0,
        Boolean = 1,
        Integer = 2,
        Hash160 = 3,
        Hash256 = 4,
        ByteArray = 5,
        PublicKey = 6,
        String = 7,
        Array = 16,
        InteropInterface = 240,
        Void = 255
    }

    [Flags]
    public enum ContractPropertyState : byte
    {
        NoProperty = 0,
        HasStorage = 1,
        HasDynamicInvoke = 2,
        Payable = 4
    }
    #endregion
}
