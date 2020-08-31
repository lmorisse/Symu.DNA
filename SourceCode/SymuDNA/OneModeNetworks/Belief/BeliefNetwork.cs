#region Licence

// Description: SymuBiz - Symu
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using Symu.Common.Interfaces.Entity;

#endregion

namespace Symu.DNA.OneModeNetworks.Belief
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
        ///     Repository of all the Beliefs used during the simulation
        /// </summary>
        public BeliefCollection Repository { get; } = new BeliefCollection();
        public bool Any()
        {
            return Repository.Any();
        }

        public void Clear()
        {
            Repository.Clear();
        }

        public IBelief GetBelief(IId beliefId)
        {
            return Repository.GetBelief(beliefId);
        }
        public TBelief GetBelief<TBelief>(IId beliefId) where TBelief : IBelief
        {
            return (TBelief)GetBelief(beliefId);
        }

        /// <summary>
        ///     Add a Belief to the repository
        /// </summary>
        public void AddBelief(IBelief belief)
        {
            if (Exists(belief))
            {
                return;
            }

            Repository.Add(belief);
        }

        public bool Exists(IBelief belief)
        {
            return Repository.Contains(belief);
        }

        public bool Exists(IId beliefId)
        {
            return Repository.Exists(beliefId);
        }
    }
}