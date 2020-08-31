#region Licence

// Description: SymuBiz - SymuTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.DNA.OneModeNetworks.Belief;
using SymuDNATests.Classes;

#endregion

namespace SymuDNATests.OneModeNetwork.Belief
{
    [TestClass]
    public class NetworkBeliefsTests
    {
        private readonly TestBelief _belief = new TestBelief(1);

        private readonly BeliefNetwork _network = new BeliefNetwork();


        [TestMethod]
        public void AddBeliefTest()
        {
            Assert.IsFalse(_network.Exists(_belief));
            _network.AddBelief(_belief);
            Assert.IsTrue(_network.Exists(_belief));
        }

        [TestMethod]
        public void AddBeliefTest1()
        {
            Assert.IsFalse(_network.Exists(_belief.Id));
            _network.AddBelief(_belief);
            Assert.IsTrue(_network.Exists(_belief.Id));
        }

        [TestMethod]
        public void AnyTest()
        {
            Assert.IsFalse(_network.Any());
            _network.AddBelief(_belief);
            Assert.IsTrue(_network.Any());
        }

        [TestMethod]
        public void ClearTest()
        {
            _network.AddBelief(_belief);
            _network.Clear();
            Assert.IsFalse(_network.Any());
        }

        [TestMethod]
        public void GetBeliefTest()
        {
            Assert.IsNull(_network.GetBelief(_belief.Id));
            _network.AddBelief(_belief);
            Assert.IsNotNull(_network.GetBelief(_belief.Id));
        }

     
    }
}