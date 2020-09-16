#region Licence

// Description: SymuBiz - SymuTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.DNA.Entities;
using Symu.DNA.GraphNetworks.TwoModesNetworks;
using SymuDNATests.Classes;

#endregion

namespace SymuDNATests.Networks.TwoModesNetworks
{
    [TestClass]
    public class AgentExpertiseTests
    {
        private readonly ActorExpertise _expertise = new ActorExpertise();

        private readonly KnowledgeEntity _knowledge =
            new KnowledgeEntity(1);

        private TestActorKnowledge _actorKnowledge;

        [TestInitialize]
        public void Initialize()
        {
            _actorKnowledge = new TestActorKnowledge(_knowledge.AgentId);
        }

        [TestMethod]
        public void ContainsTest()
        {
            Assert.IsFalse(_expertise.Contains(_knowledge.AgentId));
            _expertise.Add(_actorKnowledge);
            Assert.IsTrue(_expertise.Contains(_knowledge.AgentId));
        }

        [TestMethod]
        public void ContainsIdTest()
        {
            Assert.IsFalse(_expertise.Contains(_knowledge.AgentId));
            _expertise.Add(_actorKnowledge);
            Assert.IsTrue(_expertise.Contains(_knowledge.AgentId));
        }

        [TestMethod]
        public void GetKnowledgesTest()
        {
            Assert.AreEqual(0, _expertise.GetKnowledgeIds().Count());
            _expertise.Add(_actorKnowledge);
            Assert.AreEqual(1, _expertise.GetKnowledgeIds().Count());
        }

        /// <summary>
        ///     Non passing test
        /// </summary>
        [TestMethod]
        public void GetKnowledgeTest()
        {
            Assert.IsNull(_expertise.GetActorKnowledge(_knowledge.AgentId));
        }
    }
}