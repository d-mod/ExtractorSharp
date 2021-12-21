using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using ExtractorSharp.Components;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Compatibility;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Composition.Core;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Services.Constants;

namespace ExtractorSharp {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    [Export]
    public partial class MainWindow : BaseWindow, IPartImportsSatisfiedNotification {

        [Import]
        private Viewer Viewer { set; get; }

        [Import]
        private WindowModel Model { set; get; }

        [Import]
        private ICommonMessageBox MessageBox { set; get; }

        private DispatcherTimer AnimationTimer;


        public void OnImportsSatisfied() {


            this.DataContext = this.Model;


            this.InitializeComponent();
            this.AddListener();
            this.AddStore();
            this.AddPaint();
            this.AnimationTimer = new DispatcherTimer(DispatcherPriority.Normal, this.imageList.Dispatcher) {
                Interval = TimeSpan.FromMilliseconds(1000 / Math.Max(Model.AnimationSpeed, 1))
            };
            this.AnimationTimer.Tick += this.AnmiationTick;
        }

        private void AddListener() {
            this.Viewer.ViewCreated += this.OnViewCreated;
            this.Messager.Sent += this.OnMessageSent;
            this.Controller.CommandChanged += this.OnCommandChanged;
            this.CommandBindings.AddRange(this.Model.WindowCommandBindings);
            this.fileList.CommandBindings.AddRange(this.Model.FileListCommandBindings);
            this.imageList.CommandBindings.AddRange(this.Model.ImageListCommandBindings);
        }





        private void AddStore() {

            object ListConverter<T>(object o) {
                return o is IList list ? list.OfType<T>() : o;
            }



            this.fileList.SelectionChanged += this.Store.OnValueChanged;
            this.imageList.SelectionChanged += this.Store.OnValueChanged;



            this.Store
                .Bind(StoreKeys.SELECTED_FILE_RANGE, this.fileList, "SelectedItems", ListConverter<Album>)
                .Bind(StoreKeys.SELECTED_IMAGE_RANGE, this.imageList, "SelectedItems", ListConverter<Sprite>)
                .Compute(StoreKeys.SELECTED_FILE_INDICES, () => {
                    return this.Model.Files.FindIndices(this.fileList.SelectedItems.OfType<Album>());
                })
                .Compute(StoreKeys.SELECTED_IMAGE_INDICES, () => {
                    return this.Model.Sprites.FindIndices(this.imageList.SelectedItems.OfType<Sprite>());
                })
                .Register(StoreKeys.APP_EXIT, this.Close)
                .Register(StoreKeys.APP_NEW_WINDOW, () => {
                    Process.Start(Application.ResourceAssembly.Location);
                })
                .Register(StoreKeys.FILES_SELECT_ALL, this.fileList.SelectAll)
                .Register(StoreKeys.FILES_UNSELECT_ALL, this.fileList.UnselectAll)
                .Register(StoreKeys.IMAGES_SELECT_ALL, this.imageList.SelectAll)
                .Register(StoreKeys.IMAGES_UNSELECT_ALL, this.imageList.UnselectAll)
                ;


        }

        private void AddPaint() {
            // canvas.Children.Add(Layer);
        }

        private void OnCommandChanged(object sender, CommandEventArgs e) {
            var refreshMode = e.Context.Get<RefreshMode>("__REFRESH_MODE");
            switch(refreshMode) {
                case RefreshMode.List:
                    this.Model.OnPropertyChanged("Files");
                    this.Model.OnPropertyChanged("SelectedFile");
                    break;
                case RefreshMode.File:
                    this.Model.RefreshSprites();
                    break;
                case RefreshMode.Image:
                    this.Model.OnPropertyChanged("ImageSource");
                    break;
            }
        }

        private void OnMessageSent(MessageEventArgs e) {
            this.toast.Show(this.Language[e.Message], e.Type.ToString());
        }

        private void OnViewCreated(object sender, ViewEventArgs e) {
            if(e.View is Window window) {
                window.Name = e.Name;
                window.Owner = this;
                window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                window.Title = this.Language[e.Title];
            }
        }





        private void OpenFile(object sender, MouseButtonEventArgs e) {
            this.Controller.Do("OpenFile");
        }


        private void DragHeader(object sender, MouseButtonEventArgs e) {
            this.DragMove();
        }

        private void RealPosition(object sender, RoutedEventArgs e) {
            var x = 0;
            var y = 0;
            if(this.realPositionCheckbox.IsChecked == true) {
                if(this.imageList.SelectedItem is Sprite file) {
                    x = file.X;
                    y = file.Y;
                }
            }
            this.Model.ImageX = x;
            this.Model.ImageY = y;
        }

        private void CheckAnimation(object sender, RoutedEventArgs e) {
            this.AnimationTimer.IsEnabled = this.animationCheckbox.IsChecked ?? false;
        }



        private void AnmiationTick(object sender, EventArgs e) {
            var max = this.imageList.Items.Count - 1;
            if(max < 1) {
                return;
            }
            var i = this.imageList.SelectedIndex + 1;
            i = Math.Min(i, max);
            i = i == max ? 0 : i;
            this.imageList.SelectedIndex = i;
            this.imageList.ScrollIntoView(this.imageList.SelectedItem);
        }

        protected override void OnClosing(CancelEventArgs e) {
            this.AnimationTimer.Stop();
            var context = new CommandContext();
            Controller.Do("ClosingConfirm", context);
            e.Cancel = context.Get<bool>("cancel");
            base.OnClosing(e);
        }

    }
}
