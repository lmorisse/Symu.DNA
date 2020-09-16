#region Licence

// Description: SymuBiz - Symu
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using Symu.Common.Interfaces.Agent;
using Symu.DNA.GraphNetworks.OneModeNetworks;
using Symu.DNA.GraphNetworks.TwoModesNetworks;
using Symu.DNA.GraphNetworks.TwoModesNetworks.Sphere;
using Symu.DNA.MatrixNetworks;

#endregion

namespace Symu.DNA.GraphNetworks
{
    /// <summary>
    ///     Referential of networks for social and organizational network analysis
    ///     Used to feed the MatrixMetaNetwork which is used to analyze the networks
    /// </summary>
    public class MetaNetwork
    {
        //todo manage a list of one mode and two modes network, especially used in Clear and CopyTo methods
        public MetaNetwork(InteractionSphereModel interactionSphere)
        {
            InteractionSphere = new InteractionSphere(interactionSphere);
        }

        #region One mode networks
        /// <summary>
        ///     Local actors of this environment
        /// </summary>
        public ActorNetwork Actor { get; } = new ActorNetwork();

        /// <summary>
        ///     Directory of the roles the actor are playing in the organizationEntity
        /// </summary>
        public RoleNetwork Role { get; } = new RoleNetwork();

        /// <summary>
        ///     Directory of objects used by the actorIds
        ///     using, working, support
        /// </summary>
        public ResourceNetwork Resource { get; } = new ResourceNetwork();
        /// <summary>
        ///     Knowledge network
        ///     Who (actorId) knows what (Information)
        /// </summary>
        public KnowledgeNetwork Knowledge { get; set; } = new KnowledgeNetwork();

        /// <summary>
        ///     Belief network
        ///     List of Beliefs
        /// </summary>
        public BeliefNetwork Belief { get; } = new BeliefNetwork();
        /// <summary>
        ///     Organization network
        ///     List of organizations
        /// </summary>
        public OrganizationNetwork Organization { get; } = new OrganizationNetwork();
        /// <summary>
        ///     Activities network
        ///     Who (actorId) works on what activities (Kanban)
        /// </summary>
        public TaskNetwork Task { get; set; } = new TaskNetwork();

        /// <summary>
        /// occurrences or phenomena that happen
        /// </summary>
        public EventNetwork Event { get; } = new EventNetwork();

        #endregion

        #region Two modes networks 

        /// <summary>
        ///     Actor x Actor network
        ///     Directory of social links between ActorIds, with their interaction type
        ///     Who report/communicate to who
        ///     Sphere of interaction of actors
        /// </summary>
        public ActorActorNetwork ActorActor { get; } = new ActorActorNetwork();

        /// <summary>
        ///     Actor x Group network
        ///     Directory of the groups of the organizationEntity :
        ///     Team, task force, workgroup, circles, community of practices, ...
        /// </summary>
        public OrganizationActorNetwork OrganizationActor { get; } = new OrganizationActorNetwork();
        /// <summary>
        ///     Actor x Role network
        ///     Directory of the roles the actor are playing in the organizationEntity
        /// </summary>
        public ActorRoleNetwork ActorRole { get; } = new ActorRoleNetwork();
        /// <summary>
        ///     Actor x Resource network
        ///     Directory of objects used by the actorIds
        ///     using, working, support
        /// </summary>
        public ActorResourceNetwork ActorResource { get; } = new ActorResourceNetwork();

        /// <summary>
        ///     Actor * Knowledge network
        ///     Who (actorId) knows what (Information)
        /// </summary>
        public ActorKnowledgeNetwork ActorKnowledge { get; } = new ActorKnowledgeNetwork();
        /// <summary>
        ///     Actor * belief network
        ///     Who (actorId) believes what (Information)
        /// </summary>
        public ActorBeliefNetwork ActorBelief { get; } = new ActorBeliefNetwork();

        /// <summary>
        ///     Actor x Task network
        ///     Who (actorId) works on what task
        /// </summary>
        public ActorTaskNetwork ActorTask { get; } = new ActorTaskNetwork();

        /// <summary>
        ///     Resource x Task network
        ///     Who (actorId) works on what tasks 
        /// </summary>
        public ResourceTaskNetwork ResourceTask { get; } = new ResourceTaskNetwork();
        /// <summary>
        ///     Task * Knowledge network
        ///     What knowledge is necessary for what Task
        /// </summary>
        public TaskKnowledgeNetwork TaskKnowledge { get; } = new TaskKnowledgeNetwork();
        /// <summary>
        ///     Organization * Resource network
        ///     Which organization uses what resource
        /// </summary>
        public OrganizationResourceNetwork OrganizationResource { get; } = new OrganizationResourceNetwork();
        /// <summary>
        ///     Resource * Resource network
        ///     Which Resource uses what resource
        /// </summary>
        public ResourceResourceNetwork ResourceResource { get; } = new ResourceResourceNetwork();

        /// <summary>
        ///     Actor x Actor network
        ///     Derived Parameters from others networks.
        ///     these parameters are use indirectly to change actor behavior.
        /// </summary>
        public InteractionSphere InteractionSphere { get; }
        #endregion

        #region Initialize & remove Actors

        public void Clear()
        {
            #region One mode networks
            Role.Clear();
            Resource.Clear();
            Knowledge.Clear();
            Belief.Clear();
            Task.Clear();
            Actor.Clear();
            Event.Clear();
            Organization.Clear();
            #endregion

            #region two modes networks
            ActorActor.Clear();
            OrganizationActor.Clear();
            ActorRole.Clear();
            ActorResource.Clear();
            ActorKnowledge.Clear();
            ActorBelief.Clear();
            ActorTask.Clear();
            ResourceTask.Clear();
            TaskKnowledge.Clear();
            OrganizationResource.Clear();
            ResourceResource.Clear();

            #endregion
        }



        #endregion

        #region Methods having crossed impacts on networks

        /// <summary>
        ///     Add an actor to an organization, with its all the resources of the organization
        ///     It doesn't handle roles' impact
        /// </summary>
        /// <param name="actorOrganization"></param>
        /// <param name="organizationId"></param>
        //public void AddActorToOrganization(IActorOrganization actorOrganization, IAgentId organizationId)
        //{
        //    if (actorOrganization == null)
        //    {
        //        throw new ArgumentNullException(nameof(actorOrganization));
        //    }

        //    ActorOrganization.Add(organizationId, actorOrganization);
        //    // Actor * Resource
        //    var organizationResources = OrganizationResource.GetValues(organizationId.Id);
        //    foreach (var organizationResource in organizationResources)
        //    {
        //        ActorResource.Add(actorOrganization.ActorId, organizationResource.Clone());
        //    }
        //}

        /// <summary>
        ///     Remove an actor to an organization, and all the resources of the organization
        ///     It doesn't handle roles
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="organizationId"></param>
        public void RemoveActorFromOrganization(IAgentId actorId, IAgentId organizationId)
        {
            if (actorId == null)
            {
                throw new ArgumentNullException(nameof(actorId));
            }

            if (!OrganizationActor.Exists(organizationId))
            {
                return;
            }

            foreach (var actorIdToRemove in OrganizationActor.GetActorIds(organizationId, actorId.ClassId))
            {
                ActorActor.DecreaseInteraction(actorId, actorIdToRemove);
            }

            OrganizationActor.RemoveActor(actorId, organizationId);
            ActorRole.RemoveActor(actorId, organizationId);
            var resourceIds = OrganizationResource.GetResourceIds(organizationId.Id);
            ActorResource.Remove(actorId, resourceIds);
        }

        #endregion

        /// <summary>
        /// Transform the MetaNetwork into a MatrixMetaNetwork
        /// </summary>
        /// <returns></returns>
        public MatrixMetaNetwork ToMatrix()
        {
            return new MatrixMetaNetwork(this);
        }

        public void  CopyTo(MetaNetwork network)
        {

            #region One mode networks
            Role.CopyTo(this, network.Role);
            Resource.CopyTo(this, network.Resource);
            Knowledge.CopyTo(this, network.Knowledge);
            Belief.CopyTo(this, network.Belief);
            Task.CopyTo(this, network.Task);
            Actor.CopyTo(this, network.Actor);
            Event.CopyTo(this, network.Event);
            Organization.CopyTo(this, network.Organization);
            #endregion

            #region two modes networks
            ActorActor.CopyTo(network.ActorActor);
            OrganizationActor.CopyTo(network.OrganizationActor);
            ActorRole.CopyTo(network.ActorRole);
            ActorResource.CopyTo(network.ActorResource);
            ActorKnowledge.CopyTo(network.ActorKnowledge);
            ActorBelief.CopyTo(network.ActorBelief);
            ActorTask.CopyTo(network.ActorTask);
            ResourceTask.CopyTo(network.ResourceTask);
            TaskKnowledge.CopyTo(network.TaskKnowledge);
            OrganizationResource.CopyTo(network.OrganizationResource);
            ResourceResource.CopyTo(network.ResourceResource);
            #endregion

        }
    }
}