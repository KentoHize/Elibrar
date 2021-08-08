using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Aritiafel.Organizations.ElibrarPartFactory
{
    public partial class DockableWindow : Form
    {
        private const int ControlButtonsMarginRight = 5;
        private const int ControlButtonsInterval = 2;
        private const int WindowEdgeWidth = 10;
        private const int CaptionHeight = 28;
        private const int ControlButtonSize = 26;
        private const int DockIconMarginToEdge = 20;
        private const int DockIconHeight = 40;
        private const int DockIconWidth = 40;
        //private const int DockIconWindowHeight = 20;
        //private const int DockIconWindowWidth = 16;

        protected Button CloseButton;
        protected Button FloatButton; // Like Maximize
        protected Button AutoHideButton; //Like Minimize

        protected DockBar[] _OwnerDockBars;
        protected Form _PaintingForm;
        protected Rectangle[] DockIconRectangles;        
        public Rectangle PaintingAreaRectangle => _PaintingForm.ClientRectangle;
        public DockBar ParentDockBar { get; set; }
        public Color CaptionBackColor { get; set; }
        public Color CaptionForeColor { get; set; }

        public DockableWindow()
        {
            SetStyle(ControlStyles.ResizeRedraw, true);
            CaptionBackColor = SystemColors.MenuHighlight;
            CaptionForeColor = Color.White;
            ParentDockBar = null;
            _OwnerDockBars = new DockBar[4];

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
                TabStop = false,
            };
            AutoHideButton.FlatAppearance.BorderSize = 0;
            AutoHideButton.Click += AutoHideButton_Click;
            SetControlBoxButtonPosition();

            _PaintingForm = new Form();            
            _PaintingForm.BackColor = Color.FromArgb(1, 1, 1);
            _PaintingForm.FormBorderStyle = FormBorderStyle.None;
            _PaintingForm.TransparencyKey = _PaintingForm.BackColor;
            _PaintingForm.StartPosition = FormStartPosition.Manual;
            _PaintingForm.TopMost = true;
            _PaintingForm.Paint += _PaintingForm_Paint;
            _PaintingForm.MouseMove += _PaintingForm_MouseMove;

            InitializeComponent();
        }

        private void _PaintingForm_MouseMove(object sender, MouseEventArgs e)
        {
            _PaintingForm.Visible = false;            
            for (int i = 0; i < 4; i++)
                if (DockIconRectangles[i].Contains(e.X, e.Y))
                    _OwnerDockBars[i].AddWindow(this);            
        }

        protected void PaintDockIcon(Graphics g, int index)
        {   
            g.FillRectangle(new SolidBrush(Color.Transparent), DockIconRectangles[index]);
            g.DrawRectangle(new Pen(Color.Gray), DockIconRectangles[index]);
            g.FillRectangle(new SolidBrush(Color.White), DockIconRectangles[index].X + 4, DockIconRectangles[index].Y + 4, DockIconRectangles[index].Width - 8, DockIconRectangles[index].Height - 8);            
            if (index == 0)
            {
                g.FillRectangle(new SolidBrush(CaptionBackColor), DockIconRectangles[index].X + 6, DockIconRectangles[index].Y + 6, DockIconRectangles[index].Width / 2 - 6, 4);
                g.DrawRectangle(new Pen(CaptionBackColor), DockIconRectangles[index].X + 6, DockIconRectangles[index].Y + 6, DockIconRectangles[index].Width / 2 - 6, DockIconRectangles[index].Height - 12);
                g.FillPolygon(new SolidBrush(Color.Black), new Point[] {
                new Point(DockIconRectangles[index].X + DockIconRectangles[index].Width / 2 + 8,
                    DockIconRectangles[index].Y + DockIconRectangles[index].Height / 2),
                new Point(DockIconRectangles[index].X + DockIconRectangles[index].Width / 2 + 12,
                    DockIconRectangles[index].Y + DockIconRectangles[index].Height / 2 - 4),
                new Point(DockIconRectangles[index].X + DockIconRectangles[index].Width / 2 + 12,
                    DockIconRectangles[index].Y + DockIconRectangles[index].Height / 2 + 4) });
            }

            else if (index == 1)
            {
                g.FillRectangle(new SolidBrush(CaptionBackColor), DockIconRectangles[index].X + 6, DockIconRectangles[index].Y + 6, DockIconRectangles[index].Width - 12, 4);
                g.DrawRectangle(new Pen(CaptionBackColor), DockIconRectangles[index].X + 6, DockIconRectangles[index].Y + 6, DockIconRectangles[index].Width - 12, DockIconRectangles[index].Height / 2 - 6);
                g.FillPolygon(new SolidBrush(Color.Black), new Point[] {
                new Point(DockIconRectangles[index].X + DockIconRectangles[index].Width / 2,
                    DockIconRectangles[index].Y + DockIconRectangles[index].Height / 2 + 7), //Check
                new Point(DockIconRectangles[index].X + DockIconRectangles[index].Width / 2 - 4,
                    DockIconRectangles[index].Y + DockIconRectangles[index].Height / 2 + 12),
                new Point(DockIconRectangles[index].X + DockIconRectangles[index].Width / 2 + 4,
                    DockIconRectangles[index].Y + DockIconRectangles[index].Height / 2 + 12) });
            }
            else if (index == 2)
            {
                g.FillRectangle(new SolidBrush(CaptionBackColor), DockIconRectangles[index].X + DockIconRectangles[index].Width / 2, DockIconRectangles[index].Y + 6, DockIconRectangles[index].Width / 2 - 6, 4);
                g.DrawRectangle(new Pen(CaptionBackColor), DockIconRectangles[index].X + DockIconRectangles[index].Width / 2, DockIconRectangles[index].Y + 6, DockIconRectangles[index].Width / 2 - 6, DockIconRectangles[index].Height - 12);
                g.FillPolygon(new SolidBrush(Color.Black), new Point[] {
                new Point(DockIconRectangles[index].X + 12,
                    DockIconRectangles[index].Y + DockIconRectangles[index].Height / 2),
                new Point(DockIconRectangles[index].X + 8,
                    DockIconRectangles[index].Y + DockIconRectangles[index].Height / 2 - 4),
                new Point(DockIconRectangles[index].X + 8,
                    DockIconRectangles[index].Y + DockIconRectangles[index].Height / 2 + 4) });
            }    
            else if (index == 3)
            {
                g.FillRectangle(new SolidBrush(CaptionBackColor), DockIconRectangles[index].X + 6, DockIconRectangles[index].Y + DockIconRectangles[index].Height / 2, DockIconRectangles[index].Width - 12, 4);
                g.DrawRectangle(new Pen(CaptionBackColor), DockIconRectangles[index].X + 6, DockIconRectangles[index].Y + DockIconRectangles[index].Height / 2, DockIconRectangles[index].Width - 12, DockIconRectangles[index].Height / 2 - 6);
                g.FillPolygon(new SolidBrush(Color.Black), new Point[] {
                new Point(DockIconRectangles[index].X + DockIconRectangles[index].Width / 2,
                    DockIconRectangles[index].Y + 12),
                new Point(DockIconRectangles[index].X + DockIconRectangles[index].Width / 2 - 4,
                    DockIconRectangles[index].Y + 8),
                new Point(DockIconRectangles[index].X + DockIconRectangles[index].Width / 2 + 4,
                    DockIconRectangles[index].Y + 8) });
            }
        }

        private void _PaintingForm_Paint(object sender, PaintEventArgs e)
        {
            if (_PaintingForm.Visible) // Moving
            {
                DockIconRectangles = new Rectangle[4];
                DockIconRectangles[0] = new Rectangle(DockIconMarginToEdge, (_PaintingForm.ClientSize.Height - DockIconHeight) / 2, DockIconWidth, DockIconHeight);
                DockIconRectangles[1] = new Rectangle((_PaintingForm.ClientSize.Width - DockIconWidth) / 2, DockIconMarginToEdge, DockIconWidth, DockIconHeight);
                DockIconRectangles[2] = new Rectangle(_PaintingForm.ClientSize.Width - DockIconMarginToEdge - DockIconWidth, (_PaintingForm.ClientSize.Height - DockIconHeight) / 2, DockIconWidth, DockIconHeight);
                DockIconRectangles[3] = new Rectangle((_PaintingForm.ClientSize.Width - DockIconWidth) / 2, _PaintingForm.ClientSize.Height - DockIconMarginToEdge - DockIconHeight, DockIconWidth, DockIconHeight);
                for (int i = 0; i < 4; i++)
                {
                    if (_OwnerDockBars[i] != null)
                        PaintDockIcon(e.Graphics, i);
                }
                e.Graphics.Flush();
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!DesignMode)
            {
                Controls.Add(CloseButton);
                Controls.Add(FloatButton);
                Controls.Add(AutoHideButton);
            }
        }

        private void AutoHideButton_Click(object sender, EventArgs e)
        {
            if (ParentDockBar == null)
                WindowState = FormWindowState.Minimized;
            else
                ParentDockBar.HideWindow();
        }

        private void FloatButton_Click(object sender, EventArgs e)
        {
            if (ParentDockBar == null)
                WindowState = WindowState == FormWindowState.Normal ? FormWindowState.Maximized : FormWindowState.Normal;
            else
            {
                Left += 20;
                Top += 20;
                ParentDockBar.RemoveWindow(this);
            }
        }

        private void CloseButton_Click(object sender, EventArgs e)
            => Close();
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (ParentDockBar != null)
                ParentDockBar.RemoveWindow(this);
            base.OnFormClosed(e);
        }
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
            Rectangle rc = new Rectangle(0, 0, ClientSize.Width, CaptionHeight);
            e.Graphics.FillRectangle(new SolidBrush(CaptionBackColor), rc);
            TextRenderer.DrawText(e.Graphics, Text, Font, new Point(WindowEdgeWidth, (CaptionHeight - Font.Height) / 2), CaptionForeColor);
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (Visible)
                SetOwnerAndPaintingFormAndDockBars();
            else
                DisableOwner();
        }

        internal void DisableOwner()
        {
            if(Owner != null)
            {
                Owner.Resize -= Owner_Resize;
                Owner.Move -= Owner_Move;
            }
        }

        internal void SetOwnerAndPaintingFormAndDockBars()
        {
            if (Owner != null)
            {
                Owner.Resize -= Owner_Resize;
                Owner.Resize += Owner_Resize;
                Owner.Move -= Owner_Move;
                Owner.Move += Owner_Move;
                _OwnerDockBars[0] = _OwnerDockBars[1] =
                _OwnerDockBars[2] = _OwnerDockBars[3] = null;
                foreach (Control c in Owner.Controls)
                {
                    if (c is DockBar db && db.Enabled)
                    {
                        if (db.Dock == DockStyle.Left)
                            _OwnerDockBars[0] = db;
                        else if (db.Dock == DockStyle.Top)
                            _OwnerDockBars[1] = db;
                        else if (db.Dock == DockStyle.Right)
                            _OwnerDockBars[2] = db;
                        else if (db.Dock == DockStyle.Bottom)
                            _OwnerDockBars[3] = db;
                    }
                }
                SetPaintingFormBoundsAndLocation();
            }
        }

        protected void SetPaintingFormBoundsAndLocation()
        {
            _PaintingForm.Bounds = Owner.Bounds;
            _PaintingForm.Location = Owner.Location;
            foreach (Control c in Owner.Controls)
            {
                if (c is MdiClient)
                {
                    _PaintingForm.Bounds = c.Bounds;
                    _PaintingForm.Location = c.PointToScreen(new Point(0, 0));
                }
            }
        }

        private void Owner_Move(object sender, EventArgs e)
        {
            SetPaintingFormBoundsAndLocation();
        }

        private void Owner_Resize(object sender, EventArgs e)
        {
            SetPaintingFormBoundsAndLocation();
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x84) // MouseMoving
            {
                if (_PaintingForm.Visible)
                    _PaintingForm.Visible = false;

                Point location = new Point(m.LParam.ToInt32());
                location = PointToClient(location);
                if (location.X >= 0 && location.Y >= 0 && location.X <= WindowEdgeWidth && location.Y <= WindowEdgeWidth)
                {
                    if (ParentDockBar == null)
                        m.Result = (IntPtr)ResizeDirection.TopLeft;
                    else if (ParentDockBar.Dock == DockStyle.Right)
                        m.Result = (IntPtr)ResizeDirection.Left;
                    else if (ParentDockBar.Dock == DockStyle.Bottom)
                        m.Result = (IntPtr)ResizeDirection.Top;
                    else
                        goto AtEnd;
                }
                else if (location.X >= Width - WindowEdgeWidth && location.Y >= 0 && location.X <= Width && location.Y <= WindowEdgeWidth)
                {
                    if (ParentDockBar == null)
                        m.Result = (IntPtr)ResizeDirection.TopRight;
                    else if (ParentDockBar.Dock == DockStyle.Left)
                        m.Result = (IntPtr)ResizeDirection.Right;
                    else if (ParentDockBar.Dock == DockStyle.Bottom)
                        m.Result = (IntPtr)ResizeDirection.Top;
                    else
                        goto AtEnd;
                }
                else if (location.X >= 0 && location.Y >= Height - WindowEdgeWidth && location.X <= WindowEdgeWidth && location.Y <= Height)
                {
                    if (ParentDockBar == null)
                        m.Result = (IntPtr)ResizeDirection.BottomLeft;
                    else if (ParentDockBar.Dock == DockStyle.Right)
                        m.Result = (IntPtr)ResizeDirection.Left;
                    else if (ParentDockBar.Dock == DockStyle.Top)
                        m.Result = (IntPtr)ResizeDirection.Bottom;
                    else
                        goto AtEnd;
                }
                else if (location.X >= Width - WindowEdgeWidth && location.Y >= Height - WindowEdgeWidth && location.X <= Width && location.Y <= Height)
                {
                    if (ParentDockBar == null)
                        m.Result = (IntPtr)ResizeDirection.BottomRight;
                    else if (ParentDockBar.Dock == DockStyle.Left)
                        m.Result = (IntPtr)ResizeDirection.Right;
                    else if (ParentDockBar.Dock == DockStyle.Top)
                        m.Result = (IntPtr)ResizeDirection.Bottom;
                    else
                        goto AtEnd;
                }
                else if (location.X >= 0 && location.X <= Width && location.Y >= 0 && location.Y <= WindowEdgeWidth)
                {
                    if (ParentDockBar == null || ParentDockBar.Dock == DockStyle.Bottom)
                        m.Result = (IntPtr)ResizeDirection.Top;
                    else
                        goto AtEnd;
                }
                else if (location.Y < CaptionHeight)
                {
                    if (ParentDockBar == null)
                        m.Result = (IntPtr)2;
                    else
                        goto AtEnd;
                }
                else if (location.X >= 0 && location.X <= WindowEdgeWidth && location.Y >= 0 && location.Y <= Height)
                {
                    if (ParentDockBar == null || ParentDockBar.Dock == DockStyle.Right)
                        m.Result = (IntPtr)ResizeDirection.Left;
                    else
                        goto AtEnd;
                }
                else if (location.X >= Width - WindowEdgeWidth && location.X <= Width && location.Y >= 0 && location.Y <= Height)
                {
                    if (ParentDockBar == null || ParentDockBar.Dock == DockStyle.Left)
                        m.Result = (IntPtr)ResizeDirection.Right;
                    else
                        goto AtEnd;
                }
                else if (location.X >= 0 && location.X <= Width && location.Y >= Height - WindowEdgeWidth && location.Y <= Height)
                {
                    if (ParentDockBar == null || ParentDockBar.Dock == DockStyle.Top)
                        m.Result = (IntPtr)ResizeDirection.Bottom;
                    else
                        goto AtEnd;
                }
                else
                    goto AtEnd;
                return;
            }
            else if (m.Msg == 0xA1) //FunctionalMouseDown
            {
                Point location = new Point(m.LParam.ToInt32());
                location = PointToClient(location);
                if (location.Y < CaptionHeight && location.Y > WindowEdgeWidth)
                    _PaintingForm.Visible = true;
            }
        AtEnd:
            base.WndProc(ref m);
        }
    }
}
