using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagManager.Models.EverythinModels;
public class FileData
{
    public FileData(string name, string path, bool isFile, string thumbnailPath)
    {
        FileName = name;
        Path = path;
        FullPath = path;
        IsFile = isFile;
        ThumbnailPath = thumbnailPath;
    }  

    public string FileName { get; }
    public bool IsFile { get; }
    public string Path { get; }
    public string FullPath { get; }

    public string ThumbnailPath { get; }

}
