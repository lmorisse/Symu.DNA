#region Licence

// Description: SymuBiz - SymuDNA
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
                case "ActorTask":
                    return NetworkType.ActorTask;
                case "ActorActor":
                    return NetworkType.ActorActor;
                case "ActorBelief":
                    return NetworkType.ActorBelief;
                case "ActorOrganization":
                    return NetworkType.ActorOrganization;
                case "ActorKnowledge":
                    return NetworkType.ActorKnowledge;
                case "ActorResource":
                    return NetworkType.ActorResource;
                case "ActorRole":
                    return NetworkType.ActorRole;
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