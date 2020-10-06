#region Licence

// Description: SymuBiz - SymuDNA
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using Symu.Common.Interfaces;
using Symu.DNA.Entities;

#endregion

namespace Symu.DNA.Edges
{
    /// <summary>
    ///     Resource * Resource
    ///     Default implementation of IResourceResource
    ///     Source : Resource
    ///     Target : Resource
    /// </summary>
    public class ResourceResource : IResourceResource
    {
        public ResourceResource(IAgentId source, IAgentId target, IResourceUsage usage, float weight = 100)
        {
            Source = source;
            Target = target;
            Weight = weight;
            Usage = usage;
        }

        #region IResourceResource Members

        /// <summary>
        ///     Define how the AgentId is using the resource
        /// </summary>
        public IResourceUsage Usage { get; }


        public bool Equals(IResourceUsage resourceUsage)
        {
            return Usage.Equals(resourceUsage);
        }

        #endregion

        public override bool Equals(object obj)
        {
            return obj is ResourceResource resourceResource &&
                   Target.Equals(resourceResource.Target) &&
                   Source.Equals(resourceResource.Source) &&
                   Usage.Equals(resourceResource.Usage);
        }

        #region Interface IEdge

        /// <summary>
        ///     Number of interactions between the two agents
        /// </summary>
        public float Weight { get; set; }
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
            return new ResourceResource(Source, Target,Usage, Weight);
        }
        #endregion
    }
}