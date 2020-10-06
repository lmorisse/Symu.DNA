#region Licence

// Description: SymuBiz - SymuDNATests
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.Common.Interfaces;
using Symu.DNA.Edges;

#endregion

namespace SymuDNATests.Edges
{
    [TestClass]
    public class ActorKnowledgeTests
    {
        private readonly IAgentId _actorId = new AgentId(1, 1);
        private readonly IAgentId _knowledgeId = new AgentId(2, 2);

        private IEntityKnowledge _edge;
        private IEntityKnowledge _edge1;

        [TestInitialize]
        public void Initialize()
        {
            _edge = new EntityKnowledge(_actorId, _knowledgeId);
            _edge1 = new EntityKnowledge(_actorId, _knowledgeId, 2);
        }

        [TestMethod]
        public void CompareToTest()
        {
            Assert.AreEqual(2, _edge.CompareTo(_edge1));
        }
        [TestMethod]
        public void CloneTest()
        {
            var clone = _edge.Clone();
            Assert.AreEqual(_edge, clone);
        }
    }
}