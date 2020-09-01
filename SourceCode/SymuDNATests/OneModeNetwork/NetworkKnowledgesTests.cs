#region Licence

// Description: SymuBiz - SymuTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.DNA.OneModeNetworks;
using SymuDNATests.Classes;

#endregion

namespace SymuDNATests.OneModeNetwork
{
    [TestClass]
    public class NetworkKnowledgesTests
    {
        private readonly TestKnowledge _knowledge =
            new TestKnowledge(1);

        private readonly KnowledgeNetwork _knowledgeNetwork = new KnowledgeNetwork();

        [TestMethod]
        public void GetKnowledgeTest()
        {
            Assert.IsNull(_knowledgeNetwork.Get(_knowledge.Id));
            _knowledgeNetwork.Add(_knowledge);
            Assert.IsNotNull(_knowledgeNetwork.Get(_knowledge.Id));
        }

       

        [TestMethod]
        public void ClearTest()
        {
            _knowledgeNetwork.Add(_knowledge);
            _knowledgeNetwork.Clear();
            Assert.IsFalse(_knowledgeNetwork.Any());
        }

     
    }
}