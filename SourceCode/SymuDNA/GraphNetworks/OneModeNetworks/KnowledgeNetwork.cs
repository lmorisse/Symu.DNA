#region Licence

// Description: SymuBiz - Symu
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System.Collections.Generic;
using System.Linq;
using Symu.Common.Interfaces.Entity;
using Symu.DNA.Entities;

#endregion

namespace Symu.DNA.GraphNetworks.OneModeNetworks
{
    /// <summary>
    ///     List of the knowledge of the meta network:
    ///     A knowledge is cognitive capabilities and skills
    /// </summary>
    public class KnowledgeNetwork : OneModeNetwork<IKnowledge>
    {
        
        //public IKnowledge GetEntity(IId knowledgeId)
        //{
        //    return List.Find(k => k.EntityId.Equals(knowledgeId));
        //}
        //public TKnowledge GetEntity<TKnowledge>(IId knowledgeId) where TKnowledge : IKnowledge
        //{
        //    return (TKnowledge) GetEntity(knowledgeId);
        //}
        //public IEnumerable<IId> GetEntityIds()
        //{
        //    return List.Select(x => x.EntityId);
        //}
        
        //public IReadOnlyList<IId> ToVector()
        //{
        //    return GetEntityIds().OrderBy(x => x).ToList();
        //}

    }
}