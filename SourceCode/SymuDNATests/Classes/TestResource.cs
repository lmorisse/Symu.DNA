#region Licence

// Description: SymuBiz - SymuTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using Symu.Common.Interfaces.Entity;
using Symu.DNA.Resources;

#endregion

namespace SymuDNATests.Classes
{
    /// <summary>
    ///     Class for tests
    /// </summary>
    internal sealed class TestResource : IResource
    {

        private readonly UId _id;
        public TestResource(UId id)
        {
            _id = (UId)id;
        }
        public TestResource(ushort id)
        {
            _id = new UId(id);
        }
        /// <summary>
        ///     The unique agentId of the resource
        /// </summary>
        public IId Id => _id;
    }
}