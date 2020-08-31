﻿#region Licence

// Description: SymuBiz - SymuTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.DNA.Beliefs;
using SymuDNATests.Classes;

#endregion

namespace SymuDNATests.Beliefs
{
    [TestClass]
    public class BeliefCollectionTests
    {
        private readonly TestBelief _belief =
            new TestBelief(1);

        private readonly BeliefCollection _beliefs = new BeliefCollection();

        [TestMethod]
        public void ClearTest()
        {
            _beliefs.Add(_belief);
            _beliefs.Clear();
            Assert.IsFalse(_beliefs.Any());
        }

        [TestMethod]
        public void AddTest()
        {
            Assert.IsFalse(_beliefs.Any());
            _beliefs.Add(_belief);
            Assert.IsTrue(_beliefs.Any());
        }

        [TestMethod]
        public void ContainsTest()
        {
            Assert.IsFalse(_beliefs.Contains(_belief));
            _beliefs.Add(_belief);
            Assert.IsTrue(_beliefs.Contains(_belief));
        }

        [TestMethod]
        public void GetBeliefTest()
        {
            Assert.IsNull(_beliefs.GetBelief(_belief.Id));
            _beliefs.Add(_belief);
            Assert.IsNotNull(_beliefs.GetBelief(_belief.Id));
        }

        [TestMethod]
        public void ExistsTest()
        {
            Assert.IsFalse(_beliefs.Exists(_belief.Id));
            _beliefs.Add(_belief);
            Assert.IsTrue(_beliefs.Exists(_belief.Id));
        }
    }
}