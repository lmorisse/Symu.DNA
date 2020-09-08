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
using Symu.DNA.Networks.TwoModesNetworks;
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
        private readonly AgentAgentNetwork _agentAgents = new AgentAgentNetwork();

        private TestAgentAgent _interaction12;
        private TestAgentAgent _interaction21;
        private TestAgentAgent _interaction31;

        [TestInitialize]
        public void Initialize()
        {
            _interaction12 = new TestAgentAgent(_agentId1, _agentId2);
            _interaction21 = new TestAgentAgent(_agentId2, _agentId1);
            _interaction31 = new TestAgentAgent(_agentId3, _agentId1);
        }

        [TestMethod]
        public void RemoveAgentTest()
        {
            _agentAgents.AddInteraction(_interaction12);
            _agentAgents.AddInteraction(_interaction31);
            _agentAgents.RemoveAgent(_agentId1);
            Assert.IsFalse(_agentAgents.Any());
        }

        [TestMethod]
        public void AnyTest()
        {
            Assert.IsFalse(_agentAgents.Any());
            _agentAgents.AddInteraction(_interaction12);
            Assert.IsTrue(_agentAgents.Any());
        }

        [TestMethod]
        public void ClearTest()
        {
            _agentAgents.AddInteraction(_interaction12);
            _agentAgents.AddInteraction(_interaction31);
            _agentAgents.Clear();
            Assert.IsFalse(_agentAgents.Any());
        }

        [TestMethod]
        public void DeactivateTeammatesLinkTest()
        {
            _agentAgents.AddInteraction(_interaction12);
            var link = _agentAgents[0];
            // Active link
            Assert.IsTrue(link.IsActive);
            // Deactivate
            _agentAgents.DecreaseInteraction(_agentId1, _agentId2);
            Assert.IsFalse(link.IsActive);
            Assert.IsTrue(link.IsPassive);
        }

        [TestMethod]
        public void HasActiveLinkTest()
        {
            _agentAgents.AddInteraction(_interaction12);
            var link = _agentAgents[0];
            Assert.IsTrue(link.HasActiveInteraction(_agentId1, _agentId2));
            Assert.IsFalse(link.HasActiveInteraction(_agentId1, _agentId3));
        }

        [TestMethod]
        public void HasPassiveLinkTest()
        {
            _agentAgents.AddInteraction(_interaction12);
            var link = _agentAgents[0];
            link.DecreaseWeight();
            Assert.IsTrue(link.HasPassiveInteraction(_agentId1, _agentId2));
            Assert.IsFalse(link.HasPassiveInteraction(_agentId1, _agentId3));
        }

        [TestMethod]
        public void GetActiveLinksTest()
        {
            Assert.AreEqual(0, new List<IAgentId>(_agentAgents.GetActiveInteractions(_agentId1)).Count);
            _agentAgents.AddInteraction(_interaction12);
            _agentAgents.AddInteraction(_interaction31);
            var teammateId4 = new AgentId(5, 2);
            var interaction = new TestAgentAgent(_agentId1, teammateId4);
            _agentAgents.AddInteraction(interaction);
            Assert.AreEqual(3, new List<IAgentId>(_agentAgents.GetActiveInteractions(_agentId1)).Count);

            // Distinct test
            _agentAgents.AddInteraction(_interaction12);
            Assert.AreEqual(3, new List<IAgentId>(_agentAgents.GetActiveInteractions(_agentId1)).Count);
        }

        [TestMethod]
        public void TeammateExistsTest()
        {
            _agentAgents.AddInteraction(_interaction12);
            Assert.IsTrue(_agentAgents.Exists(_agentId1, _agentId2));
            Assert.IsTrue(_agentAgents.Exists(_agentId2, _agentId1));
        }

        [TestMethod]
        public void ExistsTest()
        {
            var link = new TestAgentAgent(_agentId1, _agentId2);
            Assert.IsFalse(_agentAgents.Exists(link));
            _agentAgents.List.Add(link);
            Assert.IsTrue(_agentAgents.Exists(link));
        }

        [TestMethod]
        public void AddLinkTest()
        {
            var link = new TestAgentAgent(_agentId1, _agentId2);
            _agentAgents.AddInteraction(_interaction12);
            Assert.IsTrue(_agentAgents.Exists(link));
            // Deactivate test
            link.DecreaseWeight();
            _agentAgents.AddInteraction(_interaction12);
            Assert.AreEqual(1, _agentAgents.List.Count);
            Assert.IsTrue(_agentAgents[0].IsActive);
        }

        /// <summary>
        ///     Empty list
        /// </summary>
        [TestMethod]
        public void AddLinksTest()
        {
            var agents = new List<IAgentAgent>();
            _agentAgents.AddInteractions(agents);
            Assert.AreEqual(0, _agentAgents.Count);
        }

        /// <summary>
        ///     Empty list
        /// </summary>
        [TestMethod]
        public void AddLinksTest1()
        {
            var agents = new List<IAgentAgent> {_interaction12, _interaction31};
            _agentAgents.AddInteractions(agents);
            Assert.AreEqual(2, _agentAgents.Count);
            for (var i = 0; i < 2; i++)
            {
                Assert.AreEqual(1, _agentAgents[i].Weight);
            }
        }

        [TestMethod]
        public void GetInteractionWeightTest()
        {
            Assert.AreEqual(0, _agentAgents.GetInteractionWeight(_agentId1, _agentId2));
            _agentAgents.AddInteraction(_interaction12);
            Assert.AreEqual(1, _agentAgents.GetInteractionWeight(_agentId1, _agentId2));
            _agentAgents.AddInteraction(_interaction21);
            Assert.AreEqual(2, _agentAgents.GetInteractionWeight(_agentId1, _agentId2));
        }

        [TestMethod]
        public void NormalizedCountLinksTest()
        {
            Assert.AreEqual(0, _agentAgents.NormalizedCountLinks(_agentId1, _agentId2));
            _agentAgents.AddInteraction(_interaction12);
            _agentAgents.SetMaxLinksCount();
            Assert.AreEqual(1, _agentAgents.NormalizedCountLinks(_agentId1, _agentId2));
            _agentAgents.AddInteraction(_interaction21);
            _agentAgents.SetMaxLinksCount();
            Assert.AreEqual(1, _agentAgents.NormalizedCountLinks(_agentId1, _agentId2));
        }
    }
}