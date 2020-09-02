#region Licence

// Description: SymuBiz - Symu
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using MathNet.Numerics.LinearAlgebra;
using Symu.Common.Classes;

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Symu.Common.Interfaces;
using Symu.DNA.MatrixNetworks;

#endregion

namespace Symu.DNA
{
    /// <summary>
    ///     A meta-network overtime is collected as a set of Matrix Networks
    /// </summary>
    /// <remarks>DNA book</remarks>
    /// <remarks>Organizational Network Analysis book</remarks>
    public class KeyFrames : IResult
    {

        private readonly Dictionary<ushort, MatrixMetaNetwork> _list = new Dictionary<ushort, MatrixMetaNetwork>();

        public void Add(ushort frame, MatrixMetaNetwork network)
        {
            _list.Add(frame, network);
        }

        public MatrixMetaNetwork Get(ushort frame)
        {
            return _list.ContainsKey(frame) ? _list[frame] : null;
        }

        public IEnumerable<MatrixMetaNetwork> GetNetworks => _list.Values;
        public List<ushort> GetFrames => _list.Keys.ToList();

        /// <summary>
        /// Get a specific analysis about a network over-time
        /// </summary>
        /// <param name="analysisType"></param>
        /// <param name="networkType"></param>
        /// <returns></returns>
        public List<float> Analysis(NetworkAnalysisType analysisType, NetworkType networkType)
        {

            return GetNetworks.Select(network => NetworkAnalysis.Analysis(analysisType, network.Get(networkType))).ToList();
        }

        #region IResult interface   
        /// <summary>
        ///     If set to true, Tasks will be filled with value and stored during the simulation
        /// </summary>
        public bool On { get; set; }

        /// <summary>
        ///     Frequency of the results
        /// </summary>
        public TimeStepType Frequency { get; set; }
     
        /// <summary>
        /// Clear all the existing frames
        /// </summary>
        public void Clear()
        {
            _list.Clear();
        }
        /// <summary>
        ///     Put the logic to compute the result and store it in the list
        /// </summary>
        public void SetResults()
        {
            // intentionally empty => done in Add method
        }

        /// <summary>
        ///     Copy each parameter of the instance in the object
        /// </summary>
        /// <returns></returns>
        public void CopyTo(object clone)
        {
            if (!(clone is KeyFrames cloneMessages))
            {
                return;
            }

            foreach (var result in _list)
            {
                cloneMessages.Add(result.Key, result.Value);
            }
        }

        /// <summary>
        ///     Clone the instance.
        ///     IterationResult is the actual iterationResult.
        ///     With a multiple iterations simulation, SimulationResults store a clone of each IterationResult
        ///     It is a factory that create a SymuResults then CopyTo the existing instance and return the clone
        /// </summary>
        /// <returns></returns>
        public IResult Clone()
        {
            var clone = new KeyFrames();
            CopyTo(clone);
            return clone;
        }
        #endregion
    }
}