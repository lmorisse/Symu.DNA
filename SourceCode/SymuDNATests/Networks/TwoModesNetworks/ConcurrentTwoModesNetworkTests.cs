using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.Common.Interfaces.Agent;
using Symu.DNA.Networks.TwoModesNetworks;
using SymuDNATests.Classes;

namespace SymuDNATests.Networks.TwoModesNetworks
{
    [TestClass()]
    public class ConcurrentTwoModesNetworkTests
    {
        private readonly TestTask _task = new TestTask(1);
        private readonly AgentId _agentId = new AgentId(1, 1);
        private readonly AgentId _agentId2 = new AgentId(2, 1);
        private TestAgentTask _agentTask;
        private TestAgentTask _agentTask2;
        private readonly AgentTaskNetwork _network = new AgentTaskNetwork();

        [TestInitialize]
        public void Initialize()
        {
            _agentTask = new TestAgentTask(_agentId, _task);
            _agentTask2 = new TestAgentTask(_agentId2, _task);
        }

        [TestMethod()]
        public void AnyTest()
        {
            Assert.IsFalse(_network.Any());
            _network.Add(_agentId, _agentTask);
            Assert.IsTrue(_network.Any());
        }

        [TestMethod()]
        public void ClearTest()
        {
            _network.Add(_agentId, _agentTask);
            _network.Clear();
            Assert.IsFalse(_network.Any());
        }

        [TestMethod()]
        public void ExistsTest()
        {
            Assert.IsFalse(_network.Exists(_agentId));
            _network.Add(_agentId, _agentTask);
            Assert.IsTrue(_network.Exists(_agentId));
            Assert.IsFalse(_network.Exists(_agentId2));
        }

        [TestMethod()]
        public void AddTest()
        {
            Assert.IsFalse(_network.Exists(_agentId, _agentTask));
            _network.Add(_agentId, _agentTask);
            Assert.IsTrue(_network.Exists(_agentId, _agentTask));
        }

        [TestMethod()]
        public void AddTest1()
        {
            var values = new List<IAgentTask> {_agentTask};
            Assert.IsFalse(_network.Exists(_agentId, _agentTask));
            _network.Add(_agentId, values);
            Assert.IsTrue(_network.Exists(_agentId, _agentTask));
        }

        [TestMethod()]
        public void AddValueTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                _network.AddValue(_agentId, null));
            Assert.ThrowsException<ArgumentNullException>(() =>
                _network.AddValue(_agentId, _agentTask));
        }

        [TestMethod()]
        public void AddKeyTest()
        {
            Assert.IsFalse(_network.Exists(_agentId));
            _network.AddKey(_agentId);
            Assert.IsTrue(_network.Exists(_agentId));
            Assert.IsFalse(_network.Exists(_agentId2));
        }

        [TestMethod()]
        public void GetKeysTest()
        {
            Assert.IsNotNull(_network.GetKeys());
            Assert.AreEqual(0, _network.GetKeys().Count());
            _network.AddKey(_agentId);
            Assert.AreEqual(1, _network.GetKeys().Count());
            _network.AddKey(_agentId2);
            Assert.AreEqual(2, _network.GetKeys().Count());
        }

        [TestMethod()]
        public void GetValuesCountTest()
        {
            Assert.AreEqual(0, _network.GetValuesCount(_agentId));
            _network.Add(_agentId, _agentTask);
            Assert.AreEqual(1, _network.GetValuesCount(_agentId));
            // Check duplication
            _network.Add(_agentId, _agentTask);
            Assert.AreEqual(1, _network.GetValuesCount(_agentId));
            _network.Add(_agentId2, _agentTask2);
            Assert.AreEqual(1, _network.GetValuesCount(_agentId));
        }

        [TestMethod()]
        public void RemoveAgentTest()
        {
            // Without key
            _network.RemoveKey(_agentId);
            // With keys
            _network.AddKey(_agentId);
            _network.AddKey(_agentId2);
            _network.RemoveKey(_agentId);
            Assert.IsFalse(_network.Exists(_agentId));
            Assert.IsTrue(_network.Exists(_agentId2));
        }

        [TestMethod()]
        public void CopyToTest()
        {
            _network.Add(_agentId, _agentTask);

            var copy = new AgentTaskNetwork();
            _network.CopyTo(copy);
            CollectionAssert.AreNotEqual(_network.List, copy.List);
            Assert.AreEqual(_network.List.Count, copy.List.Count);
        }
    }
}