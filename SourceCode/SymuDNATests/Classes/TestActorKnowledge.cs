#region Licence

// Description: SymuBiz - Symu
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

using Symu.Common.Interfaces.Entity;
using Symu.DNA.GraphNetworks.TwoModesNetworks;

namespace SymuDNATests.Classes
{    /// <summary>
    ///     Describe an area of knowledge
    /// </summary>
    public class TestActorKnowledge : IActorKnowledge
    {
        public float Value { get; set; }
        public IId KnowledgeId { get; }
        public TestActorKnowledge(IId knowledgeId)
        {
            KnowledgeId = knowledgeId;
        }
        public TestActorKnowledge(IId knowledgeId, float value): this(knowledgeId)
        {
            Value = value;
        }

        public float CompareTo(IActorKnowledge other)
        {
            if (other is TestActorKnowledge test)
            {
                return Value * test.Value;
            }
            return 0;
        }
    }
}