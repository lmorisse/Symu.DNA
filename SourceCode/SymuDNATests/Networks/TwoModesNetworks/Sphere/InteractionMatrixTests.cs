#region Licence

// Description: SymuBiz - SymuTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.Common.Interfaces.Agent;
using Symu.DNA.Entities;
using Symu.DNA.GraphNetworks;
using Symu.DNA.GraphNetworks.TwoModesNetworks;
using Symu.DNA.GraphNetworks.TwoModesNetworks.Sphere;
using SymuDNATests.Classes;

#endregion

namespace SymuDNATests.Networks.TwoModesNetworks.Sphere
{
    [TestClass]
    public class InteractionMatrixTests
    {
        private readonly List<AgentId> _actors = new List<AgentId>();
        private List<IAgentId> Actors => _actors.Cast<IAgentId>().ToList();
        private readonly AgentId _agentId1 = new AgentId(1, 1);
        private readonly AgentId _agentId2 = new AgentId(2, 1);
        private readonly AgentId _agentId3 = new AgentId(3, 1);

        private readonly KnowledgeEntity _info1 =
            new KnowledgeEntity(1);

        private readonly KnowledgeEntity _info2 =
            new KnowledgeEntity(2);

        private readonly KnowledgeEntity _info3 =
            new KnowledgeEntity(3);

        private MetaNetwork _network;

        private ActorKnowledgeNetwork _networkKnowledge;
        private TestActorKnowledge _actorKnowledge;
        private TestActorKnowledge _actorKnowledge2;
        private TestActorKnowledge _actorKnowledge3;

        [TestInitialize]
        public void Initialize()
        {
            var interactionSphereModel = new InteractionSphereModel
            {
                On = true,
                RelativeActivityWeight = 0,
                RelativeBeliefWeight = 0,
                SocialDemographicWeight = 0
            };
            _network = new MetaNetwork(interactionSphereModel);
            _networkKnowledge = _network.ActorKnowledge;
            _actorKnowledge = new TestActorKnowledge(_info1.AgentId, 1);
            _actorKnowledge2 = new TestActorKnowledge(_info2.AgentId, 1);
            _actorKnowledge3 = new TestActorKnowledge(_info3.AgentId, 1);
        }

        private void Interaction1X1()
        {
            _actors.Add(_agentId1);
            _networkKnowledge.Add(_agentId1, _actorKnowledge);
            _network.InteractionSphere.SetSphere(true, Actors, _network);
        }

        private void NoInteraction2X2()
        {
            Interaction1X1();
            _actors.Add(_agentId2);
            _networkKnowledge.Add(_agentId2, _actorKnowledge2);
            _network.InteractionSphere.SetSphere(true, Actors, _network);
        }

        private void NoInteraction3X3()
        {
            NoInteraction2X2();
            _actors.Add(_agentId3);
            _networkKnowledge.Add(_agentId3, _actorKnowledge3);
            _network.InteractionSphere.SetSphere(true, Actors, _network);
        }

        [TestMethod]
        public void MaxTriadsTest()
        {
            Assert.AreEqual((uint) 0, InteractionMatrix.MaxTriads(1));
            Assert.AreEqual((uint) 0, InteractionMatrix.MaxTriads(2));
            Assert.AreEqual((uint) 1, InteractionMatrix.MaxTriads(3));
            Assert.AreEqual((uint) 4, InteractionMatrix.MaxTriads(4));
            Assert.AreEqual((uint) 10, InteractionMatrix.MaxTriads(5));
            Assert.AreEqual((uint) 20, InteractionMatrix.MaxTriads(6));
        }

        #region Average interaction

        [TestMethod]
        public void AverageInteraction1X1Test()
        {
            Interaction1X1();
            Assert.AreEqual(0,
                InteractionMatrix.GetAverageInteractionMatrix(
                    InteractionMatrix.GetInteractionMatrix(_network.InteractionSphere.Sphere)));
        }

        [TestMethod]
        public void Average0Interaction2X2Test()
        {
            NoInteraction2X2();

            Assert.AreEqual(0,
                InteractionMatrix.GetAverageInteractionMatrix(
                    InteractionMatrix.GetInteractionMatrix(_network.InteractionSphere.Sphere)));
        }

        [TestMethod]
        public void Average1Interaction2X2Test()
        {
            NoInteraction2X2();
            _networkKnowledge.Add(_agentId1, _actorKnowledge2);
            _networkKnowledge.Add(_agentId2, _actorKnowledge);
            _network.InteractionSphere.SetSphere(true, Actors, _network);

            Assert.AreEqual(1F,
                InteractionMatrix.GetAverageInteractionMatrix(
                    InteractionMatrix.GetInteractionMatrix(_network.InteractionSphere.Sphere)));
        }

        [TestMethod]
        public void Average0Interaction3X3Test()
        {
            NoInteraction3X3();

            Assert.AreEqual(0,
                InteractionMatrix.GetAverageInteractionMatrix(
                    InteractionMatrix.GetInteractionMatrix(_network.InteractionSphere.Sphere)));
        }

        #endregion

        #region triad

        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        [DataRow(4)]
        [DataRow(5)]
        [DataRow(6)]
        [TestMethod]
        public void SameKnowledgeNumberEqualMaxTriads(int count)
        {
            for (ushort i = 0; i < count; i++)
            {
                var agentId = new AgentId(i, 1);
                _actors.Add(agentId);
                _networkKnowledge.Add(agentId, _actorKnowledge);
            }

            _network.InteractionSphere.SetSphere(true, Actors, _network);
            Assert.AreEqual(InteractionMatrix.MaxTriads(count),
                InteractionMatrix.NumberOfTriads(_network.InteractionSphere.Sphere));
        }

        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        [DataRow(4)]
        [DataRow(5)]
        [DataRow(6)]
        [TestMethod]
        public void DifferentKnowledgeNumberEqualMaxTriads(int count)
        {
            for (ushort i = 0; i < count; i++)
            {
                var info =
                    new KnowledgeEntity(i);
                var agentId = new AgentId(i, 1);
                _actors.Add(agentId);
                var agentKnowledge = new TestActorKnowledge(info.AgentId, 1);
                _networkKnowledge.Add(agentId, agentKnowledge);
            }

            _network.InteractionSphere.SetSphere(true, Actors, _network);
            Assert.AreEqual((uint) 0, InteractionMatrix.NumberOfTriads(_network.InteractionSphere.Sphere));
        }

        #endregion
    }
}