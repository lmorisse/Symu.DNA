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

namespace Symu.DNA.Networks.OneModeNetworks
{
    /// <summary>
    ///    Knowledge network
    ///    List of all the information known in the network
    /// </summary>
    /// <example></example>
    public class KnowledgeNetwork
    {

        /// <summary>
        ///     Repository of all the knowledges used in the network
        /// </summary>
        public List<IKnowledge> List { get; } = new List<IKnowledge>();

        public bool Any()
        {
            return List.Any();
        }

        public void Clear()
        {
            List.Clear();
        }
        public IKnowledge Get(IId knowledgeId)
        {
            return List.Find(k => k.Id.Equals(knowledgeId));
        }
        public TKnowledge Get<TKnowledge>(IId knowledgeId) where TKnowledge : IKnowledge
        {
            return (TKnowledge) Get(knowledgeId);
        }
        public IEnumerable<IId> GetIds()
        {
            return List.Select(x => x.Id);
        }
        /// <summary>
        ///     Add a Knowledge to the repository
        ///     Should be called only by NetWork, not directly to add belief in parallel
        /// </summary>
        public void Add(IKnowledge knowledge)
        {
            if (List.Contains(knowledge))
            {
                return;
            }

            List.Add(knowledge);
        }

        /// <summary>
        ///     Add a set of Knowledge to the repository
        /// </summary>
        public void Add(IEnumerable<IKnowledge> knowledges)
        {
            if (knowledges is null)
            {
                throw new ArgumentNullException(nameof(knowledges));
            }

            foreach (var knowledge in knowledges)
            {
                Add(knowledge);
            }
        }

    }
}