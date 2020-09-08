#region Licence

// Description: SymuBiz - Symu
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;
using Symu.Common.Interfaces.Agent;
using Symu.Common.Interfaces.Entity;
using Symu.DNA.MatrixNetworks;
using Symu.DNA.Networks.OneModeNetworks;

#endregion

namespace Symu.DNA.Networks.TwoModesNetworks
{
    /// <summary>
    ///     Agent x Resource network, called capabilities network
    ///     Who has what resource
    /// </summary>
    /// <example>database, products, routines, processes, ...</example>
    public class AgentResourceNetwork : ConcurrentTwoModesNetwork<IAgentId, IAgentResource>
    {

        public bool Exists(IAgentId agentId, IId resourceId)
        {
            return Exists(agentId) && List[agentId].Exists(x => x.Equals(resourceId));
        }

        /// <summary>
        ///     Add a value to a key
        ///     Key is supposed to be already present in the collection.
        ///     if not use Add method
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public override void AddValue(IAgentId key, IAgentResource value)
        {
            if (List[key].Contains(value))
            {
                return;
            }

            List[key].Add(value);

            var totalAllocation =
                List[key].Sum(x => x.Allocation);

            // There is non main object used at 100%
            // Objects are added as things progress, with the good allocation
            if (!(totalAllocation >= 100))
            {
                return;
            }

            foreach (var resource in List[key])
            {
                //Don't use ComponentAllocation[ca] *= 100/ => return 0
                resource.Allocation = Convert.ToSingle(resource.Allocation * 100 / totalAllocation);
            }
        }

        public float GetAllocation(IAgentId agentId, IId resourceId)
        {
            if (HasResource(agentId, resourceId))
            {
                return GetAgentResource(agentId, resourceId).Sum(x => x.Allocation);
            }

            return 0;
        }

        public float GetAllocation(IAgentId agentId, IId resourceId, IResourceUsage resourceUsage)
        {
            if (HasResource(agentId, resourceId, resourceUsage))
            {
                return GetAgentResource(agentId, resourceId, resourceUsage).Allocation;
            }

            return 0;
        }
   
        /// <summary>
        ///     Get the IAgentResource used by an agent with a specific type of use
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        public List<IAgentResource> GetAgentResource(IAgentId agentId, IId resourceId)
        {
            return HasResource(agentId, resourceId) ? List[agentId].FindAll(n => n.Equals(resourceId)) : null;
        }
        /// <summary>
        ///     Get the IAgentResource used by an agent with a specific type of use
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="resourceId"></param>
        /// <param name="resourceUsage"></param>
        /// <returns></returns>
        public IAgentResource GetAgentResource(IAgentId agentId, IId resourceId, IResourceUsage resourceUsage)
        {
            return HasResource(agentId, resourceId, resourceUsage) ? List[agentId].Find(n => n.Equals(resourceId, resourceUsage)) : null;
        }
        public bool HasResource(IAgentId agentId, IId resourceId, IResourceUsage resourceUsage)
        {
            return Exists(agentId) && List[agentId].Exists(n => n.Equals(resourceId, resourceUsage));
        }
        public bool HasResource(IAgentId agentId, IId resourceId)
        {
            return Exists(agentId) && List[agentId].Exists(n => n.Equals(resourceId));
        }
        public bool HasResource(IAgentId agentId, IResourceUsage resourceUsage)
        {
            return Exists(agentId) && List[agentId].Exists(n => n.Equals(resourceUsage));
        }

        /// <summary>
        ///     Get the list of all the resources the agentId is using
        /// </summary>
        /// <param name="agentId"></param>
        /// <returns></returns>
        public IEnumerable<IId> GetResourceIds(IAgentId agentId)
        {
            return Exists(agentId) ? List[agentId].Select(x => x.ResourceId) : new List<IId>();
        }

        /// <summary>
        ///     Get the list of all the resources the agentId is using filtered by type of use
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="resourceUsage"></param>
        /// <returns></returns>
        public IEnumerable<IId> GetResourceIds(IAgentId agentId, IResourceUsage resourceUsage)
        {
            return Exists(agentId)
                ? List[agentId].FindAll(n => n.Equals(resourceUsage)).Select(x => x.ResourceId)
                : new List<IId>();
        }

        /// <summary>
        ///     Get all the agents of classId using resourceId
        /// </summary>
        /// <param name="resourceId"></param>
        /// <param name="resourceUsage"></param>
        /// <param name="agentClassId"></param>
        /// <returns></returns>
        public List<IAgentId> GetAgentIds(IId resourceId, IResourceUsage resourceUsage, IClassId agentClassId)
        {
            return (from agentId 
                in List.Keys.Where(x => x.ClassId.Equals(agentClassId)) 
                let agentResource = List[agentId] 
                where agentResource.Exists(x => x.Equals(resourceId, resourceUsage))
                select agentId).ToList();
        }

        /// <summary>
        ///     agentId is added to groupId 
        ///     agentId inherits all the resources of the groupId
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="groupId"></param>
        public void AddMemberToGroup(IAgentId agentId, IAgentId groupId)
        {
            if (!Exists(groupId))
            {
                return;
            }
            AddKey(agentId);
            foreach (var groupResourceId in List[groupId])
            {
                Add(agentId, groupResourceId.Clone());//.ResourceId, groupResourceId.ResourceUsage, groupResourceId.ResourceAllocation);
            }
        }

        /// <summary>
        ///     agentId is removed to groupId 
        ///     agentId looses all the resources of the groupId
        /// </summary>
        public void RemoveMemberFromGroup(IAgentId agentId, IAgentId groupId)
        {
            if (!Exists(agentId) || !Exists(groupId))
            {
                return;
            }

            foreach (var groupResourceId in List[groupId])
            {
                List[agentId].RemoveAll(n => n.Equals(groupResourceId.ResourceId) && n.Equals(groupResourceId.Usage));
            }
        }

        public void Remove(IAgentId agentId, IId resourceId)
        {
            if (!Exists(agentId))
            {
                return;
            }
            List[agentId].RemoveAll(l => l.Equals(resourceId));
        }
        /// <summary>
        ///     Copy the same network from an agent to another
        /// </summary>
        /// <param name="fromAgentId"></param>
        /// <param name="toAgentId"></param>
        public void CopyTo(IAgentId fromAgentId, IAgentId toAgentId)
        {
            if (!Exists(fromAgentId))
            {
                return;
            }
            AddKey(toAgentId);
            List[toAgentId].Clear();
            foreach (var agentResource in List[fromAgentId])
            {
                Add(toAgentId, agentResource.Clone());
            }
        }


        /// <summary>
        /// Convert the network into a matrix
        /// </summary>
        /// <param name="agentIds"></param>
        /// <param name="resourceIds"></param>
        /// <returns></returns>
        public Matrix<float> ToMatrix(VectorNetwork<IAgentId> agentIds, VectorNetwork<IId> resourceIds)
        {
            if (!agentIds.Any || !resourceIds.Any)
            {
                return null;
            }
            var matrix = Matrix<float>.Build.Dense(agentIds.Count, resourceIds.Count);
            foreach (var agentResources in List)
            {
                if (!agentIds.ItemIndex.ContainsKey(agentResources.Key))
                {
                    throw new NullReferenceException(nameof(agentIds.ItemIndex));
                }
                var row = agentIds.ItemIndex[agentResources.Key];
                foreach (var agentResource in agentResources.Value)
                {
                    if (!resourceIds.ItemIndex.ContainsKey(agentResource.ResourceId))
                    {
                        throw new NullReferenceException(nameof(resourceIds.ItemIndex));
                    }
                    var column = resourceIds.ItemIndex[agentResource.ResourceId];
                    matrix[row, column] = agentResource.Allocation;
                }
            }
            return matrix;
        }
    }
}