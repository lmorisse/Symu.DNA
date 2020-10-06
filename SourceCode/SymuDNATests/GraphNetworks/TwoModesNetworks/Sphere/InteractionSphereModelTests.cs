using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.DNA.GraphNetworks.TwoModesNetworks.Sphere;

namespace SymuDNATests.GraphNetworks.TwoModesNetworks.Sphere
{
    [TestClass()]
    public class InteractionSphereModelTests
    {
        private readonly InteractionSphereModel _model = new InteractionSphereModel();
        [TestMethod()]
        public void CopyToTest()
        {
            _model.SphereUpdateOverTime = true;
            _model.MaxSphereDensity = 0;
            _model.MinSphereDensity = 0;
            _model.RandomlyGeneratedSphere = true;
            _model.RelativeActivityWeight = 0;
            _model.RelativeBeliefWeight = 0;
            _model.RelativeKnowledgeWeight = 0;
            _model.SocialDemographicWeight = 0;
            var clone = new InteractionSphereModel();
            _model.CopyTo(clone);
            Assert.AreEqual(0, clone.MaxSphereDensity);
            Assert.AreEqual(0, clone.MinSphereDensity);
            Assert.AreEqual(0, clone.RelativeActivityWeight);
            Assert.AreEqual(0, clone.RelativeBeliefWeight);
            Assert.AreEqual(0, clone.SocialDemographicWeight);
            Assert.IsTrue(clone.SphereUpdateOverTime);
            Assert.IsTrue(clone.RandomlyGeneratedSphere);
        }
    }
}