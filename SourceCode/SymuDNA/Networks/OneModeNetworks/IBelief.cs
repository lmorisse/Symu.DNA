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
    ///     Describe a belief, based on knowledge/fact
    /// </summary>
    public interface IBelief
    {
        /// <summary>
        ///     Unique identifier of the belief
        /// </summary>
        IId Id { get; }        
        bool Equals(IBelief belief);
    }
}