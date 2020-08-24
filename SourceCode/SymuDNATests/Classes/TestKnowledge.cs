#region Licence

// Description: SymuBiz - Symu
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

using Symu.Common.Interfaces.Entity;

namespace SymuDNATests.Classes
{    /// <summary>
    ///     Describe an area of knowledge
    /// </summary>
    public class TestKnowledge : IKnowledge
    {
        private readonly UId _id;
        public TestKnowledge(UId id, byte length)
        {
            _id = id;
            Length = length;
        }
        public TestKnowledge(ushort id, byte length)
        {
            _id = new UId(id);
            Length = length;
        }
        /// <summary>
        ///     The unique agentId of the resource
        /// </summary>
        public IId Id => _id;

        /// <summary>
        ///     Each area of knowledge is represented by a collection of KnowledgeBits
        ///     The size define the length of the collection
        ///     each bit represent a single atomic fact
        ///     size range [0; 10]
        /// </summary>
        public byte Length { get; }
        public bool Equals(IKnowledge knowledge)
        {
            return knowledge is TestKnowledge test &&
                   Id.Equals(test.Id);
        }
    }
}