using ACadSharp.Entities;
using Svg;
using Svg.FilterEffects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        private String path = AppDomain.CurrentDomain.BaseDirectory + @"tmp\display.svg";//.exe loc
        //private String path = AppDomain.CurrentDomain.BaseDirectory + @"tmp\truite.jpg";//used for debug


        private SvgDocument svg = null;
        private bool open { get; set; }

        public GraphicsContainer(Graphics graphics)
        {
            this.graphics = graphics;
            open = false;
        }

        public GraphicsContainer()
        {
            this.graphics = null;
            open = false;
        }


        /**
         * <summary>draw a circle whose center is at positionX ; positionY  in the given graphics</summary>
         */
        public void DrawCircle(float positionX, float positionY, float radius)
        {
            RectangleF shape = new RectangleF();
            shape.X = positionX - radius;
            shape.Y = positionY - radius;
            shape.Width = 2 * radius;
            shape.Height = 2 * radius;

            SolidBrush drawBrush = new SolidBrush(Color.Purple);
            graphics.FillEllipse(drawBrush, shape);
            graphics.DrawEllipse(Pens.White, shape);

        }

        public void Draw_Stud(Circle stud)
        {
            float StudRadius = (float)stud.Radius;
            RectangleF shape = new RectangleF();
            shape.X = (float)stud.Center.X - StudRadius + Constants.Origin_Coord.X;
            shape.Y = (float)stud.Center.Y - StudRadius + Constants.Origin_Coord.Y;
            shape.Width = 2 * StudRadius;
            shape.Height = 2 * StudRadius;

            SolidBrush drawBrush = new SolidBrush(Color.Green);
            graphics.FillEllipse(drawBrush, shape);

        }
        public void Draw_Studs(BindingList<Stud> Studs)
        {
            foreach (var item in Studs)
            {
                Draw_Stud(item.circle);
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
            Pen dashPen = new Pen(Color.Red, 1);
            dashPen.DashPattern = dashValues;

            graphics.DrawLine(dashPen, Constants.Origin_Coord.X, Constants.Origin_Coord.Y, Constants.Origin_Coord.X, Constants.WorkZoneLimits_Coord.Y);
            graphics.DrawLine(dashPen, Constants.Origin_Coord.X, Constants.WorkZoneLimits_Coord.Y, Constants.WorkZoneLimits_Coord.X, Constants.WorkZoneLimits_Coord.Y);
            graphics.DrawLine(dashPen, Constants.WorkZoneLimits_Coord.X, Constants.WorkZoneLimits_Coord.Y, Constants.WorkZoneLimits_Coord.X, Constants.Origin_Coord.Y);
            graphics.DrawLine(dashPen, Constants.WorkZoneLimits_Coord.X, Constants.Origin_Coord.Y, Constants.Origin_Coord.X, Constants.Origin_Coord.Y);


        }


        public void OpenSVG()
        {
            open = true;
            svg = SvgDocument.Open(path);
        }

        public void CloseSVG()
        {
            open = false;
            svg = null;
        }

        private void RenderSVG()
        {
            // Create rectangle for source image.
            //RectangleF srcRect = svg.Bounds;
            RectangleF srcRect = new Rectangle((int)Constants.Origin_Coord.X, (int)Constants.Origin_Coord.Y, (int)svg.Bounds.Width, (int)svg.Bounds.Height);
            //DEBUG
            MessageBox.Show("look at " + srcRect.X + " ; " + srcRect.Y);

            //create image.
            Image newImage = Image.FromFile(path);

            //coordinates for upper-left corner of image.
            float x = 50.0f;
            float y = 50.0f;


            GraphicsUnit units = GraphicsUnit.Pixel;


            // Draw image to screen.
            graphics.DrawImage(newImage, x, y, srcRect, units);
            //graphics.DrawImage(newImage, x, y, new Rectangle(50,50,300,450), units);

        }

        public void DrawSVG()
        {
            if (open)
            {
                RenderSVG();
            }
        }

        private void Draw_ReferenceCircles()
        {
            float radius = 5.0f;
            DrawCircle(Constants.Origin_Coord.X + radius + 5, Constants.Origin_Coord.Y - radius, radius);
        }

        public void Paint(BindingList<Stud> Studs)
        {
            //draw the basic from of the workzone
            //including but not restricted to :
            //the background, the rectangular coordinate system, the scale
            //the workzone, the landmarks

            graphics.Clear(Color.Black);


            Draw_CoordSystem();
            Draw_WorkZoneLimits();
            Draw_ReferenceCircles();
            DrawSVG();
            Draw_Studs(Studs);

        }


    }
}
