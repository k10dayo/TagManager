using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagManager.ViewModels;
using TagManager.Views;

namespace TagManager.Models
{
    public class ManagerWindowWrapperList : BindableBase, INotifyPropertyChanged
    {
        CommonProperty _commonProperty;
        public ManagerWindowWrapperList(CommonProperty commonProperty)
        {
            _commonProperty = commonProperty;
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

        //リストにビューを追加
        public void AddView(ManagerWindow managerWindow)
        {
            int count = ViewCollection.Count;

            var mww = new ManagerWindowWrapper("Folder" + count, managerWindow);

            ViewCollection.Add(mww);
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
            var m = new ManagerWindowViewModel(_commonProperty);
            var view = new TagManager.Views.ManagerWindow { DataContext = m };
            var mww = new ManagerWindowWrapper("Folder" + ViewCollection.Count, view);

            ViewCollection.Add(mww);
        }
    }
}
