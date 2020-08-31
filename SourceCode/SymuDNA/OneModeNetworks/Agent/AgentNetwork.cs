﻿#region Licence

// Description: SymuBiz - Symu
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Collections.Generic;
using System.Linq;
using Symu.Common.Interfaces.Agent;
using Symu.Common.Interfaces.Entity;

#endregion

namespace Symu.DNA.OneModeNetworks.Agent
{
    /// <summary>
    ///     Network agents of this environment
    /// </summary>
    /// <remarks>Also named Actor in social network analysis</remarks>
    public class AgentNetwork
    {

        public ConcurrentAgents Agents { get; } = new ConcurrentAgents();
        public int Count => Agents.Count;

        public bool Any()
        {
            return Agents.Count > 0;
        }
        public void Clear()
        {
            Agents.Clear();
        }

        public void Add(IAgent agent)
        {
            if (agent == null)
            {
                throw new ArgumentNullException(nameof(agent));
            }

            if (!Exists(agent.AgentId))
            {
                Agents.Add(agent);
            }
        }

        public void RemoveAgent(IAgentId agentId)
        {
            Agents.Remove(agentId);
        }

        public bool Exists(IAgentId agentId)
        {
            return Agents.Exists(agentId);
        }

        public TAgent Get<TAgent>(IAgentId agentId) where TAgent : IAgent
        {
            return Agents.Get<TAgent>(agentId);
        }

        public IAgent Get(IAgentId agentId)
        {
            return Agents.Get(agentId);
        }

        /// <summary>
        ///     Returns a list with the names of all the agents.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IAgentId> GetKeys()
        {
            return Agents.GetKeys();
        }

        /// <summary>
        ///     Returns a list with the names of all the agents.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IAgent> GetValues()
        {
            return Agents.GetValues();
        }

        /// <summary>
        ///     The number of agents in the environment
        /// </summary>
        public ushort CountByClassId(IClassId classKey)
        {
            return Agents.CountByClassId(classKey);
        }

        /// <summary>
        ///     Returns a list with the names of all the agents that contain a certain string.
        /// </summary>
        /// <returns>The name fragment that the agent names should contain</returns>
        public IEnumerable<IAgentId> FilteredKeysByClassId(IClassId classId)
        {
            return Agents.FilteredKeysByClassId(classId);
        }

        /// <summary>
        ///     Returns a list with the names of all the agents that contain a certain string.
        /// </summary>
        /// <returns>The name fragment that the agent names should contain</returns>
        public IEnumerable<IAgent> FilteredByClassId(IClassId classId)
        {
            return Agents.FilteredByClassId(classId);
        }

        public IAgentId GetAgentId(IId id)
        {
            return Agents.GetKeys().ToList().Find(x => x.Id.Equals(id));
        }
    }
}