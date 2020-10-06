#region Licence

// Description: SymuBiz - SymuDNATests
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.Common.Interfaces;

using Symu.DNA.Edges;
using Symu.DNA.Entities;
using Symu.DNA.GraphNetworks;

#endregion

namespace SymuDNATests.Entities
{
    [TestClass]
    public class KnowledgeEntityTests
    {
        private readonly IAgentId _agentId = new AgentId(2, 2);
        private readonly MetaNetwork _metaNetwork = new MetaNetwork();
        private KnowledgeEntity _entity;

        [TestInitialize]
        public void Initialize()
        {
            _entity = new KnowledgeEntity(_metaNetwork);
        }
        private void TestMetaNetwork(IEntity entity)
        {
            Assert.AreEqual(1, _metaNetwork.ActorKnowledge.EdgesFilteredByTargetCount(entity.EntityId));
            Assert.AreEqual(1, _metaNetwork.TaskKnowledge.EdgesFilteredByTargetCount(entity.EntityId));
            Assert.AreEqual(1, _metaNetwork.ResourceKnowledge.EdgesFilteredByTargetCount(entity.EntityId));
        }
        private void SetMetaNetwork()
        {
            _metaNetwork.ActorKnowledge.Add(new EntityKnowledge(_agentId, _entity.EntityId));
            _metaNetwork.TaskKnowledge.Add(new EntityKnowledge(_agentId, _entity.EntityId));
            _metaNetwork.ResourceKnowledge.Add(new EntityKnowledge(_agentId, _entity.EntityId));
        }

        [TestMethod]
        public void CloneTest()
        {
            SetMetaNetwork();
            var clone = _entity.Clone() as KnowledgeEntity;
            Assert.IsNotNull(clone);
            Assert.AreEqual(_entity.EntityId, clone.EntityId);
            Assert.AreEqual(_entity.Name, clone.Name);
            TestMetaNetwork(clone);
        }

        [TestMethod]
        public void DuplicateTest()
        {
            SetMetaNetwork();
            var clone = _entity.Duplicate< KnowledgeEntity>();
            Assert.IsNotNull(clone);
            Assert.IsNotNull(_metaNetwork.Knowledge.GetEntity(clone.EntityId));
            Assert.AreNotEqual(_entity.EntityId, clone.EntityId);
            Assert.AreEqual(_entity.Name, clone.Name);
            TestMetaNetwork(clone);
        }

        [TestMethod]
        public void CopyMetaNetworkToTest()
        {
            SetMetaNetwork();
            var knowledge1 = new KnowledgeEntity(_metaNetwork);
            _entity.CopyMetaNetworkTo(knowledge1.EntityId);
            TestMetaNetwork(knowledge1);
        }

        [TestMethod]
        public void RemoveTest()
        {
            SetMetaNetwork();
            _entity.Remove();
            Assert.IsFalse(_metaNetwork.ActorKnowledge.Any());
            Assert.IsFalse(_metaNetwork.TaskKnowledge.Any());
            Assert.IsFalse(_metaNetwork.ResourceKnowledge.Any());
            Assert.IsFalse(_metaNetwork.Knowledge.Any());
        }
    }
}