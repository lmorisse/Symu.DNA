#region Licence

// Description: SymuBiz - SymuTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
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
    public class AgentBeliefNetworkTests
    {
        private readonly AgentId _agentId = new AgentId(1, 1);
        private readonly BeliefEntity _belief = new BeliefEntity(1);
        private ActorBeliefNetwork _network ;
        private TestActorBelief _actorBelief;

        [TestInitialize]
        public void Initialize()
        {
            _network = new ActorBeliefNetwork();
            _actorBelief = new TestActorBelief(_belief.AgentId);
        }
        
        [TestMethod]
        public void AnyTest()
        {
            Assert.IsFalse(_network.Any());
            _network.Add(_agentId, _actorBelief);
            Assert.IsTrue(_network.Any());
        }

        [TestMethod]
        public void ClearTest()
        {
            _network.AddActorId(_agentId);
            _network.Clear();
            Assert.IsFalse(_network.Any());
        }

        [TestMethod]
        public void AddTest()
        {
            Assert.IsFalse(_network.Exists(_agentId));
            _network.Add(_agentId, _actorBelief);
            Assert.IsTrue(_network.Exists(_agentId));
            Assert.IsNotNull(_network.GetAgentBelief(_agentId, _belief.AgentId));
        }

        [TestMethod]
        public void AddAgentIdTest()
        {
            Assert.IsFalse(_network.Exists(_agentId));
            _network.AddActorId(_agentId);
            Assert.IsTrue(_network.Exists(_agentId));
        }

        [TestMethod]
        public void RemoveAgentTest()
        {
            // no agent Id
            _network.RemoveActor(_agentId);
            _network.Add(_agentId, _actorBelief);
            _network.RemoveActor(_agentId);
            Assert.IsFalse(_network.Any());
        }

        [TestMethod]
        public void NullGetAgentBeliefsTest()
        {
            Assert.ThrowsException<NullReferenceException>(() => _network.GetActorBeliefs(_agentId));
        }

        [TestMethod]
        public void GetAgentBeliefsTest()
        {
            _network.Add(_agentId, _actorBelief);
            var agentBeliefs = _network.GetActorBeliefs(_agentId);
            Assert.AreEqual(1, agentBeliefs.Count);
        }

        [TestMethod]
        public void NullGetAgentBeliefTest()
        {
            Assert.ThrowsException<NullReferenceException>(() => _network.GetAgentBelief(_agentId, _belief.AgentId));
        }

        [TestMethod]
        public void GetAgentBeliefTest()
        {
            _network.Add(_agentId, _actorBelief);
            Assert.IsNull(_network.GetAgentBelief(_agentId, new UId(0)));
            Assert.IsNotNull(_network.GetAgentBelief(_agentId, _belief.AgentId));
        }

        [TestMethod]
        public void GetBeliefIdsTest()
        {
            _network.AddActorId(_agentId);
            Assert.AreEqual(0, _network.GetBeliefIds(_agentId).Count());
            _network.AddActorBelief(_agentId, _actorBelief);
            Assert.AreEqual(1, _network.GetBeliefIds(_agentId).Count());
        }
    }
}