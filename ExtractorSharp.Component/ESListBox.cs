using System;
using System.Drawing;
using System.Windows.Forms;
using ExtractorSharp.Component.EventArguments;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Component {
    public partial class ESListBox<T> : CheckedListBox {
        public delegate void ItemEventHandler(object sender, ItemEventArgs e);

        public delegate void ItemHoverHandler(object sender, ItemHoverEventArgs e);

        private bool _canClear = true;

        private bool _canDelete = true;
        public int move_mode;


        public ESListBox() {
            InitializeComponent();
            deleteItem.Click += DeleteItem;
            clearItem.Click += (o, e) => Clear();
            checkAllItem.Click += (o, e) => CheckAll();
            checkAllItem.ShortcutKeys = Keys.Control | Keys.A;
            reverseCheckItem.Click += ReverseCheck;
            reverseCheckItem.ShortcutKeys = Keys.Control | Keys.D;
            deleteItem.ShortcutKeys = Keys.Delete;
            unCheckAllItem.Click += (o, e) => UnCheckAll();
            unCheckAllItem.ShortcutKeys = Keys.Control | Keys.F;
            DragOver += ListDragOver;
            DragDrop += ListDragDrop;
        }

        public Language Language { set; get; } = Language.Default;

        public T HoverItem { private set; get; }
        public int HoverIndex { private set; get; }


        public bool CanClear {
            set {
                _canClear = value;
                if (_canClear) {
                    ContextMenuStrip.Items.Add(clearItem);
                } else {
                    ContextMenuStrip.Items.Remove(clearItem);
                }
            }
            get => _canClear;
        }

        public bool CanDelete {
            set {
                _canDelete = value;
                if (_canDelete) {
                    ContextMenuStrip.Items.Add(deleteItem);
                } else {
                    ContextMenuStrip.Items.Remove(deleteItem);
                }
            }
            get => _canDelete;
        }


        public int[] SelectIndexes {
            get {
                var indexes = CheckedIndices;
                var array = new int[indexes.Count];
                indexes.CopyTo(array, 0);
                if (array.Length > 0 || SelectedIndex == -1) return array;
                return new[] {SelectedIndex};
            }
        }

        public T[] SelectItems {
            get {
                var array = new T[CheckedItems.Count];
                CheckedItems.CopyTo(array, 0);
                if (array.Length > 0 || SelectedItem == null) return array;
                return new[] {SelectedItem};
            }
        }

        public new T SelectedItem {
            get => (T) base.SelectedItem;
            set => base.SelectedItem = value;
        }

        public T[] AllItems {
            get {
                var array = new T[Items.Count];
                for (var i = 0; i < Items.Count; i++) array[i] = (T) Items[i];
                return array;
            }
        }

        public event ItemEventHandler ItemDraged;
        public event ItemEventHandler ItemDeleted;
        public event ItemEventHandler ItemCleared;

        protected void OnItemDeleted(ItemEventArgs e) {
            ItemDeleted?.Invoke(this, e);
        }

        protected void OnItemDraged(ItemEventArgs e) {
            ItemDraged?.Invoke(this, e);
        }

        protected void OnItemCleared(ItemEventArgs e) {
            ItemCleared?.Invoke(this, e);
        }

        public event ItemHoverHandler ItemHoverChanged;

        protected void OnItemHover(ItemHoverEventArgs e) {
            ItemHoverChanged?.Invoke(this, e);
        }


        protected override void OnMouseHover(EventArgs e) {
            base.OnMouseHover(e);
            HoverItem = default;
            var point = PointToClient(Cursor.Position);
            var index = IndexFromPoint(point);
            if (index != HoverIndex && index > -1 && index < Items.Count) {
                var he = new ItemHoverEventArgs();
                he.LastIndex = HoverIndex;
                he.Index = HoverIndex = index;
                he.LastItem = HoverItem;
                he.Item = HoverItem = (T) Items[index];
                OnItemHover(he);
            }
        }


        protected override void OnMouseMove(MouseEventArgs e) {
            base.OnMouseMove(e);
            OnMouseHover(e);
            if (move_mode > 0) {
                var i = IndexFromPoint(e.Location);
                if (i > -1) {
                    if (ModifierKeys.HasFlag(Keys.Control) && ModifierKeys.HasFlag(Keys.Shift)) {
                        SetItemChecked(i, false);
                    } else if (ModifierKeys.HasFlag(Keys.Shift)) {
                        SetItemChecked(i, true);
                    }
                }
            }
        }

        protected override void OnMouseLeave(EventArgs e) {
            move_mode = 0;
            var he = new ItemHoverEventArgs();
            he.Item = null;
            he.Index = -1;
            OnItemHover(he);
            base.OnMouseLeave(e);
        }


        protected override void OnMouseDown(MouseEventArgs e) {
            if (Items.Count == 0 || e.Button != MouseButtons.Left) return;
            move_mode = 1;
            if (SelectedIndex < 0 || ModifierKeys != Keys.Alt) return;
            DoDragDrop(SelectedItem, DragDropEffects.Move);
        }

        private void DeleteItem(object sender, EventArgs e) {
            ItemDeleted?.Invoke(sender, new ItemEventArgs {Indices = SelectIndexes});
        }


        public void CheckAll() {
            for (var i = 0; i < Items.Count; i++) SetItemChecked(i, true);
        }

        public void UnCheckAll() {
            for (var i = 0; i < Items.Count; i++) SetItemChecked(i, false);
        }

        private void ReverseCheck(object sender, EventArgs e) {
            ReverseCheck();
        }

        public void ReverseCheck() {
            for (var i = 0; i < Items.Count; i++) SetItemChecked(i, !GetItemChecked(i));
        }


        public void Filter(Predicate<T> predicate) {
            for (var i = 0; i < Items.Count; i++) {
                if (predicate.Invoke((T) Items[i])) {
                    SetItemChecked(i, true);
                }
            }
        }

        protected override void OnDragEnter(DragEventArgs e) {
            if (e.Data.GetDataPresent(DataFormats.Serializable)) {
                e.Effect = DragDropEffects.Move;
            } else {
                e.Effect = DragDropEffects.None;
            }
        }

        public void Clear() {
            Items.Clear();
            OnItemCleared(new ItemEventArgs());
        }


        private void ListDragDrop(object sender, DragEventArgs e) {
            if (!e.Data.GetDataPresent(DataFormats.Serializable)){
                return;
            }
                var target = IndexFromPoint(PointToClient(new Point(e.X, e.Y)));
            if (target == SelectedIndex || SelectedIndex < 0 || Items.Count < 0) return;
            target = target < Items.Count && target > -1 ? target : Items.Count - 1;
            var args = new ItemEventArgs();
            args.Index = SelectedIndex;
            args.Target = target;
            OnItemDraged(args);
        }

        private void ListDragOver(object sender, DragEventArgs e) {
            e.Effect = DragDropEffects.Move;
        }
    }
}