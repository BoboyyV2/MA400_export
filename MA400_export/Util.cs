using ACadSharp.Entities;
using CSMath;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MA400_export
{
    public static class Util
    {

        /**
         * <summary>Adjust a point to the new scale</summary>
         * <returns>a new point that have been adjusted to the new scale</returns>
         */
        public static XYZ AdjustPoint(XYZ point, PointF offset, RectangleF dimension, Scale scale)
        {

            double posx = point.X + offset.X;
            double posy = point.Y + offset.Y;
            if (scale.Xscale < 0)
            {
                posx = -point.X + offset.X; 
            }
            if (scale.Yscale < 0)
            {
                posy = -point.Y + offset.Y;
            }
            
            return new XYZ(posx, posy, 0);
        }

        /**
         * <summary>Adjust a point to the new scale, take the GPH properties into account</summary>
         * <returns>a new point that have been adjusted to the new scale</returns>
         */
        public static XYZ AdjustPointGPH(XYZ point, PointF offset, RectangleF dimension, Scale scale)
        {

            double posx = point.X + offset.X;
            double posy = point.Y + offset.Y;
            if (scale.Xscale < 0)
            {
                posx = -point.X + offset.X + dimension.Width;
            }
            if (scale.Yscale < 0)
            {
                posy = -point.Y + offset.Y + dimension.Height;
            }

            return new XYZ(posx, posy, 0);
        }

        /**
         * <returns>the absolute distance between the 2 studs on a 2D plan</returns>
         */
        public static double getStudDistance(Circle from, Circle to)
        {
            return Math.Sqrt(Math.Pow(from.Center.X - to.Center.X, 2) + Math.Pow(from.Center.Y - to.Center.Y, 2));
        }
        /**
         * <returns>the absolute distance between the 2 studs on a 2D plan</returns>
         */
        public static double getStudDistance(System.Drawing.Point from, Circle to)
        {
            return Math.Sqrt(Math.Pow(from.X - to.Center.X, 2) + Math.Pow(from.Y - to.Center.Y, 2));
        }

        public static double getStudDistance(PointF from, Circle to)
        {
            return Math.Sqrt(Math.Pow(from.X - to.Center.X, 2) + Math.Pow(from.Y - to.Center.Y, 2));
        }


        /**
         * <summary>check if the stud are all further away than both there radius combined</summary>
         * <returns>true if it is possible to add the stud false otherwise</returns>
         */
        public static bool IsPossibleToAddStud(Circle Stud, IEnumerable<Stud> Studs)
        {
            foreach (Stud candidate in Studs)
            {
                if (getStudDistance(candidate.circle, Stud) < (Stud.Radius + candidate.circle.Radius))
                {
                    MessageBox.Show("position invalide pour poser un goujon.\r\nTrop près d'un goujon existant");
                    return false;
                }
            }
            return true;
        }

        /**
         * <summary>check if the stud are all further away than both there radius combined</summary>
         * <returns>true if it is possible to add the stud false otherwise</returns>
         */
        public static bool IsPossibleToAddStud(Stud Stud, IEnumerable<Stud> Studs)
        {
            foreach (Stud candidate in Studs)
            {
                if (getStudDistance(candidate.circle, Stud.circle) < (Stud.circle.Radius + candidate.circle.Radius))
                {
                    MessageBox.Show("position invalide pour poser un goujon.\r\nTrop près d'un goujon existant");
                    return false;
                }
            }
            return true;
        }

        
    }
}
