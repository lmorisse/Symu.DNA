#region Licence

// Description: SymuBiz - Symu
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

using Symu.Common.Interfaces.Agent;
using Symu.DNA.Entities;

namespace Symu.DNA.GraphNetworks.TwoModesNetworks
{        
    /// <summary>
    ///     Who (Actor) does what (Tasks)
    /// </summary>
    public interface IActorTask
    {
        IAgentId ActorId { get; }
        ITask Task { get; set; }
        /// <summary>
        /// The value used to feed the matrix network
        /// For a binary matrix network, the value is 1
        /// </summary>
        float Value { get; }
    }
}