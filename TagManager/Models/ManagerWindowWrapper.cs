using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagManager.Views;

namespace TagManager.Models
{
    public class ManagerWindowWrapper
    {
        public ManagerWindowWrapper(string currentPath, ManagerWindow managerWindow) 
        {
            CurrentPath = currentPath;
            ManagerWindow = managerWindow;
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
    }
}
