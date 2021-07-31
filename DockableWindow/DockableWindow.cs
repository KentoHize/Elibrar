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
        private const int ControlButtonSize = 20;

        protected Button CloseButton;
        protected Button FloatButton; // Like Maximize
        protected Button AutoHideButton; //Like Minimize

        public Color CaptionBackColor { get; set; }
        public Color CaptionForeColor { get; set; }

        public DockableWindow()
        {
            
            SetStyle(ControlStyles.ResizeRedraw, true);
            CaptionBackColor = SystemColors.MenuHighlight;
            CaptionForeColor = Color.White;

            //加3個按鈕
            //Close
            CloseButton = new Button
            {
                Name = "___closeButton",
                Text = "C",
                Width = ControlButtonSize,
                Height = ControlButtonSize,
                FlatStyle = FlatStyle.Flat,
                BackColor = CaptionBackColor

            };
            CloseButton.FlatAppearance.BorderSize = 0;
            CloseButton.Click += CloseButton_Click;

            FloatButton = new Button
            {
                Name = "___floatButton",
                Text = "F",
                Width = ControlButtonSize,
                Height = ControlButtonSize,
                FlatStyle = FlatStyle.Flat,
                BackColor = CaptionBackColor
            };
            FloatButton.FlatAppearance.BorderSize = 0;
            FloatButton.Click += FloatButton_Click;

            AutoHideButton = new Button
            {
                Name = "__autoHideButton",
                Text = "A",
                Width = ControlButtonSize,
                Height = ControlButtonSize,
                FlatStyle = FlatStyle.Flat,
                BackColor = CaptionBackColor
            };
            AutoHideButton.FlatAppearance.BorderSize = 0;
            AutoHideButton.Click += AutoHideButton_Click;

            Controls.Add(CloseButton);
            Controls.Add(FloatButton);
            Controls.Add(AutoHideButton);
            SetControlBoxButtonPosition();
            InitializeComponent();

        }

        private void AutoHideButton_Click(object sender, EventArgs e)
            => WindowState = FormWindowState.Minimized;
        private void FloatButton_Click(object sender, EventArgs e)
            => WindowState = WindowState == FormWindowState.Normal ? FormWindowState.Maximized : FormWindowState.Normal;
        private void CloseButton_Click(object sender, EventArgs e)
            => Close();
        protected void SetControlBoxButtonPosition()
        {
            CloseButton.Left = (Width - FormEdgeWidth - CloseButton.Width);
            CloseButton.Top = (CaptionHeight - ControlButtonSize) / 2;

            FloatButton.Left = (Width - FormEdgeWidth * 2 - CloseButton.Width - FloatButton.Width);
            FloatButton.Top = (CaptionHeight - ControlButtonSize) / 2;

            AutoHideButton.Left = (Width - FormEdgeWidth * 3 - CloseButton.Width - FloatButton.Width - AutoHideButton.Width);
            AutoHideButton.Top = (CaptionHeight - ControlButtonSize) / 2;
        }

        protected override void OnResize(EventArgs e)
        {
            SetControlBoxButtonPosition();
            base.OnResize(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {   
            base.OnPaint(e);
            //Rectangle rc = new Rectangle(ClientSize.Width - FormEdgeWidth, ClientSize.Height - FormEdgeWidth, FormEdgeWidth, FormEdgeWidth);
            //ControlPaint.DrawSizeGrip(e.Graphics, BackColor, rc);
            Rectangle rc = new Rectangle(0, 0, ClientSize.Width, CaptionHeight);
            //ControlPaint.Dr .DrawSizeGrip(e.Graphics, CaptionColor, rc);            
            e.Graphics.FillRectangle(new SolidBrush(CaptionBackColor), rc);
            TextRenderer.DrawText(e.Graphics, Text, Font, new Point(FormEdgeWidth, (CaptionHeight - Font.Height) / 2), CaptionForeColor);
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
                else if (location.X >= 0 && location.X <= Width && location.Y >= 0 && location.Y <= FormEdgeWidth)
                    m.Result = (IntPtr)12;
                else if (location.Y < CaptionHeight)
                    m.Result = (IntPtr)2;
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
