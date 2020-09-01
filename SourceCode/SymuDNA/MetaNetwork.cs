#region Licence

// Description: SymuBiz - Symu
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using Symu.Common.Interfaces.Agent;
using Symu.DNA.Activities;
using Symu.DNA.OneModeNetworks.Agent;
using Symu.DNA.OneModeNetworks.Belief;
using Symu.DNA.OneModeNetworks.Event;
using Symu.DNA.OneModeNetworks.Knowledge;
using Symu.DNA.OneModeNetworks.Resource;
using Symu.DNA.Roles;
using Symu.DNA.TwoModesNetworks.AgentBelief;
using Symu.DNA.TwoModesNetworks.AgentGroup;
using Symu.DNA.TwoModesNetworks.AgentKnowledge;
using Symu.DNA.TwoModesNetworks.AgentResource;
using Symu.DNA.TwoModesNetworks.Interaction;
using Symu.DNA.TwoModesNetworks.Sphere;

#endregion

namespace Symu.DNA
{
    /// <summary>
    ///     MetaNetwork: referential of networks for social and organizational network analysis
    /// </summary>
    public class MetaNetwork
    {
        public MetaNetwork(InteractionSphereModel interactionSphere)
        {
            InteractionSphere = new InteractionSphere(interactionSphere);
            AgentBelief = new AgentBeliefNetwork();
        }


        /// <summary>
        ///     Local agents of this environment
        /// </summary>
        public AgentNetwork Agent { get; } = new AgentNetwork();

        /// <summary>
        ///     Directory of social links between AgentIds, with their interaction type
        ///     Who report/communicate to who
        ///     Sphere of interaction of agents
        /// </summary>
        public InteractionNetwork Interaction { get; } = new InteractionNetwork();

        /// <summary>
        ///     Directory of the groups of the organizationEntity :
        ///     Team, task force, workgroup, circles, community of practices, ...
        /// </summary>
        public AgentGroupNetwork AgentGroup { get; } = new AgentGroupNetwork();

        /// <summary>
        ///     Directory of the roles the agent are playing in the organizationEntity
        /// </summary>
        public RoleNetwork Role { get; } = new RoleNetwork();

        /// <summary>
        ///     Directory of objects used by the agentIds
        ///     using, working, support
        /// </summary>
        public ResourceNetwork Resource { get; } = new ResourceNetwork();
        /// <summary>
        ///     Directory of objects used by the agentIds
        ///     using, working, support
        /// </summary>
        public AgentResourceNetwork AgentResource { get; } = new AgentResourceNetwork();

        /// <summary>
        ///     Knowledge network
        ///     Who (agentId) knows what (Information)
        /// </summary>
        public KnowledgeNetwork Knowledge { get; } = new KnowledgeNetwork();
        /// <summary>
        ///     Knowledge network
        ///     Who (agentId) knows what (Information)
        /// </summary>
        public AgentKnowledgeNetwork AgentKnowledge { get; } = new AgentKnowledgeNetwork();

        /// <summary>
        ///     Belief network
        ///     List of Beliefs
        /// </summary>
        public BeliefNetwork Belief { get; } = new BeliefNetwork();
        /// <summary>
        ///     Agent * belief network
        ///     Who (agentId) believes what (Information)
        /// </summary>
        public AgentBeliefNetwork AgentBelief { get; } 

        /// <summary>
        ///     Kanban activities network
        ///     Who (agentId) works on what activities (Kanban)
        /// </summary>
        public ActivityNetwork Activities { get; } = new ActivityNetwork();

        /// <summary>
        /// occurrences or phenomena that happen
        /// </summary>
        public EventNetwork Event { get; } = new EventNetwork();

        /// <summary>
        ///     Derived Parameters from others networks.
        ///     these parameters are use indirectly to change agent behavior.
        /// </summary>
        public InteractionSphere InteractionSphere { get; }

        #region Initialize & remove Agents

        public void Clear()
        {
            Interaction.Clear();
            AgentGroup.Clear();
            Role.Clear();
            Resource.Clear();
            AgentResource.Clear();
            Knowledge.Clear();
            AgentKnowledge.Clear();
            Belief.Clear();
            AgentBelief.Clear();
            Activities.Clear(); 
            Agent.Clear();
            Event.Clear();
        }

        public void RemoveAgent(IAgentId agentId)
        {
            Interaction.RemoveAgent(agentId);
            AgentGroup.RemoveAgent(agentId);
            Role.RemoveAgent(agentId);
            AgentResource.RemoveAgent(agentId);
            AgentKnowledge.RemoveAgent(agentId);
            Activities.RemoveAgent(agentId);
            AgentBelief.RemoveAgent(agentId);
            Agent.RemoveAgent(agentId);
        }

        #endregion

        #region Methods having crossed impacts on networks

        /// <summary>
        ///     Add an agent to a group
        ///     It doesn't handle roles' impact
        /// </summary>
        /// <param name="agentGroup"></param>
        /// <param name="groupId"></param>
        public void AddAgentToGroup(IAgentGroup agentGroup, IAgentId groupId)
        {
            if (agentGroup == null)
            {
                throw new ArgumentNullException(nameof(agentGroup));
            }

            lock (AgentGroup)
            {
                AgentGroup.AddGroup(groupId);
                AgentGroup.AddAgent(agentGroup, groupId);
            }

            AgentResource.AddMemberToGroup(agentGroup.AgentId, groupId);
        }

        /// <summary>
        ///     Remove an agent to a group
        ///     It doesn't handle roles
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="groupId"></param>
        public void RemoveAgentFromGroup(IAgentId agentId, IAgentId groupId)
        {
            if (agentId == null)
            {
                throw new ArgumentNullException(nameof(agentId));
            }

            if (!AgentGroup.Exists(groupId))
            {
                return;
            }

            foreach (var agentIdToRemove in AgentGroup.GetAgents(groupId, agentId.ClassId))
            {
                Interaction.DecreaseInteraction(agentId, agentIdToRemove);
            }

            AgentGroup.RemoveMember(agentId, groupId);
            Role.RemoveMember(agentId, groupId);
            AgentResource.RemoveMemberFromGroup(agentId, groupId);

            // Remove all the groupId activities to the AgentId
            Activities.RemoveMember(agentId, groupId);
        }

        #endregion
    }
}