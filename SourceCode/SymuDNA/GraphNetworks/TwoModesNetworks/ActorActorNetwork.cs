#region Licence

// Description: SymuBiz - Symu
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Collections.Generic;
using System.Linq;
using Symu.Common.Interfaces.Agent;
using static Symu.Common.Constants;
#endregion

namespace Symu.DNA.GraphNetworks.TwoModesNetworks
{
    /// <summary>
    ///     Actor x Actor network, called interaction network, social network
    ///     network of social links between agents, with their interaction type
    ///     Who interacts to / knows who
    /// </summary>
    public class ActorActorNetwork
    {
        private float _maxWeight;
        public List<IActorActor> List { get; private set; } = new List<IActorActor>();
        public int Count => List.Count;

        /// <summary>
        ///     Gets or sets the element at the specified index
        /// </summary>
        /// <param name="index">0 based</param>
        /// <returns></returns>
        public IActorActor this[int index]
        {
            get => List[index];
            set => List[index] = value;
        }

        public void RemoveActor(IAgentId actorId)
        {
            List.RemoveAll(l => l.Id1.Equals(actorId) || l.Id2.Equals(actorId));
        }

        public bool Any()
        {
            return List.Any();
        }

        /// <summary>
        ///     Reinitialize links between members of a group :
        ///     Add a bi directional link between every member of a group
        /// </summary>
        public void AddInteractions(List<IActorActor> interactions)
        {
            if (interactions == null)
            {
                throw new ArgumentNullException(nameof(interactions));
            }

            foreach (var interaction in interactions)
            {
                AddInteraction(interaction);
            }
        }


        public void Clear()
        {
            List.Clear();
        }

        /// <summary>
        ///     Add interaction.
        /// If interaction already exist, it calls IncreaseInteraction
        /// </summary>
        /// <param name="actorActor"></param>
        public void AddInteraction(IActorActor actorActor)
        {
            if (actorActor == null)
            {
                throw new ArgumentNullException(nameof(actorActor));
            }

            if (actorActor.Id2.Equals(actorActor.Id1))
            {
                return;
            }

            if (Exists(actorActor))
            {
                IncreaseInteraction(actorActor);
            }
            else  
            {
                List.Add(actorActor);
            }
        }

        public bool Exists(IActorActor actorActor)
        {
            return List.Contains(actorActor);
        }

        /// <summary>
        ///     Link exists between actorId1 and actorId2 
        /// </summary>
        /// <param name="actorId1"></param>
        /// <param name="actorId2"></param>
        public bool Exists(IAgentId actorId1, IAgentId actorId2)
        {
            return List.Exists(x => x.HasLink(actorId1, actorId2));
        }

        private IActorActor Get(IAgentId actorId1, IAgentId actorId2)
        {
            return List.Find(x => x.HasLink(actorId1, actorId2));
        }

        /// <summary>
        /// Increase the weight of the interaction if the interaction is weighted
        /// </summary>
        private void IncreaseInteraction(IActorActor actorActor)
        {
            // As interaction can be a new instance of IInteraction, it may be not the one that is stored in the network
            var interactionFromNetwork = Get(actorActor.Id1, actorActor.Id2);
            interactionFromNetwork.IncreaseWeight();
        }

        /// <summary>
        /// Decrease the weight of the interaction if the interaction is weighted
        /// </summary>
        public void DecreaseInteraction(IAgentId actorId1, IAgentId actorId2)
        {
            if (Exists(actorId1, actorId2))
            {
                Get(actorId1, actorId2).DecreaseWeight();
            }
        }

        public bool HasActiveInteraction(IAgentId actorId1, IAgentId actorId2)
        {
            return List.Exists(l => l.HasActiveInteraction(actorId1, actorId2));
        }

        public float GetInteractionWeight(IAgentId actorId1, IAgentId actorId2)
        {
            return Exists(actorId1, actorId2) ? Get(actorId1, actorId2).Weight : 0;
        }

        public float NormalizedCountLinks(IAgentId actorId1, IAgentId actorId2)
        {
            return _maxWeight < Tolerance ? 0 : GetInteractionWeight(actorId1, actorId2) / _maxWeight;
        }

        public void SetMaxLinksCount()
        {
            _maxWeight = List.Any() ? List.Max(x => x.Weight) : 0;
        }

        #region unit tests

        public bool HasPassiveInteraction(IAgentId actorId1, IAgentId actorId2)
        {
            return List.Exists(l => l.HasPassiveInteraction(actorId1, actorId2));
        }

        /// <summary>
        ///     Get all the active links of an agent
        /// </summary>
        public IEnumerable<IAgentId> GetActiveInteractions(IAgentId agentId)
        {
            return List.FindAll(l => l.HasActiveInteractions(agentId)).Select(l => l.Id2).Distinct();
        }

        #endregion
        /// <summary>
        ///     Make a clone of Portfolios from modeling to Symu
        /// </summary>
        /// <param name="network"></param>
        public void CopyTo(ActorActorNetwork network)
        {
            if (network is null)
            {
                throw new ArgumentNullException(nameof(network));
            }

            network.List = List.ToList();
        }
    }
}