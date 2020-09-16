#region Licence

// Description: SymuBiz - Symu
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;
using Symu.Common.Interfaces.Agent;
using Symu.Common.Interfaces.Entity;
using Symu.DNA.Entities;
using Symu.DNA.MatrixNetworks;

#endregion

namespace Symu.DNA.GraphNetworks.TwoModesNetworks
{
    /// <summary>
    ///     Task * Knowledge network
    ///     What knowledge is necessary for what Task
    /// </summary>
    public class TaskKnowledgeNetwork : TwoModesNetwork<IId, IKnowledge>
    {

        public IEnumerable<IId> GetKnowledgeIds(IId taskId)
        {
            return Exists(taskId) ? List[taskId].Select(x => x.EntityId) : new IId[0];
        }


        public void RemoveTask(IAgentId taskId)
        {
            Remove(taskId);
        }
        public void RemoveKnowledge(IAgentId knowledgeId)
        {
            foreach (var knowledge in List.Values)
            {
                knowledge.RemoveAll(x => x.EntityId.Equals(knowledgeId));
            }
        }

        /// <summary>
        /// Convert the network into a matrix
        /// </summary>
        /// <param name="taskIds"></param>
        /// <param name="knowledgeIds"></param>
        /// <returns></returns>
        public Matrix<float> ToMatrix(VectorNetwork<IId> taskIds, VectorNetwork<IId> knowledgeIds)
        {
            if (!taskIds.Any || !knowledgeIds.Any)
            {
                return null;
            }
            var matrix = Matrix<float>.Build.Dense(taskIds.Count, knowledgeIds.Count);
            for (var i = 0; i < taskIds.Count; i++)
            {
                var taskId = taskIds.IndexItem[i];
                if (!taskIds.ItemIndex.ContainsKey(taskId))
                {
                    throw new NullReferenceException(nameof(taskIds.ItemIndex));
                }
                var row = taskIds.ItemIndex[taskId];
                foreach (var knowledge in List[taskId])
                {
                    if (!knowledgeIds.ItemIndex.ContainsKey(knowledge.EntityId))
                    {
                        throw new NullReferenceException(nameof(knowledgeIds.ItemIndex));
                    }
                    var column = knowledgeIds.ItemIndex[knowledge.EntityId];
                    matrix[row, column] = 1;//resourceTask.Value;
                }
            }
            return matrix;
        }

    }
}