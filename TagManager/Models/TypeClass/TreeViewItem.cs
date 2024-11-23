using System.Collections.Generic;

namespace TagManager.Models.TypeClass;
public class TreeViewItem
{
    public TreeViewItem(string name)
    {
        Name = name;
    }

    public string Name { get; }
    public List<TreeViewItem> ChildrenItems { get; set; } = new List<TreeViewItem>();
}    
