#region Licence

// Description: SymuBiz - Symu
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

using System.Reflection.Metadata.Ecma335;
using Symu.Common.Interfaces.Agent;
using Symu.Common.Interfaces.Entity;
using Symu.DNA.Entities;
using Symu.DNA.GraphNetworks.TwoModesNetworks;

namespace SymuDNATests.Classes
{
    /// <summary>
    /// Interface to define who is member of a group and how
    /// By default how is characterized by an allocation of capacity to define part-time membership
    /// 
    /// </summary>
    public class TestOrganizationResource : IOrganizationResource
    {
        public TestOrganizationResource(IAgentId resourceId, IResourceUsage usage, float allocation)
        {
            ResourceId = resourceId;
            Usage = usage;
            Allocation = allocation;
        }
        public IAgentId ResourceId { get; set; }

        /// <summary>
        ///     Range 0 - 100
        /// </summary>
        public float Allocation { get; set; }

        public IResourceUsage Usage { get; }
    

        public IOrganizationResource Clone()
        {
            return new TestOrganizationResource(ResourceId, Usage, Allocation);
        }

        public bool Equals(IResourceUsage resourceUsage)
        {
            return Usage.Equals(resourceUsage);
        }

        public bool Equals(IAgentId resourceId, IResourceUsage resourceUsage)
        {
            return Equals(resourceId) && Equals(resourceUsage);
        }

        public bool Equals(IAgentId resourceId)
        {
            return ResourceId.Equals(resourceId) ;
        }
    }
}