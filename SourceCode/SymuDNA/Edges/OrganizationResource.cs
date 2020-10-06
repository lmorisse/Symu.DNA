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
    ///     Interface to define who is member of a group and how
    ///     By default how is characterized by an allocation of capacity to define part-time membership
    /// </summary>
    public class OrganizationResource : IOrganizationResource
    {
        public OrganizationResource(IAgentId organizationId, IAgentId resourceId, IResourceUsage usage, float weight=100)
        {
            Source = organizationId;
            Target = resourceId;
            Usage = usage;
            Weight = weight;
        }

        #region IOrganizationResource Members

        public IResourceUsage Usage { get; }


        public object Clone()
        {
            return new OrganizationResource(Source, Target, Usage, Weight);
        }

        public bool Equals(IResourceUsage resourceUsage)
        {
            return Usage.Equals(resourceUsage);
        }

        #endregion

        public override bool Equals(object obj)
        {
            return obj is OrganizationResource organizationResource &&
                   Target.Equals(organizationResource.Target) &&
                   Source.Equals(organizationResource.Source) &&
                   Usage.Equals(organizationResource.Usage);
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


        #endregion
    }
}