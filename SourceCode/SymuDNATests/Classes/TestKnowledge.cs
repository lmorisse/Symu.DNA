#region Licence

// Description: SymuBiz - Symu
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

using Symu.Common.Interfaces.Entity;
using Symu.DNA.OneModeNetworks;

namespace SymuDNATests.Classes
{    /// <summary>
    ///     Describe an area of knowledge
    /// </summary>
    public class TestKnowledge : IKnowledge
    {
        private readonly UId _id;
        public TestKnowledge(UId id)
        {
            _id = id;
        }
        public TestKnowledge(ushort id)
        {
            _id = new UId(id);
        }
        /// <summary>
        ///     The unique agentId of the resource
        /// </summary>
        public IId Id => _id;
        public bool Equals(IKnowledge knowledge)
        {
            return knowledge is TestKnowledge test &&
                   Id.Equals(test.Id);
        }
    }
}