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
using Symu.DNA.Networks.TwoModesNetworks;
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
        public KnowledgeNetwork Knowledge { get; set; } = new KnowledgeNetwork();

        /// <summary>
        ///     Belief network
        ///     List of Beliefs
        /// </summary>
        public BeliefNetwork Belief { get; } = new BeliefNetwork();
        /// <summary>
        ///     Activities network
        ///     Who (agentId) works on what activities (Kanban)
        /// </summary>
        public TaskNetwork Task { get; set; } = new TaskNetwork();

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
        public AgentAgentNetwork AgentAgent { get; } = new AgentAgentNetwork();

        /// <summary>
        ///     Agent x Group network
        ///     Directory of the groups of the organizationEntity :
        ///     Team, task force, workgroup, circles, community of practices, ...
        /// </summary>
        public AgentOrganizationNetwork AgentOrganization { get; } = new AgentOrganizationNetwork();
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
        ///     Agent x Task network
        ///     Who (agentId) works on what task
        /// </summary>
        public AgentTaskNetwork AgentTask { get; } = new AgentTaskNetwork();

        /// <summary>
        ///     Resource x Task network
        ///     Who (agentId) works on what tasks 
        /// </summary>
        public ResourceTaskNetwork ResourceTask { get; } = new ResourceTaskNetwork();
        /// <summary>
        ///     Task * Knowledge network
        ///     What knowledge is necessary for what Task
        /// </summary>
        public TaskKnowledgeNetwork TaskKnowledge { get; } = new TaskKnowledgeNetwork();

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
            Task.Clear();
            Agent.Clear();
            Event.Clear();
            #endregion

            #region two modes networks
            AgentAgent.Clear();
            AgentOrganization.Clear();
            AgentRole.Clear();
            AgentResource.Clear();
            AgentKnowledge.Clear();
            AgentBelief.Clear();
            AgentTask.Clear();
            ResourceTask.Clear();
            TaskKnowledge.Clear();

            #endregion
        }

        public void RemoveAgent(IAgentId agentId)
        {
            AgentAgent.RemoveAgent(agentId);
            AgentOrganization.RemoveKey(agentId);
            AgentRole.RemoveAgent(agentId);
            AgentResource.RemoveKey(agentId);
            AgentKnowledge.RemoveAgent(agentId);
            AgentTask.RemoveKey(agentId);
            AgentBelief.RemoveAgent(agentId);
            Agent.Remove(agentId);
        }

        #endregion

        #region Methods having crossed impacts on networks

        /// <summary>
        ///     Add an agent to a group
        ///     It doesn't handle roles' impact
        /// </summary>
        /// <param name="agentOrganization"></param>
        /// <param name="groupId"></param>
        public void AddAgentToGroup(IAgentOrganization agentOrganization, IAgentId groupId)
        {
            if (agentOrganization == null)
            {
                throw new ArgumentNullException(nameof(agentOrganization));
            }

            lock (AgentOrganization)
            {
                AgentOrganization.AddKey(groupId);
                AgentOrganization.Add(groupId, agentOrganization);
            }

            AgentResource.AddMemberToGroup(agentOrganization.AgentId, groupId);
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

            if (!AgentOrganization.Exists(groupId))
            {
                return;
            }

            foreach (var agentIdToRemove in AgentOrganization.GetAgents(groupId, agentId.ClassId))
            {
                AgentAgent.DecreaseInteraction(agentId, agentIdToRemove);
            }

            AgentOrganization.RemoveMember(agentId, groupId);
            AgentRole.RemoveMember(agentId, groupId);
            AgentResource.RemoveMemberFromGroup(agentId, groupId);

            // Remove all the groupId activities to the AgentId
            //AgentTask.RemoveMember(agentId, groupId);
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
            Role.CopyTo(network.Role);
            Resource.CopyTo(network.Resource);
            Knowledge.CopyTo(network.Knowledge);
            Belief.CopyTo(network.Belief);
            Task.CopyTo(network.Task);
            Agent.CopyTo(network.Agent);
            Event.CopyTo(network.Event);
            #endregion

            #region two modes networks
            AgentAgent.CopyTo(network.AgentAgent);
            AgentOrganization.CopyTo(network.AgentOrganization);
            AgentRole.CopyTo(network.AgentRole);
            AgentResource.CopyTo(network.AgentResource);
            AgentKnowledge.CopyTo(network.AgentKnowledge);
            AgentBelief.CopyTo(network.AgentBelief);
            AgentTask.CopyTo(network.AgentTask);
            ResourceTask.CopyTo(network.ResourceTask);
            TaskKnowledge.CopyTo(network.TaskKnowledge);
            #endregion

        }
    }
}