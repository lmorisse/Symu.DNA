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
using Symu.DNA.MatrixNetworks.OneModeNetworks;
using Symu.DNA.Networks.OneModeNetworks;

#endregion

namespace Symu.DNA.Networks.TwoModesNetworks.AgentRole
{
    public class AgentRoleNetwork
    {
        public List<IAgentRole> List { get; } = new List<IAgentRole>();

        /// <summary>
        ///     Gets or sets the element at the specified index
        /// </summary>
        /// <param name="index">0 based</param>
        /// <returns></returns>
        public IAgentRole this[int index]
        {
            get => List[index];
            set => List[index] = value;
        }

        public void Clear()
        {
            List.Clear();
        }

        public void RemoveAgent(IAgentId agentId)
        {
            // Remove agentId as an Agent
            List.RemoveAll(l => l.AgentId.Equals(agentId));
            // Remove agentId as a Group
            List.RemoveAll(l => l.GroupId.Equals(agentId));
        }

        public bool Any()
        {
            return List.Any();
        }

        /// <summary>
        ///     List of groupIds teammate is member of
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="groupClassId"></param>
        /// <returns></returns>
        public IEnumerable<IAgentId> IsMemberOfGroups(IAgentId agentId, IClassId groupClassId)
        {
            return List.FindAll(l => l != null && l.IsMemberOfGroups(agentId, groupClassId)).Select(x => x.GroupId);
        }

        /// <summary>
        ///     List of groupIds teammate is member of
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="groupClassId"></param>
        /// <returns></returns>
        public bool IsMember(IAgentId agentId, IClassId groupClassId)
        {
            return List.Exists(l => l != null && l.IsMemberOfGroups(agentId, groupClassId));
        }

        public bool ExistAgentForRoleType(IRole role, IAgentId groupId)
        {
            return List.Exists(l => l != null && l.HasRoleInGroup(role, groupId));
        }

        public IAgentId GetAgentIdForRoleType(IRole role, IAgentId groupId)
        {
            var group = List.Find(l => l != null && l.HasRoleInGroup(role, groupId));
            return group?.AgentId;
        }

        public IEnumerable<IAgentId> GetGroups(IAgentId agentId, IRole role)
        {
            return List.FindAll(l => l != null && l.HasRole(agentId, role)).Select(x => x.GroupId);
        }

        /// <summary>
        ///     Check if agentId has a role in a team
        /// </summary>
        public bool HasRoles(IAgentId agentId)
        {
            return List.Exists(l => l != null && l.AgentId.Equals(agentId));
        }

        public bool HasRole(IAgentId agentId, IRole role)
        {
            return List.Exists(l => l != null && l.HasRole(agentId, role));
        }

        public bool HasARoleIn(IAgentId agentId, IRole role, IAgentId groupId)
        {
            return List.Exists(l => l != null && l.HasRoleInGroup(agentId, role, groupId));
        }

        public bool HasARoleIn(IAgentId agentId, IAgentId groupId)
        {
            return List.Exists(l => l != null && l.HasRoleInGroup(agentId, groupId));
        }

        public void RemoveMember(IAgentId agentId, IAgentId groupId)
        {
            List.RemoveAll(l => l == null || l.HasRoleInGroup(agentId, groupId));
        }

        /// <summary>
        ///     Get the roles of the agentId for the groupId
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public IEnumerable<IAgentRole> GetRoles(IAgentId agentId, IAgentId groupId)
        {
            lock (List)
            {
                return List.Where(r => r.HasRoleInGroup(agentId, groupId));
            }
        }

        /// <summary>
        ///     Get all the roles for the groupId
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public IEnumerable<IAgentRole> GetRoles(IAgentId groupId)
        {
            return List.Where(r => r.IsGroup(groupId));
        }

        /// <summary>
        ///     Get the roles of the agentId for the groupId
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public IEnumerable<IAgentId> GetAgents(IRole role)
        {
            return List.Where(r => r.HasRole(role)).Select(x => x.AgentId);
        }

        /// <summary>
        ///     Transfer characteristics of the agentId roles with the groupSourceId to groupTargetId
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="groupSourceId"></param>
        /// <param name="groupTargetId"></param>
        public void TransferTo(IAgentId agentId, IAgentId groupSourceId, IAgentId groupTargetId)
        {
            if (groupSourceId == null)
            {
                throw new ArgumentNullException(nameof(groupSourceId));
            }

            if (groupSourceId.Equals(groupTargetId))
            {
                return;
            }

            lock (List)
            {
                var roles = GetRoles(agentId, groupSourceId).ToList();
                foreach (var role in roles)
                {
                    var agentRole = role.Clone();
                    agentRole.GroupId = groupTargetId;
                    List.Add(agentRole);
                }

                RemoveMember(agentId, groupSourceId);
            }
        }

        public void Add(IAgentRole agentRole)
        {
            if (Exists(agentRole))
            {
                return;
            }

            List.Add(agentRole);
        }

        public bool Exists(IAgentRole agentRole)
        {
            return List.Contains(agentRole);
        }

        public void RemoveMembersByRoleTypeFromGroup(IRole role, IAgentId groupId)
        {
            List.RemoveAll(l => l.HasRoleInGroup(role, groupId));
        }
        /// <summary>
        /// Convert the network into a matrix
        /// </summary>
        /// <param name="agentIds"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public Matrix<float> ToMatrix(VectorNetwork<IAgentId> agentIds, VectorNetwork<IId> roleIds)
        {
            if (!agentIds.Any || !roleIds.Any)
            {
                return null;
            }
            var matrix = Matrix<float>.Build.Dense(agentIds.Count, roleIds.Count);
            foreach (var agentRoles in List.GroupBy(x => x.AgentId))
            {
                if (!agentIds.ItemIndex.ContainsKey(agentRoles.Key))
                {
                    throw new NullReferenceException(nameof(agentIds.ItemIndex));
                }
                var row = agentIds.ItemIndex[agentRoles.Key];
                foreach (var agentRole in agentRoles)
                {
                    if (!roleIds.ItemIndex.ContainsKey(agentRole.Role.Id))
                    {
                        throw new NullReferenceException(nameof(roleIds.ItemIndex));
                    }
                    var column = roleIds.ItemIndex[agentRole.Role.Id];
                    matrix[row, column] = agentRole.Value;
                }
            }
            return matrix;
        }
    }
}