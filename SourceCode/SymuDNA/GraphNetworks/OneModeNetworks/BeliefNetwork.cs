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
    ///     List of the beliefs of the meta network:
    ///     Beliefs are any form of religion or other persuasion.
    /// </summary>
    public class BeliefNetwork : OneModeNetwork<IBelief>
    {
       
        //public TBelief GetEntity<TBelief>(IId beliefId) where TBelief : IBelief
        //{
        //    return (TBelief)GetEntity(beliefId);
        //}

        //public IBelief GetEntity(IId beliefId)
        //{
        //    return List.Find(k => k.EntityId.Equals(beliefId));
        //}

        //public bool Exists(IId beliefId)
        //{
        //    return List.Exists(k => k.EntityId.Equals(beliefId));
        //}

        ///// <summary>
        /////     Returns a list with the ids of all the beliefs
        ///// </summary>
        ///// <returns></returns>
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