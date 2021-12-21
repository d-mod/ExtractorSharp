using System;
using System.ComponentModel.Composition;
using ExtractorSharp.Composition.Config;
using ExtractorSharp.Composition.Core;
using ExtractorSharp.Core;
using MahApps.Metro.Controls;

namespace ExtractorSharp.Components {
    public class BaseWindow : MetroWindow {

        [Import]
        protected new Language Language { set; get; }

        [Import]
        protected Store Store { set; get; }

        [Import]
        protected Controller Controller { set; get; }

        [Import]
        protected IConfig Config { set; get; }

        [Import]
        protected Messager Messager { set; get; }

        public BaseWindow() {
            this.ResizeMode = System.Windows.ResizeMode.CanMinimize;
        }

        protected override void OnStateChanged(EventArgs e) {

        }

    }
}
