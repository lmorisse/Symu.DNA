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
    ///     A knowledge is cognitive capabilities and skills
    ///     Default implementation of IKnowledge
    /// </summary>
    public class KnowledgeEntity : Entity<KnowledgeEntity>, IKnowledge
    {
        public const byte Class = ClassIdCollection.Knowledge;
        public static IClassId ClassId => new ClassId(Class);
        public KnowledgeEntity(){}
        public KnowledgeEntity(MetaNetwork metaNetwork) : base(metaNetwork, metaNetwork?.Knowledge, Class)
        {
        }
        public KnowledgeEntity(MetaNetwork metaNetwork, string name) : base(metaNetwork, metaNetwork?.Knowledge, Class, name)
        {
        }
        /// <summary>
        /// Copy the metaNetwork, the two modes networks where the entity exists
        /// </summary>
        /// <param name="entityId"></param>
        public override void CopyMetaNetworkTo(IAgentId entityId)
        {
            MetaNetwork.ActorKnowledge.CopyToFromTarget(EntityId, entityId);
            MetaNetwork.TaskKnowledge.CopyToFromTarget(EntityId, entityId);
            MetaNetwork.ResourceKnowledge.CopyToFromTarget(EntityId, entityId);
        }
        public override void Remove()
        {
            base.Remove();
            MetaNetwork.ActorKnowledge.RemoveTarget(EntityId);
            MetaNetwork.TaskKnowledge.RemoveTarget(EntityId);
            MetaNetwork.ResourceKnowledge.RemoveTarget(EntityId);
        }
    }
}