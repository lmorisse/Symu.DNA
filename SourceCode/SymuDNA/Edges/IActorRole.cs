#region Licence

// Description: SymuBiz - SymuDNA
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using Symu.Common.Interfaces;

#endregion

namespace Symu.DNA.Edges
{
    /// <summary>
    ///     Who has what functions in what organization
    /// </summary>
    public interface IActorRole : IEdge
    {
        /// <summary>
        ///     Unique key of the organization
        /// </summary>
        IAgentId OrganizationId { get; set; }

        bool IsOrganization(IAgentId organizationId);
        bool IsOrganization(IClassId organizationClassId);
    }
}