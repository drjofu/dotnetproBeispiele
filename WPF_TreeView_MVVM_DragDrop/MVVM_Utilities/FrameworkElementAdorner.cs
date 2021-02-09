﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Windows;
using System.Windows.Media;
using System.Diagnostics;

namespace MVVM_Utilities
{
  class FrameworkElementAdorner : Adorner
  {
    private AdornerLayer adornerLayer;
    private double yPercentage = 0;

    public FrameworkElementAdorner(UIElement adornedElement, double yPercentage)
        : base(adornedElement)
    {
      SetPosition(yPercentage);
      this.adornerLayer = AdornerLayer.GetAdornerLayer(this.AdornedElement);
      this.adornerLayer.Add(this);
      Debug.WriteLine($"Adorner create {yPercentage}");
    }

    internal void Update(double yPercentage)
    {
      SetPosition(yPercentage);

      Debug.WriteLine($"Adorner update {yPercentage}%");
      this.adornerLayer.Update(this.AdornedElement);
      this.Visibility = System.Windows.Visibility.Visible;
    }

    private void SetPosition(double yPercentage)
    {
      if (yPercentage < 20)
        this.yPercentage = 0;
      else if (yPercentage > 80)
        this.yPercentage = 100;
      else this.yPercentage = 50;
    }

    public void Remove()
    {
      this.Visibility = System.Windows.Visibility.Collapsed;
      Debug.WriteLine($"Adorner remove");
    }

    protected override void OnRender(DrawingContext drawingContext)
    {
      Rect adornedElementRect = new Rect(this.AdornedElement.DesiredSize);

      SolidColorBrush renderBrush = new SolidColorBrush(Colors.Red);
      renderBrush.Opacity = 0.5;
      Pen renderPen = new Pen(new SolidColorBrush(Colors.White), 1.5);
      double renderRadius = 5.0;

      double y = adornedElementRect.Height * yPercentage / 100;

      if (yPercentage == 50)
      {

        drawingContext.DrawEllipse(renderBrush, renderPen, new Point(0,y), renderRadius, renderRadius);
      }
      else
      {
        Pen pen = new Pen(Brushes.Black, 2);
        Point p1 = new Point(-50, y);
        Point p2 = new Point(0, y);
        drawingContext.DrawLine(pen, p1, p2);

      }

      Debug.WriteLine($"Adorner OnRender {yPercentage}%, y: {y}");

    }

  }
}
