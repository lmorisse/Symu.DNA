#region Licence

// Description: SymuBiz - Symu
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

using Symu.Common.Interfaces.Agent;
using Symu.DNA.Entities;

namespace Symu.DNA.GraphNetworks.TwoModesNetworks
{        
    /// <summary>
    ///     Who has what functions
    /// </summary>
    public interface IActorRole
    {
        /// <summary>
        ///     Unique key of the actor
        /// </summary>
        IAgentId ActorId { get; }

        /// <summary>
        ///     Unique key of the organization
        /// </summary>
        IAgentId OrganizationId { get; set; }

        /// <summary>
        ///     An agent may have different role type in a group
        /// </summary>
        IRole Role { get; set; }

        bool IsActorOfOrganizations(IAgentId actorId, IClassId organizationClassId);

        /// <summary>
        ///     CHeck that there is a role of roleType for that OrganizationId
        /// </summary>
        /// <param name="role"></param>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        bool HasRoleInOrganization(IRole role, IAgentId organizationId);

        bool HasRoleInOrganization(IAgentId actorId, IRole role, IAgentId organizationId);
        bool HasRoleInOrganization(IAgentId actorId, IAgentId organizationId);

        /// <summary>
        ///     CHeck that there is a role of roleType for that organization
        /// </summary>
        /// <param name="role"></param>
        /// <param name="actorId"></param>
        /// <returns></returns>
        bool HasRole(IAgentId actorId, IRole role);

        bool HasRole(IRole role);
        bool IsOrganization(IAgentId organizationId);
        bool IsActor(IAgentId actorId);
        IActorRole Clone();
        /// <summary>
        /// The value used to feed the matrix network
        /// For a binary matrix network, the value is 1
        /// </summary>
        float Value { get; }
    }
}