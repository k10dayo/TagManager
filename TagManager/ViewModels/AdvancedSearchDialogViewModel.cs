using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace TagManager.ViewModels
{
	public class AdvancedSearchDialogViewModel : BindableBase, IDialogAware
    {
        public AdvancedSearchDialogViewModel()
        {
            OkCommand = new DelegateCommand(OnOkButtonClick);
            CancelCommand = new DelegateCommand(OnCancelButtonClick);

            Nodes = new ObservableCollection<TreeNode>() { DataGenerater.GenerateSample() };
        }

        // OKボタンのコマンド
        public DelegateCommand OkCommand { get; }

        // OKボタンが押されたときの処理
        private void OnOkButtonClick()
        {
            var parameters = new DialogParameters
            {
                // 結果として渡したい値
                { "resultParam", "検索結果" }
            };

            // ダイアログを閉じるときに結果を渡す
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK, parameters));
        }

        // キャンセルボタンのコマンド
        public DelegateCommand CancelCommand { get; }

        // キャンセルボタンが押されたときの処理
        private void OnCancelButtonClick()
        {
            var parameters = new DialogParameters
            {
                // 結果として渡す値（キャンセルの場合はnull）
                { "resultParam", null }
            };

            RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel, parameters));
        }

        //インタフェース必須
        public string Title => "高度な検索";

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            // パラメータを受け取る場合
            if (parameters.ContainsKey("paramKey"))
            {
                var param = parameters.GetValue<string>("paramKey");
                Debug.Print(param);
            }
        }


        public ObservableCollection<TreeNode> Nodes { get; set; }
    }


    public class DataGenerater
    {
        public static TreeNode GenerateSample()
        {
            return
                new TreeNode("Root")
                {
                    ChildrenFolder = new ObservableCollection<TreeNode>
                        {
                            new TreeNode("Folder 1")
                            {
                                ChildrenFile = new ObservableCollection<FileData>
                                {
                                    new FileData("File 1.txt", 1000, ".txt"),
                                    new FileData("File 2.jpg", 250000, ".jpg")
                                },
                                ChildrenFolder = new ObservableCollection<TreeNode>
                                {
                                    new TreeNode("Subfolder 1")
                                    {
                                        ChildrenFile = new ObservableCollection<FileData>
                                        {
                                            new FileData("File 3.txt", 500, ".txt")
                                        }
                                    }
                                }
                            },
                            new TreeNode("Folder 2")
                            {
                                ChildrenFile = new ObservableCollection<FileData>
                                {
                                    new FileData("File 4.png", 150000, ".png")
                                }
                            }
                        },
                    ChildrenFile = new ObservableCollection<FileData>
                        {
                            new FileData("File 1.txt", 1000, ".txt"),
                            new FileData("File 2.jpg", 250000, ".jpg")
                        }
                };
        }
    }


    public class TreeNode
    {
        public string Name { get; set; }
        public ObservableCollection<TreeNode> ChildrenFolder { get; set; }
        public ObservableCollection<FileData> ChildrenFile { get; set; }

        public TreeNode(string name)
        {
            Name = name;
            ChildrenFolder = new ObservableCollection<TreeNode>();
            ChildrenFile = new ObservableCollection<FileData>();
        }
    }

    public class FileData
    {
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public string FileType { get; set; }

        public FileData(string fileName, long fileSize, string fileType)
        {
            FileName = fileName;
            FileSize = fileSize;
            FileType = fileType;
        }
    }
}
