﻿#region Licence

// Description: SymuBiz - Symu
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

using Symu.Common.Interfaces;
using Symu.Common.Interfaces.Entity;

namespace Symu.DNA.Networks.TwoModesNetworks.AgentKnowledge
{
    /// <summary>
    /// Defines how who knows what
    /// </summary>
    public interface IAgentKnowledge : IComparable<IAgentKnowledge>
    {
        IId KnowledgeId { get; }
        /// <summary>
        /// The value used to feed the matrix network
        /// For a binary matrix network, the value is 1
        /// </summary>
        float Value { get; }
    }
}