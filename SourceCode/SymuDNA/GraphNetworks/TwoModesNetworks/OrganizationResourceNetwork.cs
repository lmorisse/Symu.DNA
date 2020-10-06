#region Licence

// Description: SymuBiz - SymuDNA
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Collections.Generic;
using System.Linq;
using Symu.Common.Interfaces;
using Symu.DNA.Edges;
using Symu.DNA.Entities;

#endregion

namespace Symu.DNA.GraphNetworks.TwoModesNetworks
{
    /// <summary>
    ///     Organization x Resource network
    ///     Which organization uses what resource
    ///     Source = Organization
    ///     Target = Resource
    /// </summary>
    public class OrganizationResourceNetwork : TwoModesNetwork<IOrganizationResource>
    {
        public float GetWeight(IAgentId organizationId, IAgentId resourceId, IResourceUsage resourceUsage)
        {
            if (HasResource(organizationId, resourceId, resourceUsage))
            {
                return Edge(organizationId, resourceId, resourceUsage).Weight;
            }

            return 0;
        }
        public void SetWeight(IAgentId organizationId, IAgentId resourceId, IResourceUsage resourceUsage, float weight)
        {
            if (HasResource(organizationId, resourceId, resourceUsage))
            {
                Edge(organizationId, resourceId, resourceUsage).Weight = weight;
            }
        }

        /// <summary>
        ///     Get the IOrganizationResource used by an actor with a specific type of use
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="resourceUsage"></param>
        /// <returns></returns>
        public IOrganizationResource Edge(IAgentId source, IAgentId target,
            IResourceUsage resourceUsage)
        {
            return Exists(source, target) ? Edges(source, target).FirstOrDefault(n => n.Equals(resourceUsage)) : null;
        }

        public bool HasResource(IAgentId organizationId, IAgentId resourceId, IResourceUsage resourceUsage)
        {
            return Edges(organizationId, resourceId).ToList().Exists(n => n.Equals(resourceUsage));
        }

        public bool HasResource(IAgentId organizationId, IResourceUsage resourceUsage)
        {
            return ExistsSource(organizationId) &&
                   EdgesFilteredBySource(organizationId).ToList().Exists(n => n.Equals(resourceUsage));
        }

        /// <summary>
        ///     Get the list of all the resources the organizationId is using filtered by type of use
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="resourceUsage"></param>
        /// <returns></returns>
        public IEnumerable<IAgentId> GetResourceIds(IAgentId organizationId, IResourceUsage resourceUsage)
        {
            return ExistsSource(organizationId)
                ? EdgesFilteredBySource(organizationId).Where(n => n.Usage.Equals(resourceUsage)).Select(x => x.Target)
                : new List<IAgentId>();
        }

        /// <summary>
        ///     Get all the organizationIds
        /// </summary>
        /// <param name="resourceId"></param>
        /// <param name="resourceUsage"></param>
        /// <returns></returns>
        public IEnumerable<IAgentId> GetOrganizationIds(IAgentId resourceId, IResourceUsage resourceUsage)
        {
            return ExistsTarget(resourceId)
                ? EdgesFilteredByTarget(resourceId).Where(n => n.Usage.Equals(resourceUsage)).Select(x => x.Source)
                : new List<IAgentId>();
        }

        /// <summary>
        ///     Update Resource Allocation in a delta mode
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="resourceId"></param>
        /// <param name="resourceUsage"></param>
        /// <param name="allocation"></param>
        /// <param name="capacityThreshold"></param>
        /// <example>allocation = 50 & groupAllocation = 20 => updated groupAllocation =50+20=70</example>
        public void UpdateWeight(IAgentId organizationId, IAgentId resourceId, IResourceUsage resourceUsage,
            float allocation, float capacityThreshold)
        {
            var agentResource = Edge(organizationId, resourceId, resourceUsage);
            if (agentResource is null)
            {
                throw new NullReferenceException(nameof(agentResource));
            }

            agentResource.Weight = Math.Max(agentResource.Weight + allocation, capacityThreshold);
        }

        /// <summary>
        ///     Update all groupAllocation of the organizationId filtered by the groupId.ClassKey
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="resourceClassId"></param>
        /// <param name="fullAlloc">true if all groupAllocations are added, false if we are in modeling phase</param>
        public void UpdateWeights(IAgentId organizationId, IClassId resourceClassId, bool fullAlloc)
        {
            var organizationResources = EdgesFilteredBySourceAndTargetClassId(organizationId, resourceClassId).ToList();

            if (!organizationResources.Any())
            {
                throw new ArgumentOutOfRangeException("organizationId should have a group allocation");
            }

            var totalAllocation = organizationResources.Sum(ga => ga.Weight);

            if (!fullAlloc && totalAllocation <= 100)
            {
                return;
            }

            if (totalAllocation <= 0)
            {
                throw new ArgumentOutOfRangeException("total Allocation should be strictly positif");
            }

            foreach (var organizationResource in organizationResources)
            {
                // groupAllocation come from an IEnumerable which is readonly
                var updatedGroupAllocation = Edge(organizationResource);
                updatedGroupAllocation.Weight =
                    Math.Min(100F, updatedGroupAllocation.Weight * 100F / totalAllocation);
            }
        }
    }
}