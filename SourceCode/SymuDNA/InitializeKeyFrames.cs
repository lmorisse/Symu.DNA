#region Licence

// Description: SymuBiz - SymuDNA
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;
using Symu.Common.Interfaces;
using Symu.DNA.MatrixNetworks;

#endregion

namespace Symu.DNA
{
    /// <summary>
    ///     Networks may have different lengths over time.
    ///     Initialize analysis normalizes all the network over the time
    /// </summary>
    /// <example>
    ///     An AgentGroup network may have 1 group at frame 1 and 2 groups at frame 2.
    ///     the MatrixAgentGroup may have 1 column at frame 1 and two columns at frame 2.
    ///     If it is the case, the initialization will add a new column filled with 0 at frame 1 for the second new group
    /// </example>
    public static class InitializeKeyFrames
    {
        /// <summary>
        ///     Networks may have different lengths over time.
        ///     Initialize analysis normalizes all the network over the time
        /// </summary>
        /// <example>
        ///     An AgentGroup network may have 1 group at frame 1 and 2 groups at frame 2.
        ///     the MatrixAgentGroup may have 1 column at frame 1 and two columns at frame 2.
        ///     If it is the case, the initialization will add a new column filled with 0 at frame 1 for the second new group
        /// </example>
        public static MatrixMetaNetwork InitializeAnalysis(Dictionary<ushort, MatrixMetaNetwork> list)
        {
            var refMetaNetwork = new MatrixMetaNetwork
            {
                Actor = new VectorNetwork(GetAgentIdsOverTime(list.Values.Select(x => x.Actor))),
                Role = new VectorNetwork(GetAgentIdsOverTime(list.Values.Select(x => x.Role))),
                Resource = new VectorNetwork(GetAgentIdsOverTime(list.Values.Select(x => x.Resource))),
                Knowledge = new VectorNetwork(GetAgentIdsOverTime(list.Values.Select(x => x.Knowledge))),
                Belief = new VectorNetwork(GetAgentIdsOverTime(list.Values.Select(x => x.Belief))),
                Task = new VectorNetwork(GetAgentIdsOverTime(list.Values.Select(x => x.Task))),
                Event = new VectorNetwork(GetAgentIdsOverTime(list.Values.Select(x => x.Event))),
                Organization = new VectorNetwork(GetAgentIdsOverTime(list.Values.Select(x => x.Organization)))
            };

            foreach (var frame in list.Values)
            {
                frame.ActorBelief = InitializeMatrix(refMetaNetwork.Actor, refMetaNetwork.Belief, frame.Actor,
                    frame.Belief, frame.ActorBelief);
                frame.ActorOrganization = InitializeMatrix(refMetaNetwork.Actor, refMetaNetwork.Organization,
                    frame.Actor, frame.Organization, frame.ActorOrganization);
                frame.ActorKnowledge = InitializeMatrix(refMetaNetwork.Actor, refMetaNetwork.Knowledge, frame.Actor,
                    frame.Knowledge, frame.ActorKnowledge);
                frame.ActorResource = InitializeMatrix(refMetaNetwork.Actor, refMetaNetwork.Resource, frame.Actor,
                    frame.Resource, frame.ActorResource);
                frame.ActorRole = InitializeMatrix(refMetaNetwork.Actor, refMetaNetwork.Role, frame.Actor, frame.Role,
                    frame.ActorRole);
                //frame.Interaction = InitializeMatrix(refMetaNetwork.Agent, refMetaNetwork.Belief, frame.Agent, frame.Belief, frame.AgentBelief);
            }

            return refMetaNetwork;
        }

        public static Matrix<float> InitializeMatrix(VectorNetwork rowVectorRef, VectorNetwork colVectorRef,
            VectorNetwork rowVector, VectorNetwork colVector, Matrix<float> matrix)
        {
            // row comparison
            if (rowVector.Count != rowVectorRef.Count)
            {
                foreach (var agentRef in rowVectorRef.ItemIndex.Where(x => !rowVector.ItemIndex.ContainsKey(x.Key)))
                {
                    var newRow = Vector<float>.Build.Dense(matrix.ColumnCount);
                    matrix = matrix.InsertRow(agentRef.Value, newRow);
                }
            }

            // Column comparison
            if (colVector.Count == colVectorRef.Count)
            {
                return matrix;
            }

            foreach (var beliefRef in colVectorRef.ItemIndex.Where(x => !colVector.ItemIndex.ContainsKey(x.Key)))
            {
                var newColumn = Vector<float>.Build.Dense(matrix.RowCount);
                matrix = matrix.InsertColumn(beliefRef.Value, newColumn);
            }

            return matrix;
        }

        /// <summary>
        ///     Get the exhaustive list of the agentIds over-time
        /// </summary>
        /// <returns></returns>
        public static IReadOnlyList<IAgentId> GetAgentIdsOverTime(IEnumerable<VectorNetwork> vectors)
        {
            var agentIds = new List<IAgentId>();
            foreach (var vector in vectors)
            {
                agentIds.AddRange(vector.IndexItem);
            }

            return agentIds.Distinct().ToList();
        }
    }
}