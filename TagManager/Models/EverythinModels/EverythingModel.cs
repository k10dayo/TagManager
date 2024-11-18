using EverythingNet.Core;
using EverythingNet.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
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

            Debug.Print("ああああああああああああ");
            IEnumerable<FileData> fileDatas =  hello(searchInfo);

            //var queryable = everything.Search().Name.Contains("\"C:\\テストフォルダー\\file_manager_directory\\名前テスト\\te st♡\"");


            SearchData results = new SearchData();

            //foreach (var result in queryable)
            //{
            //    Debug.Print(result.Path);

            //    var fileItem = new FileItem(result);
            //    fileItem.ThumbnailPath = PathProcessing.CreateThumbnailPath(result);

            //    //Debug.Print(fileItem.ThumbnailPath);

            //    results.FileList.Add(fileItem);
            //    Debug.Print(result.FullPath);
            //}

            stopwatch.Stop();

            results.CurrentPath = searchInfo.FullLabel;

            results.SearchInfo = searchInfo;

            results.SearchTime = stopwatch.ElapsedMilliseconds;
            //results.FileCount = queryable.Count;            

            Debug.Print(results.SearchTime.ToString() + "ミリ秒");
            Debug.Print(results.FileCount.ToString() + "件");


            foreach(var a in fileDatas)
            {
                results.FileList.Add(a);
            }

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









        // Everything SDK の Unicode バージョンの関数をインポート
        [DllImport("Everything64.dll", CharSet = CharSet.Unicode)]
        public static extern int Everything_SetSearchW(string lpSearchString);

        [DllImport("Everything64.dll")]
        public static extern void Everything_QueryW(bool bWait);

        [DllImport("Everything64.dll")]
        public static extern int Everything_GetNumResults();

        [DllImport("Everything64.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr Everything_GetResultFullPathNameW(int nIndex, StringBuilder lpString, int nMaxCount);

        [DllImport("Everything64.dll")]
        public static extern uint Everything_GetResultAttributes(int nIndex);

        [DllImport("Everything64.dll")]
        public static extern void Everything_SetRequestFlags(uint dwRequestFlags);
        // フラグの定数
        const uint EVERYTHING_REQUEST_FILE_NAME = 0x00000001;
        const uint EVERYTHING_REQUEST_PATH = 0x00000002;
        const uint EVERYTHING_REQUEST_SIZE = 0x00000010;
        const uint EVERYTHING_REQUEST_DATE_MODIFIED = 0x00000020;
        const uint EVERYTHING_REQUEST_ATTRIBUTES = 0x00000100;


        // 属性定数
        const uint EA_FILE = 0x00000020; // ファイル
        const uint EA_FOLDER = 0x00000010; // フォルダ


        public IEnumerable<FileData> hello(ISearchInfo info)
        {
            // 検索する日本語の文字列を設定
            //string searchQuery = "\"C:\\テストフォルダー\\file_manager_directory\\名前テスト\\te st♡\" ha";
            string searchQuery = info.Command;

            // 属性を含む結果をリクエスト
            Everything_SetRequestFlags(EVERYTHING_REQUEST_FILE_NAME | EVERYTHING_REQUEST_PATH | EVERYTHING_REQUEST_ATTRIBUTES);


            // 検索クエリを設定
            Everything_SetSearchW(searchQuery);

            // 検索を実行
            Everything_QueryW(true);

            // 結果の数を取得
            int numResults = Everything_GetNumResults();

            FileData[] fileDatas = new FileData[numResults];

            // 結果を取得して表示
            for (int i = 0; i < numResults; i++)
            {
                StringBuilder resultPath = new StringBuilder(260);  // パスの最大長
                Everything_GetResultFullPathNameW(i, resultPath, resultPath.Capacity);

                // 取得した結果はUTF-16の文字列であるため、文字列をそのまま表示
                string result = resultPath.ToString();

                string path = result;
                string name = Path.GetFileName(result);


                // 属性を取得してファイルかフォルダか判定
                uint attributes = Everything_GetResultAttributes(i);

                string type = "不明";
                bool isFile = true;
                string thumbnailPath = "";
                if ((attributes & EA_FOLDER) != 0)
                {
                    type = "フォルダ";
                    isFile = false;
                    thumbnailPath = PathProcessing.CreateFolderThumbnailPath(path);                    
                }
                else if ((attributes & EA_FILE) != 0)
                {
                    type = "ファイル";
                    thumbnailPath = PathProcessing.CreateFileThumbnailPath(path);
                }

                

                fileDatas[i] = new FileData(name, result, isFile, thumbnailPath);

                Debug.Print(isFile + ":" + fileDatas[i].FileName);
            }

            return fileDatas;
        }






    }
}
