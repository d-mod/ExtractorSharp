using System;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Windows.Forms;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Composition.Core;
using ExtractorSharp.Core;
using ExtractorSharp.Core.Model;
using ExtractorSharp.EventArguments;

namespace ExtractorSharp.View.Pane {

    [Export(typeof(DropPage))]
    public partial class PalettePage : DropPage, IPartImportsSatisfiedNotification {

        private Album File;

        [Import]
        private Store Store;

        [Import]
        private Drawer Drawer;

        [Import]
        private Controller Controller;


        [Import]
        private Language Language;

        public void OnImportsSatisfied() {

            InitializeComponent();
            combo.SelectedIndexChanged += PaletteChanged;
            changeColorItem.Click += ChangeColor;
            changeToCurrentItem.Click += ChangeToCurrentColor;
            Controller.CommandChanged += UndoFresh;
            deleteButton.Click += DeletePalette;

            Store.Watch<Album>("/filelist/selected-item", OnFileChanged);
        }

        private void DeletePalette(object sender, EventArgs e) {
            Controller.Do("deletePalette", File);
        }

        private void UndoFresh(object sender, CommandEventArgs e) {
            if(e.Name.Equals("changeColor")) {
                OnFileChanged(File);
            }
        }

        private void ChangeColor(object sender, EventArgs e) {
            var dialog = new ColorDialog();
            var items = list.SelectedItems;
            var color = Color.Empty;
            if(items.Count == 0) {
                return;
            }
            color = (Color)items[0].Tag;
            dialog.Color = color;
            if(dialog.ShowDialog() == DialogResult.OK) {
                color = dialog.Color;
                var indices = new int[items.Count];
                for(var i = 0; i < indices.Length; i++) {
                    list.SetColor(items[i], color);
                    indices[i] = items[i].Index;
                }
                Controller.Do("ChangeColor", new CommandContext(File){
                    { "Indices",indices },
                    { "Color", color }
                });
                File.Refresh();
            }
        }

        private void OnFileChanged(Album value) {
            File = value;
            combo.Items.Clear();
            if(File != null) {
                for(var i = 0; i < File.Palettes.Count; i++) {
                    combo.Items.Add($"{Language["Palette"]} - {i}");
                }
                if(File.PaletteIndex < File.Palettes.Count) {
                    combo.SelectedIndex = File.PaletteIndex;
                }
            }
            PaletteChanged(this, null);
        }

        private void PaletteChanged(object sender, EventArgs e) {
            if(File != null) {
                Controller.Do("SwitchPalette", new CommandContext(File){
                    { "PaletteIndex", combo.SelectedIndex}
                });
                list.Colors = File.CurrentPalette.ToArray();
            } else {
                list.Colors = new Color[0];
            }
        }

        private void ChangeToCurrentColor(object sender, EventArgs e) {
            if(File != null) {
                var arr = list.SelectedItems;
                if(arr.Count == 0) {
                    return;
                }
                var color = Color.Red;//Drawer.Color;
                var indices = new int[arr.Count];
                for(var i = 0; i < arr.Count; i++) {
                    indices[i] = arr[i].Index;
                    list.SetColor(arr[i], color);
                }
                Controller.Do("ChangeColor", new CommandContext(File){
                    { "Indices",indices },
                    { "Color", color }
                });
            }
        }


    }
}