using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universe.FolderSnapshot.Tests
{
    internal class UniqueFilesystemNames
    {
        public class Names
        {
            public string Last, Next;
        }
        public static Names GetLastNextNames(bool isFile, string fullName, int indexLength)
        {
            var dir = new DirectoryInfo(Path.GetDirectoryName(fullName));
            var fileOnly = Path.GetFileName(fullName);
            var namesQuery =
                isFile
                    ? dir.GetFiles("*" + fileOnly).Select(x => x.Name)
                    : dir.GetDirectories("*" + fileOnly).Select(x => x.Name);

            var names = namesQuery.ToArray().OrderBy(x => x).ToArray();
            string lastRaw = names.LastOrDefault(x => TryGetIndex(x) != null);
            int? lastIndex = TryGetIndex(lastRaw);
            int prev = lastIndex.GetValueOrDefault();
            var next = prev + 1;
            var nextName = Path.Combine(dir.FullName, next.ToString(new string('0', indexLength)) + " " + fileOnly);
            return new Names
            {
                Last = lastRaw == null ? null : Path.Combine(dir.FullName, lastRaw), 
                Next = nextName
            };
        }

        static int? TryGetIndex(string name)
        {
            var rawIndex = name?.Split(' ').FirstOrDefault();
            if (rawIndex != null)
                if (Int32.TryParse(rawIndex, out var ret))
                    return ret;

            return null;
        }
    }
}
