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
using Symu.DNA.TwoModesNetworks.AgentResource;
using SymuDNATests.Classes;

#endregion

namespace SymuDNATests.TwoModesNetworks.AgentResource
{
    [TestClass]
    public class AgentResourceNetworkTests
    {
        private const byte IsSupportOn = 1;
        private const byte IsWorkingOn = 2;
        private const byte IsUsing = 3;
        private readonly AgentId _agentId = new AgentId(3, 3);
        private readonly AgentId _groupId = new AgentId(1, 1);
        private readonly TestResource _resource = new TestResource(2);
        private readonly AgentResourceNetwork _resources = new AgentResourceNetwork();
        private TestAgentResource _agentResourceSupportOn;
        private TestAgentResource _agentResourceWorkingOn;
        private TestAgentResource _agentResourceUsing;

        [TestInitialize]
        public void Initialize()
        {
            _agentResourceSupportOn = new TestAgentResource(_resource.Id, new TestResourceUsage(IsSupportOn), 100);
            _agentResourceWorkingOn = new TestAgentResource(_resource.Id, new TestResourceUsage(IsWorkingOn), 100);
            _agentResourceUsing = new TestAgentResource(_resource.Id, new TestResourceUsage(IsUsing), 100);
        }

        [TestMethod]
        public void ClearTest()
        {
            _resources.Add(_groupId, _agentResourceSupportOn);
            _resources.Clear();
            Assert.IsFalse(_resources.Any());
        }

        [TestMethod]
        public void ExistsTest2()
        {
            Assert.IsFalse(_resources.HasResource(_groupId, new TestResourceUsage(IsSupportOn)));
            _resources.Add(_groupId, _agentResourceSupportOn);
            Assert.IsTrue(_resources.HasResource(_groupId, new TestResourceUsage(IsSupportOn)));
        }


        [TestMethod]
        public void ExistsTest3()
        {
            Assert.IsFalse(_resources.HasResource(_groupId, _resource.Id, new TestResourceUsage(IsSupportOn)));
            _resources.Add(_groupId, _agentResourceSupportOn);
            Assert.IsTrue(_resources.HasResource(_groupId, _resource.Id, new TestResourceUsage(IsSupportOn)));
        }

        [TestMethod]
        public void AddAgentIdTest()
        {
            _resources.AddAgentId(_agentId);
            Assert.IsTrue(_resources.Any());
            Assert.IsTrue(_resources.ExistsAgentId(_agentId));
        }

        [TestMethod]
        public void RemoveAgentTest()
        {
            _resources.RemoveAgent(_agentId);
            _resources.AddAgentId(_agentId);
            _resources.RemoveAgent(_agentId);
            Assert.IsFalse(_resources.Any());
        }

        /// <summary>
        ///     With a resource
        /// </summary>
        [TestMethod]
        public void RemoveAgentTest1()
        {
            _resources.RemoveAgent(_groupId);
            _resources.Add(_groupId,  _agentResourceSupportOn);
            _resources.RemoveAgent(_groupId);
            Assert.IsFalse(_resources.Any());
            Assert.IsFalse(_resources.HasResource(_groupId, new TestResourceUsage(IsSupportOn)));
        }

        [TestMethod]
        public void GetAllocationTest()
        {
            Assert.AreEqual(0, _resources.GetAllocation(_groupId, _resource.Id, new TestResourceUsage(IsWorkingOn)));
            _resources.Add(_groupId, _agentResourceWorkingOn);
            Assert.AreEqual(100, _resources.GetAllocation(_groupId, _resource.Id, new TestResourceUsage(IsWorkingOn)));
        }

        [TestMethod]
        public void AddMemberTest()
        {
            _resources.Add(_groupId, _agentResourceSupportOn);
            _resources.Add(_groupId, _agentResourceWorkingOn);
            _resources.Add(_groupId, _agentResourceUsing);
            _resources.AddMemberToGroup(_agentId, _groupId);
            Assert.IsTrue(_resources.HasResource(_agentId, new TestResourceUsage(IsSupportOn)));
            Assert.IsTrue(_resources.HasResource(_agentId, new TestResourceUsage(IsWorkingOn)));
            Assert.IsTrue(_resources.HasResource(_agentId, new TestResourceUsage(IsUsing)));
        }

        [TestMethod]
        public void RemoveMemberTest()
        {
            _resources.Add(_groupId, _agentResourceSupportOn);
            _resources.Add(_groupId, _agentResourceWorkingOn);
            _resources.Add(_groupId, _agentResourceUsing);
            _resources.AddMemberToGroup(_agentId, _groupId);
            _resources.RemoveMemberFromGroup(_agentId, _groupId);
            Assert.IsFalse(_resources.HasResource(_agentId, new TestResourceUsage(IsSupportOn)));
            Assert.IsFalse(_resources.HasResource(_agentId, new TestResourceUsage(IsWorkingOn)));
            Assert.IsFalse(_resources.HasResource(_agentId, new TestResourceUsage(IsUsing)));
        }

        [TestMethod]
        public void GetResourceTest()
        {
            Assert.IsNull(_resources.GetAgentResource(_groupId, _resource.Id, new TestResourceUsage(IsSupportOn)));
            _resources.Add(_groupId, _agentResourceSupportOn);
            Assert.IsNotNull(_resources.GetAgentResource(_groupId, _resource.Id, new TestResourceUsage(IsSupportOn)));
        }

        [TestMethod]
        public void CopyToTest()
        {
            var teamId = new AgentId(3, 3);
            _resources.Add(teamId, _agentResourceSupportOn);
            _resources.Add(teamId, _agentResourceWorkingOn);
            _resources.Add(teamId, _agentResourceUsing);
            var newTeamId = new AgentId(4, 3);
            _resources.CopyTo(teamId, newTeamId);
            Assert.IsTrue(_resources.HasResource(newTeamId, _resource.Id, new TestResourceUsage(IsSupportOn)));
            Assert.IsTrue(_resources.HasResource(newTeamId, _resource.Id, new TestResourceUsage(IsWorkingOn)));
            Assert.IsTrue(_resources.HasResource(newTeamId, _resource.Id, new TestResourceUsage(IsUsing)));
        }

        [TestMethod]
        public void GetResourceIdsTest()
        {
            Assert.AreEqual(0, _resources.GetResourceIds(_agentId).Count());
            _resources.Add(_agentId, _agentResourceUsing);
            Assert.AreEqual(1, _resources.GetResourceIds(_agentId).Count());
        }

        [TestMethod]
        public void GetResourceIdsTest1()
        {
            Assert.AreEqual(0, _resources.GetResourceIds(_agentId, new TestResourceUsage(IsUsing)).Count());
            _resources.Add(_agentId, _agentResourceUsing);
            Assert.AreEqual(1, _resources.GetResourceIds(_agentId, new TestResourceUsage(IsUsing)).Count());
        }
    }
}