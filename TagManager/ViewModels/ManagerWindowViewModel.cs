using Prism.Commands;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

            FolderDubleClick = new DelegateCommand<object>(FolderDubleClickExecute);

            NewCurrentFolder("C:\\Users\\katotakehiro\\Pictures\\ファンブック");
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

        //現在のデータ
        private SearchData _searchData = new();
        public SearchData SearchData
        {
            get { return _searchData; }
            set 
            { 
                SetProperty(ref _searchData, value);
            }
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
            Debug.Print("いええええええ");
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
            NewCurrentFolder(parentPath);
        }

        //リストボックス内のフォルダーをダブルクリックした時の処理
        public DelegateCommand<object> FolderDubleClick { get; }
        public void FolderDubleClickExecute(object parameter)
        {
            string folderPath = parameter as string;
            if (folderPath != null)
            {
                NewCurrentFolder(folderPath);
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

        public void NewCurrentFolder(string searchPath)
        {
            HistoryIndex++;
            if (PathHistory.Count == HistoryIndex)//HistoryIndexがPathHistoryuの先頭にある場合はAddする　//HistoryuIndexは-1からCountは0から始まる
            {
                PathHistory.Add(searchPath);
            }
            else//HistoryIndexがリストの先頭でないなら、次の要素を上書きする。
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
    }
}
