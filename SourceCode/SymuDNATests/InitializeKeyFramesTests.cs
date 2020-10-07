#region Licence

// Description: SymuBiz - SymuDNATests
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.Common.Interfaces;
using Symu.DNA;
using Symu.OrgMod.GraphNetworks;

#endregion

namespace SymuDNATests
{
    [TestClass]
    public class InitializeKeyFramesTests
    {
        [TestMethod]
        public void InitializeMatrixAgentBeliefTest()
        {
            var agentIdsRef = new List<IAgentId>();
            for (ushort i = 0; i < 10; i++)
            {
                agentIdsRef.Add(new AgentId(i, 1));
            }

            var agentsRef = new VectorNetwork(agentIdsRef);

            var beliefIdsRef = new List<IAgentId>();
            for (ushort i = 0; i < 10; i++)
            {
                beliefIdsRef.Add(new AgentId(i, 1));
            }

            var beliefsRef = new VectorNetwork(beliefIdsRef);

            var agentIds = new List<IAgentId>();
            for (ushort i = 0; i < 4; i++)
            {
                agentIds.Add(new AgentId(i, 1));
            }

            for (ushort i = 5; i < 9; i++)
            {
                agentIds.Add(new AgentId(i, 1));
            }

            var beliefIds = new List<IAgentId>();
            for (ushort i = 0; i < 4; i++)
            {
                beliefIds.Add(new AgentId(i, 1));
            }

            for (ushort i = 5; i < 9; i++)
            {
                beliefIds.Add(new AgentId(i, 1));
            }

            var frame = new MatrixMetaNetwork
            {
                Actor = new VectorNetwork(agentIds),
                Belief = new VectorNetwork(beliefIds),
                ActorBelief = Matrix<float>.Build.Dense(8, 8, 1F)
            };

            var result =
                InitializeKeyFrames.InitializeMatrix(agentsRef, beliefsRef, frame.Actor, frame.Belief,
                    frame.ActorBelief);
            // Add 2 rows/cols
            Assert.AreEqual(10, result.RowCount);
            Assert.AreEqual(10, result.ColumnCount);
            // Add Row/column 4
            Assert.AreEqual(0, result[0, 4]);
            Assert.AreEqual(0, result[4, 4]);
            Assert.AreEqual(0, result[4, 0]);
            // Add Row/column 9
            Assert.AreEqual(0, result[0, 9]);
            Assert.AreEqual(0, result[9, 9]);
            Assert.AreEqual(0, result[9, 0]);
        }
    }
}