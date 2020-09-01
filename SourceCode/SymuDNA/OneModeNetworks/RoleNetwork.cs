#region Licence

// Description: SymuBiz - Symu
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

#endregion

using System.Collections.Generic;
using System.Linq;

namespace Symu.DNA.OneModeNetworks
{
    public class RoleNetwork
    {
        /// <summary>
        ///     Repository of all the resources used in the network
        /// </summary>
        public List<IRole> List { get; } = new List<IRole>();

        public void Clear()
        {
            List.Clear();
        }

        public bool Any()
        {
            return List.Any();
        }

        public void Add(IRole role)
        {
            if (Exists(role))
            {
                return;
            }

            List.Add(role);
        }

        public bool Exists(IRole role)
        {
            return List.Contains(role);
        }
    }
}