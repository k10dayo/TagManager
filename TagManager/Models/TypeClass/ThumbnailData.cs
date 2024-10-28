using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagManager.Models.TypeClass;
public class ThumbnailData
{
    public ThumbnailData(string thumbnailFolderName, string folderThumbnail)
    {
        ThumbnailFolderName = thumbnailFolderName;

        FolderThumbnail = folderThumbnail;
    }
    public string ThumbnailFolderName { get; set; }

    public string FolderThumbnail { get; set; }

}
