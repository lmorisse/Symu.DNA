#region Licence

// Description: SymuBiz - SymuDNA
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

using System;
using System.Collections.Generic;
using System.Linq;

namespace Symu.DNA.Networks.OneModeNetworks
{
    /// <summary>
    ///     Abstract class fro One mode vector 
    /// </summary>
    public abstract class OneModeNetwork<TKey> where TKey : class
    {
        /// <summary>
        ///     Latest unique index of task
        /// </summary>
        private ushort _entityIndex;
        /// <summary>
        ///     Repository of all the Tasks used in the network
        /// </summary>
        public List<TKey> List { get; private set; } = new List<TKey>();
        public int Count => List.Count;

        public void Clear()
        {
            List.Clear();
        }

        public bool Any()
        {
            return List.Any();
        }

        /// <summary>
        /// Add a key to the repository
        /// </summary>
        /// <param name="key"></param>
        public void Add(TKey key)
        {
            if (Exists(key))
            {
                return;
            }

            List.Add(key);
        }

        /// <summary>
        ///     Add a set of keys to the repository
        /// </summary>
        public void Add(IEnumerable<TKey> keys)
        {
            if (keys is null)
            {
                throw new ArgumentNullException(nameof(keys));
            }

            foreach (var key in keys)
            {
                Add(key);
            }
        }

        public bool Exists(TKey key)
        {
            return List.Contains(key);
        }

        public ushort NextIdentity()
        {
            return _entityIndex++;
        }

        public void CopyTo(OneModeNetwork<TKey> entity)
        {
            entity.List = List.ToList();
            entity._entityIndex = _entityIndex;
        }
    }
}