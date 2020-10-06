#region Licence

// Description: SymuBiz - Symu
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using Symu.Common.Interfaces;

using Symu.DNA.GraphNetworks;

#endregion

namespace Symu.DNA.Entities
{
    /// <summary>
    ///     abstract class for IEntity 
    /// </summary>
    public class Entity<TEntity> : IEntity where TEntity : IEntity, new()
    {
        protected OneModeNetwork Network;
        protected MetaNetwork MetaNetwork;

        /// <summary>
        ///     Use for clone method
        /// </summary>
        protected Entity()
        {
        }

        public Entity(MetaNetwork metaNetwork, OneModeNetwork network, byte classId)
        {
            if (metaNetwork == null)
            {
                throw new ArgumentNullException(nameof(metaNetwork));
            }

            if (network == null)
            {
                throw new ArgumentNullException(nameof(network));
            }

            Set(metaNetwork, network);
            EntityId = network.NextEntityId(classId);
            Network.Add(this);
        }
        public Entity(MetaNetwork metaNetwork, OneModeNetwork network, byte classId, string name) :this(metaNetwork, network, classId)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            Name = name;
        }

        #region IEntity Members

        public virtual void Remove()
        {
            Network.Remove(this);
        }

        public IAgentId EntityId { get; set; }

        public string Name { get; set; }
        public IAgentId Parent { get; set; }

        /// <summary>Creates a new object that is a copy of the current instance, with the same EntityId.</summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public virtual object Clone()
        {
            var clone = new TEntity();
            CopyEntityTo(clone);
            return clone;
        }
        /// <summary>Creates a new object that is a copy of the current instance, including the metaNetwork items, with a new EntityId.</summary>
        /// <returns>A new object that is a duplicate of this instance.</returns>
        public TTEntity Duplicate<TTEntity>() where TTEntity : IEntity, new()     
        {
            var clone = new TTEntity();
            Network.Add(clone);
            CopyEntityTo(clone);
            clone.EntityId = Network.NextEntityId(EntityId.ClassId);
            CopyMetaNetworkTo(clone.EntityId);
            return clone;
        }
        /// <summary>
        /// Copy the entity's properties
        /// </summary>
        /// <param name="entity"></param>
        public virtual void CopyEntityTo(IEntity entity)
        {
            entity.Set(MetaNetwork, Network);
            entity.EntityId = EntityId;
            entity.Name = Name;
        }
        /// <summary>
        /// Copy the metaNetwork, the two modes networks where the entity exists
        /// </summary>
        /// <param name="entityId"></param>
        public virtual void CopyMetaNetworkTo(IAgentId entityId)
        {
        }

        public virtual void Set(MetaNetwork metaNetwork, OneModeNetwork network)
        {
            MetaNetwork = metaNetwork;
            Network = network;
        }

        #endregion


        public override bool Equals(object obj)
        {
            return obj is TEntity entity &&
                   EntityId.Equals(entity.EntityId);
        }

        protected bool Equals(TEntity other)
        {
            return other != null && EntityId.Equals(other.EntityId);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}