#region Licence

// Description: SymuBiz - SymuDNA
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Symu.DNA.GraphNetworks.TwoModesNetworks
{
    /// <summary>
    ///     Abstract for safe two modes network
    /// </summary>
    public abstract class ConcurrentTwoModesNetwork<TKey, TValue> where TKey : class where TValue : class
    {
        /// <summary>
        ///     Key => GroupId
        ///     Value => list of AgentActivity : AgentId, activity
        /// </summary>
        public ConcurrentDictionary<TKey, List<TValue>> List { get; } =
            new ConcurrentDictionary<TKey, List<TValue>>();
        public int Count => List.Count;

        public bool Any()
        {
            return List.Any();
        }

        public void Clear()
        {
            List.Clear();
        }

        public bool Exists(TKey key)
        {
            return List.ContainsKey(key);
        }

        public virtual bool Exists(TKey key, TValue value)
        {
            return true;
        }

        public void Add(TKey key, TValue value)
        {
            AddKey(key);
            AddValue(key, value);
        }

        public void Add(TKey key, IEnumerable<TValue> values)
        {
            AddKey(key);
            foreach (var value in values)
            {
                AddValue(key, value);
            }
        }

        /// <summary>
        ///     Add a value to a key
        ///     Key is supposed to be already present in the collection.
        ///     if not use Add method
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public virtual void AddValue(TKey key, TValue value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            if (!Exists(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (!Exists(key, value))
            {
                List[key].Add(value);
            }
        }

        public void AddKey(TKey key)
        {
            if (!Exists(key))
            {
                List.TryAdd(key, new List<TValue>());
            }
        }

        public IEnumerable<TKey> GetKeys()
        {
            return List.Any() ? List.Keys : new List<TKey>();
        }
        public IEnumerable<TValue> GetValues(TKey key)
        {
            return Exists(key) ? (IEnumerable<TValue>)List[key] : new TValue[0];
        }

        /// <summary>
        ///     Get values count of a key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public byte GetValuesCount(TKey key)
        {
            return Exists(key) ? Convert.ToByte(List[key].Count) : (byte)0;
        }


        public virtual void RemoveActor(TKey key)
        {
            if (Exists(key))
            {
                List.TryRemove(key, out _);
            }
        }
        /// <summary>
        ///     Make a copy of of the network
        /// </summary>
        /// <param name="network"></param>
        public void CopyTo(ConcurrentTwoModesNetwork<TKey, TValue> network)
        {
            if (network is null)
            {
                throw new ArgumentNullException(nameof(network));
            }

            foreach (var keyValuePair in List)
            foreach (var value in keyValuePair.Value)
            {
                network.Add(keyValuePair.Key, value);
            }
        }
    }
}