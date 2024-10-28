using System;
using System.IO;
using System.Text.Json;
using TagManager.Models.TypeClass;

public class UserSettingHandler
{
    public static ThumbnailData GetUserSetting()
    {
        string json = File.ReadAllText("Resources\\UserSetting.json");

        ThumbnailData? thumbnailData = JsonSerializer.Deserialize<ThumbnailData>(json);

        if (thumbnailData == null)
        {
            throw new UserSettingsException("UserSetting.jsonが読み込めませんでした。JSONが正しいか確認してください。");
        }

        return thumbnailData;
    }

    //サムネイルの入っているフォルダ名を取得する
    public static string GetThumbnailFolderName()
    {
        ThumbnailData thumbnailData = GetUserSetting();

        return thumbnailData.ThumbnailFolderName;
    }

    public static string GetFolderThumbnail()
    {
        ThumbnailData thumbnailData = GetUserSetting();

        return thumbnailData.FolderThumbnail;
    }



    //エラー用
    public class UserSettingsException : Exception
    {
        public UserSettingsException(string message) : base(message) { }
    }


    //NoImage copyright
    //<a href="https://www.flaticon.com/free-icons/no-photo" title="no photo icons">No photo icons created by yaicon - Flaticon</a>
}
