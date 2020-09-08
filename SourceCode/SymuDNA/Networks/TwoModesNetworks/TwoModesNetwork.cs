#region Licence

// Description: SymuBiz - SymuDNA
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

using System;
using System.Collections.Generic;
using System.Linq;

namespace Symu.DNA.Networks.TwoModesNetworks
{
    /// <summary>
    ///     Abstract class for two modes network
    /// </summary>
    public abstract class TwoModesNetwork<TKey, TValue> where TKey : class where TValue : class
    {

        /// <summary>
        ///     List of all resource and their tasks
        /// </summary>
        public readonly Dictionary<TKey, List<TValue>> List =
            new Dictionary<TKey, List<TValue>>();
        public int Count => List.Count;


        public bool Any()
        {
            return List.Any();
        }

        public void Clear()
        {
            List.Clear();
        }
        /// <summary>
        /// Check that the resourceId exists in the repository
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Exists(TKey key)
        {
            return List.ContainsKey(key);
        }

        public bool Exists(TKey key, TValue value)
        {
            return Exists(key) && List[key].Contains(value);
        }

        public void Add(TKey key, TValue value)
        {
            AddKey(key);
            AddValue(key, value);
        }

        /// <summary>
        ///     Add a value to a key
        ///     Key is supposed to be already present in the collection.
        ///     if not use Add method
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddValue(TKey key, TValue value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            if (!Exists(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (!List[key].Contains(value))
            {
                List[key].Add(value);
            }
        }

        public void AddKey(TKey key)
        {
            if (!Exists(key))
            {
                List.Add(key, new List<TValue>());
            }
        }

        public IEnumerable<TValue> GetValues(TKey key)
        {
            return Exists(key) ? (IEnumerable<TValue>) List[key] : new TValue[0];
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
        /// <summary>
        ///     Make a copy of of the network
        /// </summary>
        /// <param name="network"></param>
        public void CopyTo(TwoModesNetwork<TKey, TValue> network)
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