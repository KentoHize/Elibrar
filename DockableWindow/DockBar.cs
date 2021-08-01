using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Aritiafel.Organizations.ElibrarPartFactory
{
    [Designer(typeof(DockBarDesigner))]
    public partial class DockBar : UserControl
    {
        public static DockStyle DefaultDockStyle = DockStyle.Left;
        public static int DefaultWidth = 30;
        private const string DockStyleCantBeFillOrNone = "DockStyle can't be Fill or None.";

        protected int _StartPosition;
        protected int _CurrentPosition;
        protected int _MouseOverWindowIndex;
        protected List<int> _TextWidths;
        protected List<Size> _WindowsDefaultSize;
        public override DockStyle Dock
        {
            get => base.Dock;
            set
            {
                if (value == DockStyle.Fill || value == DockStyle.None)
                    throw new ArgumentOutOfRangeException(nameof(Dock), DockStyleCantBeFillOrNone);
                base.Dock = value;                
                if (CurrentWindow != null)
                    HideWindow();                
                UpdateWidth();
            }
        }
        protected List<DockableWindow> _Windows;
        public IReadOnlyList<DockableWindow> Windows => _Windows.AsReadOnly();
        
        public int ItemInterval { get; set; }
        public Color BarColor { get; set; }
        public Color MouseOverColor { get; set; }
        public DockableWindow CurrentWindow { get; protected set; }
        public int CurrentWindowIndex { get; protected set; }

        protected void UpdateWidth()
        {
            if (Dock == DockStyle.Left || Dock == DockStyle.Right)
                Width = DefaultWidth;
            else
                Height = DefaultWidth;
            Refresh();
        }

        public DockBar()
        {
            InitializeComponent();
            _Windows = new List<DockableWindow>();
            _TextWidths = new List<int>();
            _WindowsDefaultSize = new List<Size>();
            MouseOverColor = SystemColors.MenuHighlight;
            BarColor = SystemColors.ControlLight;
            ItemInterval = 5;
            _StartPosition = 5;
            _CurrentPosition = 0;
            CurrentWindow = null;
            CurrentWindowIndex = -1;          
        }
     
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            ParentForm.Resize += ParentForm_Resize;
            ParentForm.Move += ParentForm_Move;
        }

        private void ParentForm_Move(object sender, EventArgs e)
        {
            if (CurrentWindow != null)
                SetWindowPostionAndSize(true);
        }

        private void ParentForm_Resize(object sender, EventArgs e)
        {
            if (CurrentWindow != null)
                SetWindowPostionAndSize(true);
        }

        public void ShowWindow(int index)
        {
            HideWindow();
            CurrentWindowIndex = index;
            CurrentWindow = _Windows[CurrentWindowIndex];
            CurrentWindow.StartPosition = FormStartPosition.Manual;
            SetWindowPostionAndSize();
            CurrentWindow.Show(ParentForm);
        }

        protected void SetWindowPostionAndSize(bool refresh = false)
        {
            Point p = PointToScreen(new Point(0, 0));
            switch (Dock)
            {
                case DockStyle.Left:
                    CurrentWindow.Width = _WindowsDefaultSize[CurrentWindowIndex].Width;
                    CurrentWindow.Height = Height;
                    CurrentWindow.Left = p.X + Width;
                    CurrentWindow.Top = p.Y;
                    break;
                case DockStyle.Bottom:
                    CurrentWindow.Width = Width;
                    CurrentWindow.Height = _WindowsDefaultSize[CurrentWindowIndex].Height;
                    CurrentWindow.Left = p.X;
                    CurrentWindow.Top = p.Y - CurrentWindow.Height;
                    break;
                case DockStyle.Right:
                    CurrentWindow.Width = _WindowsDefaultSize[CurrentWindowIndex].Width;
                    CurrentWindow.Height = Height;
                    CurrentWindow.Left = p.X - CurrentWindow.Width;
                    CurrentWindow.Top = p.Y;
                    break;
                case DockStyle.Top:
                    CurrentWindow.Width = Width;
                    CurrentWindow.Height = _WindowsDefaultSize[CurrentWindowIndex].Height;
                    CurrentWindow.Left = p.X;
                    CurrentWindow.Top = p.Y + Height;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(Dock));
            }
            if (refresh)
                Refresh();
        }

        public void HideWindow()
        {
            if (CurrentWindow == null)
                return;
            CurrentWindow.Hide();
            CurrentWindowIndex = -1;
            CurrentWindow = null;
        }

        public void AddWindow(DockableWindow window)
        {
            Size size = TextRenderer.MeasureText(window.Text, Font);            
            window.ParentDockBar = this;
            _Windows.Add(window);
            _TextWidths.Add(size.Width);
            _WindowsDefaultSize.Add(window.Size);
            Refresh();
        }

        public void RemoveWindow(DockableWindow window)
        {
            int index = _Windows.IndexOf(window);
            if (index != -1)
            {
                if (CurrentWindowIndex == index)
                {
                    CurrentWindow = null;
                    CurrentWindowIndex = -1;
                }
                _Windows[index].ParentDockBar = null;
                _WindowsDefaultSize.RemoveAt(index);                
                _TextWidths.RemoveAt(index);
                _Windows.RemoveAt(index);
                Refresh();
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            int position = _StartPosition;
            int newMouseOverFormIndex = _MouseOverWindowIndex;
            int i;
            for (i = 0; i < _Windows.Count; i++)
            {
                if (((Dock == DockStyle.Left || Dock == DockStyle.Right) && e.Y < position) ||
                    ((Dock == DockStyle.Top || Dock == DockStyle.Bottom) && e.X < position))
                {
                    newMouseOverFormIndex = -1;
                    break;
                }
                position += _TextWidths[i];
                if (((Dock == DockStyle.Left || Dock == DockStyle.Right) && e.Y < position) ||
                    ((Dock == DockStyle.Top || Dock == DockStyle.Bottom) && e.X < position))
                {
                    newMouseOverFormIndex = i;
                    break;
                }
                position += ItemInterval;
            }
            if (i == Windows.Count)
                newMouseOverFormIndex = -1;
            if (newMouseOverFormIndex != _MouseOverWindowIndex)
            {
                _MouseOverWindowIndex = newMouseOverFormIndex;
                Refresh();
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (_MouseOverWindowIndex != -1)
            {
                _MouseOverWindowIndex = -1;
                Refresh();
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (_MouseOverWindowIndex == -1)
                return;
            else if (CurrentWindowIndex == _MouseOverWindowIndex)
                HideWindow();
            else
            {
                HideWindow();
                ShowWindow(_MouseOverWindowIndex);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            int position = _StartPosition;
            for (int i = 0; i < _Windows.Count; i++)
            {
                Color foreColor = _MouseOverWindowIndex == i ? MouseOverColor : ForeColor;
                Color barColor = _MouseOverWindowIndex == i ? MouseOverColor : BarColor;
                StringFormat sf = new StringFormat(StringFormatFlags.DirectionVertical);
                switch (Dock)
                {
                    case DockStyle.Left:
                        e.Graphics.FillRectangle(new SolidBrush(barColor), 0, position, 7, _TextWidths[i]);
                        e.Graphics.DrawString(_Windows[i].Text, Font, new SolidBrush(foreColor), new PointF(10, position), sf);
                        break;
                    case DockStyle.Right:
                        e.Graphics.DrawString(_Windows[i].Text, Font, new SolidBrush(foreColor), new PointF(0, position), sf);
                        e.Graphics.FillRectangle(new SolidBrush(barColor), Font.Height + 5, position, 7, _TextWidths[i]);
                        break;
                    case DockStyle.Top:
                        e.Graphics.FillRectangle(new SolidBrush(barColor), position, 0, _TextWidths[i], 7);
                        e.Graphics.DrawString(_Windows[i].Text, Font, new SolidBrush(foreColor), new PointF(position, 10));
                        break;
                    case DockStyle.Bottom:
                        e.Graphics.DrawString(_Windows[i].Text, Font, new SolidBrush(foreColor), new PointF(position, 0));
                        e.Graphics.FillRectangle(new SolidBrush(barColor), position, Font.Height + 5, _TextWidths[i], 7);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(Dock), DockStyleCantBeFillOrNone);
                }
                position += _TextWidths[i] + ItemInterval;
            }

        }
    }
}
