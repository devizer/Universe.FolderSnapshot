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
            var bType = typeof(RestoreSnapshotBenchmarks);
            var dZip0 = new Descriptor(typeof(RestoreSnapshotBenchmarks), bType.GetMethod("ZipFileUncompressed"));
            var config = ManualConfig.CreateEmpty().AddJob(Job.Default).AddLogger(ConsoleLogger.Default).AddColumnProvider(EmptyColumnProvider.Instance);
            ImmutableConfig rc = config.CreateImmutableConfig();
            ParameterInstances pi = new ParameterInstances(Array.Empty<ParameterInstance>());
            var zip0 = BenchmarkCase.Create(dZip0, Job.Default, pi, rc);
            var zip0_b = new BenchmarkRunInfo(new BenchmarkCase[] { zip0 }, bType, rc);
            
            // var summary = BenchmarkRunner.Run(zip0_b);
            var summary = BenchmarkRunner.Run<RestoreSnapshotBenchmarks>();
            // BenchmarkRunner

        }
    }
}
