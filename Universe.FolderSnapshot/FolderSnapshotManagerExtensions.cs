using System.Collections.Generic;

namespace Universe.FolderSnapshot;

public static class FolderSnapshotManagerExtensions
{
    public static List<IFolderSnapshotManager> GetListByPlatform()
    {
        var ret = new List<IFolderSnapshotManager>();
        if (TinyCrossInfo.IsWindows)
        {
            ret.Add(new WindowsSnapshotManager(NetZipCompressionLevel.NoCompression));
            ret.Add(new WindowsSnapshotManager(NetZipCompressionLevel.Fastest));
        }
        else
        {
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

        if (manager is WindowsSnapshotManager winMan)
        {
            return "ZipFile." + winMan.CompressionLevel;
        }

        return manager?.GetType().Name;
    }

}