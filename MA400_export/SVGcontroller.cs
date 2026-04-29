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

        private bool fromDxf;

        public SVGcontroller()
        {
            svg = new SvgDocument();
            offset = new PointF(0, 0);
            dimension = new RectangleF(0, 0, 0, 0);
            scale = new Scale(1, 1);
        }

        public void OpenSVG(string path, string filename, bool fromDxf = true)
        {
            svg = SvgDocument.Open(path + filename);
            this.fromDxf = fromDxf;

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
            //get the scale & offset
            SvgTransform translate_to_remove = null;
            SvgTransform scale_to_remove = null;
            foreach (SvgTransform transform in svg.Transforms)
            {
                //change la scale si besoin
                
                if (transform.GetType() == typeof(SvgScale))
                {
                    if (fromDxf)
                    {
                        float[] Elem = ((SvgScale)transform).Matrix.Elements;
                        //X,?,?,Y,?,?
                        scaleX = Elem[0];
                        scaleY = Elem[3];
                        this.scale = new Scale(scaleX, scaleY);

                    }
                    else
                    {
                        this.scale = new Scale(scaleX, scaleY);

                    }
                    translate_to_remove = transform;

                }

                //change l'offset
                if (transform.GetType() == typeof(SvgTranslate))
                {
                    SvgTranslate translate = ((SvgTranslate)transform);
                    offset = new PointF(offset.X + translate.X, offset.Y + translate.Y);
                    translate_to_remove = transform;
                }

            }
            svg.Transforms.Remove(translate_to_remove);
            //svg.Transforms.Remove(scale_to_remove);

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
                translateX = -(svg.X.Value - (dims.Width * (float)scale.Xscale));
            }

            if (scale.Yscale < 0)
            {
                translateY = -svg.Y.Value - (dims.Height * (float)scale.Yscale);
                //translateY = (svg.Y.Value -)
            }

            svg.Transforms.Add(new SvgTranslate(translateX, translateY));
            svg.Transforms.Add(new SvgScale((float)scale.Xscale, (float)scale.Yscale));
        }


    }
}
