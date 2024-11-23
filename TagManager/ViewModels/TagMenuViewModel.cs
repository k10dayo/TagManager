using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TagManager.Models.TypeClass;

namespace TagManager.ViewModels
{
    public class TagMenuViewModel : BindableBase
    {
        public TagMenuViewModel()
        {
            var root = new TreeViewItem ("Root");
            var child1 = new TreeViewItem("child1");
            var child2 = new TreeViewItem("child2");

            root.ChildrenItems.Add(child1);
            root.ChildrenItems.Add(child2);

            TreeData = new ObservableCollection<TreeViewItem>() { root };
        }

        public ObservableCollection<TreeViewItem> TreeData { get; set; }
    }
}
