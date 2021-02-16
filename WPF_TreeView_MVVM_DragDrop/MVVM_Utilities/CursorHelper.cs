using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MVVM_Utilities
{
  /// <summary>
  /// Cursor für Drag & Drop aus visuellen Elementen zusammensetzen
  /// Quelle: https://wpf.2000things.com/2012/12/17/713-setting-the-cursor-to-an-image-of-an-uielement-while-dragging/
  /// Code modifiziert, da Original unter .NET 5 so nicht mehr funktioniert
  /// Package System.Drawing.Common erforderlich
  /// </summary>
  public class CursorHelper
  {
    private static class NativeMethods
    {
      public struct IconInfo
      {
        public bool fIcon;
        public int xHotspot;
        public int yHotspot;
        public IntPtr hbmMask;
        public IntPtr hbmColor;
      }

      [DllImport("user32.dll")]
      public static extern SafeIconHandle CreateIconIndirect(ref IconInfo icon);

      [DllImport("user32.dll")]
      public static extern bool DestroyIcon(IntPtr hIcon);

      [DllImport("user32.dll")]
      [return: MarshalAs(UnmanagedType.Bool)]
      public static extern bool GetIconInfo(IntPtr hIcon, ref IconInfo pIconInfo);
    }

    //[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
    private class SafeIconHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
      public SafeIconHandle()
          : base(true)
      {
      }

      override protected bool ReleaseHandle()
      {
        return NativeMethods.DestroyIcon(handle);
      }
    }

    private static Cursor InternalCreateCursor(System.Drawing.Bitmap bmp)
    {
      var iconInfo = new NativeMethods.IconInfo();
      NativeMethods.GetIconInfo(bmp.GetHicon(), ref iconInfo);

      iconInfo.xHotspot = 0;
      iconInfo.yHotspot = 0;
      iconInfo.fIcon = false;

      SafeIconHandle cursorHandle = NativeMethods.CreateIconIndirect(ref iconInfo);
      return CursorInteropHelper.Create(cursorHandle);
    }

    /// <summary>
    /// Erstellen eines Cursors für die Darstellung während eines Drag-Vorgangs
    /// </summary>
    /// <param name="dragElement"></param>
    /// <returns></returns>
    public static Cursor CreateCursor(Control dragElement)
    {
      // Zur Demo: Border-Control um Kopie des Inhalts des TreeViewItems
      ContentControl cc = new ContentControl();
      cc.Content = (dragElement.DataContext as TreeItem)?.Data;
      cc.Height = dragElement.ActualHeight;
      cc.Width = dragElement.ActualWidth;

      // Einstellungen für Rahmen
      Border border = new Border();
      border.Child = cc;
      border.BorderBrush = Brushes.Black;
      border.BorderThickness = new Thickness(1);
      border.Padding = new Thickness(2);
      border.Background = new SolidColorBrush(Color.FromArgb(230, 190, 230, 230));

      // Größe anpassen
      border.LayoutTransform = new ScaleTransform(0.8, 0.8);

      border.Visibility = Visibility.Visible;
      border.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
      var offset = new Point(0, 0);  // Offset zum Mauszeiger, falls gewünscht
      border.Arrange(new Rect(offset, border.DesiredSize));

      RenderTargetBitmap rtb =
        new RenderTargetBitmap(
          (int)border.DesiredSize.Width + (int)offset.X,
          (int)border.DesiredSize.Height + (int)offset.Y,
          96, 96, PixelFormats.Pbgra32);

      rtb.Render(border);


      var encoder = new PngBitmapEncoder();
      encoder.Frames.Add(BitmapFrame.Create(rtb));

      using (var ms = new MemoryStream())
      {
        encoder.Save(ms);
        using (var bmp = new System.Drawing.Bitmap(ms))
        {
          return InternalCreateCursor(bmp);
        }
      }
    }


  }
}
