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
    public class RoleEntityTests
    {
        private readonly IAgentId _agentId = new AgentId(2, 2);
        private readonly MetaNetwork _metaNetwork = new MetaNetwork();
        private RoleEntity _entity;

        [TestInitialize]
        public void Initialize()
        {
            _entity = new RoleEntity(_metaNetwork);
        }
        private void TestMetaNetwork(IEntity entity)
        {
            Assert.AreEqual(1, _metaNetwork.ActorRole.EdgesFilteredByTargetCount(entity.EntityId));
        }
        private void SetMetaNetwork()
        {
            _metaNetwork.ActorRole.Add(new ActorRole(_agentId, _entity.EntityId, _agentId));
        }

        [TestMethod]
        public void CloneTest()
        {
            SetMetaNetwork();
            var clone = _entity.Clone() as RoleEntity;
            Assert.IsNotNull(clone);
            Assert.AreEqual(_entity.EntityId, clone.EntityId);
            Assert.AreEqual(_entity.Name, clone.Name);
            TestMetaNetwork(clone);
        }

        [TestMethod]
        public void DuplicateTest()
        {
            SetMetaNetwork();
            var clone = _entity.Duplicate< RoleEntity>();
            Assert.IsNotNull(clone);
            Assert.IsNotNull(_metaNetwork.Role.GetEntity(clone.EntityId));
            Assert.AreNotEqual(_entity.EntityId, clone.EntityId);
            Assert.AreEqual(_entity.Name, clone.Name);
            TestMetaNetwork(clone);
        }

        [TestMethod]
        public void CopyMetaNetworkToTest()
        {
            SetMetaNetwork();
            var role1 = new RoleEntity(_metaNetwork);
            _entity.CopyMetaNetworkTo(role1.EntityId);
            TestMetaNetwork(role1);
        }
        [TestMethod]
        public void RemoveTest()
        {
            SetMetaNetwork();
            _entity.Remove();
            Assert.IsFalse(_metaNetwork.ActorRole.Any());
            Assert.IsFalse(_metaNetwork.Role.Any());
        }
    }
}