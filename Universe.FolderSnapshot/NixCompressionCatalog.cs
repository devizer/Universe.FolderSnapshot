using System.Collections.Generic;

namespace Universe.FolderSnapshot;

public class NixCompressionCatalog
{
    public static List<CompressorDefinition> TarCompressors = new List<CompressorDefinition>()
    {
        new CompressorDefinition("none", ".tar", null, null),
        new CompressorDefinition("lz4", ".lz4", "lz4 -1", "lz4 -d"),
        new CompressorDefinition("lzop", ".lzop", "lzop -1", "lzop -d"),
        new CompressorDefinition("pigz:gzip", ".parallel.gz", "pigz -1", "pigz -d"),
        new CompressorDefinition("pigz:zstd", ".parallel.zz","pigz -1 -z", "pigz -d -z"),
        new CompressorDefinition("gzip", ".parallel.gz","gzip -1", "gzip -d"),
        new CompressorDefinition("zstd", ".zz","zstd -1", "zstd -d"),
        new CompressorDefinition("brotli", ".br","brotli -1", "brotli -d"),
    };
}