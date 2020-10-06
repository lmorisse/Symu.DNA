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
using Symu.DNA.Entities;

#endregion

namespace Symu.DNA.GraphNetworks
{
    /// <summary>
    ///     Base class fro One mode vector
    /// </summary>
    public class OneModeNetwork
    {
        /// <summary>
        ///     Latest unique index of task
        /// </summary>
        private ushort _entityIndex;

        /// <summary>
        ///     Repository of all the Tasks used in the network
        /// </summary>
        public List<IEntity> List { get; } = new List<IEntity>();

        public int Count => List.Count;

        public IAgentId NextEntityId(byte classId)
        {
            return new AgentId(_entityIndex++, classId);
        }
        public IAgentId NextEntityId(IClassId classId)
        {
            return new AgentId(_entityIndex++, classId);
        }

        public bool Any()
        {
            return List.Any();
        }

        public bool Contains(IEntity key)
        {
            return List.Contains(key);
        }
        public bool Exists(IAgentId id)
        {
            return List.Exists(x => x.EntityId.Equals(id));
        }

        public void CopyTo(MetaNetwork metaNetwork, OneModeNetwork network)
        {
            network._entityIndex = _entityIndex;
            foreach (var clone in List.Select(key => key.Clone() as IEntity))
            {
                if (clone == null)
                {
                    throw new NullReferenceException(nameof(clone));
                }

                clone.Set(metaNetwork, network);
                network.List.Add(clone);
            }
        }

        public IReadOnlyList<IAgentId> ToVector()
        {
            return GetEntityIds().OrderBy(x => x.Id).ToList();
        }

        #region Add entity

        /// <summary>
        ///     Add a key to the repository
        /// </summary>
        /// <param name="key"></param>
        public void Add(IEntity key)
        {
            if (Contains(key))
            {
                return;
            }

            List.Add(key);
        }

        /// <summary>
        ///     Add a set of keys to the repository
        /// </summary>
        public void Add(IEnumerable<IEntity> keys)
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
        ///     Get an entity by its entityId
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns>The entity</returns>
        public IEntity GetEntity(IAgentId entityId)
        {
            return List.Find(x => x.EntityId.Equals(entityId));
        }

        /// <summary>
        ///     Get an entity by its entityId
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns>The entity</returns>
        public TTKey GetEntity<TTKey>(IAgentId entityId) where TTKey : IEntity
        {
            return List.OfType<TTKey>().ToList().Find(x => x.EntityId.Equals(entityId));
        }

        /// <summary>
        ///     Get all typed entities Id filtered by type
        /// </summary>
        /// <typeparam name="TTKey"></typeparam>
        /// <returns></returns>
        public IEnumerable<IAgentId> GetEntityIds<TTKey>() where TTKey : IEntity
        {
            return List.OfType<TTKey>().Select(x => x.EntityId);
        }

        /// <summary>
        ///     Get all entities Id
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IAgentId> GetEntityIds()
        {
            return List.Select(x => x.EntityId);
        }

        /// <summary>
        ///     Get all entities
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IEntity> GetEntities()
        {
            return List;
        }
        /// <summary>
        ///     Get all entities
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TTKey> GetEntities<TTKey>() where TTKey : IEntity
        {
            return List.OfType<TTKey>();
        }
        #endregion

        #region Remove

        public void Clear()
        {
            List.Clear();
            _entityIndex = 0;
        }

        public void Remove(IEntity key)
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
            return (ushort) List.Count(x => x.EntityId.ClassId.Equals(classId));
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
    }
}