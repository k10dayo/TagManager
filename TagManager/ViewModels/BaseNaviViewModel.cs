using Prism.Mvvm;
using System;
using TagManager.Models;
using TagManager.Views;
using Prism.Regions;

namespace TagManager.ViewModels
{
    public class BaseNaviViewModel : BindableBase
    {
        public BaseNaviViewModel
        (
            IRegionManager regionManager,
            ManagerWindowWrapperList managerWindowWrapperList,
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


        }
        //ナビゲーション
        private readonly IRegionManager _regionManager;

        private CommonProperty _commonProperty;
        //これがTrueだと、ナビが開くぞ！
        public bool IsNaviOpen => _commonProperty.IsNaviOpen;
        //ManagerWindowWrapperListを受け取る(シングルトン)
        private ManagerWindowWrapperList _managerWindowWrapperList;

        //表示するマネージャービュー
        public ManagerWindow SelectedManagerWindow => _managerWindowWrapperList.SelectedManagerWindow;



        

    }

}
