#region Licence

// Description: SymuBiz - Symu
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace Symu.DNA.Metrics
{
    /// <summary>
    ///     A utility to easily switch from NetworkMetricType to value
    /// </summary>
    public static class NetworkMetricTypeService
    {
        /// <summary>
        ///     Get all names of the NetworkMetricType enum
        /// </summary>
        /// <returns></returns>
        public static string[] GetNames()
        {
            return Enum.GetNames(typeof(NetworkMetricType)).ToArray();
        }

        /// <summary>
        ///     Get the value based on the GenericLevel name
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public static NetworkMetricType GetValue(string level)
        {
            switch (level)
            {
                case "Load":
                    return NetworkMetricType.Load;
                case "Density":
                    return NetworkMetricType.Density;
                case "RowDegreeCentrality":
                    return NetworkMetricType.RowDegreeCentrality;
                case "ColumnDegreeCentrality":
                    return NetworkMetricType.ColumnDegreeCentrality;
                case "RowCount":
                    return NetworkMetricType.RowCount;
                case "ColumnCount":
                    return NetworkMetricType.ColumnCount;
                case "DegreeCentralization":
                    return NetworkMetricType.DegreeCentralization;
                case "RowDegreeCentralization":
                    return NetworkMetricType.RowDegreeCentralization;
                case "ColumnDegreeCentralization":
                    return NetworkMetricType.ColumnDegreeCentralization;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        ///     Get the name of a network Analysis Type
        /// </summary>
        /// <param name="networkMetricType"></param>
        /// <returns></returns>
        public static string GetName(NetworkMetricType networkMetricType)
        {
            return networkMetricType.ToString();
        }

        /// <summary>
        ///     Get all names of the NetworkMetricType enum
        /// </summary>
        /// <returns></returns>
        public static string[] GetNetworkLevelNames()
        {
            return NetworkLevel().ToArray();
        }

        /// <summary>
        /// List of available agent level metrics Types
        /// </summary>
        public static List<string> NetworkLevel()
        {
            var result = new List<string>
            {
                NetworkMetricType.RowCount.ToString(),
                NetworkMetricType.ColumnCount.ToString(),
                NetworkMetricType.Load.ToString(),
                NetworkMetricType.Density.ToString(),
                NetworkMetricType.DegreeCentralization.ToString()
            };
            return result;
        }

        /// <summary>
        ///     Get all names of the NetworkMetricType enum
        /// </summary>
        /// <returns></returns>
        public static string[] GetAgentLevelNames()
        {
            return AgentLevel().ToArray();
        }

        /// <summary>
        /// List of available agent level metrics Types
        /// </summary>
        public static List<string> AgentLevel()
        {
            var result = new List<string>
            {
                NetworkMetricType.RowDegreeCentrality.ToString(), 
                NetworkMetricType.RowDegreeCentralization.ToString()
            };
            return result;
        }

        /// <summary>
        ///     Get all names of the NetworkMetricType enum
        /// </summary>
        /// <returns></returns>
        public static string[] GetColumnLevelNames()
        {
            return ColumnLevel().ToArray();
        }

        /// <summary>
        /// List of available agent level metrics Types
        /// </summary>
        public static List<string> ColumnLevel()
        {
            var result = new List<string>
            {
                NetworkMetricType.ColumnDegreeCentrality.ToString(),
                NetworkMetricType.ColumnDegreeCentralization.ToString()
            };
            return result;
        }
    }
}