#region Licence

// Description: SymuBiz - SymuTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.Common.Interfaces.Agent;
using Symu.DNA.GraphNetworks.TwoModesNetworks;
using SymuDNATests.Classes;

#endregion

namespace SymuDNATests.Networks.TwoModesNetworks
{
    [TestClass]
    public class AgentOrganizationNetworkTests
    {
        private readonly OrganizationActorNetwork _organization = new OrganizationActorNetwork();
        private readonly AgentId _teamId = new AgentId(1, 1);
        private readonly AgentId _teamId2 = new AgentId(2, 1);
        private readonly AgentId _teammateId = new AgentId(3, 2);
        private readonly AgentId _teammateId2 = new AgentId(4, 2);
        private readonly AgentId _teammateId3 = new AgentId(5, 2);
        private TestActorOrganization _actorOrganization;
        private TestActorOrganization _actorGroup2;
        private TestActorOrganization _actorGroup3;

        [TestInitialize]
        public void Initialize()
        {
            _actorOrganization = new TestActorOrganization(_teammateId, 100);
            _actorGroup2 = new TestActorOrganization(_teammateId2, 100);
            _actorGroup3 = new TestActorOrganization(_teammateId3, 100);
        }

        /// <summary>
        ///     With agent 1 one team
        /// </summary>
        [TestMethod]
        public void RemoveKeyTest()
        {
            // Without Agent
            _organization.Remove(_teammateId);
            // With agent 1 one team
            _organization.Add(_teamId, _actorOrganization);
            _organization.Remove(_teammateId);
            Assert.IsFalse(_organization.IsActorOfOrganization(_teammateId, _teamId));
        }

        /// <summary>
        ///     With agent 1 two teams
        /// </summary>
        [TestMethod]
        public void RemoveKeyTest1()
        {
            _organization.Add(_teamId, _actorOrganization);
            _organization.Add(_teamId2, _actorOrganization);
            _organization.Remove(_teammateId);
            Assert.IsFalse(_organization.IsActorOfOrganization(_teammateId, _teamId));
            Assert.IsFalse(_organization.IsActorOfOrganization(_teammateId, _teamId2));
        }

        [TestMethod]
        public void AddTest()
        {
            Assert.IsFalse(_organization.Any());
            _organization.Add(_teamId, _actorOrganization);
            Assert.IsTrue(_organization.Any());
            Assert.IsTrue(_organization.List[_teamId][0].ActorId.Equals(_teammateId));
        }

        [TestMethod]
        public void RemoveMemberTest()
        {
            // Without Agent
            _organization.RemoveActor(_teammateId, _teamId);
            // With agent 1 one team
            _organization.Add(_teamId, _actorOrganization);
            _organization.RemoveActor(_teammateId, _teamId);
            Assert.IsFalse(_organization.IsActorOfOrganization(_teammateId, _teamId));
            // With agent 1 two teams
            _organization.Add(_teamId, _actorOrganization);
            _organization.Add(_teamId2, _actorOrganization);
            _organization.RemoveActor(_teammateId, _teamId);
            Assert.IsFalse(_organization.IsActorOfOrganization(_teammateId, _teamId));
            Assert.IsTrue(_organization.IsActorOfOrganization(_teammateId, _teamId2));
        }

        [TestMethod]
        public void IsTeammateTest()
        {
            Assert.IsFalse(_organization.IsActorOfOrganization(_teammateId, _teamId));
            _organization.Add(_teamId, _actorOrganization);
            Assert.IsTrue(_organization.IsActorOfOrganization(_teammateId, _teamId));
        }

        [TestMethod]
        public void IsTeammateTest1()
        {
            Assert.AreEqual(0, _organization.GetOrganizationIds(_teammateId, _teamId.ClassId).Count());
            _organization.Add(_teamId, _actorOrganization);
            Assert.AreEqual(1, _organization.GetOrganizationIds(_teammateId, _teamId.ClassId).Count());
            _organization.Add(_teamId2, _actorOrganization);
            Assert.AreEqual(2, _organization.GetOrganizationIds(_teammateId, _teamId.ClassId).Count());
        }

        [TestMethod]
        public void GetCoMemberIds()
        {
            Assert.AreEqual(0, _organization.GetCoActorIds(_teammateId, _teamId.ClassId).Count());
            _organization.Add(_teamId, _actorOrganization);
            Assert.AreEqual(0, _organization.GetCoActorIds(_teammateId, _teamId.ClassId).Count());
            _organization.Add(_teamId, _actorGroup2);
            Assert.AreEqual(1, _organization.GetCoActorIds(_teammateId, _teamId.ClassId).Count());
            _organization.Add(_teamId2, _actorGroup3);
            Assert.AreEqual(1, _organization.GetCoActorIds(_teammateId, _teamId.ClassId).Count());
            _organization.Add(_teamId2, _actorOrganization);
            Assert.AreEqual(2, _organization.GetCoActorIds(_teammateId, _teamId.ClassId).Count());
        }

        #region Allocation

        [TestMethod]
        public void GetGroupAllocationsTest()
        {
            Assert.AreEqual(0, _organization.GetActorOrganizationsOfAnActor(_teammateId, _teamId.ClassId).Count());
            _organization.Add(_teamId, _actorOrganization);
            Assert.AreEqual(1, _organization.GetActorOrganizationsOfAnActor(_teammateId, _teamId.ClassId).Count());
        }

        [TestMethod]
        public void GetGroupAllocationTest()
        {
            Assert.IsNull(_organization.GetActorOrganization(_teammateId, _teamId));
            _organization.Add(_teamId, _actorOrganization);
            Assert.IsNotNull(_organization.GetActorOrganization(_teammateId, _teamId));
        }

        [TestMethod]
        public void GetAllocationTest()
        {
            Assert.AreEqual(0, _organization.GetAllocation(_teammateId, _teamId));
            _organization.Add(_teamId, _actorOrganization);
            Assert.AreEqual(100, _organization.GetAllocation(_teammateId, _teamId));
        }

        /// <summary>
        ///     Non passing test
        /// </summary>
        [TestMethod]
        public void UpdateGroupAllocationTest()
        {
            Assert.ThrowsException<NullReferenceException>(() =>
                _organization.UpdateAllocation(_teammateId, _teamId, 0, 0));
        }

        [TestMethod]
        public void UpdateGroupAllocationTest1()
        {
            // Test increment
            _actorOrganization = new TestActorOrganization(_teammateId, 50);
            _organization.Add(_teamId, _actorOrganization);
            _organization.UpdateAllocation(_teammateId, _teamId, 20, 0);
            Assert.AreEqual(70, _organization.GetAllocation(_teammateId, _teamId));
            // Test decrement with a threshold
            _organization.UpdateAllocation(_teammateId, _teamId, -70, 10);
            Assert.AreEqual(10, _organization.GetAllocation(_teammateId, _teamId));
        }

        [TestMethod]
        public void FullAllocUpdateGroupAllocationsTest()
        {
            _actorOrganization = new TestActorOrganization(_teammateId, 50);
            _organization.Add(_teamId, _actorOrganization);
            _organization.UpdateAllocations(_teammateId, _teamId.ClassId, true);
            Assert.AreEqual(100, _organization.GetAllocation(_teammateId, _teamId));
        }

        [TestMethod]
        public void UpdateGroupAllocationsTest()
        {
            _actorOrganization = new TestActorOrganization(_teammateId, 50);
            _organization.Add(_teamId, _actorOrganization);
            _organization.UpdateAllocations(_teammateId, _teamId.ClassId, false);
            Assert.AreEqual(50, _organization.GetAllocation(_teammateId, _teamId));
        }

        [TestMethod]
        public void NullUpdateGroupAllocationsTest()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                _organization.UpdateAllocations(_teammateId, _teamId.ClassId, true));
        }

        [TestMethod]
        public void CopyToTest()
        {
            _organization.Add(_teamId, _actorOrganization);
            _organization.CopyTo(_teamId, _teamId2);
            Assert.AreEqual(100, _organization.GetAllocation(_teammateId, _teamId2));
        }

        [TestMethod]
        public void GetMemberAllocationsTest()
        {
            Assert.AreEqual(0, _organization.GetSumActorAllocations(_teamId));
            _organization.Add(_teamId, _actorOrganization);
            Assert.AreEqual(100, _organization.GetSumActorAllocations(_teamId));
            _actorGroup2 = new TestActorOrganization(_teammateId2, 50);
            _organization.Add(_teamId, _actorGroup2);
            Assert.AreEqual(150, _organization.GetSumActorAllocations(_teamId));
        }

        #endregion
    }
}