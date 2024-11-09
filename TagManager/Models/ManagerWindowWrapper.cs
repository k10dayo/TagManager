using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
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
                if (e.PropertyName != null)
                {
                    ChangeCurrentFolder(e.PropertyName);
                }
            };
        }

        //
        private string _currentFolderName;
        public string CurrentFolderName
        {
            get { return _currentFolderName; }
            set { SetProperty(ref _currentFolderName, value); }
        }

        //
        private string _currentFolderThumbnailPath;
        public string CurrentFolderThumbnailPath
        {
            get { return _currentFolderThumbnailPath; }
            set { SetProperty(ref _currentFolderThumbnailPath, value); }
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

        
        private void ChangeCurrentFolder(string currentPath)
        {
            CurrentPath = currentPath;
            CurrentFolderName = TagManager.Models.EverythinModels.PathProcessing.GetFileName(currentPath);
            CurrentFolderThumbnailPath = TagManager.Models.EverythinModels.PathProcessing.CreateFolderThumbnailPath(currentPath);
        }
    }
}
