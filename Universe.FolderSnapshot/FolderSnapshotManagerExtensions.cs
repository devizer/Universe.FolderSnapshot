using System.Collections.Generic;

namespace Universe.FolderSnapshot;

public static class FolderSnapshotManagerExtensions
{
    public static List<IFolderSnapshotManager> GetListByPlatform()
    {
        var ret = new List<IFolderSnapshotManager>();
        if (TinyCrossInfo.IsWindows)
        {
            ret.Add(new ZipFileSnapshotManager(NetZipCompressionLevel.NoCompression));
            ret.Add(new ZipFileSnapshotManager(NetZipCompressionLevel.Fastest));
            ret.Add(new XCopySnapshotManager(true));
            ret.Add(new XCopySnapshotManager(false));
        }
        else
        {
            if (ZipFileSnapshotManager.IsSupported)
            {
                ret.Add(new ZipFileSnapshotManager(NetZipCompressionLevel.NoCompression));
                ret.Add(new ZipFileSnapshotManager(NetZipCompressionLevel.Fastest));
            }

            foreach (var compressorDefinition in NixCompressionCatalog.TarCompressors)
            {
                NixSnapshotManager man = new NixSnapshotManager(compressorDefinition);
                ret.Add(man);
            }
        }

        return ret;
    }

    public static bool IsSupported(this IFolderSnapshotManager manager)
    {
        NixSnapshotManager nixMan = manager as NixSnapshotManager;
        if (nixMan != null)
            return nixMan.IsCompressionSupported;

        return true;
    }

    public static string GetTitle(this IFolderSnapshotManager manager)
    {
        NixSnapshotManager nixMan = manager as NixSnapshotManager;
        if (nixMan != null)
            return nixMan.Compression.Title;

        if (manager is ZipFileSnapshotManager winMan)
        {
            return "ZipFile." + winMan.CompressionLevel;
        }

        if (manager is XCopySnapshotManager xcopyMan)
        {
            return $"xcopy.{(xcopyMan.Buffered ? "buffered" : "pass-through")}";
        }


        return manager?.GetType().Name;
    }

}