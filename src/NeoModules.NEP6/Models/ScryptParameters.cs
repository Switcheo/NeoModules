using Newtonsoft.Json;

namespace NeoModules.NEP6.Models
{
	public class ScryptParameters
	{
		/// <summary>
		/// A parameter that defines the CPU/memory cost. Must be a value 2^N. 
		/// </summary>
		[JsonProperty("n")]
		public int N { get; }

		/// <summary>
		/// A tuning parameter. 
		/// </summary>
		[JsonProperty("r")]
		public int R { get; }

		/// <summary>
		/// A tuning parameter (parallelization parameter).
		/// A large value of p can increase computational cost of SCrypt without increasing the memory usage. 
		/// </summary>
		[JsonProperty("p")]
		public int P { get; }

		[JsonConstructor]
		public ScryptParameters(int n = 16384, int r = 8, int p = 8) // default values
		{
			N = n;
			R = r;
			P = p;
		}

		public static ScryptParameters FromJson(string json) => JsonConvert.DeserializeObject<ScryptParameters>(json);

		public static string ToJson(ScryptParameters self) => JsonConvert.SerializeObject(self);
	}
}
