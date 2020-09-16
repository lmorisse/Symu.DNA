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

#endregion

namespace SymuDNATests.Networks.OneModeNetworks
{
    [TestClass]
    public class KnowledgeNetworkTests
    {
        private readonly IKnowledge _knowledge =
            new KnowledgeEntity(1);

        private readonly KnowledgeNetwork _knowledgeNetwork = new KnowledgeNetwork();

        [TestMethod]
        public void GetKnowledgeTest()
        {
            Assert.IsNull(_knowledgeNetwork.GetEntity(_knowledge.EntityId));
            _knowledgeNetwork.Add(_knowledge);
            Assert.IsNotNull(_knowledgeNetwork.GetEntity(_knowledge.EntityId));
        }
    }
}