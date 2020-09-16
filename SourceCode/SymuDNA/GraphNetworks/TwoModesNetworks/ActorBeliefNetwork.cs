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
    ///     Actor * belief network
    ///     Who (Actor) believes what (Belief)
    /// </summary>
    public class ActorBeliefNetwork
    {

        /// <summary>
        ///     List
        ///     Key => ComponentId
        ///     Values => ActorBelief : list of BeliefIds/BeliefBits/BeliefLevel of an actor
        /// </summary>
        public ConcurrentDictionary<IAgentId, ActorBeliefs> List { get; } =
            new ConcurrentDictionary<IAgentId, ActorBeliefs>();

        public int Count => List.Count;

        public bool Any()
        {
            return List.Any();
        }

        public void Clear()
        {
            List.Clear();
        }

        public bool Exists(IAgentId actorId)
        {
            return List.ContainsKey(actorId);
        }

        public bool Exists(IAgentId actorId, IId beliefId)
        {
            return Exists(actorId) && List[actorId].Contains(beliefId);
        }

        public void Add(IAgentId actorId, IActorBelief actorBelief)
        {
            AddActorId(actorId);
            AddActorBelief(actorId, actorBelief);
        }

        /// <summary>
        ///     Add a Belief to an AgentId
        ///     AgentId is supposed to be already present in the collection.
        ///     if not use Add method
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="actorBelief"></param>
        public void AddActorBelief(IAgentId actorId, IActorBelief actorBelief)
        {
            if (actorBelief == null)
            {
                throw new ArgumentNullException(nameof(actorBelief));
            }

            if (!List[actorId].Contains(actorBelief.BeliefId))
            {
                List[actorId].Add(actorBelief);
            }
        }
        public void Add(IAgentId actorId, ActorBeliefs beliefs)
        {
            if (beliefs is null)
            {
                throw new ArgumentNullException(nameof(beliefs));
            }

            AddActorId(actorId, beliefs);


            foreach (var agentBelief in beliefs.List.Where(a => !List[actorId].Contains(a)))
            {
                List[actorId].Add(agentBelief);
            }
        }

        public void AddActorId(IAgentId actorId)
        {
            if (!Exists(actorId))
            {
                List.TryAdd(actorId, new ActorBeliefs());
            }
        }
        public void AddActorId(IAgentId actorId, ActorBeliefs actorBeliefs)
        {
            if (!Exists(actorId))
            {
                List.TryAdd(actorId, actorBeliefs);
            }
        }


        public void RemoveActor(IAgentId actorId)
        {
            List.TryRemove(actorId, out _);
        }
        public void RemoveBelief(IAgentId beliefId)
        {
            foreach (var actorBeliefs in List.Values)
            {
                actorBeliefs.List.RemoveAll(x => x.BeliefId.Equals(beliefId));
            }
        }

        /// <summary>
        ///     Get actor's beliefs
        /// </summary>
        /// <param name="actorId"></param>
        /// <returns>null if actorId don't Exists, ActorBelief otherwise</returns>
        public ActorBeliefs GetActorBeliefs(IAgentId actorId)
        {
            if (!Exists(actorId))
            {
                throw new NullReferenceException(nameof(actorId));
            }

            return List[actorId];
        }

        public IEnumerable<IId> GetBeliefIds(IAgentId actorId)
        {
            if (!Exists(actorId))
            {
                throw new NullReferenceException(nameof(actorId));
            }

            return List[actorId].GetBeliefIds();
        }

        /// <summary>
        ///     Get actor's belief
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="beliefId"></param>
        /// <returns>null if actorId don't Exists, ActorBelief otherwise</returns>
        public IActorBelief GetAgentBelief(IAgentId actorId, IId beliefId)
        {
            var agentBeliefs = GetActorBeliefs(actorId);
            if (agentBeliefs is null)
            {
                throw new NullReferenceException(nameof(agentBeliefs));
            }

            return agentBeliefs.GetActorBelief(beliefId);
        }  
        
        /// <summary>
        ///     Get actor's belief
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="beliefId"></param>
        /// <returns>null if actorId don't Exists, ActorBelief otherwise</returns>
        public TActorBelief GetAgentBelief<TActorBelief>(IAgentId actorId, IId beliefId) where TActorBelief : IActorBelief
        {
            return (TActorBelief) GetAgentBelief(actorId, beliefId);
        }

        /// <summary>
        /// Convert the network into a matrix
        /// </summary>
        /// <param name="actorIds"></param>
        /// <param name="beliefIds"></param>
        /// <returns></returns>
        public Matrix<float> ToMatrix(VectorNetwork<IAgentId> actorIds, VectorNetwork<IId> beliefIds)
        {
            if (!actorIds.Any || !beliefIds.Any)
            {
                return null;
            }
            var matrix = Matrix<float>.Build.Dense(actorIds.Count, beliefIds.Count);
            foreach (var agentBeliefs in List)
            {
                if (!actorIds.ItemIndex.ContainsKey(agentBeliefs.Key))
                {
                    throw new NullReferenceException(nameof(actorIds.ItemIndex));
                }
                var row = actorIds.ItemIndex[agentBeliefs.Key];
                foreach (var agentBelief in agentBeliefs.Value.List)
                {
                    if (!beliefIds.ItemIndex.ContainsKey(agentBelief.BeliefId))
                    {
                        throw new NullReferenceException(nameof(beliefIds.ItemIndex));
                    }
                    var column = beliefIds.ItemIndex[agentBelief.BeliefId];
                    matrix[row, column] = agentBelief.Value;
                }
            }
            return matrix;
        }
        /// <summary>
        ///     Make a clone of Portfolios from modeling to Symu
        /// </summary>
        /// <param name="network"></param>
        public void CopyTo(ActorBeliefNetwork network)
        {
            if (network is null)
            {
                throw new ArgumentNullException(nameof(network));
            }

            foreach (var keyValuePair in List)
            {
                ActorBeliefs actorBeliefs = new ActorBeliefs { List = keyValuePair.Value.List.ToList() };

                network.Add(keyValuePair.Key, actorBeliefs);
            }
        }


    }
}