using System.Windows;
using System.Windows.Controls;

namespace TagManager.Views
{
    /// <summary>
    /// Interaction logic for TabMenu
    /// </summary>
    public partial class TabMenu : UserControl
    {
        public TabMenu()
        {
            InitializeComponent();
        }

        //コンテキストメニューは開くたびにコードビハインドでデータコンテキストを渡す？さいないといけないらしい
        private void OnContextMenuOpened(object sender, RoutedEventArgs e)
        {
            if (sender is ContextMenu contextMenu)
            {
                // ListBox のデータコンテキストを ContextMenu に設定
                contextMenu.DataContext = Lst.DataContext;

            }
        }
    }
}
