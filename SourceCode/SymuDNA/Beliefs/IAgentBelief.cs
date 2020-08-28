﻿#region Licence

// Description: SymuBiz - Symu
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

using Symu.Common.Interfaces;
using Symu.Common.Interfaces.Entity;

namespace Symu.DNA.Beliefs
{
    /// <summary>
    /// Defines how who beliefs what
    /// </summary>
    public interface IAgentBelief: IComparable<IAgentBelief>
    {
        IId BeliefId { get; }
    }
}