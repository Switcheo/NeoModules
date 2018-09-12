using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace NeoModules.Rest.DTOs.HappyNodes
{
    public class NodeLatency
    {
        public DateTime Date { get; set; }
        public decimal Latency { get; set; }

        public static IList<NodeLatency> DailyFromJson(string json) => DeserializeDailyNodeLatency(json);

        public static IList<NodeLatency> WeeklyFromJson(string json) => DeserializeWeeklyNodeLatency(json);

        private static IList<NodeLatency> DeserializeDailyNodeLatency(string data)
        {
            var nodeLatencyList = new List<NodeLatency>();
            foreach (Match match in Utils.DataRegex.Matches(data))
            {
                var trimed = match.Value.Replace("\'", "").Replace("(", "").Replace(")", "");
                var temp = trimed.Split(',');
                var latency = Convert.ToDecimal(temp[1].Trim(), CultureInfo.InvariantCulture);

                nodeLatencyList.Add(new NodeLatency
                {
                    Date = DateTime.ParseExact(temp[0], "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    Latency = latency
                });
            }

            return nodeLatencyList;
        }

        private static IList<NodeLatency> DeserializeWeeklyNodeLatency(string data)
        {
            var nodeStabilityList = new List<NodeLatency>();
            foreach (Match match in Utils.DataRegex.Matches(data))
            {
                var trimed = match.Value.Replace("\'", "").Replace("(", "").Replace(")", "");
                var temp = trimed.Split(',');
                var year = Convert.ToInt32(Convert.ToDecimal(temp[0], CultureInfo.InvariantCulture));
                var weekNumber = Convert.ToInt32(Convert.ToDecimal(temp[1], CultureInfo.InvariantCulture));

                nodeStabilityList.Add(new NodeLatency
                {
                    Date = Utils.FirstDateOfWeekISO8601(year, weekNumber),
                    Latency = Convert.ToDecimal(temp[2], CultureInfo.InvariantCulture),
                });
            }

            return nodeStabilityList;
        }
    }
}
