using EverythingNet.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TagManager.Models;

namespace TagManager.Views.Selector
{
    public class FileTypeTemplateSelector : DataTemplateSelector
    {
        public DataTemplate PictureTemplate { get; set; }

        public DataTemplate FileTemplate { get; set; }
        public DataTemplate FolderTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var fileItem = item as ISearchResult; // ここは適切な型に変更してください。
            if (fileItem != null)
            {
                if (fileItem.IsFile)
                {
                    string ext = Path.GetExtension(fileItem.FileName).ToLower();//ファイルの拡張子を取得して小文字に変換
                    if (AppConfig.extPicture.Contains(ext))//画像の拡張子に一致したら、画像用のテンプレートにする
                    {
                        return PictureTemplate;
                    }
                    return FileTemplate;
                }
                else
                {
                    return FolderTemplate;
                }
            }
            return base.SelectTemplate(item, container);
        }
    }
}
