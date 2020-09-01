#region Licence

// Description: SymuBiz - SymuTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.Common.Interfaces.Entity;
using Symu.DNA.OneModeNetworks.Resource;
using SymuDNATests.Classes;

#endregion

namespace SymuDNATests.OneModeNetwork.Resource
{
    [TestClass]
    public class ResourceCollectionTests
    {
        private readonly ResourceCollection _collection =
            new ResourceCollection();

        private TestResource _resource;

        [TestInitialize]
        public void Initialize()
        {
            _resource = new TestResource(new UId(1));
        }

        [TestMethod]
        public void AddTest()
        {
            Assert.IsFalse(_collection.Exists(_resource.Id));
            _collection.Add(_resource);
            Assert.IsTrue(_collection.Exists(_resource.Id));
            // Duplicate
            _collection.Add(_resource);
            Assert.AreEqual(1, _collection.List.Count);
        }

        [TestMethod]
        public void ContainsTest()
        {
            Assert.IsFalse(_collection.Contains(_resource));
            _collection.Add(_resource);
            Assert.IsTrue(_collection.Contains(_resource));
        }

        [TestMethod]
        public void GetDatabaseTest()
        {
            Assert.IsNull(_collection.Get(_resource.Id));
            _collection.Add(_resource);
            Assert.IsNotNull(_collection.Get(_resource.Id));
            Assert.AreEqual(_resource, _collection.Get(_resource.Id));
        }

        [TestMethod]
        public void ClearTest()
        {
            _collection.Add(_resource);
            _collection.Clear();
            Assert.IsFalse(_collection.Contains(_resource));
        }
    }
}