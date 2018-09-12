using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace NeoModules.Rest.DTOs.HappyNodes
{
    public class NodeStability
    {
        public DateTime Date { get; set; }
        public bool Online { get; set; }

        public static IList<NodeStability> DailyFromJson(string json) => DeserializeDailyNodeStability(json);

        public static IList<NodeStability> WeeklyFromJson(string json) => DeserializeWeeklyNodeStability(json);

        private static IList<NodeStability> DeserializeDailyNodeStability(string data)
        {
            var nodeStabilityList = new List<NodeStability>();
            foreach (Match match in Utils.DataRegex.Matches(data))
            {
                var trimed = match.Value.Replace("\'", "").Replace("(", "").Replace(")", "");
                var temp = trimed.Split(',');
                bool isOnline;
                isOnline = Convert.ToInt32(temp[1]) != 0;

                nodeStabilityList.Add(new NodeStability
                {
                    Date = DateTime.ParseExact(temp[0], "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    Online = isOnline
                });
            }

            return nodeStabilityList;
        }

        private static IList<NodeStability> DeserializeWeeklyNodeStability(string data)
        {
            var nodeStabilityList = new List<NodeStability>();
            foreach (Match match in Utils.DataRegex.Matches(data))
            {
                var trimed = match.Value.Replace("\'", "").Replace("(", "").Replace(")", "");
                var temp = trimed.Split(',');
                var year = Convert.ToInt32(Convert.ToDecimal(temp[0], CultureInfo.InvariantCulture));
                var weekNumber = Convert.ToInt32(Convert.ToDecimal(temp[1], CultureInfo.InvariantCulture));
                bool isOnline;
                isOnline = Convert.ToInt32(Convert.ToDecimal(temp[2], CultureInfo.InvariantCulture)) != 0;

                nodeStabilityList.Add(new NodeStability
                {
                    Date = Utils.FirstDateOfWeekISO8601(year, weekNumber),
                    Online = isOnline
                });
            }

            return nodeStabilityList;
        }
    }
}