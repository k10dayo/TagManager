using Prism.Commands;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Documents;
using TagManager.Models;
using TagManager.Models.EverythinModels;

namespace TagManager.ViewModels
{
    public class ManagerWindowViewModel : BindableBase
    {
        CommonProperty _commonProperty;
        EverythingModel _everythingModel;
        public ManagerWindowViewModel(CommonProperty commonProperty, EverythingModel everythingModel)
        {
            _commonProperty = commonProperty;
            _everythingModel = everythingModel;
            NaviButtonClick = new DelegateCommand(NaviButtonClickExecute);

            AccessPrevButton = new DelegateCommand(AccessPrevButtonExecute);
            AccessNextButton = new DelegateCommand(AccessNextButtonExecute);
            AccessParentButton =new DelegateCommand(AccessParentButtonExecute);

            PictureDubleClick = new DelegateCommand<object>(PictureDubleClickExecute);
            FolderDubleClick = new DelegateCommand<object>(FolderDubleClickExecute);

            SearchData = new();

            UpdateHistory("C:\\テストフォルダー\\file_manager_directory");
            //UpdateHistory("D:");
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
                SetProperty(ref _searchData, value);
                //カレントパスを通知
                OnSearchDataChanged(value.CurrentPath);
            }
        }
        //通知用
        public event PropertyChangedEventHandler SearchDataChanged;
        protected virtual void OnSearchDataChanged(string propertyName)
        {
            SearchDataChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //PathHistoryの現在のインデックス
        private int HistoryIndex { get; set; } = -1;
        //pathの履歴
        private List<string> _pathHistory = new();
        public List<string> PathHistory
        {
            get { return _pathHistory; }
            set { _pathHistory = value; }
        }


        //ナビボタンをクリックしたときの処理
        public DelegateCommand NaviButtonClick { get; }
        public void NaviButtonClickExecute()
        {
            _commonProperty.toggleIsNaviOpen();
        }

        //一つ戻るボタン
        public DelegateCommand AccessPrevButton {  get; }
        public void AccessPrevButtonExecute()
        {
            if (HistoryIndex > 0)
            {
                PrevCurrentFolder();
            }            
        }
        //一つ戻るボタン
        public DelegateCommand AccessNextButton { get; }
        public void AccessNextButtonExecute()
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
        public void AccessParentButtonExecute()
        {
            string parentPath = Directory.GetParent(SearchData.CurrentPath).FullName;
            Debug.Print(parentPath);
            UpdateHistory(parentPath);
        }

        //リストボックス内のピクチャーをダブルクリックした時の処理
        public DelegateCommand<object> PictureDubleClick { get; }
        public void PictureDubleClickExecute(object parameter)
        {
            string filePath = parameter as string;            

            if (filePath != null)
            {
                StartProcess(filePath);
            }
        }

        //リストボックス内のフォルダーをダブルクリックした時の処理
        public DelegateCommand<object> FolderDubleClick { get; }
        public void FolderDubleClickExecute(object parameter)
        {
            string folderPath = parameter as string;
            if (folderPath != null)
            {
                UpdateHistory(folderPath);
            }            
        }

        //カレントフォルダーを移動する関数
        public async void ChangeCurrentFolder(string searchPath)
        {
            SearchData = await Task.Run(() =>
            {
                return _everythingModel.SearchExecute(searchPath);
            });
        }

        //履歴を更新する処理関数
        public void UpdateHistory(string searchPath)
        {
            HistoryIndex++;
            //HistoryIndexがPathHistoryuの先頭にある場合はAddする　//HistoryuIndexは-1からCountは0から始まる
            if (PathHistory.Count == HistoryIndex)
            {
                PathHistory.Add(searchPath);
            }
            //HistoryIndexがリストの先頭でないなら、次の要素を上書きする。
            else
            {
                PathHistory[HistoryIndex] = searchPath;
            }            
            
            ChangeCurrentFolder(searchPath);
        }
        //次に
        public void NextCurrentFolder()
        {
            HistoryIndex++;
            var nextPath = PathHistory[HistoryIndex];
            ChangeCurrentFolder(nextPath);
        }
        public void PrevCurrentFolder()
        {
            HistoryIndex--;
            var prevPath = PathHistory[HistoryIndex];
            ChangeCurrentFolder(prevPath);
        }

        //パスからアプリを開く
        public void StartProcess(string filePath)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = filePath,
                UseShellExecute = true
            });
        }
    }
}
