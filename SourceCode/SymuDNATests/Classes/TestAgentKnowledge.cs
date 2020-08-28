#region Licence

// Description: SymuBiz - Symu
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

using Symu.Common.Interfaces.Entity;
using Symu.DNA.Knowledges;

namespace SymuDNATests.Classes
{    /// <summary>
    ///     Describe an area of knowledge
    /// </summary>
    public class TestAgentKnowledge : IAgentKnowledge
    {
        internal float Value { get; set; }
        public IId KnowledgeId { get; }
        public TestAgentKnowledge(IId knowledgeId)
        {
            KnowledgeId = knowledgeId;
        }
        public TestAgentKnowledge(IId knowledgeId, float value): this(knowledgeId)
        {
            Value = value;
        }

        public float CompareTo(IAgentKnowledge other)
        {
            if (other is TestAgentKnowledge test)
            {
                return Value * test.Value;
            }
            return 0;
        }
    }
}