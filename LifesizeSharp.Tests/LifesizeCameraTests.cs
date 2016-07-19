using System;
using System.Configuration;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LifesizeSharp.Tests
{
    [TestClass]
    public class LifesizeCameraTests
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
        public void TestCameraGetConnectSuccess()
        {
            Assert.IsTrue(_lifesizeIconClient.Camera.GetConnected(Cameras.Hdmi0));
            Assert.IsFalse(_lifesizeIconClient.Camera.GetConnected(Cameras.Dvi0));
        }

        [TestMethod]
        public void TestCameraGetSupportedSuccess()
        {
            Assert.IsTrue(_lifesizeIconClient.Camera.GetSupported(Cameras.Hdmi0));
            Assert.IsFalse(_lifesizeIconClient.Camera.GetSupported(Cameras.Dvi3));
        }

        [TestMethod]
        public void TestCameraGetAntiFlickerSuccess()
        {
            var response = _lifesizeIconClient.Camera.GetAntiFlicker(Cameras.Hdmi0);
            Assert.IsTrue(response == AntiFlicker.Auto || response == AntiFlicker.SixtyHz || response == AntiFlicker.FiftyHz);
        }

        [TestMethod]
        public void TestCameraGetPositionSuccess()
        {
            var pos = _lifesizeIconClient.Camera.GetPosition(Cameras.Hdmi0);
            Assert.IsNotNull(pos);
            Assert.IsNotNull(pos.Pan);
            Assert.IsNotNull(pos.Tilt);
            Assert.IsNotNull(pos.DigitalZoom);
            Assert.IsNotNull(pos.Zoom);
        }

        [TestMethod]
        public void TestCameraGetPresetPositionSuccess()
        {
            // Tests
            var pos = _lifesizeIconClient.Camera.GetPresetPosition(1);
            Assert.IsNotNull(pos);
            Assert.IsNotNull(pos.Pan);
            Assert.IsNotNull(pos.DigitalZoom);
            Assert.IsNotNull(pos.Tilt);
            Assert.IsNotNull(pos.Zoom);
            Assert.IsNotNull(pos.Camera);
        }

        // According to API preset: 0 should work, in practice it errors
        [TestMethod]
        [ExpectedException(typeof(Exception), "Return Value: -1")]
        public void TestCameraGetPresetPositionHomeException()
        {
            _lifesizeIconClient.Camera.GetPresetPosition(0);
        }

        [TestMethod]
        public void TestCameraStopSuccess()
        {
            _lifesizeIconClient.Camera.Stop(Cameras.Hdmi0);
        }

        [TestMethod]
        public void TestCameraSetLockEnabledSuccess()
        {
            _lifesizeIconClient.Camera.SetLockEnabled(Cameras.Hdmi0, true);
            Task.Delay(1000);
            Assert.IsTrue(_lifesizeIconClient.Camera.GetLockEnabled(Cameras.Hdmi0));
            _lifesizeIconClient.Camera.SetLockEnabled(Cameras.Hdmi0, false);
            Task.Delay(1000);
            Assert.IsFalse(_lifesizeIconClient.Camera.GetLockEnabled(Cameras.Hdmi0));
        }

        [TestMethod]
        public void TestCameraRecallPresetSuccess()
        {
            _lifesizeIconClient.Camera.RecallPreset(1);
        }

        [TestMethod]
        public void TestCameraSetPositionSuccess()
        {
            _lifesizeIconClient.Camera.SetPosition(Cameras.Hdmi0, 0, 0, 0, 0);
        }

    }
}
