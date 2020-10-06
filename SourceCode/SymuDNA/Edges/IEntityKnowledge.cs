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
    ///     Entity * Knowledge
    ///     Which entity knows what (Knowledge)
    ///     Source : Entity : Actor, Task, resource, ....
    ///     Target : Knowledge
    /// </summary>
    public interface IEntityKnowledge : IEdge, IComparable<IEntityKnowledge>
    {
    }
}