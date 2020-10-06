#region Licence

// Description: SymuBiz - SymuDNATests
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.Common.Interfaces;
using Symu.DNA.Entities;
using Symu.DNA.GraphNetworks;

#endregion

namespace SymuDNATests.GraphNetworks
{
    [TestClass]
    public class OneModeNetworkTests
    {

        private readonly MetaNetwork _metaNetwork = new MetaNetwork();
        private IEntity _entity;
        private IEntity _entity1;

        private OneModeNetwork Network => _metaNetwork.Knowledge;

        [TestInitialize]
        public void Initialize()
        {
            _entity = new KnowledgeEntity(_metaNetwork);
            _entity1 = new KnowledgeEntity(_metaNetwork);
            // constructor add entities to network
            Network.Clear();
        }

        [TestMethod]
        public void ClearTest()
        {
            Network.Add(_entity);
            Network.Clear();
            Assert.IsFalse(Network.Any());
        }

        [TestMethod]
        public void CountTest()
        {
            Assert.AreEqual(0, Network.Count);
            Network.Add(_entity);
            Assert.AreEqual(1, Network.Count);
        }

        [TestMethod]
        public void AnyTest()
        {
            Assert.IsFalse(Network.Any());
            Network.Add(_entity);
            Assert.IsTrue(Network.Any());
        }

        [TestMethod]
        public void ContainsTest()
        {
            Assert.IsFalse(Network.Contains(_entity));
            Network.Add(_entity);
            Assert.IsTrue(Network.Contains(_entity));
        }

        [TestMethod]
        public void ExistsTest()
        {
            Assert.IsFalse(Network.Exists(_entity.EntityId));
            Network.Add(_entity);
            Assert.IsTrue(Network.Exists(_entity.EntityId));
        }

        [TestMethod]
        public void CopyToTest()
        {
            Network.Add(_entity);

            var copy = new OneModeNetwork();
            Network.CopyTo(_metaNetwork, copy);
            CollectionAssert.AreEqual(Network.List, copy.List);
        }

        [TestMethod]
        public void AddTest()
        {
            Assert.IsFalse(Network.Any());
            Network.Add(_entity);
            Assert.IsTrue(Network.Any());
            Assert.IsTrue(Network.Contains(_entity));
            // Duplicate
            Network.Add(_entity);
            Assert.AreEqual(1, Network.List.Count);
        }

        /// <summary>
        ///     Empty List of entities
        /// </summary>
        [TestMethod]
        public void AddTest1()
        {
            Assert.IsFalse(Network.Any());
            Network.Add(new List<IEntity>());
            Assert.IsFalse(Network.Any());
        }

        /// <summary>
        ///     List of entities
        /// </summary>
        [TestMethod]
        public void AddTest2()
        {
            Assert.IsFalse(Network.Any());
            var entities = new List<IEntity> {_entity, _entity1};
            Network.Add(entities);
            Assert.AreEqual(2, Network.Count);
            Assert.IsTrue(Network.Contains(_entity));
            Assert.IsTrue(Network.Contains(_entity1));
        }

        [TestMethod]
        public void GetEntityTest()
        {
            Assert.IsNull(Network.GetEntity(_entity.EntityId));
            Network.Add(_entity);
            Assert.IsNotNull(Network.GetEntity(_entity.EntityId));
        }
        [TestMethod]
        public void GetEntityTest1()
        {
            Assert.IsNull(Network.GetEntity< KnowledgeEntity>(_entity.EntityId));
            Network.Add(_entity);
            var knowledgeEntity = Network.GetEntity<KnowledgeEntity>(_entity.EntityId);
            Assert.IsNotNull(knowledgeEntity);
            Assert.IsInstanceOfType(knowledgeEntity, typeof(KnowledgeEntity));
        }

        [TestMethod]
        public void GetEntitiesTest()
        {
            var entities = new List<IEntity> {_entity, _entity1};
            Network.Add(entities);
            var result = Network.GetEntities().ToList();
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Contains(_entity));
            Assert.IsTrue(result.Contains(_entity1));
        }

        /// <summary>
        ///     Empty Entities
        /// </summary>
        [TestMethod]
        public void GetEntityIdsTest()
        {
            var result = Network.GetEntityIds().ToList();
            Assert.AreEqual(0, result.Count);
        }

        /// <summary>
        ///     Non filtered entities
        /// </summary>
        [TestMethod]
        public void GetEntityIdsTest1()
        {
            var entities = new List<IEntity> {_entity, _entity1};
            Network.Add(entities);
            var result = Network.GetEntityIds().ToList();
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Contains(_entity.EntityId));
            Assert.IsTrue(result.Contains(_entity1.EntityId));
        }

        /// <summary>
        ///     Filtered entities
        /// </summary>
        [TestMethod]
        public void GetEntityIdsTest2()
        {
            var entities = new List<IEntity> {_entity, _entity1};
            Network.Add(entities);
            var result = Network.GetEntityIds<IKnowledge>().ToList();
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Contains(_entity.EntityId));
            Assert.IsTrue(result.Contains(_entity1.EntityId));
        }

        [TestMethod]
        public void RemoveTest()
        {
            Network.Add(_entity);
            Network.Remove(_entity);
            Assert.IsFalse(Network.Any());
            Assert.IsFalse(Network.Contains(_entity));
        }

        [TestMethod]
        public void RemoveTest1()
        {
            Network.Add(_entity);
            Network.Remove(_entity.EntityId);
            Assert.IsFalse(Network.Any());
            Assert.IsFalse(Network.Contains(_entity));
        }

        [TestMethod]
        public void CountByClassIdTest()
        {
            Assert.AreEqual(0, Network.CountByClassId(_entity.EntityId.ClassId));
            var entities = new List<IEntity> {_entity, _entity1};
            Network.Add(entities);
            Assert.AreEqual(2, Network.CountByClassId(_entity.EntityId.ClassId));
        }

        [TestMethod]
        public void FilteredIdsByClassIdTest()
        {
            var entities = new List<IEntity> {_entity, _entity1};
            Network.Add(entities);
            var result = Network.FilteredIdsByClassId(_entity.EntityId.ClassId).ToList();
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(_entity.EntityId, result.First());
        }

        [TestMethod]
        public void FilteredByClassIdTest()
        {
            var entities = new List<IEntity> {_entity, _entity1};
            Network.Add(entities);
            var result = Network.FilteredByClassId(_entity.EntityId.ClassId).ToList();
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(_entity, result.First());
        }

        [TestMethod]
        public void ToVectorTest()
        {
            var entities = new List<IEntity> {_entity, _entity1};
            Network.Add(entities);
            var result = Network.ToVector();
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(_entity.EntityId, result.First());
        }
    }
}