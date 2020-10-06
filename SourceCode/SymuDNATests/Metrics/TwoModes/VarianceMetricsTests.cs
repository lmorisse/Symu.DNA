#region Licence

// Description: SymuBiz - SymuDNATests
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using MathNet.Numerics.LinearAlgebra;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.DNA.Metrics.TwoModes;

#endregion

namespace SymuDNATests.Metrics.TwoModes
{
    [TestClass]
    public class VarianceMetricsTests
    {
        /// <summary>
        ///     1 dim matrix filled with 0
        /// </summary>
        private readonly Matrix<float> _matrix10 = Matrix<float>.Build.Dense(1, 1);

        /// <summary>
        ///     1 dim matrix filled with 1
        /// </summary>
        private readonly Matrix<float> _matrix11 = Matrix<float>.Build.Dense(1, 1, 1F);

        /// <summary>
        ///     2 dim matrix filled with 0
        /// </summary>
        private readonly Matrix<float> _matrix20 = Matrix<float>.Build.Dense(2, 2);

        /// <summary>
        ///     2 dim matrix filled with 1
        /// </summary>
        private readonly Matrix<float> _matrix21 = Matrix<float>.Build.Dense(2, 2, 1F);

        /// <summary>
        ///     2 dim matrix identity
        /// </summary>
        private readonly Matrix<float> _matrix2Identity = Matrix<float>.Build.DenseIdentity(2);

        private Matrix<float> _matrix2;

        [TestInitialize]
        public void Initialize()
        {
            _matrix2 = Matrix<float>.Build.Dense(2, 2);
            _matrix2[0, 0] = 1;
            _matrix2[0, 1] = 1;
        }

        [TestMethod]
        public void DegreeCentralityTest()
        {
            Assert.AreEqual(0, VarianceMetrics.DegreeCentralization(_matrix10));
            Assert.AreEqual(0, VarianceMetrics.DegreeCentralization(_matrix11));
            Assert.AreEqual(0, VarianceMetrics.DegreeCentralization(_matrix20));
            Assert.AreEqual(0, VarianceMetrics.DegreeCentralization(_matrix21));
            Assert.AreEqual(2, VarianceMetrics.DegreeCentralization(_matrix2Identity));
            Assert.AreEqual(1, VarianceMetrics.DegreeCentralization(_matrix2));
        }

        [TestMethod]
        public void RowDegreeCentralityTest()
        {
            Assert.AreEqual(0, VarianceMetrics.RowDegreeCentralization(_matrix10).Sum());
            Assert.AreEqual(0, VarianceMetrics.RowDegreeCentralization(_matrix11).Sum());
            Assert.AreEqual(0, VarianceMetrics.RowDegreeCentralization(_matrix20).Sum());
            Assert.AreEqual(0, VarianceMetrics.RowDegreeCentralization(_matrix21).Sum());
            var rowDegreeCentralization = VarianceMetrics.RowDegreeCentralization(_matrix2Identity);
            Assert.AreEqual(2, rowDegreeCentralization.Sum());
            Assert.AreEqual(1, rowDegreeCentralization[0]);
            Assert.AreEqual(1, rowDegreeCentralization[1]);

            var degreeCentralization = VarianceMetrics.RowDegreeCentralization(_matrix2);
            Assert.AreEqual(1, degreeCentralization.Sum());
            Assert.AreEqual(1, degreeCentralization[0]);
            Assert.AreEqual(0, degreeCentralization[1]);
        }

        [TestMethod]
        public void ColumnDegreeCentralityTest()
        {
            Assert.AreEqual(0, VarianceMetrics.ColumnDegreeCentralization(_matrix10).Sum());
            Assert.AreEqual(0, VarianceMetrics.ColumnDegreeCentralization(_matrix11).Sum());
            Assert.AreEqual(0, VarianceMetrics.ColumnDegreeCentralization(_matrix20).Sum());
            Assert.AreEqual(0, VarianceMetrics.ColumnDegreeCentralization(_matrix21).Sum());
            var rowDegreeCentralization = VarianceMetrics.ColumnDegreeCentralization(_matrix2Identity);
            Assert.AreEqual(2, rowDegreeCentralization.Sum());
            Assert.AreEqual(1, rowDegreeCentralization[0]);
            Assert.AreEqual(1, rowDegreeCentralization[1]);

            var degreeCentralization = VarianceMetrics.ColumnDegreeCentralization(_matrix2);
            Assert.AreEqual(2, degreeCentralization.Sum());
            Assert.AreEqual(1, degreeCentralization[0]);
            Assert.AreEqual(1, degreeCentralization[1]);
        }
    }
}