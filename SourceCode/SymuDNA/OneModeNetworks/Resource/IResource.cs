#region Licence

// Description: SymuBiz - Symu
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using Symu.Common.Interfaces.Entity;

#endregion

namespace Symu.DNA.OneModeNetworks.Resource
{
    /// <summary>
    /// The interface that let you define a resource
    /// </summary>
    public interface IResource 
    {
        /// <summary>
        /// Unique identifier of the role
        /// </summary>
        IId Id { get; }
    }
}