using System;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace Neo.RPC.Tests
{
    public class TestSettings
    {
        public TestSettings()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("test-settings.json");
            Configuration = builder.Build();
        }

        public static string TestNetSettings = "testNetSettings";

        public static string MainNetSettings = "mainNetSettings";

        public string CurrentSettings = MainNetSettings;

        public IConfigurationRoot Configuration { get; set; }

        public string GetDefaultAccount()
        {
            return GetAppSettingsValue("defaultAccount");
        }

        public string GetBlockHash()
        {
            return GetAppSettingsValue("blockhash");
        }

        public ulong GetBlockNumber()
        {
            return Convert.ToUInt64(GetAppSettingsValue("blockNumber"));
        }

        public string GetContractHash()
        {
            return GetAppSettingsValue("contractHash");
        }

        public string GetContractTransaction()
        {
            return GetAppSettingsValue("contractTransaction");
        }

        public string GetMinerTransaction()
        {
            return GetAppSettingsValue("minerTransaction");
        }

        public string GetClaimTransaction()
        {
            return GetAppSettingsValue("claimTransaction");
        }

        public string GetGoverningAssetHash()
        {
            return GetAppSettingsValue("governingAssetHash");
        }

        public string GetUtilityAssetHash()
        {
            return GetAppSettingsValue("utilityAssetHash");
        }

        public string GetOokenAssetHash()
        {
            return GetAppSettingsValue("tokenAssetHash");
        }

        private string GetAppSettingsValue(string key)
        {
            return GetSectionSettingsValue(key, CurrentSettings);
        }

        private string GetSectionSettingsValue(string key, string sectionSettingsKey)
        {
            var configuration = Configuration.GetSection(sectionSettingsKey);
            var children = configuration.GetChildren();
            var setting = children.FirstOrDefault(x => x.Key == key);
            if (setting != null)
                return setting.Value;
            throw new Exception("Setting: " + key + " Not found");
        }

        public string GetRpcUrl()
        {
            return GetAppSettingsValue("rpcUrl");
        }

        public string GetNep5TokenHash()
        {
            return GetAppSettingsValue("nep5TokenHash");
        }

        public string GetDefaultLogLocation()
        {
            return GetAppSettingsValue("debugLogLocation");
        }
    }
}