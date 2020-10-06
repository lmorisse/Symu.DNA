#region Licence

// Description: SymuBiz - SymuDNA
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Collections.Generic;
using System.Linq;
using Symu.Common.Interfaces;
using Symu.DNA.Edges;

#endregion

namespace Symu.DNA.GraphNetworks.TwoModesNetworks
{
    /// <summary>
    ///     Resource * Knowledge network
    ///     What resources store what knowledge
    ///     Source : Resource
    ///     Target : Knowledge
    /// </summary>
    public class ResourceKnowledgeNetwork : TwoModesNetwork<IEntityKnowledge>
    {
    }
}