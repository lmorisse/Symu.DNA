#region Licence

// Description: SymuBiz - SymuDNA
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using MathNet.Numerics.LinearAlgebra;

#endregion

namespace Symu.DNA.Metrics.TwoModes
{
    /// <summary>
    ///     Methods to analyze two modes networks analysis
    /// </summary>
    /// <remarks>DNA book</remarks>
    /// <remarks>Organizational Network Analysis book</remarks>
    public static class TwoModesMetrics
    {
        public static object Analysis(NetworkMetricType type, Matrix<float> matrix, int indexAgent)
        {
            if (matrix == null)
            {
                return 0;
            }

            switch (type)
            {
                #region Quantity

                case NetworkMetricType.Density:
                    return QuantityMetrics.Density(matrix);
                case NetworkMetricType.Load:
                    return QuantityMetrics.Load(matrix);
                case NetworkMetricType.RowCount:
                    return QuantityMetrics.RowCount(matrix);
                case NetworkMetricType.ColumnCount:
                    return QuantityMetrics.ColumnCount(matrix);
                case NetworkMetricType.RowDegreeCentrality:
                    return QuantityMetrics.RowDegreeCentrality(matrix, indexAgent);
                case NetworkMetricType.ColumnDegreeCentrality:
                    return QuantityMetrics.ColumnDegreeCentrality(matrix);

                #endregion

                #region Variance

                case NetworkMetricType.DegreeCentralization:
                    return VarianceMetrics.DegreeCentralization(matrix);
                case NetworkMetricType.RowDegreeCentralization:
                    return VarianceMetrics.RowDegreeCentralization(matrix);
                case NetworkMetricType.ColumnDegreeCentralization:
                    return VarianceMetrics.ColumnDegreeCentralization(matrix);

                #endregion

                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}