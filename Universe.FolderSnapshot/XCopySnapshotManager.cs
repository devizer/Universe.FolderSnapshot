using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universe.FolderSnapshot
{
    public class XCopySnapshotManager : IFolderSnapshotManager
    {
        public bool Buffered { get; }
        public XCopySnapshotManager(bool buffered)
        {
            Buffered = buffered;
        }

        public void CreateSnapshot(string sourceFolder, string destinationFolder)
        {
            XCopy(sourceFolder, destinationFolder);
        }

        public void RestoreSnapshot(string sourceFolder, string destinationFolder)
        {
            XCopy(sourceFolder, destinationFolder);
        }

        public void XCopy(string sourceFolder, string destinationFolder)
        {
            string argsBuffered = Buffered ? "" : "/J ";
            string args = $"\"{sourceFolder}\"  \"{destinationFolder}\" {argsBuffered}/E /H /Q /I /Y";
            Console.WriteLine($"XCopy Args{Environment.NewLine}{args}");
            var result = ExecProcessHelper.HiddenExec("xcopy", args);
            result.DemandGenericSuccess($"Copy failed. Source '{sourceFolder}'. Destination '{destinationFolder}'");
        }

        // TODO: Should be empty
        public string Extension { get; } = ".Folder";
    }
}
