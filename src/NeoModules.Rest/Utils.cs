using Newtonsoft.Json;

namespace NeoModules.Rest
{
    public static class Utils
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            Error = (sender, args) =>
            {
                if (System.Diagnostics.Debugger.IsAttached)
                {
                    System.Diagnostics.Debugger.Break();
                }
            },
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None
        };
    }
}
