#region Licence

// Description: SymuBiz - Symu
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using Symu.Common.Interfaces.Agent;
using Symu.DNA.MatrixNetworks;
using Symu.DNA.Networks.OneModeNetworks;
using Symu.DNA.Networks.TwoModesNetworks.AgentBelief;
using Symu.DNA.Networks.TwoModesNetworks.AgentGroup;
using Symu.DNA.Networks.TwoModesNetworks.AgentKnowledge;
using Symu.DNA.Networks.TwoModesNetworks.AgentResource;
using Symu.DNA.Networks.TwoModesNetworks.AgentRole;
using Symu.DNA.Networks.TwoModesNetworks.Assignment;
using Symu.DNA.Networks.TwoModesNetworks.Interaction;
using Symu.DNA.Networks.TwoModesNetworks.Sphere;

#endregion

namespace Symu.DNA.Networks
{
    /// <summary>
    ///     Referential of networks for social and organizational network analysis
    ///     Used to feed the MatrixMetaNetwork which is used to analyze the networks
    /// </summary>
    public class MetaNetwork
    {
        public MetaNetwork(InteractionSphereModel interactionSphere)
        {
            InteractionSphere = new InteractionSphere(interactionSphere);
        }

        #region One mode networks
        /// <summary>
        ///     Local agents of this environment
        /// </summary>
        public AgentNetwork Agent { get; } = new AgentNetwork();

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
        ///     Knowledge network
        ///     Who (agentId) knows what (Information)
        /// </summary>
        public KnowledgeNetwork Knowledge { get; } = new KnowledgeNetwork();

        /// <summary>
        ///     Belief network
        ///     List of Beliefs
        /// </summary>
        public BeliefNetwork Belief { get; } = new BeliefNetwork();
        /// <summary>
        ///     Activities network
        ///     Who (agentId) works on what activities (Kanban)
        /// </summary>
        public ActivityNetwork Activity { get; } = new ActivityNetwork();

        /// <summary>
        /// occurrences or phenomena that happen
        /// </summary>
        public EventNetwork Event { get; } = new EventNetwork();

        #endregion

        #region Two modes networks 

        /// <summary>
        ///     Agent x Agent network
        ///     Directory of social links between AgentIds, with their interaction type
        ///     Who report/communicate to who
        ///     Sphere of interaction of agents
        /// </summary>
        public InteractionNetwork Interaction { get; } = new InteractionNetwork();

        /// <summary>
        ///     Agent x Group network
        ///     Directory of the groups of the organizationEntity :
        ///     Team, task force, workgroup, circles, community of practices, ...
        /// </summary>
        public AgentGroupNetwork AgentGroup { get; } = new AgentGroupNetwork();
        /// <summary>
        ///     Agent x Role network
        ///     Directory of the roles the agent are playing in the organizationEntity
        /// </summary>
        public AgentRoleNetwork AgentRole { get; } = new AgentRoleNetwork();
        /// <summary>
        ///     Agent x Resource network
        ///     Directory of objects used by the agentIds
        ///     using, working, support
        /// </summary>
        public AgentResourceNetwork AgentResource { get; } = new AgentResourceNetwork();

        /// <summary>
        ///     Agent * Knowledge network
        ///     Who (agentId) knows what (Information)
        /// </summary>
        public AgentKnowledgeNetwork AgentKnowledge { get; } = new AgentKnowledgeNetwork();
        /// <summary>
        ///     Agent * belief network
        ///     Who (agentId) believes what (Information)
        /// </summary>
        public AgentBeliefNetwork AgentBelief { get; } = new AgentBeliefNetwork();

        /// <summary>
        ///     Agent x Activity network
        ///     Who (agentId) works on what activities (Kanban)
        /// </summary>
        public AssignmentNetwork Assignment { get; } = new AssignmentNetwork();

        /// <summary>
        ///     Agent x Agent network
        ///     Derived Parameters from others networks.
        ///     these parameters are use indirectly to change agent behavior.
        /// </summary>
        public InteractionSphere InteractionSphere { get; }
        #endregion

        #region Initialize & remove Agents

        public void Clear()
        {
            #region One mode networks
            Role.Clear();
            Resource.Clear();
            Knowledge.Clear();
            Belief.Clear();
            Activity.Clear();
            Agent.Clear();
            Event.Clear();
            #endregion

            #region two modes networks
            Interaction.Clear();
            AgentGroup.Clear();
            AgentRole.Clear();
            AgentResource.Clear();
            AgentKnowledge.Clear();
            AgentBelief.Clear();
            Assignment.Clear();
            #endregion
        }

        public void RemoveAgent(IAgentId agentId)
        {
            Interaction.RemoveAgent(agentId);
            AgentGroup.RemoveAgent(agentId);
            AgentRole.RemoveAgent(agentId);
            AgentResource.RemoveAgent(agentId);
            AgentKnowledge.RemoveAgent(agentId);
            Assignment.RemoveAgent(agentId);
            AgentBelief.RemoveAgent(agentId);
            Agent.Remove(agentId);
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
            AgentRole.RemoveMember(agentId, groupId);
            AgentResource.RemoveMemberFromGroup(agentId, groupId);

            // Remove all the groupId activities to the AgentId
            Assignment.RemoveMember(agentId, groupId);
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
    }
}