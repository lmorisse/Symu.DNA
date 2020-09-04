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
using Symu.Common.Interfaces.Agent;
using Symu.Common.Interfaces.Entity;

#endregion

namespace Symu.DNA.Networks.OneModeNetworks
{
    /// <summary>
    ///     Network agents of this environment
    /// </summary>
    /// <remarks>Also named Actor in social network analysis</remarks>
    public class AgentNetwork
    {
        public readonly ConcurrentDictionary<IAgentId, IAgent> List = new ConcurrentDictionary<IAgentId, IAgent>();
        public int Count => List.Count;

        public bool Any()
        {
            return List.Count > 0;
        }
        public void Clear()
        {
            List.Clear();
        }

        public void Add(IAgent agent)
        {
            if (agent == null)
            {
                throw new ArgumentNullException(nameof(agent));
            }

            if (!Exists(agent.AgentId))
            {
                List[agent.AgentId] = agent;
            }
        }

        public void Remove(IAgentId agentId)
        {      
            if (Exists(agentId))
            {
                var remove = List.TryRemove(agentId, out _);
                if (!remove)
                {
                    throw new Exception("Concurrent access");
                }
            }
            else
            {
                throw new Exception("Agent " + agentId + " does not exist (ConcurrentEnvironment.Remove)");
            }
        }

        public bool Exists(IAgentId agentId)
        {
            if (agentId == null)
            {
                throw new ArgumentNullException(nameof(agentId));
            }

            return List.ContainsKey(agentId);
        }

        /// <summary>
        ///     Get a typed agent by its agentId
        /// </summary>
        /// <typeparam name="TAgent"></typeparam>
        /// <param name="agentId"></param>
        /// <returns>The typed agent</returns>
        public TAgent Get<TAgent>(IAgentId agentId) where TAgent : IAgent
        {
            return (TAgent) Get(agentId);
        }
        

        public IAgent Get(IAgentId agentId)
        {
            return Exists(agentId) ? List[agentId] : default;
        }

        /// <summary>
        ///     Returns a list with the agentIds.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IAgentId> GetIds()
        {
            return List.Keys;
        }

        /// <summary>
        ///     Returns a list with the agents.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IAgent> Gets()
        {
            return List.Values;
        }

        /// <summary>
        ///     The number of agents in the environment
        /// </summary>
        public ushort CountByClassId(IClassId classId)
        {
            var count = List.Values.Count(a => a.AgentId.Equals(classId));
            return Convert.ToUInt16(count);
        }

        /// <summary>
        ///     Returns a list with the names of all the agents that contain a certain string.
        /// </summary>
        /// <returns>The name fragment that the agent names should contain</returns>
        public IEnumerable<IAgentId> FilteredKeysByClassId(IClassId classId)
        {
            return List.Keys.Where(a => a.Equals(classId));
        }

        /// <summary>
        ///     Returns a list with the names of all the agents that contain a certain string.
        /// </summary>
        /// <returns>The name fragment that the agent names should contain</returns>
        public IEnumerable<IAgent> FilteredByClassId(IClassId classId)
        {
            return List.Values.Where(a => a.AgentId.Equals(classId));
        }

        public IAgentId GetId(IId id)
        {
            return List.Keys.ToList().Find(x => x.Id.Equals(id));
        }

        public IReadOnlyList<IAgentId> ToVector()
        {
            return GetIds().OrderBy(x => x.Id).ToList();
        }
    }
}