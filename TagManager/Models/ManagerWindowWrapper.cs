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
        public ManagerWindowWrapper(string currentPath, ManagerWindow managerWindow, int listIndex)
        {
            CurrentPath = currentPath;
            ManagerWindow = managerWindow;
            ListIndex = listIndex;
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

        private int _listIndex;
        public int ListIndex
        {
            get { return _listIndex; }
            set { _listIndex = value; }
        }
    }
}
