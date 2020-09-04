﻿#region Licence

// Description: SymuBiz - Symu
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;
using Symu.Common.Interfaces.Agent;
using Symu.Common.Interfaces.Entity;
using Symu.DNA.MatrixNetworks.OneModeNetworks;
using Symu.DNA.Networks;
using Symu.DNA.Networks.TwoModesNetworks.Sphere;

#endregion

namespace Symu.DNA.MatrixNetworks
{
    /// <summary>
    ///     Referential of networks for social and organizational network analysis
    ///     Feed by the MetaNetwork or directly
    /// </summary>
    public class MatrixMetaNetwork
    {
        public MatrixMetaNetwork(){}
        public MatrixMetaNetwork(MetaNetwork metaNetwork)
        {
            #region One mode networks

            Agent = new VectorNetwork<IAgentId>(metaNetwork.Agent.ToVector());
            Role = new VectorNetwork<IId>(metaNetwork.Role.ToVector());
            Resource = new VectorNetwork<IId>(metaNetwork.Resource.ToVector());
            Knowledge = new VectorNetwork<IId>(metaNetwork.Knowledge.ToVector());
            Belief = new VectorNetwork<IId>(metaNetwork.Belief.ToVector());
            Activity = new VectorNetwork<IId>(metaNetwork.Activity.ToVector());
            Event = new VectorNetwork<IId>(metaNetwork.Event.ToVector());
            Group = new VectorNetwork<IAgentId>(metaNetwork.AgentGroup.ToVector());

            #endregion

            #region Two modes networks

            AgentBelief = metaNetwork.AgentBelief.ToMatrix(Agent, Belief);
            AgentGroup = metaNetwork.AgentGroup.ToMatrix(Agent, Group);
            AgentKnowledge = metaNetwork.AgentKnowledge.ToMatrix(Agent, Knowledge);
            AgentResource = metaNetwork.AgentResource.ToMatrix(Agent, Resource);
            AgentRole = metaNetwork.AgentRole.ToMatrix(Agent, Role);
            Assignment = metaNetwork.Assignment.ToMatrix(Agent, Activity);

            #endregion
        }

        #region One mode networks
        /// <summary>
        ///     List of the agents of this network
        /// </summary>
        public VectorNetwork<IAgentId> Agent { get; set; }
        /// <summary>
        ///     List of the groups of this network
        /// </summary>
        public VectorNetwork<IAgentId> Group { get; set; }

        /// <summary>
        ///     Directory of the roles the agent are playing in the organizationEntity
        /// </summary>
        public VectorNetwork<IId> Role { get; set; } 

        /// <summary>
        ///     Directory of objects used by the agentIds
        ///     using, working, support
        /// </summary>
        public VectorNetwork<IId> Resource { get; set; } 
        /// <summary>
        ///     Knowledge network
        ///     Who (agentId) knows what (Information)
        /// </summary>
        public VectorNetwork<IId> Knowledge { get; set; } 

        /// <summary>
        ///     Belief network
        ///     List of Beliefs
        /// </summary>
        public VectorNetwork<IId> Belief { get; set; } 
        /// <summary>
        ///     Activities network
        ///     Who (agentId) works on what activities (Kanban)
        /// </summary>
        public VectorNetwork<IId> Activity { get; set; } 

        /// <summary>
        /// occurrences or phenomena that happen
        /// </summary>
        public VectorNetwork<IId> Event { get; set; } 

        #endregion

        #region Two modes networks 

        /// <summary>
        ///     Agent x Agent network
        ///     Directory of social links between AgentIds, with their interaction type
        ///     Who report/communicate to who
        ///     Sphere of interaction of agents
        /// </summary>
        public Matrix<float> Interaction { get; set; } 

        /// <summary>
        ///     Agent x Group network
        ///     Directory of the groups of the organizationEntity :
        ///     Team, task force, workgroup, circles, community of practices, ...
        /// </summary>
        public Matrix<float> AgentGroup { get; set; }
        /// <summary>
        ///     Agent x Role network
        ///     Directory of the roles the agent are playing in the organizationEntity
        /// </summary>
        public Matrix<float> AgentRole { get; set; }
        /// <summary>
        ///     Agent x Resource network
        ///     Directory of objects used by the agentIds
        ///     using, working, support
        /// </summary>
        public Matrix<float> AgentResource { get; set; } 

        /// <summary>
        ///     Agent * Knowledge network
        ///     Who (agentId) knows what (Information)
        /// </summary>
        public Matrix<float> AgentKnowledge { get; set; } 
        /// <summary>
        ///     Agent * belief network
        ///     Who (agentId) believes what (Information)
        /// </summary>
        public Matrix<float> AgentBelief { get; set; } 

        /// <summary>
        ///     Agent x Activity network
        ///     Who (agentId) works on what activities (Kanban)
        /// </summary>
        public Matrix<float> Assignment { get; set; } 

        /// <summary>
        ///     Agent x Agent network
        ///     Derived Parameters from others networks.
        ///     these parameters are use indirectly to change agent behavior.
        /// </summary>
        /// todo 
        public InteractionSphere InteractionSphere { get; set; }
        #endregion

        /// <summary>
        /// Get the right Matrix from the networkType
        /// </summary>
        /// <param name="networkType"></param>
        /// <returns></returns>
        public Matrix<float> Get(NetworkType networkType)
        {
            switch (networkType)
            {
                case NetworkType.Interaction:
                    return Interaction;
                case NetworkType.AgentxBelief:
                    return AgentBelief;
                case NetworkType.AgentxGroup:
                    return AgentGroup;
                case NetworkType.AgentxKnowledge:
                    return AgentKnowledge;
                case NetworkType.AgentxResource:
                    return AgentResource;
                case NetworkType.AgentxRole:
                    return AgentResource;
                case NetworkType.Assignment:
                    return Assignment;
                default:
                    throw new ArgumentOutOfRangeException(nameof(networkType), networkType, null);
            }
        }
    }
}