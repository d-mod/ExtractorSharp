using ExtractorSharp.Data;
using ExtractorSharp.EventArguments;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
namespace ExtractorSharp.Component {
    public partial class ESListBox<T> : CheckedListBox {
        public Language Language { set; get; }= Language.Default;
        public Action Deleted;
        public Action Cleared;

        public delegate void ItemDragEventHandler(object sender,ItemDragEventArgs<T> e);
        public event ItemDragEventHandler ItemDraged;
        protected void OnItemDraged(ItemDragEventArgs<T> e) => ItemDraged?.Invoke(this,e);

        public T HoverItem { private set;get; }
        public int HoverIndex { private set; get; }
        public int move_mode;


        public bool CanClear {
            set {
                _canClear = value;
                if (_canClear) {
                    ContextMenuStrip.Items.Add(clearItem);
                } else {
                    ContextMenuStrip.Items.Remove(clearItem);
                }
            }
            get {
                return _canClear;
            }
        }

        private bool _canClear = true;

        public delegate void ItemHoverHandler(object sender, ItemHoverEventArgs e);

        public event ItemHoverHandler ItemHoverChanged;

        protected void OnItemHover(ItemHoverEventArgs e) => ItemHoverChanged?.Invoke(this, e);


        public new int[] CheckedIndices {
            get {
                var indexes = base.CheckedIndices;
                var array = new int[indexes.Count];
                indexes.CopyTo(array, 0);
                if (array.Length > 0) {
                    return array;
                }
                return new int[] { SelectedIndex };
            }
        }

        public new T[] CheckedItems {
            get {
                var array = new T[base.CheckedItems.Count];
                base.CheckedItems.CopyTo(array, 0);
                if (array.Length < 2) {
                    var item = SelectedItem;
                    if (item != null) {
                        array = new T[] { item };
                    }
                }
                return array;
            }
        }

        public new T SelectedItem {
            get {
                return (T)base.SelectedItem;
            }
            set {
                base.SelectedItem = value;
            }
        }

        public T[] AllItems {
            get {
                var array = new T[Items.Count];
                for(var i = 0; i < Items.Count; i++) {
                    array[i] = (T)Items[i];
                }
                return array;
            }
        }


        public ESListBox() {
            InitializeComponent();
            deleteItem.Click += (sender,e)=>Deleted?.Invoke();
            clearItem.Click += (sender, e) => Clear();
            checkAllItem.Click += CheckAll;
            checkAllItem.ShortcutKeys =Keys.Control| Keys.A;
            reverseCheckItem.Click += ReverseCheck;
            reverseCheckItem.ShortcutKeys = Keys.Control | Keys.D;
            deleteItem.ShortcutKeys = Keys.Delete;
            DragOver += ListDragOver;
            DragDrop += ListDragDrop;
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
                he.Item = HoverItem = (T)Items[index];
                OnItemHover(he);
            }
        }
        

        protected override void OnMouseMove(MouseEventArgs e) {
            base.OnMouseMove(e);
            OnMouseHover(e);
            if (move_mode>0) {
                var i = IndexFromPoint(e.Location);
                if (i > -1) {
                    if (ModifierKeys.HasFlag(Keys.Shift)) {
                        SetItemChecked(i, true);
                    }else if (ModifierKeys.HasFlag(Keys.Control)) {
                        SetItemChecked(i, false);
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
            if (Items.Count == 0 || e.Button != MouseButtons.Left) {
                return;
            }
            move_mode = 1;
            if (SelectedIndex < 0|| ModifierKeys != Keys.Alt) {
                return;
            }
            DoDragDrop(SelectedItem, DragDropEffects.Move);
        }



        private void CheckAll(object sender,EventArgs e) {
            CheckAll();
        }

        public void CheckAll() {
            for (var i = 0; i < Items.Count; i++) {
                SetItemChecked(i, true);
            }
        }

        public void UnCheckAll() {
            for (var i = 0; i < Items.Count; i++) {
                SetItemChecked(i, false);
            }
        }

        private void ReverseCheck(object sender,EventArgs e) {
            ReverseCheck();
        }

        public void ReverseCheck() {
            for (var i = 0; i < Items.Count; i++) {
                SetItemChecked(i, !GetItemChecked(i));
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
            Cleared?.Invoke();
        }


        protected void ListDragDrop(object sender, DragEventArgs e) {
            var target = IndexFromPoint(PointToClient(new Point(e.X, e.Y)));
            if (target == SelectedIndex || SelectedIndex < 0 || Items.Count < 0) {
                return;
            }
            target = (target < Items.Count && target > -1) ? target : Items.Count - 1;
            var args = new ItemDragEventArgs<T>();
            args.Index = SelectedIndex;
            args.Target = target;
            OnItemDraged(args);
        }

        protected  void ListDragOver(object sender,DragEventArgs e) => e.Effect = DragDropEffects.Move;

    }
}
