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
    ///     List of the knowledge of the meta network:
    ///     A knowledge is cognitive capabilities and skills
    /// </summary>
    public class KnowledgeNetwork : OneModeNetwork<IKnowledge>
    {
        
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
        
        public IReadOnlyList<IId> ToVector()
        {
            return GetIds().OrderBy(x => x).ToList();
        }

    }
}