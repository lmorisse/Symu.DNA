﻿#region Licence

// Description: SymuBiz - Symu
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

using Symu.Common.Interfaces.Agent;
using Symu.DNA.Networks.OneModeNetworks;

namespace Symu.DNA.Networks.TwoModesNetworks.Assignment
{
    public interface IAgentActivity
    {
        IAgentId Id { get; }
        IActivity Activity { get; set; }
        /// <summary>
        /// The value used to feed the matrix network
        /// For a binary matrix network, the value is 1
        /// </summary>
        float Value { get; }
    }
}