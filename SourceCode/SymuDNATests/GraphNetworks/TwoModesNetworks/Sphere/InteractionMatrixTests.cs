#region Licence

// Description: SymuBiz - SymuDNATests
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.Common.Interfaces;
using Symu.DNA.Edges;
using Symu.DNA.GraphNetworks;
using Symu.DNA.GraphNetworks.TwoModesNetworks;
using Symu.DNA.GraphNetworks.TwoModesNetworks.Sphere;

#endregion

namespace SymuDNATests.GraphNetworks.TwoModesNetworks.Sphere
{
    [TestClass]
    public class InteractionMatrixTests
    {
        private readonly IAgentId _actorId = new AgentId(1, 1);
        private readonly IAgentId _actorId1 = new AgentId(2, 1);
        private readonly IAgentId _actorId2 = new AgentId(3, 1);
        private readonly List<IAgentId> _actors = new List<IAgentId>();

        private readonly IAgentId _info = new AgentId(4, 2);

        private readonly IAgentId _info1 = new AgentId(5, 2);

        private readonly IAgentId _info2 = new AgentId(6, 2);
        private EntityKnowledge _actorKnowledge;
        private EntityKnowledge _actorKnowledge1;
        private EntityKnowledge _actorKnowledge2;

        private MetaNetwork _network;

        private ActorKnowledgeNetwork _networkKnowledge;

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
            _actorKnowledge = new EntityKnowledge(_actorId, _info);
            _actorKnowledge1 = new EntityKnowledge(_actorId1, _info1);
            _actorKnowledge2 = new EntityKnowledge(_actorId2, _info2);
        }

        private void Interaction1X1()
        {
            _actors.Add(_actorId);
            _networkKnowledge.Add(_actorKnowledge);
            _network.InteractionSphere.SetSphere(true, _actors, _network);
        }

        private void NoInteraction2X2()
        {
            Interaction1X1();
            _actors.Add(_actorId1);
            _networkKnowledge.Add(_actorKnowledge1);
            _network.InteractionSphere.SetSphere(true, _actors, _network);
        }

        private void NoInteraction3X3()
        {
            NoInteraction2X2();
            _actors.Add(_actorId2);
            _networkKnowledge.Add(_actorKnowledge2);
            _network.InteractionSphere.SetSphere(true, _actors, _network);
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
            _networkKnowledge.Add(new EntityKnowledge(_actorId1, _info));
            _networkKnowledge.Add(new EntityKnowledge(_actorId, _info1));
            _network.InteractionSphere.SetSphere(true, _actors, _network);

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
                var actorId = new AgentId(i, 1);
                _actors.Add(actorId);
                _networkKnowledge.Add(new EntityKnowledge(actorId, _info));
            }

            _network.InteractionSphere.SetSphere(true, _actors, _network);
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
                    new AgentId(i, 2);
                var actorId = new AgentId(i, 1);
                _actors.Add(actorId);
                var agentKnowledge = new EntityKnowledge(actorId, info);
                _networkKnowledge.Add(agentKnowledge);
            }

            _network.InteractionSphere.SetSphere(true, _actors, _network);
            Assert.AreEqual((uint) 0, InteractionMatrix.NumberOfTriads(_network.InteractionSphere.Sphere));
        }

        #endregion
    }
}