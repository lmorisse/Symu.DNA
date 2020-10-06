#region Licence

// Description: SymuBiz - SymuDNATests
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.Common.Interfaces;
using Symu.DNA.Edges;
using Symu.DNA.Entities;
using Symu.DNA.GraphNetworks;

#endregion

namespace SymuDNATests.Entities
{
    [TestClass]
    public class ActorEntityTests
    {
        private readonly IAgentId _agentId = new AgentId(2, 2);
        private readonly MetaNetwork _metaNetwork = new MetaNetwork();
        private ActorEntity _entity;

        [TestInitialize]
        public void Initialize()
        {
            _entity = new ActorEntity(_metaNetwork);
        }
        private void TestMetaNetwork(IEntity entity)
        {
            Assert.AreEqual(1, _metaNetwork.ActorResource.EdgesFilteredBySourceCount(entity.EntityId));
            Assert.AreEqual(1, _metaNetwork.ActorOrganization.EdgesFilteredBySourceCount(entity.EntityId));
            Assert.AreEqual(1, _metaNetwork.ActorTask.EdgesFilteredBySourceCount(entity.EntityId));
            Assert.AreEqual(1, _metaNetwork.ActorActor.EdgesFilteredBySourceCount(entity.EntityId));
            Assert.AreEqual(1, _metaNetwork.ActorBelief.EdgesFilteredBySourceCount(entity.EntityId));
            Assert.AreEqual(1, _metaNetwork.ActorKnowledge.EdgesFilteredBySourceCount(entity.EntityId));
            Assert.AreEqual(1, _metaNetwork.ActorRole.EdgesFilteredBySourceCount(entity.EntityId));
        }
        private void SetMetaNetwork()
        {
            _metaNetwork.ActorResource.Add(new ActorResource(_entity.EntityId, _agentId, new ResourceUsage(1), 1));
            _metaNetwork.ActorOrganization.Add(new ActorOrganization(_entity.EntityId, _agentId));
            _metaNetwork.ActorTask.Add(new ActorTask(_entity.EntityId, _agentId));
            _metaNetwork.ActorActor.Add(new ActorActor(_entity.EntityId, _agentId));
            _metaNetwork.ActorBelief.Add(new ActorBelief(_entity.EntityId, _agentId));
            _metaNetwork.ActorKnowledge.Add(new EntityKnowledge(_entity.EntityId, _agentId));
            _metaNetwork.ActorRole.Add(new ActorRole(_entity.EntityId, _agentId, _agentId));
        }

        [TestMethod]
        public void CloneTest()
        {
            SetMetaNetwork();
            var clone = _entity.Clone() as ActorEntity;
            Assert.IsNotNull(clone);
            Assert.AreEqual(_entity.EntityId, clone.EntityId);
            Assert.AreEqual(_entity.Name, clone.Name);
            TestMetaNetwork(clone);
        }

        [TestMethod]
        public void DuplicateTest()
        {
            SetMetaNetwork();
            var clone = _entity.Duplicate<ActorEntity>();
            Assert.IsNotNull(clone);
            Assert.IsNotNull(_metaNetwork.Actor.GetEntity(clone.EntityId));
            Assert.AreNotEqual(_entity.EntityId, clone.EntityId);
            Assert.AreEqual(_entity.Name, clone.Name);
            TestMetaNetwork(clone);
        }

        [TestMethod]
        public void CopyMetaNetworkToTest()
        {
            SetMetaNetwork();
            var actor1 = new ActorEntity(_metaNetwork);
            _entity.CopyMetaNetworkTo(actor1.EntityId);
            TestMetaNetwork(actor1);
        }

        [TestMethod]
        public void RemoveTest()
        {
            SetMetaNetwork();
            _entity.Remove();

            Assert.IsFalse(_metaNetwork.ActorResource.Any());
            Assert.IsFalse(_metaNetwork.ActorOrganization.Any());
            Assert.IsFalse(_metaNetwork.ActorTask.Any());
            Assert.IsFalse(_metaNetwork.ActorActor.Any());
            Assert.IsFalse(_metaNetwork.ActorBelief.Any());
            Assert.IsFalse(_metaNetwork.ActorKnowledge.Any());
            Assert.IsFalse(_metaNetwork.ActorRole.Any());
            Assert.IsFalse(_metaNetwork.Actor.Any());
        }

        #region Actor * Role
        [TestMethod]
        public void RoleTest()
        {
            Assert.IsFalse(_entity.Roles.Any());
            var actorRole = new ActorRole(_entity.EntityId, _agentId, _agentId);
            _metaNetwork.ActorRole.Add(actorRole);
            Assert.AreEqual(1, _entity.Roles.Count());
            Assert.AreEqual(actorRole, _entity.Roles.First());
        }

        [TestMethod]
        public void AddRoleTest()
        {
            Assert.IsFalse(_entity.Roles.Any());
            _entity.AddRole(_agentId, _agentId);
            Assert.AreEqual(1, _entity.Roles.Count());
        }

        [TestMethod]
        public void IsActorOfOrganizationIdsTest()
        {
            Assert.IsFalse(_entity.IsActorOfOrganizationIds(_agentId.ClassId).Any());
            _entity.AddRole(_agentId, _agentId);
            Assert.AreEqual(1, _entity.IsActorOfOrganizationIds(_agentId.ClassId).Count());
            Assert.AreEqual(_agentId, _entity.IsActorOfOrganizationIds(_agentId.ClassId).First());
        }

        [TestMethod()]
        public void IsMemberTest()
        {
            Assert.IsFalse(_entity.IsMember(_agentId.ClassId));
            _entity.AddRole(_agentId, _agentId);
            Assert.IsTrue(_entity.IsMember(_agentId.ClassId));
        }

        [TestMethod()]
        public void GetOrganizationsTest()
        {
            Assert.IsFalse(_entity.GetOrganizations(_agentId).Any());
            _entity.AddRole(_agentId, _agentId);
            Assert.IsTrue(_entity.GetOrganizations(_agentId).Any());
        }

        [TestMethod()]
        public void HasRolesTest()
        {
            Assert.IsFalse(_entity.HasRoles());
            _entity.AddRole(_agentId, _agentId);
            Assert.IsTrue(_entity.HasRoles());
        }

        [TestMethod()]
        public void HasRoleTest()
        {
            Assert.IsFalse(_entity.HasRole(_agentId));
            _entity.AddRole(_agentId, _agentId);
            Assert.IsTrue(_entity.HasRole(_agentId));
        }

        [TestMethod()]
        public void HasARoleInTest()
        {
            Assert.IsFalse(_entity.HasARoleIn(_agentId));
            _entity.AddRole(_agentId, _agentId);
            Assert.IsTrue(_entity.HasARoleIn(_agentId));
        }

        [TestMethod()]
        public void HasARoleInTest1()
        {
            Assert.IsFalse(_entity.HasARoleIn(_agentId, _agentId));
            _entity.AddRole(_agentId, _agentId);
            Assert.IsTrue(_entity.HasARoleIn(_agentId, _agentId));
        }

        [TestMethod()]
        public void GetRolesInTest()
        {
            Assert.IsFalse(_entity.GetRolesIn(_agentId).Any());
            _entity.AddRole(_agentId, _agentId);
            Assert.IsTrue(_entity.GetRolesIn(_agentId).Any());
        }
        #endregion

        #region Actor * Resource management

        [TestMethod()]
        public void AddResourceTest()
        {
            Assert.IsFalse(_metaNetwork.ActorResource.Any());
            _entity.AddResource(_agentId, new ResourceUsage(1));
            Assert.IsTrue(_metaNetwork.ActorResource.Any());
        }
        #endregion
    }
}