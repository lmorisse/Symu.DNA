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
    ///     Beliefs are any form of religion or other persuasion.
    ///     Default implementation of IBelief
    /// </summary>
    public class BeliefEntity : Entity<BeliefEntity>, IBelief
    {
        public const byte Class = ClassIdCollection.Belief;
        public static IClassId ClassId => new ClassId(Class);
        public BeliefEntity(){}
        public BeliefEntity(MetaNetwork metaNetwork) : base(metaNetwork, metaNetwork?.Belief, Class)
        {
        }
        public BeliefEntity(MetaNetwork metaNetwork, string name) : base(metaNetwork, metaNetwork?.Belief, Class, name)
        {
        }
        /// <summary>
        /// Copy the metaNetwork, the two modes networks where the entity exists
        /// </summary>
        /// <param name="entityId"></param>
        public override void CopyMetaNetworkTo(IAgentId entityId)
        {
            MetaNetwork.ActorBelief.CopyToFromTarget(EntityId, entityId);
        }
        public override void Remove()
        {
            base.Remove();
            MetaNetwork.ActorBelief.RemoveTarget(EntityId);
        }
    }
}