using System;
using System.IO;
using System.IO.Compression;

namespace Universe.FolderSnapshot
{
    public class WindowsSnapshotManager : IFolderSnapshotManager
    {

        public readonly NetZipCompressionLevel CompressionLevel;

        public WindowsSnapshotManager(NetZipCompressionLevel compressionLevel)
        {
            CompressionLevel = compressionLevel;
        }


        public void CreateSnapshot(string sourceFolder, string destinationFile)
        {
#if !NET40
            ZipFile.CreateFromDirectory(sourceFolder, destinationFile, (CompressionLevel) (int) this.CompressionLevel, false);
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

        public string Extension => $".{this.CompressionLevel}.zip";
    }
}