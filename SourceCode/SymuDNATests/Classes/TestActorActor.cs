#region Licence

// Description: SymuBiz - Symu
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using Symu.Common.Interfaces.Agent;
using Symu.DNA.GraphNetworks.TwoModesNetworks;
using static Symu.Common.Constants;

#endregion

namespace SymuDNATests.Classes
{
    /// <summary>
    ///Default implementation of IInteraction
    /// Defines the interaction between two agents used by InteractionNetwork
    ///     link are bidirectional.
    ///     AgentId1 has the smallest key
    ///     AgentId2 has the highest key
    /// </summary>
    public class TestActorActor : IActorActor
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="agentId1"></param>
        /// <param name="agentId2"></param>
        public TestActorActor(IAgentId agentId1, IAgentId agentId2)
        {
            if (agentId1 == null)
            {
                throw new ArgumentNullException(nameof(agentId1));
            }

            if (agentId1.CompareTo(agentId2))
            {
                Id1 = agentId1;
                Id2 = agentId2;
            }
            else
            {
                Id1 = agentId2;
                Id2 = agentId1;
            }

            IncreaseWeight();
        }

        public TestActorActor(IAgentId agentId1, IAgentId agentId2, float weight) : this(agentId1, agentId2)
        {
            Weight = weight;
        }

        /// <summary>
        ///     Number of interactions between the two agents
        /// </summary>
        public float Weight { get; private set; }

        /// <summary>
        ///     Unique key of the agent with the smallest key
        /// </summary>
        public IAgentId Id1 { get; }

        /// <summary>
        ///     Unique key of the agent with the highest key
        /// </summary>
        public IAgentId Id2 { get; }

        public bool IsActive => Weight > 0;
        public bool IsPassive => Weight < Tolerance;

        /// <summary>
        /// Increase the weight of the interaction
        /// </summary>
        public void IncreaseWeight()
        {
            Weight++;
        }

        /// <summary>
        /// Decrease the weight of the interaction
        /// </summary>
        public void DecreaseWeight()
        {
            if (Weight > 0)
            {
                Weight--;
            }
        }
        /// <summary>
        /// Agent has active interaction based on the weight of the interaction
        /// </summary>
        /// <param name="actorId"></param>
        /// <returns></returns>
        public bool HasActiveInteractions(IAgentId actorId)
        {
            return IsActive && (Id1.Equals(actorId) || Id2.Equals(actorId));
        }

        /// <summary>
        /// Agent has active interaction based on the weight of the interaction
        /// </summary>
        /// <param name="actorId1"></param>
        /// <param name="actorId2"></param>
        /// <returns></returns>
        public bool HasActiveInteraction(IAgentId actorId1, IAgentId actorId2)
        {
            return IsActive && HasLink(actorId1, actorId2);
        }
        /// <summary>
        /// Agent has passive interaction based on the weight of the interaction
        /// </summary>
        /// <param name="actorId1"></param>
        /// <param name="actorId2"></param>
        /// <returns></returns>
        public bool HasPassiveInteraction(IAgentId actorId1, IAgentId actorId2)
        {
            return IsPassive && HasLink(actorId1, actorId2);
        }

        public bool HasLink(IAgentId actorId1, IAgentId actorId2)
        {
            if (actorId1 == null)
            {
                throw new ArgumentNullException(nameof(actorId1));
            }

            if (actorId1.CompareTo(actorId2))
            {
                return Id1.Equals(actorId1) && Id2.Equals(actorId2);
            }

            return Id1.Equals(actorId2) && Id2.Equals(actorId1);
        }

        public override bool Equals(object obj)
        {
            return obj is TestActorActor link &&
                   link.HasLink(Id1, Id2);
        }

        public bool Equals(IActorActor obj)
        {
            return obj is TestActorActor link &&
                   link.HasLink(Id1, Id2);
        }
    }
}