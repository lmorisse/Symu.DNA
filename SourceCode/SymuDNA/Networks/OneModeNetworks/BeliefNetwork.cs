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

#endregion

namespace Symu.DNA.Networks.OneModeNetworks
{
    /// <summary>
    ///     List of the beliefs of the meta network:
    ///     Beliefs are any form of religion or other persuasion.
    /// </summary>
    public class BeliefNetwork : OneModeNetwork<IBelief>
    {
       
        public TBelief Get<TBelief>(IId beliefId) where TBelief : IBelief
        {
            return (TBelief)Get(beliefId);
        }

        public IBelief Get(IId beliefId)
        {
            return List.Find(k => k.Id.Equals(beliefId));
        }

        public bool Exists(IId beliefId)
        {
            return List.Exists(k => k.Id.Equals(beliefId));
        }

        /// <summary>
        ///     Returns a list with the ids of all the beliefs
        /// </summary>
        /// <returns></returns>
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