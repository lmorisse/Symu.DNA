﻿#region Licence

// Description: SymuBiz - Symu
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Symu.Common.Interfaces.Agent;
using static Symu.Common.Constants;
#endregion

namespace Symu.DNA.Groups
{
    /// <summary>
    ///     Dictionary of all the group of the network
    ///     for every group, the list of all the local AgentIds
    ///     Key => Group Id
    ///     Value => List of AgentIds
    /// </summary>
    /// <example>Groups : team, task force, quality circle, community of practices, committees, ....</example>
    public class GroupNetwork
    {
        /// <summary>
        ///     Key => groupId
        ///     Value => list of group allocation : AgentId, Allocation of the agentId to the groupId
        /// </summary>
        public ConcurrentDictionary<IAgentId, List<IAgentGroup>> AgentGroups { get; } =
            new ConcurrentDictionary<IAgentId, List<IAgentGroup>>();

        /// <summary>
        ///     Remove agent from network,
        ///     either it is a group or a member of a group
        /// </summary>
        /// <param name="agentId"></param>
        public void RemoveAgent(IAgentId agentId)
        {
            if (Exists(agentId))
            {
                RemoveGroup(agentId);
            }

            RemoveMember(agentId);
        }

        public void RemoveMember(IAgentId agentId)
        {
            foreach (var groupId in GetGroups().ToList())
            {
                RemoveMember(agentId, groupId);
            }
        }

        public void RemoveMember(IAgentId agentId, IAgentId groupId)
        {
            if (Exists(groupId))
            {
                AgentGroups[groupId].RemoveAll(g => g.AgentId.Equals(agentId));
            }
        }

        public IEnumerable<IAgentId> GetGroups()
        {
            return AgentGroups.Any() ? AgentGroups.Keys : new List<IAgentId>();
        }

        /// <summary>
        ///     Check that Group exist, either a team, a kanban, ...
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public bool Exists(IAgentId groupId)
        {
            return AgentGroups.ContainsKey(groupId);
        }

        public void RemoveGroup(IAgentId groupId)
        {
            AgentGroups.TryRemove(groupId, out _);
        }

        public bool Any()
        {
            return AgentGroups.Any();
        }

        public void Clear()
        {
            AgentGroups.Clear();
        }

        public void AddGroup(IAgentId groupId)
        {
            if (!Exists(groupId))
            {
                AgentGroups.TryAdd(groupId, new List<IAgentGroup>());
            }
        }

        /// <summary>
        ///     Add agent to a group
        /// </summary>
        /// <param name="agentGroup"></param>
        /// <param name="groupId"></param>
        public void AddAgent(IAgentGroup agentGroup, IAgentId groupId)
        {
            AddGroup(groupId);
            if (!IsMemberOfGroup(agentGroup.AgentId, groupId))
            {
                AgentGroups[groupId].Add(agentGroup);
            }
            else
            {
                var groupAllocation = GetGroupAllocation(agentGroup.AgentId, groupId);
                groupAllocation.Allocation = agentGroup.Allocation;
            }

            if (groupId != null)
            {
                UpdateGroupAllocations(agentGroup.AgentId, groupId.ClassId, false);
            }
        }

        /// <summary>
        ///     Get agents of a group
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public IEnumerable<IAgentId> GetAgents(IAgentId groupId)
        {
            return Exists(groupId)
                ? AgentGroups[groupId].Select(x => x.AgentId)
                : null;
        }

        /// <summary>
        ///     Get agents of a group filtered by classKey
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="classId"></param>
        /// <returns></returns>
        public IEnumerable<IAgentId> GetAgents(IAgentId groupId, IClassId classId)
        {
            return Exists(groupId)
                ? AgentGroups[groupId].FindAll(x => x.AgentId.ClassId.Equals(classId)).Select(x => x.AgentId)
                : null;
        }

        /// <summary>
        ///     Get agents count of a group
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public byte GetAgentsCount(IAgentId groupId)
        {
            return Exists(groupId) ? Convert.ToByte(AgentGroups[groupId].Count) : (byte) 0;
        }

        /// <summary>
        ///     Get members count of a group
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="classId"></param>
        /// <returns></returns>
        public byte GetAgentsCount(IAgentId groupId, IClassId classId)
        {
            if (!Exists(groupId))
            {
                return 0;
            }

            lock (AgentGroups[groupId])
            {
                return Convert.ToByte(AgentGroups[groupId].Count(x => x.AgentId.ClassId.Equals(classId)));
            }
        }

        public bool IsMemberOfGroup(IAgentId agentId, IAgentId groupId)
        {
            return Exists(groupId) && AgentGroups[groupId].Exists(g => g != null && g.AgentId.Equals(agentId));
        }

        public IAgentGroup GetGroupAllocation(IAgentId agentId, IAgentId groupId)
        {
            return Exists(groupId) ? AgentGroups[groupId].Find(g => g != null && g.AgentId.Equals(agentId)) : null;
        }

        public float GetAllocation(IAgentId agentId, IAgentId groupId)
        {
            if (IsMemberOfGroup(agentId, groupId))
            {
                return AgentGroups[groupId].Find(g => g != null && g.AgentId.Equals(agentId)).Allocation;
            }

            return 0;
        }

        /// <summary>
        ///     Get the list of the groupIds of a agentId, filtered by group.ClassKey
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="groupClassId"></param>
        /// <returns>List of groupIds</returns>
        public IEnumerable<IAgentId> GetGroups(IAgentId agentId, IClassId groupClassId)
        {
            var groupIds = new List<IAgentId>();
            if (!AgentGroups.Any())
            {
                return groupIds;
            }

            groupIds.AddRange(GetGroups().Where(g => g.ClassId.Equals(groupClassId) && IsMemberOfGroup(agentId, g)));

            return groupIds;
        }

        /// <summary>
        ///     Get the list of all the groupIds filtered by group.ClassKey
        /// </summary>
        /// <param name="groupClassId"></param>
        /// <returns>List of groupIds</returns>
        public IEnumerable<IAgentId> GetGroups(IClassId groupClassId)
        {
            var groupIds = new List<IAgentId>();
            if (!AgentGroups.Any())
            {
                return groupIds;
            }

            groupIds.AddRange(GetGroups().Where(g => g.ClassId.Equals(groupClassId)));

            return groupIds;
        }

        /// <summary>
        ///     Get the list of the members of all the groups of a agentId, filtered by group.ClassKey
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="groupClassId"></param>
        /// <returns>List of groupIds</returns>
        public IEnumerable<IAgentId> GetCoMemberIds(IAgentId agentId, IClassId groupClassId)
        {
            var coMemberIds = new List<IAgentId>();
            var groupIds = GetGroups(agentId, groupClassId).ToList();
            if (!groupIds.Any())
            {
                return coMemberIds;
            }

            foreach (var groupId in groupIds)
            {
                coMemberIds.AddRange(AgentGroups[groupId].FindAll(x => !x.AgentId.Equals(agentId)).Select(x => x.AgentId));
            }

            return coMemberIds.Distinct();
        }

        /// <summary>
        ///     Get the list of the group allocations of a agentId, filtered by group.ClassKey
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="classId"></param>
        /// <returns>List of groupAllocations (groupId, Allocation)</returns>
        public IEnumerable<IAgentGroup> GetAgentGroupsOfAnAgentId(IAgentId agentId, IClassId classId)
        {
            var groupAllocations = new List<IAgentGroup>();
            if (!AgentGroups.Any())
            {
                return groupAllocations;
            }

            foreach (var group in GetGroups().ToList()
                .Where(g => g.ClassId.Equals(classId) && IsMemberOfGroup(agentId, g)))
            {
                groupAllocations.AddRange(AgentGroups[group].FindAll(x => x.AgentId.Equals(agentId)));
            }

            return groupAllocations;
        }

        /// <summary>
        ///     Get the list of the group allocations of a groupId, filtered by agentId.ClassKey
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="classId"></param>
        /// <returns>List of groupAllocations (groupId, Allocation)</returns>
        public IEnumerable<IAgentGroup> GetAgentGroupsOfAGroupId(IAgentId groupId, IClassId classId)
        {
            return Exists(groupId)
                ? AgentGroups[groupId].FindAll(x => x.AgentId.ClassId.Equals(classId))
                : new List<IAgentGroup>();
        }

        /// <summary>
        ///     Get the total allocation of a groupId
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns>allocation</returns>
        public float GetAgentAllocations(IAgentId groupId)
        {
            return Exists(groupId) ? AgentGroups[groupId].Sum(a => a.Allocation) : 0;
        }

        /// <summary>
        ///     Update GroupAllocation in a delta mode
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="groupId"></param>
        /// <param name="allocation"></param>
        /// <param name="capacityThreshold"></param>
        /// <example>allocation = 50 & groupAllocation = 20 => updated groupAllocation =50+20=70</example>
        public void UpdateGroupAllocation(IAgentId agentId, IAgentId groupId, float allocation, float capacityThreshold)
        {
            var groupAllocation = GetGroupAllocation(agentId, groupId);
            if (groupAllocation is null)
            {
                throw new NullReferenceException(nameof(groupAllocation));
            }

            groupAllocation.Allocation = Math.Max(groupAllocation.Allocation + allocation, capacityThreshold);
        }

        /// <summary>
        ///     Update all groupAllocation of the agentId filtered by the groupId.ClassKey
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="classId">groupId.ClassKey</param>
        /// <param name="fullAlloc">true if all groupAllocations are added, false if we are in modeling phase</param>
        public void UpdateGroupAllocations(IAgentId agentId, IClassId classId, bool fullAlloc)
        {
            var groupAllocations = GetAgentGroupsOfAnAgentId(agentId, classId).ToList();

            if (!groupAllocations.Any())
            {
                throw new ArgumentOutOfRangeException("agentId should should have a group allocation");
            }

            var totalCapacityAllocation = groupAllocations.Sum(ga => ga.Allocation);

            if (!fullAlloc && totalCapacityAllocation <= 100)
            {
                return;
            }

            if (totalCapacityAllocation <= 0)
            {
                throw new ArgumentOutOfRangeException("totalCapacityAllocation should be strictly positif");
            }

            foreach (var groupId in GetGroups(agentId, classId).ToList())
            {
                // groupAllocation come from an IEnumerable which is readonly
                var updatedGroupAllocation = GetGroupAllocation(agentId, groupId);
                updatedGroupAllocation.Allocation =
                    Math.Min(100F, updatedGroupAllocation.Allocation * 100F / totalCapacityAllocation);
            }
        }

        /// <summary>
        ///     Get the main group of the agentId filter by the group.ClassKey
        ///     The main group is defined by the maximum GroupAllocation
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="classId"></param>
        /// <returns>
        ///     return AgentId of the main group is exists, default Agent if don't exist, so check the result when using this
        ///     method
        /// </returns>
        public IAgentId GetMainGroupOrDefault(IAgentId agentId, IClassId classId)
        {
            var groups = GetGroups(agentId, classId).ToList();
            if (!groups.Any())
            {
                return null;
            }
            var max = GetAgentGroupsOfAnAgentId(agentId, classId).OrderByDescending(ga => ga.Allocation).First().Allocation;

            return groups.FirstOrDefault(group => AgentGroups[group].Exists(x => Math.Abs(x.Allocation - max) < Tolerance));
        }

        /// <summary>
        ///     Copy all groupAllocations of a groupSourceId into groupTargetId
        /// </summary>
        /// <param name="groupSourceId"></param>
        /// <param name="groupTargetId"></param>
        public void CopyTo(IAgentId groupSourceId, IAgentId groupTargetId)
        {
            AddGroup(groupTargetId);
            foreach (var groupAllocation in AgentGroups[groupSourceId])
            {
                AgentGroups[groupTargetId].Add(groupAllocation);
            }
        }
    }
}