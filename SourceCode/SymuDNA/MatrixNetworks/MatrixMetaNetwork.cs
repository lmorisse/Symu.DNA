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
using Symu.DNA.GraphNetworks;

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

            Actor = new VectorNetwork<IAgentId>(metaNetwork.Actor.ToVector());
            Role = new VectorNetwork<IId>(metaNetwork.Role.ToVector());
            Resource = new VectorNetwork<IAgentId>(metaNetwork.Resource.ToVector());
            Knowledge = new VectorNetwork<IId>(metaNetwork.Knowledge.ToVector());
            Belief = new VectorNetwork<IId>(metaNetwork.Belief.ToVector());
            Task = new VectorNetwork<IId>(metaNetwork.Task.ToVector());
            Event = new VectorNetwork<IId>(metaNetwork.Event.ToVector());
            Organization = new VectorNetwork<IId>(metaNetwork.Organization.ToVector());

            #endregion

            #region Two modes networks

            ActorBelief = metaNetwork.ActorBelief.ToMatrix(Actor, Belief);
            ActorOrganization = metaNetwork.OrganizationActor.ToMatrix(Actor, Organization);
            ActorKnowledge = metaNetwork.ActorKnowledge.ToMatrix(Actor, Knowledge);
            ActorResource = metaNetwork.ActorResource.ToMatrix(Actor, Resource);
            ActorRole = metaNetwork.ActorRole.ToMatrix(Actor, Role);
            ActorTask = metaNetwork.ActorTask.ToMatrix(Actor, Task);

            #endregion
        }

        #region One mode networks
        /// <summary>
        ///     List of the agents of the meta network, called Actor network
        ///     An agent is an individual decision makers
        /// </summary>
        public VectorNetwork<IAgentId> Actor { get; set; }
        /// <summary>
        ///     List of the organizations of this network:
        ///     An organization is collectives of people that try to reach a common goal
        /// </summary>
        public VectorNetwork<IId> Organization { get; set; }

        /// <summary>
        ///     List of the roles of the meta network:
        ///     A role describe functions of agents
        /// </summary>
        public VectorNetwork<IId> Role { get; set; }

        /// <summary>
        ///     List of the resources of the meta network:
        ///     Resources are products, materials, or goods that are necessary to perform Tasks and Events
        /// </summary>
        public VectorNetwork<IAgentId> Resource { get; set; }
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
        ///     Actor x Actor network, called interaction network, social network
        ///     network of social links between agents, with their interaction type
        ///     Who interacts to / knows who
        /// </summary>
        public Matrix<float> ActorActor { get; set; }

        /// <summary>
        ///     Actor x Organization network, called work network, employment network
        ///     Who works where
        /// </summary>
        public Matrix<float> ActorOrganization { get; set; }
        /// <summary>
        ///     Actor x Role network
        ///     Who has what functions
        /// </summary>
        public Matrix<float> ActorRole { get; set; }
        /// <summary>
        ///     Actor x Resource network, called capabilities network
        ///     Who has what resource
        /// </summary>
        public Matrix<float> ActorResource { get; set; }
        /// <summary>
        ///     Actor * Knowledge network, called knowledge network
        ///     Who (Actor) knows what (Knowledge)
        /// </summary>
        public Matrix<float> ActorKnowledge { get; set; }
        /// <summary>
        ///     Actor * belief network
        ///     Who (Actor) believes what (Belief)
        /// </summary>
        public Matrix<float> ActorBelief { get; set; } 

        /// <summary>
        ///     Actor x Task network, called assignment network
        ///     Who (agent) does what (Tasks)
        /// </summary>
        public Matrix<float> ActorTask { get; set; } 
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
                case NetworkType.ActorActor:
                    return ActorActor;
                case NetworkType.ActorBelief:
                    return ActorBelief;
                case NetworkType.ActorOrganization:
                    return ActorOrganization;
                case NetworkType.ActorKnowledge:
                    return ActorKnowledge;
                case NetworkType.ActorResource:
                    return ActorResource;
                case NetworkType.ActorRole:
                    return ActorResource;
                case NetworkType.ActorTask:
                    return ActorTask;
                default:
                    throw new ArgumentOutOfRangeException(nameof(networkType), networkType, null);
            }
        }
    }
}