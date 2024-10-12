using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static TagManager.ViewModels.ManagerWindowViewModel;
using static TagManager.ViewModels.TabMenuViewModel;
using TagManager.Models;
using TagManager.Views;
using Prism.Navigation.Regions;

namespace TagManager.ViewModels
{
    public class BaseNaviViewModel : BindableBase
    {
        public BaseNaviViewModel
        (
            IRegionManager regionManager,
            ManagerWindowWrapperList managerWindowWrapperList,
            TabMenuViewModel tabMenuViewModel,
            CommonProperty commonProperty
        )
        {
            _regionManager = regionManager;
            //選択されたマネージャウィンドウの変更の通知を購読
            _managerWindowWrapperList = managerWindowWrapperList;
            _managerWindowWrapperList.SelectedManagerWindowChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(managerWindowWrapperList.SelectedManagerWindow))
                {
                    RaisePropertyChanged(nameof(SelectedManagerWindow));
                }
            };

            //コモンプロパティのイズナビオープンの値変更の通知を購読
            _commonProperty = commonProperty;
            _commonProperty.PropertyChanged += (s, e) => {
                if (e.PropertyName == nameof(commonProperty.IsNaviOpen))
                {
                    RaisePropertyChanged(nameof(IsNaviOpen));
                }
            };

            //ナビメニューを表示する
            _regionManager.RegisterViewWithRegion("NaviMenuRegion", nameof(TabMenu));
            //タブメニューのアドボタンのイベントを購読する
            _tabMenuViewModel = tabMenuViewModel;
            _tabMenuViewModel.RemoveViewButtonHandler -= RemoveViewButtonEvent;

            }
        //ナビゲーション
        private readonly IRegionManager _regionManager;

        private CommonProperty _commonProperty;
        //これがTrueだと、ナビが開くぞ！
        public bool IsNaviOpen => _commonProperty.IsNaviOpen;

        //タブメニューを受け取る
        private TabMenuViewModel _tabMenuViewModel;
        //タブのリストを受け取る
        private ManagerWindowWrapperList _managerWindowWrapperList;

        public void RemoveViewButtonEvent(object sender, EventArgs e)
        {
            //_managerWindowWrapperList.AddView(view);
        }

        public ManagerWindow SelectedManagerWindow => _managerWindowWrapperList.SelectedManagerWindow;

    }

}
