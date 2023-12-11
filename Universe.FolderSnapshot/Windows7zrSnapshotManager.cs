namespace Universe.FolderSnapshot
{

    public class Windows7zrSnapshotManager : IFolderSnapshotManager
    {
        public readonly int CompressionLevel;

        public Windows7zrSnapshotManager(int compressionLevel)
        {
            CompressionLevel = compressionLevel;
        }

        public bool IsSupported
        {
            get
            {
                try
                {
                    var result = ExecProcessHelper.HiddenExec("7zr", "");
                    return result.ExitCode == 0;
                }
                catch
                {
                    return false;
                }
            }
        }


        public void CreateSnapshot(string sourceFolder, string destinationFile)
        {
            string args = $"a -mx={CompressionLevel} \"{destinationFile}\" \"{sourceFolder}\"";
            // Console.WriteLine($"{Compression.Title} command line: {Environment.NewLine}sh {args}");
            var result = ExecProcessHelper.HiddenExec("7zr", args);
            result.DemandGenericSuccess($"Create 7z archive '{destinationFile}' from '{sourceFolder}' folder using 7zr(.exe)");
        }

        public void RestoreSnapshot(string sourceFile, string destinationFolder)
        {
            string args = $"x -o\"{destinationFolder}\" \"{sourceFile}\"";
            var result = ExecProcessHelper.HiddenExec("7zr", args);
            result.DemandGenericSuccess($"Extract 7z archive '{sourceFile}' to '{destinationFolder}' folder using 7zr(.exe)");
        }

        public string Extension => $".Level{CompressionLevel}.7z";
    }
}