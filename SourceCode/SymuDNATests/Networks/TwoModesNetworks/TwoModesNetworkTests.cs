using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.DNA.Networks.TwoModesNetworks;
using SymuDNATests.Classes;

namespace SymuDNATests.Networks.TwoModesNetworks
{
    [TestClass()]
    public class TwoModesNetworkTests
    {
        private readonly TestTask _task = new TestTask(1);
        private readonly TestKnowledge _knowledge = new TestKnowledge(1);
        private readonly TaskKnowledgeNetwork _network = new TaskKnowledgeNetwork();

        [TestMethod()]
        public void AnyTest()
        {
            Assert.IsFalse(_network.Any());
            _network.Add(_task.Id, _knowledge);
            Assert.IsTrue(_network.Any());
        }

        [TestMethod()]
        public void ClearTest()
        {
            _network.Add(_task.Id,_knowledge);
            _network.Clear();
            Assert.IsFalse(_network.Any());
        }

        [TestMethod()]
        public void ExistsTest()
        {
            Assert.IsFalse(_network.Exists(_task.Id));
            _network.Add(_task.Id, _knowledge);
            Assert.IsTrue(_network.Exists(_task.Id));
        }

        [TestMethod()]
        public void AddTest()
        {
            Assert.IsFalse(_network.Exists(_task.Id, _knowledge));
            _network.Add(_task.Id, _knowledge);
            Assert.IsTrue(_network.Exists(_task.Id, _knowledge));
        }

        [TestMethod()]
        public void AddValueTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                _network.AddValue(_task.Id, null));
            Assert.ThrowsException<ArgumentNullException>(() =>
                _network.AddValue(_task.Id, _knowledge));
        }

        [TestMethod()]
        public void AddKeyTest()
        {
            Assert.IsFalse(_network.Exists(_task.Id));
            _network.AddKey(_task.Id);
            Assert.IsTrue(_network.Exists(_task.Id));
        }

        [TestMethod()]
        public void GetValuesCountTest()
        {
            Assert.AreEqual(0, _network.GetValuesCount(_task.Id));
            _network.Add(_task.Id, _knowledge);
            Assert.AreEqual(1, _network.GetValuesCount(_task.Id));
            // Check duplication
            _network.Add(_task.Id, _knowledge);
            Assert.AreEqual(1, _network.GetValuesCount(_task.Id));
        }

        [TestMethod()]
        public void CopyToTest()
        {
            _network.Add(_task.Id, _knowledge);

            var copy = new TaskKnowledgeNetwork();
            _network.CopyTo(copy);
            CollectionAssert.AreNotEqual(_network.List, copy.List);
            Assert.AreEqual(_network.List.Count, copy.List.Count);
        }
    }
}