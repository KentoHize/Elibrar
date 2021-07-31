﻿using System;
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
        private const int ControlButtonsMarginRight = 5;
        private const int ControlButtonsInterval = 2;
        private const int FormEdgeWidth = 10;
        private const int CaptionHeight = 28;
        private const int ControlButtonSize = 26;

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
                Text = "X",
                Font = new Font(Font.FontFamily, 7, FontStyle.Regular),
                Width = ControlButtonSize,
                Height = ControlButtonSize,
                FlatStyle = FlatStyle.Flat,
                BackColor = CaptionBackColor,
                ForeColor = CaptionForeColor,
                TabStop = false
            };
            CloseButton.FlatAppearance.BorderSize = 0;            
            CloseButton.Click += CloseButton_Click;

            FloatButton = new Button
            {
                Name = "___floatButton",
                Text = "🔲",
                Font = new Font(Font.FontFamily, 7, FontStyle.Regular),
                Width = ControlButtonSize,
                Height = ControlButtonSize,
                FlatStyle = FlatStyle.Flat,
                BackColor = CaptionBackColor,
                ForeColor = CaptionForeColor,
                TabStop = false
            };
            FloatButton.FlatAppearance.BorderSize = 0;
            FloatButton.Click += FloatButton_Click;

            AutoHideButton = new Button
            {
                Name = "__autoHideButton",
                Text = "─",
                Font = new Font(Font.FontFamily, 7, FontStyle.Regular),
                Width = ControlButtonSize,
                Height = ControlButtonSize,
                FlatStyle = FlatStyle.Flat,
                BackColor = CaptionBackColor,
                ForeColor = CaptionForeColor,
                TabStop = false
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
            CloseButton.Visible = ControlBox;
            FloatButton.Visible = MaximizeBox;
            AutoHideButton.Visible = MinimizeBox;

            if (Width >= CloseButton.Width + FloatButton.Width + AutoHideButton.Width + ControlButtonsInterval * 3 + ControlButtonsMarginRight + 30)
            {
                CloseButton.Left = (Width - CloseButton.Width - ControlButtonsMarginRight);
                CloseButton.Top = (CaptionHeight - ControlButtonSize) / 2;

                FloatButton.Left = (Width - CloseButton.Width - FloatButton.Width - ControlButtonsMarginRight - ControlButtonsInterval);
                FloatButton.Top = (CaptionHeight - ControlButtonSize) / 2;

                AutoHideButton.Left = (Width - CloseButton.Width - FloatButton.Width - AutoHideButton.Width - ControlButtonsMarginRight - ControlButtonsInterval * 2);
                AutoHideButton.Top = (CaptionHeight - ControlButtonSize) / 2;
            }
            else
            {
                CloseButton.Visible =
                FloatButton.Visible = 
                AutoHideButton.Visible = false;
            }
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
            Rectangle rc = new Rectangle(0, 0, ClientSize.Width, CaptionHeight);
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
