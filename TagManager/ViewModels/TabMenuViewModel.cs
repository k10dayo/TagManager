using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Data;
using TagManager.Models;
using TagManager.Models.TypeClass.SearchInfo;
using TagManager.Views;
using static TagManager.ViewModels.ManagerWindowViewModel;

namespace TagManager.ViewModels
{
	public class TabMenuViewModel : BindableBase
	{
        ManagerWindowWrapperList _managerWindowWrapperList;
        public TabMenuViewModel(ManagerWindowWrapperList managerWindowWrapperList)
        {
            _managerWindowWrapperList = managerWindowWrapperList;
            //アドボタンのコマンド登録
            AddViewButton = new DelegateCommand(AddViewButtonExecute);
            DeleteTabButton = new DelegateCommand<object>(DeleteTabButtonExecute);

            ViewSelected = new DelegateCommand<object[]>(ViewSelectedExecute);


            DeleteTab = new DelegateCommand<object>(DeleteTabExecute);
            DeplicateTab = new DelegateCommand<object>(DeplicateTabExecute);

        }

        //タブリストをバインドしている変数
        public ObservableCollection<ManagerWindowWrapper> ViewCollection => _managerWindowWrapperList.ViewCollection;

        //アドボタン押した時のコマンド
        public DelegateCommand AddViewButton { get; }
        public void AddViewButtonExecute()
        {
            _managerWindowWrapperList.AddManagerWindow();
        }

        //リムーブボタン押したときのコマンド
        public DelegateCommand<object> DeleteTabButton { get; }
        public void DeleteTabButtonExecute(object parameter)
        {
            Debug.Print(parameter.ToString());
        }


        //タブリストを選択したときのコマンド
        public DelegateCommand<object[]> ViewSelected { get; }
        private void ViewSelectedExecute(object[] args)
        {
            if (args.Length > 0)//なんかこの条件ないと、タブを右クリックして削除するときに、右クリックした時に選択状態になり、消すとargs[0]がOutOfRangeでエラーになる？
            {
                var mww = args[0] as ManagerWindowWrapper;
                if (mww != null)
                {
                    Debug.Print(mww.CurrentPath);
                    _managerWindowWrapperList.ChangeSelectedManagerWindow(mww);
                }
            }
        }


        // コマンドの定義
        public DelegateCommand<object> DeleteTab { get; }
        // コマンドが実行されたときの処理
        private void DeleteTabExecute(object parameter)
        {
            if(parameter != null)
            {
                Guid viewId = (Guid)parameter;

                Debug.Print(parameter.ToString());
                _managerWindowWrapperList.RemoveManagerWindow(viewId);
            }            
        }

        public DelegateCommand<object> DeplicateTab { get; }
        private void DeplicateTabExecute(object parameter)
        {
            if (parameter != null)
            {
                ISearchInfo currentPath = (ISearchInfo)parameter;

                Debug.Print(parameter.ToString());
                _managerWindowWrapperList.AddManagerWindow(currentPath);
            }
        }

    }
}
