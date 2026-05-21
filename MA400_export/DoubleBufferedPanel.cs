using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MA400_export
{
    internal class DoubleBufferedPanel : Panel
    {
        public DoubleBufferedPanel()
        {
            //enable double buffering and related styles
            this.DoubleBuffered = true;
            this.ResizeRedraw = true;

            //or, use SetStyle for more granular control:
            //this.SetStyle(ControlStyles.AllPaintingInWmPaint | 
            //              ControlStyles.UserPaint | 
            //              ControlStyles.OptimizedDoubleBuffer, true);
            //this.UpdateStyles();
        }
    }
}
