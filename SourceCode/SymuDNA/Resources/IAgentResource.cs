﻿#region Licence

// Description: SymuBiz - Symu
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

using Symu.Common.Interfaces.Entity;

namespace Symu.DNA.Resources
{
    /// <summary>
    /// Interface to define the node Agent/resource : define who is using a resource and how
    /// By default, an agent uses a resourceId, with an allocation from 0 to 100adn a certain ResourceUsage
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
        float ResourceAllocation { get; set; }
        /// <summary>
        ///     Define how the AgentId is using the resource
        /// </summary>
        IResourceUsage ResourceUsage { get; }

        IAgentResource Clone();

        bool Equals(IResourceUsage resourceUsage);
        //bool IsResourceUsageAndClassId(IResourceUsage resourceUsage, IClassId classId);
        bool Equals(IId resourceId, IResourceUsage resourceUsage);
        bool Equals(IId resourceId);
        bool Equals(object obj);
    }
}