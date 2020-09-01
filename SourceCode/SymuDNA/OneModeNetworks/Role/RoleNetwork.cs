#region Licence

// Description: SymuBiz - Symu
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

#endregion

namespace Symu.DNA.OneModeNetworks.Role
{
    public class RoleNetwork
    {
        /// <summary>
        ///     Repository of all the resources used during the simulation
        /// </summary>
        public RoleCollection Repository { get; } = new RoleCollection();

        public void Clear()
        {
            Repository.Clear();
        }

        public bool Any()
        {
            return Repository.Any();
        }

        public void Add(IRole role)
        {
            if (Exists(role))
            {
                return;
            }

            Repository.Add(role);
        }

        public bool Exists(IRole role)
        {
            return Repository.Contains(role);
        }
    }
}