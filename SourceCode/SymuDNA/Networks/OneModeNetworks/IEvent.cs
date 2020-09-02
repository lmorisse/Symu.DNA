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
    /// Interface of event :
    /// occurrences or phenomena that happen
    /// </summary>
    public interface IEvent
    {
        /// <summary>
        ///     Unique identifier of the event
        /// </summary>
        IId Id { get; }
    }
}