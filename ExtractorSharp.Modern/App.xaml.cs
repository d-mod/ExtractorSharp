using System;
using System.Windows;
using System.Windows.Threading;
using ExtractorSharp.Services;

namespace ExtractorSharp {
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application {


        protected override void OnStartup(StartupEventArgs e) {
            var mainWindow = Starter.GetValue<MainWindow>();
            mainWindow.Show();
            this.DispatcherUnhandledException += this.App_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += this.ThrowsExcepiton;

        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e) {

        }

        private void ThrowsExcepiton(object sender, UnhandledExceptionEventArgs e) {

        }


    }
}
