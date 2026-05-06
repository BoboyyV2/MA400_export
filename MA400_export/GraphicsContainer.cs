using ACadSharp.Entities;
using Svg;
using Svg.FilterEffects;
using Svg.Transforms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DXFImporter;

namespace MA400_export
{
    /**
     * <summary>
     * A class made to accomodate the graphics of the main panel so that it does not get confused with the rest. <br></br>
     * Should only handle graphical matters, nothing technical
     * </summary>
     */
    public class GraphicsContainer
    {

        /**
         * <summary>the actual graphics</summary>
         */
        public Graphics graphics { get; set; }

        /**
         * <summary>the static path to the exported svg, beware of remnant</summary>
         */
        private String path = Properties.Settings.Default.OutputPath + Constants.tmpPath + "display.svg";//.exe loc
        //private String path = AppDomain.CurrentDomain.BaseDirectory + Constants.tmpPath + "truite.jpg";//used for debug

        private SvgDocument svg = null;

        DXFImporter.Canvas canvas = null;
        private bool open { get; set; }

        private float line_thickness = 0.4f;

        //dimensions variables 
        public Layout_Info layout;


        public GraphicsContainer(Graphics graphics)
        {
            this.graphics = graphics;
            open = false;
            layout = new Layout_Info();

        }

        public GraphicsContainer()
        {
            this.graphics = null;
            open = false;
            layout = new Layout_Info();

        }

        public void reset()
        {
            CloseCanvas();
        }


        public void GetLayout()
        {
            canvas.checkLayout();
            if (open)
            {
                layout.offset = new PointF((float)canvas.XMin, (float)canvas.YMin);//tpo left corner
                layout.dimension = new RectangleF((float)canvas.XMin, (float)canvas.YMin, (float)(canvas.XMax - canvas.XMin), (float)(canvas.YMax - canvas.YMin));// offset as origin

                layout.scale = new Scale(true, false);
            }
        }

        /**
         * <summary>draw a circle whose center is at positionX ; positionY  in the given graphics</summary>
         */
        public void DrawCircle(float positionX, float positionY, float radius)
        {
            RectangleF shape = new RectangleF();
            SolidBrush drawBrush = new SolidBrush(Color.Purple);
            Pen pen = new Pen(Color.White, 1);

            shape.X = positionX - radius;
            shape.Y = positionY - radius;
            shape.Width = 2 * radius - pen.Width / 2;
            shape.Height = 2 * radius - pen.Width / 2;

            graphics.FillEllipse(drawBrush, shape);
            graphics.DrawEllipse(pen, shape);

        }


        /**
         * <summary>Draw a stud into the graphics</summary>
         */
        public void Draw_Stud(Circle stud)
        {
            float StudRadius = (float)stud.Radius;
            RectangleF shape = new RectangleF();
            shape.X = (float)stud.Center.X - StudRadius + Constants.Origin_Coord.X;
            shape.Y = (float)stud.Center.Y - StudRadius + Constants.Origin_Coord.Y;
            shape.Width = 2 * StudRadius;
            shape.Height = 2 * StudRadius;

            SolidBrush drawBrush = new SolidBrush(Color.LimeGreen);
            graphics.FillEllipse(drawBrush, shape);

        }

        /**
         * <summary>Draw all the studs into the graphics</summary>
         */
        public void Draw_Studs(IEnumerable<Stud> Studs)
        {
            foreach (var item in Studs)
            {
                Draw_Stud(item.circle);
            }

        }

        /**
         * <summary>Draw a stud into the graphics</summary>
         */
        public void Draw_Selected_Stud(Circle stud)
        {
            float StudRadius = (float)stud.Radius + 0.3f;
            RectangleF shape = new RectangleF();
            shape.X = (float)stud.Center.X - StudRadius + Constants.Origin_Coord.X;
            shape.Y = (float)stud.Center.Y - StudRadius + Constants.Origin_Coord.Y;
            shape.Width = 2 * StudRadius;
            shape.Height = 2 * StudRadius;

            SolidBrush drawBrush = new SolidBrush(Color.Cyan);
            graphics.FillEllipse(drawBrush, shape);

        }

        /**
         * <summary>Draw all the studs into the graphics</summary>
         */
        public void Draw_Selected_Studs(IEnumerable<Stud> SelectedStuds)
        {
            foreach (var item in SelectedStuds)
            {
                Draw_Selected_Stud(item.circle);
            }

        }


        private void Draw_CoordSystem()
        {

            Pen pen = new Pen(Color.Green);
            // Create font and brush.
            Font drawFont = new Font("Arial", 11);
            SolidBrush drawBrush = new SolidBrush(Color.Green);
            // Create point for upper-left corner of drawing.
            PointF Xtarget = PointF.Empty;
            PointF Ytarget = PointF.Empty;
            PointF Origin = PointF.Empty;


            // Set Maximum and minimum points
            Xtarget.X = 50;
            Xtarget.Y = 20;
            Ytarget.X = 20;
            Ytarget.Y = 50;
            Origin.X = 20;
            Origin.Y = 20;



            // Draw (dashed) connection line
            int delta = 4;
            float[] dashValues = { 6, delta };
            Pen dashPen = new Pen(Color.Green, 1);
            dashPen.DashPattern = dashValues;

            graphics.DrawLine(dashPen, Origin, Ytarget);

            //the tip
            graphics.DrawLine(Pens.Green, Ytarget.X, Ytarget.Y + 1, Ytarget.X - delta, Ytarget.Y - delta + 1);
            graphics.DrawLine(Pens.Green, Ytarget.X, Ytarget.Y + 1, Ytarget.X + delta, Ytarget.Y - delta + 1);

            graphics.DrawString("y", drawFont, drawBrush, Ytarget.X - 6, Ytarget.Y);

            //x arrow
            graphics.DrawLine(dashPen, Origin, Xtarget);

            //the tip
            graphics.DrawLine(Pens.Green, Xtarget.X + 1, Xtarget.Y, Xtarget.X - delta + 1, Xtarget.Y + delta);
            graphics.DrawLine(Pens.Green, Xtarget.X + 1, Xtarget.Y, Xtarget.X - delta + 1, Xtarget.Y - delta);
            graphics.DrawString("x", drawFont, drawBrush, Xtarget.X, Xtarget.Y - 9);

            //draw the scale indicator
            PointF ScaleStart = PointF.Empty;
            ScaleStart.X = 100;
            ScaleStart.Y = 25;

            PointF ScaleEnd = PointF.Empty;
            ScaleEnd.X = 200;
            ScaleEnd.Y = 25;

            graphics.DrawLine(Pens.Purple, ScaleStart, ScaleEnd);

            //draw tips
            graphics.DrawLine(Pens.Purple, ScaleStart.X, ScaleStart.Y, ScaleStart.X + 5, ScaleStart.Y - 5);
            graphics.DrawLine(Pens.Purple, ScaleStart.X, ScaleStart.Y, ScaleStart.X + 5, ScaleStart.Y + 5);
            graphics.DrawLine(Pens.Purple, ScaleEnd.X, ScaleEnd.Y, ScaleEnd.X - 5, ScaleEnd.Y - 5);
            graphics.DrawLine(Pens.Purple, ScaleEnd.X, ScaleEnd.Y, ScaleEnd.X - 5, ScaleEnd.Y + 5);

            drawFont = new Font("Arial", 10);
            drawBrush = new SolidBrush(Color.Purple);
            graphics.DrawString("10cm = 100u", drawFont, drawBrush, ScaleStart.X + 9, ScaleStart.Y - 18);



        }

        private void Draw_WorkZoneLimits()
        {

            // Draw (dashed) connection line
            int delta = 5;
            float[] dashValues = { delta, delta };
            Pen dashPen = new Pen(Color.Red, 0.8f);
            dashPen.DashPattern = dashValues;

            graphics.DrawLine(dashPen, Constants.Origin_Coord.X - dashPen.Width / 2, Constants.Origin_Coord.Y - dashPen.Width / 2, Constants.Origin_Coord.X - dashPen.Width / 2, Constants.WorkZoneLimits_Coord.Y - dashPen.Width / 2);
            graphics.DrawLine(dashPen, Constants.Origin_Coord.X - dashPen.Width / 2, Constants.WorkZoneLimits_Coord.Y - dashPen.Width / 2, Constants.WorkZoneLimits_Coord.X - dashPen.Width / 2, Constants.WorkZoneLimits_Coord.Y - dashPen.Width / 2);
            graphics.DrawLine(dashPen, Constants.WorkZoneLimits_Coord.X - dashPen.Width / 2, Constants.WorkZoneLimits_Coord.Y - dashPen.Width / 2, Constants.WorkZoneLimits_Coord.X - dashPen.Width / 2, Constants.Origin_Coord.Y - dashPen.Width / 2);
            graphics.DrawLine(dashPen, Constants.WorkZoneLimits_Coord.X - dashPen.Width / 2, Constants.Origin_Coord.Y - dashPen.Width / 2, Constants.Origin_Coord.X - dashPen.Width / 2, Constants.Origin_Coord.Y - dashPen.Width / 2);


        }

        public void OpenCanvas()
        {
            canvas.ReadFromFile(Constants.Outputpath + Constants.tmpPath + @"\dxftmp.dxf");
            open = true;
            
            GetLayout();
            //Debug
            MessageBox.Show("layout after update : " + Environment.NewLine
                           + "offset = " + layout.offset.ToString() + Environment.NewLine
                           + "dimention = " + layout.dimension.ToString() + Environment.NewLine
                           + "coordinates :" + Environment.NewLine
                           + "min = " + canvas.XMin + " ; " + canvas.YMin + Environment.NewLine
                           + "max = " + canvas.XMax + " ; " + canvas.YMax + Environment.NewLine);

            canvas.setup(Constants.Origin_Coord);

        }

        public void CloseCanvas() 
        {
            canvas = new Canvas();
            DXFImporter.Shape.Dimension = new RectangleF();
            DXFImporter.Shape.PartOffset = new PointF();
            DXFImporter.Shape.OriginOffset = new PointF();
        }

        public void OpenSVG()
        {
            open = true;
            
            svg = SvgDocument.Open(path);
        }

        private void RenderSVG()
        {
            
            if (svg == null) return;

            SizeF dims = svg.GetDimensions();
            if (dims.Width <= 0 || dims.Height <= 0) return;

            int renderWidth = (int)dims.Width;
            int renderHeight = (int)dims.Height;

            string bmpPath = Properties.Settings.Default.OutputPath + Constants.tmpPath + @"bmp.BMP";
            Directory.CreateDirectory(Properties.Settings.Default.OutputPath + Constants.tmpPath);
            var bmp = svg.Draw();
            bmp.Save(bmpPath, ImageFormat.Bmp);
            using (Bitmap svgBitmap = svg.Draw(renderWidth, renderHeight))
            {
                graphics.DrawImage(
                    svgBitmap,
                    Constants.Origin_Coord.X + line_thickness,
                    Constants.Origin_Coord.Y + line_thickness
                );

            }
            
            

        }

        public void DrawDXF()
        {
            if (open)
            {
                canvas.Draw(graphics);
            }
        }

        private void Draw_ReferenceCircles()
        {
            float radius = 6.5f;//6.5mm

            //ligne du haut en Y
            DrawCircle(Constants.Origin_Coord.X - radius + 20 , Constants.Origin_Coord.Y - radius, radius);
            DrawCircle(Constants.Origin_Coord.X - radius + 50, Constants.Origin_Coord.Y - radius, radius);
            DrawCircle(Constants.Origin_Coord.X - radius + 150, Constants.Origin_Coord.Y - radius, radius);
            DrawCircle(Constants.Origin_Coord.X - radius + 300, Constants.Origin_Coord.Y - radius, radius);
            DrawCircle(Constants.Origin_Coord.X - radius + 500, Constants.Origin_Coord.Y - radius, radius);

            //ligne du bas en Y
            DrawCircle(Constants.Origin_Coord.X - radius + 20, Constants.Origin_Coord.Y - radius + 430, radius);
            DrawCircle(Constants.Origin_Coord.X - radius + 50, Constants.Origin_Coord.Y - radius + 430, radius);
            DrawCircle(Constants.Origin_Coord.X - radius + 150, Constants.Origin_Coord.Y - radius + 430, radius);
            DrawCircle(Constants.Origin_Coord.X - radius + 300, Constants.Origin_Coord.Y - radius + 430, radius);
            DrawCircle(Constants.Origin_Coord.X - radius + 500, Constants.Origin_Coord.Y - radius + 430, radius);

            //à gauche
            DrawCircle(Constants.Origin_Coord.X - radius, Constants.Origin_Coord.Y - radius + 20, radius);
            DrawCircle(Constants.Origin_Coord.X - radius, Constants.Origin_Coord.Y - radius + 150, radius);
            DrawCircle(Constants.Origin_Coord.X - radius, Constants.Origin_Coord.Y - radius + 450, radius);

        }

        

        public void Paint(IEnumerable<Stud> Studs, IEnumerable<Stud>SelectedStuds, PointF offset)
        {
            //draw the basic from of the workzone
            //including but not restricted to :
            //the background, the rectangular coordinate system, the scale
            //the workzone, the landmarks

            graphics.Clear(Color.Black);


            Draw_CoordSystem();
            Draw_WorkZoneLimits();
            Draw_ReferenceCircles();
            //affichage du dxf
            
            DrawDXF();//check if open

            Draw_Studs(Studs);
            Draw_Selected_Studs(SelectedStuds);

        }


    }
}
