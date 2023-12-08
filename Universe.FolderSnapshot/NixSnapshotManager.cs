using System;
using System.Diagnostics;
using System.IO;

namespace Universe.FolderSnapshot
{
    public class NixSnapshotManager : IFolderSnapshotManager
    {
        public readonly CompressorDefinition Compression = null;

        public NixSnapshotManager(CompressorDefinition compression)
        {
            Compression = compression;
        }

        public bool IsCompressionSupported
        {
            get
            {
                var pipe = string.IsNullOrEmpty(Compression.FastestCompressPipe) ? "" : $"| {Compression.FastestCompressPipe}";
                var args = $"-e -c \"x=43; tar --help 1>/dev/null 2>&1; echo 42 {pipe} > /dev/null\"";
                var result = ExecProcessHelper.HiddenExec("sh", args);
                return result.ExitCode == 0;
            }
        }
    

        public void CreateSnapshot(string sourceFolder, string destinationFile)
        {
            var pipe = string.IsNullOrEmpty(Compression.FastestCompressPipe) ? "" : $"| {Compression.FastestCompressPipe}";
            string args = $"-e -c \"tar cf - '{sourceFolder}' {pipe} > '{destinationFile}'\"";
            // Console.WriteLine($"{Compression.Title} command line: {Environment.NewLine}sh {args}");
            var result = ExecProcessHelper.HiddenExec("sh", args);
            result.DemandGenericSuccess("Create snapshot failed");
        }

        public void RestoreSnapshot(string sourceFile, string destinationFolder)
        {
            var pipe = string.IsNullOrEmpty(Compression.FastestCompressPipe) ? "" : $"| {Compression.DecompressPipe}";
            if (!Directory.Exists(destinationFolder)) Directory.CreateDirectory(destinationFolder);
            string args = $"-e -c \"cat '{sourceFile}' {pipe}| tar xf - -C '{destinationFolder}'\"";
            var result = ExecProcessHelper.HiddenExec("sh", args);
            result.DemandGenericSuccess("Snapshot restore failed");
        }
    }
}