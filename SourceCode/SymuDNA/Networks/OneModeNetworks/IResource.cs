#region Licence

// Description: SymuBiz - Symu
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using Symu.Common.Interfaces.Entity;

#endregion

namespace Symu.DNA.Networks.OneModeNetworks
{

    /// <summary>
    ///     Resources are products, materials, or goods that are necessary to perform Tasks and Events
    /// </summary>
    /// <example>database, products, routines, processes, ...</example>
    public interface IResource 
    {
        /// <summary>
        /// Unique identifier of the role
        /// </summary>
        IId Id { get; }
    }
}