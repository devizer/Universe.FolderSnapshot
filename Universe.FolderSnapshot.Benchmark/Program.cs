using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using Universe.FolderSnapshot.Tests;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Universe.FolderSnapshot.Benchmark
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<RestoreSnapshotBenchmarks>();
        }
    }

    [RankColumn]
    [MemoryDiagnoser]
    public class RestoreSnapshotBenchmarks
    {
        private string TestObjectFullPath, TestSnapshotFolder;

        private Lazy<string> _ZipFileUncompressedSnapshot;
        private Lazy<string> _ZipFileFastestSnapshot;
        NetZipFileSnapshotManager _ZipFileUncompressedSnapshotManager = new NetZipFileSnapshotManager(NetZipCompressionLevel.NoCompression);
        NetZipFileSnapshotManager _ZipFileFastestSnapshotManager = new NetZipFileSnapshotManager(NetZipCompressionLevel.Fastest);

        public RestoreSnapshotBenchmarks()
        {
            _ZipFileUncompressedSnapshot = new Lazy<string>(GetZipFileUncompressedSnapshot, LazyThreadSafetyMode.ExecutionAndPublication);
            _ZipFileFastestSnapshot = new Lazy<string>(GetZipFileFastestSnapshot, LazyThreadSafetyMode.ExecutionAndPublication);
        }


        [GlobalSetup]
        public void GlobalSetup()
        {
            TestObjectFullPath = TestEnv.TestObjectFullPath;
            TestSnapshotFolder = TestEnv.TestSnapshotFolder;
            var folder1 = _ZipFileUncompressedSnapshot.Value;
            var folder2 = _ZipFileFastestSnapshot.Value;
        }

        string GetZipFileUncompressedSnapshot()
        {
            var snapshot = Path.Combine(TestEnv.TestSnapshotFolder, $"Snapshot.Uncompressed.{Guid.NewGuid().ToString("N")}.zip");
            _ZipFileUncompressedSnapshotManager.CreateSnapshot(TestObjectFullPath, snapshot);
            return snapshot;
        }

        string GetZipFileFastestSnapshot()
        {
            var snapshot = Path.Combine(TestEnv.TestSnapshotFolder, $"Snapshot.Fastest.{Guid.NewGuid().ToString("N")}.zip");
            _ZipFileFastestSnapshotManager.CreateSnapshot(TestObjectFullPath, snapshot);
            return snapshot;
        }

        [Benchmark]
        public void ZipFileUncompressed()
        {
            var restoreTo = Path.Combine(TestEnv.TestSnapshotFolder, $"Restored.ZipFile.Uncompressed.{Guid.NewGuid().ToString("N")}");
            _ZipFileUncompressedSnapshotManager.RestoreSnapshot(_ZipFileUncompressedSnapshot.Value, restoreTo);
        }
        [Benchmark]
        public void ZipFileFastest()
        {
            var restoreTo = Path.Combine(TestEnv.TestSnapshotFolder, $"Restored.ZipFile.Fastest.{Guid.NewGuid().ToString("N")}");
            _ZipFileFastestSnapshotManager.RestoreSnapshot(_ZipFileFastestSnapshot.Value, restoreTo);
        }
    }
}
