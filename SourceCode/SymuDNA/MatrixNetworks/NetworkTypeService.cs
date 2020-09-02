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
    ///     A utility to easily switch from NetworkType to value
    /// </summary>
    public static class NetworkTypeService
    {
        /// <summary>
        ///     Get all names of the KnowledgeLevel enum
        /// </summary>
        /// <returns></returns>
        public static string[] GetNames()
        {
            return Enum.GetNames(typeof(NetworkType)).ToArray();
        }

        /// <summary>
        ///     Get the value based on the GenericLevel name
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public static NetworkType GetValue(string level)
        {
            switch (level)
            {
                case "Assignment":
                    return NetworkType.Assignment;
                case "Interaction":
                    return NetworkType.Interaction;
                case "AgentxBelief":
                    return NetworkType.AgentxBelief;
                case "AgentxGroup":
                    return NetworkType.AgentxGroup;
                case "AgentxKnowledge":
                    return NetworkType.AgentxKnowledge;
                case "AgentxResource":
                    return NetworkType.AgentxResource;
                case "AgentxRole":
                    return NetworkType.AgentxRole;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        ///     Get the name of a network type
        /// </summary>
        /// <param name="networkType"></param>
        /// <returns></returns>
        public static string GetName(NetworkType networkType)
        {
            return networkType.ToString();
        }
    }
}