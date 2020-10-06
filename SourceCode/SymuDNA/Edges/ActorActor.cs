#region Licence

// Description: SymuBiz - SymuDNA
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using Symu.Common.Interfaces;
using static Symu.Common.Constants;

#endregion

namespace Symu.DNA.Edges
{
    /// <summary>
    ///     Default implementation of IInteraction
    ///     Defines the interaction between two agents used by InteractionNetwork
    ///     link are bidirectional.
    ///     AgentId1 has the smallest key
    ///     AgentId2 has the highest key
    /// </summary>
    public class ActorActor : IActorActor
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="agentId1"></param>
        /// <param name="agentId2"></param>
        public ActorActor(IAgentId agentId1, IAgentId agentId2)
        {
            if (agentId1 == null)
            {
                throw new ArgumentNullException(nameof(agentId1));
            }

            if (agentId1.CompareTo(agentId2))
            {
                Source = agentId1;
                Target = agentId2;
            }
            else
            {
                Source = agentId2;
                Target = agentId1;
            }

            IncreaseWeight();
        }

        public ActorActor(IAgentId agentId1, IAgentId agentId2, float weight) : this(agentId1, agentId2)
        {
            Weight = weight;
        }

        #region IActorActor Members

        public bool IsActive => Weight > 0;
        public bool IsPassive => Weight < Tolerance;

        public bool Equals(IAgentId actorId)
        {
            return EqualsSource(actorId) || EqualsTarget(actorId);
        }

        /// <summary>
        ///     Increase the weight of the interaction
        /// </summary>
        public void IncreaseWeight()
        {
            Weight++;
        }

        /// <summary>
        ///     Decrease the weight of the interaction
        /// </summary>
        public void DecreaseWeight()
        {
            if (Weight > 0)
            {
                Weight--;
            }
        }

        /// <summary>
        ///     Agent has active interaction based on the weight of the interaction
        /// </summary>
        /// <param name="actorId"></param>
        /// <returns></returns>
        public bool HasActiveInteraction(IAgentId actorId)
        {
            return IsActive && (Source.Equals(actorId) || Target.Equals(actorId));
        }

        /// <summary>
        ///     Agent has active interaction based on the weight of the interaction
        /// </summary>
        /// <param name="actorId1"></param>
        /// <param name="actorId2"></param>
        /// <returns></returns>
        public bool HasActiveInteraction(IAgentId actorId1, IAgentId actorId2)
        {
            return IsActive && HasLink(actorId1, actorId2);
        }

        /// <summary>
        ///     Agent has passive interaction based on the weight of the interaction
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

            return actorId1.CompareTo(actorId2)
                ? Source.Equals(actorId1) && Target.Equals(actorId2)
                : Source.Equals(actorId2) && Target.Equals(actorId1);
        }

        #endregion

        public override bool Equals(object obj)
        {
            return obj is ActorActor link &&
                   link.HasLink(Source, Target);
        }

        #region Interface IEdge

        /// <summary>
        ///     Number of interactions between the two agents
        /// </summary>
        public float Weight { get; set; }

        /// <summary>
        ///     Normalized weight computed by the network via the NormalizeWeights method
        /// </summary>
        public float NormalizedWeight { get; set; }

        public bool EqualsSource(IAgentId source)
        {
            return source.Equals(Source);
        }

        public bool EqualsTarget(IAgentId target)
        {
            return target.Equals(Target);
        }

        /// <summary>
        ///     Unique key of the agent with the smallest key
        /// </summary>
        public IAgentId Source { get; set; }

        /// <summary>
        ///     Unique key of the agent with the highest key
        /// </summary>
        public IAgentId Target { get; set; }

        public object Clone()
        {
            return new ActorActor(Source, Target, Weight);
        }
        #endregion
    }
}