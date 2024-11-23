using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagManager.Models.EverythinModels;
public class FileData
{
    public FileData(string name, string path, bool isFile, string thumbnailPath, long size, DateTime createDate, DateTime modifiedDate)
    {
        FileName = name;
        Path = path;
        FullPath = path;
        IsFile = isFile;
        ThumbnailPath = thumbnailPath;
        Size = size;
        CreatedDate = createDate;
        ModifiedDate = modifiedDate;

        FormatSize = FormatFileSize(Size);
    }

    public string FileName { get; }
    public bool IsFile { get; }
    public string Path { get; }
    public string FullPath { get; }
    public string ThumbnailPath { get; }
    public long Size { get; }
    public string FormatSize { get; }

    public DateTime CreatedDate { get; }
    public DateTime ModifiedDate { get; }

    private string FormatFileSize(long fileSize)
    {
        if (fileSize < 1024L)
            return $"{fileSize} B"; // バイト            
        else if (fileSize < 1024L * 1024L)
            return $"{fileSize / 1024.0:F2} KB"; // キロバイト
        else if (fileSize < 1024L * 1024L * 1024L)
            return $"{fileSize / (1024.0 * 1024.0):F2} MB"; // メガバイト
        else
            return $"{fileSize / (1024.0 * 1024.0 * 1024.0):F2} GB"; // ギガバイト
    }
}
