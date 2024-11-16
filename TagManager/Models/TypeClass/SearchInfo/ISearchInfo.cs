using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagManager.Models.TypeClass.SearchInfo;

public interface ISearchInfo
{
    /// <summary>
    ///     タブメニューに短く表示する
    /// </summary>
    public string Label { get; }
    /// <summary>
    ///     マネージャーウィンドウの上段に表示する
    /// </summary>
    public string FullLabel { get; }
    /// <summary>
    ///     everythingの検索に使用する
    /// </summary>
    public string Command { get; }

    public string Thumbnail { get; }
}
