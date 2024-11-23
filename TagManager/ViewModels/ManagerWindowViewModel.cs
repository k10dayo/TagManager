using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using TagManager.Models;
using TagManager.Models.EverythinModels;
using TagManager.Models.TypeClass.SearchInfo;
using TagManager.Views;

namespace TagManager.ViewModels
{
    public class ManagerWindowViewModel : BindableBase
    {
        CommonProperty _commonProperty;
        EverythingModel _everythingModel;
        private readonly IDialogService _dialogService;
        public ManagerWindowViewModel(CommonProperty commonProperty, EverythingModel everythingModel,IDialogService dialogService, ISearchInfo searchInfo = null)
        {
            _commonProperty = commonProperty;
            _everythingModel = everythingModel;
            _dialogService = dialogService;

            NaviButtonClick = new DelegateCommand(NaviButtonClickExecute);

            AccessPrevButton = new DelegateCommand(AccessPrevButtonExecute);
            AccessNextButton = new DelegateCommand(AccessNextButtonExecute);
            AccessParentButton =new DelegateCommand(AccessParentButtonExecute);

            SearchButton = new DelegateCommand(SearchButtonExecute);
            AdvancedSearchButton = new DelegateCommand(AdvancedSearchButtonExecute);

            PictureDubleClick = new DelegateCommand<object>(PictureDubleClickExecute);
            FolderDubleClick = new DelegateCommand<object>(FolderDubleClickExecute);

            SearchData = new();

            if (searchInfo != null)
            {
                UpdateHistory(searchInfo);
                return;
            }

            string searchPath = UserSettingHandler.GetBasePath();
            LayerSearch(searchPath);

        }

        //ファイルの横幅
        private int _fileWidth = 100;
        public int FileWidth
        {
            get { return _fileWidth; }
            set { SetProperty(ref _fileWidth, value); }
        }
        //ファイルの縦幅
        private int _fileMaxHeight = 100;
        public int FileMaxHeight
        {
            get { return _fileMaxHeight; }
            set { SetProperty(ref _fileMaxHeight, value); }
        }

        //カレントのファイルデータ
        private SearchData _searchData;
        public SearchData SearchData
        {
            get { return _searchData; }
            set 
            {
                //viewに通知
                SetProperty(ref _searchData, value);
                //タブメニューにカレントパスを通知
                OnSearchDataChanged(value.SearchInfo);
            }
        }
        //通知用
        public event EventHandler<SearchInfoChangedEventArgs> SearchDataChanged;
        protected virtual void OnSearchDataChanged(ISearchInfo propertyName)
        {
            SearchDataChanged?.Invoke(this, new SearchInfoChangedEventArgs(propertyName));
        }

        //PathHistoryの現在のインデックス
        private int HistoryIndex { get; set; } = -1;

        //pathの履歴
        private List<ISearchInfo> _pathHistory = new();
        private List<ISearchInfo> PathHistory
        {
            get { return _pathHistory; }
            set { _pathHistory = value; }
        }

        private string _searchTextBox = "";
        public string SearchTextBox
        {
            get { return _searchTextBox; }
            set { SetProperty(ref _searchTextBox, value); }
        }


        //ナビボタンをクリックしたときの処理
        public DelegateCommand NaviButtonClick { get; }
        private void NaviButtonClickExecute()
        {
            _commonProperty.toggleIsNaviOpen();
        }

        //一つ戻るボタン
        public DelegateCommand AccessPrevButton {  get; }
        private void AccessPrevButtonExecute()
        {
            if (HistoryIndex > 0)
            {
                PrevCurrentFolder();
            }            
        }
        //次へボタン
        public DelegateCommand AccessNextButton { get; }
        private void AccessNextButtonExecute()
        {
            Debug.Print(HistoryIndex.ToString());
            if (PathHistory.Count != HistoryIndex + 1)
            {
                Debug.Print(HistoryIndex.ToString());

                NextCurrentFolder();
            }
        }


        //階層を上に上がるボタン
        public DelegateCommand AccessParentButton { get; }
        private void AccessParentButtonExecute()
        {
            if(SearchData.SearchInfo is PointSearchInfo)
            {
                BaseSearch();
                return;
            }

            string parentPath = Directory.GetParent(SearchData.CurrentPath).FullName;
            LayerSearch(parentPath);
        }

        
        //詳細な検索ボタン
        public DelegateCommand AdvancedSearchButton { get; }
        private void AdvancedSearchButtonExecute()
        {
            var parameters = new DialogParameters
            {
                { "paramKey", "パラメーター"}
            };

            _dialogService.ShowDialog(nameof(AdvancedSearchDialog), parameters, r =>
            {
                if (r.Result == ButtonResult.OK)
                {
                    // OKボタンが押された場合
                    var result = r.Parameters.GetValue<string>("resultParam");
                    // 結果を処理
                    Debug.Print($"検索結果: {result}");
                }
                else if (r.Result == ButtonResult.Cancel)
                {
                    // キャンセルボタンが押された場合
                    Debug.Print("検索がキャンセルされました");
                }
            });
        }
        //検索ボタン
        public DelegateCommand SearchButton { get; }
        private void SearchButtonExecute()
        {
            var searchCommand = SearchTextBox;
            PointSearch(searchCommand);
        }

        //リストボックス内のピクチャーをダブルクリックした時の処理
        public DelegateCommand<object> PictureDubleClick { get; }
        private void PictureDubleClickExecute(object parameter)
        {
            string filePath = parameter as string;            

            if (filePath != null)
            {
                StartProcess(filePath);
            }
        }

        //リストボックス内のフォルダーをダブルクリックした時の処理
        public DelegateCommand<object> FolderDubleClick { get; }
        private void FolderDubleClickExecute(object parameter)
        {
            string folderPath = parameter as string;
            if (folderPath != null)
            {
                LayerSearch(folderPath);
            }            
        }

        //カレントフォルダーを移動する関数
        private async void ChangeCurrentFolder(ISearchInfo searchInfo)
        {
            SearchData = await Task.Run(() =>
            {
                return _everythingModel.SearchExecute(searchInfo);
            });

            //MessageBox.Show($"{SearchData.FileList.Count}");
        }

        //履歴を更新する処理関数
        private void UpdateHistory(ISearchInfo searchInfo)
        {
            HistoryIndex++;
            //HistoryIndexがPathHistoryの先頭にある場合はAddする　//HistoryuIndexは-1からCountは0から始まる
            if (PathHistory.Count == HistoryIndex)
            {
                PathHistory.Add(searchInfo);
            }
            //HistoryIndexがリストの先頭でないなら、次の要素を上書きする。
            else
            {
                PathHistory[HistoryIndex] = searchInfo;
            }            
            
            ChangeCurrentFolder(searchInfo);
        }
        //次に
        private void NextCurrentFolder()
        {
            HistoryIndex++;
            var nextPath = PathHistory[HistoryIndex];
            ChangeCurrentFolder(nextPath);
        }
        private void PrevCurrentFolder()
        {
            HistoryIndex--;
            var prevPath = PathHistory[HistoryIndex];
            ChangeCurrentFolder(prevPath);
        }

        //パスからアプリを開く(ファイルをクリックしたときの処理）
        private void StartProcess(string filePath)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = filePath,
                UseShellExecute = true
            });
        }

        /// <summary>
        ///     レイヤー検索を行う　※レイヤー検索　階層のある検索　フルパスでの検索
        /// </summary>
        private void LayerSearch(string searchPath)
        {            
            string basePath = UserSettingHandler.GetBasePath();
            if (!searchPath.Contains(basePath))
            {
                //BaseSearch();
                return;
            }

            LayerSearchInfo searchInfo = EverythingModel.CreateLayerSearchCommand(searchPath);
            UpdateHistory(searchInfo);
        }

        private void PointSearch(string searchCommand)
        {
            PointSearchInfo searchInfo = EverythingModel.CreatePointSearchCommand(searchCommand);
            UpdateHistory(searchInfo);
        }

        private void BaseSearch()
        {
            BaseSearchInfo searchInfo = new BaseSearchInfo();
            UpdateHistory(searchInfo);
        }
    }

    //通知用内部クラス
    public class SearchInfoChangedEventArgs : EventArgs
    {
        public ISearchInfo SearchInfo { get; }

        public SearchInfoChangedEventArgs(ISearchInfo searchInfo)
        {
            SearchInfo = searchInfo;
        }
    }
}
