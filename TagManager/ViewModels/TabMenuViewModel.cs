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

            ViewSelected = new DelegateCommand<object[]>(ViewSelectedExecute);
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
        public DelegateCommand RemoveViewButton { get; }
        public void RemoveViewButtonExecute()
        {
            RemoveViewButtonHandler?.Invoke(this, new EventArgs());
        }
        public event EventHandler RemoveViewButtonHandler;


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
    }
}
