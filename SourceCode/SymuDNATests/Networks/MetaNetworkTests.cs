#region Licence

// Description: SymuBiz - SymuTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.Common.Interfaces.Agent;
using Symu.DNA.Networks;
using Symu.DNA.Networks.TwoModesNetworks;
using Symu.DNA.Networks.TwoModesNetworks.Sphere;
using SymuDNATests.Classes;

#endregion

namespace SymuDNATests.Networks
{
    [TestClass]
    public class MetaNetworkTests
    {
        private const byte IsWorkingOn = 1;
        private const byte IsSupportOn = 2;

        private MetaNetwork _network;

        private TestAgent _agent;
        private readonly TestTask _task = new TestTask(1);
        private readonly TestResource _resource = new TestResource(6);
        private readonly TestKnowledge _knowledge =  new TestKnowledge(1);
        private readonly TestBelief _belief = new TestBelief(1);
        
        private TestAgentRole _testAgentRole;
        private TestAgentResource _agentResource;
        private TestAgentOrganization _agentGroup1;
        private TestAgentOrganization _agentGroup2;
        private TestAgentKnowledge _agentKnowledge;
        private TestAgentTask _agentTask ;

        private readonly AgentId _teamId = new AgentId(1, 1);
        private readonly AgentId _teamId2 = new AgentId(2, 1);
        private readonly AgentId _teammateId2 = new AgentId(5, 2);
        private readonly AgentId _managerId = new AgentId(3, 3);


        [TestInitialize]
        public void Initialize()
        {
            var interactionSphereModel = new InteractionSphereModel();
            _network = new Symu.DNA.Networks.MetaNetwork(interactionSphereModel);
            _agent = new TestAgent(4, 2);
            _testAgentRole = new TestAgentRole(_managerId, _teamId, 1);
            _agentResource = new TestAgentResource(_resource.Id, new TestResourceUsage(IsSupportOn), 100);
            _agentGroup1 = new TestAgentOrganization(_agent.AgentId, 100);
            _agentGroup2 = new TestAgentOrganization(_teammateId2, 100);
            _agentKnowledge = new TestAgentKnowledge(_knowledge.Id);
            _agentTask = new TestAgentTask(_agent.AgentId, _task);
        }

        [TestMethod]
        public void ClearTest()
        {
            InitializeNetwork();

            _network.Clear();

            Assert.IsFalse(_network.AgentAgent.Any());
            Assert.IsFalse(_network.AgentOrganization.Any());
            Assert.IsFalse(_network.Role.Any());
            Assert.IsFalse(_network.Resource.Any());
            Assert.IsFalse(_network.Knowledge.Any());
            Assert.IsFalse(_network.AgentKnowledge.Any());
            Assert.IsFalse(_network.AgentTask.Any());
            Assert.IsFalse(_network.Belief.Any());
            Assert.IsFalse(_network.AgentBelief.Any());
            Assert.IsFalse(_network.AgentTask.Any());
            Assert.IsFalse(_network.ResourceTask.Any());
            Assert.IsFalse(_network.TaskKnowledge.Any());
        }

        private void InitializeNetwork()
        {
            var interaction = new TestAgentAgent(_agent.AgentId, _managerId);
            _network.Agent.Add(_agent);
            _network.AgentAgent.AddInteraction(interaction);
            _network.AgentOrganization.AddKey(_teamId);
            _network.AgentOrganization.Add(_teamId, _agentGroup1);
            _network.AgentRole.Add(_testAgentRole);
            _network.Resource.Add(_resource);
            _network.AgentResource.Add(_agent.AgentId, _agentResource);
            _network.Knowledge.Add(_knowledge);
            _network.AgentKnowledge.Add(_agent.AgentId, _agentKnowledge);
            _network.Belief.Add(_belief);
            var agentBelief = new TestAgentBelief(_belief.Id);
            _network.AgentBelief.Add(_agent.AgentId, agentBelief);
            _network.AgentTask.Add(_agent.AgentId, _agentTask);
            _network.ResourceTask.Add(_resource.Id, _task);
            _network.TaskKnowledge.Add(_task.Id, _knowledge);
        }

        /// <summary>
        ///     With network started
        /// </summary>
        [TestMethod]
        public void AddMemberToGroupTest()
        {
            //_network.State = AgentState.Started;
            _network.AgentOrganization.AddKey(_teamId);
            _network.AgentResource.Add(_teamId, _agentResource);
            // Method to test
            _network.AddAgentToGroup(_agentGroup1, _teamId);
            _network.AddAgentToGroup(_agentGroup2, _teamId);
            // Test group
            Assert.IsTrue(_network.AgentOrganization.IsMemberOfGroup(_agent.AgentId, _teamId));
            // Resource
            Assert.IsTrue(_network.AgentResource.HasResource(_agent.AgentId, new TestResourceUsage(IsSupportOn)));
        }

        /// <summary>
        ///     With network starting
        /// </summary>
        [TestMethod]
        public void AddMemberToGroupTest2()
        {
            //_network.State = AgentState.Starting;
            _network.AgentOrganization.AddKey(_teamId);
            _network.AgentResource.Add(_teamId, _agentResource);
            // Method to test
            _network.AddAgentToGroup(_agentGroup1, _teamId);
            _network.AddAgentToGroup(_agentGroup2, _teamId);
            // Test link teammates
            Assert.IsFalse(_network.AgentAgent.HasActiveInteraction(_agent.AgentId, _teammateId2));
        }

        [TestMethod]
        public void RemoveMemberFromGroupTest()
        {
            _network.AgentOrganization.AddKey(_teamId);
            _network.AddAgentToGroup(_agentGroup1, _teamId);
            _network.AddAgentToGroup(_agentGroup2, _teamId);
            _network.AgentResource.Add(_teamId, _agentResource);
            // Method to test
            _network.RemoveAgentFromGroup(_agent.AgentId, _teamId);
            Assert.IsFalse(_network.AgentOrganization.IsMemberOfGroup(_agent.AgentId, _teamId));
            // Test link teammates
            Assert.IsFalse(_network.AgentAgent.HasActiveInteraction(_agent.AgentId, _teammateId2));
            // Test group
            Assert.IsFalse(_network.AgentOrganization.IsMemberOfGroup(_agent.AgentId, _teamId));
            // Test link subordinates
            Assert.IsFalse(_network.AgentAgent.HasActiveInteraction(_agent.AgentId, _managerId));
            // Portfolio
            Assert.IsFalse(_network.AgentResource.HasResource(_agent.AgentId, new TestResourceUsage(IsSupportOn)));
        }

        [TestMethod]
        public void RemoveAgentTest()
        {
            InitializeNetwork();

            _network.RemoveAgent(_agent.AgentId);

            Assert.IsFalse(_network.AgentAgent.Any());
            Assert.AreEqual(0, _network.AgentOrganization.GetValuesCount(_teamId, _agent.AgentId.ClassId));
            Assert.IsFalse(_network.AgentRole.IsMember(_agent.AgentId, _teamId.ClassId));
            Assert.IsFalse(_network.AgentResource.HasResource(_agent.AgentId, _resource.Id, new TestResourceUsage(IsWorkingOn)));
            Assert.IsFalse(_network.AgentKnowledge.Any());
            Assert.IsFalse(_network.AgentBelief.Any());
            Assert.IsFalse(_network.Agent.Any());
            Assert.IsFalse(_network.AgentTask.Any());
        }

        [TestMethod]
        public void GetMainGroupTest()
        {
            // Default test
            var agentId = _network.AgentOrganization.GetMainGroupOrDefault(_agent.AgentId, _teamId.ClassId);
            Assert.IsNull(agentId);
            // Normal test
            _network.AddAgentToGroup(_agentGroup1, _teamId);
            Assert.AreEqual(_teamId, _network.AgentOrganization.GetMainGroupOrDefault(_agent.AgentId, _teamId.ClassId));
            var agentGroup2 = new TestAgentOrganization(_agent.AgentId, 20);
            _network.AddAgentToGroup(agentGroup2, _teamId2);
            Assert.AreEqual(_teamId, _network.AgentOrganization.GetMainGroupOrDefault(_agent.AgentId, _teamId.ClassId));
        }
    }
}