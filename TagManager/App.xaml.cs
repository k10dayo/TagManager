using Prism.Ioc;
using System.Windows;
using TagManager.Models;
using TagManager.Models.EverythinModels;
using TagManager.ViewModels;
using TagManager.Views;

namespace TagManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterManySingleton<MainWindow>();

            containerRegistry.RegisterForNavigation<BaseNavi>();
            containerRegistry.RegisterSingleton<BaseNaviViewModel>();

            containerRegistry.RegisterForNavigation<ManagerWindow>();

            containerRegistry.RegisterForNavigation<TabMenu>();
            containerRegistry.RegisterSingleton<TabMenuViewModel>();

            containerRegistry.RegisterForNavigation<TagMenu>();
            containerRegistry.RegisterSingleton<TagMenuViewModel>();


            containerRegistry.RegisterSingleton<CommonProperty>();
            containerRegistry.RegisterSingleton<ManagerWindowWrapperList>();

            containerRegistry.RegisterSingleton<EverythingModel>();


            containerRegistry.RegisterDialog<SettingDialog>();
            containerRegistry.RegisterDialog<AdvancedSearchDialog, AdvancedSearchDialogViewModel>();



        }
    }
}
