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
    public class ActorActorTests
    {
        private readonly IAgentId _agentId1 = new AgentId(2, 2);
        private readonly IAgentId _agentId2 = new AgentId(3, 2);
        private readonly IAgentId _agentId3 = new AgentId(4, 2);

        private IActorActor _interaction12;
        private IActorActor _interaction21;
        private IActorActor _interaction31;

        [TestInitialize]
        public void Initialize()
        {
            _interaction12 = new ActorActor(_agentId1, _agentId2, 1);
            _interaction21 = new ActorActor(_agentId2, _agentId1);
            _interaction31 = new ActorActor(_agentId3, _agentId1);
        }

        [TestMethod]
        public void ActorActorTest()
        {
            Assert.AreEqual(1, _interaction12.Weight);
            Assert.AreEqual(1, _interaction21.Weight);
        }


        [TestMethod]
        public void EqualsSourceTest()
        {
            Assert.IsTrue(_interaction12.EqualsSource(_agentId1));
            Assert.IsTrue(_interaction21.EqualsSource(_agentId1));
            Assert.IsTrue(_interaction31.EqualsSource(_agentId1));
            Assert.IsFalse(_interaction31.EqualsSource(_agentId2));
            Assert.IsFalse(_interaction12.EqualsSource(_agentId2));
            Assert.IsFalse(_interaction21.EqualsSource(_agentId2));
        }

        [TestMethod]
        public void EqualsTargetTest()
        {
            Assert.IsFalse(_interaction12.EqualsTarget(_agentId1));
            Assert.IsFalse(_interaction21.EqualsTarget(_agentId1));
            Assert.IsFalse(_interaction31.EqualsTarget(_agentId1));
            Assert.IsTrue(_interaction31.EqualsTarget(_agentId3));
            Assert.IsTrue(_interaction12.EqualsTarget(_agentId2));
            Assert.IsTrue(_interaction21.EqualsTarget(_agentId2));
        }

        [TestMethod]
        public void EqualsTest()
        {
            Assert.IsTrue(_interaction12.Equals(_agentId1));
            Assert.IsTrue(_interaction21.Equals(_agentId1));
            Assert.IsTrue(_interaction31.Equals(_agentId1));
            Assert.IsFalse(_interaction31.Equals(_agentId2));
        }

        [TestMethod]
        public void IncreaseWeightTest()
        {
            Assert.AreEqual(1, _interaction12.Weight);
            _interaction12.IncreaseWeight();
            Assert.AreEqual(2, _interaction12.Weight);
        }

        [TestMethod]
        public void DecreaseWeightTest()
        {
            Assert.AreEqual(1, _interaction12.Weight);
            _interaction12.DecreaseWeight();
            Assert.AreEqual(0, _interaction12.Weight);
        }

        [TestMethod]
        public void HasActiveInteractionsTest()
        {
            Assert.IsTrue(_interaction12.HasActiveInteraction(_agentId1));
            Assert.IsTrue(_interaction12.HasActiveInteraction(_agentId2));
            Assert.IsFalse(_interaction12.HasActiveInteraction(_agentId3));
        }

        [TestMethod]
        public void HasActiveInteractionTest()
        {
            Assert.IsTrue(_interaction12.HasActiveInteraction(_agentId1, _agentId2));
            Assert.IsFalse(_interaction12.HasActiveInteraction(_agentId1, _agentId3));
        }

        [TestMethod]
        public void HasPassiveInteractionTest()
        {
            Assert.IsFalse(_interaction12.HasPassiveInteraction(_agentId1, _agentId2));
            _interaction12.DecreaseWeight();
            Assert.IsTrue(_interaction12.HasPassiveInteraction(_agentId1, _agentId2));
            Assert.IsFalse(_interaction12.HasPassiveInteraction(_agentId1, _agentId3));
        }

        [TestMethod]
        public void HasLinkTest()
        {
            Assert.IsTrue(_interaction12.HasLink(_agentId1, _agentId2));
            Assert.IsTrue(_interaction12.HasLink(_agentId2, _agentId1));
            Assert.IsFalse(_interaction12.HasLink(_agentId1, _agentId3));
        }
        [TestMethod]
        public void CloneTest()
        {
            var clone = _interaction12.Clone();
            Assert.AreEqual(_interaction12, clone);
        }
    }
}