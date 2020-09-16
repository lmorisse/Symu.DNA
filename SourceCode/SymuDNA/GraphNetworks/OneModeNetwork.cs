#region Licence

// Description: SymuBiz - SymuDNA
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using Symu.Common.Interfaces.Agent;
using Symu.DNA.Entities;

namespace Symu.DNA.GraphNetworks
{
    /// <summary>
    ///     Abstract class fro One mode vector 
    /// </summary>
    public abstract class OneModeNetwork<TKey> where TKey : class, IEntity
    {
        /// <summary>
        ///     Repository of all the Tasks used in the network
        /// </summary>
        public List<TKey> List { get; } = new List<TKey>();
        public int Count => List.Count;
        /// <summary>
        ///     Latest unique index of task
        /// </summary>
        private ushort _entityIndex;
        public ushort NextId()
        {
            return _entityIndex++;
        }

        public bool Any()
        {
            return List.Any();
        }

        public bool Exists(TKey key)
        {
            return List.Contains(key);
        }

        #region Add entity

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
        #endregion

        #region Get entities
        /// <summary>
        ///     Get a typed entity by its entityId
        /// </summary>
        /// <typeparam name="TTKey"></typeparam>
        /// <param name="entityId"></param>
        /// <returns>The typed entity</returns>
        public TTKey GetEntity<TTKey>(IAgentId entityId) where TTKey : IEntity
        {
            return (TTKey)GetEntity(entityId);
        }


        /// <summary>
        ///     Get an entity by its entityId
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns>The entity</returns>
        public IEntity GetEntity(IAgentId entityId)
        {
            return List.Find(x => x.EntityId.Equals(entityId));
        }

        /// <summary>
        /// Get all typed entities Id filtered by type
        /// </summary>
        /// <typeparam name="TTKey"></typeparam>
        /// <returns></returns>
        public IEnumerable<IAgentId> GetEntityIds<TTKey>() where TTKey : TKey
        {
            return List.OfType<TTKey>().Select(x => x.EntityId);
        }

        /// <summary>
        /// Get all entities Id 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IAgentId> GetEntityIds() 
        {
            return List.Select(x => x.EntityId);
        }

        /// <summary>
        /// Get all typed entities filtered by type
        /// </summary>
        /// <typeparam name="TTKey"></typeparam>
        /// <returns></returns>
        public IEnumerable<TTKey> GetEntities<TTKey>() where TTKey : TKey
        {
            return List.OfType<TTKey>();
        }

        /// <summary>
        /// Get all entities 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IEntity> GetEntities()
        {
            return List;
        }
        #endregion

        #region Remove
        public void Clear()
        {
            List.Clear();
        }
        public void Remove(TKey key)
        {
            List.Remove(key);
        }
        public void Remove(IAgentId entityId)
        {
            List.RemoveAll(x => x.EntityId.Equals(entityId));
        }
        #endregion

        #region Filtered by classId


        /// <summary>
        ///     The number of entities 
        /// </summary>
        public ushort CountByClassId(IClassId classId)
        {
            return (ushort)List.Count(x => x.EntityId.ClassId.Equals(classId));
        }

        /// <summary>
        ///     Returns a list of all the entities Id filtered by their ClassId.
        /// </summary>
        public IEnumerable<IAgentId> FilteredIdsByClassId(IClassId classId)
        {
            return List.Where(x => x.EntityId.ClassId.Equals(classId)).Select(x => x.EntityId);
        }

        /// <summary>
        ///     Returns a list of all the entities filtered by their ClassId.
        /// </summary>
        public IEnumerable<IEntity> FilteredByClassId(IClassId classId)
        {
            return List.Where(x => x.EntityId.ClassId.Equals(classId));
        }
        #endregion


        public void CopyTo(MetaNetwork metaNetwork, OneModeNetwork<TKey> entity)
        {
            entity._entityIndex = _entityIndex;
            foreach (var clone in List.Select(key => key.Clone() as TKey))
            {
                if (clone == null)
                {
                    throw new NullReferenceException(nameof(clone));
                }

                clone.Set(metaNetwork);
                entity.List.Add(clone);
            }
        }

        public IReadOnlyList<IAgentId> ToVector()
        {
            return GetEntityIds().OrderBy(x => x.Id).ToList();
        }
    }
}