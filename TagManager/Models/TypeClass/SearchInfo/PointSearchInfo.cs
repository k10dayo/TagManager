namespace TagManager.Models.TypeClass.SearchInfo;

public class PointSearchInfo : ISearchInfo
{
    public PointSearchInfo(string label, string fullLabel, string command, string thubnail)
    {
        Label = label;
        FullLabel = fullLabel;
        Command = command;
        Thumbnail = thubnail;
    }

    public string Label { get; }

    public string FullLabel { get; }

    public string Command { get; }

    public string Thumbnail { get; }
}
