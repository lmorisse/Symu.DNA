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
    ///     Resource x Resource network, called substitution network
    ///     What resources use what resource
    ///     What resources need what resource
    ///     What resources can be substituted for which resource
    /// </summary>
    public interface IResourceResource : IEdge
    {
        /// <summary>
        ///     Define how the AgentId is using the resource
        /// </summary>
        IResourceUsage Usage { get; }

        bool Equals(IResourceUsage resourceUsage);
        //bool Equals(IAgentId resourceId, IResourceUsage resourceUsage);
    }
}