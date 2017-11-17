namespace ExtractorSharp.UI {
    public partial class ProgressDialog : EaseForm {
        public int Value {
            set {
                bar.Value = value;
            }
            get {
                return bar.Value;
            }
        }

        public int Count {
            set {
                bar.Maximum=value;
            }
            get {
                return bar.Maximum;
            }
        }

        public string ProgressText {
            set {
                label.Text = value;
            }
            get {
                return label.Text;
            }
        }



       


        public ProgressDialog() {
            InitializeComponent();
        }
    }
}
