using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.DNA.Entities;
using Symu.DNA.GraphNetworks.TwoModesNetworks;

namespace SymuDNATests.Networks.TwoModesNetworks
{
    [TestClass]
    public class TwoModesNetworkTests
    {
        private readonly ITask _task = new TaskEntity(1);
        private readonly IKnowledge _knowledge = new KnowledgeEntity(1);
        private readonly TaskKnowledgeNetwork _network = new TaskKnowledgeNetwork();

        [TestMethod]
        public void AnyTest()
        {
            Assert.IsFalse(_network.Any());
            _network.Add(_task.EntityId, _knowledge);
            Assert.IsTrue(_network.Any());
        }

        [TestMethod]
        public void ClearTest()
        {
            _network.Add(_task.EntityId,_knowledge);
            _network.Clear();
            Assert.IsFalse(_network.Any());
        }

        [TestMethod]
        public void ExistsTest()
        {
            Assert.IsFalse(_network.Exists(_task.EntityId));
            _network.Add(_task.EntityId, _knowledge);
            Assert.IsTrue(_network.Exists(_task.EntityId));
        }

        [TestMethod]
        public void AddTest()
        {
            Assert.IsFalse(_network.Exists(_task.EntityId, _knowledge));
            _network.Add(_task.EntityId, _knowledge);
            Assert.IsTrue(_network.Exists(_task.EntityId, _knowledge));
        }

        [TestMethod]
        public void AddValueTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                _network.AddValue(_task.EntityId, null));
            Assert.ThrowsException<ArgumentNullException>(() =>
                _network.AddValue(_task.EntityId, _knowledge));
        }

        [TestMethod]
        public void AddKeyTest()
        {
            Assert.IsFalse(_network.Exists(_task.EntityId));
            _network.AddKey(_task.EntityId);
            Assert.IsTrue(_network.Exists(_task.EntityId));
        }

        [TestMethod]
        public void GetValuesCountTest()
        {
            Assert.AreEqual(0, _network.GetValuesCount(_task.EntityId));
            _network.Add(_task.EntityId, _knowledge);
            Assert.AreEqual(1, _network.GetValuesCount(_task.EntityId));
            // Check duplication
            _network.Add(_task.EntityId, _knowledge);
            Assert.AreEqual(1, _network.GetValuesCount(_task.EntityId));
        }

        [TestMethod]
        public void CopyToTest()
        {
            _network.Add(_task.EntityId, _knowledge);

            var copy = new TaskKnowledgeNetwork();
            _network.CopyTo(copy);
            CollectionAssert.AreNotEqual(_network.List, copy.List);
            Assert.AreEqual(_network.List.Count, copy.List.Count);
        }


        [TestMethod]
        public void RemoveTest()
        {
            _network.Remove(_task.EntityId);
            _network.Add(_task.EntityId, _knowledge);
            _network.Remove(_task.EntityId);
            Assert.IsFalse(_network.Any());
            Assert.IsFalse(_network.Exists(_task.EntityId));
        }
    }
}