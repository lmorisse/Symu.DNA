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
    ///     A knowledge is cognitive capabilities and skills
    /// </summary>
    public interface IKnowledge
    {
        /// <summary>
        ///     Unique identifier af the knowledge
        /// </summary>
        IId Id { get; }        
        bool Equals(IKnowledge knowledge);
    }
}