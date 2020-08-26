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
        public IId KnowledgeId { get; }
        public TestAgentKnowledge(IId knowledgeId)
        {
            KnowledgeId = knowledgeId;
        }
    }
}