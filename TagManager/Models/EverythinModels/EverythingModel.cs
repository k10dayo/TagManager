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
using System.ServiceProcess;
using EverythingNet.Core;
using EverythingNet.Interfaces;
using System.Windows;
using System.Xml.Linq;
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

            Debug.Print("検索コマンド："+ searchInfo.Command);
            SearchData result =  hello(searchInfo);

            return result;
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
        //検索クエリ設定
        [DllImport("Everything64.dll", CharSet = CharSet.Unicode)]
        public static extern int Everything_SetSearchW(string lpSearchString);
        //検索
        [DllImport("Everything64.dll")]
        public static extern void Everything_QueryW(bool bWait);
        //検索数
        [DllImport("Everything64.dll")]
        public static extern int Everything_GetNumResults();
        //絶対パス
        [DllImport("Everything64.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr Everything_GetResultFullPathNameW(int nIndex, StringBuilder lpString, int nMaxCount);
        //アトリビュート　フォルダかファイルか
        [DllImport("Everything64.dll")]
        public static extern uint Everything_GetResultAttributes(int nIndex);
        //データ取得リクエスト
        [DllImport("Everything64.dll")]
        public static extern void Everything_SetRequestFlags(uint dwRequestFlags);        
        // 作成日
        [DllImport("Everything64.dll")]
        public static extern bool Everything_GetResultDateCreated(int nIndex, out long lpFileTime);
        //更新日
        [DllImport("Everything64.dll")]
        public static extern bool Everything_GetResultDateModified(int nIndex, out long lpFileTime);
        //サイズ
        [DllImport("Everything64.dll")]
        public static extern bool Everything_GetResultSize(int nIndex, out long lpFileSize);


        // フラグの定数
        const uint EVERYTHING_REQUEST_FILE_NAME = 0x00000001;
        const uint EVERYTHING_REQUEST_PATH = 0x00000002;
        const uint EVERYTHING_REQUEST_SIZE = 0x00000010;
        const uint EVERYTHING_REQUEST_DATE_CREATED = 0x00000020;
        const uint EVERYTHING_REQUEST_DATE_MODIFIED = 0x00000040;
        const uint EVERYTHING_REQUEST_DATE_ACCESSED = 0x00000080;        
        const uint EVERYTHING_REQUEST_ATTRIBUTES = 0x00000100;

        // 属性定数
        // ファイル 32 128
        const uint EA_FILE = 0x000000A0;
        // フォルダ 16 48
        const uint EA_FOLDER = 0x00000010;


        public SearchData hello(ISearchInfo info)
        {
            // 属性を含む結果をリクエスト
            Everything_SetRequestFlags
                (
                    EVERYTHING_REQUEST_FILE_NAME |
                    EVERYTHING_REQUEST_PATH |
                    EVERYTHING_REQUEST_DATE_CREATED |
                    EVERYTHING_REQUEST_DATE_MODIFIED |
                    EVERYTHING_REQUEST_SIZE |
                    EVERYTHING_REQUEST_ATTRIBUTES
                );

            string searchQuery = info.Command;
            // 検索クエリを設定
            Everything_SetSearchW(searchQuery);

            // 検索を実行
            Everything_QueryW(true);

            // 結果の数を取得
            int numResults = Everything_GetNumResults();            
                      

            FileData[] fileDatas = new FileData[numResults];
            List<FileData> fileList = new List<FileData>();
            List<FileData> folderList = new List<FileData>();

            //デバック用
            //List<string> debugList = new List<string>();

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            // 結果を取得して表示
            for (int i = 0; i < numResults; i++)
            {
                StringBuilder resultPath = new StringBuilder(260);  // パスの最大長
                Everything_GetResultFullPathNameW(i, resultPath, resultPath.Capacity);

                // 取得した結果はUTF-16の文字列であるため、文字列をそのまま表示
                string result = resultPath.ToString();

                string path = result;
                string name = Path.GetFileName(result);
                // 作成日・更新日を取得
                DateTime createdDate = new DateTime(0001, 1, 1);
                DateTime modifiedDate = new DateTime(0001, 1, 1);

                //作成日
                if (Everything_GetResultDateCreated(i, out long createdFileTime))
                {
                    createdDate = DateTime.FromFileTime(createdFileTime);
                }
                //更新日
                if (Everything_GetResultDateModified(i, out long modifiedFileTime))
                {
                    modifiedDate = DateTime.FromFileTime(modifiedFileTime);
                }

                //サイズ
                long fileSize = -1;
                // サイズを取得
                if (Everything_GetResultSize(i, out long size))
                {
                    fileSize = size;
                }

                // アトリビュートからファイルかフォルダか判定
                uint attributes = Everything_GetResultAttributes(i);

                string type = "不明";
                bool isFile = true;
                string thumbnailPath = "";
                
                if ((attributes & EA_FOLDER) != 0)
                {
                    type = "フォルダ";
                    isFile = false;
                    thumbnailPath = PathProcessing.CreateFolderThumbnailPath(path);

                    folderList.Add(new FileData(name, result, isFile, thumbnailPath, fileSize, createdDate, modifiedDate));
                }
                else if ((attributes & EA_FILE) != 0)
                {
                    type = "ファイル";
                    thumbnailPath = PathProcessing.CreateFileThumbnailPath(path);

                    fileList.Add(new FileData(name, result, isFile, thumbnailPath, fileSize, createdDate, modifiedDate));
                }
                else
                {
                    //デバック用
                    //debugList.Add(attributes.ToString());
                }
            }            


            //デバック用
            //if (debugList.Count > 0)
            //{
            //    debugList.Distinct<string>();
            //    List<string> uniqueList = debugList.Distinct().ToList();

            //    // 改行で区切った文字列を作成
            //    string resultaa = string.Join(Environment.NewLine, uniqueList);

            //    // メッセージボックスに表示
            //    MessageBox.Show(resultaa, "Distinct Items");
            //}

            var folderListAsc = folderList.OrderBy(file => file.FileName, new NaturalStringComparer()).ToList();
            var fileListAsk = fileList.OrderBy(file => file.FileName, new NaturalStringComparer()).ToList();
            folderListAsc.AddRange(fileListAsk);

            stopwatch.Stop();

            SearchData results = new SearchData();
            foreach (FileData file in folderListAsc)
            {
                results.FileList.Add(file);
            }
            results.CurrentPath = info.FullLabel;
            results.SearchInfo = info;
            results.SearchTime = stopwatch.ElapsedMilliseconds;
            results.FileCount = numResults;

            Debug.Print(results.SearchTime.ToString() + "ミリ秒");
            Debug.Print(results.FileCount.ToString() + "件");

            return results;
        }

    }
}
