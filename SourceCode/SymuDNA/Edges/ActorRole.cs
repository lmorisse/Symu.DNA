#region Licence

// Description: SymuBiz - SymuDNA
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using Symu.Common.Interfaces;

#endregion

namespace Symu.DNA.Edges
{
    /// <summary>
    ///     Default implementation of IActorRole
    ///     An actor has a role in a organization
    /// </summary>
    public class ActorRole : IActorRole
    {
        public ActorRole(IAgentId actorId, IAgentId roleId, IAgentId organizationId)
        {
            Source = actorId;
            Target = roleId;
            OrganizationId = organizationId;
        }

        #region IActorRole Members

        /// <summary>
        ///     Unique key of the group
        /// </summary>
        public IAgentId OrganizationId { get; set; }


        public bool IsOrganization(IAgentId organizationId)
        {
            return OrganizationId.Equals(organizationId);
        }

        public bool IsOrganization(IClassId organizationClassId)
        {
            return OrganizationId.ClassId.Equals(organizationClassId);
        }

        #endregion

        public override bool Equals(object obj)
        {
            return obj is ActorRole actorRole &&
                   Target.Equals(actorRole.Target) &&
                   Source.Equals(actorRole.Source) &&
                   OrganizationId.Equals(actorRole.OrganizationId);
        }

        #region Interface IEdge

        /// <summary>
        ///     Number of interactions between the two agents
        /// </summary>
        public float Weight { get; set; } = 1;
        /// <summary>
        ///     Normalized weight computed by the network via the NormalizeWeights method
        /// </summary>
        public float NormalizedWeight { get; set; }
        public bool Equals(IAgentId source, IAgentId target)
        {
            return EqualsTarget(target) && EqualsSource(source);
        }

        public bool EqualsSource(IAgentId source)
        {
            return source.Equals(Source);
        }

        public bool EqualsTarget(IAgentId target)
        {
            return target.Equals(Target);
        }

        /// <summary>
        ///     Unique key of the agent with the smallest key
        /// </summary>
        public IAgentId Source { get; set; }

        /// <summary>
        ///     Unique key of the agent with the highest key
        /// </summary>
        public IAgentId Target { get; set; }
        public object Clone()
        {
            return new ActorRole(Source, Target, OrganizationId);
        }

        #endregion

        /// <summary>
        ///     CHeck that there is a role of roleType for that groupId
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public bool HasRoleInOrganization(IAgentId roleId, IAgentId organizationId)
        {
            return EqualsTarget(roleId) && IsOrganization(organizationId);
        }

        public bool HasRoleInOrganization(IAgentId actorId, IAgentId roleId, IAgentId organizationId)
        {
            return EqualsSource(actorId) && HasRoleInOrganization(roleId, organizationId);
        }

        public bool HasRolesInOrganization(IAgentId actorId, IAgentId organizationId)
        {
            return EqualsSource(actorId) && IsOrganization(organizationId);
        }
        public bool IsActorOfOrganizations(IAgentId actorId, IClassId organizationClassId)
        {
            return OrganizationId.Equals(organizationClassId) && EqualsSource(actorId);
        }
    }
}