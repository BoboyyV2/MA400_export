using System.Drawing;

namespace MA400_export
{
    public class Layout_Info
    {
        public PointF offset { get; set; }
        public RectangleF dimension { get; set; }
        public Scale scale { get; set; }

        public Layout_Info()
        {
            offset = new PointF();
            dimension = new RectangleF();
            scale = new Scale();
        }
    }
}
