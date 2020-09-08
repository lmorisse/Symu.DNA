#region Licence

// Description: SymuBiz - Symu
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Collections.Concurrent;
using MathNet.Numerics.LinearAlgebra;
using Symu.Common.Classes;
using Symu.Common.Interfaces.Agent;
using Symu.Common.Interfaces.Entity;
using Symu.DNA.MatrixNetworks;
using Symu.DNA.Networks.OneModeNetworks;

#endregion

namespace Symu.DNA.Networks.TwoModesNetworks
{
    /// <summary>
    ///     Resource * task network
    ///     What resources can do what task
    /// </summary>
    public class ResourceTaskNetwork : TwoModesNetwork<IId, ITask>
    {
        /// <summary>
        /// Convert the network into a matrix
        /// </summary>
        /// <param name="resourceIds"></param>
        /// <param name="taskIds"></param>
        /// <returns></returns>
        public Matrix<float> ToMatrix(VectorNetwork<IId> resourceIds, VectorNetwork<IId> taskIds)
        {
            if (!resourceIds.Any || !taskIds.Any)
            {
                return null;
            }
            var matrix = Matrix<float>.Build.Dense(resourceIds.Count, taskIds.Count);
            for (var i = 0; i < resourceIds.Count; i++)
            {
                var agentId = resourceIds.IndexItem[i];
                if (!resourceIds.ItemIndex.ContainsKey(agentId))
                {
                    throw new NullReferenceException(nameof(resourceIds.ItemIndex));
                }
                var row = resourceIds.ItemIndex[agentId];
                foreach (var task in List[agentId])
                {
                    if (!taskIds.ItemIndex.ContainsKey(task.Id))
                    {
                        throw new NullReferenceException(nameof(taskIds.ItemIndex));
                    }
                    var column = taskIds.ItemIndex[task.Id];
                    matrix[row, column] = 1;//resourceTask.Value;
                }
            }
            return matrix;
        }
    }
}