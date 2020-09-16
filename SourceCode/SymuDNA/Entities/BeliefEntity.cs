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
{
    /// <summary>
    ///     Default implementation of IBelief
    /// </summary>
    public class BeliefEntity: IBelief
    {
        public static BeliefEntity CreateInstance()
        {
            return new BeliefEntity();
        }

        private MetaNetwork _metaNetwork;
        /// <summary>
        /// Use for clone method
        /// </summary>
        private BeliefEntity()
        {
        }
        public BeliefEntity(MetaNetwork metaNetwork, byte classId)
        {
            Set(metaNetwork);
            EntityId = new AgentId(_metaNetwork.Belief.NextId(), classId);
            _metaNetwork.Belief.Add(this);
        }

        public void Remove()
        {
            _metaNetwork.ActorBelief.RemoveBelief(EntityId);
            _metaNetwork.Belief.Remove(this);
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
            return obj is BeliefEntity belief &&
                   EntityId.Equals(belief.EntityId);
        }
    }
}