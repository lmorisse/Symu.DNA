using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.DNA.Networks.OneModeNetworks;
using SymuDNATests.Classes;

namespace SymuDNATests.Networks.OneModeNetworks
{
    [TestClass()]
    public class OneModeNetworkTests
    {
        private readonly TestKnowledge _knowledge =
            new TestKnowledge(1);

        private readonly KnowledgeNetwork _network = new KnowledgeNetwork();
        [TestMethod()]
        public void ClearTest()
        {
            _network.Add(_knowledge);
            _network.Clear();
            Assert.IsFalse(_network.Any());
        }

        [TestMethod()]
        public void AnyTest()
        {
            Assert.IsFalse(_network.Any());
            _network.Add(_knowledge);
            Assert.IsTrue(_network.Any());
        }

        [TestMethod()]
        public void AddTest()
        {
            Assert.IsFalse(_network.Any());
            _network.Add(_knowledge);
            Assert.IsTrue(_network.Any());
            Assert.IsTrue(_network.Exists(_knowledge));
            // Duplicate
            _network.Add(_knowledge);
            Assert.AreEqual(1, _network.List.Count);
        }

        [TestMethod()]
        public void ExistsTest()
        {
            Assert.IsFalse(_network.Exists(_knowledge));
            _network.Add(_knowledge);
            Assert.IsTrue(_network.Exists(_knowledge));
        }

        [TestMethod()]
        public void NextIdentityTest()
        {
            Assert.AreEqual(0, _network.NextIdentity());
            Assert.AreEqual(1, _network.NextIdentity());
        }

        [TestMethod()]
        public void CopyToTest()
        {
            _network.Add(_knowledge);

            var copy = new KnowledgeNetwork();
            _network.CopyTo(copy);
            CollectionAssert.AreEqual(_network.List, copy.List);
        }
    }
}