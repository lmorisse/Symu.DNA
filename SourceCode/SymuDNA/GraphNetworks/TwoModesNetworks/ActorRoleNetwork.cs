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
    ///     Actor x Role network
    ///     Who has what functions
    /// </summary>
    public class ActorRoleNetwork
    {
        public List<IActorRole> List { get; private set; } = new List<IActorRole>();

        /// <summary>
        ///     Gets or sets the element at the specified index
        /// </summary>
        /// <param name="index">0 based</param>
        /// <returns></returns>
        public IActorRole this[int index]
        {
            get => List[index];
            set => List[index] = value;
        }

        public void Clear()
        {
            List.Clear();
        }

        public void RemoveActor(IAgentId actorId)
        {
            // Remove actorId as an Agent
            List.RemoveAll(l => l.ActorId.Equals(actorId));
            // Remove actorId as a Group
            List.RemoveAll(l => l.OrganizationId.Equals(actorId));
        }

        public void RemoveActor(IAgentId actorId, IAgentId organizationId)
        {
            List.RemoveAll(l => l == null || l.HasRoleInOrganization(actorId, organizationId));
        }
        public void RemoveRole(IAgentId roleId)
        {
            List.RemoveAll(x => x.HasRole(roleId));
        }


        public bool Any()
        {
            return List.Any();
        }

        /// <summary>
        ///     List of organizationIds actor is member of
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="organizationClassId"></param>
        /// <returns></returns>
        public IEnumerable<IAgentId> IsActorOfOrganizations(IAgentId actorId, IClassId organizationClassId)
        {
            return List.FindAll(l => l != null && l.IsActorOfOrganizations(actorId, organizationClassId)).Select(x => x.OrganizationId);
        }

        /// <summary>
        ///     List of organizationIds actor is member of
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="organizationClassId"></param>
        /// <returns></returns>
        public bool IsActorOf(IAgentId actorId, IClassId organizationClassId)
        {
            return List.Exists(l => l != null && l.IsActorOfOrganizations(actorId, organizationClassId));
        }

        public bool ExistActorForRoleType(IRole role, IAgentId organizationId)
        {
            return List.Exists(l => l != null && l.HasRoleInOrganization(role, organizationId));
        }

        public IAgentId GetActorIdForRoleType(IRole role, IAgentId organizationId)
        {
            var group = List.Find(l => l != null && l.HasRoleInOrganization(role, organizationId));
            return group?.ActorId;
        }

        public IEnumerable<IAgentId> GetOrganizations(IAgentId actorId, IRole role)
        {
            return List.FindAll(l => l != null && l.HasRole(actorId, role)).Select(x => x.OrganizationId);
        }

        /// <summary>
        ///     Check if actorId has a role 
        /// </summary>
        public bool HasRoles(IAgentId actorId)
        {
            return List.Exists(l => l != null && l.ActorId.Equals(actorId));
        }

        public bool HasRole(IAgentId actorId, IRole role)
        {
            return List.Exists(l => l != null && l.HasRole(actorId, role));
        }

        public bool HasARoleIn(IAgentId actorId, IRole role, IAgentId organizationId)
        {
            return List.Exists(l => l != null && l.HasRoleInOrganization(actorId, role, organizationId));
        }

        public bool HasARoleIn(IAgentId actorId, IAgentId organizationId)
        {
            return List.Exists(l => l != null && l.HasRoleInOrganization(actorId, organizationId));
        }


        /// <summary>
        ///     Get the roles of the actorId
        /// </summary>
        /// <param name="actorId"></param>
        /// <returns></returns>
        public IEnumerable<IActorRole> GetActorRoles(IAgentId actorId)
        {
            return List.Where(r => r.IsActor(actorId));
        }

        /// <summary>
        ///     Get the roles of the actorId for the organizationId
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public IEnumerable<IActorRole> GetRolesIn(IAgentId actorId, IAgentId organizationId)
        {
            lock (List)
            {
                return List.Where(r => r.HasRoleInOrganization(actorId, organizationId));
            }
        }

        /// <summary>
        ///     Get all the roles for the organizationId
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public IEnumerable<IActorRole> GetRoles(IAgentId organizationId)
        {
            return List.Where(r => r.IsOrganization(organizationId));
        }

        /// <summary>
        ///     Get the roles of the actorId for the organizationId
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public IEnumerable<IAgentId> GetActors(IRole role)
        {
            return List.Where(r => r.HasRole(role)).Select(x => x.ActorId);
        }

        /// <summary>
        ///     Transfer characteristics of the actorId roles with the organizationSourceId to organizationTargetId
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="organizationSourceId"></param>
        /// <param name="organizationTargetId"></param>
        public void TransferTo(IAgentId actorId, IAgentId organizationSourceId, IAgentId organizationTargetId)
        {
            if (organizationSourceId == null)
            {
                throw new ArgumentNullException(nameof(organizationSourceId));
            }

            if (organizationSourceId.Equals(organizationTargetId))
            {
                return;
            }

            lock (List)
            {
                var roles = GetRolesIn(actorId, organizationSourceId).ToList();
                foreach (var role in roles)
                {
                    var actorRole = role.Clone();
                    actorRole.OrganizationId = organizationTargetId;
                    List.Add(actorRole);
                }

                RemoveActor(actorId, organizationSourceId);
            }
        }

        public void Add(IActorRole actorRole)
        {
            if (Exists(actorRole))
            {
                return;
            }

            List.Add(actorRole);
        }

        public bool Exists(IActorRole actorRole)
        {
            return List.Contains(actorRole);
        }

        public void RemoveActorsByRoleFromOrganization(IRole role, IAgentId organizationId)
        {
            List.RemoveAll(l => l.HasRoleInOrganization(role, organizationId));
        }
        /// <summary>
        /// Convert the network into a matrix
        /// </summary>
        /// <param name="actorIds"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public Matrix<float> ToMatrix(VectorNetwork<IAgentId> actorIds, VectorNetwork<IId> roleIds)
        {
            if (!actorIds.Any || !roleIds.Any)
            {
                return null;
            }
            var matrix = Matrix<float>.Build.Dense(actorIds.Count, roleIds.Count);
            foreach (var actorRoles in List.GroupBy(x => x.ActorId))
            {
                if (!actorIds.ItemIndex.ContainsKey(actorRoles.Key))
                {
                    throw new NullReferenceException(nameof(actorIds.ItemIndex));
                }
                var row = actorIds.ItemIndex[actorRoles.Key];
                foreach (var actorRole in actorRoles)
                {
                    if (!roleIds.ItemIndex.ContainsKey(actorRole.Role.EntityId))
                    {
                        throw new NullReferenceException(nameof(roleIds.ItemIndex));
                    }
                    var column = roleIds.ItemIndex[actorRole.Role.EntityId];
                    matrix[row, column] = actorRole.Value;
                }
            }
            return matrix;
        }
        /// <summary>
        ///     Make a clone of Portfolios from modeling to Symu
        /// </summary>
        /// <param name="network"></param>
        public void CopyTo(ActorRoleNetwork network)
        {
            if (network is null)
            {
                throw new ArgumentNullException(nameof(network));
            }

            network.List = List.ToList();
        }


    }
}