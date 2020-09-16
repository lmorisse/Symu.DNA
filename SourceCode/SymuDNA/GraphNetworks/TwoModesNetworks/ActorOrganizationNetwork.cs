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
using Symu.DNA.MatrixNetworks;
using static Symu.Common.Constants;
#endregion

namespace Symu.DNA.GraphNetworks.TwoModesNetworks
{        
    /// <summary>
    ///     Actor x Organization network, called work network, employment network
    ///     Who works where
    ///     (Should be named OrganizationActor)
    ///     Key : OrganizationId
    ///     Value: ActorOrganization(AgentId, Allocation)
    /// </summary>
    /// <example>Groups : team, task force, quality circle, community of practices, committees, enterprise....</example>
    public class OrganizationActorNetwork : ConcurrentTwoModesNetwork<IAgentId, IActorOrganization>
    {
        //replace by RemoveKey
        public void RemoveOrganization(IAgentId organizationId)
        {
            if (Exists(organizationId))
            {
                List.TryRemove(organizationId, out _);
            }
        }

        public override void RemoveActor(IAgentId actorId)
        {
            foreach (var organizationId in GetKeys().ToList())
            {
                RemoveActor(actorId, organizationId);
            }
        }

        public void RemoveActor(IAgentId actorId, IAgentId organizationId)
        {
            if (Exists(organizationId))
            {
                List[organizationId].RemoveAll(g => g.ActorId.Equals(actorId));
            }
        }

        /// <summary>
        ///     Add actor to an organization
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public override void AddValue(IAgentId key, IActorOrganization value)
        {
            if (!IsActorOfOrganization(value.ActorId, key))
            {
                List[key].Add(value);
            }
            else
            {
                var actorOrganization = GetActorOrganization(value.ActorId, key);
                actorOrganization.Allocation = value.Allocation;
            }

            if (key != null)
            {
                UpdateAllocations(value.ActorId, key.ClassId, false);
            }
        }

        /// <summary>
        ///     Get actors of an organization
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public IEnumerable<IAgentId> GetActorIds(IAgentId organizationId)
        {
            return Exists(organizationId)
                ? List[organizationId].Select(x => x.ActorId)
                : null;
        }

        /// <summary>
        ///     Get actorIds of an organization filtered by classKey
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="classId"></param>
        /// <returns></returns>
        public IEnumerable<IAgentId> GetActorIds(IAgentId organizationId, IClassId classId)
        {
            return Exists(organizationId)
                ? List[organizationId].FindAll(x => x.ActorId.ClassId.Equals(classId)).Select(x => x.ActorId)
                : null;
        }

        /// <summary>
        ///     Get actors count of an organization
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="classId"></param>
        /// <returns></returns>
        public byte GetValuesCount(IAgentId organizationId, IClassId classId)
        {
            if (!Exists(organizationId))
            {
                return 0;
            }

            lock (List[organizationId])
            {
                return Convert.ToByte(List[organizationId].Count(x => x.ActorId.ClassId.Equals(classId)));
            }
        }

        public bool IsActorOfOrganization(IAgentId actorId, IAgentId organizationId)
        {
            return Exists(organizationId) && List[organizationId].Exists(g => g != null && g.ActorId.Equals(actorId));
        }

        public IActorOrganization GetActorOrganization(IAgentId actorId, IAgentId organizationId)
        {
            return Exists(organizationId) ? List[organizationId].Find(g => g != null && g.ActorId.Equals(actorId)) : null;
        }

        public float GetAllocation(IAgentId actorId, IAgentId organizationId)
        {
            if (IsActorOfOrganization(actorId, organizationId))
            {
                return List[organizationId].Find(g => g != null && g.ActorId.Equals(actorId)).Allocation;
            }

            return 0;
        }

        /// <summary>
        ///     Get the list of the organizationIds of a actorId, filtered by group.ClassKey
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="groupClassId"></param>
        /// <returns>List of groupIds</returns>
        public IEnumerable<IAgentId> GetOrganizationIds(IAgentId actorId, IClassId groupClassId)
        {
            var groupIds = new List<IAgentId>();
            if (!List.Any())
            {
                return groupIds;
            }

            groupIds.AddRange(GetKeys().Where(g => g.ClassId.Equals(groupClassId) && IsActorOfOrganization(actorId, g)));

            return groupIds;
        }

        /// <summary>
        ///     Get the list of all the groupIds filtered by group.ClassKey
        /// </summary>
        /// <param name="groupClassId"></param>
        /// <returns>List of groupIds</returns>
        public IEnumerable<IAgentId> GetOrganizationIds(IClassId groupClassId)
        {
            var organizationIds = new List<IAgentId>();
            if (!List.Any())
            {
                return organizationIds;
            }

            organizationIds.AddRange(GetKeys().Where(g => g.ClassId.Equals(groupClassId)));

            return organizationIds;
        }

        /// <summary>
        ///     Get the list of the actors of all the groups of a actorId, filtered by group.ClassKey
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="organizationClassId"></param>
        /// <returns>List of groupIds</returns>
        public IEnumerable<IAgentId> GetCoActorIds(IAgentId actorId, IClassId organizationClassId)
        {
            var coMemberIds = new List<IAgentId>();
            var groupIds = GetOrganizationIds(actorId, organizationClassId).ToList();
            if (!groupIds.Any())
            {
                return coMemberIds;
            }

            foreach (var groupId in groupIds)
            {
                coMemberIds.AddRange(List[groupId].FindAll(x => !x.ActorId.Equals(actorId)).Select(x => x.ActorId));
            }

            return coMemberIds.Distinct();
        }

        /// <summary>
        ///     Get the list of the organization allocations of a actorId, filtered by group.ClassKey
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="classId"></param>
        /// <returns>List of actorOrganizations (groupId, Allocation)</returns>
        public IEnumerable<IActorOrganization> GetActorOrganizationsOfAnActor(IAgentId actorId, IClassId classId)
        {
            var agentOrganizations = new List<IActorOrganization>();
            if (!List.Any())
            {
                return agentOrganizations;
            }

            foreach (var group in GetKeys().ToList()
                .Where(g => g.ClassId.Equals(classId) && IsActorOfOrganization(actorId, g)))
            {
                agentOrganizations.AddRange(List[group].FindAll(x => x.ActorId.Equals(actorId)));
            }

            return agentOrganizations;
        }

        /// <summary>
        ///     Get the list of the organization allocations, filtered by actorId.ClassKey
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="classId"></param>
        /// <returns>List of actorOrganizations (groupId, Allocation)</returns>
        public IEnumerable<IActorOrganization> GetActorOrganizationsOfAnOrganization(IAgentId organizationId, IClassId classId)
        {
            return Exists(organizationId)
                ? List[organizationId].FindAll(x => x.ActorId.ClassId.Equals(classId))
                : new List<IActorOrganization>();
        }

        /// <summary>
        ///     Get the total allocation of an organization
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns>allocation</returns>
        public float GetSumActorAllocations(IAgentId organizationId)
        {
            return Exists(organizationId) ? List[organizationId].Sum(a => a.Allocation) : 0;
        }

        /// <summary>
        ///     Update allocation in a delta mode
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="organizationId"></param>
        /// <param name="allocation"></param>
        /// <param name="capacityThreshold"></param>
        /// <example>allocation = 50 & ActorOrganization.Allocation = 20 => updated allocation =50+20=70</example>
        public void UpdateAllocation(IAgentId actorId, IAgentId organizationId, float allocation, float capacityThreshold)
        {
            var actorOrganization = GetActorOrganization(actorId, organizationId);
            if (actorOrganization is null)
            {
                throw new NullReferenceException(nameof(actorOrganization));
            }

            actorOrganization.Allocation = Math.Max(actorOrganization.Allocation + allocation, capacityThreshold);
        }

        /// <summary>
        ///     Update all actorOrganization of the actorId filtered by the groupId.ClassKey
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="classId">groupId.ClassKey</param>
        /// <param name="fullAlloc">true if all actorOrganizations are added, false if we are in modeling phase</param>
        public void UpdateAllocations(IAgentId actorId, IClassId classId, bool fullAlloc)
        {
            var actorOrganizations = GetActorOrganizationsOfAnActor(actorId, classId).ToList();

            if (!actorOrganizations.Any())
            {
                throw new ArgumentOutOfRangeException("actorId should have a group allocation");
            }

            var totalCapacityAllocation = actorOrganizations.Sum(ga => ga.Allocation);

            if (!fullAlloc && totalCapacityAllocation <= 100)
            {
                return;
            }

            if (totalCapacityAllocation <= 0)
            {
                throw new ArgumentOutOfRangeException("totalCapacityAllocation should be strictly positif");
            }

            foreach (var groupId in GetOrganizationIds(actorId, classId).ToList())
            {
                // actorOrganization come from an IEnumerable which is readonly
                var updatedGroupAllocation = GetActorOrganization(actorId, groupId);
                updatedGroupAllocation.Allocation =
                    Math.Min(100F, updatedGroupAllocation.Allocation * 100F / totalCapacityAllocation);
            }
        }

        /// <summary>
        ///     Get the main group of the actorId filter by the group.ClassKey
        ///     The main group is defined by the maximum GroupAllocation
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="classId"></param>
        /// <returns>
        ///     return AgentId of the main group is exists, default Agent if don't exist, so check the result when using this
        ///     method
        /// </returns>
        public IAgentId GetMainOrganizationOrDefault(IAgentId actorId, IClassId classId)
        {
            var organizationIds = GetOrganizationIds(actorId, classId).ToList();
            if (!organizationIds.Any())
            {
                return null;
            }
            var max = GetActorOrganizationsOfAnActor(actorId, classId).OrderByDescending(ga => ga.Allocation).First().Allocation;

            return organizationIds.FirstOrDefault(group => List[group].Exists(x => Math.Abs(x.Allocation - max) < Tolerance));
        }

        /// <summary>
        ///     Copy all actorOrganizations of a groupSourceId into groupTargetId
        /// </summary>
        /// <param name="organizationSourceId"></param>
        /// <param name="organizationTargetId"></param>
        public void CopyTo(IAgentId organizationSourceId, IAgentId organizationTargetId)
        {
            AddKey(organizationTargetId);
            foreach (var actorOrganization in List[organizationSourceId])
            {
                List[organizationTargetId].Add(actorOrganization);
            }
        }

        /// <summary>
        /// Convert the network into a matrix
        /// </summary>
        /// <param name="actorIds"></param>
        /// <param name="organizationIds"></param>
        /// <returns></returns>
        public Matrix<float> ToMatrix(VectorNetwork<IAgentId> actorIds, VectorNetwork<IId> organizationIds)
        {
            if (!actorIds.Any || !organizationIds.Any)
            {
                return null;
            }
            var matrix = Matrix<float>.Build.Dense(actorIds.Count, organizationIds.Count);
            foreach (var agentOrganizations in List)
            {
                if (!actorIds.ItemIndex.ContainsKey(agentOrganizations.Key))
                {
                    throw new NullReferenceException(nameof(actorIds.ItemIndex));
                }
                var row = actorIds.ItemIndex[agentOrganizations.Key];
                foreach (var agentOrganization in agentOrganizations.Value)
                {
                    if (!organizationIds.ItemIndex.ContainsKey(agentOrganization.ActorId.Id))
                    {
                        throw new NullReferenceException(nameof(organizationIds.ItemIndex));
                    }
                    var column = organizationIds.ItemIndex[agentOrganization.ActorId.Id];
                    matrix[row, column] = agentOrganization.Allocation;
                }
            }
            return matrix;
        }

    }
}