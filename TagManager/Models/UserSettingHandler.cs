using System;
using System.IO;
using System.Text.Json;
using TagManager.Models.TypeClass;

public class UserSettingHandler
{
    public static UserSettingData GetUserSetting()
    {
        string json = File.ReadAllText("Resources\\UserSetting.json");

        UserSettingData? userSettingData = JsonSerializer.Deserialize<UserSettingData>(json);

        if (userSettingData == null)
        {
            throw new UserSettingsException("UserSetting.jsonが読み込めませんでした。JSONが正しいか確認してください。");
        }

        return userSettingData;
    }

    public static string GetBasePath()
    {
        UserSettingData userSettingData = GetUserSetting();

        return userSettingData.BasePath;
    }

    public static void SetBasePath(string basePath)
    {
        UserSettingData userSettingData = GetUserSetting();

        // BasePathの値を新しい値で更新
        userSettingData.BasePath = basePath;
        // 更新された設定をJSON形式にシリアライズ
        string updatedJson = JsonSerializer.Serialize(userSettingData, new JsonSerializerOptions { WriteIndented = true });
        // JSONファイルに書き戻す
        File.WriteAllText("Resources\\UserSetting.json", updatedJson);
    }

    //サムネイルの入っているフォルダ名を取得する
    public static string GetThumbnailFolderName()
    {
        UserSettingData userSettingData = GetUserSetting();

        return userSettingData.ThumbnailFolderName;
    }

    public static string GetFolderThumbnail()
    {
        UserSettingData userSettingData = GetUserSetting();

        return userSettingData.FolderThumbnail;
    }



    //エラー用
    public class UserSettingsException : Exception
    {
        public UserSettingsException(string message) : base(message) { }
    }


    //NoImage copyright
    //<a href="https://www.flaticon.com/free-icons/no-photo" title="no photo icons">No photo icons created by yaicon - Flaticon</a>
}
