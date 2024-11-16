using EverythingNet.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagManager.Models.TypeClass.SearchInfo;

namespace TagManager.Models.EverythinModels
{
    public class SearchData
    {
        public SearchData() 
        {

        }

        private string _currentPath = "";
        public string CurrentPath {
            get { return _currentPath; }
            set {
                    _currentPath = value;
                    OnCurrentPathChanged(value);
            } 
        }

        private ISearchInfo _searchInfo;
        public ISearchInfo SearchInfo
        {
            get { return _searchInfo; }
            set { _searchInfo = value;}
        }

        public string ThumbnailPath { get; set; } = "";
        public long FileCount { get; set; } = 0;
        public long SearchTime { get; set; } = 0;

        public ObservableCollection<FileItem> FileList { get; set; } = new ObservableCollection<FileItem>();


        public event PropertyChangedEventHandler CurrentPathChanged;
        protected virtual void OnCurrentPathChanged(string currentPaht)
        {
            CurrentPathChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(currentPaht)));
        }
    }
}
