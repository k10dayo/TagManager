using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagManager.Views;

namespace TagManager.Models
{
    //一つのタブマネージャー
    public class ManagerWindowWrapper
    {
        public ManagerWindowWrapper(string currentPath, ManagerWindow managerWindow, Guid viewId)
        {
            CurrentPath = currentPath;
            ManagerWindow = managerWindow;
            ViewId = viewId;
        }

        //カレントパス
        private string _currentPath;
        public string CurrentPath
        { 
            get { return _currentPath; } 
            set { _currentPath = value; } 
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
