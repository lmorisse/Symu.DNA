#region Licence

// Description: SymuBiz - Symu
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using Symu.Common.Classes;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using Symu.Common.Interfaces;
using Symu.Common.Interfaces.Agent;
using Symu.Common.Interfaces.Entity;
using Symu.DNA.MatrixNetworks;
using Symu.DNA.Metrics;
using Symu.DNA.Metrics.TwoModes;

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

        #region Initialization

        private MatrixMetaNetwork _refMetaNetwork;
        /// <summary>
        /// Networks may have different lengths over time.
        /// Initialize analysis normalizes all the network over the time 
        /// </summary>
        /// <example>An AgentGroup network may have 1 group at frame 1 and 2 groups at frame 2.
        /// the MatrixAgentGroup may have 1 column at frame 1 and two columns at frame 2.
        /// If it is the case, the initialization will add a new column filled with 0 at frame 1 for the second new group</example>
        public void InitializeAnalysis()
        {
            _refMetaNetwork = InitializeKeyFrames.InitializeAnalysis(_list);
        }

        public IAgentId[] AgentIds => _refMetaNetwork.Agent.IndexItem;

        public IAgentId[] GroupsIds => _refMetaNetwork.Group.IndexItem;

        public IId[] RoleIds => _refMetaNetwork.Role.IndexItem;

        public IId[] ResourceIds => _refMetaNetwork.Resource.IndexItem;
        
        public IId[] KnowledgeIds => _refMetaNetwork.Knowledge.IndexItem;
        
        public IId[] BeliefIds => _refMetaNetwork.Belief.IndexItem;
        
        public IId[] ActivityIds => _refMetaNetwork.Activity.IndexItem;
        
        public IId[] EventIds => _refMetaNetwork.Event.IndexItem;

        #endregion


        #region Analyis

        public const int NoFilter = -1;
        /// <summary>
        /// Get a specific analysis about a network over-time
        /// </summary>
        /// <param name="metricType"></param>
        /// <param name="networkType"></param>
        /// <param name="id">If you want to filter a specific agentId</param>
        /// <returns></returns>
        public List<object> Analysis(NetworkMetricType metricType, NetworkType networkType, IId id)
        {
            var filter = NoFilter;
            if (id != null && !id.IsNull)
            {
                var agentId = _refMetaNetwork.Agent.ItemIndex.Keys.ToList().Find(x => x.Id.Equals(id));
                filter = _refMetaNetwork.Agent.ItemIndex[agentId];
            }
            return GetNetworks.Select(network => TwoModesMetrics.Analysis(metricType, network.Get(networkType), filter))
                    .ToList();
        }
        #endregion

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