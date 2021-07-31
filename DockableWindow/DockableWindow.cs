using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Aritiafel.Organizations.ElibrarPartFactory
{
    public partial class DockableWindow : Form
    {
        private const int FormEdgeWidth = 10;
        private const int CaptionHeight = 32;

        public DockableWindow()
        {
            InitializeComponent();
        }

        //HTLEFT = 10,
        //HTRIGHT = 11,
        //HTTOP = 12,
        //HTTOPLEFT = 13,
        //HTTOPRIGHT = 14,
        //HTBOTTOM = 15,
        //HTBOTTOMLEFT = 16,
        //HTBOTTOMRIGHT = 17;
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x84)
            {   
                Point location = new Point(m.LParam.ToInt32());
                location = PointToClient(location);
                
                if (location.X >= 0 && location.Y >= 0 && location.X <= FormEdgeWidth && location.Y <= FormEdgeWidth)
                    m.Result = (IntPtr)13;
                else if (location.X >= Width - FormEdgeWidth && location.Y >= 0 && location.X <= Width && location.Y <= FormEdgeWidth)
                    m.Result = (IntPtr)14;
                else if (location.X >= 0 && location.Y >= Height - FormEdgeWidth && location.X <= FormEdgeWidth && location.Y <= Height)
                    m.Result = (IntPtr)16;
                else if (location.X >= Width - FormEdgeWidth && location.Y >= Height - FormEdgeWidth && location.X <= Width && location.Y <= Height)
                    m.Result = (IntPtr)17;
                else if (location.Y < CaptionHeight)
                    m.Result = (IntPtr)2;
                else if (location.X >= 0 && location.X <= Width && location.Y >= 0 && location.Y <= FormEdgeWidth)
                    m.Result = (IntPtr)12;
                else if (location.X >= 0 && location.X <= FormEdgeWidth && location.Y >= 0 && location.Y <= Height)
                    m.Result = (IntPtr)10;
                else if (location.X >= Width - FormEdgeWidth && location.X <= Width && location.Y >= 0 && location.Y <= Height)
                    m.Result = (IntPtr)11;
                else if (location.X >= 0 && location.X <= Width && location.Y >= Height - FormEdgeWidth && location.Y <= Height)
                    m.Result = (IntPtr)15;                
                else
                    base.WndProc(ref m);
                return;
            }
            base.WndProc(ref m);
        }
    }
}
