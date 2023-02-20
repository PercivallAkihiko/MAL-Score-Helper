using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tier_List_3
{
    internal class DragDropFlowLayoutPanel : FlowLayoutPanel
    {
        public DragDropFlowLayoutPanel()
        {
            AllowDrop = true;
        }

        [DefaultValue(true)]
        public override bool AllowDrop
        {
            get => base.AllowDrop;
            set => base.AllowDrop = value;
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);

            if (e.Control is DragDropControl)
            {
                e.Control.DragOver += OnControlDragOver;
                e.Control.DragDrop += OnControlDragDrop;
                e.Control.MouseDown += OnControlMouseDown;
            }
        }

        protected override void OnControlRemoved(ControlEventArgs e)
        {
            base.OnControlRemoved(e);

            e.Control.DragOver -= OnControlDragOver;
            e.Control.DragDrop -= OnControlDragDrop;
            e.Control.MouseDown -= OnControlMouseDown;
        }

        protected override void OnDragEnter(DragEventArgs e)
        {
            base.OnDragEnter(e);

            if (e.Data.GetDataPresent(typeof(DragDropControl)))
                e.Effect = DragDropEffects.Move;
        }

        protected override void OnDragOver(DragEventArgs e)
        {
            base.OnDragOver(e);

            if (e.Data.GetDataPresent(typeof(DragDropControl)))
                e.Effect = DragDropEffects.Move;
        }

        protected override void OnDragDrop(DragEventArgs e)
        {
            base.OnDragDrop(e);
            DropControl(e);
        }

        private void OnControlDragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetData(typeof(DragDropControl)) is DragDropControl ddc)
            {
                var p = PointToClient(new Point(e.X, e.Y));

                if (GetChildAtPoint(p) == ddc)
                    e.Effect = DragDropEffects.None;
                else
                    e.Effect = DragDropEffects.Move;
            }
        }

        private void OnControlDragDrop(object sender, DragEventArgs e)
        {
            DropControl(e);
        }

        private void OnControlMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var control = sender as DragDropControl;
                DoDragDrop(control, DragDropEffects.Move);
            }
        }

        private void DropControl(DragEventArgs e)
        {
            if (e.Data.GetData(typeof(DragDropControl)) is DragDropControl ddc)
            {
                var p = PointToClient(new Point(e.X, e.Y));
                var child = GetChildAtPoint(p);
                var index = child == null
                    ? Controls.Count
                    : Controls.GetChildIndex(child);

                ddc.Parent = this;
                Controls.SetChildIndex(ddc, index);
            }
        }
    }
}
