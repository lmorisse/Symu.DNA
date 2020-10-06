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
using static Symu.Common.Constants;

#endregion

namespace Symu.DNA.GraphNetworks.TwoModesNetworks
{
    /// <summary>
    ///     Actor x Organization network, called work network, employment network
    ///     Who works where
    ///     Source : Actor
    ///     Target : Organization
    /// </summary>
    /// <example>Groups : team, task force, quality circle, community of practices, committees, enterprise....</example>
    public class
        ActorOrganizationNetwork : TwoModesNetwork<IActorOrganization> //ConcurrentTwoModesNetwork<IAgentId, IActorOrganization>
    {
        

        /// <summary>
        ///     Add actor to an organization
        /// </summary>
        /// <param name="actorOrganization"></param>
        public override void Add(IActorOrganization actorOrganization)
        {
            if (!Exists(actorOrganization))
            {
                List.Add(actorOrganization);
            }
            else
            {
                var edge = Edge(actorOrganization);
                edge.Weight = actorOrganization.Weight;
            }

            UpdateWeights(actorOrganization.Source, actorOrganization.Target.ClassId, false);
        }

       
        /// <summary>
        ///     Get the list of the actors of all the organizations of an actorId, filtered by group.ClassKey
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="organizationClassId"></param>
        /// <returns>List of groupIds</returns>
        public IEnumerable<IAgentId> GetCoActorIds(IAgentId actorId, IClassId organizationClassId)
        {
            var coMemberIds = new List<IAgentId>();
            var groupIds = TargetsFilteredBySourceAndTargetClassId(actorId, organizationClassId).ToList();
            if (!groupIds.Any())
            {
                return coMemberIds;
            }

            foreach (var groupId in groupIds)
            {
                coMemberIds.AddRange(EdgesFilteredByTarget(groupId).Where(x => !x.Source.Equals(actorId))
                    .Select(x => x.Source));
            }

            return coMemberIds.Distinct();
        }

        /// <summary>
        ///     Update wright in a delta mode
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="organizationId"></param>
        /// <param name="weight"></param>
        /// <param name="capacityThreshold"></param>
        /// <example>allocation = 50 & ActorOrganization.Allocation = 20 => updated allocation =50+20=70</example>
        public void UpdateWeight(IAgentId actorId, IAgentId organizationId, float weight,
            float capacityThreshold)
        {
            var actorOrganization = Edge(actorId, organizationId);
            if (actorOrganization is null)
            {
                throw new NullReferenceException(nameof(actorOrganization));
            }

            actorOrganization.Weight = Math.Max(actorOrganization.Weight + weight, capacityThreshold);
        }

        /// <summary>
        ///     Update all actorOrganization of the actorId filtered by the groupId.ClassKey
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="organizationClassId">groupId.ClassKey</param>
        /// <param name="fullWeight">true if all actorOrganizations are added, false if we are in modeling phase</param>
        public void UpdateWeights(IAgentId actorId, IClassId organizationClassId, bool fullWeight)
        {
            var actorOrganizations = EdgesFilteredBySourceAndTargetClassId(actorId, organizationClassId).ToList();

            if (!actorOrganizations.Any())
            {
                throw new ArgumentOutOfRangeException("actorId should have a group allocation");
            }

            var totalCapacityAllocation = actorOrganizations.Sum(ga => ga.Weight);

            if (!fullWeight && totalCapacityAllocation <= 100)
            {
                return;
            }

            if (totalCapacityAllocation <= 0)
            {
                throw new ArgumentOutOfRangeException("totalCapacityAllocation should be strictly positif");
            }

            foreach (var groupId in TargetsFilteredBySourceAndTargetClassId(actorId, organizationClassId).ToList())
            {
                // actorOrganization come from an IEnumerable which is readonly
                var updatedGroupAllocation = Edge(actorId, groupId);
                updatedGroupAllocation.Weight =
                    Math.Min(100F, updatedGroupAllocation.Weight * 100F / totalCapacityAllocation);
            }
        }

        /// <summary>
        ///     Get the main group of the actorId filter by the group.ClassKey
        ///     The main group is defined by the maximum GroupAllocation
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="targetClassId"></param>
        /// <returns>
        ///     return AgentId of the main group is exists, default Agent if don't exist, so check the result when using this
        ///     method
        /// </returns>
        public IAgentId GetMainOrganizationOrDefault(IAgentId actorId, IClassId targetClassId)
        {
            var organizationIds = TargetsFilteredBySourceAndTargetClassId(actorId, targetClassId).ToList();
            if (!organizationIds.Any())
            {
                return null;
            }

            var max = EdgesFilteredBySourceAndTargetClassId(actorId, targetClassId).OrderByDescending(ga => ga.Weight).First()
                .Weight;

            return organizationIds.FirstOrDefault(group =>
                EdgesFilteredByTarget(group).ToList().Exists(x => Math.Abs(x.Weight - max) < Tolerance));
        }
    }
}