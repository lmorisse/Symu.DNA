using Microsoft.VisualStudio.TestTools.UnitTesting;

using Symu.DNA;

using System;
using System.Collections.Generic;
using System.Text;
using MathNet.Numerics.LinearAlgebra;
using Symu.Common.Interfaces.Agent;
using Symu.Common.Interfaces.Entity;
using Symu.DNA.MatrixNetworks;
using Symu.DNA.MatrixNetworks.OneModeNetworks;

namespace Symu.DNA.Tests
{
    [TestClass()]
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
            var agentsRef = new VectorNetwork<IAgentId>(agentIdsRef);

            var beliefIdsRef = new List<IId>();
            for (ushort i = 0; i < 10; i++)
            {
                beliefIdsRef.Add(new UId(i));
            }
            var beliefsRef = new VectorNetwork<IId>(beliefIdsRef);

            var agentIds = new List<IAgentId>();
            for (ushort i = 0; i < 4; i++)
            {
                agentIds.Add(new AgentId(i, 1));
            }
            for (ushort i = 5; i < 9; i++)
            {
                agentIds.Add(new AgentId(i, 1));
            }
            var beliefIds = new List<IId>();
            for (ushort i = 0; i < 4; i++)
            {
                beliefIds.Add(new UId(i));
            }
            for (ushort i = 5; i < 9; i++)
            {
                beliefIds.Add(new UId(i));
            }
            var frame = new MatrixMetaNetwork
            {
                Agent = new VectorNetwork<IAgentId>(agentIds),
                Belief = new VectorNetwork<IId>(beliefIds),
                AgentBelief = Matrix<float>.Build.Dense(8, 8, 1F)
            };

            var result = InitializeKeyFrames.InitializeMatrix(agentsRef, beliefsRef, frame.Agent, frame.Belief, frame.AgentBelief);
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