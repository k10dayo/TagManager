using EverythingNet.Core;
using EverythingNet.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagManager.Models.TypeClass.SearchInfo;

namespace TagManager.Models.EverythinModels
{
    //シングルトンクラス
    public class EverythingModel
    {
        private readonly IEverything everything = new Everything();        

        //Everythinで検索する関数　SearchDataを返す
        public SearchData SearchExecute(ISearchInfo searchInfo)
        {
            EnsureServiceStarted();

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var queryable = everything
                            .Search()
                            .Name
                            .Contains(searchInfo.Command);

            SearchData results = new SearchData();

            foreach (var result in queryable)
            {
                var fileItem = new FileItem(result);
                fileItem.ThumbnailPath = PathProcessing.CreateThumbnailPath(result);

                Debug.Print(fileItem.ThumbnailPath);

                results.FileList.Add(fileItem);
                //Debug.Print(result.FullPath);
            }

            stopwatch.Stop();

            results.CurrentPath = searchInfo.FullLabel;

            results.SearchInfo = searchInfo;

            results.SearchTime = stopwatch.ElapsedMilliseconds;
            results.FileCount = queryable.Count;            

            Debug.Print(results.SearchTime.ToString() + "ミリ秒");
            Debug.Print(results.FileCount.ToString() + "件");

            return results;

        }        

        /// <summary>
        ///     レイヤー検索用にフルパスを加工する
        /// </summary>
        public static LayerSearchInfo CreateLayerSearchCommand(string searchPath)
        {
            string fileName = PathProcessing.GetFileName(searchPath);
            string layerSearchCommand =
                "\"" + UserSettingHandler.GetBasePath() + "\"" + " " +
                "parent:" + "\"" + searchPath + "\"";

            string Thumbnail = PathProcessing.CreateFolderThumbnailPathPublic(searchPath);

            return new LayerSearchInfo(fileName, searchPath, layerSearchCommand, Thumbnail);
        }

        /// <summary>
        ///     ポイント検索用にコマンドを加工する
        /// </summary>
        public static PointSearchInfo CreatePointSearchCommand(string searchCommand)
        {
            string label = "検索：" + searchCommand;
            string fullLabel = "検索：" + searchCommand;

            string pointSearchCommand =
                "\"" + UserSettingHandler.GetBasePath() + "\"" + " " +
                searchCommand ;

            return new PointSearchInfo(label, fullLabel, pointSearchCommand, "");
        }

        /// <summary>
        ///     検索の前準備、サービスがスタートしていることを保証する
        /// </summary>
        private void EnsureServiceStarted()
        {
            if (!EverythingState.IsStarted())
                EverythingState.StartService(true, EverythingState.StartMode.Service);

            everything.Reset();
        }

    }
}
