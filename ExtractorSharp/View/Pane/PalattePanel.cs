using System;
using System.Drawing;
using System.Windows.Forms;
using ExtractorSharp.Data;
using ExtractorSharp.EventArguments;
using ExtractorSharp.Core;

namespace ExtractorSharp.View.Pane {
    public partial class PalattePanel : TabPage {
        private Album Album;
        private Language Language => Language.Default;
        private IConnector Connector => Program.Connector;
        public PalattePanel() {
            InitializeComponent();
            Program.Drawer.ImageChanged += SelectImageChanged;
            combo.SelectedIndexChanged += ColorChanged;
            changeColorItem.Click += ChangeColor;
            changeToCurrentItem.Click += ChangeToCurrentColor;
        }

        private void ChangeColor(object sender, EventArgs e) {
            var dialog = new ColorDialog();
            var items = list.SelectedItems;
            var color = Color.Empty;
            if (items.Count > 0)
                color = (Color)items[0].Tag;
            dialog.Color = color;
            if (dialog.ShowDialog() == DialogResult.OK) {
                color = dialog.Color;
                var indexes = new int[items.Count];
                for (var i=0;i<indexes.Length;i++) {
                    list.SetColor(items[i], color);
                    indexes[i] = items[i].Index;
                }
                Connector.Do("changeColor", Album, Album.TableIndex, indexes, color);
                Album.Refresh();
                Connector.CanvasFlush();
            }
        }

        private void SelectImageChanged(object sender, ImageEntityEventArgs e) {
            Album = e.Album;
            if (Album != null) {
                combo.Items.Clear();
                for (var i = 0; i < Album.Tables.Count; i++) {
                    combo.Items.Add($"{Language["ColorChart"]} - {i}");
                }
                if (Album.TableIndex < Album.Tables.Count) {
                    combo.SelectedIndex = Album.TableIndex;
                }
            }
        }

        private void ColorChanged(object sender,EventArgs e) {
            if (Album != null) {
                Album.TableIndex = combo.SelectedIndex;
                list.Colors = Album.CurrentTable.ToArray();
                Connector.CanvasFlush();
            }
        }

        private void ChangeToCurrentColor(object sender,EventArgs e) {
            if (Album != null) {
                var arr = list.SelectedItems;
                var color = Program.Drawer.Color;
                var index_arr = new int[arr.Count];
                for (var i=0;i<arr.Count;i++) {
                    index_arr[i] = arr[i].Index;
                    list.SetColor(arr[i], color);
                }
                Connector.Do("changeColor", Album, Album.TableIndex, index_arr, color);
            }
        }
    }
}
