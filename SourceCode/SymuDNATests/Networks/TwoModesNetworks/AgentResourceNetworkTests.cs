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
using Symu.DNA.Networks.TwoModesNetworks;
using SymuDNATests.Classes;

#endregion

namespace SymuDNATests.Networks.TwoModesNetworks
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
        private readonly AgentResourceNetwork _network = new AgentResourceNetwork();
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
        public void ExistsTest2()
        {
            Assert.IsFalse(_network.HasResource(_groupId, new TestResourceUsage(IsSupportOn)));
            _network.Add(_groupId, _agentResourceSupportOn);
            Assert.IsTrue(_network.HasResource(_groupId, new TestResourceUsage(IsSupportOn)));
        }


        [TestMethod]
        public void ExistsTest3()
        {
            Assert.IsFalse(_network.HasResource(_groupId, _resource.Id, new TestResourceUsage(IsSupportOn)));
            _network.Add(_groupId, _agentResourceSupportOn);
            Assert.IsTrue(_network.HasResource(_groupId, _resource.Id, new TestResourceUsage(IsSupportOn)));
        }

        /// <summary>
        ///     With a resource
        /// </summary>
        [TestMethod]
        public void RemoveAgentTest1()
        {
            _network.RemoveKey(_groupId);
            _network.Add(_groupId,  _agentResourceSupportOn);
            _network.RemoveKey(_groupId);
            Assert.IsFalse(_network.Any());
            Assert.IsFalse(_network.HasResource(_groupId, new TestResourceUsage(IsSupportOn)));
        }

        [TestMethod]
        public void GetAllocationTest()
        {
            Assert.AreEqual(0, _network.GetAllocation(_groupId, _resource.Id, new TestResourceUsage(IsWorkingOn)));
            _network.Add(_groupId, _agentResourceWorkingOn);
            Assert.AreEqual(100, _network.GetAllocation(_groupId, _resource.Id, new TestResourceUsage(IsWorkingOn)));
        }

        [TestMethod]
        public void AddMemberTest()
        {
            _network.Add(_groupId, _agentResourceSupportOn);
            _network.Add(_groupId, _agentResourceWorkingOn);
            _network.Add(_groupId, _agentResourceUsing);
            _network.AddMemberToGroup(_agentId, _groupId);
            Assert.IsTrue(_network.HasResource(_agentId, new TestResourceUsage(IsSupportOn)));
            Assert.IsTrue(_network.HasResource(_agentId, new TestResourceUsage(IsWorkingOn)));
            Assert.IsTrue(_network.HasResource(_agentId, new TestResourceUsage(IsUsing)));
        }

        [TestMethod]
        public void RemoveMemberTest()
        {
            _network.Add(_groupId, _agentResourceSupportOn);
            _network.Add(_groupId, _agentResourceWorkingOn);
            _network.Add(_groupId, _agentResourceUsing);
            _network.AddMemberToGroup(_agentId, _groupId);
            _network.RemoveMemberFromGroup(_agentId, _groupId);
            Assert.IsFalse(_network.HasResource(_agentId, new TestResourceUsage(IsSupportOn)));
            Assert.IsFalse(_network.HasResource(_agentId, new TestResourceUsage(IsWorkingOn)));
            Assert.IsFalse(_network.HasResource(_agentId, new TestResourceUsage(IsUsing)));
        }

        [TestMethod]
        public void GetResourceTest()
        {
            Assert.IsNull(_network.GetAgentResource(_groupId, _resource.Id, new TestResourceUsage(IsSupportOn)));
            _network.Add(_groupId, _agentResourceSupportOn);
            Assert.IsNotNull(_network.GetAgentResource(_groupId, _resource.Id, new TestResourceUsage(IsSupportOn)));
        }

        [TestMethod]
        public void CopyToTest()
        {
            var teamId = new AgentId(3, 3);
            _network.Add(teamId, _agentResourceSupportOn);
            _network.Add(teamId, _agentResourceWorkingOn);
            _network.Add(teamId, _agentResourceUsing);
            var newTeamId = new AgentId(4, 3);
            _network.CopyTo(teamId, newTeamId);
            Assert.IsTrue(_network.HasResource(newTeamId, _resource.Id, new TestResourceUsage(IsSupportOn)));
            Assert.IsTrue(_network.HasResource(newTeamId, _resource.Id, new TestResourceUsage(IsWorkingOn)));
            Assert.IsTrue(_network.HasResource(newTeamId, _resource.Id, new TestResourceUsage(IsUsing)));
        }

        [TestMethod]
        public void GetResourceIdsTest()
        {
            Assert.AreEqual(0, _network.GetResourceIds(_agentId).Count());
            _network.Add(_agentId, _agentResourceUsing);
            Assert.AreEqual(1, _network.GetResourceIds(_agentId).Count());
        }

        [TestMethod]
        public void GetResourceIdsTest1()
        {
            Assert.AreEqual(0, _network.GetResourceIds(_agentId, new TestResourceUsage(IsUsing)).Count());
            _network.Add(_agentId, _agentResourceUsing);
            Assert.AreEqual(1, _network.GetResourceIds(_agentId, new TestResourceUsage(IsUsing)).Count());
        }
    }
}