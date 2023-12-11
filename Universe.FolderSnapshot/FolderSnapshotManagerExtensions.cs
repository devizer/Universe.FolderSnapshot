using System.Collections.Generic;

namespace Universe.FolderSnapshot;

public static class FolderSnapshotManagerExtensions
{
    public static List<IFolderSnapshotManager> GetListByPlatform()
    {
        var ret = new List<IFolderSnapshotManager>();


        if (TinyCrossInfo.IsWindows)
        {
            ret.Add(new NetZipFileSnapshotManager(NetZipCompressionLevel.NoCompression));
            ret.Add(new NetZipFileSnapshotManager(NetZipCompressionLevel.Fastest));
            ret.Add(new XCopySnapshotManager(true));
            ret.Add(new XCopySnapshotManager(false));
        }
        else
        {
            if (NetZipFileSnapshotManager.IsSupported)
            {
                ret.Add(new NetZipFileSnapshotManager(NetZipCompressionLevel.NoCompression));
                ret.Add(new NetZipFileSnapshotManager(NetZipCompressionLevel.Fastest));
            }

            foreach (var compressorDefinition in NixCompressionCatalog.TarCompressors)
            {
                NixSnapshotManager man = new NixSnapshotManager(compressorDefinition);
                ret.Add(man);
            }
        }

        ret.Add(new Windows7zrSnapshotManager(0));
        ret.Add(new Windows7zrSnapshotManager(1));

        return ret;
    }

    public static bool IsSupported(this IFolderSnapshotManager manager)
    {
        if (manager is NixSnapshotManager nixMan)
            return nixMan.IsCompressionSupported;

        if (manager is Windows7zrSnapshotManager win7z)
            return win7z.IsSupported;

        if (manager is NetZipFileSnapshotManager netZip)
            return NetZipFileSnapshotManager.IsSupported;

        return true;
    }

    public static string GetTitle(this IFolderSnapshotManager manager)
    {
        NixSnapshotManager nixMan = manager as NixSnapshotManager;
        if (nixMan != null)
            return nixMan.Compression.Title;

        if (manager is NetZipFileSnapshotManager winMan)
        {
            return "ZipFile." + winMan.CompressionLevel;
        }

        if (manager is XCopySnapshotManager xcopyMan)
        {
            return $"xcopy.{(xcopyMan.Buffered ? "buffered" : "pass-through")}";
        }

        if (manager is Windows7zrSnapshotManager win7z)
        {
            return $"7z.{win7z.CompressionLevel}";
        }

        return manager?.GetType().Name;
    }

}