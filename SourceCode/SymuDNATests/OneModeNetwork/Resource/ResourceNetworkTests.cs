#region Licence

// Description: SymuBiz - SymuTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.Common.Interfaces.Agent;
using Symu.DNA.OneModeNetworks.Resource;
using SymuDNATests.Classes;

#endregion

namespace SymuDNATests.OneModeNetwork.Resource
{
    [TestClass]
    public class ResourceNetworkTests
    {
        private readonly TestResource _resource = new TestResource(2);
        private readonly ResourceNetwork _resources = new ResourceNetwork();

        [TestMethod]
        public void ClearTest()
        {
            _resources.Add(_resource);
            _resources.Clear();
            Assert.IsFalse(_resources.Any());
        }

        [TestMethod]
        public void ExistsTest1()
        {
            Assert.IsFalse(_resources.Exists(_resource));
            _resources.Add(_resource);
            Assert.IsTrue(_resources.Exists(_resource));
        }

        [TestMethod]
        public void AddTest()
        {
            Assert.IsFalse(_resources.Any());
            _resources.Add(_resource);
            Assert.IsTrue(_resources.Any());
            Assert.IsTrue(_resources.Exists(_resource));
        }

        [TestMethod]
        public void RemoveTest()
        {
            _resources.Remove(_resource);
            _resources.Add(_resource);
            _resources.Remove(_resource);
            Assert.IsFalse(_resources.Any());
            Assert.IsFalse(_resources.Exists(_resource));
        }
    }
}