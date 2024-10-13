using Prism.Mvvm;
using Prism.Regions;
using TagManager.Views;

namespace TagManager.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Prism Application";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private readonly IRegionManager _regionManager;
        public MainWindowViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;

            regionManager.RegisterViewWithRegion("MainWindowRegion", nameof(BaseNavi));
        }
        
    }
}
