#region Licence

// Description: SymuBiz - SymuDNA
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using Symu.Common.Interfaces;
using Symu.DNA.GraphNetworks;

#endregion

namespace Symu.DNA.Entities
{
    /// <summary>
    ///     A role describe functions of actors
    ///     Default implementation of IRole
    /// </summary>
    public class RoleEntity : Entity<RoleEntity>, IRole
    {
        public const byte Class = ClassIdCollection.Role;
        public static IClassId ClassId => new ClassId(Class);
        public RoleEntity(){ }
        public RoleEntity(MetaNetwork metaNetwork) : base(metaNetwork, metaNetwork?.Role, Class)
        {
        }
        public RoleEntity(MetaNetwork metaNetwork, string name) : base(metaNetwork, metaNetwork?.Role, Class, name)
        {
        }
        /// <summary>
        /// Copy the metaNetwork, the two modes networks where the entity exists
        /// </summary>
        /// <param name="entityId"></param>
        public override void CopyMetaNetworkTo(IAgentId entityId)
        {
            MetaNetwork.ActorRole.CopyToFromTarget(EntityId, entityId);
        }
        public override void Remove()
        {
            base.Remove();
            MetaNetwork.ActorRole.RemoveTarget(EntityId);
        }
    }
}