#region Licence

// Description: SymuBiz - SymuTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.DNA.Entities;
using Symu.DNA.GraphNetworks.OneModeNetworks;
using SymuDNATests.Classes;

#endregion

namespace SymuDNATests.Networks.OneModeNetworks
{
    [TestClass]
    public class BeliefNetworkTests
    {
        private readonly BeliefEntity _belief = new BeliefEntity(1);

        private readonly BeliefNetwork _network = new BeliefNetwork();


        [TestMethod]
        public void GetBeliefTest()
        {
            Assert.IsNull(_network.GetEntity(_belief.AgentId));
            _network.Add(_belief);
            Assert.IsNotNull(_network.GetEntity(_belief.AgentId));
        }

    }
}