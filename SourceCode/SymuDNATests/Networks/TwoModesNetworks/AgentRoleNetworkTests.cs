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
using Symu.DNA.GraphNetworks.TwoModesNetworks;
using SymuDNATests.Classes;

#endregion

namespace SymuDNATests.Networks.TwoModesNetworks
{
    [TestClass]
    public class AgentRoleNetworkTests
    {
        private readonly ActorRoleNetwork _roles = new ActorRoleNetwork();
        private readonly AgentId _teamId = new AgentId(1, 1);
        private readonly AgentId _teamId2 = new AgentId(2, 1);
        private readonly AgentId _teammateId = new AgentId(2, 2);
        private readonly ClassId _classId0 = new ClassId(0);
        private TestActorRole _actorRole;
        private TestActorRole _actorRole2;

        [TestInitialize]
        public void Initialize()
        {
            _actorRole = new TestActorRole(_teammateId, _teamId, 1);
            _actorRole2 = new TestActorRole(_teammateId, _teamId2, 1);
        }

        [TestMethod]
        public void ClearTest()
        {
            _roles.Add(_actorRole);
            _roles.Clear();
            Assert.IsFalse(_roles.Any());
        }

        /// <summary>
        ///     Remove agent
        /// </summary>
        [TestMethod]
        public void RemoveAgentTest()
        {
            _roles.Add(_actorRole);
            _roles.RemoveActor(_teammateId);
            Assert.IsFalse(_roles.Any());
        }

        /// <summary>
        ///     Remove group
        /// </summary>
        [TestMethod]
        public void RemoveAgentTest1()
        {
            _roles.Add(_actorRole);
            _roles.RemoveActor(_teamId);
            Assert.IsFalse(_roles.Any());
        }

        [TestMethod]
        public void AddTest()
        {
            Assert.IsFalse(_roles.Exists(_actorRole));
            Assert.IsFalse(_roles.Any());
            _roles.Add(_actorRole);
            Assert.IsTrue(_roles.Any());
            Assert.IsTrue(_roles.Exists(_actorRole));
        }

        [TestMethod]
        public void IsTeammateOfTest()
        {
            Assert.AreEqual(0, _roles.IsActorOfOrganizations(_teammateId, _classId0).Count());
            _roles.Add(_actorRole);
            Assert.AreEqual(0, _roles.IsActorOfOrganizations(_teammateId, _classId0).Count());
            Assert.AreEqual(1, _roles.IsActorOfOrganizations(_teammateId, _teamId.ClassId).Count());
            _roles.Add(_actorRole2);
            Assert.AreEqual(0, _roles.IsActorOfOrganizations(_teammateId, _classId0).Count());
            Assert.AreEqual(2, _roles.IsActorOfOrganizations(_teammateId, _teamId.ClassId).Count());
        }

        [TestMethod]
        public void RemoveMemberTest()
        {
            _roles.Add(_actorRole);
            _roles.Add(_actorRole2);
            _roles.RemoveActor(_teammateId, _teamId);
            Assert.IsFalse(_roles.HasARoleIn(_teammateId, _teamId));
            Assert.IsTrue(_roles.HasARoleIn(_teammateId, _teamId2));
        }

        [TestMethod]
        public void GetRolesTest()
        {
            var getRoles = _roles.GetRoles(_teamId);
            Assert.IsFalse(getRoles.Any());
            _roles.Add(_actorRole);
            getRoles = _roles.GetRoles(_teamId);
            Assert.IsTrue(getRoles.Any());
        }

        [TestMethod]
        public void GetRolesTest1()
        {
            var getRoles = _roles.GetRolesIn(_teammateId, _teamId);
            Assert.IsFalse(getRoles.Any());
            _roles.Add(_actorRole);
            getRoles = _roles.GetRolesIn(_teammateId, _teamId);
            Assert.IsTrue(getRoles.Any());
        }

        [TestMethod]
        public void GetAgentsTest()
        {
            Assert.AreEqual(0, _roles.GetActors(_actorRole.Role).Count());
            _roles.Add(_actorRole);
            Assert.AreEqual(1, _roles.GetActors(_actorRole.Role).Count());
            var roleEntity3 = new TestActorRole(_teammateId, _teamId, 2);
            _roles.Add(roleEntity3);
            Assert.AreEqual(1, _roles.GetActors(_actorRole.Role).Count());
        }

        [TestMethod]
        public void HasRoleTest()
        {
            Assert.IsFalse(_roles.HasRole(_teammateId, _actorRole.Role));
            _roles.Add(_actorRole);
            Assert.IsTrue(_roles.HasRole(_teammateId, _actorRole.Role));
        }


        [TestMethod]
        public void GetGroupsTest()
        {
            Assert.AreEqual(0, _roles.GetOrganizations(_teammateId, _actorRole.Role).Count());
            _roles.Add(_actorRole);
            Assert.AreEqual(1, _roles.GetOrganizations(_teammateId, _actorRole.Role).Count());
            Assert.AreEqual(_teamId, _roles.GetOrganizations(_teammateId, _actorRole.Role).ElementAt(0));
            _roles.Add(_actorRole2);
            Assert.AreEqual(2, _roles.GetOrganizations(_teammateId, _actorRole2.Role).Count());
        }

        [TestMethod]
        public void RemoveMembersByRoleTypeFromGroupTest()
        {
            _roles.Add(_actorRole);
            _roles.RemoveActorsByRoleFromOrganization(_actorRole.Role, _teamId);
            Assert.AreEqual(0, _roles.GetOrganizations(_teammateId, _actorRole.Role).Count());
        }
    }
}