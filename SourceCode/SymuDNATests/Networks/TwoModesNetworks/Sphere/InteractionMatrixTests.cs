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
using Symu.DNA.Networks.TwoModesNetworks.AgentKnowledge;
using Symu.DNA.Networks.TwoModesNetworks.Sphere;
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

        private readonly TestKnowledge _info1 =
            new TestKnowledge(1);

        private readonly TestKnowledge _info2 =
            new TestKnowledge(2);

        private readonly TestKnowledge _info3 =
            new TestKnowledge(3);

        private Symu.DNA.Networks.MetaNetwork _network;

        private AgentKnowledgeNetwork _networkKnowledge;
        private TestAgentKnowledge _agentKnowledge;
        private TestAgentKnowledge _agentKnowledge2;
        private TestAgentKnowledge _agentKnowledge3;

        [TestInitialize]
        public void Initialize()
        {
            //_templates.Human.Cognitive.InteractionPatterns.SetInteractionPatterns(InteractionStrategy.Knowledge);

            var interactionSphereModel = new InteractionSphereModel { On = true };
            _network = new Symu.DNA.Networks.MetaNetwork(interactionSphereModel);
            _networkKnowledge = _network.AgentKnowledge;
            _agentKnowledge = new TestAgentKnowledge(_info1.Id, 1);
            _agentKnowledge2 = new TestAgentKnowledge(_info2.Id, 1);
            _agentKnowledge3 = new TestAgentKnowledge(_info3.Id, 1);
        }

        private void Interaction1X1()
        {
            _actors.Add(_agentId1);
            _networkKnowledge.Add(_agentId1, _agentKnowledge);
            _network.InteractionSphere.SetSphere(true, Actors, _network);
        }

        private void NoInteraction2X2()
        {
            Interaction1X1();
            _actors.Add(_agentId2);
            _networkKnowledge.Add(_agentId2, _agentKnowledge2);
            _network.InteractionSphere.SetSphere(true, Actors, _network);
        }

        private void NoInteraction3X3()
        {
            NoInteraction2X2();
            _actors.Add(_agentId3);
            _networkKnowledge.Add(_agentId3, _agentKnowledge3);
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
            _networkKnowledge.Add(_agentId1, _agentKnowledge2);
            _networkKnowledge.Add(_agentId2, _agentKnowledge);
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
                _networkKnowledge.Add(agentId, _agentKnowledge);
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
                    new TestKnowledge(i);
                var agentId = new AgentId(i, 1);
                _actors.Add(agentId);
                var agentKnowledge = new TestAgentKnowledge(info.Id, 1);
                _networkKnowledge.Add(agentId, agentKnowledge);
            }

            _network.InteractionSphere.SetSphere(true, Actors, _network);
            Assert.AreEqual((uint) 0, InteractionMatrix.NumberOfTriads(_network.InteractionSphere.Sphere));
        }

        #endregion
    }
}