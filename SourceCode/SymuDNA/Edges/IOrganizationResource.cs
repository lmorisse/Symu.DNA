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
    ///     Organization x Resource network, called capabilities network
    ///     Which organization has what resource
    /// </summary>
    public interface IOrganizationResource : IEdge
    {
        /// <summary>
        ///     Define how the AgentId is using the resource
        /// </summary>
        IResourceUsage Usage { get; }

        bool Equals(IResourceUsage resourceUsage);
    }
}