using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Media;
using TagManager.Models;

namespace TagManager.ViewModels
{
    public class ManagerWindowViewModel : BindableBase
    {
        CommonProperty _commonProperty;
        public ManagerWindowViewModel(CommonProperty commonProperty)
        {
            _commonProperty = commonProperty;
            NaviButtonClick = new DelegateCommand(NaviButtonClickExecute);
        }
        

        public DelegateCommand NaviButtonClick { get; }
        public void NaviButtonClickExecute()
        {
            _commonProperty.toggleIsNaviOpen();
        }
    }
}
