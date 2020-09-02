#region Licence

// Description: SymuBiz - Symu
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using MathNet.Numerics.LinearAlgebra;

#endregion

namespace Symu.DNA.MatrixNetworks
{
    /// <summary>
    ///     Methods to analyze a network
    /// </summary>
    /// <remarks>DNA book</remarks>
    /// <remarks>Organizational Network Analysis book</remarks>
    public static class NetworkAnalysis
    {
        public static float Analysis(NetworkAnalysisType type, Matrix<float> matrix)
        {
            if (matrix == null)
            {
                return 0;
            }
            switch (type)
            {
                case NetworkAnalysisType.Connectivity:
                    return Connectivity(matrix);
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        /// <summary>
        /// Technical method to calculate the grand sum of a matrix (the sum of all the elements)
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static float GrandSum(Matrix<float> matrix)
        {
            return matrix.RowSums().Sum();
        }

        /// <summary>
        /// The ratio of the number of links versus the maximum possible links for a network
        /// Number of possible relation that reflects the level of social  organizational cohesion.
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static float Connectivity(Matrix<float> matrix)
        {
            if (matrix.RowCount * matrix.ColumnCount > 0)
            {
                return GrandSum(matrix) / (matrix.RowCount * matrix.ColumnCount);
            }

            return 0;
        }
    }
}