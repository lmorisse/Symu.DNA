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
    ///     Actor * Knowledge network, called knowledge network
    ///     Who (Actor) knows what (Knowledge)
    ///     Source : Actor
    ///     Target : Knowledge
    /// </summary>
    public class ActorKnowledgeNetwork : TwoModesNetwork<IEntityKnowledge>
    {

        public IEnumerable<IAgentId> FilterActorsWithKnowledge(IEnumerable<IAgentId> actorIds, IAgentId knowledgeId)
        {
            if (actorIds is null)
            {
                throw new ArgumentNullException(nameof(actorIds));
            }

            return actorIds.Where(actorId => Exists(actorId, knowledgeId));
        }
    }
}