using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExtractorSharp {
    public static class Tools {
        public static void InsertRange(this CheckedListBox.ObjectCollection collection, int index, object[] array) {
            var i = 0;
            while(i < array.Length) {
                collection.Insert(index++, array[i++]);
            }

        }

        public static void AddSeparator(this ToolStripItemCollection items) {
            items.Add(new ToolStripSeparator());
        }
    }
}
