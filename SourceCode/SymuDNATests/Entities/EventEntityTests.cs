#region Licence

// Description: SymuBiz - SymuDNATests
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Symu.DNA.Entities;
using Symu.DNA.GraphNetworks;

#endregion

namespace SymuDNATests.Entities
{
    [TestClass]
    public class EventEntityTests
    {
        private readonly MetaNetwork _metaNetwork = new MetaNetwork();
        private IEvent _entity;

        [TestInitialize]
        public void Initialize()
        {
            _entity = new EventEntity(_metaNetwork);
        }

        [TestMethod]
        public void RemoveTest()
        {
            _entity.Remove();

            Assert.IsFalse(_metaNetwork.Event.Any());
        }
    }
}