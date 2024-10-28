using EverythingNet.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagManager.Models.EverythinModels
{
    public class FileItem
    {
        
        public FileItem(ISearchResult searchResult)
        {
            SearchResult = searchResult;
        }

        public ISearchResult SearchResult { get; set; }


        //オリジナル
        public string ThumbnailPath { get; set; }

        public long Index { get { return SearchResult.Index; } }

        public bool IsFile { get { return SearchResult.IsFile; } }

        public string FullPath { get { return SearchResult.FullPath; } }

        public string Path { get { return SearchResult.Path; } }

        public string FileName { get { return SearchResult.FileName; } }

        public long Size { get { return SearchResult.Size; } }

        public uint Attributes { get { return SearchResult.Attributes; } }

        DateTime Created { get { return SearchResult.Created; } }

        DateTime Modified { get { return SearchResult.Modified; } }

        DateTime Accessed { get { return SearchResult.Accessed; } }

        DateTime Executed { get { return SearchResult.Executed; } }

        Exception? LastException { get { return SearchResult.LastException; } }
    }
}
