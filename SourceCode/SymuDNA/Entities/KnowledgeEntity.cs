#region Licence

// Description: SymuBiz - Symu
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

using Symu.Common.Interfaces.Agent;
using Symu.Common.Interfaces.Entity;
using Symu.DNA.GraphNetworks;

namespace Symu.DNA.Entities
{    /// <summary>
    ///     Default implementation of IKnowledge
    /// </summary>
    public class KnowledgeEntity : IKnowledge
    {
        public static KnowledgeEntity CreateInstance()
        {
            return new KnowledgeEntity();
        }

        private MetaNetwork _metaNetwork;
        /// <summary>
        /// Use for clone method
        /// </summary>
        private KnowledgeEntity()
        {
        }
        public KnowledgeEntity(MetaNetwork metaNetwork, byte classId)
        {
            Set(metaNetwork);
            EntityId = new AgentId(_metaNetwork.Knowledge.NextId(), classId);
            _metaNetwork.Knowledge.Add(this);
        }

        public void Remove()
        {
            _metaNetwork.ActorKnowledge.RemoveKnowledge(EntityId);
            _metaNetwork.TaskKnowledge.RemoveKnowledge(EntityId);
            _metaNetwork.Knowledge.Remove(this);
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
            return obj is KnowledgeEntity knowledge &&
                   EntityId.Equals(knowledge.EntityId);
        }
    }
}