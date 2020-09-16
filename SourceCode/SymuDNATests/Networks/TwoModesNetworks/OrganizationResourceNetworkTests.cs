#region Licence

// Description: SymuBiz - SymuTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.Common.Interfaces.Agent;
using Symu.Common.Interfaces.Entity;
using Symu.DNA.Entities;
using Symu.DNA.GraphNetworks.TwoModesNetworks;
using SymuDNATests.Classes;

#endregion

namespace SymuDNATests.Networks.TwoModesNetworks
{
    [TestClass]
    public class OrganizationResourceNetworkTests
    {
        private const byte IsSupportOn = 1;
        private const byte IsWorkingOn = 2;
        private const byte IsUsing = 3;
        private readonly IId _resourceId = new UId(3);
        private readonly IOrganization _organization = new OrganizationEntity(2);
        private readonly IResource _resource = new ResourceEntity(2);
        private readonly OrganizationResourceNetwork _network = new OrganizationResourceNetwork();
        private IOrganizationResource _actorResourceSupportOn;
        private IOrganizationResource _actorResourceWorkingOn;
        private IOrganizationResource _actorResourceUsing;
        private UId OrganizationId => (UId)_organization.EntityId;

        [TestInitialize]
        public void Initialize()
        {
            _actorResourceSupportOn = new TestOrganizationResource(_resource.EntityId, new ResourceUsage(IsSupportOn), 100);
            _actorResourceWorkingOn = new TestOrganizationResource(_resource.EntityId, new ResourceUsage(IsWorkingOn), 100);
            _actorResourceUsing = new TestOrganizationResource(_resource.EntityId, new ResourceUsage(IsUsing), 100);
        }

        [TestMethod]
        public void HasResourceTest()
        {
            Assert.IsFalse(_network.HasResource(OrganizationId, new ResourceUsage(IsSupportOn)));
            _network.Add(OrganizationId, _actorResourceSupportOn);
            Assert.IsTrue(_network.HasResource(OrganizationId, new ResourceUsage(IsSupportOn)));
        }


        [TestMethod]
        public void HasResourceTest2()
        {
            Assert.IsFalse(_network.HasResource(OrganizationId, _resource.EntityId, new ResourceUsage(IsSupportOn)));
            _network.Add(OrganizationId, _actorResourceSupportOn);
            Assert.IsTrue(_network.HasResource(OrganizationId, _resource.EntityId, new ResourceUsage(IsSupportOn)));
        }

        [TestMethod]
        public void GetAllocationTest()
        {
            Assert.AreEqual(0, _network.GetAllocation(OrganizationId, _resource.EntityId, new ResourceUsage(IsWorkingOn)));
            _network.Add(OrganizationId, _actorResourceWorkingOn);
            Assert.AreEqual(100, _network.GetAllocation(OrganizationId, _resource.EntityId, new ResourceUsage(IsWorkingOn)));
        }

        [TestMethod]
        public void GetResourceTest()
        {
            Assert.IsNull(_network.GetOrganizationResource(OrganizationId, _resource.EntityId, new ResourceUsage(IsSupportOn)));
            _network.Add(OrganizationId, _actorResourceSupportOn);
            Assert.IsNotNull(_network.GetOrganizationResource(OrganizationId, _resource.EntityId, new ResourceUsage(IsSupportOn)));
        }

        [TestMethod]
        public void GetResourceIdsTest()
        {
            Assert.AreEqual(0, _network.GetResourceIds(_resourceId).Count());
            _network.Add(_resourceId, _actorResourceUsing);
            Assert.AreEqual(1, _network.GetResourceIds(_resourceId).Count());
        }

        [TestMethod]
        public void GetResourceIdsTest1()
        {
            Assert.AreEqual(0, _network.GetResourceIds(_resourceId, new ResourceUsage(IsUsing)).Count());
            _network.Add(_resourceId, _actorResourceUsing);
            Assert.AreEqual(1, _network.GetResourceIds(_resourceId, new ResourceUsage(IsUsing)).Count());
        }
    }
}