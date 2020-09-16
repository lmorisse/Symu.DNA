#region Licence

// Description: SymuBiz - SymuTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

using Symu.Common.Interfaces.Agent;
using Symu.Common.Interfaces.Entity;
using Symu.DNA.GraphNetworks;

namespace Symu.DNA.Entities
{
    /// <summary>
    /// Default implementation of IRole
    /// </summary>
    public class RoleEntity : IRole
    {
        public static RoleEntity CreateInstance()
        {
            return new RoleEntity();
        }

        private MetaNetwork _metaNetwork;
        /// <summary>
        /// Use for clone method
        /// </summary>
        private RoleEntity()
        {
        }
        public RoleEntity(MetaNetwork metaNetwork, byte classId)
        {
            Set(metaNetwork);
            EntityId = new AgentId(_metaNetwork.Role.NextId(), classId);
            _metaNetwork.Role.Add(this);
        }

        public void Remove()
        {
            _metaNetwork.ActorRole.RemoveRole(EntityId);
            _metaNetwork.Role.Remove(this);
        }

        public IAgentId EntityId { get; set; }

        /// <summary>Creates a new object that is a copy of the current instance.</summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public virtual object Clone()
        {
            var clone = CreateInstance();
            clone.EntityId = EntityId;
            return clone;
        }
        public void Set(MetaNetwork metaNetwork)
        {
            _metaNetwork = metaNetwork;
        }
        public override bool Equals(object obj)
        {
            return obj is RoleEntity role &&
                   EntityId.Equals(role.EntityId);
        }
    }
}