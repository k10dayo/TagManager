using Prism.Mvvm;
using System;
using TagManager.Models;
using TagManager.Views;
using Prism.Regions;
using Prism.Commands;
using System.Windows;
using Prism.Services.Dialogs;

namespace TagManager.ViewModels
{
    public class BaseNaviViewModel : BindableBase
    {
        public BaseNaviViewModel
        (
            IRegionManager regionManager,
            ManagerWindowWrapperList managerWindowWrapperList,
            CommonProperty commonProperty,
            IDialogService dialogService
        )
        {
            _regionManager = regionManager;
            _dialogService = dialogService;
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

            //ボタン
            SettingButtonClick = new DelegateCommand(SettingButtonClickExecute);




        }
        //ナビゲーション
        private readonly IRegionManager _regionManager;

        //ダイアログ
        private readonly IDialogService _dialogService;

        private CommonProperty _commonProperty;
        //これがTrueだと、ナビが開くぞ！
        public bool IsNaviOpen => _commonProperty.IsNaviOpen;
        //ManagerWindowWrapperListを受け取る(シングルトン)
        private ManagerWindowWrapperList _managerWindowWrapperList;

        //表示するマネージャービュー
        public ManagerWindow SelectedManagerWindow => _managerWindowWrapperList.SelectedManagerWindow;


        //設定ボタンをクリックしたときの処理
        public DelegateCommand SettingButtonClick { get; }
        public void SettingButtonClickExecute()
        {

            var parameters = new DialogParameters
            {
                { "paramKey", "パラメーター"}
            };


            _dialogService.ShowDialog(nameof(SettingDialog), parameters, r =>
            {
                if (r.Result == ButtonResult.OK)
                {
                    // 以下のresultでは、"SampleDialogViewModel"のOKコマンドで追加した
                    // "結果のパラメーター"という文字列が取得できます。
                    var result = r.Parameters.GetValue<string>("resultParam");
                }
            });

            

        }
    }

}
