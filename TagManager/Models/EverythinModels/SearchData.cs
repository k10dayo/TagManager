using EverythingNet.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagManager.Models.EverythinModels
{
    public class SearchData
    {
        public SearchData() 
        {

        }

        public string CurrentPath { get; set; } = "";
        public long FileCount { get; set; } = 0;
        public long SearchTime { get; set; } = 0;

        public ObservableCollection<ISearchResult> FileList { get; set; } = new ObservableCollection<ISearchResult>();
    }
}
