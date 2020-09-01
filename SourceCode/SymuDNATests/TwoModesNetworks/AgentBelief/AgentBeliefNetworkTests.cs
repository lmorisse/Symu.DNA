﻿#region Licence

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
using Symu.DNA.TwoModesNetworks.AgentBelief;
using SymuDNATests.Classes;

#endregion

namespace SymuDNATests.TwoModesNetworks.AgentBelief
{
    [TestClass]
    public class AgentBeliefNetworkTests
    {
        private readonly AgentId _agentId = new AgentId(1, 1);
        private readonly TestBelief _belief = new TestBelief(1);
        private AgentBeliefNetwork _network ;
        private TestAgentBelief _agentBelief;

        [TestInitialize]
        public void Initialize()
        {
            _network = new AgentBeliefNetwork();
            _agentBelief = new TestAgentBelief(_belief.Id);
        }
        
        [TestMethod]
        public void AnyTest()
        {
            Assert.IsFalse(_network.Any());
            _network.Add(_agentId, _agentBelief);
            Assert.IsTrue(_network.Any());
        }

        [TestMethod]
        public void ClearTest()
        {
            _network.AddAgentId(_agentId);
            _network.Clear();
            Assert.IsFalse(_network.Any());
        }

        [TestMethod]
        public void AddTest()
        {
            Assert.IsFalse(_network.Exists(_agentId));
            _network.Add(_agentId, _agentBelief);
            Assert.IsTrue(_network.Exists(_agentId));
            Assert.IsNotNull(_network.GetAgentBelief(_agentId, _belief.Id));
        }

        [TestMethod]
        public void AddAgentIdTest()
        {
            Assert.IsFalse(_network.Exists(_agentId));
            _network.AddAgentId(_agentId);
            Assert.IsTrue(_network.Exists(_agentId));
        }

        [TestMethod]
        public void RemoveAgentTest()
        {
            // no agent Id
            _network.RemoveAgent(_agentId);
            _network.Add(_agentId, _agentBelief);
            _network.RemoveAgent(_agentId);
            Assert.IsFalse(_network.Any());
        }

        [TestMethod]
        public void NullGetAgentBeliefsTest()
        {
            Assert.ThrowsException<NullReferenceException>(() => _network.GetAgentBeliefs(_agentId));
        }

        [TestMethod]
        public void GetAgentBeliefsTest()
        {
            _network.Add(_agentId, _agentBelief);
            var agentBeliefs = _network.GetAgentBeliefs(_agentId);
            Assert.AreEqual(1, agentBeliefs.Count);
        }

        [TestMethod]
        public void NullGetAgentBeliefTest()
        {
            Assert.ThrowsException<NullReferenceException>(() => _network.GetAgentBelief(_agentId, _belief.Id));
        }

        [TestMethod]
        public void GetAgentBeliefTest()
        {
            _network.Add(_agentId, _agentBelief);
            Assert.IsNull(_network.GetAgentBelief(_agentId, new UId(0)));
            Assert.IsNotNull(_network.GetAgentBelief(_agentId, _belief.Id));
        }

        [TestMethod]
        public void GetBeliefIdsTest()
        {
            _network.AddAgentId(_agentId);
            Assert.AreEqual(0, _network.GetBeliefIds(_agentId).Count());
            _network.AddAgentBelief(_agentId, _agentBelief);
            Assert.AreEqual(1, _network.GetBeliefIds(_agentId).Count());
        }
    }
}