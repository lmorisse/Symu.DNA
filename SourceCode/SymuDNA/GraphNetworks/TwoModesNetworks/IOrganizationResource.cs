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
    ///     Organization x Resource network, called capabilities network
    ///     Which organization has what resource
    /// </summary>
    public interface IOrganizationResource
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

        IOrganizationResource Clone();

        bool Equals(IResourceUsage resourceUsage);
        bool Equals(IAgentId resourceId, IResourceUsage resourceUsage);
        bool Equals(IAgentId resourceId);
        bool Equals(object obj);
    }
}