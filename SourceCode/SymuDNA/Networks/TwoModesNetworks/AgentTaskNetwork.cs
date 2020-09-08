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
using Symu.DNA.MatrixNetworks;
using Symu.DNA.Networks.OneModeNetworks;
using IKnowledge = Symu.DNA.Networks.OneModeNetworks.IKnowledge;

#endregion

namespace Symu.DNA.Networks.TwoModesNetworks
{
    /// <summary>
    ///     Agent x Task network, called assignment network
    ///     Who (agent) does what (Tasks)
    /// </summary>
    public class AgentTaskNetwork: ConcurrentTwoModesNetwork<IAgentId, IAgentTask>
    {

        public override bool Exists(IAgentId key, IAgentTask value)
        {
            return Exists(key) && List[key].Exists(x => x.Task.Equals(value.Task));
        }

        public IEnumerable<ITask> GetTasks(IAgentId agentId)
        {
            return Exists(agentId) ? List[agentId].Select(x => x.Task) : null;
        }

        /// <summary>
        /// Convert the network into a matrix
        /// </summary>
        /// <param name="agentIds"></param>
        /// <param name="taskIds"></param>
        /// <returns></returns>
        public Matrix<float> ToMatrix(VectorNetwork<IAgentId> agentIds, VectorNetwork<IId> taskIds)
        {
            if (!agentIds.Any || !taskIds.Any)
            {
                return null;
            }
            var matrix = Matrix<float>.Build.Dense(agentIds.Count, taskIds.Count);
            for (var i = 0; i < agentIds.Count; i++)
            {
                var agentId = agentIds.IndexItem[i];
                if (!agentIds.ItemIndex.ContainsKey(agentId))
                {
                    throw new NullReferenceException(nameof(agentIds.ItemIndex));
                }

                var row = agentIds.ItemIndex[agentId];
                if (!Exists(agentId))
                {
                    continue;
                }

                foreach (var agentTask in List[agentId])
                {
                    if (!taskIds.ItemIndex.ContainsKey(agentTask.Task.Id))
                    {
                        throw new NullReferenceException(nameof(taskIds.ItemIndex));
                    }

                    var column = taskIds.ItemIndex[agentTask.Task.Id];
                    matrix[row, column] = 1; //agentActivity.Value;
                }
            }
            return matrix;
        }
    }
}