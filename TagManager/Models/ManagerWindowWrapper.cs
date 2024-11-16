using Prism.Mvvm;
using System;
using TagManager.Models.TypeClass.SearchInfo;
using TagManager.ViewModels;
using TagManager.Views;

namespace TagManager.Models
{
    //一つのタブマネージャー
    public class ManagerWindowWrapper : BindableBase
    {
        public ManagerWindowWrapper( ManagerWindow managerWindow, Guid viewId)
        {
            ManagerWindow = managerWindow;
            ViewId = viewId;

            //作ったManagerWindowViewModelのSearchDataが変わったら通知を受けるのを購読
            (ManagerWindow.DataContext as ManagerWindowViewModel).SearchDataChanged += (s, e) =>
            {
                if (e != null)
                {
                    ISearchInfo searchInfo = e.SearchInfo;
                    SearchInfo = searchInfo;
                }
            };
        }

        private ISearchInfo _searchInfo;
        public ISearchInfo SearchInfo
        {
            get { return _searchInfo; }
            set { SetProperty(ref _searchInfo, value); }
        }

        //カレントパス
        private string _currentPath;
        public string CurrentPath
        { 
            get { return _currentPath; } 
            set { SetProperty(ref _currentPath, value); } 
        }

        //ビュー実体
        private ManagerWindow _managerWindow;
        public ManagerWindow ManagerWindow
        {
            get { return _managerWindow; }
            set { _managerWindow = value; }
        }

        private Guid _viewId;
        public Guid ViewId
        {
            get { return _viewId; }
            set { _viewId = value; }
        }
    
    }
}
