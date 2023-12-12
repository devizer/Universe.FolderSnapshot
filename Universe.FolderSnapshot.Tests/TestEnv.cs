using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Universe.FolderSnapshot.Tests
{
    public class TestEnv
    {
        private static Lazy<string> _TestObjectFullPath = new Lazy<string>(PrepareTestObjectFullPath, LazyThreadSafetyMode.ExecutionAndPublication);

        private static Lazy<string> _TestSnapshotFolder = new Lazy<string>(GetSnapshotFolder, LazyThreadSafetyMode.ExecutionAndPublication);

        public static string TestObjectFullPath => _TestObjectFullPath.Value;
        public static string TestSnapshotFolder => _TestSnapshotFolder.Value;

        private static string PrepareTestObjectFullPath()
        {
            var tempRoot = GetTempRoot();

            var slash = Path.DirectorySeparatorChar;
            var ret = Path.Combine(tempRoot, "Snapshot tests original object");
            if (!Directory.Exists(ret)) Directory.CreateDirectory(ret);

            var testObjectRelativePath = $"TestObject{slash}TestObject.7z";
            Console.WriteLine($"Current Directory: '{Environment.CurrentDirectory}'. Is {testObjectRelativePath} exists: {File.Exists(testObjectRelativePath)}");
            var extractArgs = $"x -y -o\"{ret}\" {testObjectRelativePath}";
            CrossInfo.HiddenExec("7z", extractArgs, out var output, out var exitCode);
            if (exitCode != 0)
            {
                throw new Exception($"Unable to extract 7z {extractArgs}");
            }

            return ret;
        }

        private static string GetTempRoot()
        {
            string tempRoot;
            if (CrossInfo.ThePlatform == CrossInfo.Platform.Windows)
            {
                tempRoot = Environment.GetEnvironmentVariable("TEMP");
            }
            else
            {
                tempRoot = Environment.GetEnvironmentVariable("TMPDIR");
            }

            if (string.IsNullOrEmpty(tempRoot))
                tempRoot = Path.DirectorySeparatorChar + "tmp";
            
            return tempRoot;
        }

        private static string GetSnapshotFolder()
        {
            var ret = Path.Combine(GetTempRoot(), "Temp Snapshot tests");
            if (!Directory.Exists(ret)) Directory.CreateDirectory(ret);
            return ret;
        }


    }
}
