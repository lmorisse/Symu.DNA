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
using Symu.Common.Interfaces.Entity;

#endregion

namespace Symu.DNA.GraphNetworks.TwoModesNetworks
{
    /// <summary>
    ///     Expertise of an actor is defined by the list of all its knowledge (hard skills)  x KnowledgeLevel
    /// </summary>
    /// <example>Dev Java, test, project management, sociology, ...</example>
    public class ActorBeliefs
    {
        /// <summary>
        ///     Key => ComponentId
        ///     Values => List of Knowledge
        /// </summary>
        public List<IActorBelief> List { get; set; } = new List<IActorBelief>();

        public bool Any()
        {
            return List.Any();
        }

        public int Count => List.Count;

        public void Add(IActorBelief actorBelief)
        {
            if (!Contains(actorBelief))
            {
                List.Add(actorBelief);
            }
        }

        public bool Contains(IActorBelief actorBelief)
        {
            if (actorBelief is null)
            {
                throw new ArgumentNullException(nameof(actorBelief));
            }

            return Contains(actorBelief.BeliefId);
        }

        public bool Contains(IId beliefId)
        {
            return List.Exists(x => x.BeliefId.Equals(beliefId));
        }

        public IActorBelief GetActorBelief(IId beliefId)
        {
            return List.Find(x => x.BeliefId.Equals(beliefId));
        }

        public TActorBelief GetActorBelief<TActorBelief>(IId beliefId) where TActorBelief : IActorBelief
        {
            return (TActorBelief)GetActorBelief(beliefId);
        }

        public IEnumerable<TActorBelief> GetAgentBeliefs<TActorBelief>() where TActorBelief : IActorBelief
        {
            return List.Cast<TActorBelief>();
        }

        /// <summary>
        ///     Get all the belief Ids of an actor
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IId> GetBeliefIds()
        {
            return List.Select(x => x.BeliefId);
        }
    }
}