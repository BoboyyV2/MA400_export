using ACadSharp;
using ACadSharp.IO;
using CSMath;
using Svg;
using Svg.Transforms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MA400_export
{
    public class SVGcontroller
    {
        public SvgDocument svg { get; private set; }

        public PointF offset { get; set; }
        public RectangleF dimension { get; set; }
        public Scale scale { get; set; }

        public SVGcontroller()
        {
            svg = new SvgDocument();
            offset = new PointF(0, 0);
            dimension = new RectangleF(0, 0, 0, 0);
            scale = new Scale(1, 1);
        }

        public void OpenSVG(string path, string filename)
        {
            svg = SvgDocument.Open(path + filename);
            BuildSVG();
        }

        

        public void BuildSVG()
        {

            if (svg == null) return;

            //get the dimensions
            SizeF dims = svg.GetDimensions();
            if (dims.Width <= 0 || dims.Height <= 0) return;
            dimension = new RectangleF(0, 0, dims.Width, dims.Height);

            //get the offset
            RectangleF bounds = svg.Bounds;
            //MessageBox.Show("bounds : " + bounds.X + "; " + bounds.Y + ", dim = " + bounds.Width + "; " + bounds.Height);
            offset = new PointF(bounds.X, bounds.Y);

            float scaleX = 1;
            float scaleY = 1;
            //get the scale
            foreach (SvgTransform transform in svg.Transforms)
            {
                if(transform.GetType() == typeof(SvgScale) ) 
                {
                    SvgScale scale = ((SvgScale)transform);
                    float[]Elem = scale.Matrix.Elements;
                    //X,?,?,Y,?,?
                     scaleX = Elem[0];
                     scaleY = Elem[3];
                    this.scale = new Scale(scaleX, scaleY);



                }
            }
            
            SvgScale();
            svg.Write(Constants.Outputpath + Constants.tmpPath + @"\display.svg");
            

        }
        private void SvgScale() 
        {
            SizeF dims = svg.GetDimensions();
            float translateX = 0;
            float translateY = 0;

            if (scale.Xscale < 0)
            {
                translateX = -(svg.X.Value - dims.Width * (float)scale.Xscale);
            }

            if (scale.Yscale < 0)
            {
                translateY = -(svg.Y.Value - dims.Height * (float)scale.Yscale);
            }

            svg.Transforms.Add(new SvgTranslate(translateX, translateY));
        }


    }
}
