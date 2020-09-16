#region Licence

// Description: SymuBiz - Symu
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;
using Symu.Common.Interfaces.Agent;
using Symu.Common.Interfaces.Entity;
using Symu.DNA.MatrixNetworks;

#endregion

namespace Symu.DNA.GraphNetworks.TwoModesNetworks
{
    /// <summary>
    ///     Actor * Knowledge network, called knowledge network
    ///     Who (Actor) knows what (Knowledge)
    /// </summary>
    public class ActorKnowledgeNetwork 
    {
        /// <summary>
        ///     List
        ///     Key => ComponentId
        ///     Values => ActorExpertise : list of KnowledgeIds/KnowledgeBits/KnowledgeLevel of an actor
        /// </summary>
        public ConcurrentDictionary<IAgentId, ActorExpertise> List { get; } =
            new ConcurrentDictionary<IAgentId, ActorExpertise>();

        public bool Any()
        {
            return List.Any();
        }

        public void Clear()
        {
            List.Clear();
        }
        public bool Exists(IAgentId actorId, IId knowledgeId)
        {
            return Exists(actorId) && List[actorId].Contains(knowledgeId);
        }

        public bool Exists(IAgentId actorId)
        {
            return List.ContainsKey(actorId);
        }

        public void Add(IAgentId actorId, ActorExpertise expertise)
        {
            if (expertise is null)
            {
                throw new ArgumentNullException(nameof(expertise));
            }

            AddActorId(actorId, expertise);


            foreach (var actorKnowledge in expertise.List.Where(a => !List[actorId].Contains(a)))
            {
                List[actorId].Add(actorKnowledge);
            }
        }

        public void Add(IAgentId actorId, IActorKnowledge actorKnowledge)
        {
            AddActorId(actorId);
            AddActorKnowledge(actorId, actorKnowledge);
        }

        /// <summary>
        ///     Add a knowledge to an AgentId
        ///     Actor is supposed to be already present in the collection.
        ///     if not use Add method
        /// </summary>
        /// <param name="actorKnowledge"></param>
        /// <param name="actorId"></param>
        public void AddActorKnowledge(IAgentId actorId, IActorKnowledge actorKnowledge)
        {
            if (actorKnowledge == null)
            {
                throw new ArgumentNullException(nameof(actorKnowledge));
            }

            if (!Exists(actorId, actorKnowledge.KnowledgeId))
            {
                List[actorId].Add(actorKnowledge);
            }
        }

        public void AddActorId(IAgentId actorId, ActorExpertise actorExpertise)
        {
            if (!Exists(actorId))
            {
                List.TryAdd(actorId, actorExpertise);
            }
        }

        public void AddActorId(IAgentId actorId)
        {
            if (!Exists(actorId))
            {
                List.TryAdd(actorId, new ActorExpertise());
            }
        }

        public IEnumerable<IAgentId> FilterActorsWithKnowledge(IEnumerable<IAgentId> actorIds, IId knowledgeId)
        {
            if (actorIds is null)
            {
                throw new ArgumentNullException(nameof(actorIds));
            }

            return actorIds.Where(actorId => Exists(actorId) && List[actorId].Contains(knowledgeId))
                .ToList();
        }

        public IEnumerable<IId> GetKnowledgeIds(IAgentId actorId)
        {
            if (!Exists(actorId))
            {
                throw new NullReferenceException(nameof(actorId));
            }

            return List[actorId].GetKnowledgeIds();
        }

        public void RemoveActor(IAgentId actorId)
        {
            List.TryRemove(actorId, out _);
        }

        public void RemoveKnowledge(IAgentId knowledgeId)
        {
            foreach (var actorExpertise in List.Values)
            {
                actorExpertise.List.RemoveAll(x => x.KnowledgeId.Equals(knowledgeId));
            }
        }

        /// <summary>
        ///     Get actor's expertise
        /// </summary>
        /// <param name="actorId"></param>
        /// <returns>NullReferenceException if actorId don't Exists, ActorExpertise otherwise</returns>
        public ActorExpertise GetActorExpertise(IAgentId actorId)
        {
            if (!Exists(actorId))
            {
                throw new NullReferenceException(nameof(actorId));
            }

            return List[actorId];
        }

        /// <summary>
        ///     Get actor's knowledge
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="knowledgeId"></param>
        /// <returns>null if actorId don't Exists, ActorExpertise otherwise</returns>
        public IActorKnowledge GetActorKnowledge(IAgentId actorId, IId knowledgeId)
        {
            if (!Exists(actorId, knowledgeId))
            {
                throw new NullReferenceException(nameof(actorId));
            }

            return List[actorId].GetActorKnowledge(knowledgeId);
        }

        /// <summary>
        ///     Get actor's knowledge
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="knowledgeId"></param>
        /// <returns>null if actorId don't Exists, ActorExpertise otherwise</returns>
        public TActorKnowledge GetActorKnowledge<TActorKnowledge>(IAgentId actorId, IId knowledgeId) where TActorKnowledge : IActorKnowledge
        {
            return (TActorKnowledge)GetActorKnowledge(actorId, knowledgeId);
        }
        /// <summary>
        /// Convert the network into a matrix
        /// </summary>
        /// <param name="actorIds"></param>
        /// <param name="knowledgeIds"></param>
        /// <returns></returns>
        public Matrix<float> ToMatrix(VectorNetwork<IAgentId> actorIds, VectorNetwork<IId> knowledgeIds)
        {
            if (!actorIds.Any || !knowledgeIds.Any)
            {
                return null;
            }
            var matrix = Matrix<float>.Build.Dense(actorIds.Count, knowledgeIds.Count);
            foreach (var agentExpertise in List)
            {
                if (!actorIds.ItemIndex.ContainsKey(agentExpertise.Key))
                {
                    throw new NullReferenceException(nameof(actorIds.ItemIndex));
                }
                var row = actorIds.ItemIndex[agentExpertise.Key];
                foreach (var actorKnowledge in agentExpertise.Value.List)
                {
                    if (!knowledgeIds.ItemIndex.ContainsKey(actorKnowledge.KnowledgeId))
                    {
                        throw new NullReferenceException(nameof(knowledgeIds.ItemIndex));
                    }
                    var column = knowledgeIds.ItemIndex[actorKnowledge.KnowledgeId];
                    matrix[row, column] = actorKnowledge.Value;
                }
            }
            return matrix;
        }
        /// <summary>
        ///     Make a clone of Portfolios from modeling to Symu
        /// </summary>
        /// <param name="network"></param>
        public void CopyTo(ActorKnowledgeNetwork network)
        {
            if (network is null)
            {
                throw new ArgumentNullException(nameof(network));
            }

            foreach (var keyValuePair in List)
            {
                var agentExpertise = new ActorExpertise {List = keyValuePair.Value.List.ToList()};

                network.Add(keyValuePair.Key, agentExpertise);
            }
        }
    }
}