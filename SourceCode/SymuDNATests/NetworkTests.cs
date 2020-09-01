#region Licence

// Description: SymuBiz - SymuTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualBasic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.Common.Interfaces.Agent;
using Symu.DNA;
using Symu.DNA.TwoModesNetworks.Assignment;
using Symu.DNA.TwoModesNetworks.Sphere;
using SymuDNATests.Classes;

#endregion

namespace SymuDNATests
{
    [TestClass]
    public class NetworkTests
    {
        private const byte IsWorkingOn = 1;
        private const byte IsSupportOn = 2;
        private readonly TestActivity _activity = new TestActivity("a");
        private readonly TestResource _component = new TestResource(6);

        private readonly TestKnowledge _knowledge =
            new TestKnowledge(1);

        private MetaNetwork _network ;
        private readonly AgentId _teamId = new AgentId(1, 1);
        private readonly AgentId _teamId2 = new AgentId(2, 1);
        private IAgentId TeammateId => _agent.AgentId;
        private readonly AgentId _teammateId2 = new AgentId(5, 2);
        private readonly AgentId _managerId = new AgentId(3, 3);
        private TestAgent _agent;
        private readonly TestBelief _belief = new TestBelief(1);
        private TestAgentRole _testAgentRole;
        private TestAgentResource _agentResource;
        private TestAgentGroup _agentGroup1;
        private TestAgentGroup _agentGroup2;
        private TestAgentKnowledge _agentKnowledge;

        [TestInitialize]
        public void Initialize()
        {
            var interactionSphereModel = new InteractionSphereModel();
            _network = new MetaNetwork(interactionSphereModel);
            _agent = new TestAgent(4, 2);
            _network.Assignment.AddActivities(new List<TestActivity> {_activity}, _teamId);
            _testAgentRole = new TestAgentRole(_managerId, _teamId, 1);
            _agentResource = new TestAgentResource(_component.Id, new TestResourceUsage(IsSupportOn), 100);
            _agentGroup1 = new TestAgentGroup(TeammateId, 100);
            _agentGroup2 = new TestAgentGroup(_teammateId2, 100);
            _agentKnowledge = new TestAgentKnowledge(_knowledge.Id);
        }

        [TestMethod]
        public void ClearTest()
        {
            var interaction = new TestInteraction(TeammateId, _managerId);
            _network.Interaction.AddInteraction(interaction);
            _network.AgentGroup.AddGroup(_teamId);
            _network.AgentRole.Add(_testAgentRole);
            _network.Resource.Add(_component);
            _network.Knowledge.Add(_knowledge);
            _network.AgentKnowledge.Add(TeammateId, _agentKnowledge);
            _network.Belief.Add(_belief);
            var agentBelief = new TestAgentBelief(_belief.Id);
            _network.AgentBelief.Add(TeammateId, agentBelief);
            _network.Assignment.AddActivities(TeammateId, _teamId, new List<IAgentActivity> { new TestAgentActivity(TeammateId, _activity) });
            _network.Clear();
            Assert.IsFalse(_network.Interaction.Any());
            Assert.IsFalse(_network.AgentGroup.Any());
            Assert.IsFalse(_network.Role.Any());
            Assert.IsFalse(_network.Resource.Any());
            Assert.IsFalse(_network.Knowledge.Any());
            Assert.IsFalse(_network.AgentKnowledge.Any());
            Assert.IsFalse(_network.Assignment.Any());
            Assert.IsFalse(_network.Belief.Any());
            Assert.IsFalse(_network.AgentBelief.Any());
        }

        /// <summary>
        ///     With network started
        /// </summary>
        [TestMethod]
        public void AddMemberToGroupTest()
        {
            //_network.State = AgentState.Started;
            _network.AgentGroup.AddGroup(_teamId);
            _network.AgentResource.Add(_teamId, _agentResource);
            // Method to test
            _network.AddAgentToGroup(_agentGroup1, _teamId);
            _network.AddAgentToGroup(_agentGroup2, _teamId);
            // Test group
            Assert.IsTrue(_network.AgentGroup.IsMemberOfGroup(TeammateId, _teamId));
            // Resource
            Assert.IsTrue(_network.AgentResource.HasResource(TeammateId, new TestResourceUsage(IsSupportOn)));
        }

        /// <summary>
        ///     With network starting
        /// </summary>
        [TestMethod]
        public void AddMemberToGroupTest2()
        {
            //_network.State = AgentState.Starting;
            _network.AgentGroup.AddGroup(_teamId);
            _network.AgentResource.Add(_teamId, _agentResource);
            // Method to test
            _network.AddAgentToGroup(_agentGroup1, _teamId);
            _network.AddAgentToGroup(_agentGroup2, _teamId);
            // Test link teammates
            Assert.IsFalse(_network.Interaction.HasActiveInteraction(TeammateId, _teammateId2));
        }

        [TestMethod]
        public void RemoveMemberFromGroupTest()
        {
            _network.AgentGroup.AddGroup(_teamId);
            _network.AddAgentToGroup(_agentGroup1, _teamId);
            _network.AddAgentToGroup(_agentGroup2, _teamId);
            _network.AgentResource.Add(_teamId, _agentResource);
            // Method to test
            _network.RemoveAgentFromGroup(TeammateId, _teamId);
            Assert.IsFalse(_network.AgentGroup.IsMemberOfGroup(TeammateId, _teamId));
            // Test link teammates
            Assert.IsFalse(_network.Interaction.HasActiveInteraction(TeammateId, _teammateId2));
            // Test group
            Assert.IsFalse(_network.AgentGroup.IsMemberOfGroup(TeammateId, _teamId));
            // Test link subordinates
            Assert.IsFalse(_network.Interaction.HasActiveInteraction(TeammateId, _managerId));
            // Portfolio
            Assert.IsFalse(_network.AgentResource.HasResource(TeammateId, new TestResourceUsage(IsSupportOn)));
        }

        [TestMethod]
        public void RemoveAgentTest()
        {
            var interaction = new TestInteraction(TeammateId, _managerId);
            _network.Agent.Add(_agent);
            _network.Interaction.AddInteraction(interaction);
            _network.AgentGroup.AddAgent(_agentGroup1, _teamId);
            _network.AgentRole.Add(_testAgentRole);
            _network.AgentResource.Add(TeammateId, _agentResource);
            _network.AgentKnowledge.Add(TeammateId, _agentKnowledge);
            _network.Belief.Add(_belief);
            _network.Assignment.AddActivities(TeammateId, _teamId, new List<IAgentActivity> {new TestAgentActivity(TeammateId, _activity)});
            _network.RemoveAgent(TeammateId);
            Assert.IsFalse(_network.Interaction.Any());
            Assert.AreEqual(0, _network.AgentGroup.GetAgentsCount(_teamId, TeammateId.ClassId));
            Assert.IsFalse(_network.AgentRole.IsMember(TeammateId, _teamId.ClassId));
            Assert.IsFalse(_network.AgentResource.HasResource(TeammateId, _component.Id, new TestResourceUsage(IsWorkingOn)));
            Assert.IsFalse(_network.AgentKnowledge.Any());
            Assert.IsFalse(_network.AgentBelief.Any());
            Assert.IsFalse(_network.Agent.Any());
            Assert.IsFalse(_network.Assignment.AgentHasActivitiesOn(TeammateId, _teamId));
        }

        [TestMethod]
        public void GetMainGroupTest()
        {
            // Default test
            var agentId = _network.AgentGroup.GetMainGroupOrDefault(TeammateId, _teamId.ClassId);
            Assert.IsNull(agentId);
            // Normal test
            _network.AddAgentToGroup(_agentGroup1, _teamId);
            Assert.AreEqual(_teamId, _network.AgentGroup.GetMainGroupOrDefault(TeammateId, _teamId.ClassId));
            _network.AddAgentToGroup(_agentGroup1, _teamId2);
            Assert.AreEqual(_teamId, _network.AgentGroup.GetMainGroupOrDefault(TeammateId, _teamId.ClassId));
        }
    }
}