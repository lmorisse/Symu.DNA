#region Licence

// Description: SymuBiz - SymuDNA
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using MathNet.Numerics.LinearAlgebra;
using Symu.Common.Math.LinearAlgebra;
using static Symu.Common.Constants;

#endregion

namespace Symu.DNA.Metrics.TwoModes
{
    /// <summary>
    ///     Methods to analyze two modes networks analysis group by Variance
    /// </summary>
    /// <remarks>DNA book</remarks>
    /// <remarks>Organizational Network Analysis book</remarks>
    // todo RowDegreeCentralization
    // todo ColumnDegreeCentralization
    // todo InDegreeCentralization : mesure de la popularité
    // todo OutDegreeCentralization : mesure de la grégarité
    public static class VarianceMetrics
    {
        #region Centralization

        /// <summary>
        ///     The centrality degree can be interpreted in terms of the ability of a node to pick up anything that passes nearby
        ///     on the network (such as a virus, or information)
        ///     Highly centralized networks are often not very robust since single nodes have very much power, influence, or
        ///     control over the network and removing these nodes can endanger the whole network
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        /// <remarks>https://fr.wikipedia.org/wiki/Centralit%C3%A9</remarks>
        public static float DegreeCentralization(Matrix<float> matrix)
        {
            if (matrix == null)
            {
                throw new ArgumentNullException(nameof(matrix));
            }

            var degreeCentrality = matrix.DegreeCentrality();
            var maxDegree = degreeCentrality.Maximum();
            //maxDegree- degreeCentrality
            var tempMatrix = Matrix<float>.Build.Dense(matrix.RowCount, matrix.ColumnCount, maxDegree) -
                             degreeCentrality;
            var tempVector = tempMatrix.RowSums();
            var max = tempVector.Maximum();
            return max > Tolerance ? tempVector.Sum() / max : 0;
        }

        /// <summary>
        ///     The centrality degree can be interpreted in terms of the ability of a node to pick up anything that passes nearby
        ///     on the network (such as a virus, or information)
        ///     Calculated by row
        ///     Highly centralized networks are often not very robust since single nodes have very much power, influence, or
        ///     control over the network and removing these nodes can endanger the whole network
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        /// <remarks>https://fr.wikipedia.org/wiki/Centralit%C3%A9</remarks>
        public static Vector<float> RowDegreeCentralization(Matrix<float> matrix)
        {
            if (matrix == null)
            {
                throw new ArgumentNullException(nameof(matrix));
            }

            var degreeCentrality = matrix.DegreeCentrality();
            var maxDegree = degreeCentrality.Maximum();
            var tempMatrix = Matrix<float>.Build.Dense(matrix.RowCount, matrix.ColumnCount, maxDegree) -
                             degreeCentrality;
            var tempVector = tempMatrix.RowSums();
            var max = tempVector.Maximum();
            return max > Tolerance ? tempVector / max : Vector<float>.Build.Dense(tempVector.Count);
        }

        /// <summary>
        ///     The centrality degree can be interpreted in terms of the ability of a node to pick up anything that passes nearby
        ///     on the network (such as a virus, or information)
        ///     Calculated by column
        ///     Highly centralized networks are often not very robust since single nodes have very much power, influence, or
        ///     control over the network and removing these nodes can endanger the whole network
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        /// <remarks>https://fr.wikipedia.org/wiki/Centralit%C3%A9</remarks>
        public static Vector<float> ColumnDegreeCentralization(Matrix<float> matrix)
        {
            if (matrix == null)
            {
                throw new ArgumentNullException(nameof(matrix));
            }

            var degreeCentrality = matrix.DegreeCentrality();
            var maxDegree = degreeCentrality.Maximum();
            var tempMatrix = Matrix<float>.Build.Dense(matrix.RowCount, matrix.ColumnCount, maxDegree) -
                             degreeCentrality;
            var tempVector = tempMatrix.ColumnSums();
            var max = tempVector.Maximum();
            return max > Tolerance ? tempVector / max : Vector<float>.Build.Dense(tempVector.Count);
        }

        #endregion
    }
}