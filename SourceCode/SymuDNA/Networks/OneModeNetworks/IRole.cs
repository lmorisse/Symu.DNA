#region Licence

// Description: SymuBiz - Symu
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

using Symu.Common.Interfaces.Entity;

namespace Symu.DNA.Networks.OneModeNetworks
{
    /// <summary>
    ///     A role describe functions of agents
    /// </summary>
    public interface IRole
    {
        /// <summary>
        ///     Unique identifier of the Role
        /// </summary>
        IId Id { get; }
        bool Equals(IRole role);
    }
}