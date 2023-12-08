using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Universe.FolderSnapshot.Tests
{
    internal class TestEnv
    {
        private static Lazy<string> _TestObjectFullPath =
            new Lazy<string>(PrepareTestObjectFullPath, LazyThreadSafetyMode.ExecutionAndPublication);

        public static string TestObjectFullPath => _TestObjectFullPath.Value;

        private static string PrepareTestObjectFullPath()
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

            var slash = Path.DirectorySeparatorChar;
            if (string.IsNullOrEmpty(tempRoot))
                tempRoot = slash + "tmp";

            var ret = Path.Combine(tempRoot, "Snapshot tests original object");
            if (!Directory.Exists(ret)) Directory.CreateDirectory(ret);

            var extractArgs = $"x -y -o\"{ret}\" TestObject{slash}TestObject.7z";
            CrossInfo.HiddenExec("7z", extractArgs, out var output, out var exitCode);
            if (exitCode != 0)
            {
                throw new Exception($"Unable to extract 7z {extractArgs}");
            }

            return ret;
        }
    }
}
