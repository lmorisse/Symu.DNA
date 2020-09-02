#region Licence

// Description: SymuBiz - SymuDNA
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

namespace Symu.DNA.MatrixNetworks
{
    /// <summary>
    /// List of all the networks that are available for measurements
    /// </summary>
    public enum NetworkType
    {
        /// <summary>
        /// Agent x Agent
        /// </summary>
        Interaction,
        AgentxBelief,
        AgentxGroup,
        AgentxKnowledge,
        AgentxResource,
        AgentxRole,
        /// <summary>
        /// Agent x Activity (Task)
        /// </summary>
        Assignment
    }
}