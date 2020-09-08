#region Licence

// Description: SymuBiz - Symu
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

using System;
using System.Collections.Generic;

namespace Symu.DNA.MatrixNetworks
{
    /// <summary>
    /// The generic class that is used for OneNodeNetwork to identify the index of a item
    /// It is a bi directional Vector index = itemId
    /// </summary>
    public readonly struct VectorNetwork<TId> where TId : class
    {
        public Dictionary<TId, int> ItemIndex { get; } 
        public TId[] IndexItem { get; }

        public int Count => IndexItem.Length;
        public bool Any => IndexItem.Length > 0;

        public VectorNetwork(IReadOnlyList<TId> ids)
        {
            if (ids == null)
            {
                throw new ArgumentNullException(nameof(ids));
            }

            ItemIndex = new Dictionary<TId, int>();
            IndexItem = new TId[ids.Count];

            for (var i = 0; i < ids.Count; i++)
            {
                ItemIndex.Add(ids[i], i);
                IndexItem[i] = ids[i];
            }
        }
    }
}