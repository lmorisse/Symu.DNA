#region Licence

// Description: SymuBiz - Symu
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

using Symu.Common.Interfaces.Agent;
using Symu.DNA.TwoModesNetworks.AgentGroup;

namespace SymuDNATests.Classes
{
    /// <summary>
    /// Interface to define who is member of a group and how
    /// By default how is characterized by an allocation of capacity to define part-time membership
    /// 
    /// </summary>
    public class TestAgentGroup : IAgentGroup
    {
        public TestAgentGroup(IAgentId id, float allocation)
        {
            AgentId = id;
            Allocation = allocation;
        }
        public IAgentId AgentId { get; }

        /// <summary>
        ///     Range 0 - 100
        /// </summary>
        public float Allocation { get; set; }
    }
}