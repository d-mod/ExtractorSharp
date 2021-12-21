using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace ExtractorSharp.Components {
    public class DragImage : Image {

        private Point CursorPoint { set; get; }

        private bool IsMove { set; get; }

        private int Scale { set; get; } = 100;




        public DragImage() {
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e) {
            this.CursorPoint = e.GetPosition(this);
            this.IsMove = true;
        }


        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e) {
            this.IsMove = false;
        }

        protected override void OnMouseLeave(MouseEventArgs e) {
            this.IsMove = false;
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e) {
            if(Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) {
                var scale = (this.Scale += e.Delta / 12) / 100.0;
                scale = Math.Max(0.2, scale);
                this.RenderTransform = new ScaleTransform(scale, scale);
            }
        }

        protected override void OnMouseMove(MouseEventArgs e) {
            if(this.IsMove && e.LeftButton == MouseButtonState.Pressed) {
                var point = e.GetPosition(this.Parent as IInputElement);
                point.Offset(-this.CursorPoint.X, -this.CursorPoint.Y);
                Canvas.SetLeft(this, point.X);
                Canvas.SetTop(this, point.Y);
               // this.RenderTransform = new TranslateTransform(point.X,point.Y);
                /*
                this.Margin = new Thickness(point.X, point.Y, 0, 0);*//*
                this.SetValue(MarginProperty, point.X);
                this.SetValue(Canvas.TopProperty, point.Y);*/
            }
            //base.OnMouseMove(e);
        }

    }
}
