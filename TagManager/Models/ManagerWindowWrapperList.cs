﻿using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using TagManager.Models.EverythinModels;
using TagManager.Models.TypeClass.SearchInfo;
using TagManager.ViewModels;
using TagManager.Views;

namespace TagManager.Models
{
    public class ManagerWindowWrapperList : BindableBase, INotifyPropertyChanged
    {
        CommonProperty _commonProperty;
        EverythingModel _everythingModel;
        private readonly IDialogService _dialogService;
        //コンストラクタ
        public ManagerWindowWrapperList(CommonProperty commonProperty, EverythingModel everythingModel, IDialogService dialogService)
        {
            _commonProperty = commonProperty;
            _everythingModel = everythingModel;
            if(dialogService == null)
            {
                Debug.Print("なぜぬるなん？？");
            }
            _dialogService = dialogService;
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
            var baseSearchInfo = new BaseSearchInfo();
            Debug.Print("ここから");
            var view = new TagManager.Views.ManagerWindow
            {                
                DataContext = new ManagerWindowViewModel(_commonProperty, _everythingModel, _dialogService, baseSearchInfo)
            };
            Debug.Print("ここまで");
            Guid uniqueId = Guid.NewGuid();

            
            var mww = new ManagerWindowWrapper(view, uniqueId);
            

            ViewCollection.Add(mww);
        }
        //新しいマネージャービューをリストに追加する関数
        public void AddManagerWindow(ISearchInfo searchInfo)
        {
            var view = new TagManager.Views.ManagerWindow
            {
                DataContext = new ManagerWindowViewModel(_commonProperty, _everythingModel, _dialogService, searchInfo)
            };

            Guid uniqueId = Guid.NewGuid();

            var mww = new ManagerWindowWrapper(view, uniqueId);

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
