#region Licence

// Description: SymuBiz - SymuDNA
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;
using Symu.Common.Interfaces;
using Symu.DNA.Edges;
using Symu.DNA.MatrixNetworks;

using static Symu.Common.Constants;

#endregion

namespace Symu.DNA.GraphNetworks.TwoModesNetworks
{
    /// <summary>
    ///     Abstract class for two modes network
    /// </summary>
    /// <remarks>
    ///     todo should be replaced by QuickGraph
    ///     TVertex = IAgentId
    /// </remarks>
    public class TwoModesNetwork<TEdge> where TEdge : class, IEdge
    {
        /// <summary>
        ///     List of all edges of the graph
        /// </summary>
        protected readonly List<TEdge> List = new List<TEdge>();

        /// <summary>
        ///     Gets or sets the element at the specified index
        /// </summary>
        /// <param name="index">0 based</param>
        /// <returns></returns>
        public TEdge this[int index]
        {
            get => List[index];
            set => List[index] = value;
        }

        public int Count => List.Count;


        public bool Any()
        {
            return List.Any();
        }

        public void Clear()
        {
            List.Clear();
        }

        /// <summary>
        ///     Check that the source exists in the repository
        /// </summary>
        /// <param name="sourceId"></param>
        /// <returns></returns>
        public bool ExistsSource(IAgentId sourceId)
        {
            return List.Exists(x => x.Source.Equals(sourceId));
        }

        /// <summary>
        ///     Check that the source exists in the repository
        /// </summary>
        /// <param name="targetId"></param>
        /// <returns></returns>
        public bool ExistsTarget(IAgentId targetId)
        {
            return List.Exists(x => x.Target.Equals(targetId));
        }

        public bool Exists(TEdge edge)
        {
            return List.Exists(x => x.Equals(edge));
        }

        public virtual bool Exists(IAgentId sourceId, IAgentId targetId)
        {
            return List.Exists(x => x.Source.Equals(sourceId) && x.Target.Equals(targetId));
        }

        public virtual void Add(TEdge edge)
        {
            if (Exists(edge))
            {
                return;
            }

            List.Add(edge);
        }

        public void Add(IEnumerable<TEdge> edges)
        {
            foreach (var edge in edges)
            {
                Add(edge);
            }
        }

        /// <summary>
        ///     Make a copy of of the network
        /// </summary>
        /// <param name="network"></param>
        public void CopyTo(TwoModesNetwork<TEdge> network)
        {
            if (network is null)
            {
                throw new ArgumentNullException(nameof(network));
            }

            foreach (var edge in List)
            {
                network.Add(edge);
            }
        }

        public void Remove(TEdge edge)
        {
            List.RemoveAll(x => x.Equals(edge));
        }

        public void Remove(IAgentId sourceId, IAgentId targetId)
        {
            List.RemoveAll(x => x.Source.Equals(sourceId) && x.Target.Equals(targetId));
        }

        public void RemoveSource(IAgentId sourceId)
        {
            List.RemoveAll(x => x.Source.Equals(sourceId));
        }

        public void RemoveTarget(IAgentId targetId)
        {
            List.RemoveAll(x => x.Target.Equals(targetId));
        }

        /// <summary>
        ///     Convert the network into a matrix
        /// </summary>
        /// <param name="sourceIds"></param>
        /// <param name="targetIds"></param>
        /// <returns></returns>
        public Matrix<float> ToMatrix(VectorNetwork sourceIds, VectorNetwork targetIds)
        {
            if (!sourceIds.Any || !targetIds.Any)
            {
                return null;
            }

            var matrix = Matrix<float>.Build.Dense(sourceIds.Count, targetIds.Count);
            for (var i = 0; i < sourceIds.Count; i++)
            {
                var sourceId = sourceIds.IndexItem[i];
                if (!sourceIds.ItemIndex.ContainsKey(sourceId))
                {
                    throw new NullReferenceException(nameof(sourceIds.ItemIndex));
                }

                var row = sourceIds.ItemIndex[sourceId];
                foreach (var edge in EdgesFilteredBySource(sourceId))
                {
                    if (!targetIds.ItemIndex.ContainsKey(edge.Target))
                    {
                        throw new NullReferenceException(nameof(targetIds.ItemIndex));
                    }

                    var column = targetIds.ItemIndex[edge.Target];
                    matrix[row, column] = edge.Weight;
                }
            }

            return matrix;
        }

        #region Edge

        public virtual TEdge Edge(IAgentId sourceId, IAgentId targetId)
        {
            return List.FirstOrDefault(x => x.Source.Equals(sourceId) && x.Target.Equals(targetId));
        }
        public virtual TTEdge Edge<TTEdge>(IAgentId sourceId, IAgentId targetId) where TTEdge : TEdge
        {
            return (TTEdge)List.FirstOrDefault(x => x.Source.Equals(sourceId) && x.Target.Equals(targetId));
        }

        public TEdge Edge(TEdge edge)
        {
            return Edge(edge.Source, edge.Target);
        }

        public IEnumerable<TEdge> Edges()
        {
            return List;
        }

        public virtual IEnumerable<TEdge> Edges(IAgentId sourceId, IAgentId targetId)
        {
            return List.Where(x => x.Source.Equals(sourceId) && x.Target.Equals(targetId));
        }

        public IEnumerable<TEdge> EdgesFilteredBySourceAndTargetClassId(IAgentId sourceId, IClassId targetClassId)
        {
            return List.Where(x => x.Source.Equals(sourceId) && x.Target.ClassId.Equals(targetClassId));
        }

        public IEnumerable<TEdge> EdgesFilteredByTargetAndSourceClassId(IAgentId targetId, IClassId sourceClassId)
        {
            return List.Where(x => x.Target.Equals(targetId) && x.Source.ClassId.Equals(sourceClassId));
        }

        public IEnumerable<TEdge> EdgesFilteredBySource(IAgentId sourceId)
        {
            return List.FindAll(x => x.Source.Equals(sourceId));
        }

        public IEnumerable<TTEdge> EdgesFilteredBySource<TTEdge>(IAgentId sourceId) where TTEdge : TEdge
        {
            return EdgesFilteredBySource(sourceId).Cast<TTEdge>();
        }

        public IEnumerable<TEdge> EdgesFilteredByTarget(IAgentId targetId)
        {
            return List.FindAll(x => x.Target.Equals(targetId));
        }

        public ushort EdgesFilteredBySourceCount(IAgentId sourceId)
        {
            return (ushort) List.Count(x => x.Source.Equals(sourceId));
        }

        public ushort EdgesFilteredByTargetCount(IAgentId targetId)
        {
            return (ushort) List.Count(x => x.Target.Equals(targetId));
        }

        #endregion

        #region Target

        public IEnumerable<IAgentId> Targets()
        {
            return List.Select(x => x.Target).Distinct();
        }

        public IEnumerable<IAgentId> TargetsFilteredBySource(IAgentId sourceId)
        {
            return List.FindAll(x => x.Source.Equals(sourceId)).Select(x => x.Target).Distinct();
        }

        public IEnumerable<IAgentId> TargetsFilteredByTargetClassId(IClassId targetClassId)
        {
            return List.FindAll(x => x.Target.ClassId.Equals(targetClassId)).Select(x => x.Target).Distinct();
        }

        public IEnumerable<IAgentId> TargetsFilteredBySourceAndTargetClassId(IAgentId sourceId, IClassId targetClassId)
        {
            return EdgesFilteredBySourceAndTargetClassId(sourceId, targetClassId).Select(x => x.Target).Distinct();
        }

        #endregion

        #region Source

        public IEnumerable<IAgentId> Sources()
        {
            return List.Select(x => x.Source).Distinct();
        }

        public IEnumerable<IAgentId> SourcesFilteredByTarget(IAgentId targetId)
        {
            return List.FindAll(x => x.Target.Equals(targetId)).Select(x => x.Source).Distinct();
        }

        public IEnumerable<IAgentId> SourcesFilteredByTargetAndSourceClassId(IAgentId targetId, IClassId sourceClassId)
        {
            return EdgesFilteredByTargetAndSourceClassId(targetId, sourceClassId).Select(x => x.Source).Distinct();
        }

        public ushort SourcesFilteredByTargetAndSourceClassIdCount(IAgentId targetId, IClassId sourceClassId)
        {
            return (ushort) EdgesFilteredByTargetAndSourceClassId(targetId, sourceClassId).Count();
        }

        #endregion

        #region Weight

        public float Weight(IEdge edge)
        {
            return Weight(edge.Source, edge.Target);
        }

        public float Weight(IAgentId sourceId, IAgentId targetId)
        {
            return Edges(sourceId, targetId).Sum(x => x.Weight);
        }

        public float WeightFilteredBySource(IAgentId sourceId)
        {
            return EdgesFilteredBySource(sourceId).Sum(x => x.Weight);
        }

        public float WeightFilteredByTarget(IAgentId targetId)
        {
            return EdgesFilteredByTarget(targetId).Sum(x => x.Weight);
        }
        private float _maxWeight;
        public float NormalizedWeight(IAgentId actorId1, IAgentId actorId2)
        {
            return _maxWeight < Tolerance ? 0 : Weight(actorId1, actorId2) / _maxWeight;
        }
        /// <summary>
        /// Normalize weights of the network
        /// Call this method before calling NormalizedWeight method
        /// This method compute the edge.NormalizedWeight by dividing the edge.Weight by the maximum weight found in the network
        /// </summary>
        public void NormalizeWeights()
        {
            _maxWeight = Any() ? List.Max(x => x.Weight) : 0;
            foreach (var edge in List)
            {
                edge.NormalizedWeight = edge.Weight / _maxWeight;
            }
        }

        #endregion

        /// <summary>
        /// Copy all edges of a sourceId * targetFromId to another targetToId
        /// </summary>
        /// <param name="sourceId"></param>
        /// <param name="targetFromId"></param>
        /// <param name="targetToId"></param>
        public void CopyTo(IAgentId sourceId, IAgentId targetFromId, IAgentId targetToId)
        {
            foreach (var edge in Edges(sourceId, targetFromId).ToImmutableList())
            {
                var copy = (TEdge)edge.Clone();
                copy.Target = targetToId;
                Add(copy);
            }
        }
        /// <summary>
        /// Copy all edges of a targetFromId to another targetToId
        /// </summary>
        /// <param name="targetFromId"></param>
        /// <param name="targetToId"></param>
        public void CopyToFromTarget(IAgentId targetFromId, IAgentId targetToId)
        {
            foreach (var edge in EdgesFilteredByTarget(targetFromId).ToImmutableList())
            {
                var copy = (TEdge)edge.Clone();
                copy.Target = targetToId;
                Add(copy);
            }
        }
        /// <summary>
        /// Copy all edges of a sourceFromId to another sourceToId
        /// </summary>
        /// <param name="sourceFromId"></param>
        /// <param name="sourceToId"></param>
        public void CopyToFromSource(IAgentId sourceFromId, IAgentId sourceToId)
        {
            foreach (var edge in EdgesFilteredBySource(sourceFromId).ToImmutableList())
            {
                var copy = (TEdge)edge.Clone();
                copy.Source = sourceToId;
                Add(copy);
            }
        }
    }
}