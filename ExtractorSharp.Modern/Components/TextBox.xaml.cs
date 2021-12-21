using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace ExtractorSharp.Components {
    /// <summary>
    /// ETextbox.xaml 的交互逻辑
    /// </summary>
    public partial class ETextbox : TextBox {

        [Bindable(true)]
        [Category("Common")]
        [DefaultValue("")]
        public string PlaceHolder {
            set => this.SetValue(PlaceHolderProperty, value);
            get => (string)this.GetValue(PlaceHolderProperty);
        }

        public static readonly DependencyProperty PlaceHolderProperty = DependencyProperty.Register("PlaceHolder", typeof(string), typeof(ETextbox), new PropertyMetadata(null));


        public ETextbox() {
            this.InitializeComponent();
        }
    }
}
