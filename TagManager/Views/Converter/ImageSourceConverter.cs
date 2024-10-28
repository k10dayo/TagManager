using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace TagManager.Views.Converter;

public class ImageSourceConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        string imagePath = value as string;

        // 画像が存在するかどうかをチェック
        if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
        {
            return imagePath;
        }

        // 画像が存在しない場合のデフォルトパス
        return "pack://application:,,,/TagManager;component/Resources/Images/NoImage.png";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
