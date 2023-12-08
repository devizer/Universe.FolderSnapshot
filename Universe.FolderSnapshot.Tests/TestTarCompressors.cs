using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Universe.NUnitTests;

namespace Universe.FolderSnapshot.Tests
{
    [TestFixture]
    public class TestTarCompressors : NUnitTestsBase
    {
        [Test]
        public void Show_Tar_Compressors()
        {
            foreach (var compressorDefinition in NixCompressionCatalog.TarCompressors)
            {
                Console.WriteLine(compressorDefinition);
            }
        }

        [Test]
        public void Show_Test_Object()
        {
            Console.WriteLine(TestEnv.TestObjectFullPath);
        }

        [Test]
        [RequiredOs(Os.Linux, Os.Mac, Os.FreeBSD)]
        public void X_CreateSnapshot()
        {
            foreach (var compressorDefinition in NixCompressionCatalog.TarCompressors)
            {
                NixSnapshotManager man = new NixSnapshotManager(compressorDefinition);
                var snapshotFullName = Path.Combine(TestEnv.TestSnapshotFolder, $"snapshot.{compressorDefinition.Title}");
                Stopwatch sw = Stopwatch.StartNew();
                man.CreateSnapshot(TestEnv.TestObjectFullPath, snapshotFullName);
                Console.WriteLine($"{compressorDefinition.Title}: {new FileInfo(snapshotFullName).Length:n0} bytes, {sw.ElapsedMilliseconds:n0} msec");
            }
        }


    }
}
