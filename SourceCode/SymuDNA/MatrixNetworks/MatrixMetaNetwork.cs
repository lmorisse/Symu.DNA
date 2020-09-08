#region Licence

// Description: SymuBiz - Symu
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using MathNet.Numerics.LinearAlgebra;
using Symu.Common.Interfaces.Agent;
using Symu.Common.Interfaces.Entity;
using Symu.DNA.Networks;

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
            Task = new VectorNetwork<IId>(metaNetwork.Task.ToVector());
            Event = new VectorNetwork<IId>(metaNetwork.Event.ToVector());
            Organization = new VectorNetwork<IAgentId>(metaNetwork.AgentOrganization.ToVector());

            #endregion

            #region Two modes networks

            AgentBelief = metaNetwork.AgentBelief.ToMatrix(Agent, Belief);
            AgentOrganization = metaNetwork.AgentOrganization.ToMatrix(Agent, Organization);
            AgentKnowledge = metaNetwork.AgentKnowledge.ToMatrix(Agent, Knowledge);
            AgentResource = metaNetwork.AgentResource.ToMatrix(Agent, Resource);
            AgentRole = metaNetwork.AgentRole.ToMatrix(Agent, Role);
            AgentTask = metaNetwork.AgentTask.ToMatrix(Agent, Task);

            #endregion
        }

        #region One mode networks
        /// <summary>
        ///     List of the agents of the meta network, called Actor network
        ///     An agent is an individual decision makers
        /// </summary>
        public VectorNetwork<IAgentId> Agent { get; set; }
        /// <summary>
        ///     List of the organizations of this network:
        ///     An organization is collectives of people that try to reach a common goal
        /// </summary>
        public VectorNetwork<IAgentId> Organization { get; set; }

        /// <summary>
        ///     List of the roles of the meta network:
        ///     A role describe functions of agents
        /// </summary>
        public VectorNetwork<IId> Role { get; set; }

        /// <summary>
        ///     List of the resources of the meta network:
        ///     Resources are products, materials, or goods that are necessary to perform Tasks and Events
        /// </summary>
        public VectorNetwork<IId> Resource { get; set; }
        /// <summary>
        ///     List of the knowledge of the meta network:
        ///     A knowledge is cognitive capabilities and skills
        /// </summary>
        public VectorNetwork<IId> Knowledge { get; set; }

        /// <summary>
        ///     List of the beliefs of the meta network:
        ///     Beliefs are any form of religion or other persuasion.
        /// </summary>
        public VectorNetwork<IId> Belief { get; set; }
        /// <summary>
        ///     List of the tasks of the meta network:
        ///     A Task is a well defined procedures or goals of an organization, scheduled or planned activities
        /// </summary>
        public VectorNetwork<IId> Task { get; set; }

        /// <summary>
        ///     List of the Events of the meta network:
        ///     An Event is occurrences or phenomena that happen
        /// </summary>
        public VectorNetwork<IId> Event { get; set; } 

        #endregion

        #region Two modes networks 

        /// <summary>
        ///     Agent x Agent network, called interaction network, social network
        ///     network of social links between agents, with their interaction type
        ///     Who interacts to / knows who
        /// </summary>
        public Matrix<float> AgentAgent { get; set; }

        /// <summary>
        ///     Agent x Organization network, called work network, employment network
        ///     Who works where
        /// </summary>
        public Matrix<float> AgentOrganization { get; set; }
        /// <summary>
        ///     Agent x Role network
        ///     Who has what functions
        /// </summary>
        public Matrix<float> AgentRole { get; set; }
        /// <summary>
        ///     Agent x Resource network, called capabilities network
        ///     Who has what resource
        /// </summary>
        public Matrix<float> AgentResource { get; set; }
        /// <summary>
        ///     Agent * Knowledge network, called knowledge network
        ///     Who (agent) knows what (Knowledge)
        /// </summary>
        public Matrix<float> AgentKnowledge { get; set; } 
        /// <summary>
        ///     Agent * belief network
        ///     Who (agent) believes what (Belief)
        /// </summary>
        public Matrix<float> AgentBelief { get; set; } 

        /// <summary>
        ///     Agent x Task network, called assignment network
        ///     Who (agent) does what (Tasks)
        /// </summary>
        public Matrix<float> AgentTask { get; set; } 
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
                    return AgentAgent;
                case NetworkType.AgentxBelief:
                    return AgentBelief;
                case NetworkType.AgentxGroup:
                    return AgentOrganization;
                case NetworkType.AgentxKnowledge:
                    return AgentKnowledge;
                case NetworkType.AgentxResource:
                    return AgentResource;
                case NetworkType.AgentxRole:
                    return AgentResource;
                case NetworkType.Assignment:
                    return AgentTask;
                default:
                    throw new ArgumentOutOfRangeException(nameof(networkType), networkType, null);
            }
        }
    }
}