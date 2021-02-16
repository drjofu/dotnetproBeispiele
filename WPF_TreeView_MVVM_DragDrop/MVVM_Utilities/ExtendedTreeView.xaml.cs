using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace MVVM_Utilities
{
  /// <summary>
  /// Interaction logic for ExtendedTreeView.xaml
  /// </summary>
  public partial class ExtendedTreeView : UserControl
  {
    private bool isDragging = false;
    private Point startposition;
    private TreeViewItem treeViewItemToDrag;

    private DispatcherTimer delayTimer = new DispatcherTimer();
    private TreeItem treeItemToExpandAfterDelay;

    private Cursor customCursor = null;

    private TreeItemDragOverAdorner adorner;


    public ExtendedTreeView()
    {
      delayTimer.Interval = TimeSpan.FromMilliseconds(500);
      delayTimer.Tick += DelayTimer_Tick;
      InitializeComponent();
    }

    // Nach Ablauf der eingestellten Zeit soll das TreeViewItem unter 
    // dem Mauszeiger automatisch aufgeklappt werden
    private void DelayTimer_Tick(object sender, EventArgs e)
    {
      delayTimer.Stop();
      if (treeItemToExpandAfterDelay != null)
        treeItemToExpandAfterDelay.IsExpanded = true;
    }


    public TreeItemList Items
    {
      get { return (TreeItemList)GetValue(ItemsProperty); }
      set { SetValue(ItemsProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Items.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ItemsProperty =
        DependencyProperty.Register("Items", typeof(TreeItemList), typeof(ExtendedTreeView), new FrameworkPropertyMetadata(OnItemsChanged));

    private static void OnItemsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
      var uc = sender as ExtendedTreeView;
      uc._tv_.ItemsSource = uc.Items;
    }


    public DragDropController DragDropController
    {
      get { return (DragDropController)GetValue(DragDropControllerProperty); }
      set { SetValue(DragDropControllerProperty, value); }
    }

    // Using a DependencyProperty as the backing store for DragDropController.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty DragDropControllerProperty =
        DependencyProperty.Register("DragDropController", typeof(DragDropController), typeof(ExtendedTreeView));



    private void TVI_PreviewMouseDown(object sender, MouseButtonEventArgs e)
    {
      startposition = e.GetPosition(this);
      treeViewItemToDrag = sender as TreeViewItem;
      isDragging = false;
    }

    private void TVI_MouseMove(object sender, MouseEventArgs e)
    {
      if (DragDropController?.CanDrag == null) return;

      TreeViewItem tvi = sender as TreeViewItem;
      
      // Ist die Maus vielleicht bereits auf ein anderes Item gerutscht?
      if (tvi != treeViewItemToDrag)
      {
        return;
      }

      isDragging = e.GetPosition(this) != startposition;
      var ti = tvi.DataContext as TreeItem;

      // Rahmenbedingungen für DragDrop-Start prüfen
      if (e.LeftButton == MouseButtonState.Pressed && isDragging && DragDropController.CanDrag(ti))
      {
        Debug.WriteLine($"DragDrop starting: {ti}");
        ScrollUpBtn.Visibility = Visibility.Visible;
        ScrollDownBtn.Visibility = Visibility.Visible;

        // DoDragDrop kehrt erst nach Abschluss der Aktion wieder zurück
        DragDrop.DoDragDrop(tvi, tvi.DataContext, DragDropEffects.All);

        // fertig
        ScrollUpBtn.Visibility = Visibility.Collapsed;
        ScrollDownBtn.Visibility = Visibility.Collapsed;

        this.adorner?.Remove();
        adorner = null;
        customCursor = null;

        Debug.WriteLine($"DragDrop completed: {ti}");
      }

    }

    // sender ist das TreeViewItem, das verschoben wird
    private void TVI_GiveFeedback(object sender, GiveFeedbackEventArgs e)
    {
      var tvi = sender as TreeViewItem;
      var ti = tvi.DataContext as TreeItem;
      Debug.WriteLine($"GiveFeedback {ti.Data}, effects: {e.Effects}");

      if (e.Effects.HasFlag(DragDropEffects.Move))
      {
        if (customCursor == null)
          customCursor = CursorHelper.CreateCursor(e.Source as Control);

        if (customCursor != null)
        {
          e.UseDefaultCursors = false;
          Mouse.SetCursor(customCursor);
        }
      }
      else
      {
        e.UseDefaultCursors = true;
      }

      e.Handled = true;

    }


    private void TVI_DragEnter(object sender, DragEventArgs e)
    {
      if (DragDropController?.CanDrop == null) return;
      var tvi = sender as TreeViewItem;
      var ti = tvi.DataContext as TreeItem;

      if (this.adorner == null)
      {
        var mousePos = e.GetPosition(tvi);
        var header = tvi.Template.FindName("PART_Header", tvi) as FrameworkElement;
        var heightHeader = header?.ActualHeight ?? tvi.ActualHeight;

        var draggedTreeItem = e.Data.GetData(typeof(TreeItem));
        var yPercentage = mousePos.Y * 100 / heightHeader;

        this.adorner = new TreeItemDragOverAdorner(header, yPercentage);

      }

      Debug.WriteLine($"DragEnter {ti.Data}, Effects: {e.Effects}");

      // keine weitere Aktion
      e.Handled = true;
    }

    private void TVI_PreviewDragEnter(object sender, DragEventArgs e)
    {
      var tvi = sender as TreeViewItem;
      var ti = tvi.DataContext as TreeItem;
      Debug.WriteLine($"PreviewDragEnter {ti.Data}");

      // das TreeItem ggf. zum automatischen Aufklappen vorsehen
      if (ti != treeItemToExpandAfterDelay)
      {
        delayTimer.Stop();
        treeItemToExpandAfterDelay = ti;
        delayTimer.Start();
      }
    }

    private void TVI_Drop(object sender, DragEventArgs e)
    {
      if (DragDropController?.Drop == null) return;
      var tvi = sender as TreeViewItem;
      var ti = tvi.DataContext as TreeItem;

      var mousePos = e.GetPosition(tvi);
      var header = tvi.Template.FindName("PART_Header", tvi) as FrameworkElement;
      var heightHeader = header?.ActualHeight ?? tvi.ActualHeight;


      this.adorner?.Remove();
      adorner = null;
      customCursor = null;

      var draggedTreeItem = e.Data.GetData(typeof(TreeItem));
      DragDropController.Drop(draggedTreeItem, ti, mousePos.Y * 100 / heightHeader);
      e.Handled = true;
    }




    private void TVI_DragOver(object sender, DragEventArgs e)
    {
      if (DragDropController?.CanDrop == null) return;
      var tvi = sender as TreeViewItem;
      var ti = tvi.DataContext as TreeItem;

      var mousePos = e.GetPosition(tvi);
      var header = tvi.Template.FindName("PART_Header", tvi) as FrameworkElement;
      var heightHeader = header?.ActualHeight ?? tvi.ActualHeight;

      var draggedTreeItem = e.Data.GetData(typeof(TreeItem));
      var yPercentage = mousePos.Y * 100 / heightHeader;
      e.Effects = DragDropController.CanDrop(draggedTreeItem, ti, yPercentage);
      Debug.WriteLine($"DragOver {ti.Data}, Effects: {e.Effects}, MousePos: {mousePos}");

      if (this.adorner != null)
        this.adorner.Update(yPercentage);

      e.Handled = true;

    }

    // TreeView DragOver, um kein Drop auf TreeView selbst zuzulassen
    private void TV_DragOver(object sender, DragEventArgs e)
    {
      e.Effects = DragDropEffects.None;
      e.Handled = true;
    }

    private void TVI_DragLeave(object sender, DragEventArgs e)
    {
      this.adorner?.Remove();
      adorner = null;
      e.Handled = true;
      Debug.WriteLine("TVI DragLeave");
    }

    // ScrollViewer aus Template des TreeViews ermitteln
    private ScrollViewer TVScrollViewer => _tv_.Template.FindName("_tv_scrollviewer_", _tv_) as ScrollViewer;

  
    // Hilfs-Buttons für Scrolling während Drag & Drop
    private void ScrollUpBtn_DragOver(object sender, DragEventArgs e)
    {
      TVScrollViewer.LineUp();

      // Kein Drop zulassen
      e.Effects = DragDropEffects.None;
    }

    private void ScrollDownBtn_DragOver(object sender, DragEventArgs e)
    {
      TVScrollViewer.LineDown();

      // Kein Drop zulassen
      e.Effects = DragDropEffects.None;

    }

    private async void TVI_Selected(object sender, RoutedEventArgs e)
    {
      var tvi = sender as TreeViewItem;
      await Task.Delay(100);
      tvi.BringIntoView();
    }
  }
}
