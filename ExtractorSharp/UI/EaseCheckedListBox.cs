using ExtractorSharp.Data;
using ExtractorSharp.EventArguments;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
namespace ExtractorSharp.EaseUI {
    public partial class EaseListBox<T> : CheckedListBox {
        public Language Language = Language.Default;
        public Action Deleted;
        public Action Cleared;
        public Action Draged;
        public T DragItem;
        public int DragTarget { private set; get; }
        public T HoverItem { private set;get; }
        public int HoverIndex { private set; get; }


        public bool CanClear {
            set {
                if (value) 
                    ContextMenuStrip.Items.Add(clearItem);
                else 
                    ContextMenuStrip.Items.Remove(clearItem);             
            }
        }

        public delegate void ItemHoverHandler(object sender, ItemHoverEventArgs e);

        public event ItemHoverHandler ItemHoverChanged;

        protected void OnItemHover(ItemHoverEventArgs e) => ItemHoverChanged?.Invoke(this, e);


        public EaseListBox() {
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
            HoverItem = default(T);
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
        }

        protected override void OnMouseLeave(EventArgs e) {
            var he = new ItemHoverEventArgs();
            he.Item = null;
            he.Index = -1;
            OnItemHover(he);
            base.OnMouseLeave(e);
        }




        private void CheckAll(object sender,EventArgs e) {
            CheckAll();
        }

        public void CheckAll() {
            for (var i = 0; i < Items.Count; i++)
                SetItemChecked(i, true);
        }

        private void ReverseCheck(object sender,EventArgs e) {
            ReverseCheck();
        }

        public void ReverseCheck() {
            for (var i = 0; i < Items.Count; i++)
                SetItemChecked(i, !GetItemChecked(i));
        }
        protected override void OnDragEnter(DragEventArgs e) {
            if (e.Data.GetDataPresent(DataFormats.Serializable)) 
                e.Effect = DragDropEffects.Move;
             else
                e.Effect = DragDropEffects.None;
        }

        public void Clear() {
            Items.Clear();
            Cleared?.Invoke();
        }

        protected override void OnMouseDown(MouseEventArgs e) {
            if (Items.Count == 0 || e.Button != MouseButtons.Left || SelectedIndex < 0 || ModifierKeys != Keys.Alt)
                return;
            DoDragDrop(SelectedItem, DragDropEffects.Move);
        }


        protected  void ListDragDrop(object sender,DragEventArgs e) {
            DragItem = (T)SelectedItem;
            DragTarget = IndexFromPoint(PointToClient(new Point(e.X, e.Y)));
            if (DragTarget == SelectedIndex)
                return;
            if (DragItem != null && Items.Count > -1) {
                DragTarget = (DragTarget < Items.Count && DragTarget > -1) ? DragTarget : Items.Count - 1;
                Items.Remove(DragItem);
                Items.Insert(DragTarget, DragItem);
                Draged?.Invoke();
                SelectedIndex = DragTarget;
            }
        }

        protected  void ListDragOver(object sender,DragEventArgs e) => e.Effect = DragDropEffects.Move;

    }
}
