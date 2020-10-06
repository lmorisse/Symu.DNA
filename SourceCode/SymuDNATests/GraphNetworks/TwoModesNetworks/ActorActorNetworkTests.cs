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
using Symu.DNA.GraphNetworks.TwoModesNetworks;

#endregion

namespace SymuDNATests.GraphNetworks.TwoModesNetworks
{
    [TestClass]
    public class ActorActorNetworkTests
    {
        private readonly IAgentId _agentId1 = new AgentId(2, 2);
        private readonly IAgentId _agentId2 = new AgentId(3, 2);
        private readonly IAgentId _agentId3 = new AgentId(4, 2);
        private readonly ActorActorNetwork _network = new ActorActorNetwork();

        private IActorActor _interaction12;
        private IActorActor _interaction21;
        private IActorActor _interaction31;

        [TestInitialize]
        public void Initialize()
        {
            _interaction12 = new ActorActor(_agentId1, _agentId2);
            _interaction21 = new ActorActor(_agentId2, _agentId1);
            _interaction31 = new ActorActor(_agentId3, _agentId1);
        }

        [TestMethod]
        public void RemoveActorTest()
        {
            _network.Add(_interaction12);
            _network.Add(_interaction31);
            _network.RemoveActor(_agentId1);
            Assert.IsFalse(_network.Any());
        }
        /// <summary>
        /// Direct test
        /// </summary>
        [TestMethod]
        public void DecreaseInteractionTest()
        {
            _network.Add(_interaction12);
            var link = _network[0];
            // Active link
            Assert.IsTrue(link.IsActive);
            // Deactivate
            _network.DecreaseInteraction(_agentId1, _agentId2);
            Assert.IsFalse(link.IsActive);
            Assert.IsTrue(link.IsPassive);
        }
        /// <summary>
        /// indirect test
        /// </summary>
        [TestMethod]
        public void DecreaseInteractionTest1()
        {
            _network.Add(_interaction12);
            var link = _network[0];
            // Active link
            Assert.IsTrue(link.IsActive);
            // Deactivate
            _network.DecreaseInteraction(_agentId2, _agentId1);
            Assert.IsFalse(link.IsActive);
            Assert.IsTrue(link.IsPassive);
        }
        /// <summary>
        /// Direct test
        /// </summary>
        [TestMethod]
        public void HasActiveInteractionTest()
        {
            _network.Add(_interaction12);
            var link = _network[0];
            Assert.IsTrue(link.HasActiveInteraction(_agentId1, _agentId2));
            Assert.IsFalse(link.HasActiveInteraction(_agentId1, _agentId3));
        }
        /// <summary>
        /// Indirect test
        /// </summary>
        [TestMethod]
        public void HasActiveInteractionTest1()
        {
            _network.Add(_interaction12);
            var link = _network[0];
            Assert.IsTrue(link.HasActiveInteraction(_agentId2, _agentId1));
            Assert.IsFalse(link.HasActiveInteraction(_agentId1, _agentId3));
        }
        /// <summary>
        /// Direct test
        /// </summary>
        [TestMethod]
        public void HasPassiveInteractionTest()
        {
            _network.Add(_interaction12);
            var link = _network[0];
            link.DecreaseWeight();
            Assert.IsTrue(link.HasPassiveInteraction(_agentId1, _agentId2));
            Assert.IsFalse(link.HasPassiveInteraction(_agentId1, _agentId3));
        }
        /// <summary>
        /// Indirect test
        /// </summary>
        [TestMethod]
        public void HasPassiveInteractionTest1()
        {
            _network.Add(_interaction12);
            var link = _network[0];
            link.DecreaseWeight();
            Assert.IsTrue(link.HasPassiveInteraction(_agentId2, _agentId1));
            Assert.IsFalse(link.HasPassiveInteraction(_agentId1, _agentId3));
        }

        [TestMethod]
        public void GetActiveInteractionsTest()
        {
            Assert.AreEqual(0, _network.ActiveInteractionCount(_agentId1));
            _network.Add(_interaction12);
            _network.Add(_interaction31);
            var teammateId4 = new AgentId(5, 2);
            var interaction = new ActorActor(_agentId1, teammateId4);
            _network.Add(interaction);
            Assert.AreEqual(3, _network.ActiveInteractionCount(_agentId1));

            // Distinct test
            _network.Add(_interaction12);
            Assert.AreEqual(3, _network.ActiveInteractionCount(_agentId1));
        }

        [TestMethod]
        public void ExistsTest()
        {
            _network.Add(_interaction12);
            Assert.IsTrue(_network.Exists(_agentId1, _agentId2));
            Assert.IsTrue(_network.Exists(_agentId2, _agentId1));
        }


        [TestMethod]
        public void EdgeTest()
        {
            _network.Add(_interaction12);
            Assert.AreEqual(_interaction12, _network.Edge(_agentId1, _agentId2));
            Assert.AreEqual(_interaction12, _network.Edge(_agentId2, _agentId1));
        }

        [TestMethod]
        public void AddTest()
        {
            var link = new ActorActor(_agentId1, _agentId2);
            _network.Add(_interaction12);
            Assert.IsTrue(_network.Exists(link));
            // Deactivate test
            link.DecreaseWeight();
            _network.Add(_interaction12);
            Assert.AreEqual(1, _network.Count);
            Assert.IsTrue(_network[0].IsActive);
        }

        [TestMethod]
        public void WeightTest()
        {
            Assert.AreEqual(0, _network.Weight(_agentId1, _agentId2));
            _network.Add(_interaction12);
            Assert.AreEqual(1, _network.Weight(_agentId1, _agentId2));
            Assert.AreEqual(1, _network.Weight(_agentId2, _agentId1));
            _network.Add(_interaction21);
            Assert.AreEqual(2, _network.Weight(_agentId1, _agentId2));
        }

        [TestMethod]
        public void NormalizedCountLinksTest()
        {
            Assert.AreEqual(0, _network.NormalizedWeight(_agentId1, _agentId2));
            _network.Add(_interaction12);
            _network.NormalizeWeights();
            Assert.AreEqual(1, _network.NormalizedWeight(_agentId1, _agentId2));
            _network.Add(_interaction21);
            _network.NormalizeWeights();
            Assert.AreEqual(1, _network.NormalizedWeight(_agentId1, _agentId2));
        }
    }
}