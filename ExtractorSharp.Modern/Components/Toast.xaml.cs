
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace ExtractorSharp.Components {

    public partial class Toast : UserControl, INotifyPropertyChanged {


        private DispatcherTimer timer = new DispatcherTimer();

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _message = "Message!";

        public string Message {
            set {
                this._message = value;
                this.OnPropertyChanged("Message");
            }
            get => this._message;
        }


        private string _iconKind = null;

        public string IconKind {
            set {
                this._iconKind = value;
                this.OnPropertyChanged("IconKind");
            }
            get => this._iconKind;
        }

        private string _iconColor = null;

        public string IconColor {
            set {
                this._iconColor = value;
                this.OnPropertyChanged("IconColor");
            }
            get => this._iconColor;
        }

        #region Constructors
        public Toast() {
            this.DataContext = this;
            this.InitializeComponent();
        }

        #endregion

        /// <summary>
        /// Display a toast message for a defined amount of time
        /// </summary>
        /// <param name="message">The message to be shown</param>
        /// <param name="title">The title of the toast</param>
        /// <param name="delay">Time in seconds that the toast will be shown</param>
        /// <param name="position">The position of the toast on the screen</param>
        public void Show(string message, string type = "info", int delay = 4500) {
            this.Message = message;
            this.SetType(type);

            this.Visibility = Visibility.Visible;
            if(this.timer != null) {
                this.timer.Stop();
            }
            this.timer = new DispatcherTimer {
                Interval = TimeSpan.FromMilliseconds(delay)
            };
            this.timer.Tick += this.CloseToast;
            this.timer.Start();
        }

        private void SetType(string type) {
            switch(type.ToLower()) {
                case "success":
                    this.IconKind = "CheckCircleOutline";
                    this.IconColor = "#EE67C23A";
                    break;
                case "info":
                    this.IconKind = "Information";
                    this.IconColor = "Gray";
                    break;
                case "error":
                    this.IconKind = "CloseCircle";
                    this.IconColor = "Red";
                    break;
                case "warning":
                    this.IconKind = "Alert";
                    this.IconColor = "Orange";
                    break;
            }
        }

        public void Close() {
            this.Visibility = Visibility.Collapsed;
        }

        private void CloseToast(object sender, EventArgs e) {
            this.Close();
        }

    }
}