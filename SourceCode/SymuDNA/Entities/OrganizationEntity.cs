#region Licence

// Description: SymuBiz - SymuTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using Symu.Common.Interfaces.Agent;
using Symu.Common.Interfaces.Entity;
using Symu.DNA.GraphNetworks;

#endregion

namespace Symu.DNA.Entities
{
    /// <summary>
    ///     Default implementation of IOrganization
    /// </summary>
    public class OrganizationEntity : IOrganization
    {
        public static OrganizationEntity CreateInstance()
        {
            return new OrganizationEntity();
        }

        private MetaNetwork _metaNetwork;
        /// <summary>
        /// Use for clone method
        /// </summary>
        private OrganizationEntity()
        {
        }
        public OrganizationEntity(MetaNetwork metaNetwork, byte classId)
        {
            Set(metaNetwork);
            EntityId = new AgentId(_metaNetwork.Organization.NextId(), classId);
            _metaNetwork.Organization.Add(this);
        }

        public void Remove()
        {
            _metaNetwork.OrganizationActor.RemoveOrganization(EntityId);
            _metaNetwork.OrganizationResource.RemoveOrganization(EntityId);
            _metaNetwork.Organization.Remove(this);
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
            return obj is OrganizationEntity organization &&
                   EntityId.Equals(organization.EntityId);
        }
    }
}