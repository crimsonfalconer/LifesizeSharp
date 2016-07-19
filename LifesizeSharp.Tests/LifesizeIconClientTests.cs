using System;
using System.Configuration;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LifesizeSharp.Tests
{
    [TestClass]
    public class LifesizeIconClientTests
    {
        [TestMethod]
        public void TestConnectSuccess()
        {
            var user = ConfigurationManager.AppSettings["LifesizeUsername"];
            var pass = ConfigurationManager.AppSettings["LifesizePassword"];
            var url = ConfigurationManager.AppSettings["LifesizeUrl"];

            var lifesizeIconClient = new LifesizeIconClient(url);
            lifesizeIconClient.Connect(user, pass);
            Assert.IsTrue(lifesizeIconClient.Connected);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Unauthorized - Unauthorized")]
        public void TestConnectInvalidUser()
        {
            var user = "a";
            var pass = ConfigurationManager.AppSettings["LifesizePassword"];
            var url = ConfigurationManager.AppSettings["LifesizeUrl"];

            var lifesizeIconClient = new LifesizeIconClient(url);
            lifesizeIconClient.Connect(user, pass);
        }


        [TestMethod]
        [ExpectedException(typeof(Exception), "Unauthorized - Unauthorized")]
        public void TestConnectInvalidPass()
        {
            var user = ConfigurationManager.AppSettings["LifesizeUsername"];
            var url = ConfigurationManager.AppSettings["LifesizeUrl"];
            var pass = "x";

            var lifesizeIconClient = new LifesizeIconClient(url);
            lifesizeIconClient.Connect(user, pass);
        }

        [TestMethod]
        [ExpectedException(typeof(WebException))]
        public void TestConnectInvalidUrl()
        {
            var url = "http://8.8.8.8";
            var user = ConfigurationManager.AppSettings["LifesizeUsername"];
            var pass = ConfigurationManager.AppSettings["LifesizePassword"];

            var lifesizeIconClient = new LifesizeIconClient(url);
            lifesizeIconClient.Connect(user, pass);
        }
    }
}
