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
            Upload(ftpPath: "test.txt", localPath: "test.txt");
            Download(ftpPath: "test.txt", localPath: "dummy.txt");

            var original = File.ReadAllText("test.txt");
            var returned = File.ReadAllText("dummy.txt");

            Assert.AreEqual(original, returned);
        }

        private void Upload(string ftpPath, string localPath)
        {
            // with support for SSL
            var request = (FtpWebRequest)WebRequest.Create("ftp://196.34.95.218/" + ftpPath);
            request.Credentials = new NetworkCredential("betterAfrica", "r4_g28#hDQ");
            request.Method = WebRequestMethods.Ftp.UploadFile;

            using (Stream fileStream = File.OpenRead(localPath))
            using (Stream ftpStream = request.GetRequestStream())
            {
                fileStream.CopyTo(ftpStream);
            }
        }

        private void Download(string ftpPath, string localPath)
        {
            // with support for SSL
            var request = (FtpWebRequest)WebRequest.Create("ftp://196.34.95.218/" + ftpPath);
            request.Credentials = new NetworkCredential("betterAfrica", "r4_g28#hDQ");
            request.Method = WebRequestMethods.Ftp.DownloadFile;

            using (Stream ftpStream = request.GetResponse().GetResponseStream())
            using (Stream fileStream = File.Create(localPath))
            {
                ftpStream.CopyTo(fileStream);
            }
        }
    }
}