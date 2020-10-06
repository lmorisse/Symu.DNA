#region Licence

// Description: SymuBiz - SymuDNATests
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Collections.Generic;
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
    public class TaskEntityTests
    {
        private readonly IAgentId _agentId = new AgentId(2, 2);
        private readonly MetaNetwork _metaNetwork = new MetaNetwork();
        private TaskEntity _entity;
        private IKnowledge _knowledge;
        private IEntityKnowledge _taskKnowledge;

        [TestInitialize]
        public void Initialize()
        {
            _entity = new TaskEntity(_metaNetwork);
            _knowledge = new KnowledgeEntity(_metaNetwork);
            _taskKnowledge = new EntityKnowledge(_entity.EntityId, _knowledge.EntityId);
        }
        private void TestMetaNetwork(IEntity entity)
        {
            Assert.AreEqual(1, _metaNetwork.TaskKnowledge.EdgesFilteredBySourceCount(entity.EntityId));
            Assert.AreEqual(1, _metaNetwork.ResourceTask.EdgesFilteredByTargetCount(entity.EntityId));
            Assert.AreEqual(1, _metaNetwork.ActorTask.EdgesFilteredByTargetCount(entity.EntityId));
        }
        private void SetMetaNetwork()
        {
            _metaNetwork.TaskKnowledge.Add(new EntityKnowledge(_entity.EntityId, _agentId));
            _metaNetwork.ResourceTask.Add(new ResourceTask(_agentId, _entity.EntityId));
            _metaNetwork.ActorTask.Add(new ActorTask(_agentId, _entity.EntityId));
        }

        [TestMethod]
        public void CloneTest()
        {
            SetMetaNetwork();
            var clone = _entity.Clone() as TaskEntity;
            Assert.IsNotNull(clone);
            Assert.AreEqual(_entity.EntityId, clone.EntityId);
            Assert.AreEqual(_entity.Name, clone.Name);
            TestMetaNetwork(clone);
        }

        [TestMethod]
        public void DuplicateTest()
        {
            SetMetaNetwork();
            var clone = _entity.Duplicate< TaskEntity>();
            Assert.IsNotNull(clone);
            Assert.IsNotNull(_metaNetwork.Task.GetEntity(clone.EntityId));
            Assert.AreNotEqual(_entity.EntityId, clone.EntityId);
            Assert.AreEqual(_entity.Name, clone.Name);
            TestMetaNetwork(clone);
        }

        [TestMethod]
        public void CopyMetaNetworkToTest()
        {
            SetMetaNetwork();
            var task1 = new TaskEntity(_metaNetwork);
            _entity.CopyMetaNetworkTo(task1.EntityId);
            TestMetaNetwork(task1);
        }

        [TestMethod]
        public void RemoveTest()
        {
            SetMetaNetwork();
            _entity.Remove();
            Assert.IsFalse(_metaNetwork.TaskKnowledge.Any());
            Assert.IsFalse(_metaNetwork.ResourceTask.Any());
            Assert.IsFalse(_metaNetwork.ActorTask.Any());
            Assert.IsFalse(_metaNetwork.Task.Any());
        }

        [TestMethod]
        public void AddKnowledgeTest()
        {
            Assert.IsFalse(_entity.Knowledge.Contains(_knowledge));
            _entity.AddKnowledge(_taskKnowledge);
            Assert.AreEqual(1, _entity.Knowledge.Count);
            Assert.IsTrue(_entity.Knowledge.Contains(_knowledge));
            _entity.AddKnowledge(_taskKnowledge); //handle duplicate
            Assert.AreEqual(1, _entity.Knowledge.Count);
        }

        [TestMethod]
        public void CheckKnowledgeIdsNullTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _entity.CheckKnowledgeIds(null));
        }

        /// <summary>
        ///     Without knowledge
        /// </summary>
        [TestMethod]
        public void CheckKnowledgeIdsTest()
        {
            var agentId1 = new AgentId(3, 2);
            var agentId2 = new AgentId(4, 2);
            var list = new List<IAgentId> {agentId1, agentId2};
            Assert.IsFalse(_entity.CheckKnowledgeIds(list));
        }

        /// <summary>
        ///     Non passing test
        /// </summary>
        [TestMethod]
        public void CheckKnowledgeIdsTest1()
        {
            _entity.AddKnowledge(_taskKnowledge);
            var agentId1 = new AgentId(3, 2);
            var agentId2 = new AgentId(4, 2);
            var list = new List<IAgentId> {agentId1, agentId2};
            Assert.IsFalse(_entity.CheckKnowledgeIds(list));
        }

        /// <summary>
        ///     Passing test
        /// </summary>
        [TestMethod]
        public void CheckKnowledgeIdsTest2()
        {
            _entity.AddKnowledge(_taskKnowledge);
            var agentId1 = new AgentId(3, 2);
            var agentId2 = new AgentId(4, 2);
            var list = new List<IAgentId> {_knowledge.EntityId, agentId1, agentId2};
            Assert.IsTrue(_entity.CheckKnowledgeIds(list));
        }
    }
}