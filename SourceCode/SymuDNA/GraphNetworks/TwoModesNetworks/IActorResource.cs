#region Licence

// Description: SymuBiz - Symu
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

using Symu.Common.Interfaces.Agent;
using Symu.DNA.Entities;

namespace Symu.DNA.GraphNetworks.TwoModesNetworks
{
    /// <summary>
    ///     Actor x Resource network, called capabilities network
    ///     Who has what resource
    /// By default, an actor uses a resourceId, with an allocation from 0 to 100 with a certain ResourceUsage
    /// </summary>
    public interface IActorResource
    {
        /// <summary>
        ///     The unique identifier of the resource
        /// </summary>
        IAgentId ResourceId { get; set; }
        /// <summary>
        ///     Allocation of capacity per resource
        ///     capacity allocation ranging from [0; 100]
        /// </summary>
        float Allocation { get; set; }
        /// <summary>
        ///     Define how the AgentId is using the resource
        /// </summary>
        IResourceUsage Usage { get; }

        IActorResource Clone();

        bool Equals(IResourceUsage resourceUsage);
        bool Equals(IAgentId resourceId, IResourceUsage resourceUsage);
        bool Equals(IAgentId resourceId);
        bool Equals(object obj);
    }
}