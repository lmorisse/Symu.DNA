#region Licence

// Description: SymuBiz - SymuDNATests
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.Common.Interfaces;
using Symu.DNA.Edges;
using Symu.DNA.GraphNetworks.TwoModesNetworks;

#endregion

namespace SymuDNATests.GraphNetworks.TwoModesNetworks
{
    [TestClass]
    public class ActorOrganizationNetworkTests
    {
        private readonly IAgentId _actorId = new AgentId(3, 2);
        private readonly IAgentId _actorId1 = new AgentId(4, 2);
        private readonly IAgentId _actorId2 = new AgentId(5, 2);
        private readonly ActorOrganizationNetwork _network = new ActorOrganizationNetwork();
        private readonly IAgentId _organizationId = new AgentId(1, 1);
        private IActorOrganization _edge;
        private IActorOrganization _edge1;
        private IActorOrganization _edge2;

        [TestInitialize]
        public void Initialize()
        {
            _edge = new ActorOrganization(_actorId, _organizationId);
            _edge1 = new ActorOrganization(_actorId1, _organizationId);
            _edge2 = new ActorOrganization(_actorId2, _organizationId);
        }

        /// <summary>
        ///     Don't exists
        /// </summary>
        [TestMethod]
        public void AddTest()
        {
            Assert.IsFalse(_network.Any());
            _network.Add(_edge);
            Assert.IsTrue(_network.Any());
        }

        /// <summary>
        ///     Exists + updateAllocations
        /// </summary>
        [TestMethod]
        public void AddTest1()
        {
            _network.Add(_edge);
            _edge.Weight = 0;
            Assert.AreEqual(0, _network.Weight(_edge));
        }

        [TestMethod]
        public void GetCoActorIdsTest()
        {
            Assert.AreEqual(0, _network.GetCoActorIds(_actorId, _organizationId.ClassId).Count());
            _network.Add(_edge);
            Assert.AreEqual(0, _network.GetCoActorIds(_actorId, _organizationId.ClassId).Count());
            _network.Add(_edge1);
            Assert.AreEqual(1, _network.GetCoActorIds(_actorId, _organizationId.ClassId).Count());
            _network.Add(_edge2);
            Assert.AreEqual(2, _network.GetCoActorIds(_actorId, _organizationId.ClassId).Count());
        }

        /// <summary>
        ///     Default organization
        /// </summary>
        [TestMethod]
        public void GetMainOrganizationOrDefaultTest()
        {
            _network.Clear();
            Assert.IsNull(_network.GetMainOrganizationOrDefault(_actorId, _organizationId.ClassId));
            Assert.AreNotEqual(_organizationId,
                _network.GetMainOrganizationOrDefault(_actorId, _organizationId.ClassId));
        }

        /// <summary>
        ///     First organization
        /// </summary>
        [TestMethod]
        public void GetMainOrganizationOrDefaultTest1()
        {
            _network.Add(_edge);
            IAgentId teamId1 = new AgentId(2, _organizationId.ClassId);
            var actorOrganization1 = new ActorOrganization(_actorId, teamId1, 20);
            _network.Add(actorOrganization1);
            Assert.AreEqual(_organizationId, _network.GetMainOrganizationOrDefault(_actorId, _organizationId.ClassId));
        }

        #region Allocation

        /// <summary>
        ///     Non passing test
        /// </summary>
        [TestMethod]
        public void UpdateAllocationTest()
        {
            Assert.ThrowsException<NullReferenceException>(() =>
                _network.UpdateWeight(_actorId, _organizationId, 0, 0));
        }

        [TestMethod]
        public void UpdateAllocationTest1()
        {
            // Test increment
            _edge = new ActorOrganization(_actorId, _organizationId, 50);
            _network.Add(_edge);
            _network.UpdateWeight(_actorId, _organizationId, 20, 0);
            Assert.AreEqual(70, _network.Weight(_actorId, _organizationId));
            // Test decrement with a threshold
            _network.UpdateWeight(_actorId, _organizationId, -70, 10);
            Assert.AreEqual(10, _network.Weight(_actorId, _organizationId));
        }

        /// <summary>
        ///     Full allocation true
        /// </summary>
        [TestMethod]
        public void UpdateAllocationsTest()
        {
            _edge = new ActorOrganization(_actorId, _organizationId, 50);
            _network.Add(_edge);
            _network.UpdateWeights(_actorId, _organizationId.ClassId, true);
            Assert.AreEqual(100, _network.Weight(_actorId, _organizationId));
        }

        /// <summary>
        ///     Full allocation false
        /// </summary>
        [TestMethod]
        public void UpdateAllocationsTest1()
        {
            _edge = new ActorOrganization(_actorId, _organizationId, 50);
            _network.Add(_edge);
            _network.UpdateWeights(_actorId, _organizationId.ClassId, false);
            Assert.AreEqual(50, _network.Weight(_actorId, _organizationId));
        }

        [TestMethod]
        public void UpdateAllocationsTest2()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                _network.UpdateWeights(_actorId, _organizationId.ClassId, true));
        }

        #endregion
    }
}