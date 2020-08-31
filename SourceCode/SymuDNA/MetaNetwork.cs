﻿#region Licence

// Description: SymuBiz - Symu
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using Symu.Common.Interfaces.Agent;
using Symu.DNA.Activities;
using Symu.DNA.Agent;
using Symu.DNA.Beliefs;
using Symu.DNA.Groups;
using Symu.DNA.Knowledges;
using Symu.DNA.Resources;
using Symu.DNA.Roles;
using Symu.DNA.TwoModesNetworks.Interactions;
using Symu.DNA.TwoModesNetworks.Sphere;
using Symu.Repository.Networks.Events;

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
        }


        /// <summary>
        ///     Local agents of this environment
        /// </summary>
        public AgentNetwork Agents { get; } = new AgentNetwork();

        /// <summary>
        ///     Directory of social links between AgentIds, with their interaction type
        ///     Who report/communicate to who
        ///     Sphere of interaction of agents
        /// </summary>
        public InteractionNetwork Interactions { get; } = new InteractionNetwork();

        /// <summary>
        ///     Directory of the groups of the organizationEntity :
        ///     Team, task force, workgroup, circles, community of practices, ...
        /// </summary>
        public GroupNetwork Groups { get; } = new GroupNetwork();

        /// <summary>
        ///     Directory of the roles the agent are playing in the organizationEntity
        /// </summary>
        public RoleNetwork Roles { get; } = new RoleNetwork();

        /// <summary>
        ///     Directory of objects used by the agentIds
        ///     using, working, support
        /// </summary>
        public ResourceNetwork Resources { get; } = new ResourceNetwork();

        /// <summary>
        ///     Knowledge network
        ///     Who (agentId) knows what (Information)
        /// </summary>
        public KnowledgeNetwork Knowledge { get; } = new KnowledgeNetwork();

        /// <summary>
        ///     Belief network
        ///     Who (agentId) believes what (Information)
        /// </summary>
        public BeliefNetwork Beliefs { get; } = new BeliefNetwork();

        /// <summary>
        ///     Kanban activities network
        ///     Who (agentId) works on what activities (Kanban)
        /// </summary>
        public ActivityNetwork Activities { get; } = new ActivityNetwork();

        /// <summary>
        /// occurrences or phenomena that happen
        /// </summary>
        public EventNetwork Events { get; } = new EventNetwork();

        /// <summary>
        ///     Derived Parameters from others networks.
        ///     these parameters are use indirectly to change agent behavior.
        /// </summary>
        public InteractionSphere InteractionSphere { get; }

        #region Initialize & remove Agents

        public void Clear()
        {
            Interactions.Clear();
            Groups.Clear();
            Roles.Clear();
            Resources.Clear();
            Knowledge.Clear();
            Beliefs.Clear();
            Activities.Clear(); 
            Agents.Clear();
            Events.Clear();

            //Enculturation.Clear();
            //Influences.Clear();
        }

        public void RemoveAgent(IAgentId agentId)
        {
            Interactions.RemoveAgent(agentId);
            Groups.RemoveAgent(agentId);
            Roles.RemoveAgent(agentId);
            Resources.RemoveAgent(agentId);
            Knowledge.RemoveAgent(agentId);
            Activities.RemoveAgent(agentId);
            Beliefs.RemoveAgent(agentId);
            Agents.RemoveAgent(agentId);
            //Enculturation.RemoveAgent(agentId);
            //Influences.RemoveAgent(agentId);
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

            lock (Groups)
            {
                Groups.AddGroup(groupId);
                Groups.AddAgent(agentGroup, groupId);
            }

            Resources.AddMemberToGroup(agentGroup.AgentId, groupId);
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

            if (!Groups.Exists(groupId))
            {
                return;
            }

            foreach (var agentIdToRemove in Groups.GetAgents(groupId, agentId.ClassId))
            {
                Interactions.DecreaseInteraction(agentId, agentIdToRemove);
            }

            Groups.RemoveMember(agentId, groupId);
            Roles.RemoveMember(agentId, groupId);
            Resources.RemoveMemberFromGroup(agentId, groupId);

            // Remove all the groupId activities to the AgentId
            Activities.RemoveMember(agentId, groupId);
        }

        #endregion
    }
}