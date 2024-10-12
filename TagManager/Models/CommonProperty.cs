using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagManager.Models
{
    //ナビのボタンなど、共通で使いたいプロパティ
    public class CommonProperty : BindableBase, INotifyPropertyChanged
    {
        private bool _isNaviOpen = true;
        public bool IsNaviOpen
        {
            get { return _isNaviOpen; }
            set 
            {
                SetProperty(ref _isNaviOpen, value);
                OnNaviButtonHandler(nameof(IsNaviOpen));
            }
        }
        public void toggleIsNaviOpen()
        {
            IsNaviOpen = !IsNaviOpen;
        }


        //通知を飛ばす用
        public event PropertyChangedEventHandler NaviButtonHandler;
        protected virtual void OnNaviButtonHandler(string propertyName)
        {
            NaviButtonHandler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
