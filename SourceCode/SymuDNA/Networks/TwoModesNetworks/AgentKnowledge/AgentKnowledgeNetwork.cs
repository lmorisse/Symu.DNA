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
using Symu.DNA.MatrixNetworks.OneModeNetworks;

#endregion

namespace Symu.DNA.Networks.TwoModesNetworks.AgentKnowledge
{
    /// <summary>
    ///     Knowledge network
    ///     Who (agentId) knows what (knowledge)
    ///     Key => the agentId
    ///     Value : the list of NetworkInformation the agent knows
    /// </summary>
    /// <example></example>
    public class AgentKnowledgeNetwork
    {
        /// <summary>
        ///     List
        ///     Key => ComponentId
        ///     Values => AgentExpertise : list of KnowledgeIds/KnowledgeBits/KnowledgeLevel of an agent
        /// </summary>
        public ConcurrentDictionary<IAgentId, AgentExpertise> List { get; } =
            new ConcurrentDictionary<IAgentId, AgentExpertise>();

        public bool Any()
        {
            return List.Any();
        }

        public void Clear()
        {
            List.Clear();
        }
        public bool Exists(IAgentId agentId, IId knowledgeId)
        {
            return Exists(agentId) && List[agentId].Contains(knowledgeId);
        }

        public bool Exists(IAgentId agentId)
        {
            return List.ContainsKey(agentId);
        }

        public void Add(IAgentId agentId, AgentExpertise expertise)
        {
            if (expertise is null)
            {
                throw new ArgumentNullException(nameof(expertise));
            }

            AddAgentId(agentId, expertise);


            foreach (var agentKnowledge in expertise.List.Where(a => !List[agentId].Contains(a)))
            {
                List[agentId].Add(agentKnowledge);
            }
        }

        public void Add(IAgentId agentId, IAgentKnowledge agentKnowledge)
        {
            AddAgentId(agentId);
            AddAgentKnowledge(agentId, agentKnowledge);
        }

        /// <summary>
        ///     Add a knowledge to an AgentId
        ///     AgentId is supposed to be already present in the collection.
        ///     if not use Add method
        /// </summary>
        /// <param name="agentKnowledge"></param>
        /// <param name="agentId"></param>
        public void AddAgentKnowledge(IAgentId agentId, IAgentKnowledge agentKnowledge)
        {
            if (agentKnowledge == null)
            {
                throw new ArgumentNullException(nameof(agentKnowledge));
            }

            if (!Exists(agentId, agentKnowledge.KnowledgeId))
            {
                List[agentId].Add(agentKnowledge);
            }
        }

        public void AddAgentId(IAgentId agentId, AgentExpertise agentExpertise)
        {
            if (!Exists(agentId))
            {
                List.TryAdd(agentId, agentExpertise);
            }
        }

        public void AddAgentId(IAgentId agentId)
        {
            if (!Exists(agentId))
            {
                List.TryAdd(agentId, new AgentExpertise());
            }
        }

        public IEnumerable<IAgentId> FilterAgentsWithKnowledge(IEnumerable<IAgentId> agentIds, IId knowledgeId)
        {
            if (agentIds is null)
            {
                throw new ArgumentNullException(nameof(agentIds));
            }

            return agentIds.Where(agentId => Exists(agentId) && List[agentId].Contains(knowledgeId))
                .ToList();
        }

        public IEnumerable<IId> GetKnowledgeIds(IAgentId agentId)
        {
            if (!Exists(agentId))
            {
                throw new NullReferenceException(nameof(agentId));
            }

            return List[agentId].GetKnowledgeIds();
        }

        public void RemoveAgent(IAgentId agentId)
        {
            List.TryRemove(agentId, out _);
        }

        /// <summary>
        ///     Get Agent Expertise
        /// </summary>
        /// <param name="agentId"></param>
        /// <returns>NullReferenceException if agentId don't Exists, AgentExpertise otherwise</returns>
        public AgentExpertise GetAgentExpertise(IAgentId agentId)
        {
            if (!Exists(agentId))
            {
                throw new NullReferenceException(nameof(agentId));
            }

            return List[agentId];
        }

        /// <summary>
        ///     Get Agent Knowledge
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="knowledgeId"></param>
        /// <returns>null if agentId don't Exists, AgentExpertise otherwise</returns>
        public IAgentKnowledge GetAgentKnowledge(IAgentId agentId, IId knowledgeId)
        {
            if (!Exists(agentId, knowledgeId))
            {
                throw new NullReferenceException(nameof(agentId));
            }

            return List[agentId].GetAgentKnowledge(knowledgeId);
        }

        /// <summary>
        ///     Get Agent Knowledge
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="knowledgeId"></param>
        /// <returns>null if agentId don't Exists, AgentExpertise otherwise</returns>
        public TAgentKnowledge GetAgentKnowledge<TAgentKnowledge>(IAgentId agentId, IId knowledgeId) where TAgentKnowledge : IAgentKnowledge
        {
            return (TAgentKnowledge)GetAgentKnowledge(agentId, knowledgeId);
        }
        /// <summary>
        /// Convert the network into a matrix
        /// </summary>
        /// <param name="agentIds"></param>
        /// <param name="knowledgeIds"></param>
        /// <returns></returns>
        public Matrix<float> ToMatrix(VectorNetwork<IAgentId> agentIds, VectorNetwork<IId> knowledgeIds)
        {
            if (!agentIds.Any || !knowledgeIds.Any)
            {
                return null;
            }
            var matrix = Matrix<float>.Build.Dense(agentIds.Count, knowledgeIds.Count);
            foreach (var agentExpertise in List)
            {
                if (!agentIds.ItemIndex.ContainsKey(agentExpertise.Key))
                {
                    throw new NullReferenceException(nameof(agentIds.ItemIndex));
                }
                var row = agentIds.ItemIndex[agentExpertise.Key];
                foreach (var agentKnowledge in agentExpertise.Value.List)
                {
                    if (!knowledgeIds.ItemIndex.ContainsKey(agentKnowledge.KnowledgeId))
                    {
                        throw new NullReferenceException(nameof(knowledgeIds.ItemIndex));
                    }
                    var column = knowledgeIds.ItemIndex[agentKnowledge.KnowledgeId];
                    matrix[row, column] = agentKnowledge.Value;
                }
            }
            return matrix;
        }
    }
}