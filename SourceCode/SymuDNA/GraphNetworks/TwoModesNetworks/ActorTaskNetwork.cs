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
    ///     Actor x Task network, called assignment network
    ///     Who (Actor) does what (Tasks)
    /// </summary>
    public class ActorTaskNetwork: ConcurrentTwoModesNetwork<IAgentId, IActorTask>
    {

        public override bool Exists(IAgentId key, IActorTask value)
        {
            return Exists(key) && List[key].Exists(x => x.Task.Equals(value.Task));
        }

        public IEnumerable<ITask> GetTasks(IAgentId actorId)
        {
            return Exists(actorId) ? List[actorId].Select(x => x.Task) : new ITask[0];
        }

        public void RemoveTask(IAgentId taskId)
        {
            foreach (var actorTask in List.Values)
            {
                actorTask.RemoveAll(x => x.Task.EntityId.Equals(taskId));
            }
        }

        /// <summary>
        /// Convert the network into a matrix
        /// </summary>
        /// <param name="actorIds"></param>
        /// <param name="taskIds"></param>
        /// <returns></returns>
        public Matrix<float> ToMatrix(VectorNetwork<IAgentId> actorIds, VectorNetwork<IId> taskIds)
        {
            if (!actorIds.Any || !taskIds.Any)
            {
                return null;
            }
            var matrix = Matrix<float>.Build.Dense(actorIds.Count, taskIds.Count);
            for (var i = 0; i < actorIds.Count; i++)
            {
                var actorId = actorIds.IndexItem[i];
                if (!actorIds.ItemIndex.ContainsKey(actorId))
                {
                    throw new NullReferenceException(nameof(actorIds.ItemIndex));
                }

                var row = actorIds.ItemIndex[actorId];
                if (!Exists(actorId))
                {
                    continue;
                }

                foreach (var agentTask in List[actorId])
                {
                    if (!taskIds.ItemIndex.ContainsKey(agentTask.Task.EntityId))
                    {
                        throw new NullReferenceException(nameof(taskIds.ItemIndex));
                    }

                    var column = taskIds.ItemIndex[agentTask.Task.EntityId];
                    matrix[row, column] = 1; //agentActivity.Value;
                }
            }
            return matrix;
        }

        public bool HasTask(ITask task)
        {
            return List.Values.ToList().Exists(x => x.Exists(y => y.Task.Equals(task)));
        }


    }
}