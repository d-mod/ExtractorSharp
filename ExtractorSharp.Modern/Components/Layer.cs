using System.ComponentModel.Composition;
using System.Windows.Controls;
using System.Windows.Media;
using ExtractorSharp.Composition.Core;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Components {

    [Export]
    internal class Layer : Image, IPartImportsSatisfiedNotification {


        private ImageSource ImageSource { get; }

        [Import]
        private Store Store { set; get; }

        public void OnImportsSatisfied() {
            this.Store.Watch<Sprite>("/sprites/selected-item", sprite => {

            });
        }

    }
}
