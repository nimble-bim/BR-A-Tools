using System;
using System.Diagnostics;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace BRPLUSA.Database.Tests
{
    [TestFixture]
    public class LocalAPITests
    {
        private const string _something = "thing";
        private Process _cmd;

        [OneTimeSetUp]
        public async Task PrepLocalServer()
        {
            _cmd = StartCMD();
            _cmd.Start();

            //await _cmd.StandardInput.WriteLineAsync("heroku local web");
            _cmd.StandardInput.WriteLine("heroku local web");
            _cmd.StandardInput.Flush();
            //_cmd.Close();
            var content = _cmd.StandardOutput.ReadToEnd();

            Console.WriteLine(content);
        }

        [OneTimeTearDown]
        public void CloseDownLocalServer()
        {
            _cmd.Close();
            //_cmd.StandardInput.Flush();
            //_cmd.StandardInput.Close();
            //_cmd.WaitForExit();
        }

        public Process StartCMD()
        {
            var cmd = new Process
            {
                StartInfo =
                {
                    FileName = "cmd.exe",
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    UseShellExecute = false
                }
            };

            return cmd;
        }

        [Test]
        public void ShouldGetResponseFromWorksharingGETEndpoint()
        {
            Assert.Pass();
        }
    }
}
