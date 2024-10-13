using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Data;
using TagManager.Models;
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


            TestButton = new DelegateCommand<object>(TestButtonExecute);

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
            Debug.Print("えええええ");
            Debug.Print(parameter.ToString());
        }


        //タブリストを選択したときのコマンド
        public DelegateCommand<object[]> ViewSelected { get; }
        private void ViewSelectedExecute(object[] args)
        {
            var a = args[0] as ManagerWindowWrapper;
            if (a != null)
            {
                Debug.Print(a.CurrentPath);
                _managerWindowWrapperList.SelectedManagerWindow = a.ManagerWindow;
            }
            else
            {
                Debug.Print("ぬるやんけ");
            }
        }




        // コマンドの定義
        public DelegateCommand<object> TestButton { get; }

        // コマンドが実行されたときの処理
        private void TestButtonExecute(object parameter)
        {
            if (parameter != null)
            {
                Debug.Print(parameter.ToString());
            }
            else
            {
                Debug.Print("ぬる");
            }

            Debug.Print("わお"); // ここで"わお"を出力
        }

    }
}
