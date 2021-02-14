using MVVM_Utilities;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using WPF_TreeView_MVVM_DragDrop.Models;

namespace WpfTreeViewBeispiel
{
  public class MainViewModel : NotificationObject
  {
    private TreeItem rootTreeItem1;

    public TreeItemList Tree1
    {
      get { return rootTreeItem1.Items; }
    }

    private TreeItem rootTreeItem2;

    public TreeItemList Tree2
    {
      get { return rootTreeItem2.Items; }
    }

    public ActionCommand ExpandAllCommand { get; set; }
    public ActionCommand CollapseAllCommand { get; set; }
    public ActionCommand OpenAppXamlCommand { get; set; }


    public DragDropController DragDropControllerTV1 { get; set; }

    public MainViewModel()
    {
      rootTreeItem1 = new TreeItem();
      rootTreeItem2 = new TreeItem();

      // Pfad bei Bedarf anpassen
      var path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName;

      FillTree(Tree1, path,0);
      FillTree(Tree2, path,0);

      ExpandAllCommand = new ActionCommand(() => Tree1.ExpandOrCollapseAll(true));
      CollapseAllCommand = new ActionCommand(() => Tree1.ExpandOrCollapseAll(false));
      OpenAppXamlCommand = new ActionCommand(OpenAppXaml);


      DragDropControllerTV1 = new DragDropController() { CanDrag = CanDragTV1, CanDrop=CanDropTV1, Drop=DropTV1 };
    }

    private bool DropTV1(object draggedObject, TreeItem target, double verticalPercentage)
    {
      var source = draggedObject as TreeItem;

      // Knoten soll nicht auf sich selbst gezogen werden
      if (draggedObject == target) return false;

      // Anwendungsspezifische Aktion
      source.Parent.Items.Remove(source);
      if (verticalPercentage < 20)
      {
        // davor einfügen
        var parentList = target.Parent.Items;
        var pos = parentList.IndexOf(target);
        parentList.Insert(pos, source);
      }
      else if (verticalPercentage>80)
      {
        // dahinter einfügen
        var parentList = target.Parent.Items;
        var pos = parentList.IndexOf(target);
        parentList.Insert(pos+1, source);
      }
      else
      {
        // als Unterelement einfügen
        target.Items.Add(source);
      }
      return true;
    }

    private DragDropEffects CanDropTV1(object draggedObject, TreeItem dropTarget, double verticalPercentage)
    {
      Debug.WriteLine($"CanDropTV1 y:{verticalPercentage}");

      if (dropTarget == draggedObject) return DragDropEffects.None;
      var data = dropTarget.Data as DataObjectBase;
      if (data == null) return DragDropEffects.None;
      return data.Caption.StartsWith(".") ? DragDropEffects.None : DragDropEffects.Move;
    }

    private bool CanDragTV1(TreeItem itemToDrag)
    {
      var data = itemToDrag.Data as DataObjectBase;
      if (data == null) return false;
      var startsWithPoint = data.Caption.StartsWith(".");
      return !startsWithPoint;
    }



    private void FillTree(TreeItemList tree, string directory, int level)
    {
      try
      {
        foreach (var dir in Directory.EnumerateDirectories(directory))
        {
          var ti = new TreeItem { Data = new DirectoryDataObject { Caption = Path.GetFileName(dir), Level=level }, ToolTip = dir, IsEnabled= Path.GetFileName(dir) != "obj" };
          tree.Add(ti);
          FillTree(ti.Items, dir, level+1);
        }

        foreach (var file in Directory.EnumerateFiles(directory))
        {
          var ti = new TreeItem { Data = new FileDataObject { Caption = Path.GetFileName(file), Level=level }, ToolTip = file };
          ti.IsSelectedChanged += Ti_IsSelectedChanged;
          tree.Add(ti);
        }

      }
      catch (Exception)
      {
      }
    }

    private void Ti_IsSelectedChanged(object sender, EventArgs e)
    {
      var selectedItem = sender as TreeItem;
      var data = selectedItem?.Data as DataObjectBase;
      Debug.WriteLine($"Selection changed: {data?.Caption}: {selectedItem.IsSelected} ");
    }

    private void OpenAppXaml()
    {
      var item = FindCaption("App.xaml", Tree1);
      Debug.WriteLine("App.xaml gefunden: " + ((FileDataObject)item.Data).Caption);
      item.IsSelected = true;
      item.ExpandAncestors(true);
    }

    private TreeItem FindCaption(string caption, TreeItemList list)
    {
      foreach (var item in list)
      {
        if (((DataObjectBase)item.Data).Caption == caption) return item;
        var subitem = FindCaption(caption, item.Items);
        if (subitem != null) return subitem;
      }
      return null;
    }

  }
}
