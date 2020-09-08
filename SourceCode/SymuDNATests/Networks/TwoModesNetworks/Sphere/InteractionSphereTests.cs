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
using Symu.DNA.Networks;
using Symu.DNA.Networks.TwoModesNetworks.Sphere;
using SymuDNATests.Classes;

#endregion

namespace SymuDNATests.Networks.TwoModesNetworks.Sphere
{
    [TestClass]
    public class InteractionSphereTests
    {
        private readonly TestTask _task = new TestTask(1);
        private readonly AgentId _agentId1 = new AgentId(1, 1);
        private readonly AgentId _agentId2 = new AgentId(2, 1);
        private readonly List<IAgentId> _agents = new List<IAgentId>();
        private readonly TestBelief _belief = new TestBelief(1);
        private readonly TestKnowledge _knowledge = new TestKnowledge(1);
        private InteractionSphere InteractionSphere => _network.InteractionSphere;
        private MetaNetwork _network;
        private InteractionSphereModel _interactionSphereModel;

        [TestInitialize]
        public void Initialize()
        {
            _interactionSphereModel = new InteractionSphereModel {On = true};
            _network = new MetaNetwork(_interactionSphereModel);
            _agents.Add(_agentId1);
            _agents.Add(_agentId2);
            _network.Knowledge.Add(_knowledge);
            _network.Belief.Add(_belief);
            _network.Task.Add(_task);
        }

        /// <summary>
        ///     No interaction
        /// </summary>
        [TestMethod]
        public void GeAgentIdsForInteractionsTest()
        {
            InteractionSphere.SetSphere(true, _agents, _network);
            Assert.AreEqual(0, InteractionSphere.GetAgentIdsForInteractions(_agentId1, InteractionStrategy.Homophily).Count());
        }

        /// <summary>
        ///     Without link & !_model.SphereUpdateOverTime
        /// </summary>
        [TestMethod]
        public void GeAgentIdsForNewInteractionsTest()
        {
            InteractionSphere.SetSphere(true, _agents, _network);
            Assert.AreEqual(0,
                InteractionSphere.GetAgentIdsForNewInteractions(_agentId1, InteractionStrategy.Homophily).Count());
        }

        /// <summary>
        ///     Without link & _model.SphereUpdateOverTime
        /// </summary>
        [TestMethod]
        public void GeAgentIdsForNewInteractionsTest1()
        {
            _interactionSphereModel.SphereUpdateOverTime = true;
            InteractionSphere.SetSphere(true, _agents, _network);
            Assert.AreEqual(1,
                InteractionSphere.GetAgentIdsForNewInteractions(_agentId1, InteractionStrategy.Homophily).Count());
        }

        /// <summary>
        ///     With link
        /// </summary>
        [TestMethod]
        public void GeAgentIdsForNewInteractionsTest2()
        {
            _interactionSphereModel.SphereUpdateOverTime = true;
            AddLink();
            InteractionSphere.SetSphere(true, _agents, _network);
            Assert.AreEqual(0,
                InteractionSphere.GetAgentIdsForNewInteractions(_agentId1, InteractionStrategy.Homophily).Count());
        }

        /// <summary>
        ///     With no interaction
        /// </summary>
        [TestMethod]
        public void GetSphereWeightTest()
        {
            InteractionSphere.SetSphere(true, _agents, _network);
            Assert.AreEqual(0, InteractionSphere.GetSphereWeight());
        }

        /// <summary>
        ///     With full interaction
        /// </summary>
        [TestMethod]
        public void GetSphereWeightTest1()
        {
            AddBelief(_agentId1, 1);
            AddBelief(_agentId2, 1);
            AddKnowledge(_agentId1, 1);
            AddKnowledge(_agentId2, 1);
            AddLink();
            AddActivity(_agentId1);
            AddActivity(_agentId2);
            InteractionSphere.SetSphere(true, _agents, _network);
            Assert.AreEqual(InteractionSphere.GetMaxSphereWeight(), InteractionSphere.GetSphereWeight());
        }

        [TestMethod]
        public void GetMaxSphereWeightTest()
        {
            InteractionSphere.SetSphere(true, _agents, _network);
            Assert.AreEqual(8, InteractionSphere.GetMaxSphereWeight());
        }

        #region common

        private void AddLink()
        {
            var interaction = new TestAgentAgent(_agentId1, _agentId2);
            _network.AgentAgent.AddInteraction(interaction);
        }

        private void AddKnowledge(AgentId agentId, float knowledgeValue)
        {
            var agentKnowledge = new TestAgentKnowledge(_knowledge.Id);
            _network.AgentKnowledge.Add(agentId, agentKnowledge);
            agentKnowledge.Value = knowledgeValue;
        }

        private void AddActivity(AgentId agentId)
        {
            var agentActivity = new TestAgentTask(agentId, _task);
            _network.AgentTask.Add(agentId, agentActivity);
        }

        private void AddBelief(AgentId agentId, float beliefValue)
        {
            var agentBelief = new TestAgentBelief(_belief.Id);
            _network.AgentBelief.Add(agentId, agentBelief);
            agentBelief.Value = beliefValue;
        }

        #endregion

        #region Homophily

        /// <summary>
        ///     Empty network
        /// </summary>
        [TestMethod]
        public void GetHomophilyTest()
        {
            InteractionSphere.SetSphere(true, _agents, _network);
            Assert.AreEqual(0, InteractionSphere.GetHomophily(_agentId1, _agentId2));
        }

        /// <summary>
        ///     Linked agents
        /// </summary>
        [TestMethod]
        public void GetHomophilyTest1()
        {
            AddLink();
            _interactionSphereModel.SetInteractionPatterns(InteractionStrategy.SocialDemographics);
            InteractionSphere.SetSphere(true, _agents, _network);
            Assert.AreEqual(1, InteractionSphere.GetHomophily(_agentId1, _agentId2));
        }

        /// <summary>
        ///     Knowledge
        /// </summary>
        [TestMethod]
        public void GetHomophilyTest2()
        {
            AddKnowledge(_agentId1, 1);
            AddKnowledge(_agentId2, 1);
            InteractionSphere.SetSphere(true, _agents, _network);
            Assert.AreEqual(1, InteractionSphere.GetHomophily(_agentId1, _agentId2));
        }

        /// <summary>
        ///     Belief
        /// </summary>
        [TestMethod]
        public void GetHomophilyTest3()
        {
            AddBelief(_agentId1, 1);
            AddBelief(_agentId2, 1);
            InteractionSphere.SetSphere(true, _agents, _network);
            Assert.AreEqual(1, InteractionSphere.GetHomophily(_agentId1, _agentId2));
        }

        #endregion

        #region Belief

        /// <summary>
        ///     Without belief
        /// </summary>
        [TestMethod]
        public void SetRelativeBeliefTest()
        {
            Assert.AreEqual(0, InteractionSphere.SetRelativeBelief(_agentId1, _agentId2, _network.AgentBelief));
        }

        /// <summary>
        ///     With same belief 1
        /// </summary>
        [TestMethod]
        public void SetRelativeBeliefTest1()
        {
            AddBelief(_agentId1, 1);
            AddBelief(_agentId2, 1);
            Assert.AreEqual(1, InteractionSphere.SetRelativeBelief(_agentId1, _agentId2, _network.AgentBelief));
        }

        /// <summary>
        ///     With same belief -1
        /// </summary>
        [TestMethod]
        public void SetRelativeBeliefTest2()
        {
            AddBelief(_agentId1, -1);
            AddBelief(_agentId2, -1);
            Assert.AreEqual(1, InteractionSphere.SetRelativeBelief(_agentId1, _agentId2, _network.AgentBelief));
        }

        /// <summary>
        ///     With same belief 0
        /// </summary>
        [TestMethod]
        public void SetRelativeBeliefTest3()
        {
            AddBelief(_agentId1, 0);
            AddBelief(_agentId2, 0);
            Assert.AreEqual(0, InteractionSphere.SetRelativeBelief(_agentId1, _agentId2, _network.AgentBelief));
        }

        /// <summary>
        ///     With different belief
        /// </summary>
        [TestMethod]
        public void SetRelativeBeliefTest4()
        {
            AddBelief(_agentId1, -1);
            AddBelief(_agentId2, 1);
            Assert.AreEqual(-1, InteractionSphere.SetRelativeBelief(_agentId1, _agentId2, _network.AgentBelief));
        }

        #endregion

        #region Knowledge

        /// <summary>
        ///     Without knowledge
        /// </summary>
        [TestMethod]
        public void SetRelativeExpertiseTest()
        {
            Assert.AreEqual(0,
                InteractionSphere.SetRelativeKnowledge(_agentId1, _agentId2, _network.AgentKnowledge));
        }

        /// <summary>
        ///     With different knowledge level
        /// </summary>
        [TestMethod]
        public void SetRelativeExpertiseTest1()
        {
            AddKnowledge(_agentId1, 1);
            AddKnowledge(_agentId2, 0);
            Assert.AreEqual(0,
                InteractionSphere.SetRelativeKnowledge(_agentId1, _agentId2, _network.AgentKnowledge));
        }

        /// <summary>
        ///     With same knowledge level
        /// </summary>
        [TestMethod]
        public void SetRelativeExpertiseTest2()
        {
            AddKnowledge(_agentId1, 1);
            AddKnowledge(_agentId2, 1);
            Assert.AreEqual(1,
                InteractionSphere.SetRelativeKnowledge(_agentId1, _agentId2, _network.AgentKnowledge));
        }

        #endregion

        #region SocialProximity

        /// <summary>
        ///     Without link
        /// </summary>
        [TestMethod]
        public void SetSocialProximityTest()
        {
            _network.AgentAgent.SetMaxLinksCount();
            Assert.AreEqual(0, InteractionSphere.SetSocialProximity(_agentId1, _agentId2, _network.AgentAgent));
        }

        /// <summary>
        ///     With active link
        /// </summary>
        [TestMethod]
        public void SetSocialProximityTest1()
        {
            AddLink();
            _network.AgentAgent.SetMaxLinksCount();
            Assert.AreEqual(1, InteractionSphere.SetSocialProximity(_agentId1, _agentId2, _network.AgentAgent));
        }

        /// <summary>
        ///     With passive link
        /// </summary>
        [TestMethod]
        public void SetSocialProximityTest2()
        {
            var networkLink = new TestAgentAgent(_agentId1, _agentId2);
            networkLink.DecreaseWeight();
            _network.AgentAgent.AddInteraction(networkLink);
            Assert.AreEqual(0F, InteractionSphere.SetSocialProximity(_agentId1, _agentId2, _network.AgentAgent));
        }

        #endregion

        #region Activities

        /// <summary>
        ///     Without Activity
        /// </summary>
        [TestMethod]
        public void SetRelativeActivityTest()
        {
            Assert.AreEqual(0, InteractionSphere.SetRelativeActivity(_agentId1, _agentId2, _network.AgentTask));
        }

        /// <summary>
        ///     With different activities level
        /// </summary>
        [TestMethod]
        public void SetRelativeActivityTest1()
        {
            AddActivity(_agentId1);
            AddActivity(_agentId2);
            Assert.AreEqual(1, InteractionSphere.SetRelativeActivity(_agentId1, _agentId2, _network.AgentTask));
        }

        #endregion
    }
}