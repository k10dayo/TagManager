using System.Windows;
using System.Windows.Controls;
using TagManager.ViewModels;
using TagManager.Models;
using System.Diagnostics;

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
                // ContextMenuが開かれたターゲットを取得
                var target = contextMenu.PlacementTarget as FrameworkElement;
                if (target != null)
                {
                    // ターゲットのデータコンテキストを取得
                    var dataContext = target.DataContext as ManagerWindowWrapper;

                    // ListBox のデータコンテキストを ContextMenu に設定
                    contextMenu.DataContext = Lst.DataContext;

                    // dataContextがnullでない場合
                    if (dataContext != null)
                    {
                        // 既存のアイテムをクリア
                        contextMenu.Items.Clear();

                        //MenuItemを作成
                        contextMenu.Items.Add(
                            new MenuItem
                            {
                                Header = "削除する",
                                Command = (contextMenu.DataContext as TabMenuViewModel)?.DeleteTab,
                                CommandParameter = dataContext.ViewId // ここでCommandParameterを設定
                            }
                        );
                        contextMenu.Items.Add(
                            new MenuItem
                            {
                                Header = "お気に入りに登録",
                                Command = (contextMenu.DataContext as TabMenuViewModel)?.DeleteTab,
                                CommandParameter = dataContext.ViewId // ここでCommandParameterを設定
                            }
                        );
                        contextMenu.Items.Add(
                            new MenuItem
                            {
                                Header = "タブの複製",
                                Command = (contextMenu.DataContext as TabMenuViewModel)?.DeplicateTab,
                                CommandParameter = dataContext.SearchInfo // ここでCommandParameterを設定
                            }
                        );
                    }
                    else
                    {
                        Debug.Print("データコンテキストがnullです。");
                    }
                }
                else
                {
                    Debug.Print("PlacementTargetがnullです。");
                }
            }

        }
    }
}
