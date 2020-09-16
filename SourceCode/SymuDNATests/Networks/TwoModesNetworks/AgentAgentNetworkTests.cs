#region Licence

// Description: SymuBiz - SymuTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.Common.Interfaces.Agent;
using Symu.DNA.GraphNetworks.TwoModesNetworks;
using SymuDNATests.Classes;

#endregion


namespace SymuDNATests.Networks.TwoModesNetworks
{
    [TestClass]
    public class AgentAgentNetworkTests
    {
        private readonly AgentId _agentId1 = new AgentId(2, 2);
        private readonly AgentId _agentId2 = new AgentId(3, 2);
        private readonly AgentId _agentId3 = new AgentId(4, 2);
        private readonly ActorActorNetwork _actorActors = new ActorActorNetwork();

        private TestActorActor _interaction12;
        private TestActorActor _interaction21;
        private TestActorActor _interaction31;

        [TestInitialize]
        public void Initialize()
        {
            _interaction12 = new TestActorActor(_agentId1, _agentId2);
            _interaction21 = new TestActorActor(_agentId2, _agentId1);
            _interaction31 = new TestActorActor(_agentId3, _agentId1);
        }

        [TestMethod]
        public void RemoveAgentTest()
        {
            _actorActors.AddInteraction(_interaction12);
            _actorActors.AddInteraction(_interaction31);
            _actorActors.RemoveActor(_agentId1);
            Assert.IsFalse(_actorActors.Any());
        }

        [TestMethod]
        public void AnyTest()
        {
            Assert.IsFalse(_actorActors.Any());
            _actorActors.AddInteraction(_interaction12);
            Assert.IsTrue(_actorActors.Any());
        }

        [TestMethod]
        public void ClearTest()
        {
            _actorActors.AddInteraction(_interaction12);
            _actorActors.AddInteraction(_interaction31);
            _actorActors.Clear();
            Assert.IsFalse(_actorActors.Any());
        }

        [TestMethod]
        public void DeactivateTeammatesLinkTest()
        {
            _actorActors.AddInteraction(_interaction12);
            var link = _actorActors[0];
            // Active link
            Assert.IsTrue(link.IsActive);
            // Deactivate
            _actorActors.DecreaseInteraction(_agentId1, _agentId2);
            Assert.IsFalse(link.IsActive);
            Assert.IsTrue(link.IsPassive);
        }

        [TestMethod]
        public void HasActiveLinkTest()
        {
            _actorActors.AddInteraction(_interaction12);
            var link = _actorActors[0];
            Assert.IsTrue(link.HasActiveInteraction(_agentId1, _agentId2));
            Assert.IsFalse(link.HasActiveInteraction(_agentId1, _agentId3));
        }

        [TestMethod]
        public void HasPassiveLinkTest()
        {
            _actorActors.AddInteraction(_interaction12);
            var link = _actorActors[0];
            link.DecreaseWeight();
            Assert.IsTrue(link.HasPassiveInteraction(_agentId1, _agentId2));
            Assert.IsFalse(link.HasPassiveInteraction(_agentId1, _agentId3));
        }

        [TestMethod]
        public void GetActiveLinksTest()
        {
            Assert.AreEqual(0, new List<IAgentId>(_actorActors.GetActiveInteractions(_agentId1)).Count);
            _actorActors.AddInteraction(_interaction12);
            _actorActors.AddInteraction(_interaction31);
            var teammateId4 = new AgentId(5, 2);
            var interaction = new TestActorActor(_agentId1, teammateId4);
            _actorActors.AddInteraction(interaction);
            Assert.AreEqual(3, new List<IAgentId>(_actorActors.GetActiveInteractions(_agentId1)).Count);

            // Distinct test
            _actorActors.AddInteraction(_interaction12);
            Assert.AreEqual(3, new List<IAgentId>(_actorActors.GetActiveInteractions(_agentId1)).Count);
        }

        [TestMethod]
        public void TeammateExistsTest()
        {
            _actorActors.AddInteraction(_interaction12);
            Assert.IsTrue(_actorActors.Exists(_agentId1, _agentId2));
            Assert.IsTrue(_actorActors.Exists(_agentId2, _agentId1));
        }

        [TestMethod]
        public void ExistsTest()
        {
            var link = new TestActorActor(_agentId1, _agentId2);
            Assert.IsFalse(_actorActors.Exists(link));
            _actorActors.List.Add(link);
            Assert.IsTrue(_actorActors.Exists(link));
        }

        [TestMethod]
        public void AddLinkTest()
        {
            var link = new TestActorActor(_agentId1, _agentId2);
            _actorActors.AddInteraction(_interaction12);
            Assert.IsTrue(_actorActors.Exists(link));
            // Deactivate test
            link.DecreaseWeight();
            _actorActors.AddInteraction(_interaction12);
            Assert.AreEqual(1, _actorActors.List.Count);
            Assert.IsTrue(_actorActors[0].IsActive);
        }

        /// <summary>
        ///     Empty list
        /// </summary>
        [TestMethod]
        public void AddLinksTest()
        {
            var agents = new List<IActorActor>();
            _actorActors.AddInteractions(agents);
            Assert.AreEqual(0, _actorActors.Count);
        }

        /// <summary>
        ///     Empty list
        /// </summary>
        [TestMethod]
        public void AddLinksTest1()
        {
            var agents = new List<IActorActor> {_interaction12, _interaction31};
            _actorActors.AddInteractions(agents);
            Assert.AreEqual(2, _actorActors.Count);
            for (var i = 0; i < 2; i++)
            {
                Assert.AreEqual(1, _actorActors[i].Weight);
            }
        }

        [TestMethod]
        public void GetInteractionWeightTest()
        {
            Assert.AreEqual(0, _actorActors.GetInteractionWeight(_agentId1, _agentId2));
            _actorActors.AddInteraction(_interaction12);
            Assert.AreEqual(1, _actorActors.GetInteractionWeight(_agentId1, _agentId2));
            _actorActors.AddInteraction(_interaction21);
            Assert.AreEqual(2, _actorActors.GetInteractionWeight(_agentId1, _agentId2));
        }

        [TestMethod]
        public void NormalizedCountLinksTest()
        {
            Assert.AreEqual(0, _actorActors.NormalizedCountLinks(_agentId1, _agentId2));
            _actorActors.AddInteraction(_interaction12);
            _actorActors.SetMaxLinksCount();
            Assert.AreEqual(1, _actorActors.NormalizedCountLinks(_agentId1, _agentId2));
            _actorActors.AddInteraction(_interaction21);
            _actorActors.SetMaxLinksCount();
            Assert.AreEqual(1, _actorActors.NormalizedCountLinks(_agentId1, _agentId2));
        }
    }
}