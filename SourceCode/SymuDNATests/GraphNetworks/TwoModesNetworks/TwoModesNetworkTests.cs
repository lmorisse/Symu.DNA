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
using Symu.DNA.Edges;
using Symu.DNA.Entities;
using Symu.DNA.GraphNetworks;
using Symu.DNA.GraphNetworks.TwoModesNetworks;
using Symu.DNA.MatrixNetworks;

#endregion


namespace SymuDNATests.GraphNetworks.TwoModesNetworks
{
    [TestClass]
    public class TwoModesNetworkTests
    {
        private readonly MetaNetwork _metaNetwork = new MetaNetwork();
        private readonly TwoModesNetwork<IEdge> _network = new TwoModesNetwork<IEdge>();
        private IActor _actor;
        private IEdge _edge;
        private IEdge _edge1;
        private IKnowledge _knowledge;
        private IKnowledge _knowledge1;

        [TestInitialize]
        public void Initialize()
        {
            _knowledge = new KnowledgeEntity(_metaNetwork);
            _knowledge1 = new KnowledgeEntity(_metaNetwork);
            _actor = new ActorEntity(_metaNetwork);
            _edge = new EntityKnowledge(_actor.EntityId, _knowledge.EntityId);
            _edge1 = new EntityKnowledge(_actor.EntityId, _knowledge1.EntityId);
        }

        [TestMethod]
        public void AnyTest()
        {
            Assert.IsFalse(_network.Any());
            _network.Add(_edge);
            Assert.IsTrue(_network.Any());
        }

        [TestMethod]
        public void ClearTest()
        {
            _network.Add(_edge);
            _network.Clear();
            Assert.IsFalse(_network.Any());
        }

        [TestMethod]
        public void AddTest()
        {
            Assert.IsFalse(_network.Exists(_edge));
            _network.Add(_edge);
            Assert.IsTrue(_network.Exists(_edge));
            // Duplicate
            _network.Add(_edge);
            Assert.AreEqual(1, _network.Count);
        }

        /// <summary>
        ///     Empty list of edges
        /// </summary>
        [TestMethod]
        public void AddTest1()
        {
            _network.Add(new List<IEdge>());
            Assert.AreEqual(0, _network.Count);
        }

        /// <summary>
        ///     List of edges
        /// </summary>
        [TestMethod]
        public void AddTest2()
        {
            var edges = new List<IEdge> {_edge, _edge1};
            _network.Add(edges);
            Assert.AreEqual(2, _network.Count);
            Assert.IsTrue(_network.Exists(_edge));
            Assert.IsTrue(_network.Exists(_edge1));
        }

        [TestMethod]
        public void CopyToTest()
        {
            _network.Add(_edge);

            var copy = new TwoModesNetwork<IEdge>();
            _network.CopyTo(copy);
            CollectionAssert.AreEqual(_network.Edges().ToList(), copy.Edges().ToList());
        }

        [TestMethod]
        public void ExistsSourceTest()
        {
            Assert.IsFalse(_network.ExistsSource(_edge.Source));
            _network.Add(_edge);
            Assert.IsTrue(_network.ExistsSource(_edge.Source));
        }

        [TestMethod]
        public void ExistsTargetTest()
        {
            Assert.IsFalse(_network.ExistsTarget(_edge.Target));
            _network.Add(_edge);
            Assert.IsTrue(_network.ExistsTarget(_edge.Target));
        }

        [TestMethod]
        public void ExistsTest()
        {
            Assert.IsFalse(_network.Exists(_edge));
            _network.Add(_edge);
            Assert.IsTrue(_network.Exists(_edge));
        }

        [TestMethod]
        public void ExistsTest1()
        {
            Assert.IsFalse(_network.Exists(_edge.Source, _edge.Target));
            _network.Add(_edge);
            Assert.IsTrue(_network.Exists(_edge.Source, _edge.Target));
            Assert.IsFalse(_network.Exists(_edge.Target, _edge.Source));
        }

        [TestMethod]
        public void ExistsTest2()
        {
            var actor1 = new ActorEntity(_metaNetwork);
            var edge = new EntityKnowledge(_actor.EntityId, _knowledge.EntityId);
            var edge1 = new EntityKnowledge(actor1.EntityId, _knowledge1.EntityId);
            _network.Add(edge);
            _network.Add(edge1);
            Assert.IsFalse(_network.Exists(_actor.EntityId, _knowledge1.EntityId));
            Assert.IsFalse(_network.Exists(actor1.EntityId, _knowledge.EntityId));
        }

        [TestMethod]
        public void EdgeTest()
        {
            Assert.IsNull(_network.Edge(_edge.Source, _edge.Target));
            _network.Add(_edge);
            Assert.AreEqual(_edge, _network.Edge(_edge.Source, _edge.Target));
        }

        [TestMethod]
        public void EdgeTest1()
        {
            Assert.IsNull(_network.Edge(_edge));
            _network.Add(_edge);
            Assert.AreEqual(_edge, _network.Edge(_edge));
        }

        [TestMethod]
        public void EdgesTest()
        {
            var edges = new List<IEdge> {_edge, _edge1};
            _network.Add(edges);
            var result = _network.Edges().ToList();
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Contains(_edge));
            Assert.IsTrue(result.Contains(_edge1));
        }

        [TestMethod]
        public void EdgesTest1()
        {
            var edges = new List<IEdge> {_edge, _edge1};
            _network.Add(edges);
            var result = _network.Edges(_edge.Source, _edge.Target).ToList();
            Assert.AreEqual(1, result.Count);
            Assert.IsTrue(result.Contains(_edge));
            Assert.IsFalse(result.Contains(_edge1));
        }

        [TestMethod]
        public void EdgesFilteredByTargetClassIdTest()
        {
            var edges = new List<IEdge> {_edge, _edge1};
            _network.Add(edges);
            var result = _network.EdgesFilteredBySourceAndTargetClassId(_edge.Source, _edge.Target.ClassId).ToList();
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Contains(_edge));
            Assert.IsTrue(result.Contains(_edge1));
        }

        [TestMethod]
        public void EdgesFilteredBySourceClassIdTest()
        {
            var edges = new List<IEdge> {_edge, _edge1};
            _network.Add(edges);
            var result = _network.EdgesFilteredByTargetAndSourceClassId(_edge.Target, _edge.Source.ClassId).ToList();
            Assert.AreEqual(1, result.Count);
            Assert.IsTrue(result.Contains(_edge));
            Assert.IsFalse(result.Contains(_edge1));
        }

        [TestMethod]
        public void EdgesFilteredBySourceTest()
        {
            var edges = new List<IEdge> {_edge, _edge1};
            _network.Add(edges);
            var result = _network.EdgesFilteredBySource(_actor.EntityId).ToList();
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Contains(_edge));
            Assert.IsTrue(result.Contains(_edge1));
        }

        [TestMethod]
        public void EdgesFilteredByTargetTest()
        {
            var edges = new List<IEdge> {_edge, _edge1};
            _network.Add(edges);
            var result = _network.EdgesFilteredByTarget(_edge.Target).ToList();
            Assert.AreEqual(1, result.Count);
            Assert.IsTrue(result.Contains(_edge));
            Assert.IsFalse(result.Contains(_edge1));
        }

        [TestMethod]
        public void EdgesFilteredBySourceCountTest()
        {
            var edges = new List<IEdge> {_edge, _edge1};
            _network.Add(edges);
            Assert.AreEqual(2, _network.EdgesFilteredBySourceCount(_actor.EntityId));
        }

        [TestMethod]
        public void EdgesFilteredByTargetCountTest()
        {
            var edges = new List<IEdge> {_edge, _edge1};
            _network.Add(edges);
            Assert.AreEqual(1, _network.EdgesFilteredByTargetCount(_edge.Target));
        }

        [TestMethod]
        public void TargetsFilteredBySourceTest()
        {
            var edges = new List<IEdge> {_edge, _edge1};
            _network.Add(edges);
            var result = _network.TargetsFilteredBySource(_actor.EntityId).ToList();
            Assert.AreEqual(2, result.Count);
            Assert.IsFalse(result.Contains(_edge.Source));
            Assert.IsTrue(result.Contains(_edge.Target));
            Assert.IsTrue(result.Contains(_edge1.Target));
        }

        [TestMethod]
        public void TargetsFilteredByTargetClassIdTest()
        {
            var edges = new List<IEdge> {_edge, _edge1};
            _network.Add(edges);
            var result = _network.TargetsFilteredByTargetClassId(_edge.Target.ClassId).ToList();
            Assert.AreEqual(2, result.Count);
            Assert.IsFalse(result.Contains(_edge.Source));
            Assert.IsTrue(result.Contains(_edge.Target));
            Assert.IsTrue(result.Contains(_edge1.Target));
        }

        [TestMethod]
        public void TargetsFilteredBySourceAndTargetClassIdTest()
        {
            var edges = new List<IEdge> {_edge, _edge1};
            _network.Add(edges);
            var result = _network.TargetsFilteredBySourceAndTargetClassId(_edge.Source, _edge.Target.ClassId).ToList();
            Assert.AreEqual(2, result.Count);
            Assert.IsFalse(result.Contains(_edge.Source));
            Assert.IsTrue(result.Contains(_edge.Target));
            Assert.IsTrue(result.Contains(_edge1.Target));
        }

        [TestMethod]
        public void TargetsTest()
        {
            var edges = new List<IEdge> {_edge, _edge1};
            _network.Add(edges);
            var result = _network.Targets().ToList();
            Assert.AreEqual(2, result.Count);
            Assert.IsFalse(result.Contains(_edge.Source));
            Assert.IsTrue(result.Contains(_edge.Target));
            Assert.IsTrue(result.Contains(_edge1.Target));
        }

        [TestMethod]
        public void SourcesTest()
        {
            var edges = new List<IEdge> {_edge, _edge1};
            _network.Add(edges);
            var result = _network.Sources().ToList();
            Assert.AreEqual(1, result.Count);
            Assert.IsTrue(result.Contains(_edge.Source));
            Assert.IsFalse(result.Contains(_edge.Target));
            Assert.IsTrue(result.Contains(_edge1.Source));
        }

        [TestMethod]
        public void SourcesFilteredByTargetTest()
        {
            var edges = new List<IEdge> {_edge, _edge1};
            _network.Add(edges);
            var result = _network.SourcesFilteredByTarget(_edge.Target).ToList();
            Assert.AreEqual(1, result.Count);
            Assert.IsTrue(result.Contains(_edge.Source));
            Assert.IsFalse(result.Contains(_edge.Target));
            Assert.IsFalse(result.Contains(_edge1.Target));
        }

        [TestMethod]
        public void SourcesFilteredByTargetAndSourceClassIdTest()
        {
            var edges = new List<IEdge> {_edge, _edge1};
            _network.Add(edges);
            var result = _network.SourcesFilteredByTargetAndSourceClassId(_edge.Target, _edge.Source.ClassId).ToList();
            Assert.AreEqual(1, result.Count);
            Assert.IsTrue(result.Contains(_edge.Source));
            Assert.IsFalse(result.Contains(_edge.Target));
            Assert.IsFalse(result.Contains(_edge1.Target));
        }

        [TestMethod]
        public void SourcesFilteredByTargetAndSourceClassIdCountTest()
        {
            var edges = new List<IEdge> {_edge, _edge1};
            _network.Add(edges);
            Assert.AreEqual(1,
                _network.SourcesFilteredByTargetAndSourceClassIdCount(_edge.Target, _edge.Source.ClassId));
        }

        [TestMethod]
        public void WeightTest()
        {
            var edges = new List<IEdge> {_edge, _edge1};
            _network.Add(edges);
            Assert.AreEqual(1, _network.Weight(_edge.Source, _edge.Target));
        }

        [TestMethod]
        public void WeightTest1()
        {
            var edges = new List<IEdge> {_edge, _edge1};
            _network.Add(edges);
            Assert.AreEqual(1, _network.Weight(_edge));
        }

        [TestMethod]
        public void WeightFilteredBySourceTest()
        {
            var edges = new List<IEdge> {_edge, _edge1};
            _network.Add(edges);
            Assert.AreEqual(2, _network.WeightFilteredBySource(_actor.EntityId));
        }

        [TestMethod]
        public void WeightFilteredByTargetTest()
        {
            var edges = new List<IEdge> {_edge, _edge1};
            _network.Add(edges);
            Assert.AreEqual(1, _network.WeightFilteredByTarget(_edge.Target));
        }

        [TestMethod]
        public void RemoveTest()
        {
            _network.Add(_edge);
            _network.Remove(_edge.Source, _edge.Target);
            Assert.IsFalse(_network.Any());
        }

        [TestMethod]
        public void RemoveTest1()
        {
            _network.Remove(_edge);
            var edges = new List<IEdge> {_edge, _edge1};
            _network.Add(edges);
            _network.Remove(_edge);
            Assert.IsFalse(_network.Exists(_edge));
            Assert.IsTrue(_network.Exists(_edge1));
        }

        [TestMethod]
        public void RemoveSourceTest()
        {
            _network.Remove(_edge);
            var edges = new List<IEdge> {_edge, _edge1};
            _network.Add(edges);
            _network.RemoveSource(_actor.EntityId);
            Assert.IsFalse(_network.Exists(_edge));
            Assert.IsFalse(_network.Exists(_edge1));
        }

        [TestMethod]
        public void RemoveTargetTest()
        {
            _network.Remove(_edge);
            var edges = new List<IEdge> {_edge, _edge1};
            _network.Add(edges);
            _network.RemoveTarget(_edge.Target);
            Assert.IsFalse(_network.Exists(_edge));
            Assert.IsTrue(_network.Exists(_edge1));
        }

        [TestMethod]
        public void ToMatrixTest()
        {
            var edges = new List<IEdge> {_edge, _edge1};
            _network.Add(edges);
            var sourceIds = new VectorNetwork(_network.Sources().ToList());
            var targetIds = new VectorNetwork(_network.Targets().ToList());
            var matrix = _network.ToMatrix(sourceIds, targetIds);
            Assert.AreEqual(1, matrix.RowCount);
            Assert.AreEqual(2, matrix.ColumnCount);
        }

        [TestMethod()]
        public void CopyToTest1()
        {
            _network.Add(_edge);
            _network.CopyTo(_actor.EntityId, _knowledge.EntityId, _knowledge1.EntityId);

            Assert.IsTrue(_network.Exists(_actor.EntityId, _knowledge.EntityId));
            Assert.IsTrue(_network.Exists(_actor.EntityId, _knowledge1.EntityId));
        }

        [TestMethod()]
        public void CopyToFromTargetTest()
        {
            _network.Add(_edge);
            _network.CopyToFromTarget(_knowledge.EntityId, _knowledge1.EntityId);

            Assert.IsTrue(_network.Exists(_actor.EntityId, _knowledge.EntityId));
            Assert.IsTrue(_network.Exists(_actor.EntityId, _knowledge1.EntityId));
        }

        [TestMethod()]
        public void CopyToFromSourceTest()
        {
            var actor1 = new ActorEntity(_metaNetwork);
            _network.Add(_edge);
            _network.CopyToFromSource(_actor.EntityId, actor1.EntityId);

            Assert.IsTrue(_network.Exists(_actor.EntityId, _knowledge.EntityId));
            Assert.IsTrue(_network.Exists(actor1.EntityId, _knowledge.EntityId));
        }
    }
}