#region Licence

// Description: SymuBiz - SymuDNA
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System.Collections.Generic;
using System.Linq;
using Symu.Common.Interfaces;
using Symu.DNA.Edges;
using Symu.DNA.GraphNetworks;

#endregion

namespace Symu.DNA.Entities
{
    /// <summary>
    ///     An actor is an individual decision makers
    ///     Default implementation of IActor
    /// </summary>
    /// <remarks>Also named agent in social network analysis, but agent is used for multi agents system</remarks>
    public class ActorEntity : Entity<ActorEntity>, IActor
    {
        public const byte Class = ClassIdCollection.Actor;
        public static IClassId ClassId => new ClassId(Class);
        public ActorEntity(){ }
        public ActorEntity(MetaNetwork metaNetwork) : base(metaNetwork, metaNetwork.Actor, Class)
        {
        }
        public ActorEntity(MetaNetwork metaNetwork, string name) : base(metaNetwork, metaNetwork.Actor, Class, name)
        {
        }

        /// <summary>
        /// Copy the metaNetwork, the two modes networks where the entity exists
        /// </summary>
        /// <param name="entityId"></param>
        public override void CopyMetaNetworkTo(IAgentId entityId)
        {
            MetaNetwork.ActorResource.CopyToFromSource(EntityId, entityId);
            MetaNetwork.ActorOrganization.CopyToFromSource(EntityId, entityId);
            MetaNetwork.ActorTask.CopyToFromSource(EntityId, entityId);
            MetaNetwork.ActorActor.CopyToFromSource(EntityId, entityId);
            MetaNetwork.ActorBelief.CopyToFromSource(EntityId, entityId);
            MetaNetwork.ActorKnowledge.CopyToFromSource(EntityId, entityId);
            MetaNetwork.ActorRole.CopyToFromSource(EntityId, entityId);
        }

        public override void Remove()
        {
            base.Remove();
            MetaNetwork.ActorResource.RemoveSource(EntityId);
            MetaNetwork.ActorOrganization.RemoveSource(EntityId);
            MetaNetwork.ActorTask.RemoveSource(EntityId);
            MetaNetwork.ActorActor.RemoveActor(EntityId);
            MetaNetwork.ActorBelief.RemoveSource(EntityId);
            MetaNetwork.ActorKnowledge.RemoveSource(EntityId);
            MetaNetwork.ActorRole.RemoveSource(EntityId);
        }

        #region Actor * Role Management
        public IEnumerable<IActorRole> Roles => MetaNetwork.ActorRole.EdgesFilteredBySource(EntityId);

        public void AddRole(IAgentId roleId, IAgentId organizationId)
        {
            var actorRole = new ActorRole(EntityId, roleId, organizationId);
            MetaNetwork.ActorRole.Add(actorRole);
        }

        /// <summary>
        ///     List of organizationIds actor is member of
        /// </summary>
        /// <param name="organizationClassId"></param>
        /// <returns></returns>
        public IEnumerable<IAgentId> IsActorOfOrganizationIds(IClassId organizationClassId)
        {
            return MetaNetwork.ActorRole.IsActorOfOrganizationIds(EntityId, organizationClassId);
        }
        /// <summary>
        ///     List of organizationIds actor is member of
        /// </summary>
        /// <param name="organizationClassId"></param>
        /// <returns></returns>
        public bool IsMember(IClassId organizationClassId)
        {
            return MetaNetwork.ActorRole.IsActorOf(EntityId, organizationClassId);
        }

        public IEnumerable<IAgentId> GetOrganizations(IAgentId roleId)
        {
            return MetaNetwork.ActorRole.Edges(EntityId, roleId).Select(x => x.OrganizationId);
        }

        /// <summary>
        ///     Check if actor has a role in a team
        /// </summary>
        public bool HasRoles()
        {
            return MetaNetwork.ActorRole.ExistsSource(EntityId);
        }

        public bool HasRole(IAgentId roleId)
        {
            return MetaNetwork.ActorRole.Exists(EntityId, roleId);
        }

        public bool HasARoleIn(IAgentId roleId, IAgentId organizationId)
        {
            return MetaNetwork.ActorRole.HasARoleIn(EntityId, roleId, organizationId);
        }

        public bool HasARoleIn(IAgentId organizationId)
        {
            return MetaNetwork.ActorRole.HasARoleIn(EntityId, organizationId);
        }

        /// <summary>
        ///     Get the roles of the actor for the organizationId
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public IEnumerable<IActorRole> GetRolesIn(IAgentId organizationId)
        {
            return MetaNetwork.ActorRole.GetRolesIn(EntityId, organizationId);
        }

        #endregion

        #region Actor * Resource management
        public void AddResource(IAgentId resourceId, IResourceUsage resourceUsage, float resourceAllocation = 100)
        {
            var actorResource = new ActorResource(EntityId, resourceId, resourceUsage, resourceAllocation);
            MetaNetwork.ActorResource.Add(actorResource);
        }
        #endregion
    }
}