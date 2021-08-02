using System;
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
        private const int DockIconHeight = 30;
        private const int DockIconWidth = 20;
        private const int DockIconWindowHeight = 20;
        private const int DockIconWindowWidth = 16;

        protected Button CloseButton;
        protected Button FloatButton; // Like Maximize
        protected Button AutoHideButton; //Like Minimize
        protected bool _IsMoving;
        protected DockBar[] _OwnerDockBars;
        protected Control _PaintingArea;
        protected Form _PaintingForm;
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
            _PaintingForm.BackColor = Color.Bisque;
            _PaintingForm.FormBorderStyle = FormBorderStyle.None;
            _PaintingForm.TransparencyKey = _PaintingForm.BackColor;
            //_PaintingForm.Bounds = Screen.PrimaryScreen.Bounds;
            _PaintingForm.StartPosition = FormStartPosition.Manual;
            _PaintingForm.TopMost = true;
            _PaintingForm.Paint += _PaintingForm_Paint;
            _PaintingForm.MouseMove += _PaintingForm_MouseMove;            
            InitializeComponent();
        }

        private void _PaintingForm_MouseMove(object sender, MouseEventArgs e)
        {
            _PaintingForm.Visible = false;
        }

        private void _PaintingForm_Paint(object sender, PaintEventArgs e)
        {
            //Graphics g = f.CreateGraphics();
            //f.Show();
            //Console.WriteLine("in");
            if (_PaintingForm.Visible) // Moving
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.White), DockIconMarginToEdge, (_PaintingForm.ClientSize.Height - DockIconHeight) / 2, DockIconWidth, DockIconHeight);
                //e.Graphics.DrawRectangle(new Pen(Color.Black), DockIconMarginToEdge, (_PaintingForm.ClientSize.Height - DockIconHeight) / 2, DockIconWidth, DockIconHeight);
                e.Graphics.Flush();
            }



            if (_OwnerDockBars[0] != null)
            {

                //e.Graphics
                //g Left

                //e.Graphics.FillRectangle(Brushes.Black, 20, 20, 100, 100);
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
        }

        protected void SetOwnerAndPaintingFormAndDockBars()
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
                    if (c is DockBar db)
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
                    if (c is MdiClient)
                    {
                        _PaintingForm.Bounds = c.Bounds;
                        _PaintingForm.Location = c.PointToScreen(new Point(0, 0));
                    }
                }
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
