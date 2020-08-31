#region Licence

// Description: SymuBiz - Symu
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Collections.Generic;
using Symu.Common.Interfaces.Entity;

#endregion

namespace Symu.DNA.OneModeNetworks.Knowledge
{
    /// <summary>
    ///    Knowledge network
    ///    List of all the information known in the network
    /// </summary>
    /// <example></example>
    public class KnowledgeNetwork
    {

        /// <summary>
        ///     Repository of all the knowledges used during the simulation
        /// </summary>
        public KnowledgeCollection Repository { get; } = new KnowledgeCollection();

        public bool Any()
        {
            return Repository.Any();
        }

        public void Clear()
        {
            Repository.Clear();
        }
        public IKnowledge GetKnowledge(IId knowledgeId)
        {
            return Repository.GetKnowledge(knowledgeId);
        }
        public TKnowledge GetKnowledge<TKnowledge>(IId knowledgeId) where TKnowledge : IKnowledge
        {
            return (TKnowledge) GetKnowledge(knowledgeId);
        }

        /// <summary>
        ///     Add a Knowledge to the repository
        ///     Should be called only by NetWork, not directly to add belief in parallel
        /// </summary>
        public void AddKnowledge(IKnowledge knowledge)
        {
            if (Repository.Contains(knowledge))
            {
                return;
            }

            Repository.Add(knowledge);
        }

        /// <summary>
        ///     Add a set of Knowledge to the repository
        /// </summary>
        public void AddKnowledges(IEnumerable<IKnowledge> knowledges)
        {
            if (knowledges is null)
            {
                throw new ArgumentNullException(nameof(knowledges));
            }

            foreach (var knowledge in knowledges)
            {
                AddKnowledge(knowledge);
            }
        }
    }
}