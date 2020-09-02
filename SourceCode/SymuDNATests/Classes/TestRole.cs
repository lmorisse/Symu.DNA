#region Licence

// Description: SymuBiz - SymuTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

using Symu.Common.Interfaces.Entity;
using Symu.DNA.Networks.OneModeNetworks;

namespace SymuDNATests.Classes
{
    internal sealed class TestRole : IRole
    {
        public TestRole(byte role)
        {
            Role = role;
        }
        public IId Id => new UId(Role);
        public readonly byte Role;
        public bool Equals(IRole role)
        {
            return role is TestRole rol
                   && Role == rol.Role;
        }
    }
}