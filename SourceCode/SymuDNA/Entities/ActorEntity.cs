#region Licence

// Description: SymuBiz - Symu
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

using Symu.Common.Interfaces.Agent;
using Symu.DNA.GraphNetworks;

namespace Symu.DNA.Entities
{
    /// <summary>
    ///     Default implementation of IActor
    /// </summary>
    public class ActorEntity : IActor
    {
        public static ActorEntity CreateInstance()
        {
            return new ActorEntity();
        }

        private MetaNetwork _metaNetwork;
        /// <summary>
        /// Use for clone method
        /// </summary>
        private ActorEntity()
        {
        }
        public ActorEntity(MetaNetwork metaNetwork, byte classId)
        {
            Set(metaNetwork);
            EntityId = new AgentId(_metaNetwork.Actor.NextId(), classId);
            _metaNetwork.Actor.Add(this);
        }

        public void Remove()
        {
            _metaNetwork.ActorResource.RemoveActor(EntityId);
            _metaNetwork.OrganizationActor.RemoveActor(EntityId);
            _metaNetwork.ActorTask.RemoveActor(EntityId);
            _metaNetwork.ActorActor.RemoveActor(EntityId);
            _metaNetwork.ActorBelief.RemoveActor(EntityId);
            _metaNetwork.ActorKnowledge.RemoveActor(EntityId);
            _metaNetwork.ActorRole.RemoveActor(EntityId);
            _metaNetwork.Actor.Remove(this);
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
            return obj is ActorEntity actor &&
                   EntityId.Equals(actor.EntityId);
        }
    }
}