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
    ///     Belief network
    ///     Who (agentId) knows what (Belief)
    ///     Key => the agentId
    ///     Value : the list of NetworkInformation the agent knows
    /// </summary>
    /// <example></example>
    public class BeliefNetwork
    {
        /// <summary>
        ///     Repository of all the Beliefs used in the network
        /// </summary>
        public List<IBelief> List { get; } = new List<IBelief>();
        public bool Any()
        {
            return List.Any();
        }

        public void Clear()
        {
            List.Clear();
        }
        public TBelief Get<TBelief>(IId beliefId) where TBelief : IBelief
        {
            return (TBelief)Get(beliefId);
        }

        public IBelief Get(IId beliefId)
        {
            return List.Find(k => k.Id.Equals(beliefId));
        }

        /// <summary>
        ///     Add a Belief to the repository
        /// </summary>
        public void Add(IBelief belief)
        {
            if (Exists(belief))
            {
                return;
            }

            List.Add(belief);
        }

        public bool Exists(IBelief belief)
        {
            return List.Contains(belief);
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