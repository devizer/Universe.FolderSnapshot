using System.Collections.Immutable;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Parameters;
using BenchmarkDotNet.Running;
using System.Linq;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Loggers;


namespace Universe.FolderSnapshot.Benchmark
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<RestoreSnapshotBenchmarks>();
        }
    }
}
