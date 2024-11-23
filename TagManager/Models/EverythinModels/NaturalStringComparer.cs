using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TagManager.Models.EverythinModels;

public class NaturalStringComparer : IComparer<string>
{
    private static readonly Regex _numberRegex = new Regex(@"(\d+)", RegexOptions.Compiled);

    public int Compare(string x, string y)
    {
        if (x == null || y == null) return Comparer.Default.Compare(x, y);

        var xParts = _numberRegex.Split(x);
        var yParts = _numberRegex.Split(y);

        for (int i = 0; i < xParts.Length && i < yParts.Length; i++)
        {
            int result;

            // 数字部分を数値として比較
            if (int.TryParse(xParts[i], out int xNum) && int.TryParse(yParts[i], out int yNum))
            {
                result = xNum.CompareTo(yNum);
            }
            else
            {
                // 文字列部分をアルファベット順で比較
                result = string.Compare(xParts[i], yParts[i], CultureInfo.CurrentCulture, CompareOptions.IgnoreCase);
            }

            if (result != 0) return result;
        }

        return xParts.Length.CompareTo(yParts.Length);
    }
}