namespace Universe.FolderSnapshot
{
    public interface IFolderSnapshotManager
    {
        void CreateSnapshot(string sourceFolder, string destinationFile);
        void RestoreSnapshot(string sourceFile, string destinationFolder);
        // Including dot
        string Extension { get; }

    }
}