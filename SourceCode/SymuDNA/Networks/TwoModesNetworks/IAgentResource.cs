#region Licence

// Description: SymuBiz - Symu
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

using Symu.Common.Interfaces.Entity;
using Symu.DNA.Networks.OneModeNetworks;

namespace Symu.DNA.Networks.TwoModesNetworks
{
    /// <summary>
    ///     Agent x Resource network, called capabilities network
    ///     Who has what resource
    /// By default, an agent uses a resourceId, with an allocation from 0 to 100 with a certain ResourceUsage
    /// </summary>
    public interface IAgentResource
    {
        /// <summary>
        ///     The unique agentId of the resource
        /// </summary>
        IId ResourceId { get; }
        /// <summary>
        ///     Allocation of capacity per resource
        ///     capacity allocation ranging from [0; 100]
        /// </summary>
        float Allocation { get; set; }
        /// <summary>
        ///     Define how the AgentId is using the resource
        /// </summary>
        IResourceUsage Usage { get; }

        IAgentResource Clone();

        bool Equals(IResourceUsage resourceUsage);
        bool Equals(IId resourceId, IResourceUsage resourceUsage);
        bool Equals(IId resourceId);
        bool Equals(object obj);
    }
}