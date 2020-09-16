using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.DNA.Entities;
using Symu.DNA.GraphNetworks;
using Symu.DNA.GraphNetworks.OneModeNetworks;
using Symu.DNA.GraphNetworks.TwoModesNetworks.Sphere;

namespace SymuDNATests.Networks.OneModeNetworks
{
    [TestClass]
    public class OneModeNetworkTests
    {
        private readonly MetaNetwork _metaNetwork =new MetaNetwork(new InteractionSphereModel());
        private readonly IKnowledge _knowledge =
            new KnowledgeEntity(1);

        private KnowledgeNetwork KnowledgeNetwork => _metaNetwork.Knowledge;

        [TestMethod]
        public void ClearTest()
        {
            KnowledgeNetwork.Add(_knowledge);
            KnowledgeNetwork.Clear();
            Assert.IsFalse(KnowledgeNetwork.Any());
        }

        [TestMethod]
        public void AnyTest()
        {
            Assert.IsFalse(KnowledgeNetwork.Any());
            KnowledgeNetwork.Add(_knowledge);
            Assert.IsTrue(KnowledgeNetwork.Any());
        }

        [TestMethod]
        public void AddTest()
        {
            Assert.IsFalse(KnowledgeNetwork.Any());
            KnowledgeNetwork.Add(_knowledge);
            Assert.IsTrue(KnowledgeNetwork.Any());
            Assert.IsTrue(KnowledgeNetwork.Exists(_knowledge));
            // Duplicate
            KnowledgeNetwork.Add(_knowledge);
            Assert.AreEqual(1, KnowledgeNetwork.List.Count);
        }

        [TestMethod]
        public void ExistsTest()
        {
            Assert.IsFalse(KnowledgeNetwork.Exists(_knowledge));
            KnowledgeNetwork.Add(_knowledge);
            Assert.IsTrue(KnowledgeNetwork.Exists(_knowledge));
        }

        [TestMethod]
        public void NextIdentityTest()
        {
            Assert.AreEqual(0, KnowledgeNetwork.NextId());
            Assert.AreEqual(1, KnowledgeNetwork.NextId());
        }

        [TestMethod]
        public void CopyToTest()
        {
            KnowledgeNetwork.Add(_knowledge);

            var copy = new KnowledgeNetwork();
            KnowledgeNetwork.CopyTo(_metaNetwork, copy);
            CollectionAssert.AreEqual(KnowledgeNetwork.List, copy.List);
        }
    }
}