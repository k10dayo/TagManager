using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
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
        //コンストラクタ
        public ManagerWindowWrapperList(CommonProperty commonProperty, EverythingModel everythingModel)
        {
            _commonProperty = commonProperty;
            _everythingModel = everythingModel;
        }

        //選択中のListIndex
        private Guid _selectedViewId;
        public Guid SelectedViewId
        {
            get { return _selectedViewId; }
            set
            {
                _selectedViewId = value;
            }
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

        public HashSet<int> UniqueViewIds = new();

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

            Guid uniqueId = Guid.NewGuid();

            var mww = new ManagerWindowWrapper("Folder" + ViewCollection.Count, view, uniqueId);

            ViewCollection.Add(mww);
        }

        //引数のインデックスをもつマネージャービューを削除する
        public void RemoveManagerWindow(Guid viewId)
        {
            int index = _viewCollection.ToList().FindIndex(item => item.ViewId == viewId);
            Debug.Print("インデックス: " + index.ToString());

            if (viewId == SelectedViewId)
            {
                if (index != 0)
                {
                    ChangeSelectedManagerWindow(ViewCollection[index - 1]);
                }
                else if (ViewCollection.Count == 1)
                {
                    AddManagerWindow();
                    ChangeSelectedManagerWindow(ViewCollection[1]);
                }
                else
                {
                    ChangeSelectedManagerWindow(ViewCollection[index + 1]);
                }
            }


            ViewCollection.RemoveAt(index);

        }

        //選択中のマネージャービューを変更するとき、選択中のインデックスも変更する
        public void ChangeSelectedManagerWindow(ManagerWindowWrapper mww)
        {
            SelectedViewId = mww.ViewId;
            SelectedManagerWindow = mww.ManagerWindow;
        }

       
        
    }
}
