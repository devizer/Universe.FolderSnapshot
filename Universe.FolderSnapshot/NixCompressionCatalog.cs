using System.Collections.Generic;

namespace Universe.FolderSnapshot;

public class NixCompressionCatalog
{
    public static List<CompressorDefinition> TarCompressors = new List<CompressorDefinition>()
    {
        new CompressorDefinition("none", null, null),
        new CompressorDefinition("lz4", "lz4 -1", "lz4 -d"),
        new CompressorDefinition("lzop", "lzop -1", "lzop -d"),
        new CompressorDefinition("pigz:gzip", "pigz -1", "pigz -d"),
        new CompressorDefinition("pigz:zstd", "pigz -1 -z", "pigz -d -z"),
        new CompressorDefinition("gzip", "gzip -1", "gzip -d"),
        new CompressorDefinition("zstd", "zstd -1", "zstd -d"),
        new CompressorDefinition("brotli", "brotli -1", "brotli -d"),
    };
}