#region Licence

// Description: SymuBiz - SymuDNATests
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.Common.Interfaces;
using Symu.DNA.Edges;
using Symu.DNA.Entities;

#endregion

namespace SymuDNATests.Edges
{
    [TestClass]
    public class OrganizationResourceTests
    {
        private readonly IAgentId _organizationId = new AgentId(1, 1);
        private readonly IAgentId _resourceId = new AgentId(2, 2);
        private readonly IResourceUsage _usage = new ResourceUsage(1);
        private readonly IResourceUsage _usage1 = new ResourceUsage(2);

        private IOrganizationResource _edge;
        private IOrganizationResource _edge1;
        private IOrganizationResource _edge2;

        [TestInitialize]
        public void Initialize()
        {
            _edge = new OrganizationResource(_organizationId, _resourceId, _usage, 1);
            _edge1 = new OrganizationResource(_organizationId, _resourceId, _usage1, 1);
            _edge2 = new OrganizationResource(_organizationId, _resourceId, _usage, 0);
        }

        [TestMethod]
        public void EqualsSourceTest()
        {
            Assert.IsTrue(_edge.EqualsSource(_organizationId));
            Assert.IsFalse(_edge.EqualsSource(_resourceId));
        }

        [TestMethod]
        public void EqualsTargetTest()
        {
            Assert.IsFalse(_edge.EqualsTarget(_organizationId));
            Assert.IsTrue(_edge.EqualsTarget(_resourceId));
        }

        [TestMethod]
        public void CloneTest()
        {
            var clone = _edge.Clone();
            Assert.AreEqual(_edge, clone);
        }

        [TestMethod]
        public void EqualsTest()
        {
            Assert.IsFalse(_edge.Equals(_edge1.Usage));
            Assert.IsTrue(_edge.Equals(_edge.Usage));
        }

        [TestMethod]
        public void EqualsTest1()
        {
            Assert.IsFalse(_edge.Equals(_edge1));
            Assert.IsTrue(_edge.Equals(_edge2));
        }
    }
}