using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace CamMonitor
{
    public partial class ContanerItem : UserControl
    {
        private bool isPressed;
        Point buffer;
        public ContanerItem()
        {
            InitializeComponent();
            sizeBuf = MinimumSize;
        }
        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
            isPressed = true;
            buffer = e.Location;
        }

        private void label1_MouseUp(object sender, MouseEventArgs e)
        {
            isPressed = false;
        }
        private void label1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isPressed) return;
            Size += new Size(e.X - buffer.X, e.Y - buffer.Y);
        }
        private Size sizeBuf;
        private void label1_DoubleClick(object sender, EventArgs e)
        {
            if (Size == MinimumSize) Size = sizeBuf;
            else
            {
                sizeBuf = Size;
                Size = MinimumSize;
            }
        }

        private void label2_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isPressed) return;
            Width += e.X - buffer.X;
        }

        private void label3_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isPressed) return;
            Height += e.Y - buffer.Y;
        }
    }
}
