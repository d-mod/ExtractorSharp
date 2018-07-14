using System;
using System.Drawing;
using System.Windows.Forms;
using ExtractorSharp.Core.Composition;
using ExtractorSharp.Core.Model;
using ExtractorSharp.EventArguments;

namespace ExtractorSharp.View.Pane {
    public partial class PalattePanel : TabPage {
        private Album Album;

        public PalattePanel() {
            InitializeComponent();
            Program.Drawer.PalatteChanged += SelectImageChanged;
            combo.SelectedIndexChanged += ColorChanged;
            changeColorItem.Click += ChangeColor;
            changeToCurrentItem.Click += ChangeToCurrentColor;
            Program.Controller.CommandUndid += UndoFresh;
            Program.Controller.CommandRedid += UndoFresh;
        }

        private Language Language => Language.Default;
        private IConnector Connector => Program.Connector;

        private void UndoFresh(object sender, CommandEventArgs e) {
            if (e.Name.Equals("changeColor")) {
                SelectImageChanged(sender, new FileEventArgs {
                    Album = Album
                });
            }
        }

        private void ChangeColor(object sender, EventArgs e) {
            var dialog = new ColorDialog();
            var items = list.SelectedItems;
            var color = Color.Empty;
            if (items.Count > 0) {
                color = (Color) items[0].Tag;
            }
            dialog.Color = color;
            if (dialog.ShowDialog() == DialogResult.OK) {
                color = dialog.Color;
                var indexes = new int[items.Count];
                for (var i = 0; i < indexes.Length; i++) {
                    list.SetColor(items[i], color);
                    indexes[i] = items[i].Index;
                }
                Connector.Do("changeColor", Album, Album.TableIndex, indexes, color);
                Album.Refresh();
                Connector.CanvasFlush();
            }
        }

        private void SelectImageChanged(object sender, FileEventArgs e) {

            Album = e.Album;
            combo.Items.Clear();
            if (Album != null) {
                for (var i = 0; i < Album.Tables.Count; i++) {
                    combo.Items.Add($"{Language["Palette"]} - {i}");
                }
                if (Album.TableIndex < Album.Tables.Count) {
                    combo.SelectedIndex = Album.TableIndex;
                }
            }
            ColorChanged(sender, e);
        }

        private void ColorChanged(object sender, EventArgs e) {
            if (Album != null) {
                Album.TableIndex = combo.SelectedIndex;
                list.Colors = Album.CurrentTable.ToArray();
                Connector.CanvasFlush();
            } else {
                list.Colors = new Color[0];
            }
        }

        private void ChangeToCurrentColor(object sender, EventArgs e) {
            if (Album != null) {
                var arr = list.SelectedItems;
                var color = Program.Drawer.Color;
                var index_arr = new int[arr.Count];
                for (var i = 0; i < arr.Count; i++) {
                    index_arr[i] = arr[i].Index;
                    list.SetColor(arr[i], color);
                }
                Connector.Do("changeColor", Album, Album.TableIndex, index_arr, color);
            }
        }
    }
}