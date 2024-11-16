namespace TagManager.Models.TypeClass.SearchInfo;

public class LayerSearchInfo : ISearchInfo
{
    public LayerSearchInfo(string label, string fullLabel, string command, string thumbnail)
    {
        Label = label;
        FullLabel = fullLabel;
        Command = command;
        Thumbnail = thumbnail;
    }

    public string Label { get;}

    public string FullLabel { get;}

    public string Command { get;}

    public string Thumbnail { get;}
}
