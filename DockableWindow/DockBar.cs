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
        public override DockStyle Dock
        {
            get => base.Dock;
            set
            {
                if (value == DockStyle.Fill || value == DockStyle.None)
                    throw new ArgumentOutOfRangeException(nameof(Dock), DockStyleCantBeFillOrNone);
                base.Dock = value;
                for (int i = 0; i < _Windows.Count; i++)
                    _Windows[i].DockAt = value;
                UpdateWidth();
            }
        }
        protected List<DockableWindow> _Windows;
        public IReadOnlyList<DockableWindow> Windows => _Windows.AsReadOnly();
        protected List<int> TextWidths { get; set; }
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
            TextWidths = new List<int>();
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
        }
        private void ParentForm_Resize(object sender, EventArgs e)
        {
            if (CurrentWindow != null)
                SetWindowPostionAndSize(true);
        }

        protected void ShowForm(int index)
        {
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
                    CurrentWindow.Width = _Windows[CurrentWindowIndex].Width;
                    CurrentWindow.Height = Height;
                    CurrentWindow.Left = p.X + Width;
                    CurrentWindow.Top = p.Y;
                    break;
                case DockStyle.Bottom:
                    CurrentWindow.Width = Width;
                    CurrentWindow.Height = _Windows[CurrentWindowIndex].Height;
                    CurrentWindow.Left = p.X;
                    CurrentWindow.Top = p.Y - CurrentWindow.Height;
                    break;
                case DockStyle.Right:
                    CurrentWindow.Width = _Windows[CurrentWindowIndex].Width;
                    CurrentWindow.Height = Height;
                    CurrentWindow.Left = p.X - CurrentWindow.Width;
                    CurrentWindow.Top = p.Y;
                    break;
                case DockStyle.Top:
                    CurrentWindow.Width = Width;
                    CurrentWindow.Height = _Windows[CurrentWindowIndex].Height;
                    CurrentWindow.Left = p.X;
                    CurrentWindow.Top = p.Y + Height;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(Dock));
            }
            if (refresh)
                Refresh();
        }

        protected void HideForm()
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
            window.FormClosed += Window_FormClosed;
            window.ParentChanged += Window_ParentChanged;            
            window.DockAt = Dock;
            _Windows.Add(window);
            TextWidths.Add(size.Width);
            Refresh();
        }

        private void Window_ParentChanged(object sender, EventArgs e)
        {
            if (Parent == null)
                RemoveWindow(sender as DockableWindow);
        }

        private void Window_FormClosed(object sender, FormClosedEventArgs e)
        {
            RemoveWindow(sender as DockableWindow);
        }

        public void RemoveWindow(DockableWindow window)
        {
            int index = _Windows.IndexOf(window);
            if (index != -1)
            {
                if (CurrentWindowIndex == index)
                    HideForm();
                _Windows[index].FormClosed -= Window_FormClosed;
                _Windows[index].ParentChanged -= Window_ParentChanged;
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
                position += TextWidths[i];
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
                HideForm();
            else
            {
                HideForm();
                ShowForm(_MouseOverWindowIndex);
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
                        e.Graphics.FillRectangle(new SolidBrush(barColor), 0, position, 7, TextWidths[i]);
                        e.Graphics.DrawString(_Windows[i].Text, Font, new SolidBrush(foreColor), new PointF(10, position), sf);
                        break;
                    case DockStyle.Right:
                        e.Graphics.DrawString(_Windows[i].Text, Font, new SolidBrush(foreColor), new PointF(0, position), sf);
                        e.Graphics.FillRectangle(new SolidBrush(barColor), Font.Height + 5, position, 7, TextWidths[i]);
                        break;
                    case DockStyle.Top:
                        e.Graphics.FillRectangle(new SolidBrush(barColor), position, 0, TextWidths[i], 7);
                        e.Graphics.DrawString(_Windows[i].Text, Font, new SolidBrush(foreColor), new PointF(position, 10));
                        break;
                    case DockStyle.Bottom:
                        e.Graphics.DrawString(_Windows[i].Text, Font, new SolidBrush(foreColor), new PointF(position, 0));
                        e.Graphics.FillRectangle(new SolidBrush(barColor), position, Font.Height + 5, TextWidths[i], 7);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(Dock), DockStyleCantBeFillOrNone);
                }
                position += TextWidths[i] + ItemInterval;
            }

        }
    }
}
