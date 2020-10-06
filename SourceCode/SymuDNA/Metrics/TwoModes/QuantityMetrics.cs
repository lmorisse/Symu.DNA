#region Licence

// Description: SymuBiz - SymuDNA
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using MathNet.Numerics.LinearAlgebra;
using Symu.Common.Math.LinearAlgebra;

#endregion

namespace Symu.DNA.Metrics.TwoModes
{
    /// <summary>
    ///     Methods to analyze two modes networks analysis group by Quantity
    ///     Derived from sum or count
    /// </summary>
    /// <remarks>DNA book</remarks>
    /// <remarks>Organizational Network Analysis book</remarks>
    //todo InDegreeCentrality : for directed link
    //todo OutDegreeCentrality : for directed link
    //todo EdgeCount 
    public static class QuantityMetrics
    {
        #region Degree

        /// <summary>
        ///     Row count
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static int RowCount(Matrix<float> matrix)
        {
            return matrix.RowCount;
        }

        /// <summary>
        ///     Column count
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static int ColumnCount(Matrix<float> matrix)
        {
            return matrix.ColumnCount;
        }

        /// <summary>
        ///     Normalized row entries count
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="indexAgent">Filtering on agentId</param>
        /// <returns>A Vector<float> if NoFilter, a float otherwise</float></returns>
        public static object RowDegreeCentrality(Matrix<float> matrix, int indexAgent)
        {
            var result = RowDegreeCentrality(matrix);
            if (indexAgent == KeyFrames.NoFilter)
            {
                return RowDegreeCentrality(matrix);
            }

            return result[indexAgent];
        }

        /// <summary>
        ///     Normalized row entries count
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static Vector<float> RowDegreeCentrality(Matrix<float> matrix)
        {
            var result = Vector<float>.Build.Dense(matrix.RowCount);
            if (matrix.RowCount * matrix.ColumnCount > 0)
            {
                result = matrix.RowSums() / matrix.ColumnCount;
            }

            return result;
        }

        /// <summary>
        ///     Normalized column entries count
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static Vector<float> ColumnDegreeCentrality(Matrix<float> matrix)
        {
            var result = Vector<float>.Build.Dense(matrix.ColumnCount);
            if (matrix.RowCount * matrix.ColumnCount > 0)
            {
                result = matrix.ColumnSums() / matrix.RowCount;
            }

            return result;
        }

        #endregion

        #region Load & density

        /// <summary>
        ///     It's a network level concept that identifies the average amount of, e.g. knowledge, per agent
        ///     It tell us how loaded a network is
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static float Load(Matrix<float> matrix)
        {
            if (matrix.ColumnCount > 0)
            {
                return matrix.GrandSum() / matrix.ColumnCount;
            }

            return 0;
        }

        /// <summary>
        ///     The ratio of the number of links versus the maximum possible links for a network
        ///     Number of possible relation that reflects the level of social  organizational cohesion.
        ///     Density is a normalized value of the load
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns>the density from 0 to 1</returns>
        /// <remarks>
        ///     typical social network density levels range from 5% to 30%  in the case of frequent or very frequent
        ///     information exchanges
        /// </remarks>
        /// <remarks>
        ///     a minimum social network density of 15%—20% may  reflect efficient information and knowledge sharing in a
        ///     network comprising approximately 100 nodes
        /// </remarks>
        public static float Density(Matrix<float> matrix)
        {
            if (matrix.RowCount * matrix.ColumnCount > 0)
            {
                return matrix.GrandSum() / (matrix.RowCount * matrix.ColumnCount);
            }

            return 0;
        }

        #endregion
    }
}