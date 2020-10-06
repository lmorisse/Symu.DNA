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
    public class OrganizationEntityTests
    {
        private readonly MetaNetwork _metaNetwork = new MetaNetwork();
        private OrganizationEntity _entity;

        [TestInitialize]
        public void Initialize()
        {
            _entity = new OrganizationEntity(_metaNetwork);
        }
        private void TestMetaNetwork(IEntity entity)
        {
            Assert.AreEqual(1, _metaNetwork.ActorOrganization.EdgesFilteredByTargetCount(entity.EntityId));
            Assert.AreEqual(1, _metaNetwork.OrganizationResource.EdgesFilteredBySourceCount(entity.EntityId));
        }
        private void SetMetaNetwork()
        {
            AddActorToTeam();
            var resourceId = new AgentId(0, ResourceEntity.ClassId);
            _metaNetwork.OrganizationResource.Add(new OrganizationResource(_entity.EntityId, resourceId,
                new ResourceUsage(1), 1));
        }

        [TestMethod]
        public void CloneTest()
        {
            SetMetaNetwork();
            var clone = _entity.Clone() as OrganizationEntity;
            Assert.IsNotNull(clone);
            Assert.AreEqual(_entity.EntityId, clone.EntityId);
            Assert.AreEqual(_entity.Name, clone.Name);
            TestMetaNetwork(clone);
        }

        [TestMethod]
        public void DuplicateTest()
        {
            SetMetaNetwork();
            var clone = _entity.Duplicate< OrganizationEntity>();
            Assert.IsNotNull(clone);
            Assert.IsNotNull(_metaNetwork.Organization.GetEntity(clone.EntityId));
            Assert.AreNotEqual(_entity.EntityId, clone.EntityId);
            Assert.AreEqual(_entity.Name, clone.Name);
            TestMetaNetwork(clone);
        }

        [TestMethod]
        public void CopyMetaNetworkToTest()
        {
            SetMetaNetwork();
            var organization1 = new OrganizationEntity(_metaNetwork);
            _entity.CopyMetaNetworkTo(organization1.EntityId);
            TestMetaNetwork(organization1);
        }

        [TestMethod]
        public void RemoveTest()
        {   
            SetMetaNetwork();
            _entity.Remove();
            Assert.IsFalse(_metaNetwork.ActorOrganization.Any());
            Assert.IsFalse(_metaNetwork.OrganizationResource.Any());
            Assert.IsFalse(_metaNetwork.Organization.Any());
        }

        #region Actors management
        [TestMethod()]
        public void ActorsCountTest()
        {
            Assert.AreEqual(0, _entity.ActorsCount);
            AddActorToTeam();
            Assert.AreEqual(1, _entity.ActorsCount);
        }
        [TestMethod()]
        public void ActorIdsTest()
        {
            Assert.IsFalse(_entity.ActorIds.Any());
            var actorEntity = AddActorToTeam();
            Assert.IsTrue(_entity.ActorIds.Any());
            Assert.AreEqual(actorEntity.EntityId, _entity.ActorIds.First());
        }
        [TestMethod()]
        public void ActorsTest()
        {
            Assert.IsFalse(_entity.Actors.Any());
            var actorEntity = AddActorToTeam();
            Assert.IsTrue(_entity.Actors.Any());
            Assert.AreEqual(actorEntity, _entity.Actors.First());
        }

        private ActorEntity AddActorToTeam()
        {
            var actorEntity = new ActorEntity(_metaNetwork);
            _metaNetwork.Actor.Add(actorEntity);
            _metaNetwork.ActorOrganization.Add(new ActorOrganization(actorEntity.EntityId, _entity.EntityId));
            return actorEntity;
        }

        [TestMethod]
        public void GetFirstActorIdTest()
        {
            Assert.IsTrue(_entity.GetFirstActorId.IsNull);
            var actorEntity = AddActorToTeam();
            Assert.IsFalse(_entity.GetFirstActorId.IsNull);
            Assert.AreEqual(actorEntity.EntityId, _entity.GetFirstActorId);
        }
        #endregion
    }
}