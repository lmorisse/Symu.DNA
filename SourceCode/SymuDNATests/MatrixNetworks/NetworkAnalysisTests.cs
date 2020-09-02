using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Complex;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.DNA.MatrixNetworks;



namespace SymuDNATests.MatrixNetworks
{
    [TestClass]
    public class NetworkAnalysisTests
    {
        private readonly Matrix<float> _matrix10 = Matrix<float>.Build.Dense(1, 1);
        private readonly Matrix<float> _matrix20 = Matrix<float>.Build.Dense(2, 2);
        private readonly Matrix<float> _matrix11 = Matrix<float>.Build.Dense(1, 1, 1F);
        private readonly Matrix<float> _matrix21 = Matrix<float>.Build.Dense(2, 2, 1F);

        [TestMethod]
        public void GrandSumTest()
        {
            Assert.AreEqual(0,NetworkAnalysis.GrandSum(_matrix10));
            Assert.AreEqual(1, NetworkAnalysis.GrandSum(_matrix11));
            Assert.AreEqual(0, NetworkAnalysis.GrandSum(_matrix20));
            Assert.AreEqual(4, NetworkAnalysis.GrandSum(_matrix21));
        }


        [TestMethod]
        public void ConnectivityTest()
        {
            Assert.AreEqual(0, NetworkAnalysis.Connectivity(_matrix10));
            Assert.AreEqual(1, NetworkAnalysis.Connectivity(_matrix11));
            Assert.AreEqual(0, NetworkAnalysis.Connectivity(_matrix20));
            Assert.AreEqual(1, NetworkAnalysis.Connectivity(_matrix21));
        }
    }
}