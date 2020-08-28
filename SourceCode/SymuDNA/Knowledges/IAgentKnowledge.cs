#region Licence

// Description: SymuBiz - Symu
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

using Symu.Common.Interfaces.Entity;
using Symu.Common.Interfaces;

namespace Symu.DNA.Knowledges
{
    /// <summary>
    /// Defines how who knows what
    /// </summary>
    public interface IAgentKnowledge : IComparable<IAgentKnowledge>
    {
        IId KnowledgeId { get; }
    }
}