#region Licence

// Description: SymuBiz - Symu
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

using Symu.Common.Interfaces.Entity;
using Symu.DNA.Beliefs;

namespace SymuDNATests.Classes
{
    /// <summary>
    ///     Defines a belief
    /// </summary>
    public class TestBelief: IBelief
    {
        public TestBelief(ushort id)
        {
            Id = new UId(id);
        }
        public TestBelief(IId id)
        {
            Id = id;
        }
        /// <summary>
        ///     Unique identifier af the knowledge
        /// </summary>
        public IId Id { get; }
        public bool Equals(IBelief belief)
        {
            return belief is TestBelief bel &&
                   Id.Equals(bel.Id);
        }
    }
}