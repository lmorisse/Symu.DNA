#region Licence

// Description: SymuBiz - Symu
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;
using Symu.Common.Interfaces.Agent;
using Symu.Common.Interfaces.Entity;
using Symu.DNA.Entities;
using Symu.DNA.MatrixNetworks;

#endregion

namespace Symu.DNA.GraphNetworks.TwoModesNetworks
{
    /// <summary>
    ///     Organization x Resource network
    ///     Which organization uses what resource
    ///     Key => OrganizationId
    ///     Value => ResourceId
    /// </summary>
    public class OrganizationResourceNetwork : TwoModesNetwork<IId, IOrganizationResource>
    {

        public float GetAllocation(IId organizationId, IAgentId resourceId)
        {
            if (HasResource(organizationId, resourceId))
            {
                return GetOrganizationResources(organizationId, resourceId).Sum(x => x.Allocation);
            }

            return 0;
        }

        public float GetAllocation(IId organizationId, IAgentId resourceId, IResourceUsage resourceUsage)
        {
            if (HasResource(organizationId, resourceId, resourceUsage))
            {
                return GetOrganizationResource(organizationId, resourceId, resourceUsage).Allocation;
            }

            return 0;
        }

        /// <summary>
        ///     Get the IActorResource used by an actor with a specific type of use
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        public List<IOrganizationResource> GetOrganizationResources(IId organizationId, IAgentId resourceId)
        {
            return HasResource(organizationId, resourceId) ? List[organizationId].FindAll(n => n.Equals(resourceId)) : new List<IOrganizationResource> ();
        }
        public IEnumerable<IOrganizationResource> GetValues(IId organizationId, IClassId resourceClassId)
        {
            return Exists(organizationId) ? List[organizationId].Where(x => x.ResourceId.ClassId.Equals(resourceClassId)): new IOrganizationResource[0];
        }
        /// <summary>
        ///     Get the IActorResource used by an actor with a specific type of use
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="resourceId"></param>
        /// <param name="resourceUsage"></param>
        /// <returns></returns>
        public IOrganizationResource GetOrganizationResource(IId organizationId, IAgentId resourceId, IResourceUsage resourceUsage)
        {
            return HasResource(organizationId, resourceId, resourceUsage) ? List[organizationId].Find(n => n.Equals(resourceId, resourceUsage)) : null;
        }
        public bool HasResource(IId organizationId, IAgentId resourceId, IResourceUsage resourceUsage)
        {
            return Exists(organizationId) && List[organizationId].Exists(n => n.Equals(resourceId, resourceUsage));
        }
        public bool HasResource(IId organizationId, IAgentId resourceId)
        {
            return Exists(organizationId) && List[organizationId].Exists(n => n.Equals(resourceId));
        }
        public bool HasResource(IId organizationId, IResourceUsage resourceUsage)
        {
            return Exists(organizationId) && List[organizationId].Exists(n => n.Equals(resourceUsage));
        }

        /// <summary>
        ///     Get the list of all the resources the organizationId is using
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public IEnumerable<IAgentId> GetResourceIds(IId organizationId)
        {
            return Exists(organizationId) ? List[organizationId].Select(x => x.ResourceId) : new IAgentId[0];
        }

        /// <summary>
        ///     Get the list of all the resources the organizationId is using
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="resourceClassId"></param>
        /// <returns></returns>
        public IEnumerable<IAgentId> GetResourceIds(IId organizationId, IClassId resourceClassId)
        {
            return Exists(organizationId) ? List[organizationId].Where(x => x.ResourceId.ClassId.Equals(resourceClassId)).Select(x => x.ResourceId) : new List<IAgentId>();
        }

        /// <summary>
        ///     Get the list of all the resources the organizationId is using filtered by type of use
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="resourceUsage"></param>
        /// <returns></returns>
        public IEnumerable<IAgentId> GetResourceIds(IId organizationId, IResourceUsage resourceUsage)
        {
            return Exists(organizationId)
                ? List[organizationId].FindAll(n => n.Equals(resourceUsage)).Select(x => x.ResourceId)
                : new List<IAgentId>();
        }

        /// <summary>
        ///     Get all the organizationIds
        /// </summary>
        /// <param name="resourceId"></param>
        /// <param name="resourceUsage"></param>
        /// <returns></returns>
        public List<IId> GetOrganizationIds(IAgentId resourceId, IResourceUsage resourceUsage)
        {
            return (from organizationId
                in List.Keys
                    let agentResource = List[organizationId]
                    where agentResource.Exists(x => x.Equals(resourceId, resourceUsage))
                    select organizationId).ToList();
        }

        /// <summary>
        ///     Get all the organizationIds
        /// </summary>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        public List<IOrganizationResource> GetOrganizationResources(IId resourceId)
        {
            var result = new List<IOrganizationResource>();
            foreach (var organizationId in List.Keys)
            {
                result.AddRange(List[organizationId].FindAll(x => x.ResourceId.Equals(resourceId)));
            }
            return result;
        }

        /// <summary>
        /// Convert the network into a matrix
        /// </summary>
        /// <param name="organizationIds"></param>
        /// <param name="resourceIds"></param>
        /// <returns></returns>
        public Matrix<float> ToMatrix(VectorNetwork<IId> organizationIds, VectorNetwork<IAgentId> resourceIds)
        {
            if (!organizationIds.Any || !resourceIds.Any)
            {
                return null;
            }
            var matrix = Matrix<float>.Build.Dense(organizationIds.Count, resourceIds.Count);
            for (var i = 0; i < organizationIds.Count; i++)
            {
                var organizationId = organizationIds.IndexItem[i];
                if (!organizationIds.ItemIndex.ContainsKey(organizationId))
                {
                    throw new NullReferenceException(nameof(organizationIds.ItemIndex));
                }
                var row = organizationIds.ItemIndex[organizationId];
                foreach (var agentResource in List[organizationId])
                {
                    if (!resourceIds.ItemIndex.ContainsKey(agentResource.ResourceId))
                    {
                        throw new NullReferenceException(nameof(resourceIds.ItemIndex));
                    }
                    var column = resourceIds.ItemIndex[agentResource.ResourceId];
                    matrix[row, column] = agentResource.Allocation;
                }
            }
            return matrix;
        }

        public void Remove(IId organizationId, IAgentId resourceId)
        {
            if (HasResource(organizationId, resourceId))
            {
                List[organizationId].RemoveAll(n => n.Equals(resourceId));
            }
        }

        public void RemoveOrganization(IAgentId organizationId)
        {
            Remove(organizationId);
        }

        public void RemoveResource(IAgentId resourceId)
        {
            foreach (var organizationResource in List.Values)
            {
                organizationResource.RemoveAll(x => x.ResourceId.Equals(resourceId));
            }
        }

        public IOrganizationResource Get(IId organizationId, IAgentId resourceId)
        {
            if (HasResource(organizationId, resourceId))
            {
                return List[organizationId].Find(n => n.Equals(resourceId));
            }

            return null;
        }
        public IEnumerable<TOrganizationResource> GetValues<TOrganizationResource>(IId key) where TOrganizationResource : IOrganizationResource
        {
            return GetValues(key).OfType<TOrganizationResource>();
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
        public void UpdateAllocation(IId organizationId, IAgentId resourceId, IResourceUsage resourceUsage, float allocation, float capacityThreshold)
        {
            var agentResource = GetOrganizationResource(organizationId, resourceId, resourceUsage);
            if (agentResource is null)
            {
                throw new NullReferenceException(nameof(agentResource));
            }

            agentResource.Allocation = Math.Max(agentResource.Allocation + allocation, capacityThreshold);
        }

        /// <summary>
        ///     Update all groupAllocation of the organizationId filtered by the groupId.ClassKey
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="fullAlloc">true if all groupAllocations are added, false if we are in modeling phase</param>
        public void UpdateAllocations<TOrganizationResource>(IId organizationId, bool fullAlloc) where TOrganizationResource : IOrganizationResource
        {
            var organizationResources = GetValues<TOrganizationResource>(organizationId).ToList();

            if (!organizationResources.Any())
            {
                throw new ArgumentOutOfRangeException("organizationId should have a group allocation");
            }

            var totalAllocation = organizationResources.Sum(ga => ga.Allocation);

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
                var updatedGroupAllocation = List[organizationId].Find(x => x.Equals(organizationResource));
                updatedGroupAllocation.Allocation =
                    Math.Min(100F, updatedGroupAllocation.Allocation * 100F / totalAllocation);
            }
        }
    }
}