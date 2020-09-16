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
using Symu.DNA.Entities;
using Symu.DNA.GraphNetworks;
using Symu.DNA.GraphNetworks.TwoModesNetworks;
using Symu.DNA.GraphNetworks.TwoModesNetworks.Sphere;
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

        private IActor _agent;
        private readonly ITask _task = new TaskEntity(1);
        private readonly IOrganization _organization = new OrganizationEntity(1);
        private readonly IResource _resource = new ResourceEntity(1);
        private readonly IKnowledge _knowledge =  new KnowledgeEntity(1);
        private IBelief _belief;
        
        private IActorRole _testActorRole;
        private IActorResource _actorResource;
        private IActorOrganization _actorGroup1;
        private IActorOrganization _actorGroup2;
        private IActorKnowledge _actorKnowledge;
        private IActorTask _actorTask ;

        private readonly IAgentId _teamId = new AgentId(1, 1);
        private readonly IAgentId _teammateId2 = new AgentId(5, 2);
        private readonly IAgentId _managerId = new AgentId(3, 3);


        [TestInitialize]
        public void Initialize()
        {
            var interactionSphereModel = new InteractionSphereModel();
            _network = new MetaNetwork(interactionSphereModel);
            _agent = new ActorEntity(_network, 2);
            _belief = new BeliefEntity(_network, 1);
            _testActorRole = new TestActorRole(_managerId, _teamId, 1);
            _actorResource = new TestActorResource(_resource.EntityId, new ResourceUsage(IsSupportOn), 100);
            _actorGroup1 = new TestActorOrganization(_agent.EntityId, 100);
            _actorGroup2 = new TestActorOrganization(_teammateId2, 100);
            _actorKnowledge = new TestActorKnowledge(_knowledge.EntityId);
            _actorTask = new TestActorTask(_agent.EntityId, _task);
        }

        [TestMethod]
        public void ClearTest()
        {
            InitializeNetwork();

            _network.Clear();

            Assert.IsFalse(_network.ActorActor.Any());
            Assert.IsFalse(_network.Organization.Any());
            Assert.IsFalse(_network.OrganizationActor.Any());
            Assert.IsFalse(_network.Role.Any());
            Assert.IsFalse(_network.Resource.Any());
            Assert.IsFalse(_network.Knowledge.Any());
            Assert.IsFalse(_network.ActorKnowledge.Any());
            Assert.IsFalse(_network.ActorTask.Any());
            Assert.IsFalse(_network.Belief.Any());
            Assert.IsFalse(_network.ActorBelief.Any());
            Assert.IsFalse(_network.ActorTask.Any());
            Assert.IsFalse(_network.ResourceTask.Any());
            Assert.IsFalse(_network.TaskKnowledge.Any());
        }

        private void InitializeNetwork()
        {
            var interaction = new TestActorActor(_agent.EntityId, _managerId);
            _network.Actor.Add(_agent);
            _network.ActorActor.AddInteraction(interaction);
            _network.OrganizationActor.AddKey(_teamId);
            _network.OrganizationActor.Add(_teamId, _actorGroup1);
            _network.ActorRole.Add(_testActorRole);
            _network.Organization.Add(_organization);
            _network.Resource.Add(_resource);
            _network.ActorResource.Add(_agent.EntityId, _actorResource);
            _network.Knowledge.Add(_knowledge);
            _network.ActorKnowledge.Add(_agent.EntityId, _actorKnowledge);
            _network.Belief.Add(_belief);
            var agentBelief = new TestActorBelief(_belief.EntityId);
            _network.ActorBelief.Add(_agent.EntityId, agentBelief);
            _network.ActorTask.Add(_agent.EntityId, _actorTask);
            _network.ResourceTask.Add(_organization.EntityId, _task);
            _network.TaskKnowledge.Add(_task.EntityId, _knowledge);
        }

        [TestMethod]
        public void RemoveMemberFromGroupTest()
        {
            _network.OrganizationActor.AddKey(_teamId);
            _network.OrganizationActor.AddValue(_teamId, _actorGroup1);
            _network.OrganizationActor.AddValue(_teamId, _actorGroup2);
            _network.ActorResource.Add(_teamId, _actorResource);
            // Method to test
            _network.RemoveActorFromOrganization(_agent.EntityId, _teamId);
            Assert.IsFalse(_network.OrganizationActor.IsActorOfOrganization(_agent.EntityId, _teamId));
            // Test link teammates
            Assert.IsFalse(_network.ActorActor.HasActiveInteraction(_agent.EntityId, _teammateId2));
            // Test group
            Assert.IsFalse(_network.OrganizationActor.IsActorOfOrganization(_agent.EntityId, _teamId));
            // Test link subordinates
            Assert.IsFalse(_network.ActorActor.HasActiveInteraction(_agent.EntityId, _managerId));
            // Portfolio
            Assert.IsFalse(_network.ActorResource.HasResource(_agent.EntityId, new ResourceUsage(IsSupportOn)));
        }

        [TestMethod]
        public void RemoveAgentTest()
        {
            InitializeNetwork();

            _network.RemoveActor(_agent.EntityId);

            Assert.IsFalse(_network.ActorActor.Any());
            Assert.AreEqual(0, _network.OrganizationActor.GetValuesCount(_teamId, _agent.EntityId.ClassId));
            Assert.IsFalse(_network.ActorRole.IsActorOf(_agent.EntityId, _teamId.ClassId));
            Assert.IsFalse(_network.ActorResource.HasResource(_agent.EntityId, _resource.EntityId, new ResourceUsage(IsWorkingOn)));
            Assert.IsFalse(_network.ActorKnowledge.Any());
            Assert.IsFalse(_network.ActorBelief.Any());
            Assert.IsFalse(_network.Actor.Any());
            Assert.IsFalse(_network.ActorTask.Any());
        }
        /// <summary>
        /// ActorOrganization doesn't exists
        /// </summary>
        [TestMethod]
        public void GetMainGroupTest()
        {
            var agentId = _network.OrganizationActor.GetMainOrganizationOrDefault(_agent.EntityId, _teamId.ClassId);
            Assert.IsNull(agentId);
        }

        [TestMethod]
        public void GetMainGroupTest1()
        {
            _network.OrganizationActor.Add(_teamId, _actorGroup1);
            Assert.AreEqual(_teamId, _network.OrganizationActor.GetMainOrganizationOrDefault(_agent.EntityId, _teamId.ClassId));
            var _ = new TestActorOrganization(_agent.EntityId, 20);
            _network.OrganizationActor.AddValue(_teamId, _actorGroup2);
            Assert.AreEqual(_teamId, _network.OrganizationActor.GetMainOrganizationOrDefault(_agent.EntityId, _teamId.ClassId));
        }
    }
}