#region Licence

// Description: SymuBiz - Symu
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using Symu.Common.Interfaces.Entity;
using Symu.DNA.Beliefs;

#endregion

namespace SymuDNATests.Classes
{
    public class TestAgentBelief : IAgentBelief
    {
        internal float Value { get; set; }
        public TestAgentBelief(IId beliefId)
        {
            BeliefId = beliefId;
        }
        public IId BeliefId { get; }

        public float CompareTo(IAgentBelief other)
        {
            if (other is TestAgentBelief test)
            {
                return Value * test.Value;
            }
            return 0;
        }
    }
}