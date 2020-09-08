#region Licence

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
using MathNet.Numerics.LinearAlgebra;
using Symu.Common.Interfaces.Agent;
using Symu.DNA.MatrixNetworks;
using static Symu.Common.Constants;
#endregion

namespace Symu.DNA.Networks.TwoModesNetworks
{        
    /// <summary>
    ///     Agent x Organization network, called work network, employment network
    ///     Who works where
    /// </summary>
    /// <example>Groups : team, task force, quality circle, community of practices, committees, enterprise....</example>
    public class AgentOrganizationNetwork : ConcurrentTwoModesNetwork<IAgentId, IAgentOrganization>
    {
        /// <summary>
        ///     Remove agent from network,
        ///     either it is a group or a member of a group
        /// </summary>
        /// <param name="key"></param>
        public override void RemoveKey(IAgentId key)
        {
            base.RemoveKey(key);

            RemoveMember(key);
        }

        public void RemoveMember(IAgentId agentId)
        {
            foreach (var groupId in GetKeys().ToList())
            {
                RemoveMember(agentId, groupId);
            }
        }

        public void RemoveMember(IAgentId agentId, IAgentId groupId)
        {
            if (Exists(groupId))
            {
                List[groupId].RemoveAll(g => g.AgentId.Equals(agentId));
            }
        }

        /// <summary>
        ///     Add agent to a group
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public override void AddValue(IAgentId key, IAgentOrganization value)
        {
            if (!IsMemberOfGroup(value.AgentId, key))
            {
                List[key].Add(value);
            }
            else
            {
                var groupAllocation = GetGroupAllocation(value.AgentId, key);
                groupAllocation.Allocation = value.Allocation;
            }

            if (key != null)
            {
                UpdateGroupAllocations(value.AgentId, key.ClassId, false);
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
                ? List[groupId].Select(x => x.AgentId)
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
                ? List[groupId].FindAll(x => x.AgentId.ClassId.Equals(classId)).Select(x => x.AgentId)
                : null;
        }

        /// <summary>
        ///     Get members count of a group
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="classId"></param>
        /// <returns></returns>
        public byte GetValuesCount(IAgentId groupId, IClassId classId)
        {
            if (!Exists(groupId))
            {
                return 0;
            }

            lock (List[groupId])
            {
                return Convert.ToByte(List[groupId].Count(x => x.AgentId.ClassId.Equals(classId)));
            }
        }

        public bool IsMemberOfGroup(IAgentId agentId, IAgentId groupId)
        {
            return Exists(groupId) && List[groupId].Exists(g => g != null && g.AgentId.Equals(agentId));
        }

        public IAgentOrganization GetGroupAllocation(IAgentId agentId, IAgentId groupId)
        {
            return Exists(groupId) ? List[groupId].Find(g => g != null && g.AgentId.Equals(agentId)) : null;
        }

        public float GetAllocation(IAgentId agentId, IAgentId groupId)
        {
            if (IsMemberOfGroup(agentId, groupId))
            {
                return List[groupId].Find(g => g != null && g.AgentId.Equals(agentId)).Allocation;
            }

            return 0;
        }

        /// <summary>
        ///     Get the list of the groupIds of a agentId
        /// </summary>
        /// <param name="agentId"></param>
        /// <returns>List of groupIds</returns>
        public IEnumerable<IAgentId> GetGroups(IAgentId agentId)
        {
            var groupIds = new List<IAgentId>();
            if (!List.Any())
            {
                return groupIds;
            }

            groupIds.AddRange(GetKeys().Where(g => IsMemberOfGroup(agentId, g)));

            return groupIds;
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
            if (!List.Any())
            {
                return groupIds;
            }

            groupIds.AddRange(GetKeys().Where(g => g.ClassId.Equals(groupClassId) && IsMemberOfGroup(agentId, g)));

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
            if (!List.Any())
            {
                return groupIds;
            }

            groupIds.AddRange(GetKeys().Where(g => g.ClassId.Equals(groupClassId)));

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
                coMemberIds.AddRange(List[groupId].FindAll(x => !x.AgentId.Equals(agentId)).Select(x => x.AgentId));
            }

            return coMemberIds.Distinct();
        }

        /// <summary>
        ///     Get the list of the group allocations of a agentId, filtered by group.ClassKey
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="classId"></param>
        /// <returns>List of groupAllocations (groupId, Allocation)</returns>
        public IEnumerable<IAgentOrganization> GetAgentGroupsOfAnAgentId(IAgentId agentId, IClassId classId)
        {
            var agentGroups = new List<IAgentOrganization>();
            if (!List.Any())
            {
                return agentGroups;
            }

            foreach (var group in GetKeys().ToList()
                .Where(g => g.ClassId.Equals(classId) && IsMemberOfGroup(agentId, g)))
            {
                agentGroups.AddRange(List[group].FindAll(x => x.AgentId.Equals(agentId)));
            }

            return agentGroups;
        }

        /// <summary>
        ///     Get the list of the group allocations of a groupId, filtered by agentId.ClassKey
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="classId"></param>
        /// <returns>List of groupAllocations (groupId, Allocation)</returns>
        public IEnumerable<IAgentOrganization> GetAgentGroupsOfAGroupId(IAgentId groupId, IClassId classId)
        {
            return Exists(groupId)
                ? List[groupId].FindAll(x => x.AgentId.ClassId.Equals(classId))
                : new List<IAgentOrganization>();
        }

        /// <summary>
        ///     Get the total allocation of a groupId
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns>allocation</returns>
        public float GetAgentAllocations(IAgentId groupId)
        {
            return Exists(groupId) ? List[groupId].Sum(a => a.Allocation) : 0;
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

            return groups.FirstOrDefault(group => List[group].Exists(x => Math.Abs(x.Allocation - max) < Tolerance));
        }

        /// <summary>
        ///     Copy all groupAllocations of a groupSourceId into groupTargetId
        /// </summary>
        /// <param name="groupSourceId"></param>
        /// <param name="groupTargetId"></param>
        public void CopyTo(IAgentId groupSourceId, IAgentId groupTargetId)
        {
            AddKey(groupTargetId);
            foreach (var groupAllocation in List[groupSourceId])
            {
                List[groupTargetId].Add(groupAllocation);
            }
        }

        /// <summary>
        /// Convert the network into a matrix
        /// </summary>
        /// <param name="agentIds"></param>
        /// <param name="groupIds"></param>
        /// <returns></returns>
        public Matrix<float> ToMatrix(VectorNetwork<IAgentId> agentIds, VectorNetwork<IAgentId> groupIds)
        {
            if (!agentIds.Any || !groupIds.Any)
            {
                return null;
            }
            var matrix = Matrix<float>.Build.Dense(agentIds.Count, groupIds.Count);

            for (var i = 0; i < agentIds.Count; i++)
            {
                var row = i;
                var agentId = agentIds.IndexItem[i];
                foreach (var groupId in GetGroups(agentId))
                {
                    if (!groupIds.ItemIndex.ContainsKey(groupId))
                    {
                        throw new NullReferenceException(nameof(groupIds.ItemIndex));
                    }
                    var column = groupIds.ItemIndex[groupId];
                    matrix[row, column] = GetAllocation(agentId, groupId);
                }
            }
            return matrix;
        }

        public IReadOnlyList<IAgentId> ToVector()
        {
            return GetKeys().OrderBy(x => x.Id).ToList();
        }
    }
}