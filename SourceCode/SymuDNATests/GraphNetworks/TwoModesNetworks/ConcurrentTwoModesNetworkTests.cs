#region Licence

// Description: SymuBiz - SymuDNATests
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace SymuDNATests.GraphNetworks.TwoModesNetworks
{
    [TestClass]
    public class ConcurrentTwoModesNetworkTests
    {
        //private readonly ITask _task = new TaskEntity(1);
        //private readonly IAgentId _agentId = new AgentId(1, 1);
        //private readonly IAgentId _agentId2 = new AgentId(2, 1);
        //private IActorTask _actorTask;
        //private IActorTask _actorTask2;
        //private readonly ActorTaskNetwork _network = new ActorTaskNetwork();

        //[TestInitialize]
        //public void Initialize()
        //{
        //    _actorTask = new ActorTask(_agentId, _task);
        //    _actorTask2 = new ActorTask(_agentId2, _task);
        //}

        //[TestMethod()]
        //public void AnyTest()
        //{
        //    Assert.IsFalse(_network.Any());
        //    _network.Add(_agentId, _actorTask);
        //    Assert.IsTrue(_network.Any());
        //}

        //[TestMethod()]
        //public void ClearTest()
        //{
        //    _network.Add(_agentId, _actorTask);
        //    _network.Clear();
        //    Assert.IsFalse(_network.Any());
        //}

        //[TestMethod()]
        //public void ExistsTest()
        //{
        //    Assert.IsFalse(_network.Exists(_agentId));
        //    _network.Add(_agentId, _actorTask);
        //    Assert.IsTrue(_network.Exists(_agentId));
        //    Assert.IsFalse(_network.Exists(_agentId2));
        //}

        //[TestMethod()]
        //public void AddTest()
        //{
        //    Assert.IsFalse(_network.Exists(_agentId, _actorTask));
        //    _network.Add(_agentId, _actorTask);
        //    Assert.IsTrue(_network.Exists(_agentId, _actorTask));
        //}

        //[TestMethod()]
        //public void AddTest1()
        //{
        //    var values = new List<IActorTask> {_actorTask};
        //    Assert.IsFalse(_network.Exists(_agentId, _actorTask));
        //    _network.Add(_agentId, values);
        //    Assert.IsTrue(_network.Exists(_agentId, _actorTask));
        //}

        //[TestMethod()]
        //public void AddValueTest()
        //{
        //    Assert.ThrowsException<ArgumentNullException>(() =>
        //        _network.AddValue(_agentId, null));
        //    Assert.ThrowsException<ArgumentNullException>(() =>
        //        _network.AddValue(_agentId, _actorTask));
        //}

        //[TestMethod()]
        //public void AddKeyTest()
        //{
        //    Assert.IsFalse(_network.Exists(_agentId));
        //    _network.AddKey(_agentId);
        //    Assert.IsTrue(_network.Exists(_agentId));
        //    Assert.IsFalse(_network.Exists(_agentId2));
        //}

        //[TestMethod()]
        //public void GetKeysTest()
        //{
        //    Assert.IsNotNull(_network.GetKeys());
        //    Assert.AreEqual(0, _network.GetKeys().Count());
        //    _network.AddKey(_agentId);
        //    Assert.AreEqual(1, _network.GetKeys().Count());
        //    _network.AddKey(_agentId2);
        //    Assert.AreEqual(2, _network.GetKeys().Count());
        //}

        //[TestMethod()]
        //public void GetValuesCountTest()
        //{
        //    Assert.AreEqual(0, _network.GetValuesCount(_agentId));
        //    _network.Add(_agentId, _actorTask);
        //    Assert.AreEqual(1, _network.GetValuesCount(_agentId));
        //    // Check duplication
        //    _network.Add(_agentId, _actorTask);
        //    Assert.AreEqual(1, _network.GetValuesCount(_agentId));
        //    _network.Add(_agentId2, _actorTask2);
        //    Assert.AreEqual(1, _network.GetValuesCount(_agentId));
        //}

        //[TestMethod()]
        //public void RemoveAgentTest()
        //{
        //    // Without key
        //    _network.RemoveActor(_agentId);
        //    // With keys
        //    _network.AddKey(_agentId);
        //    _network.AddKey(_agentId2);
        //    _network.RemoveActor(_agentId);
        //    Assert.IsFalse(_network.Exists(_agentId));
        //    Assert.IsTrue(_network.Exists(_agentId2));
        //}

        //[TestMethod()]
        //public void CopyToTest()
        //{
        //    _network.Add(_agentId, _actorTask);

        //    var copy = new ActorTaskNetwork();
        //    _network.CopyTo(copy);
        //    CollectionAssert.AreNotEqual(_network.List, copy.List);
        //    Assert.AreEqual(_network.List.Count, copy.List.Count);
        //}
    }
}