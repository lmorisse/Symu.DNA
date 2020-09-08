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

namespace SymuDNATests.Networks.OneModeNetworks
{
    [TestClass]
    public class ResourceNetworkTests
    {
        private readonly TestResource _resource = new TestResource(2);
        private readonly ResourceNetwork _resources = new ResourceNetwork();


        [TestMethod]
        public void RemoveTest()
        {
            _resources.Remove(_resource);
            _resources.Add(_resource);
            _resources.Remove(_resource);
            Assert.IsFalse(_resources.Any());
            Assert.IsFalse(_resources.Exists(_resource));
        }


        [TestMethod]
        public void GetTest()
        {
            Assert.IsNull(_resources.Get(_resource.Id));
            _resources.Add(_resource);
            Assert.IsNotNull(_resources.Get(_resource.Id));
            Assert.AreEqual(_resource, _resources.Get(_resource.Id));
        }
    }
}