using System.Diagnostics;
using TagManager.Models.EverythinModels;

namespace TagManager.Models.TypeClass.SearchInfo;

public class BaseSearchInfo : ISearchInfo
{
    public BaseSearchInfo()
    {
        string basePath = UserSettingHandler.GetBasePath();
        LayerSearchInfo searchinfo = EverythingModel.CreateLayerSearchCommand(basePath);

        Label = searchinfo.Label;
        FullLabel = searchinfo.FullLabel;
        Command = searchinfo.Command;
        Thumbnail = searchinfo.Thumbnail;
    }

    public string Label { get; }

    public string FullLabel { get; }

    public string Command { get; }

    public string Thumbnail { get; }
}
