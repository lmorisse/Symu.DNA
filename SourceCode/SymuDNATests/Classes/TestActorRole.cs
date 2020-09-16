#region Licence

// Description: SymuBiz - SymuTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using Symu.Common.Interfaces.Agent;
using Symu.Common.Interfaces.Entity;
using Symu.DNA.Entities;
using Symu.DNA.GraphNetworks.TwoModesNetworks;

#endregion

namespace SymuDNATests.Classes
{
    /// <summary>
    ///     Class for tests
    /// </summary>
    internal sealed class TestActorRole : IActorRole
    {
        public TestActorRole(IAgentId agentId, IAgentId groupId, ushort role)
        {
            ActorId = agentId;
            OrganizationId = groupId;
            Role = new RoleEntity(role);
        }
        public TestActorRole(IAgentId agentId, IAgentId groupId, IId role)
        {
            ActorId = agentId;
            OrganizationId = groupId;
            Role = new RoleEntity(role);
        }

        /// <summary>
        ///     Unique key of the agent
        /// </summary>
        public IAgentId ActorId { get; }

        /// <summary>
        ///     Unique key of the group
        /// </summary>
        public IAgentId OrganizationId { get; set; }

        /// <summary>
        ///     An agent may have different role type in a group
        /// </summary>
        public IRole Role { get; set; }
        public bool IsActorOfOrganizations(IAgentId actorId, IClassId organizationClassId)
        {
            return OrganizationId.Equals(organizationClassId) && IsActor(actorId);
        }

        /// <summary>
        ///     CHeck that there is a role of roleType for that groupId
        /// </summary>
        /// <param name="role"></param>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public bool HasRoleInOrganization(IRole role, IAgentId organizationId)
        {
            return Role.Equals(role) && IsOrganization(organizationId);
        }

        public bool HasRoleInOrganization(IAgentId agentId, IRole role, IAgentId organizationId)
        {
            return Role.Equals(role) && IsActor(agentId) && IsOrganization(organizationId);
        }

        public bool HasRoleInOrganization(IAgentId agentId, IAgentId organizationId)
        {
            return IsActor(agentId) && IsOrganization(organizationId);
        }

        /// <summary>
        ///     CHeck that there is a role of roleType for that groupId
        /// </summary>
        /// <param name="role"></param>
        /// <param name="agentId"></param>
        /// <returns></returns>
        public bool HasRole(IAgentId agentId, IRole role)
        {
            return Role.Equals(role) && IsActor(agentId);
        }

        public bool HasRole(IRole role)
        {
            return Role.Equals(role);
        }

        public bool IsOrganization(IAgentId organizationId)
        {
            return OrganizationId.Equals(organizationId);
        }

        public bool IsActor(IAgentId agentId)
        {
            return ActorId.Equals(agentId);
        }
        public IActorRole Clone()
        {
            return new TestActorRole(ActorId, OrganizationId, ((RoleEntity)Role).AgentId);
        }

        /// <summary>
        /// The value used to feed the matrix network
        /// For a binary matrix network, the value is 1
        /// </summary>
        public float Value => 1;
    }
}