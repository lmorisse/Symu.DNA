#region Licence

// Description: SymuBiz - SymuDNA
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System.Collections.Generic;
using System.Linq;
using Symu.Common.Interfaces;
using Symu.DNA.Edges;
using Symu.DNA.Entities;

#endregion

namespace Symu.DNA.GraphNetworks.TwoModesNetworks
{
    /// <summary>
    ///     Resource x Resource network
    ///     Which Resource uses what resource
    ///     Source : Resource
    ///     Target : Resource
    /// </summary>
    public class ResourceResourceNetwork : TwoModesNetwork<IResourceResource>
    {

        /// <summary>
        ///     Get the IActorResource used by an actor with a specific type of use
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="resourceUsage"></param>
        /// <returns></returns>
        public IResourceResource Edge(IAgentId source, IAgentId target, IResourceUsage resourceUsage)
        {
            return Exists(source, target) ? Edges(source, target).FirstOrDefault(n => n.Equals(resourceUsage)) : null;
        }

        public bool HasResource(IAgentId source, IAgentId target, IResourceUsage resourceUsage)
        {
            return Exists(source, target) && Edges(source, target).ToList().Exists(n => n.Usage.Equals(resourceUsage));
        }

        /// <summary>
        ///     Get the list of all the resources the agentId is using filtered by type of use
        /// </summary>
        /// <param name="source"></param>
        /// <param name="resourceUsage"></param>
        /// <returns></returns>
        public IEnumerable<IAgentId> TargetsFromSource(IAgentId source, IResourceUsage resourceUsage)
        {
            return ExistsSource(source)
                ? EdgesFilteredBySource(source).Where(n => n.Equals(resourceUsage)).Select(x => x.Target)
                : new List<IAgentId>();
        }

        /// <summary>
        ///     Get the list of all the resources the agentId is using filtered by type of use
        /// </summary>
        /// <param name="target"></param>
        /// <param name="resourceUsage"></param>
        /// <returns></returns>
        public IEnumerable<IAgentId> SourcesFromTarget(IAgentId target, IResourceUsage resourceUsage)
        {
            return ExistsTarget(target)
                ? EdgesFilteredByTarget(target).Where(n => n.Equals(resourceUsage)).Select(x => x.Source)
                : new List<IAgentId>();
        }

        public void RemoveResource(IAgentId resourceId)
        {
            RemoveSource(resourceId);
            RemoveTarget(resourceId);
        }

        public float GetWeight(IAgentId source, IAgentId target, IResourceUsage resourceUsage)
        {
            if (HasResource(source, target, resourceUsage))
            {
                return Edge(source, target, resourceUsage).Weight;
            }

            return 0;
        }
    }
}