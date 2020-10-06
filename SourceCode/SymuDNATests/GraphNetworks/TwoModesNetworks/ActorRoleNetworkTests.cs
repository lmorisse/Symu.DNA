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
using Symu.DNA.GraphNetworks.TwoModesNetworks;

#endregion

namespace SymuDNATests.GraphNetworks.TwoModesNetworks
{
    [TestClass]
    public class ActorRoleNetworkTests
    {
        private readonly IAgentId _actorId = new AgentId(3, ActorEntity.ClassId);
        private readonly IClassId _classId0 = new ClassId(0);
        private readonly ActorRoleNetwork _network = new ActorRoleNetwork();
        private readonly IAgentId _organizationId = new AgentId(1, OrganizationEntity.ClassId);
        private readonly IAgentId _organizationId1 = new AgentId(2, OrganizationEntity.ClassId);
        private readonly IAgentId _roleId = new AgentId(4, RoleEntity.ClassId);
        private IActorRole _edge;
        private IActorRole _edge1;

        [TestInitialize]
        public void Initialize()
        {
            _edge = new ActorRole(_actorId, _roleId, _organizationId);
            _edge1 = new ActorRole(_actorId, _roleId, _organizationId1);
        }

        [TestMethod]
        public void RemoveSourceTest()
        {
            _network.Add(_edge);
            _network.Add(_edge1);
            _network.RemoveSource(_actorId, _organizationId);
            Assert.IsFalse(_network.HasARoleIn(_actorId, _organizationId));
            Assert.IsTrue(_network.HasARoleIn(_actorId, _organizationId1));
        }

        [TestMethod]
        public void IsActorOfOrganizationIdsTest()
        {
            Assert.AreEqual(0, _network.IsActorOfOrganizationIds(_actorId, _classId0).Count());
            _network.Add(_edge);
            Assert.AreEqual(0, _network.IsActorOfOrganizationIds(_actorId, _classId0).Count());
            Assert.AreEqual(1, _network.IsActorOfOrganizationIds(_actorId, _organizationId.ClassId).Count());
            Assert.AreEqual(_organizationId.ClassId, _network.IsActorOfOrganizationIds(_actorId, _organizationId.ClassId).Select(x=> x.ClassId).First());
            _network.Add(_edge1);
            Assert.AreEqual(0, _network.IsActorOfOrganizationIds(_actorId, _classId0).Count());
            Assert.AreEqual(2, _network.IsActorOfOrganizationIds(_actorId, _organizationId.ClassId).Count());
        }

        [TestMethod]
        public void IsActorOfTest()
        {
            Assert.IsFalse(_network.IsActorOf(_actorId, _organizationId.ClassId));
            _network.Add(_edge);
            Assert.IsTrue(_network.IsActorOf(_actorId, _organizationId.ClassId));
        }

        [TestMethod]
        public void ExistActorForRoleTypeTest()
        {
            Assert.IsFalse(_network.ExistActorForRoleType(_roleId, _organizationId));
            _network.Add(_edge);
            Assert.IsTrue(_network.ExistActorForRoleType(_roleId, _organizationId));
        }

        [TestMethod]
        public void GetActorIdForRoleTypeTest()
        {
            Assert.IsNull(_network.GetActorIdForRoleType(_roleId, _organizationId));
            _network.Add(_edge);
            Assert.AreEqual(_actorId, _network.GetActorIdForRoleType(_roleId, _organizationId));
        }

        [TestMethod]
        public void HasARoleInTest()
        {
            Assert.IsFalse(_network.HasARoleIn(_actorId, _organizationId));
            Assert.IsFalse(_network.HasARoleIn(_actorId, _organizationId1));
            _network.Add(_edge);
            _network.Add(_edge1);
            Assert.IsTrue(_network.HasARoleIn(_actorId, _organizationId));
            Assert.IsTrue(_network.HasARoleIn(_actorId, _organizationId1));
        }

        [TestMethod]
        public void HasARoleInTest1()
        {
            Assert.IsFalse(_network.HasARoleIn(_actorId, _roleId, _organizationId));
            _network.Add(_edge);
            Assert.IsTrue(_network.HasARoleIn(_actorId, _roleId, _organizationId));
        }

        [TestMethod]
        public void GetRolesInTest()
        {
            var getRoles = _network.GetRolesIn(_actorId, _organizationId);
            Assert.IsFalse(getRoles.Any());
            _network.Add(_edge);
            getRoles = _network.GetRolesIn(_actorId, _organizationId);
            Assert.IsTrue(getRoles.Any());
        }

        [TestMethod]
        public void GetRolesTest()
        {
            var getRoles = _network.GetRoles(_organizationId);
            Assert.IsFalse(getRoles.Any());
            _network.Add(_edge);
            getRoles = _network.GetRoles(_organizationId);
            Assert.IsTrue(getRoles.Any());
        }

        [TestMethod]
        public void TransferToTest()
        {
            _network.Add(_edge);
            _network.TransferTo(_actorId, _organizationId, _organizationId1);
            Assert.IsFalse(_network.HasARoleIn(_actorId, _organizationId));
            Assert.IsTrue(_network.HasARoleIn(_actorId, _organizationId1));
        }


        [TestMethod]
        public void RemoveMembersByRoleTypeFromGroupTest()
        {
            _network.Add(_edge);
            _network.RemoveActorsByRoleFromOrganization(_roleId, _organizationId);
            Assert.IsFalse(_network.Any());
        }
    }
}