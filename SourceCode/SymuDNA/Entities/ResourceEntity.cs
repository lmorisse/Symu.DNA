#region Licence

// Description: SymuBiz - SymuTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using Symu.Common.Interfaces.Agent;
using Symu.DNA.GraphNetworks;

#endregion

namespace Symu.DNA.Entities
{
    /// <summary>
    ///     Default implementation of IResource
    /// </summary>
    public class ResourceEntity : IResource
    {
        public static ResourceEntity CreateInstance()
        {
            return new ResourceEntity();
        }

        private MetaNetwork _metaNetwork;
        /// <summary>
        /// Use for clone method
        /// </summary>
        private ResourceEntity()
        {
        }
        public ResourceEntity(MetaNetwork metaNetwork, byte classId)
        {
            Set(metaNetwork);
            EntityId = new AgentId(_metaNetwork.Resource.NextId(), classId);
            _metaNetwork.Resource.Add(this);
        }

        public void Remove()
        {
            _metaNetwork.ResourceResource.RemoveResource(EntityId);
            _metaNetwork.ResourceTask.RemoveResource(EntityId);
            _metaNetwork.OrganizationResource.RemoveResource(EntityId);
            _metaNetwork.ActorResource.RemoveResource(EntityId);
            _metaNetwork.Resource.Remove(this);
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
            return obj is ResourceEntity resource &&
                   EntityId.Equals(resource.EntityId);
        }
    }
}