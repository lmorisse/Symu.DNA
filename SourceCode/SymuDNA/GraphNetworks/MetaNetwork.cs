#region Licence

// Description: SymuBiz - SymuDNA
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System.Collections.Generic;
using Symu.DNA.Entities;
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

        public List<OneModeNetwork> OneModeNetworks { get; } = new List<OneModeNetwork>();
        public string Version { get; set; } = "reference";

        public MetaNetwork(InteractionSphereModel interactionSphere)
        {
            InteractionSphere = new InteractionSphere(interactionSphere);
            OneModeNetworks.Add(Actor);
            OneModeNetworks.Add(Role);
            OneModeNetworks.Add(Resource);
            OneModeNetworks.Add(Knowledge);
            OneModeNetworks.Add(Belief);
            OneModeNetworks.Add(Organization);
            OneModeNetworks.Add(Task);
            OneModeNetworks.Add(Event);
        }

        public MetaNetwork() : this(new InteractionSphereModel())
        {
        }

        #region Initialize & remove Actors

        public void Clear()
        {
            foreach (var network in OneModeNetworks)
            {
                network.Clear();
            }

            #region two modes networks

            ActorActor.Clear();
            ActorOrganization.Clear();
            ActorRole.Clear();
            ActorResource.Clear();
            ActorKnowledge.Clear();
            ActorBelief.Clear();
            ActorTask.Clear();
            ResourceTask.Clear();
            TaskKnowledge.Clear();
            OrganizationResource.Clear();
            ResourceResource.Clear();
            ResourceKnowledge.Clear();

            #endregion
        }

        #endregion

        /// <summary>
        ///     Transform the MetaNetwork into a MatrixMetaNetwork
        /// </summary>
        /// <returns></returns>
        public MatrixMetaNetwork ToMatrix()
        {
            return new MatrixMetaNetwork(this);
        }

        public MetaNetwork Clone()
        {
            var clone = new MetaNetwork {Version = "clone"};
            for (var index = 0; index < OneModeNetworks.Count; index++)
            {
                var oneModeNetwork = OneModeNetworks[index];
                var copyTo = clone.OneModeNetworks[index];
                oneModeNetwork.CopyTo(clone, copyTo);
            }

            #region two modes networks

            ActorActor.CopyTo(clone.ActorActor);
            ActorOrganization.CopyTo(clone.ActorOrganization);
            ActorRole.CopyTo(clone.ActorRole);
            ActorResource.CopyTo(clone.ActorResource);
            ActorKnowledge.CopyTo(clone.ActorKnowledge);
            ActorBelief.CopyTo(clone.ActorBelief);
            ActorTask.CopyTo(clone.ActorTask);
            ResourceTask.CopyTo(clone.ResourceTask);
            TaskKnowledge.CopyTo(clone.TaskKnowledge);
            OrganizationResource.CopyTo(clone.OrganizationResource);
            ResourceResource.CopyTo(clone.ResourceResource);
            ResourceKnowledge.CopyTo(clone.ResourceKnowledge);

            #endregion

            //todo temporary
            InteractionSphere.Model.CopyTo(clone.InteractionSphere.Model);

            return clone;
        }

        #region One mode networks

        /// <summary>
        ///     Local actors of this environment
        /// </summary>
        public OneModeNetwork Actor { get; } = new OneModeNetwork();

        /// <summary>
        ///     Directory of the roles the actor are playing in the organizationEntity
        /// </summary>
        public OneModeNetwork Role { get; } = new OneModeNetwork();

        /// <summary>
        ///     Directory of objects used by the actorIds
        ///     using, working, support
        /// </summary>
        public OneModeNetwork Resource { get; } = new OneModeNetwork();

        /// <summary>
        ///     Knowledge network
        ///     Who (actorId) knows what (Information)
        /// </summary>
        public OneModeNetwork Knowledge { get; set; } = new OneModeNetwork();

        /// <summary>
        ///     Belief network
        ///     List of Beliefs
        /// </summary>
        public OneModeNetwork Belief { get; } = new OneModeNetwork();

        /// <summary>
        ///     Organization network
        ///     List of organizations
        /// </summary>
        public OneModeNetwork Organization { get; } = new OneModeNetwork();

        /// <summary>
        ///     Activities network
        ///     Who (actorId) works on what activities (Kanban)
        /// </summary>
        public OneModeNetwork Task { get; set; } = new OneModeNetwork();

        /// <summary>
        ///     occurrences or phenomena that happen
        /// </summary>
        public OneModeNetwork Event { get; } = new OneModeNetwork();

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
        public ActorOrganizationNetwork ActorOrganization { get; } = new ActorOrganizationNetwork();

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
        ///     Resource * Knowledge network
        ///     Which Resource stores what knowledge
        /// </summary>
        public ResourceKnowledgeNetwork ResourceKnowledge { get; } = new ResourceKnowledgeNetwork();

        /// <summary>
        ///     Actor x Actor network
        ///     Derived Parameters from others networks.
        ///     these parameters are use indirectly to change actor behavior.
        /// </summary>
        public InteractionSphere InteractionSphere { get; }

        #endregion

        public void NormalizeWeights()
        {
            ActorActor.NormalizeWeights();
            ActorOrganization.NormalizeWeights();
            ActorRole.NormalizeWeights();
            ActorResource.NormalizeWeights();
            ActorKnowledge.NormalizeWeights();
            ActorBelief.NormalizeWeights();
            ActorTask.NormalizeWeights();
            ResourceTask.NormalizeWeights();
            TaskKnowledge.NormalizeWeights();
            OrganizationResource.NormalizeWeights();
            ResourceResource.NormalizeWeights();
            ResourceKnowledge.NormalizeWeights();
        }
    }
}