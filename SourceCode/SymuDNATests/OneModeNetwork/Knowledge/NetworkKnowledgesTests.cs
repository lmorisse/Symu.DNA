#region Licence

// Description: SymuBiz - SymuTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.Common.Interfaces.Agent;
using Symu.Common.Interfaces.Entity;
using Symu.DNA.OneModeNetworks.Knowledge;
using SymuDNATests.Classes;

#endregion

namespace SymuDNATests.OneModeNetwork.Knowledge
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
            Assert.IsNull(_knowledgeNetwork.GetKnowledge(_knowledge.Id));
            _knowledgeNetwork.AddKnowledge(_knowledge);
            Assert.IsNotNull(_knowledgeNetwork.GetKnowledge(_knowledge.Id));
        }

       

        [TestMethod]
        public void ClearTest()
        {
            _knowledgeNetwork.AddKnowledge(_knowledge);
            _knowledgeNetwork.Clear();
            Assert.IsFalse(_knowledgeNetwork.Any());
        }

     
    }
}