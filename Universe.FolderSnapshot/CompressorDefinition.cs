namespace Universe.FolderSnapshot;

public class CompressorDefinition
{
    public string Title { get; }
    public string FastestCompressPipe { get; }
    public string DecompressPipe { get; }

    public CompressorDefinition(string title, string fastestCompressPipe, string decompressPipe)
    {
        Title = title;
        FastestCompressPipe = fastestCompressPipe;
        DecompressPipe = decompressPipe;
    }

    public override string ToString()
    {
        return $"{nameof(Title)}: {Title}, {nameof(FastestCompressPipe)}: {FastestCompressPipe}, {nameof(DecompressPipe)}: {DecompressPipe}";
    }
}