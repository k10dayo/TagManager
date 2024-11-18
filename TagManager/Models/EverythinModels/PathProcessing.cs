using EverythingNet.Interfaces;
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
        /// <summary>
        ///     フルパスからファイル名だけを取得する
        /// </summary>
        public static string GetFileName(string fullPath)
        {
            Debug.Print(Path.GetFileName(fullPath));
            return Path.GetFileName(fullPath);
        }


        /// <summary>
        ///     引数のオブジェクトのフルパスからサムネイルパスを作成する
        /// </summary>
        public static string CreateThumbnailPath(ISearchResult searchResult)
        {
            if (searchResult.IsFile == true)
            {
                return CreateFileThumbnailPath(searchResult.FullPath);
            }

            return CreateFolderThumbnailPath(searchResult.FullPath);
        }

        /// <summary>
        ///     引数のフォルダのフルパスからサムネイルパスを作成する　公開用
        /// </summary>
        public static string CreateFolderThumbnailPathPublic(string fullPath)
        {
            return CreateFolderThumbnailPath(fullPath);
        }


        /// <summary>
        ///     引数のフォルダーのサムネイルを作成するパス
        /// </summary>
        public static string CreateFolderThumbnailPath(string fullPath)
        {
            return
                fullPath + "\\" +
                UserSettingHandler.GetThumbnailFolderName() + "\\" +
                UserSettingHandler.GetFolderThumbnail();
        }

        /// <summary>
        ///     引数のファイルのサムネイルパスを作成する
        /// </summary>
        public static string CreateFileThumbnailPath(string fullPath)
        {
            return
                    Path.GetDirectoryName(fullPath) + "\\" +
                    UserSettingHandler.GetThumbnailFolderName() + "\\" +
                    Path.GetFileName(fullPath) + ".jpg";
        }
    }
}
