#region Licence

// Description: SymuBiz - Symu
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using Symu.Common.Interfaces.Entity;
using Symu.DNA.GraphNetworks.TwoModesNetworks;

#endregion

namespace SymuDNATests.Classes
{
    public class TestActorBelief : IActorBelief
    {
        public float Value { get; set; }
        public TestActorBelief(IId beliefId)
        {
            BeliefId = beliefId;
        }
        public IId BeliefId { get; }

        public float CompareTo(IActorBelief other)
        {
            if (other is TestActorBelief test)
            {
                return Value * test.Value;
            }
            return 0;
        }
    }
}