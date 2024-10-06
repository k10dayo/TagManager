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
    public class CommonProperty : BindableBase, INotifyPropertyChanged
    {
        private bool _isNaviOpen = true;
        public bool IsNaviOpen
        {
            get { return _isNaviOpen; }
            set 
            {
                SetProperty(ref _isNaviOpen, value);
                OnPropertyChanged(nameof(IsNaviOpen));
                Debug.Print("noo");

            }
        }
        public void toggleIsNaviOpen()
        {
            IsNaviOpen = !IsNaviOpen;
        }


        //通知を飛ばす用
        public new event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
