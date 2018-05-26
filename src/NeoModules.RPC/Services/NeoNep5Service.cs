using System;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using NeoModules.JsonRpc.Client;
using NeoModules.RPC.Helpers;
using NeoModules.RPC.Services.Nep5;

namespace NeoModules.RPC.Services
{
    public class NeoNep5Service : RpcClientWrapper
    {
        public NeoNep5Service(IClient client, string tokenScriptHash) : base(client)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));
            if (string.IsNullOrEmpty(tokenScriptHash)) throw new ArgumentNullException(nameof(tokenScriptHash));

            GetTokenBalance = new TokenBalanceOf(client, tokenScriptHash);
            GetTokenDecimals = new TokenDecimals(client, tokenScriptHash);
            GetTokenName = new TokenName(client, tokenScriptHash);
            GetTokenTotalSupply = new TokenTotalSupply(client, tokenScriptHash);
            GetTokenSymbol = new TokenSymbol(client, tokenScriptHash);
        }

        public async Task<string> GetName()
        {
            string name = string.Empty;
            var result = await GetTokenName.SendRequestAsync();
            if (result != null)
            {
                var temp = result.Stack[0].Value.ToString();
                name = temp.HexToString();
            }
            return name;
        }

        public async Task<string> GetSymbol()
        {
            string name = string.Empty;
            var result = await GetTokenSymbol.SendRequestAsync();
            if (result != null)
            {
                var temp = result.Stack[0].Value.ToString();
                name = temp.HexToString();
            }
            return name;
        }

        public async Task<string> GetTotalSupply(string decimals)
        {
            string totalSupply = string.Empty;
            if (string.IsNullOrEmpty(decimals)) return totalSupply;

            var result = await GetTokenTotalSupply.SendRequestAsync();
            if (result != null)
            {
                var value = result.Stack[0].Value.ToString();
                var supplyValueArray = value.HexToBytes().Reverse().ToArray();
                value = BitConverter.ToString(supplyValueArray).Replace("-", "");
                totalSupply = (HexToBigInteger(value) / DecimalStringToBigInteger(decimals)).ToString();
            }
            return totalSupply;
        }

        public async Task<string> GetDecimals()
        {
            string decimals = string.Empty;

            var result = await GetTokenDecimals.SendRequestAsync();
            if (result != null)
            {
                decimals = result.Stack[0].Value.ToString();
            }
            return decimals;
        }

        public async Task<string> GetBalance(string addressScriptHash, string decimals)
        {
            string balance = string.Empty;

            var result = await GetTokenBalance.SendRequestAsync(addressScriptHash);
            if (result != null)
            {
                balance = result.Stack[0].Value.ToString();
                if (string.IsNullOrEmpty(balance)) return "0";
                var supplyValueArray = balance.HexToBytes().Reverse().ToArray();
                balance = BitConverter.ToString(supplyValueArray).Replace("-", "");
                balance = GetDecimal(HexToBigInteger(balance), (int)DecimalStringToBigInteger(decimals));              
            }
            return balance;
        }

        private TokenBalanceOf GetTokenBalance { get; }
        private TokenDecimals GetTokenDecimals { get; }
        private TokenName GetTokenName { get; }
        private TokenTotalSupply GetTokenTotalSupply { get; }
        private TokenSymbol GetTokenSymbol { get; }


        private string GetDecimal(BigInteger bigInteger, int divisor)
        {
            var quotient = BigInteger.DivRem(bigInteger, divisor, out var remainder);

            const int decimalPlaces = 2;
            var decimalPart = BigInteger.Zero;
            for (int i = 0; i < decimalPlaces; i++)
            {
                var div = (remainder * 10) / divisor;

                decimalPart *= 10;
                decimalPart += div;

                remainder = remainder * 10 - div * divisor;
            }

            var retValue = quotient + "." + decimalPart.ToString(new string('0', decimalPlaces));
            return retValue;
        }

        private static BigInteger HexToBigInteger(string hexNumber)
        {
            return BigInteger.Parse(hexNumber, NumberStyles.HexNumber);
        }

        private BigInteger DecimalStringToBigInteger(string hexDecimals)
        {
            return BigInteger.Parse(Math.Pow(10, double.Parse(hexDecimals)).ToString(CultureInfo.InvariantCulture));
        }
    }
}
