#region Licence

// Description: SymuBiz - SymuTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.Common.Interfaces.Agent;
using Symu.Common.Interfaces.Entity;
using Symu.DNA.Entities;
using Symu.DNA.GraphNetworks.TwoModesNetworks;
using SymuDNATests.Classes;

#endregion

namespace SymuDNATests.Networks.TwoModesNetworks
{
    [TestClass]
    public class AgentKnowledgeNetworkTests
    {
        private readonly AgentId _agentId = new AgentId(1, 1);

        private readonly KnowledgeEntity _knowledge =
            new KnowledgeEntity(1);

        private readonly ActorKnowledgeNetwork _knowledgeNetwork = new ActorKnowledgeNetwork();
        private TestActorKnowledge _actorKnowledge;

        [TestInitialize]
        public void Initialize()
        {
            _actorKnowledge = new TestActorKnowledge(_knowledge.AgentId);
            //_knowledgeNetwork.AddKnowledge(_knowledge);
        }



        [TestMethod]
        public void FilterAgentsWithKnowledgeTest()
        {
            var agentIds = new List<IAgentId>
            {
                _agentId
            };
            // Non passing tests
            var filteredAgents = _knowledgeNetwork.FilterActorsWithKnowledge(agentIds, new UId(0));
            Assert.AreEqual(0, filteredAgents.Count());
            filteredAgents = _knowledgeNetwork.FilterActorsWithKnowledge(agentIds, _knowledge.AgentId);
            Assert.AreEqual(0, filteredAgents.Count());
            // Passing test
            _knowledgeNetwork.Add(_agentId, _actorKnowledge);
            filteredAgents = _knowledgeNetwork.FilterActorsWithKnowledge(agentIds, _knowledge.AgentId);
            Assert.AreEqual(1, filteredAgents.Count());
        }

        [TestMethod]
        public void AddAgentIdTest()
        {
            var agentId2 = new AgentId(2, 1);
            Assert.IsFalse(_knowledgeNetwork.Exists(agentId2));
            _knowledgeNetwork.AddActorId(agentId2);
            Assert.IsTrue(_knowledgeNetwork.Exists(agentId2));
        }

        /// <summary>
        ///     Add expertise
        /// </summary>
        [TestMethod]
        public void AddTest()
        {
            Assert.IsFalse(_knowledgeNetwork.Exists(_agentId));
            Assert.IsFalse(_knowledgeNetwork.Exists(_agentId, _knowledge.AgentId));
            _knowledgeNetwork.Add(_agentId, _actorKnowledge);
            Assert.IsTrue(_knowledgeNetwork.Exists(_agentId));
            Assert.IsTrue(_knowledgeNetwork.Exists(_agentId, _knowledge.AgentId));
        }

        [TestMethod]
        public void AddKnowledgeTest()
        {
            _knowledgeNetwork.AddActorId(_agentId);
            Assert.IsFalse(_knowledgeNetwork.Exists(_agentId, _knowledge.AgentId));
            _knowledgeNetwork.AddActorKnowledge(_agentId, _actorKnowledge);
            Assert.IsTrue(_knowledgeNetwork.Exists(_agentId, _knowledge.AgentId));
        }

        [TestMethod]
        public void GetKnowledgeIdsTest()
        {
            _knowledgeNetwork.AddActorId(_agentId);
            Assert.AreEqual(0, _knowledgeNetwork.GetKnowledgeIds(_agentId).Count());
            _knowledgeNetwork.AddActorKnowledge(_agentId, _actorKnowledge);
            Assert.AreEqual(1, _knowledgeNetwork.GetKnowledgeIds(_agentId).Count());
        }

        [TestMethod]
        public void RemoveAgentTest()
        {
            _knowledgeNetwork.AddActorId(_agentId);
            _knowledgeNetwork.RemoveActor(_agentId);
            Assert.IsFalse(_knowledgeNetwork.Exists(_agentId));
        }

        [TestMethod]
        public void ClearTest()
        {
            _knowledgeNetwork.Add(_agentId, _actorKnowledge);
            _knowledgeNetwork.Clear();
            Assert.IsFalse(_knowledgeNetwork.Any());
        }

        [TestMethod]
        public void ExistsTest()
        {
            Assert.IsFalse(_knowledgeNetwork.Exists(_agentId));
            _knowledgeNetwork.Add(_agentId, _actorKnowledge);
            Assert.IsTrue(_knowledgeNetwork.Exists(_agentId));
        }

        [TestMethod]
        public void ExistsAgentTest()
        {
            Assert.IsFalse(_knowledgeNetwork.Exists(_agentId, _knowledge.AgentId));
            _knowledgeNetwork.Add(_agentId, _actorKnowledge);
            Assert.IsTrue(_knowledgeNetwork.Exists(_agentId, _knowledge.AgentId));
        }

        /// <summary>
        ///     Add knowledge
        /// </summary>
        [TestMethod]
        public void Add1Test()
        {
            Assert.IsFalse(_knowledgeNetwork.Any());
            _knowledgeNetwork.Add(_agentId, _actorKnowledge);
            Assert.IsTrue(_knowledgeNetwork.Any());
        }

        /// <summary>
        ///     Non passing test
        /// </summary>
        [TestMethod]
        public void GetAgentExpertiseTest()
        {
            Assert.ThrowsException<NullReferenceException>(() => _knowledgeNetwork.GetActorExpertise(_agentId));
        }

        /// <summary>
        ///     Passing test
        /// </summary>
        [TestMethod]
        public void GetAgentExpertiseTest1()
        {
            _knowledgeNetwork.Add(_agentId, _actorKnowledge);
            var agentExpertise = _knowledgeNetwork.GetActorExpertise(_agentId);
            Assert.IsNotNull(agentExpertise);
        }

        [TestMethod]
        public void AddTest1()
        {
            var agentExpertise = new ActorExpertise();
            agentExpertise.Add(_actorKnowledge);
            _knowledgeNetwork.Add(_agentId, agentExpertise);
            agentExpertise = _knowledgeNetwork.GetActorExpertise(_agentId);
            Assert.IsNotNull(agentExpertise);
        }

        [TestMethod]
        public void GetAgentKnowledgeTest()
        {
            Assert.ThrowsException<NullReferenceException>(() => _knowledgeNetwork.GetActorKnowledge(_agentId, _knowledge.AgentId));
            _knowledgeNetwork.Add(_agentId, _actorKnowledge);
            Assert.IsNotNull(_knowledgeNetwork.GetActorKnowledge(_agentId, _knowledge.AgentId));
        }

    }
}