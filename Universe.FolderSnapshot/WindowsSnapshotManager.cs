using System;
using System.IO.Compression;

namespace Universe.FolderSnapshot;

public class WindowsSnapshotManager : IFolderSnapshotManager
{
    public void CreateSnapshot(string sourceFolder, string destinationFile)
    {
#if !NET40
        ZipFile.CreateFromDirectory(sourceFolder, destinationFile);
#else
            throw new NotSupportedException();
#endif
    }

    public void RestoreSnapshot(string sourceFile, string destinationFolder)
    {
#if !NET40
        ZipFile.ExtractToDirectory(sourceFile, destinationFolder);
#else
            throw new NotSupportedException();
#endif
    }

    public string Extension { get; } = ".zip";
}