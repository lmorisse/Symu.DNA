#region Licence

// Description: SymuBiz - Symu
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Linq;

#endregion

namespace Symu.DNA.MatrixNetworks
{
    /// <summary>
    ///     A utility to easily switch from NetworkAnalysisType to value
    /// </summary>
    public static class NetworkAnalysisTypeService
    {
        /// <summary>
        ///     Get all names of the KnowledgeLevel enum
        /// </summary>
        /// <returns></returns>
        public static string[] GetNames()
        {
            return Enum.GetNames(typeof(NetworkAnalysisType)).ToArray();
        }

        /// <summary>
        ///     Get the value based on the GenericLevel name
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public static NetworkAnalysisType GetValue(string level)
        {
            switch (level)
            {
                case "Connectivity":
                    return NetworkAnalysisType.Connectivity;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        ///     Get the name of a network Analysis Type
        /// </summary>
        /// <param name="networkAnalysisType"></param>
        /// <returns></returns>
        public static string GetName(NetworkAnalysisType networkAnalysisType)
        {
            return networkAnalysisType.ToString();
        }
    }
}