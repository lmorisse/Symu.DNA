﻿#region Licence

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

#endregion

namespace Symu.DNA.Agent
{
    /// <summary>
    ///     thread-safe list of agents that can be accessed by multiple threads concurrently
    /// </summary>
    public class ConcurrentAgents//<T> where T : IAgent
    {
        private readonly ConcurrentDictionary<IAgentId, IAgent> _list = new ConcurrentDictionary<IAgentId, IAgent>();
        public int Count => _list.Count;

        internal ushort CountByClassId(IClassId classId)
        {
            var count = _list.Values.Count(a => a.AgentId.Equals(classId));
            return Convert.ToUInt16(count);
        }

        internal void Add(IAgent agent)
        {
            _list[agent.AgentId] = agent;
        }

        public bool Exists(IAgentId agentId)
        {
            if (agentId == null)
            {
                throw new ArgumentNullException(nameof(agentId));
            }

            return _list.ContainsKey(agentId);
        }

        public void Clear()
        {
            _list.Clear();
        }

        /// <summary>
        ///     Get a typed agent by its agentId
        /// </summary>
        /// <typeparam name="TAgent"></typeparam>
        /// <param name="agentId"></param>
        /// <returns>The typed agent</returns>
        public TAgent Get<TAgent>(IAgentId agentId) where TAgent : IAgent
        {
            if (Exists(agentId))
            {
                return (TAgent)_list[agentId] ;
            }

            return default;
        }

        public IAgent Get(IAgentId agentId)
        {
            return Exists(agentId) ? _list[agentId] : default;
        }

        /// <summary>
        ///     Returns a list with the names of all the agents.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IAgentId> GetKeys()
        {
            return _list.Keys;
        }

        /// <summary>
        ///     Returns a list with the names of all the agents.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IAgent> GetValues()
        {
            return _list.Values;
        }

        /// <summary>
        ///     Returns a list with the names of all the agents that contain a certain string.
        /// </summary>
        /// <returns>The name fragment that the agent names should contain</returns>
        public IEnumerable<IAgent> FilteredByClassId(IClassId classId)
        {
            return GetValues().Where(a => a.AgentId.Equals(classId));
        }

        /// <summary>
        ///     Returns a list with the names of all the agents that contain a certain string.
        /// </summary>
        /// <returns>The name fragment that the agent names should contain</returns>
        public IEnumerable<IAgentId> FilteredKeysByClassId(IClassId classId)
        {
            return _list.Keys.Where(a => a.Equals(classId));
        }

        /// <summary>
        ///     Stops the execution of the agent identified by name and removes it from the environment. Use the Remove method
        ///     instead of Agent.Stop
        ///     when the decision to stop an agent does not belong to the agent itself, but to some other agent or to an external
        ///     factor.
        ///     Don't call it directly, use WhitePages.RemoveAgent
        /// </summary>
        /// <param name="agentId">The name of the agent to be removed</param>
        public void Remove(IAgentId agentId)
        {
            if (Exists(agentId))
            {
                var remove = _list.TryRemove(agentId, out _);
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
    }
}