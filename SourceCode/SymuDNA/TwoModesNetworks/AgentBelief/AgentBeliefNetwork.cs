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

namespace Symu.DNA.TwoModesNetworks.AgentBelief
{
    /// <summary>
    ///     Belief network
    ///     Who (agentId) knows what (Belief)
    ///     Key => the agentId
    ///     Value : the list of NetworkInformation the agent knows
    /// </summary>
    /// <example></example>
    public class AgentBeliefNetwork
    {

        /// <summary>
        ///     List
        ///     Key => ComponentId
        ///     Values => AgentBelief : list of BeliefIds/BeliefBits/BeliefLevel of an agent
        /// </summary>
        public ConcurrentDictionary<IAgentId, AgentBeliefs> List { get; } =
            new ConcurrentDictionary<IAgentId, AgentBeliefs>();

        public int Count => List.Count;

        public bool Any()
        {
            return List.Any();
        }

        public void Clear()
        {
            List.Clear();
        }

        public bool Exists(IAgentId agentId)
        {
            return List.ContainsKey(agentId);
        }

        public bool Exists(IAgentId agentId, IId beliefId)
        {
            return Exists(agentId) && List[agentId].Contains(beliefId);
        }

        public void Add(IAgentId agentId, IAgentBelief agentBelief)
        {
            AddAgentId(agentId);
            AddBelief(agentId, agentBelief);
        }

        /// <summary>
        ///     Add a Belief to an AgentId
        ///     AgentId is supposed to be already present in the collection.
        ///     if not use Add method
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="agentBelief"></param>
        public void AddBelief(IAgentId agentId, IAgentBelief agentBelief)
        {
            if (agentBelief == null)
            {
                throw new ArgumentNullException(nameof(agentBelief));
            }

            if (!List[agentId].Contains(agentBelief.BeliefId))
            {
                List[agentId].Add(agentBelief);
            }
        }

        public void AddAgentId(IAgentId agentId)
        {
            if (!Exists(agentId))
            {
                List.TryAdd(agentId, new AgentBeliefs());
            }
        }

        public void RemoveAgent(IAgentId agentId)
        {
            List.TryRemove(agentId, out _);
        }

        /// <summary>
        ///     Get Agent beliefs
        /// </summary>
        /// <param name="agentId"></param>
        /// <returns>null if agentId don't Exists, AgentBelief otherwise</returns>
        public AgentBeliefs GetAgentBeliefs(IAgentId agentId)
        {
            if (!Exists(agentId))
            {
                throw new NullReferenceException(nameof(agentId));
            }

            return List[agentId];
        }

        public IEnumerable<IId> GetBeliefIds(IAgentId agentId)
        {
            if (!Exists(agentId))
            {
                throw new NullReferenceException(nameof(agentId));
            }

            return List[agentId].GetBeliefIds();
        }

        /// <summary>
        ///     Get Agent belief
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="beliefId"></param>
        /// <returns>null if agentId don't Exists, AgentBelief otherwise</returns>
        public IAgentBelief GetAgentBelief(IAgentId agentId, IId beliefId)
        {
            var agentBeliefs = GetAgentBeliefs(agentId);
            if (agentBeliefs is null)
            {
                throw new NullReferenceException(nameof(agentBeliefs));
            }

            return agentBeliefs.GetAgentBelief(beliefId);
        }  
        
        /// <summary>
        ///     Get Agent belief
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="beliefId"></param>
        /// <returns>null if agentId don't Exists, AgentBelief otherwise</returns>
        public TAgentBelief GetAgentBelief<TAgentBelief>(IAgentId agentId, IId beliefId) where TAgentBelief : IAgentBelief
        {
            return (TAgentBelief) GetAgentBelief(agentId, beliefId);
        }
    }
}