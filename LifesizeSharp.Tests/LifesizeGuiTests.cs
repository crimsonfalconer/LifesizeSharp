using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LifesizeSharp.Tests
{
    [TestClass]
    public class LifesizeGuiTests
    {
        private LifesizeIconClient _lifesizeIconClient;

        [TestInitialize]
        public void Init()
        {
            var user = ConfigurationManager.AppSettings["LifesizeUsername"];
            var pass = ConfigurationManager.AppSettings["LifesizePassword"];
            var url = ConfigurationManager.AppSettings["LifesizeUrl"];

            _lifesizeIconClient = new LifesizeIconClient(url);
            _lifesizeIconClient.Connect(user, pass);
            Assert.IsTrue(_lifesizeIconClient.Connected);
        }

        [TestMethod]
        public void TestGuiGetPresentationStateSuccess()
        {
            // Tests
            var r = _lifesizeIconClient.Gui.GetPresentationState();
            Assert.IsNotNull(r);
        }

        [TestMethod]
        public void TestGuiStartPresentationSuccess()
        {
            // Tests
            _lifesizeIconClient.Gui.StartPresentation();
        }

        [TestMethod]
        public void TestGuiStopPresentationSuccess()
        {
            // Tests
            _lifesizeIconClient.Gui.StopPresentation();
        }
    }
}
