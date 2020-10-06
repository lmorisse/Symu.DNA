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
    public class QuantityMetricsTests
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

        #region Quantity

        [TestMethod]
        public void RowDegreeCentralityTest()
        {
            Assert.AreEqual(0, QuantityMetrics.RowDegreeCentrality(_matrix10).Sum());
            Assert.AreEqual(1, QuantityMetrics.RowDegreeCentrality(_matrix11).Sum());
            Assert.AreEqual(0, QuantityMetrics.RowDegreeCentrality(_matrix20).Sum());
            Assert.AreEqual(2, QuantityMetrics.RowDegreeCentrality(_matrix21).Sum());
            Assert.AreEqual(1, QuantityMetrics.RowDegreeCentrality(_matrix2Identity).Sum());
        }

        [TestMethod]
        public void ColumnDegreeCentralityTest()
        {
            Assert.AreEqual(0, QuantityMetrics.ColumnDegreeCentrality(_matrix10).Sum());
            Assert.AreEqual(1, QuantityMetrics.ColumnDegreeCentrality(_matrix11).Sum());
            Assert.AreEqual(0, QuantityMetrics.ColumnDegreeCentrality(_matrix20).Sum());
            Assert.AreEqual(2, QuantityMetrics.ColumnDegreeCentrality(_matrix21).Sum());
            Assert.AreEqual(1, QuantityMetrics.ColumnDegreeCentrality(_matrix2Identity).Sum());
        }

        #endregion

        #region Load & density

        [TestMethod]
        public void LoadTest()
        {
            Assert.AreEqual(0, QuantityMetrics.Load(_matrix10));
            Assert.AreEqual(1, QuantityMetrics.Load(_matrix11));
            Assert.AreEqual(0, QuantityMetrics.Load(_matrix20));
            Assert.AreEqual(2, QuantityMetrics.Load(_matrix21));
            Assert.AreEqual(1, QuantityMetrics.Load(_matrix2Identity));
        }


        [TestMethod]
        public void ConnectivityTest()
        {
            Assert.AreEqual(0, QuantityMetrics.Density(_matrix10));
            Assert.AreEqual(1, QuantityMetrics.Density(_matrix11));
            Assert.AreEqual(0, QuantityMetrics.Density(_matrix20));
            Assert.AreEqual(1, QuantityMetrics.Density(_matrix21));
        }

        #endregion
    }
}