using EverythingNet.Core;
using EverythingNet.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagManager.Models.EverythinModels
{
    //シングルトンクラス
    public class EverythingModel
    {
        private readonly IEverything everything = new Everything();        

        //Everythinで検索する関数　SearchDataを返す
        public SearchData SearchExecute(string searchPath)
        {
            if (!EverythingState.IsStarted())
                EverythingState.StartService(true, EverythingState.StartMode.Service); 

            everything.Reset();

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            string completPath = "parent:" + searchPath;


            var queryable = everything
                            .Search()
                            .Name
                            .Contains(completPath);

            SearchData Results = new SearchData();

            foreach (var result in queryable)
            {
                var fileItem = new FileItem(result);
                fileItem.ThumbnailPath = CreateThumbnailPath(result);

                Debug.Print(fileItem.ThumbnailPath);

                Results.FileList.Add(fileItem);
                //Debug.Print(result.FullPath);
            }
            stopwatch.Stop();

            Results.CurrentPath = searchPath;
            Results.SearchTime = stopwatch.ElapsedMilliseconds;
            Results.FileCount = queryable.Count;
            

            Debug.Print(Results.SearchTime.ToString() + "ミリ秒");
            Debug.Print(Results.FileCount.ToString() + "件");

            return Results;

        }


        private string CreateThumbnailPath(ISearchResult searchResult)
        {
            if(searchResult.IsFile == true)
            {
                return PathProcessing.CreateFileThumbnailPath(searchResult.FullPath);
            }

            return PathProcessing.CreateFolderThumbnailPath(searchResult.FullPath);

        }

    }
}
