#region Licence

// Description: SymuBiz - SymuTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.DNA.Entities;
using Symu.DNA.GraphNetworks.TwoModesNetworks;
using SymuDNATests.Classes;

#endregion


namespace SymuDNATests.Networks.TwoModesNetworks
{
    [TestClass]
    public class AgentBeliefsTests
    {
        private TestActorBelief _actorBelief ;
        private readonly BeliefEntity _belief = new BeliefEntity(1);
        private readonly ActorBeliefs _beliefs = new ActorBeliefs();

        [TestInitialize]
        public void Initialize()
        {
            _actorBelief = new TestActorBelief(_belief.AgentId);
        }

        [TestMethod]
        public void AddTest()
        {
            Assert.AreEqual(0, _beliefs.Count);
            _beliefs.Add(_actorBelief);
            Assert.AreEqual(1, _beliefs.Count);
            // Duplicate
            _beliefs.Add(_actorBelief);
            Assert.AreEqual(1, _beliefs.Count);
        }

        [TestMethod]
        public void AddTest1()
        {
            Assert.AreEqual(0, _beliefs.Count);
            _beliefs.Add(_actorBelief);
            Assert.AreEqual(1, _beliefs.Count);
            // Duplicate
            _beliefs.Add(_actorBelief);
            Assert.AreEqual(1, _beliefs.Count);
        }

        [TestMethod]
        public void ContainsTest()
        {
            Assert.IsFalse(_beliefs.Contains(_belief.AgentId));
            _beliefs.Add(_actorBelief);
            Assert.IsTrue(_beliefs.Contains(_belief.AgentId));
        }

        [TestMethod]
        public void ContainsTest1()
        {
            Assert.IsFalse(_beliefs.Contains(_actorBelief));
            _beliefs.Add(_actorBelief);
            Assert.IsTrue(_beliefs.Contains(_actorBelief));
        }

        [TestMethod]
        public void GetBeliefTest()
        {
            Assert.IsNull(_beliefs.GetActorBelief(_belief.AgentId));
            _beliefs.Add(_actorBelief);
            Assert.IsNotNull(_beliefs.GetActorBelief(_belief.AgentId));
        }

    }
}