﻿using System;
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
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {

        }

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

        // [Test]
        [RequiredOs(Os.Linux, Os.Mac, Os.FreeBSD)]
        public void X1_CreateNixSnapshot()
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

        // [Test]
        [RequiredOs(Os.Linux, Os.Mac, Os.FreeBSD)]
        public void X2_RestoreNixSnapshot()
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

        [Test]
        [TestCase("First")]
        [TestCase("Next")]
        public void Y1_CreateSnapshot(string kind)
        {
            foreach (var manager in FolderSnapshotManagerExtensions.GetListByPlatform())
            {
                if (manager.IsSupported())
                {
                    var snapshotFullName = Path.Combine(TestEnv.TestSnapshotFolder, $"snapshot{manager.Extension}");
                    // if (File.Exists(snapshotFullName)) { File.Delete(snapshotFullName); }
                    snapshotFullName = UniqueFilesystemNames.GetLastNextNames(true, snapshotFullName, 5).Next;
                    Stopwatch sw = Stopwatch.StartNew();
                    manager.CreateSnapshot(TestEnv.TestObjectFullPath, snapshotFullName);
                    var elapsed = sw.ElapsedMilliseconds;
                    string humanLength = File.Exists(snapshotFullName) ? $"{new FileInfo(snapshotFullName).Length:n0} bytes" : "directory";
                    Console.WriteLine($"{manager.GetTitle()}: stored as {humanLength}, {elapsed:n0} msec");
                }
                else
                {
                    Console.WriteLine($"{manager.GetTitle()} IS NOT SUPPORTED");
                }
            }
        }

        [Test]
        [TestCase("First")]
        [TestCase("Next")]
        public void Y2_RestoreSnapshot(string kind)
        {
            foreach (var manager in FolderSnapshotManagerExtensions.GetListByPlatform())
            {
                if (manager.IsSupported())
                {
                    bool isFile = !(manager is XCopySnapshotManager);
                    var snapshotFullName2 = Path.Combine(TestEnv.TestSnapshotFolder, $"snapshot{manager.Extension}");
                    var snapshotFullName = UniqueFilesystemNames.GetLastNextNames(isFile, snapshotFullName2, 5).Last;
                    var restoreTo2 = Path.Combine(TestEnv.TestSnapshotFolder, $"Restored.{manager.GetTitle()}");
                    var restoreTo = UniqueFilesystemNames.GetLastNextNames(false, restoreTo2, 5).Next;
                    if (Directory.Exists(restoreTo)) TryAndForget(() => Directory.Delete(restoreTo, true)); 
                    Stopwatch sw = Stopwatch.StartNew();
                    manager.RestoreSnapshot(snapshotFullName, restoreTo);
                    var elapsed = sw.ElapsedMilliseconds;
                    Console.WriteLine($"{manager.GetTitle()}: restored {elapsed:n0} msec");
                }
                else
                {
                    Console.WriteLine($"{manager.GetTitle()} IS NOT SUPPORTED");
                }
            }
        }

        static void TryAndForget(Action action)
        {
            try
            {
                action();
            }
            catch {}
        }
    }
}
