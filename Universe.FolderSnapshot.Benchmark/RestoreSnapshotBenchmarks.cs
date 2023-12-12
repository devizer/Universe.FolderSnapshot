using BenchmarkDotNet.Attributes;
using System.Collections.Generic;
using System.IO;
using Universe.FolderSnapshot.Tests;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Universe.FolderSnapshot.Benchmark;

[RankColumn]
[MemoryDiagnoser]
public class RestoreSnapshotBenchmarks
{
    private List<string> _CleanFilesAndDirectories = new List<string>();

    private static Lazy<List<SnapshotBenchmarkCase>> _Cases = new Lazy<List<SnapshotBenchmarkCase>>(GetBenchmarkCases, LazyThreadSafetyMode.ExecutionAndPublication);

    public RestoreSnapshotBenchmarks()
    {
    }


    [Benchmark]
    [ArgumentsSource(nameof(Cases))]
    public void Restore(SnapshotBenchmarkCase manager)
    {
        var restoreTo = Path.Combine(TestEnv.TestSnapshotFolder, $"Restored.{Guid.NewGuid().ToString("N")}{manager.Manager.Extension}");
        _CleanFilesAndDirectories.Add(restoreTo);
        manager.Manager.RestoreSnapshot(manager.Snapshot, restoreTo);
    }

    public List<SnapshotBenchmarkCase> Cases => _Cases.Value;

    [GlobalCleanup]
    public void GlobalCleanup()
    {
        foreach (var path in _CleanFilesAndDirectories)
        {
            try
            {
                if (File.Exists(path)) File.Delete(path);
                if (Directory.Exists(path)) Directory.Delete(path, true);
            }
            catch
            {
            }
        }
    }

    static List<SnapshotBenchmarkCase> GetBenchmarkCases()
    {
        var supportedManagers =
            FolderSnapshotManagerExtensions.GetListByPlatform()
                .Where(x => x.IsSupported())
                .ToList();

        List<SnapshotBenchmarkCase> ret = new List<SnapshotBenchmarkCase>();
        foreach (var manager in supportedManagers)
        {
            var snapshotFullName = Path.Combine(TestEnv.TestSnapshotFolder, $"Snapshot{manager.Extension}");
            try
            {
                if (File.Exists(snapshotFullName)) File.Delete(snapshotFullName);
                if (Directory.Exists(snapshotFullName)) Directory.Delete(snapshotFullName, true);
            }
            catch
            {
            }
            manager.CreateSnapshot(TestEnv.TestObjectFullPath, snapshotFullName);
            ret.Add(new SnapshotBenchmarkCase()
            {
                Manager = manager,
                Snapshot = snapshotFullName
            });
        }

        var titles = ret.Select(x => x.Manager.GetTitle());
        Console.WriteLine($"// Total Supported Snapshot Managers {ret.Count}: {string.Join(", ", titles)}");

        return ret;
    }

    public class SnapshotBenchmarkCase
    {
        public IFolderSnapshotManager Manager;
        public string Snapshot;

        public override string ToString()
        {
            return Manager.GetTitle();
        }
    }
}