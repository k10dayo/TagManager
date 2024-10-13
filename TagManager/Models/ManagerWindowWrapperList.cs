using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagManager.Models.EverythinModels;
using TagManager.ViewModels;
using TagManager.Views;

namespace TagManager.Models
{
    public class ManagerWindowWrapperList : BindableBase, INotifyPropertyChanged
    {
        CommonProperty _commonProperty;
        EverythingModel _everythingModel;
        public ManagerWindowWrapperList(CommonProperty commonProperty, EverythingModel everythingModel)
        {
            _commonProperty = commonProperty;
            _everythingModel = everythingModel;
        }

        //選択中のマネージャーウィンドウ
        private ManagerWindow _selectedManagerWindow;
        public ManagerWindow SelectedManagerWindow
        {
            get { return _selectedManagerWindow; }
            set
            {
                SetProperty(ref _selectedManagerWindow, value);
                OnSelectedManagerWindowChanged(nameof(SelectedManagerWindow));
            }
        }

        //マネージャービューのリスト（コレクション）
        public ObservableCollection<ManagerWindowWrapper> _viewCollection = new ObservableCollection<ManagerWindowWrapper>();
        public ObservableCollection<ManagerWindowWrapper> ViewCollection
        {
            get { return _viewCollection; }
            set
            {
                SetProperty(ref _viewCollection, value); 
                OnPropertyChanged(nameof(ViewCollection));
            }
        }


        //リストが変更されると通知を飛ばす
        public new event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //選択中のビューが変更されると通知を飛ばす
        public event PropertyChangedEventHandler SelectedManagerWindowChanged;
        protected virtual void OnSelectedManagerWindowChanged(string propertyName)
        {
            SelectedManagerWindowChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //新しいマネージャービューをリストに追加する関数
        public void AddManagerWindow()
        {
            var view = new TagManager.Views.ManagerWindow 
            {
                DataContext = new ManagerWindowViewModel(_commonProperty, _everythingModel)
            };

            var mww = new ManagerWindowWrapper("Folder" + ViewCollection.Count, view, ViewCollection.Count);

            ViewCollection.Add(mww);
        }
        
    }
}
