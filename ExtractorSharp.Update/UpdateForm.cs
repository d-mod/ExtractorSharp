using ExtractorSharp.Loose;
using System.Windows.Forms;

namespace ExtractorSharp.Update {
    public partial class UpdateForm : Form {

        private  const string UPDATE_URL =
            #if DEBUG
                "http://localhost/"
            #else
                "http://extractorsharp.kritsu.net/"
            #endif 
                +"api/program/update";
        private string CurrentVersion { set; get; }
        public UpdateForm(string[] args) {
            InitializeComponent();
            //CurrentVersion = args[0];
           // Compare();
            MessageBox.Show(UPDATE_URL);
        }

        private void Compare() {
            var builder = new LSBuilder();
            var obj = builder.Read($"{UPDATE_URL}?oldVersion={CurrentVersion}");
        }





    }
}
