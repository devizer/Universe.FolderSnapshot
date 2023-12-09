namespace Universe.FolderSnapshot
{
    public interface IFolderSnapshotManager
    {
        void CreateSnapshot(string sourceFolder, string destinationFile);
        void RestoreSnapshot(string sourceFile, string destinationFolder);
        string Extension { get; }

    }
}