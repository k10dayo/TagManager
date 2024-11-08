using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagManager.Models.TypeClass;
public class UserSettingData
{
    public UserSettingData(string basePath, string thumbnailFolderName, string folderThumbnail)
    {
        BasePath = basePath;

        ThumbnailFolderName = thumbnailFolderName;

        FolderThumbnail = folderThumbnail;
    }

    public string BasePath { get; set; }
    public string ThumbnailFolderName { get; set; }

    public string FolderThumbnail { get; set; }

}
