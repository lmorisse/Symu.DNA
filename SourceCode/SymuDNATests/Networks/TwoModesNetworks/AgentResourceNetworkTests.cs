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
using Symu.DNA.Entities;
using Symu.DNA.GraphNetworks.TwoModesNetworks;
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
        private readonly IAgentId _agentId = new AgentId(3, 3);
        private readonly IAgentId _groupId = new AgentId(1, 1);
        private readonly IResource _resource = new ResourceEntity(2);
        private readonly ActorResourceNetwork _network = new ActorResourceNetwork();
        private IActorResource _actorResourceSupportOn;
        private IActorResource _actorResourceWorkingOn;
        private IActorResource _actorResourceUsing;

        [TestInitialize]
        public void Initialize()
        {
            _actorResourceSupportOn = new TestActorResource(_resource.EntityId, new ResourceUsage(IsSupportOn), 100);
            _actorResourceWorkingOn = new TestActorResource(_resource.EntityId, new ResourceUsage(IsWorkingOn), 100);
            _actorResourceUsing = new TestActorResource(_resource.EntityId, new ResourceUsage(IsUsing), 100);
        }

        [TestMethod]
        public void ExistsTest2()
        {
            Assert.IsFalse(_network.HasResource(_groupId, new ResourceUsage(IsSupportOn)));
            _network.Add(_groupId, _actorResourceSupportOn);
            Assert.IsTrue(_network.HasResource(_groupId, new ResourceUsage(IsSupportOn)));
        }


        [TestMethod]
        public void ExistsTest3()
        {
            Assert.IsFalse(_network.HasResource(_groupId, _resource.EntityId, new ResourceUsage(IsSupportOn)));
            _network.Add(_groupId, _actorResourceSupportOn);
            Assert.IsTrue(_network.HasResource(_groupId, _resource.EntityId, new ResourceUsage(IsSupportOn)));
        }

        /// <summary>
        ///     With a resource
        /// </summary>
        [TestMethod]
        public void RemoveAgentTest1()
        {
            _network.RemoveActor(_groupId);
            _network.Add(_groupId,  _actorResourceSupportOn);
            _network.RemoveActor(_groupId);
            Assert.IsFalse(_network.Any());
            Assert.IsFalse(_network.HasResource(_groupId, new ResourceUsage(IsSupportOn)));
        }

        [TestMethod]
        public void GetAllocationTest()
        {
            Assert.AreEqual(0, _network.GetAllocation(_groupId, _resource.EntityId, new ResourceUsage(IsWorkingOn)));
            _network.Add(_groupId, _actorResourceWorkingOn);
            Assert.AreEqual(100, _network.GetAllocation(_groupId, _resource.EntityId, new ResourceUsage(IsWorkingOn)));
        }

        [TestMethod]
        public void GetResourceTest()
        {
            Assert.IsNull(_network.GetActorResource(_groupId, _resource.EntityId, new ResourceUsage(IsSupportOn)));
            _network.Add(_groupId, _actorResourceSupportOn);
            Assert.IsNotNull(_network.GetActorResource(_groupId, _resource.EntityId, new ResourceUsage(IsSupportOn)));
        }

        [TestMethod]
        public void CopyToTest()
        {
            var teamId = new AgentId(3, 3);
            _network.Add(teamId, _actorResourceSupportOn);
            _network.Add(teamId, _actorResourceWorkingOn);
            _network.Add(teamId, _actorResourceUsing);
            var newTeamId = new AgentId(4, 3);
            _network.CopyTo(teamId, newTeamId);
            Assert.IsTrue(_network.HasResource(newTeamId, _resource.EntityId, new ResourceUsage(IsSupportOn)));
            Assert.IsTrue(_network.HasResource(newTeamId, _resource.EntityId, new ResourceUsage(IsWorkingOn)));
            Assert.IsTrue(_network.HasResource(newTeamId, _resource.EntityId, new ResourceUsage(IsUsing)));
        }

        [TestMethod]
        public void GetResourceIdsTest()
        {
            Assert.AreEqual(0, _network.GetResourceIds(_agentId).Count());
            _network.Add(_agentId, _actorResourceUsing);
            Assert.AreEqual(1, _network.GetResourceIds(_agentId).Count());
        }

        [TestMethod]
        public void GetResourceIdsTest1()
        {
            Assert.AreEqual(0, _network.GetResourceIds(_agentId, new ResourceUsage(IsUsing)).Count());
            _network.Add(_agentId, _actorResourceUsing);
            Assert.AreEqual(1, _network.GetResourceIds(_agentId, new ResourceUsage(IsUsing)).Count());
        }
    }
}