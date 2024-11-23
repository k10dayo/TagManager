using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TagManager.Models;

namespace TagManager.ViewModels
{
    public class SettingDialogViewModel : BindableBase, IDialogAware
    {
        public SettingDialogViewModel()
        {
            BasePath = UserSettingHandler.GetBasePath();
            ClickSaveButton = new DelegateCommand(ClickSaveButtonExecute);
        }

        public string Title => "設定ダイアログ";

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog() => true;

        public void OnDialogClosed() { }

        public void OnDialogOpened(IDialogParameters parameters) { }


        // TextBoxのバインディング用のプロパティ
        private string _basePath;
        public string BasePath
        {
            get { return _basePath; }
            set { SetProperty(ref _basePath, value); }
        }

        public DelegateCommand ClickSaveButton { get; }
        public void ClickSaveButtonExecute()
        {
            Debug.Print($"{BasePath}");

            UserSettingHandler.SetBasePath(BasePath);
        }
    }
}
