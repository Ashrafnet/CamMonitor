using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace CamMonitor
{
    public partial class LayOutContaner : UserControl
    {
        public LayOutContaner()
        {
            InitializeComponent();
        }
        public ControlCollection controls
        {
            get { return flowLayoutPanel1.Controls; }
        }

        private void flowLayoutPanel1_ControlAdded(object sender, ControlEventArgs e)
        {
            e.Control.MouseDown += new MouseEventHandler(Control_MouseDown);
            e.Control.MouseUp += new MouseEventHandler(Control_MouseUp);
            e.Control.MouseMove += new MouseEventHandler(Control_MouseMove);
        }
        void Control_MouseMove(object sender, MouseEventArgs e)
        {
            label1.Visible = isPressed;
            if (isPressed)
            {
                flowLayoutPanel1.Cursor = Cursors.Hand;
                Control c = GetAtMousePostion();
                if (c != null)
                {
                    label1.Height = c.Height;
                    label1.Top = c.Top;
                    label1.Left = c.Left - 2;
                }
            }
            else flowLayoutPanel1.Cursor = Cursors.Default;
        }
        bool isPressed;
        void Control_MouseDown(object sender, MouseEventArgs e)
        {
            isPressed = true;
        }
        public Control GetAtMousePostion()
        {
            return GetAtPostion(MousePosition);
        }
        public Control GetAtPostion(Point toScreen)
        {
            Point to = flowLayoutPanel1.PointToClient(toScreen);
            return flowLayoutPanel1.GetChildAtPoint(to);
        }
        void Control_MouseUp(object sender, MouseEventArgs e)
        {
            label1.Visible = false;
            if (isPressed)
            {
                Control control = GetAtMousePostion();
                int index1 = controls.IndexOf(control);
                int index2 = controls.IndexOf((Control)sender);
                controls.SetChildIndex((Control)sender, index1);
                if(control!=null)
                controls.SetChildIndex(control, index2);
            }
            isPressed = false;
        }
        private void flowLayoutPanel1_ControlRemoved(object sender, ControlEventArgs e)
        {
            e.Control.MouseDown -= new MouseEventHandler(Control_MouseDown);
            e.Control.MouseUp -= new MouseEventHandler(Control_MouseUp);
            e.Control.MouseMove -= new MouseEventHandler(Control_MouseMove);
        }
    }
}
