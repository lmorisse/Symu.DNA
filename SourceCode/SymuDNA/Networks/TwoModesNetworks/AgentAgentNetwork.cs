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

namespace Symu.DNA.Networks.TwoModesNetworks
{
    /// <summary>
    ///     Agent x Agent network, called interaction network, social network
    ///     network of social links between agents, with their interaction type
    ///     Who interacts to / knows who
    /// </summary>
    public class AgentAgentNetwork
    {
        private float _maxWeight;
        public List<IAgentAgent> List { get; private set; } = new List<IAgentAgent>();
        public int Count => List.Count;

        /// <summary>
        ///     Gets or sets the element at the specified index
        /// </summary>
        /// <param name="index">0 based</param>
        /// <returns></returns>
        public IAgentAgent this[int index]
        {
            get => List[index];
            set => List[index] = value;
        }

        public void RemoveAgent(IAgentId agentId)
        {
            List.RemoveAll(l => l.AgentId1.Equals(agentId) || l.AgentId2.Equals(agentId));
        }

        public bool Any()
        {
            return List.Any();
        }

        /// <summary>
        ///     Reinitialize links between members of a group :
        ///     Add a bi directional link between every member of a group
        /// </summary>
        public void AddInteractions(List<IAgentAgent> interactions)
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
        /// <param name="agentAgent"></param>
        public void AddInteraction(IAgentAgent agentAgent)
        {
            if (agentAgent == null)
            {
                throw new ArgumentNullException(nameof(agentAgent));
            }

            if (agentAgent.AgentId2.Equals(agentAgent.AgentId1))
            {
                return;
            }

            if (Exists(agentAgent))
            {
                IncreaseInteraction(agentAgent);
            }
            else  
            {
                List.Add(agentAgent);
            }
        }

        public bool Exists(IAgentAgent agentAgent)
        {
            return List.Contains(agentAgent);
        }

        /// <summary>
        ///     Link exists between agentId1 and agentId2 in the context of the groupId
        /// </summary>
        /// <param name="agentId1"></param>
        /// <param name="agentId2"></param>
        public bool Exists(IAgentId agentId1, IAgentId agentId2)
        {
            return List.Exists(x => x.HasLink(agentId1, agentId2));
        }

        private IAgentAgent Get(IAgentId agentId1, IAgentId agentId2)
        {
            return List.Find(x => x.HasLink(agentId1, agentId2));
        }

        /// <summary>
        /// Increase the weight of the interaction if the interaction is weighted
        /// </summary>
        private void IncreaseInteraction(IAgentAgent agentAgent)
        {
            // As interaction can be a new instance of IInteraction, it may be not the one that is stored in the network
            var interactionFromNetwork = Get(agentAgent.AgentId1, agentAgent.AgentId2);
            interactionFromNetwork.IncreaseWeight();
        }

        /// <summary>
        /// Decrease the weight of the interaction if the interaction is weighted
        /// </summary>
        public void DecreaseInteraction(IAgentId agentId1, IAgentId agentId2)
        {
            if (Exists(agentId1, agentId2))
            {
                Get(agentId1, agentId2).DecreaseWeight();
            }
        }

        public bool HasActiveInteraction(IAgentId agentId1, IAgentId agentId2)
        {
            return List.Exists(l => l.HasActiveInteraction(agentId1, agentId2));
        }

        public float GetInteractionWeight(IAgentId agentId1, IAgentId agentId2)
        {
            return Exists(agentId1, agentId2) ? Get(agentId1, agentId2).Weight : 0;
        }

        public float NormalizedCountLinks(IAgentId agentId1, IAgentId agentId2)
        {
            return _maxWeight < Tolerance ? 0 : GetInteractionWeight(agentId1, agentId2) / _maxWeight;
        }

        public void SetMaxLinksCount()
        {
            _maxWeight = List.Any() ? List.Max(x => x.Weight) : 0;
        }

        #region unit tests

        public bool HasPassiveInteraction(IAgentId agentId1, IAgentId agentId2)
        {
            return List.Exists(l => l.HasPassiveInteraction(agentId1, agentId2));
        }

        /// <summary>
        ///     Get all the active links of an agent
        /// </summary>
        public IEnumerable<IAgentId> GetActiveInteractions(IAgentId agentId)
        {
            return List.FindAll(l => l.HasActiveInteractions(agentId)).Select(l => l.AgentId2).Distinct();
        }

        #endregion
        /// <summary>
        ///     Make a clone of Portfolios from modeling to Symu
        /// </summary>
        /// <param name="network"></param>
        public void CopyTo(AgentAgentNetwork network)
        {
            if (network is null)
            {
                throw new ArgumentNullException(nameof(network));
            }

            network.List = List.ToList();
        }
    }
}