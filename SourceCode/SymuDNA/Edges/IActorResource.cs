#region Licence

// Description: SymuBiz - SymuDNA
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using Symu.DNA.Entities;

#endregion

namespace Symu.DNA.Edges
{
    /// <summary>
    ///     Actor x Resource network, called capabilities network
    ///     Who has what resource
    ///     By default, an actor uses a resourceId, with an allocation from 0 to 100 with a certain ResourceUsage
    /// </summary>
    public interface IActorResource : IEdge
    {
        /// <summary>
        ///     Define how the AgentId is using the resource
        /// </summary>
        IResourceUsage Usage { get; }

        bool Equals(IResourceUsage resourceUsage);
    }
}