#region Licence

// Description: SymuBiz - SymuDNA
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using Symu.Common.Interfaces;

#endregion

namespace Symu.DNA.Edges
{
    /// <summary>
    /// </summary>
    public interface IEdge : ICloneable
    {
        /// <summary>
        ///     Gets the source entity
        /// </summary>
        IAgentId Source { get; set; }

        /// <summary>
        ///     Gets the target entity
        /// </summary>
        IAgentId Target { get; set; }

        /// <summary>
        ///     Get the weight of the edge
        ///     The value used to feed the matrix network
        ///     For a binary matrix network, the value is 1
        /// </summary>
        float Weight { get; set; }
        /// <summary>
        ///     Get the normalized weight of the edge
        ///     Should be in the range [0;1]
        /// </summary>
        float NormalizedWeight { get; set; }

        bool EqualsSource(IAgentId source);
        bool EqualsTarget(IAgentId target);
    }
}