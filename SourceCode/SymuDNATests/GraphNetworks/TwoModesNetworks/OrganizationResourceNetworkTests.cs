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
using Symu.DNA.Entities;
using Symu.DNA.GraphNetworks.TwoModesNetworks;

#endregion

namespace SymuDNATests.GraphNetworks.TwoModesNetworks
{
    [TestClass]
    public class OrganizationResourceNetworkTests
    {
        private readonly OrganizationResourceNetwork _network = new OrganizationResourceNetwork();
        private readonly IAgentId _organizationId = new AgentId(2, 2);
        private readonly IAgentId _resourceId = new AgentId(1, 1);
        private readonly IResourceUsage _usage = new ResourceUsage(1);

        private IOrganizationResource _edge;

        [TestInitialize]
        public void Initialize()
        {
            _edge = new OrganizationResource(_organizationId, _resourceId, _usage, 100);
        }

        [TestMethod]
        public void GetWeightTest()
        {
            Assert.AreEqual(0, _network.GetWeight(_organizationId, _resourceId, _usage));
            _network.Add(_edge);
            Assert.AreEqual(100, _network.GetWeight(_organizationId, _resourceId, _usage));
        }
        [TestMethod()]
        public void SetWeightTest()
        {
            _network.Add(_edge);
            Assert.AreEqual(100, _network.GetWeight(_organizationId, _resourceId, _usage));
            _network.SetWeight(_organizationId, _resourceId, _usage, 50);
            Assert.AreEqual(50, _network.GetWeight(_organizationId, _resourceId, _usage));
        }

        [TestMethod]
        public void GetOrganizationResourceTest()
        {
            Assert.IsNull(_network.Edge(_organizationId, _resourceId, _usage));
            _network.Add(_edge);
            Assert.IsNotNull(_network.Edge(_organizationId, _resourceId, _usage));
        }

        [TestMethod]
        public void HasResourceTest()
        {
            Assert.IsFalse(_network.HasResource(_organizationId, _usage));
            _network.Add(_edge);
            Assert.IsTrue(_network.HasResource(_organizationId, _usage));
        }


        [TestMethod]
        public void HasResourceTest1()
        {
            Assert.IsFalse(_network.HasResource(_organizationId, _resourceId, _usage));
            _network.Add(_edge);
            Assert.IsTrue(_network.HasResource(_organizationId, _resourceId, _usage));
        }

        [TestMethod]
        public void GetResourceIdsTest()
        {
            Assert.AreEqual(0, _network.GetResourceIds(_organizationId, _usage).Count());
            _network.Add(_edge);
            Assert.AreEqual(1, _network.GetResourceIds(_organizationId, _usage).Count());
        }

        [TestMethod]
        public void GetOrganizationIdsTest()
        {
            Assert.AreEqual(0, _network.GetOrganizationIds(_resourceId, _usage).Count());
            _network.Add(_edge);
            Assert.AreEqual(1, _network.GetOrganizationIds(_resourceId, _usage).Count());
        }

        /// <summary>
        ///     Non passing test
        /// </summary>
        [TestMethod]
        public void UpdateAllocationTest()
        {
            Assert.ThrowsException<NullReferenceException>(() =>
                _network.UpdateWeight(_organizationId, _resourceId, _usage, 0, 0));
        }

        [TestMethod]
        public void UpdateAllocationTest1()
        {
            // Test increment
            _edge = new OrganizationResource(_organizationId, _resourceId, _usage, 50);
            _network.Add(_edge);
            _network.UpdateWeight(_organizationId, _resourceId, _usage, 20, 0);
            Assert.AreEqual(70, _network.Weight(_organizationId, _resourceId));
            // Test decrement with a threshold
            _network.UpdateWeight(_organizationId, _resourceId, _usage, -70, 10);
            Assert.AreEqual(10, _network.Weight(_organizationId, _resourceId));
        }

        /// <summary>
        ///     Full allocation true
        /// </summary>
        [TestMethod]
        public void UpdateAllocationsTest()
        {
            _edge = new OrganizationResource(_organizationId, _resourceId, _usage, 50);
            _network.Add(_edge);
            _network.UpdateWeights(_organizationId, _resourceId.ClassId, true);
            Assert.AreEqual(100, _network.Weight(_organizationId, _resourceId));
        }

        /// <summary>
        ///     Full allocation false
        /// </summary>
        [TestMethod]
        public void UpdateAllocationsTest1()
        {
            _edge = new OrganizationResource(_organizationId, _resourceId, _usage, 50);
            _network.Add(_edge);
            _network.UpdateWeights(_organizationId, _resourceId.ClassId, false);
            Assert.AreEqual(50, _network.Weight(_organizationId, _resourceId));
        }

        [TestMethod]
        public void UpdateAllocationsTest2()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                _network.UpdateWeights(_organizationId, _resourceId.ClassId, true));
        }
    }
}