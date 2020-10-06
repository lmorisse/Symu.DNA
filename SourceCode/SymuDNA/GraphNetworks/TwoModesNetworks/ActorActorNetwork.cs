#region Licence

// Description: SymuBiz - SymuDNA
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Collections.Generic;
using System.Linq;
using Symu.Common.Interfaces;
using Symu.DNA.Edges;
using Symu.DNA.Entities;

#endregion

namespace Symu.DNA.GraphNetworks.TwoModesNetworks
{
    /// <summary>
    ///     Actor x Actor network, called interaction network, social network
    ///     network of social links between agents, with their interaction type
    ///     Who interacts to / knows who
    ///     Source : Actor
    ///     Target : Actor
    /// </summary>
    public class ActorActorNetwork : TwoModesNetwork<IActorActor>
    {
        
        public void RemoveActor(IAgentId actorId)
        {
            //List.RemoveAll(l => l.Source.Equals(actorId) || l.Target.Equals(actorId));
            RemoveSource(actorId);
            RemoveTarget(actorId);
        }

        /// <summary>
        ///     Add interaction.
        ///     If interaction already exist, it calls IncreaseInteraction
        /// </summary>
        /// <param name="actorActor"></param>
        public override void Add(IActorActor actorActor)
        {
            if (actorActor == null)
            {
                throw new ArgumentNullException(nameof(actorActor));
            }

            if (actorActor.Target.Equals(actorActor.Source))
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

        /// <summary>
        ///     Link exists between actorId1 and actorId2
        /// </summary>
        /// <param name="actorId1"></param>
        /// <param name="actorId2"></param>
        /// <remarks>Specific because the link is bi directional, should be resolved with QuickGraph</remarks>
        public override bool Exists(IAgentId actorId1, IAgentId actorId2)
        {
            //todo should be removed with QuickGraph and directed graph
            return List.Exists(x => x.HasLink(actorId1, actorId2));
        }

        public override IActorActor Edge(IAgentId actorId1, IAgentId actorId2)
        {
            //todo should be removed with QuickGraph and directed graph
            return List.FirstOrDefault(x => x.HasLink(actorId1, actorId2));
        }

        public override IEnumerable<IActorActor> Edges(IAgentId sourceId, IAgentId targetId)
        {
            //todo should be removed with QuickGraph and directed graph
            return sourceId.CompareTo(targetId)
                ? base.Edges(sourceId, targetId)
                : base.Edges(targetId, sourceId);
        }

        /// <summary>
        ///     Increase the weight of the interaction if the interaction is weighted
        /// </summary>
        private void IncreaseInteraction(IEdge actorActor)
        {
            // As interaction can be a new instance of IInteraction, it may be not the one that is stored in the network
            var interactionFromNetwork = Edge(actorActor.Source, actorActor.Target);
            interactionFromNetwork.IncreaseWeight();
        }

        /// <summary>
        ///     Decrease the weight of the interaction if the interaction is weighted
        /// </summary>
        public void DecreaseInteraction(IAgentId actorId1, IAgentId actorId2)
        {
            if (!Exists(actorId1, actorId2))
            {
                return;
            }

            Edge(actorId1, actorId2).DecreaseWeight();
        }

        public bool HasActiveInteraction(IAgentId actorId1, IAgentId actorId2)
        {
            return Edges(actorId1, actorId2).ToList().Exists(l => l.HasActiveInteraction(actorId1, actorId2));
        }

        #region unit tests

        public bool HasPassiveInteraction(IAgentId actorId1, IAgentId actorId2)
        {
            return Edges(actorId1, actorId2).ToList().Exists(l => l.HasPassiveInteraction(actorId1, actorId2));
        }

        /// <summary>
        ///     Get the number of the active links of an actor
        /// </summary>
        public int ActiveInteractionCount(IAgentId actorId)
        {
            return List.Count(x => x.Equals(actorId) && x.IsActive);
        }

        #endregion

    }
}