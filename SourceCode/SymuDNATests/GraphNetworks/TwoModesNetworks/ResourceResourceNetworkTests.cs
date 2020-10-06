#region Licence

// Description: SymuBiz - SymuDNATests
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.Common.Interfaces;
using Symu.DNA.Edges;
using Symu.DNA.Entities;
using Symu.DNA.GraphNetworks.TwoModesNetworks;

#endregion

namespace SymuDNATests.GraphNetworks.TwoModesNetworks
{
    [TestClass]
    public class ResourceResourceNetworkTests
    {
        private readonly ResourceResourceNetwork _network = new ResourceResourceNetwork();
        private readonly IAgentId _resourceId = new AgentId(1, 1);
        private readonly IAgentId _resourceId1 = new AgentId(2, 1);
        private readonly IResourceUsage _usage = new ResourceUsage(1);

        private IResourceResource _edge;

        [TestInitialize]
        public void Initialize()
        {
            _edge = new ResourceResource(_resourceId, _resourceId1, _usage);
        }

        [TestMethod]
        public void GetWeightTest()
        {
            Assert.AreEqual(0, _network.GetWeight(_resourceId, _resourceId1, _usage));
            _network.Add(_edge);
            Assert.AreEqual(100, _network.GetWeight(_resourceId, _resourceId1, _usage));
        }

        [TestMethod]
        public void EdgeTest()
        {
            Assert.IsNull(_network.Edge(_resourceId, _resourceId1, _usage));
            _network.Add(_edge);
            Assert.AreEqual(_edge, _network.Edge(_resourceId, _resourceId1, _usage));
        }

        [TestMethod]
        public void HasResourceTest()
        {
            Assert.IsFalse(_network.HasResource(_resourceId, _resourceId1, _usage));
            _network.Add(_edge);
            Assert.IsTrue(_network.HasResource(_resourceId, _resourceId1, _usage));
        }

        [TestMethod]
        public void TargetsFromSourceTest()
        {
            Assert.AreEqual(0, _network.TargetsFromSource(_resourceId, _usage).Count());
            _network.Add(_edge);
            Assert.AreEqual(1, _network.TargetsFromSource(_resourceId, _usage).Count());
        }

        [TestMethod]
        public void RemoveResourceTest()
        {
            //Remove empty network
            _network.RemoveResource(_resourceId);
            _network.Add(_edge);
            _network.RemoveResource(_resourceId);
            Assert.IsFalse(_network.Any());
        }
    }
}