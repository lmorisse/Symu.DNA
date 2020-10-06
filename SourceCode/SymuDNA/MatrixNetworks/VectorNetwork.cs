#region Licence

// Description: SymuBiz - SymuDNA
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Collections.Generic;
using Symu.Common.Interfaces;

#endregion

namespace Symu.DNA.MatrixNetworks
{
    /// <summary>
    ///     The generic class that is used for OneNodeNetwork to identify the index of a item
    ///     It is a bi directional Vector index = itemId
    /// </summary>
    public readonly struct VectorNetwork
    {
        public Dictionary<IAgentId, int> ItemIndex { get; }
        public IAgentId[] IndexItem { get; }

        public int Count => IndexItem.Length;
        public bool Any => IndexItem.Length > 0;

        public VectorNetwork(IReadOnlyList<IAgentId> ids)
        {
            if (ids == null)
            {
                throw new ArgumentNullException(nameof(ids));
            }

            ItemIndex = new Dictionary<IAgentId, int>();
            IndexItem = new IAgentId[ids.Count];

            for (var i = 0; i < ids.Count; i++)
            {
                ItemIndex.Add(ids[i], i);
                IndexItem[i] = ids[i];
            }
        }
    }
}