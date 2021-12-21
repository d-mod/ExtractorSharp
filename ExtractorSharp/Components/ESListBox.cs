using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ExtractorSharp.Core;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Core.Providers;
using ExtractorSharp.EventArguments;

namespace ExtractorSharp.Components {
    public partial class ESListBox<T> : CheckedListBox, INotifyPropertyChanged {

        public delegate void ItemEventHandler(object sender, ItemEventArgs e);

        public delegate void ItemHoverHandler(object sender, ItemHoverEventArgs e);

        public int move_mode;

        private Language _language = Language.Empty;

        public Language Language {
            set {
                this._language = value;
                this.FormatInfo = new LanguageProvider(value);
                OnPropertyChanged("Language");
            }
            get {
                return _language;
            }
        }


        public ESListBox() {
            InitializeComponent();
            ContextMenuStrip.Opening += ShowMenuStrip;

            deleteItem.Click += DeleteItem;
            clearItem.Click += (o, e) => Clear();
            checkAllItem.Click += (o, e) => CheckAll();
            checkAllItem.ShortcutKeys = Keys.Control | Keys.A;
            reverseCheckItem.Click += ReverseCheck;
            reverseCheckItem.ShortcutKeys = Keys.Control | Keys.D;
            deleteItem.ShortcutKeys = Keys.Delete;
            unCheckAllItem.Click += (o, e) => UnCheckAll();
            unCheckAllItem.ShortcutKeys = Keys.Control | Keys.F;
            SelectedIndexChanged += OnSelectedIndexChanged;
            DragOver += ListDragOver;
            DragDrop += ListDragDrop;
        }


        private void OnSelectedIndexChanged(object sender, EventArgs e) {
            OnPropertyChanged("SelectedIndex");
            OnPropertyChanged("SelectedIndices");
            OnPropertyChanged("SelectedItem");
            OnPropertyChanged("SelectedValue");
        }



        public T HoverItem { private set; get; }

        public int HoverIndex { private set; get; }


        public bool CanClear { set; get; } = true;

        public bool CanDelete { set; get; } = true;

        public int[] SelectIndices {
            get {
                var indexes = CheckedIndices;
                var array = new int[indexes.Count];
                indexes.CopyTo(array, 0);
                if(array.Length > 0 || SelectedIndex == -1) {
                    return array;
                }
                return new[] { SelectedIndex };
            }
        }

        public T[] SelectItems {
            get {
                var array = new T[CheckedItems.Count];
                CheckedItems.CopyTo(array, 0);
                if(array.Length > 0 || SelectedItem == null) {
                    return array;
                }
                return new[] { SelectedItem };
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
            if(index != HoverIndex && index > -1 && index < Items.Count) {
                var he = new ItemHoverEventArgs {
                    LastIndex = HoverIndex,
                    Index = HoverIndex = index,
                    LastItem = HoverItem,
                    Item = HoverItem = (T)Items[index]
                };
                OnItemHover(he);
            }
        }


        protected override void OnMouseMove(MouseEventArgs e) {
            base.OnMouseMove(e);
            OnMouseHover(e);
            if(move_mode > 0) {
                var i = IndexFromPoint(e.Location);
                if(i > -1) {
                    if(ModifierKeys.HasFlag(Keys.Control) && ModifierKeys.HasFlag(Keys.Shift)) {
                        SetItemChecked(i, false);
                    } else if(ModifierKeys.HasFlag(Keys.Shift)) {
                        SetItemChecked(i, true);
                    }
                }
            }
        }

        protected override void OnMouseLeave(EventArgs e) {
            move_mode = 0;
            var he = new ItemHoverEventArgs {
                Item = null,
                Index = -1
            };
            OnItemHover(he);
            base.OnMouseLeave(e);
        }


        protected override void OnMouseDown(MouseEventArgs e) {
            if(Items.Count == 0 || e.Button != MouseButtons.Left) {
                return;
            }
            move_mode = 1;
            if(SelectedIndex < 0 || ModifierKeys != Keys.Alt) {
                return;
            }
            DoDragDrop(SelectedItem, DragDropEffects.Move);
        }

        protected override void OnSelectedIndexChanged(EventArgs e) {
            base.OnSelectedIndexChanged(e);
            OnPropertyChanged("SelectedItem");
        }



        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private void ShowMenuStrip(object sender, EventArgs e) {
            checkAllItem.Text = Language["CheckAll"];
            reverseCheckItem.Text = Language["ReverseCheck"];
            unCheckAllItem.Text = Language["UnCheckAll"];
            deleteItem.Text = Language["DeleteCheck"];
            clearItem.Text = Language["ClearList"];
            deleteItem.Visible = CanDelete;
            clearItem.Visible = CanClear;
        }

        private void DeleteItem(object sender, EventArgs e) {
            ItemDeleted?.Invoke(sender, new ItemEventArgs { Indices = SelectIndices });
        }


        public void CheckAll() {
            for(var i = 0; i < Items.Count; i++) {
                SetItemChecked(i, true);
            }
        }

        public void UnCheckAll() {
            for(var i = 0; i < Items.Count; i++) {
                SetItemChecked(i, false);
            }
        }

        private void ReverseCheck(object sender, EventArgs e) {
            ReverseCheck();
        }

        public void ReverseCheck() {
            for(var i = 0; i < Items.Count; i++) {
                SetItemChecked(i, !GetItemChecked(i));
            }
        }


        /// <summary>
        /// 选择符合指定条件的项
        /// </summary>
        /// <param name="predicate"></param>
        public void CheckWith(Predicate<T> predicate) {
            for(var i = 0; i < Items.Count; i++) {
                if(predicate.Invoke((T)Items[i])) {
                    SetItemChecked(i, true);
                }
            }
        }

        protected override void OnDragEnter(DragEventArgs e) {
            if(e.Data.GetDataPresent(typeof(T))) {
                e.Effect = DragDropEffects.Move;
            } else {
                e.Effect = DragDropEffects.None;
            }
        }

        public void Clear() {
            if(this.InvokeRequired) {
                this.Invoke(new MethodInvoker(this.Clear));
                return;
            }
            Items.Clear();
            OnItemCleared(new ItemEventArgs());
        }


        private void ListDragDrop(object sender, DragEventArgs e) {
            if(!e.Data.GetDataPresent(typeof(T))) {
                return;
            }
            var target = IndexFromPoint(PointToClient(new Point(e.X, e.Y)));
            if(target == SelectedIndex || SelectedIndex < 0 || Items.Count < 0) {
                return;
            }
            target = target < Items.Count && target > -1 ? target : Items.Count - 1;
            var args = new ItemEventArgs {
                Index = SelectedIndex,
                Target = target
            };
            OnItemDraged(args);
        }

        private void ListDragOver(object sender, DragEventArgs e) {
            e.Effect = DragDropEffects.Move;
        }
    }
}