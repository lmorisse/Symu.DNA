#region Licence

// Description: SymuBiz - SymuDNA
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using MathNet.Numerics.LinearAlgebra;
using Symu.OrgMod.Edges;
using Symu.OrgMod.GraphNetworks;
using Symu.OrgMod.GraphNetworks.TwoModesNetworks;

#endregion

namespace Symu.DNA
{
    /// <summary>
    ///     Referential of networks for social and organizational network analysis
    ///     Feed by the MetaNetwork or directly
    /// </summary>
    public class MatrixMetaNetwork
    {
        public MatrixMetaNetwork()
        {
        }

        public MatrixMetaNetwork(GraphMetaNetwork metaNetwork)
        {
            #region One mode networks

            Actor = new VectorNetwork(metaNetwork.Actor.ToVector());
            Role = new VectorNetwork(metaNetwork.Role.ToVector());
            Resource = new VectorNetwork(metaNetwork.Resource.ToVector());
            Knowledge = new VectorNetwork(metaNetwork.Knowledge.ToVector());
            Belief = new VectorNetwork(metaNetwork.Belief.ToVector());
            Task = new VectorNetwork(metaNetwork.Task.ToVector());
            Event = new VectorNetwork(metaNetwork.Event.ToVector());
            Organization = new VectorNetwork(metaNetwork.Organization.ToVector());

            #endregion

            #region Two modes networks

            ActorActor = ImportToMatrix(metaNetwork.ActorActor.Clone() , Actor, Actor);
            ActorBelief = ImportToMatrix(metaNetwork.ActorBelief.Clone(), Actor, Belief);
            ActorOrganization = ImportToMatrix(metaNetwork.ActorOrganization.Clone(), Actor, Organization);
            ActorKnowledge = ImportToMatrix(metaNetwork.ActorKnowledge.Clone(), Actor, Knowledge);
            ActorResource = ImportToMatrix(metaNetwork.ActorResource.Clone(), Actor, Resource);
            ActorRole = ImportToMatrix(metaNetwork.ActorRole.Clone(), Actor, Role);
            ActorTask = ImportToMatrix(metaNetwork.ActorTask.Clone(), Actor, Task);
            ResourceTask = ImportToMatrix(metaNetwork.ResourceTask.Clone(), Resource, Task);
            TaskKnowledge = ImportToMatrix(metaNetwork.TaskKnowledge.Clone(), Task, Knowledge);
            OrganizationResource = ImportToMatrix(metaNetwork.OrganizationResource.Clone(), Organization, Resource);
            ResourceResource = ImportToMatrix(metaNetwork.ResourceResource.Clone(), Resource, Resource);
            ResourceKnowledge = ImportToMatrix(metaNetwork.ResourceKnowledge.Clone(), Resource, Knowledge);

            #endregion
        }

        /// <summary>
        ///     Get the right Matrix from the networkType
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

        #region One mode networks

        /// <summary>
        ///     List of the agents of the meta network, called Actor network
        ///     An agent is an individual decision makers
        /// </summary>
        public VectorNetwork Actor { get; set; }

        /// <summary>
        ///     List of the organizations of this network:
        ///     An organization is collectives of people that try to reach a common goal
        /// </summary>
        public VectorNetwork Organization { get; set; }

        /// <summary>
        ///     List of the roles of the meta network:
        ///     A role describe functions of agents
        /// </summary>
        public VectorNetwork Role { get; set; }

        /// <summary>
        ///     List of the resources of the meta network:
        ///     Resources are products, materials, or goods that are necessary to perform Tasks and Events
        /// </summary>
        public VectorNetwork Resource { get; set; }

        /// <summary>
        ///     List of the knowledge of the meta network:
        ///     A knowledge is cognitive capabilities and skills
        /// </summary>
        public VectorNetwork Knowledge { get; set; }

        /// <summary>
        ///     List of the beliefs of the meta network:
        ///     Beliefs are any form of religion or other persuasion.
        /// </summary>
        public VectorNetwork Belief { get; set; }

        /// <summary>
        ///     List of the tasks of the meta network:
        ///     A Task is a well defined procedures or goals of an organization, scheduled or planned activities
        /// </summary>
        public VectorNetwork Task { get; set; }

        /// <summary>
        ///     List of the Events of the meta network:
        ///     An Event is occurrences or phenomena that happen
        /// </summary>
        public VectorNetwork Event { get; set; }

        #endregion

        #region Two modes networks

        /// <summary>
        ///     Resource * Resource network
        ///     Which Resource uses what resource
        /// </summary>
        public Matrix<float> ResourceResource { get; set; }

        /// <summary>
        ///     Organization * Resource network
        ///     Which organization uses what resource
        /// </summary>
        public Matrix<float> OrganizationResource { get; set; }

        /// <summary>
        ///     Task * Knowledge network
        ///     What knowledge is necessary for what Task
        /// </summary>
        public Matrix<float> TaskKnowledge { get; set; }

        /// <summary>
        ///     Resource x Task network
        ///     What resource works on what tasks
        /// </summary>
        public Matrix<float> ResourceTask { get; set; }

        /// <summary>
        ///     Resource x Knowledge network
        ///     What resource stores what knowledge
        /// </summary>
        public Matrix<float> ResourceKnowledge { get; set; }

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
        ///     Convert the network into a matrix
        /// </summary>
        /// <param name="network"></param>
        /// <param name="sourceIds"></param>
        /// <param name="targetIds"></param>
        /// <returns></returns>
        public Matrix<float> ImportToMatrix(TwoModesNetwork<IEdge> network, VectorNetwork sourceIds, VectorNetwork targetIds)
        {
            if (!sourceIds.Any || !targetIds.Any)
            {
                return null;
            }

            var matrix = Matrix<float>.Build.Dense(sourceIds.Count, targetIds.Count);
            for (var i = 0; i < sourceIds.Count; i++)
            {
                var sourceId = sourceIds.IndexItem[i];
                if (!sourceIds.ItemIndex.ContainsKey(sourceId))
                {
                    throw new NullReferenceException(nameof(sourceIds.ItemIndex));
                }

                var row = sourceIds.ItemIndex[sourceId];
                foreach (var edge in network.EdgesFilteredBySource(sourceId))
                {
                    if (!targetIds.ItemIndex.ContainsKey(edge.Target))
                    {
                        throw new NullReferenceException(nameof(targetIds.ItemIndex));
                    }

                    var column = targetIds.ItemIndex[edge.Target];
                    matrix[row, column] = edge.Weight;
                }
            }

            return matrix;
        }
    }
}