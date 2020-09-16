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
    public class ActorExpertise
    {
        /// <summary>
        ///     Key => ComponentId
        ///     Values => List of Knowledge
        /// </summary>
        public List<IActorKnowledge> List { get; set; } = new List<IActorKnowledge>();

        public int Count => List.Count;

        public IEnumerable<TActorKnowledge> GetActorKnowledges<TActorKnowledge>() where TActorKnowledge : IActorKnowledge
        {
            return List.Cast<TActorKnowledge>();
        }
        public bool Any()
        {
            return List.Any();
        }

        public void Clear()
        {
            List.Clear();
        }

        public void Add(IActorKnowledge actorKnowledge)
        {
            if (actorKnowledge == null)
            {
                throw new ArgumentNullException(nameof(actorKnowledge));
            }

            if (Contains(actorKnowledge))
            {
                return;
            }

            List.Add(actorKnowledge);
        }

        public bool Contains(IActorKnowledge actorKnowledge)
        {
            if (actorKnowledge is null)
            {
                throw new ArgumentNullException(nameof(actorKnowledge));
            }

            return Contains(actorKnowledge.KnowledgeId);
        }

        public bool Contains(IId knowledgeId)
        {
            return List.Exists(x => x.KnowledgeId.Equals(knowledgeId));
        }

        public IActorKnowledge GetActorKnowledge(IId knowledgeId)
        {
            return List.Find(x => x.KnowledgeId.Equals(knowledgeId));
        }

        public TActorKnowledge GetActorKnowledge<TActorKnowledge>(IId knowledgeId) where TActorKnowledge : IActorKnowledge
        {
            return (TActorKnowledge)GetActorKnowledge(knowledgeId);
        }

        /// <summary>
        ///     Get all the knowledge of an agent
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IId> GetKnowledgeIds()
        {
            return List.Select(x => x.KnowledgeId);
        }
    }
}