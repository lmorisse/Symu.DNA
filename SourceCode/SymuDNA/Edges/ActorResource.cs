#region Licence

// Description: SymuBiz - SymuDNA
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using Symu.Common.Interfaces;
using Symu.DNA.Entities;

#endregion

namespace Symu.DNA.Edges
{
    /// <summary>
    ///     Class for tests
    /// </summary>
    public class ActorResource : IActorResource
    {
        private float _weight;

        public ActorResource(IAgentId actorId, IAgentId resourceId, IResourceUsage resourceUsage,
            float weight=100)
        {
            Source = actorId;
            Target = resourceId;
            Usage = resourceUsage;
            Weight = weight;
        }

        #region IActorResource Members

        /// <summary>
        ///     Define how the AgentId is using the resource
        /// </summary>
        public IResourceUsage Usage { get; }

        public object Clone()
        {
            return new ActorResource(Source, Target, (ResourceUsage) Usage, Weight);
        }

        public bool Equals(IResourceUsage resourceUsage)
        {
            return Usage.Equals(resourceUsage);
        }

        #endregion

        #region Interface IEdge

        /// <summary>
        ///     Number of interactions between the two agents
        ///     Weight of capacity per resource
        ///     Ranging from [0; 100]
        /// </summary>
        public float Weight
        {
            get => _weight;
            set
            {
                if (value < 0 || value > 100)
                {
                    throw new ArgumentOutOfRangeException("Weight should be between [0;100]");
                }

                _weight = value;
            }
        }
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

        #endregion

        public override bool Equals(object obj)
        {
            return obj is ActorResource agentResource &&
                   Target.Equals(agentResource.Target) &&
                   Source.Equals(agentResource.Source) &&
                   Usage.Equals(agentResource.Usage);
        }

    }
}