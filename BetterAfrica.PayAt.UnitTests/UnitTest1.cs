using System;
using System.IO;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BetterAfrica.PayAt.UnitTests
{
    [TestClass]
    public class PayAt_FTPTests
    {
        [TestMethod]
        public void CanConnectToFTPServer()
        {
            PayAtWorker.Upload(ftpPath: "test.txt", localPath: "test.txt");
            PayAtWorker.Download(ftpPath: "test.txt", localPath: "dummy.txt");

            var original = File.ReadAllText("test.txt");
            var returned = File.ReadAllText("dummy.txt");

            Assert.AreEqual(original, returned);
        }
    }
}