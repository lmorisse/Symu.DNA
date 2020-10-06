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
using static Symu.Common.Constants;

#endregion

namespace Symu.DNA.GraphNetworks.TwoModesNetworks
{
    /// <summary>
    ///     Actor x Resource network, called capabilities network
    ///     Who has what resource
    ///     Source : Actor
    ///     Target : Resource
    /// </summary>
    /// <example>database, products, routines, processes, ...</example>
    public class
        ActorResourceNetwork : TwoModesNetwork<IActorResource> //ConcurrentTwoModesNetwork<IAgentId, IActorResource>
    {
        /// <summary>
        ///     Add actor to a resource
        /// </summary>
        /// <param name="actorResource"></param>
        public override void Add(IActorResource actorResource)
        {
            if (!Exists(actorResource))
            {
                List.Add(actorResource);
            }
            else
            {
                var edge = Edge(actorResource);
                edge.Weight = actorResource.Weight;
            }

            UpdateWeights(actorResource.Source, actorResource.Target.ClassId, false);
        }
        
        public float GetWeight(IAgentId actorId, IAgentId resourceId, IResourceUsage resourceUsage)
        {
            if (HasResource(actorId, resourceId, resourceUsage))
            {
                return GetActorResource(actorId, resourceId, resourceUsage).Weight;
            }

            return 0;
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
            return HasResource(actorId, resourceId, resourceUsage)
                ? Edges(actorId, resourceId).FirstOrDefault(n => n.Equals(resourceUsage))
                : null;
        }

        public bool HasResource(IAgentId actorId, IAgentId resourceId, IResourceUsage resourceUsage)
        {
            return Edges(actorId, resourceId).ToList().Exists(n => n.Equals(resourceUsage));
        }

        public bool HasResource(IAgentId actorId, IResourceUsage resourceUsage)
        {
            return EdgesFilteredBySource(actorId).ToList().Exists(n => n.Usage.Equals(resourceUsage));
        }

        /// <summary>
        ///     Get the list of all the resources the actorId is using filtered by type of use
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="resourceUsage"></param>
        /// <returns></returns>
        public IEnumerable<IAgentId> GetResourceIds(IAgentId actorId, IResourceUsage resourceUsage)
        {
            return ExistsSource(actorId)
                ? EdgesFilteredBySource(actorId).Where(n => n.Equals(resourceUsage)).Select(x => x.Target)
                : new List<IAgentId>();
        }

        /// <summary>
        ///     Get the list of all the resources the actorId is using filtered by type of use
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="resourceUsage"></param>
        /// <param name="resourceClassId"></param>
        /// <returns></returns>
        public IEnumerable<IAgentId> GetResourceIds(IAgentId actorId, IResourceUsage resourceUsage,
            IClassId resourceClassId)
        {
            return ExistsSource(actorId)
                ? EdgesFilteredBySource(actorId)
                    .Where(n => n.Target.ClassId.Equals(resourceClassId) && n.Equals(resourceUsage))
                    .Select(x => x.Target)
                : new List<IAgentId>();
        }

        /// <summary>
        ///     Get all the actorIds of classId using resourceId
        /// </summary>
        /// <param name="resourceId"></param>
        /// <param name="resourceUsage"></param>
        /// <param name="actorClassId"></param>
        /// <returns></returns>
        public IEnumerable<IAgentId> GetActorIds(IAgentId resourceId, IResourceUsage resourceUsage,
            IClassId actorClassId)
        {
            return ExistsTarget(resourceId)
                ? EdgesFilteredByTarget(resourceId)
                    .Where(n => n.Source.ClassId.Equals(actorClassId) && n.Equals(resourceUsage)).Select(x => x.Source)
                : new List<IAgentId>();
        }

        /// <summary>
        ///     Update Resource Allocation in a delta mode
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="resourceId"></param>
        /// <param name="resourceUsage"></param>
        /// <param name="weight"></param>
        /// <param name="capacityThreshold"></param>
        /// <example>allocation = 50 & groupAllocation = 20 => updated groupAllocation =50+20=70</example>
        public void UpdateWeight(IAgentId actorId, IAgentId resourceId, IResourceUsage resourceUsage, float weight,
            float capacityThreshold)
        {
            var actorResource = GetActorResource(actorId, resourceId, resourceUsage);
            if (actorResource is null)
            {
                throw new NullReferenceException(nameof(actorResource));
            }

            actorResource.Weight = Math.Max(actorResource.Weight + weight, capacityThreshold);
        }

        /// <summary>
        ///     Update all groupAllocation of the actorId filtered by the groupId.ClassKey
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="resourceClassId"></param>
        /// <param name="fullAlloc">true if all groupAllocations are added, false if we are in modeling phase</param>
        public void UpdateWeights(IAgentId actorId, IClassId resourceClassId, bool fullAlloc)
        {
            var actorResources = EdgesFilteredBySourceAndTargetClassId(actorId, resourceClassId).ToList();

            if (!actorResources.Any())
            {
                throw new ArgumentOutOfRangeException("actorId should have a group allocation");
            }

            var totalAllocation = actorResources.Sum(ga => ga.Weight);

            if (!fullAlloc || Math.Abs(totalAllocation - 100) < Tolerance)
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
                var updatedGroupAllocation = Edge(actorResource);
                updatedGroupAllocation.Weight =
                    Math.Min(100F, updatedGroupAllocation.Weight * 100F / totalAllocation);
            }
        }

        /// <summary>
        ///     Get the main resource of the actorId filter by the group.ClassKey
        ///     The main resource is defined by the maximum weight
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="targetClassId"></param>
        /// <returns>
        ///     return AgentId of the main resource is exists, default Agent if don't exist, so check the result when using this
        ///     method
        /// </returns>
        public IAgentId GetMainResourceOrDefault(IAgentId actorId, IClassId targetClassId)
        {
            var resourceIds = TargetsFilteredBySourceAndTargetClassId(actorId, targetClassId).ToList();
            if (!resourceIds.Any())
            {
                return null;
            }

            var max = EdgesFilteredBySourceAndTargetClassId(actorId, targetClassId).OrderByDescending(ga => ga.Weight).First()
                .Weight;

            return resourceIds.FirstOrDefault(resourceId =>
                EdgesFilteredByTarget(resourceId).ToList().Exists(x => Math.Abs(x.Weight - max) < Tolerance));
        }
    }
}