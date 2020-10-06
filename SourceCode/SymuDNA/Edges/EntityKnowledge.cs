#region Licence

// Description: SymuBiz - SymuDNA
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using Symu.Common.Interfaces;

#endregion

namespace Symu.DNA.Edges
{
    /// <summary>
    ///     Describe an area of knowledge
    /// </summary>
    public class EntityKnowledge : IEntityKnowledge
    {
        public EntityKnowledge(){}

        public EntityKnowledge(IAgentId actorId, IAgentId knowledgeId, float weight = 1)
        {
            Source = actorId;
            Target = knowledgeId;
            Weight = weight;
            NormalizedWeight = weight;
        }

        #region IActorKnowledge Members

        public float CompareTo(IEntityKnowledge other)
        {
            if (other is EntityKnowledge test)
            {
                return NormalizedWeight * test.NormalizedWeight;
            }

            return 0;
        }

        #endregion

        public override bool Equals(object obj)
        {
            return obj is EntityKnowledge actorKnowledge &&
                   Target.Equals(actorKnowledge.Target) &&
                   Source.Equals(actorKnowledge.Source);
        }

        #region Interface IEdge

        /// <summary>
        ///     Number of interactions between the two agents
        ///     Default 1
        /// </summary>
        public virtual float Weight { get; set; }

        /// <summary>
        ///     Normalized weight computed by the network via the NormalizeWeights method
        /// </summary>
        public float NormalizedWeight { get; set; }

        public bool EqualsSource(IAgentId source)
        {
            return source.Equals(Source);
        }

        public bool EqualsTarget(IAgentId target)
        {
            return target.Equals(Target);
        }

        /// <summary>
        ///     Unique key of the agent with the smallest key
        /// </summary>
        public IAgentId Source { get; set; }

        /// <summary>
        ///     Unique key of the agent with the highest key
        /// </summary>
        public IAgentId Target { get; set; }
        public object Clone()
        {
            return new EntityKnowledge(Source, Target, Weight);
        }

        #endregion
    }
}