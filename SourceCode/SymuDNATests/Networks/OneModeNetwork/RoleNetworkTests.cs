#region Licence

// Description: SymuBiz - SymuTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.DNA.Networks.OneModeNetworks;
using SymuDNATests.Classes;

#endregion

namespace SymuDNATests.Networks.OneModeNetwork
{
    [TestClass]
    public class RoleNetworkTests
    {
        private readonly RoleNetwork _roles = new RoleNetwork();
        private readonly TestRole _role = new TestRole(1);


        [TestMethod]
        public void ExistsTest()
        {
            Assert.IsFalse(_roles.Exists(_role));
            _roles.Add(_role);
            Assert.IsTrue(_roles.Exists(_role));
        }
        [TestMethod]
        public void ClearTest()
        {
            _roles.Add(_role);
            _roles.Clear();
            Assert.IsFalse(_roles.Any());
        }


        [TestMethod]
        public void AddTest()
        {
            Assert.IsFalse(_roles.Exists(_role));
            Assert.IsFalse(_roles.Any());
            _roles.Add(_role);
            Assert.IsTrue(_roles.Any());
            Assert.IsTrue(_roles.Exists(_role));
        }
    }
}