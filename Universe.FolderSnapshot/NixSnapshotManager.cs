namespace Universe.FolderSnapshot
{
    public class NixSnapshotManager : IFolderSnapshotManager
    {
        public readonly CompressorDefinition Compression = null;

        public NixSnapshotManager(CompressorDefinition compression)
        {
            Compression = compression;
        }

        public void CreateSnapshot(string sourceFolder, string destinationFile)
        {
            var pipe = string.IsNullOrEmpty(Compression.FastestCompressPipe) ? "" : Compression.FastestCompressPipe;
            string args = $"-c tar cf - \"sourceFolder\" {pipe} > \"{destinationFile}\"";
            throw new System.NotImplementedException();
        }

        public void RestoreSnapshot(string sourceFile, string destinationFolder)
        {
            var pipe = string.IsNullOrEmpty(Compression.FastestCompressPipe) ? "" : Compression.FastestCompressPipe;
            string args = $"-c cat \"{sourceFile}\" | tar xf - -C \"{destinationFolder}\"";
            throw new System.NotImplementedException();
        }
    }
}