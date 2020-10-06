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

#endregion

namespace Symu.DNA.GraphNetworks.TwoModesNetworks
{
    /// <summary>
    ///     Actor x Role network
    ///     Who has what functions
    ///     Source : Actor
    ///     Target : Role
    /// </summary>
    public class ActorRoleNetwork : TwoModesNetwork<IActorRole>
    {
        public void RemoveSource(IAgentId actorId, IAgentId organizationId)
        {
            var roles = GetRolesIn(actorId, organizationId);
            foreach (var role in roles)
            {
                Remove(role);
            }
        }

        /// <summary>
        ///     List of organizationIds actor is member of
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="organizationClassId"></param>
        /// <returns></returns>
        public IEnumerable<IAgentId> IsActorOfOrganizationIds(IAgentId actorId, IClassId organizationClassId)
        {
            return EdgesFilteredBySource(actorId).Where(l => l.IsOrganization(organizationClassId))
                .Select(x => x.OrganizationId);
        }

        /// <summary>
        ///     List of organizationIds actor is member of
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="organizationClassId"></param>
        /// <returns></returns>
        public bool IsActorOf(IAgentId actorId, IClassId organizationClassId)
        {
            return EdgesFilteredBySource(actorId).ToList().Exists(l => l.IsOrganization(organizationClassId));
        }

        public bool ExistActorForRoleType(IAgentId roleId, IAgentId organizationId)
        {
            return EdgesFilteredByTarget(roleId).ToList().Exists(l => l.IsOrganization(organizationId));
        }

        public IAgentId GetActorIdForRoleType(IAgentId roleId, IAgentId organizationId)
        {
            return ExistActorForRoleType(roleId, organizationId)
                ? EdgesFilteredByTarget(roleId).First(l => l.IsOrganization(organizationId)).Source
                : null;
        }
        public bool HasARoleIn(IAgentId actorId, IAgentId roleId, IAgentId organizationId)
        {
            return Edges(actorId, roleId).ToList().Exists(l => l.IsOrganization(organizationId));
        }

        public bool HasARoleIn(IAgentId actorId, IAgentId organizationId)
        {
            return EdgesFilteredBySource(actorId).ToList().Exists(l => l.IsOrganization(organizationId));
        }

        /// <summary>
        ///     Get the roles of the actorId for the organizationId
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public IEnumerable<IActorRole> GetRolesIn(IAgentId actorId, IAgentId organizationId)
        {
            return EdgesFilteredBySource(actorId).Where(x => x.IsOrganization(organizationId));
        }

        /// <summary>
        ///     Get all the roles for the organizationId
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public IEnumerable<IActorRole> GetRoles(IAgentId organizationId)
        {
            return Edges().Where(r => r.IsOrganization(organizationId));
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

            var roles = GetRolesIn(actorId, organizationSourceId).ToList();
            foreach (var actorRole in roles.Select(role => (IActorRole)role.Clone()))
            {
                actorRole.OrganizationId = organizationTargetId;
                Add(actorRole);
            }

            RemoveSource(actorId, organizationSourceId);
        }

        public void RemoveActorsByRoleFromOrganization(IAgentId roleId, IAgentId organizationId)
        {
            var actorId = GetActorIdForRoleType(roleId, organizationId);
            RemoveSource(actorId);
        }
    }
}