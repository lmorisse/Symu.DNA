#region Licence

// Description: SymuBiz - SymuDNA
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Collections.Generic;
using Symu.Common.Classes;
using Symu.Common.Interfaces;
using Symu.DNA.Edges;

#endregion

namespace Symu.DNA.GraphNetworks.TwoModesNetworks
{
    /// <summary>
    ///     Task * Knowledge network
    ///     What knowledge is necessary for what Task
    ///     Source : Task
    ///     Target : Knowledge
    /// </summary>
    public class TaskKnowledgeNetwork : TwoModesNetwork<IEntityKnowledge>
    {
    }
}