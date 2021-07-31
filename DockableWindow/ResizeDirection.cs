using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Aritiafel.Organizations.ElibrarPartFactory
{

    public enum ResizeDirection
    {
        None = 0,
        Left = 10,
        Right = 11,
        Top = 12,
        TopLeft = 13,        
        TopRight = 14,
        Bottom = 15,
        BottomLeft = 16,
        BottomRight = 17
    }

    public static partial class Extension
    {
        public static Cursor GetResizeCursor(this ResizeDirection rd)
        {
            switch (rd)
            {
                case ResizeDirection.None:
                    return Cursors.Default;
                case ResizeDirection.Left:
                case ResizeDirection.Right:
                    return Cursors.SizeWE;
                case ResizeDirection.Top:
                case ResizeDirection.Bottom:
                    return Cursors.SizeNS;
                case ResizeDirection.TopLeft:
                case ResizeDirection.BottomRight:
                    return Cursors.SizeNWSE;
                case ResizeDirection.TopRight:
                case ResizeDirection.BottomLeft:
                    return Cursors.SizeNESW;
                default:
                    throw new ArgumentOutOfRangeException(nameof(rd));
            }
        }
    }

}
