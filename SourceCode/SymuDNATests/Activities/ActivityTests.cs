﻿#region Licence

// Description: SymuBiz - SymuTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SymuDNATests.Classes;

#endregion

namespace SymuDNATests.Activities
{
    [TestClass]
    public class ActivityTests
    {
        private readonly TestActivity _activity = new TestActivity("a");

        private readonly TestKnowledge _knowledge =
            new TestKnowledge(1, 10);

        [TestMethod]
        public void AddKnowledgeTest()
        {
            Assert.IsFalse(_activity.Knowledges.Contains(_knowledge));
            _activity.AddKnowledge(_knowledge);
            Assert.IsTrue(_activity.Knowledges.Contains(_knowledge));
            _activity.AddKnowledge(_knowledge); //handle duplicate
            Assert.AreEqual(1, _activity.Knowledges.Count);
        }
    }
}