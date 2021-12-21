using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Input;
using ExtractorSharp.Composition;

namespace ExtractorSharp.Components {

    public class BaseDialog : BaseWindow, IView, IPartImportsSatisfiedNotification {

        protected override void OnKeyDown(KeyEventArgs e) {
            if(e.Key == Key.Escape) {
                e.Handled = true;
                this.Hide();
                return;
            }
            base.OnKeyDown(e);
        }

        public virtual object ShowView(params object[] args) {
            this.OnShowing(args);
            return this.ShowDialog();
        }

        public void ShowAsync() {
            this.Show();
        }

        protected override void OnClosing(CancelEventArgs e) {
            e.Cancel = true;  // cancels the window close    
            this.Hide();      // Programmatically hides the window
        }

        protected virtual void OnShowing(params object[] args) {
            
        }

        public virtual void OnImportsSatisfied() {
            this.ResizeMode = ResizeMode.NoResize;
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            this.ShowInTaskbar = false;
        }

    }
}
