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
        public void X1_CreateSnapshot()
        {
            foreach (var compressorDefinition in NixCompressionCatalog.TarCompressors)
            {
                NixSnapshotManager man = new NixSnapshotManager(compressorDefinition);
                if (man.IsCompressionSupported)
                {
                    var snapshotFullName = Path.Combine(TestEnv.TestSnapshotFolder, $"snapshot.{compressorDefinition.Title}");
                    Stopwatch sw = Stopwatch.StartNew();
                    man.CreateSnapshot(TestEnv.TestObjectFullPath, snapshotFullName);
                    var elapsed = sw.ElapsedMilliseconds;
                    Console.WriteLine($"{compressorDefinition.Title}: stored as {new FileInfo(snapshotFullName).Length:n0} bytes, {elapsed:n0} msec");
                }
                else
                {
                    Console.WriteLine($"{compressorDefinition.Title} IS NOT SUPPORTED");
                }
            }
        }

        [Test]
        [RequiredOs(Os.Linux, Os.Mac, Os.FreeBSD)]
        public void X2_RestoreSnapshot()
        {
            foreach (var compressorDefinition in NixCompressionCatalog.TarCompressors)
            {
                NixSnapshotManager man = new NixSnapshotManager(compressorDefinition);
                if (man.IsCompressionSupported)
                {
                    var snapshotFullName = Path.Combine(TestEnv.TestSnapshotFolder, $"snapshot.{compressorDefinition.Title}");
                    var restoreTo = Path.Combine(TestEnv.TestSnapshotFolder, $"Restored.{compressorDefinition.Title}");
                    Stopwatch sw = Stopwatch.StartNew();
                    man.RestoreSnapshot(snapshotFullName, restoreTo);
                    var elapsed = sw.ElapsedMilliseconds;
                    Console.WriteLine($"{compressorDefinition.Title}: restored {elapsed:n0} msec");
                }
                else
                {
                    Console.WriteLine($"{compressorDefinition.Title} IS NOT SUPPORTED");
                }
            }
        }
    }
}
