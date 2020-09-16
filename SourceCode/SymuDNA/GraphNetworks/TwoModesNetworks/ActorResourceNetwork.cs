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
    ///     Actor x Resource network, called capabilities network
    ///     Who has what resource
    /// </summary>
    /// <example>database, products, routines, processes, ...</example>
    public class ActorResourceNetwork : ConcurrentTwoModesNetwork<IAgentId, IActorResource>
    {

        public bool Exists(IAgentId actorId, IId resourceId)
        {
            return Exists(actorId) && List[actorId].Exists(x => x.Equals(resourceId));
        }

        /// <summary>
        ///     Add a value to a key
        ///     Key is supposed to be already present in the collection.
        ///     if not use Add method
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public override void AddValue(IAgentId key, IActorResource value)
        {
            if (List[key].Contains(value))
            {
                return;
            }

            List[key].Add(value);
            var actorResources = List[key].Where(x => x.GetType() == value.GetType()).ToList();

            // Re allocation is done by type of ActorResource
            var totalAllocation =
                actorResources.Sum(x => x.Allocation);

            // There is non main object used at 100%
            // Objects are added as things progress, with the good allocation
            if (!(totalAllocation > 100))
            {
                return;
            }

            foreach (var resource in actorResources)
            {
                //Don't use ComponentAllocation[ca] *= 100/ => return 0
                resource.Allocation = Convert.ToSingle(resource.Allocation * 100 / totalAllocation);
            }
        }

        public byte GetValuesCount<TActorResource>(IAgentId actorId) where TActorResource : IActorResource
        {
            return Exists(actorId) ? Convert.ToByte(List[actorId].OfType<TActorResource>().Count()) : (byte)0;
        }

        public float GetAllocation(IAgentId actorId, IAgentId resourceId)
        {
            if (HasResource(actorId, resourceId))
            {
                return GetActorResources(actorId, resourceId).Sum(x => x.Allocation);
            }

            return 0;
        }

        public float GetAllocation(IAgentId actorId, IAgentId resourceId, IResourceUsage resourceUsage)
        {
            if (HasResource(actorId, resourceId, resourceUsage))
            {
                return GetActorResource(actorId, resourceId, resourceUsage).Allocation;
            }

            return 0;
        }
        public IEnumerable<TActorResource> GetActorResources<TActorResource>(IAgentId key) where TActorResource : IActorResource
        {
            return GetValues(key).OfType<TActorResource>();
        }

        /// <summary>
        ///     Get the IAgentResource used by an agent with a specific type of use
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        public IEnumerable<IActorResource> GetActorResources(IAgentId actorId, IAgentId resourceId)
        {
            return HasResource(actorId, resourceId) ? List[actorId].FindAll(n => n.Equals(resourceId)) : null;
        }

        /// <summary>
        ///     Get the ActorResource used by an actor with a specific type of use
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="resourceId"></param>
        /// <param name="resourceUsage"></param>
        /// <returns></returns>
        public IActorResource GetActorResource(IAgentId actorId, IAgentId resourceId, IResourceUsage resourceUsage)
        {
            return HasResource(actorId, resourceId, resourceUsage) ? List[actorId].Find(n => n.Equals(resourceId, resourceUsage)) : null;
        }
        public bool HasResource(IAgentId actorId, IAgentId resourceId, IResourceUsage resourceUsage)
        {
            return Exists(actorId) && List[actorId].Exists(n => n.Equals(resourceId, resourceUsage));
        }
        public bool HasResource(IAgentId actorId, IAgentId resourceId)
        {
            return Exists(actorId) && List[actorId].Exists(n => n.Equals(resourceId));
        }
        public bool HasResource(IAgentId actorId, IClassId resourceClassId)
        {
            return Exists(actorId) && List[actorId].Exists(n => n.ResourceId.ClassId.Equals(resourceClassId));
        }
        public bool HasResource(IAgentId actorId, IResourceUsage resourceUsage)
        {
            return Exists(actorId) && List[actorId].Exists(n => n.Equals(resourceUsage));
        }

        /// <summary>
        ///     Get the list of all the resources the actorId is using
        /// </summary>
        /// <param name="actorId"></param>
        /// <returns></returns>
        public IEnumerable<IAgentId> GetResourceIds(IAgentId actorId)
        {
            return Exists(actorId) ? List[actorId].Select(x => x.ResourceId) : new List<IAgentId>();
        }

        /// <summary>
        ///     Get the list of all the resources the actorId is using filtered by type of use
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="resourceUsage"></param>
        /// <returns></returns>
        public IEnumerable<IAgentId> GetResourceIds(IAgentId actorId, IResourceUsage resourceUsage)
        {
            return Exists(actorId)
                ? List[actorId].Where(n => n.Equals(resourceUsage)).Select(x => x.ResourceId)
                : new List<IAgentId>();
        }

        /// <summary>
        ///     Get the list of all the resources the actorId is using filtered by type of use
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="resourceUsage"></param>
        /// <param name="resourceClassId"></param>
        /// <returns></returns>
        public IEnumerable<IAgentId> GetResourceIds(IAgentId actorId, IResourceUsage resourceUsage, IClassId resourceClassId)
        {
            return Exists(actorId)
                ? List[actorId].Where(n => n.ResourceId.ClassId.Equals(resourceClassId) && n.Equals(resourceUsage)).Select(x => x.ResourceId)
                : new List<IAgentId>();
        }
        public IEnumerable<IAgentId> GetResourceIds<TActorResource>(IAgentId actorId, IResourceUsage resourceUsage) where TActorResource : IActorResource
        {
            return Exists(actorId)
                ? List[actorId].OfType<TActorResource>().Where(n => n.Equals(resourceUsage)).Select(x => x.ResourceId)
                : new List<IAgentId>();
        }

        /// <summary>
        ///     Get all the actorIds of classId using resourceId
        /// </summary>
        /// <param name="resourceId"></param>
        /// <param name="resourceUsage"></param>
        /// <param name="actorClassId"></param>
        /// <returns></returns>
        public List<IAgentId> GetActorIds(IAgentId resourceId, IResourceUsage resourceUsage, IClassId actorClassId)
        {
            return (from actorId 
                in List.Keys.Where(x => x.ClassId.Equals(actorClassId)) 
                let actorResource = List[actorId] 
                where actorResource.Exists(x => x.Equals(resourceId, resourceUsage))
                select actorId).ToList();
        }
        /// <summary>
        ///     Get actors of an organization filtered by classKey
        /// </summary>
        /// <param name="resourceId"></param>
        /// <param name="actorClassId"></param>
        /// <returns></returns>
        public IEnumerable<IAgentId> GetActorIds(IAgentId resourceId, IClassId actorClassId)
        {
            return (from actorId
                    in List.Keys.Where(x => x.ClassId.Equals(actorClassId))
                let actorResource = List[actorId]
                where actorResource.Exists(x => x.Equals(resourceId))
                select actorId).ToList();
        }
        /// <summary>
        ///     Get actors count using a resource
        /// </summary>
        /// <param name="resourceId"></param>
        /// <param name="actorClassId"></param>
        /// <returns></returns>
        public byte GetActorsCount(IAgentId resourceId, IClassId actorClassId)
        {
            return (byte)List.Keys.Where(x => x.ClassId.Equals(actorClassId)).Sum(actorId => List[actorId].Count(x => x.ResourceId.Equals(resourceId)));
        }
        /// <summary>
        ///     Get sum allocations using a resource
        /// </summary>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        public float GetSumAllocation(IAgentId resourceId)
        {
            return List.Keys.Sum(actorId => List[actorId].Where(x => x.ResourceId.Equals(resourceId)).Sum(x => x.Allocation));
        }


        /// <summary>
        ///     Update Resource Allocation in a delta mode
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="resourceId"></param>
        /// <param name="resourceUsage"></param>
        /// <param name="allocation"></param>
        /// <param name="capacityThreshold"></param>
        /// <example>allocation = 50 & groupAllocation = 20 => updated groupAllocation =50+20=70</example>
        public void UpdateAllocation(IAgentId actorId, IAgentId resourceId, IResourceUsage resourceUsage, float allocation, float capacityThreshold)
        {
            var actorResource = GetActorResource(actorId, resourceId, resourceUsage);
            if (actorResource is null)
            {
                throw new NullReferenceException(nameof(actorResource));
            }

            actorResource.Allocation = Math.Max(actorResource.Allocation + allocation, capacityThreshold);
        }

        /// <summary>
        ///     Update all groupAllocation of the actorId filtered by the groupId.ClassKey
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="fullAlloc">true if all groupAllocations are added, false if we are in modeling phase</param>
        public void UpdateAllocations<TActorResource>(IAgentId actorId, bool fullAlloc) where TActorResource: IActorResource
        {
            //todo should be done group by type of ActorResource
            var actorResources = GetActorResources<TActorResource>(actorId).ToList();

            if (!actorResources.Any())
            {
                throw new ArgumentOutOfRangeException("actorId should have a group allocation");
            }

            var totalAllocation = actorResources.Sum(ga => ga.Allocation);

            if (!fullAlloc || totalAllocation <= 100)
            {
                return;
            }

            if (totalAllocation <= 0)
            {
                throw new ArgumentOutOfRangeException("total Allocation should be strictly positif");
            }

            foreach (var actorResource in actorResources)
            {
                // groupAllocation come from an IEnumerable which is readonly
                var updatedGroupAllocation = List[actorId].Find(x => x.Equals(actorResource));
                updatedGroupAllocation.Allocation =
                    Math.Min(100F, updatedGroupAllocation.Allocation * 100F / totalAllocation);
            }
        }

        public void Remove(IAgentId actorId, IAgentId resourceId)
        {
            if (!HasResource(actorId, resourceId))
            {
                return;
            }
            List[actorId].RemoveAll(l => l.Equals(resourceId));
        }
        public void Remove(IAgentId actorId, IEnumerable<IAgentId> resourceIds)
        {
            foreach (var resourceId in resourceIds)
            {
                Remove(actorId, resourceId);
            }
        }
        public void RemoveResource(IAgentId resourceId)
        {
            foreach (var actorResource in List.Values)
            {
                actorResource.RemoveAll(x => x.ResourceId.Equals(resourceId));
            }
        }
        /// <summary>
        ///     Copy the same network from an actor to another
        /// </summary>
        /// <param name="fromActorId"></param>
        /// <param name="toActorId"></param>
        public void CopyTo(IAgentId fromActorId, IAgentId toActorId)
        {
            if (!Exists(fromActorId))
            {
                return;
            }
            AddKey(toActorId);
            List[toActorId].Clear();
            foreach (var actorResource in List[fromActorId])
            {
                Add(toActorId, actorResource.Clone());
            }
        }


        /// <summary>
        /// Convert the network into a matrix
        /// </summary>
        /// <param name="actorIds"></param>
        /// <param name="resourceIds"></param>
        /// <returns></returns>
        public Matrix<float> ToMatrix(VectorNetwork<IAgentId> actorIds, VectorNetwork<IAgentId> resourceIds)
        {
            if (!actorIds.Any || !resourceIds.Any)
            {
                return null;
            }
            var matrix = Matrix<float>.Build.Dense(actorIds.Count, resourceIds.Count);
            foreach (var actorResources in List)
            {
                if (!actorIds.ItemIndex.ContainsKey(actorResources.Key))
                {
                    throw new NullReferenceException(nameof(actorIds.ItemIndex));
                }
                var row = actorIds.ItemIndex[actorResources.Key];
                foreach (var actorResource in actorResources.Value)
                {
                    if (!resourceIds.ItemIndex.ContainsKey(actorResource.ResourceId))
                    {
                        throw new NullReferenceException(nameof(resourceIds.ItemIndex));
                    }
                    var column = resourceIds.ItemIndex[actorResource.ResourceId];
                    matrix[row, column] = actorResource.Allocation;
                }
            }
            return matrix;
        }
    }
}