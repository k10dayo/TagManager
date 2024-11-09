using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagManager.Models.EverythinModels
{
    public static class PathProcessing
    {

        public static string GetFileName(string fullPath)
        {
            Debug.Print(Path.GetFileName(fullPath));
            return Path.GetFileName(fullPath);
        }

        public static string CreateFolderThumbnailPath(string fullPath)
        {
            return
                fullPath + "\\" +
                UserSettingHandler.GetThumbnailFolderName() + "\\" +
                UserSettingHandler.GetFolderThumbnail();
        }

        public static string CreateFileThumbnailPath(string fullPath)
        {
            return
                    Path.GetDirectoryName(fullPath) + "\\" +
                    UserSettingHandler.GetThumbnailFolderName() + "\\" +
                    Path.GetFileName(fullPath) + ".jpg";
        }
    }
}
