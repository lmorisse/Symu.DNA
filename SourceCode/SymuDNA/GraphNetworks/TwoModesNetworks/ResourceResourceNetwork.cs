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
using Symu.DNA.Entities;
using Symu.DNA.MatrixNetworks;

#endregion

namespace Symu.DNA.GraphNetworks.TwoModesNetworks
{
    /// <summary>
    ///     Organization x Resource network
    ///     Which organization uses what resource
    ///     Key => ResourceId1
    ///     Value => ResourceId2
    /// </summary>
    public class ResourceResourceNetwork : TwoModesNetwork<IAgentId, IResourceResource>
    {
        public float GetAllocation(IAgentId resourceId1, IAgentId resourceId2)
        {
            if (HasResource(resourceId1, resourceId2))
            {
                return GetResourceResources(resourceId1, resourceId2).Sum(x => x.Allocation);
            }

            return 0;
        }

        public float GetAllocation(IAgentId resourceId1, IAgentId resourceId2, IResourceUsage resourceUsage)
        {
            if (HasResource(resourceId1, resourceId2, resourceUsage))
            {
                return GetResourceResource(resourceId1, resourceId2, resourceUsage).Allocation;
            }

            return 0;
        }

        /// <summary>
        ///     Get the IActorResource used by an actor with a specific type of use
        /// </summary>
        /// <param name="resourceId1"></param>
        /// <param name="resourceId2"></param>
        /// <returns></returns>
        public List<IResourceResource> GetResourceResources(IAgentId resourceId1, IAgentId resourceId2)
        {
            return HasResource(resourceId1, resourceId2) ? List[resourceId1].FindAll(n => n.Equals(resourceId2)) : null;
        }
        /// <summary>
        ///     Get the IActorResource used by an actor with a specific type of use
        /// </summary>
        /// <param name="resourceId1"></param>
        /// <param name="resourceId2"></param>
        /// <param name="resourceUsage"></param>
        /// <returns></returns>
        public IResourceResource GetResourceResource(IAgentId resourceId1, IAgentId resourceId2, IResourceUsage resourceUsage)
        {
            return HasResource(resourceId1, resourceId2, resourceUsage) ? List[resourceId1].Find(n => n.Equals(resourceId2, resourceUsage)) : null;
        }
        public bool HasResource(IAgentId resourceId1, IAgentId resourceId2, IResourceUsage resourceUsage)
        {
            return Exists(resourceId1) && List[resourceId1].Exists(n => n.Equals(resourceId2, resourceUsage));
        }
        public bool HasResource(IAgentId resourceId1, IAgentId resourceId2)
        {
            return Exists(resourceId1) && List[resourceId1].Exists(n => n.Equals(resourceId2));
        }
        public bool HasResource(IAgentId resourceId1, IResourceUsage resourceUsage)
        {
            return Exists(resourceId1) && List[resourceId1].Exists(n => n.Equals(resourceUsage));
        }

        /// <summary>
        ///     Get the list of all the resources the agentId is using
        /// </summary>
        /// <param name="resourceId1"></param>
        /// <returns></returns>
        public IEnumerable<IAgentId> GetResourceIds(IAgentId resourceId1)
        {
            return Exists(resourceId1) ? List[resourceId1].Select(x => x.ResourceId) : new List<IAgentId>();
        }

        /// <summary>
        ///     Get the list of all the resources the agentId is using filtered by type of use
        /// </summary>
        /// <param name="resourceId1"></param>
        /// <param name="resourceUsage"></param>
        /// <returns></returns>
        public IEnumerable<IAgentId> GetResourceId2S(IAgentId resourceId1, IResourceUsage resourceUsage)
        {
            return Exists(resourceId1)
                ? List[resourceId1].FindAll(n => n.Equals(resourceUsage)).Select(x => x.ResourceId)
                : new List<IAgentId>();
        }

        /// <summary>
        ///     Get all the resourceId1s having a resourceUsage of resourceId2
        /// </summary>
        /// <param name="resourceId1"></param>
        /// <param name="resourceUsage"></param>
        /// <returns></returns>
        public List<IAgentId> GetResourceId1S(IAgentId resourceId1, IResourceUsage resourceUsage)
        {
            return (from agentId
                in List.Keys
                    let resourceResources = List[agentId]
                    where resourceResources.Exists(x => x.Equals(resourceId1, resourceUsage))
                    select agentId).ToList();
        }

        /// <summary>
        ///     Get all the organizationIds
        /// </summary>
        /// <param name="resourceId2"></param>
        /// <returns></returns>
        public List<IResourceResource> GetResourceResources(IAgentId resourceId2)
        {
            var result = new List<IResourceResource>();
            foreach (var resourceId1 in List.Keys)
            {
                result.AddRange(List[resourceId1].FindAll(x => x.ResourceId.Equals(resourceId2)));
            }
            return result;
        }

        /// <summary>
        /// Convert the network into a matrix
        /// </summary>
        /// <param name="resourceId1S"></param>
        /// <param name="resourceId2S"></param>
        /// <returns></returns>
        public Matrix<float> ToMatrix(VectorNetwork<IAgentId> resourceId1S, VectorNetwork<IAgentId> resourceId2S)
        {
            if (!resourceId1S.Any || !resourceId2S.Any)
            {
                return null;
            }
            var matrix = Matrix<float>.Build.Dense(resourceId1S.Count, resourceId2S.Count);
            for (var i = 0; i < resourceId1S.Count; i++)
            {
                var organizationId = resourceId1S.IndexItem[i];
                if (!resourceId1S.ItemIndex.ContainsKey(organizationId))
                {
                    throw new NullReferenceException(nameof(resourceId1S.ItemIndex));
                }
                var row = resourceId1S.ItemIndex[organizationId];
                foreach (var agentResource in List[organizationId])
                {
                    if (!resourceId2S.ItemIndex.ContainsKey(agentResource.ResourceId))
                    {
                        throw new NullReferenceException(nameof(resourceId2S.ItemIndex));
                    }
                    var column = resourceId2S.ItemIndex[agentResource.ResourceId];
                    matrix[row, column] = agentResource.Allocation;
                }
            }
            return matrix;
        }
        public void RemoveResource(IAgentId resourceId)
        {
            // ResourceId1
            Remove(resourceId);
            // ResourceId2

            foreach (var resourceResource in List.Values)
            {
                resourceResource.RemoveAll(x => x.ResourceId.Equals(resourceId));
            }
        }

        public void Remove(IAgentId resourceId1, IAgentId resourceId2)
        {
            if (HasResource(resourceId1, resourceId2))
            {
                List[resourceId1].RemoveAll(n => n.Equals(resourceId2));
            }
        }

        public IResourceResource Get(IAgentId resourceId1, IAgentId resourceId2)
        {
            if (HasResource(resourceId1, resourceId2))
            {
                return List[resourceId1].Find(n => n.Equals(resourceId2));
            }

            return null;
        }
        public IEnumerable<TResourceResource> GetValues<TResourceResource>(IAgentId key) where TResourceResource : IResourceResource
        {
            return GetValues(key).OfType<TResourceResource>();
        }
        /// <summary>
        ///     Update Resource Allocation in a delta mode
        /// </summary>
        /// <param name="resourceId1"></param>
        /// <param name="resourceId2"></param>
        /// <param name="resourceUsage"></param>
        /// <param name="allocation"></param>
        /// <param name="capacityThreshold"></param>
        /// <example>allocation = 50 & groupAllocation = 20 => updated groupAllocation =50+20=70</example>
        public void UpdateAllocation(IAgentId resourceId1, IAgentId resourceId2, IResourceUsage resourceUsage, float allocation, float capacityThreshold)
        {
            var agentResource = GetResourceResource(resourceId1, resourceId2, resourceUsage);
            if (agentResource is null)
            {
                throw new NullReferenceException(nameof(agentResource));
            }

            agentResource.Allocation = Math.Max(agentResource.Allocation + allocation, capacityThreshold);
        }

        /// <summary>
        ///     Update all groupAllocation of the agentId filtered by the groupId.ClassKey
        /// </summary>
        /// <param name="resourceId1"></param>
        /// <param name="fullAlloc">true if all groupAllocations are added, false if we are in modeling phase</param>
        public void UpdateAllocations<TResourceResource>(IAgentId resourceId1, bool fullAlloc) where TResourceResource : IResourceResource
        {
            var resourceResources = GetValues<TResourceResource>(resourceId1).ToList();

            if (!resourceResources.Any())
            {
                throw new ArgumentOutOfRangeException("agentId should have a group allocation");
            }

            var totalAllocation = resourceResources.Sum(ga => ga.Allocation);

            if (!fullAlloc && totalAllocation <= 100)
            {
                return;
            }

            if (totalAllocation <= 0)
            {
                throw new ArgumentOutOfRangeException("total Allocation should be strictly positif");
            }

            foreach (var resourceResource in resourceResources)
            {
                // groupAllocation come from an IEnumerable which is readonly
                var updatedGroupAllocation = List[resourceId1].Find(x => x.Equals(resourceResource));
                updatedGroupAllocation.Allocation =
                    Math.Min(100F, updatedGroupAllocation.Allocation * 100F / totalAllocation);
            }
        }


    }
}