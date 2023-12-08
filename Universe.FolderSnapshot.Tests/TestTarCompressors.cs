using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Universe.NUnitTests;

namespace Universe.FolderSnapshot.Tests
{
    [TestFixture]
    public class TestTarCompressors : NUnitTestsBase
    {
        [Test]
        public void Show_Tar_Compressors()
        {
            foreach (var compressorDefinition in NixCompressionCatalog.TarCompressors)
            {
                Console.WriteLine(compressorDefinition);
            }
        }

        [Test]
        public void Show_Test_Object()
        {
            Console.WriteLine(TestEnv.TestObjectFullPath);
        }
    }
}
