using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TagManager.ViewModels
{
    public class SettingDialogViewModel : BindableBase, IDialogAware
    {
        public SettingDialogViewModel()
        {

        }



        public string Title => "設定ダイアログ";

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog() => true;

        public void OnDialogClosed() { }

        public void OnDialogOpened(IDialogParameters parameters) { }
    }
}
