using ACadSharp;
using ACadSharp.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace MA400_export
{

    public struct GeneratorData
    {
        public int ProgramNumber;
        public string Company;
        public string PartDesignation;
        public string PartNumber;
        public string DrawingNumber;
        public string Notes;
        public Machine machine;

        //dates toujours sur 10 char DD.MM.YYYY si non renseigné : on écrit 10 espaces.
        public string DateCreation;
        public string DateModification;

    }

    /**
      * <summary>
      * The <c>ProdFileGenerator</c> Class is an extention of the <c>FileSystem</c> class used to generate de production files
      * </summary>
      */
    public abstract class ProdFileGenerator
    {

        protected int reoccuring_number = 233;

        protected BindingList<Stud> Studs;//can't use ref or pointer it 
        protected CadObjectCollection<Entity> Entities;//same shee

        protected GeneratorData Data;

        protected RectangleF Dimension;
        protected PointF Offset;
        protected Scale Scalefact;
        protected double Rotation;
        public int[] PTS_300_PARAM = new int[100];



        public ProdFileGenerator(BindingList<Stud> Studs, CadObjectCollection<Entity> Entities, RectangleF Dimension, PointF Offset, GeneratorData Data, Scale Scalefact, double Rotation)//should work but C# and ref being themselves 
        {
            this.Studs = Studs;
            this.Entities = Entities;
            this.Dimension = Dimension;
            this.Offset = Offset;
            this.Data = Data;
            this.Scalefact = Scalefact;
            this.Rotation = Rotation;

        }


        /*_____________________________________UTIL_____________________________________*/


        /**
         * <summary>Turn a double value into a string correctly formated for output files, it take care of rounding and significant number display.</summary>
         * <returns>the formated string corresponding to value.</returns>
         */
        protected string FormatValue(double value)
        {
            CultureInfo culture = System.Globalization.CultureInfo.InvariantCulture;

            return value.ToString("0.####", culture); //14max
        }


        /**
         * <summary>Write numlines empty line into the stream connected to sw.</summary>
         * <param name="sw">The StreamWriter connected to the outputf file.</param>
         * <param name="numlines">The number of empty lines to write</param>
         */
        protected void WriteEmptyLine(StreamWriter sw, int numlines)
        {
            for (int i = 0; i < numlines; i++)
            {
                sw.WriteLine();
            }
        }


        /**
         * <summary>Write numlines line containing "0" into the stream connected to sw.</summary>
         * <param name="sw">The StreamWriter connected to the outputf file.</param>
         * <param name="numlines">The number of lines to write</param>
         */
        protected void WriteLine0(StreamWriter sw, int numlines)
        {
            for (int i = 0; i < numlines; i++)
            {
                sw.WriteLine("0");
            }
        }

        /**
         * <summary>Write numlines line containing "*" into the stream connected to sw.</summary>
         * <param name="sw">The StreamWriter connected to the outputf file.</param>
         * <param name="numlines">The number of lines to write</param>
         */
        protected void WriteLineStar(StreamWriter sw, int numlines)
        {
            for (int i = 0; i < numlines; i++)
            {
                sw.WriteLine("*");
            }
        }


        /** 
         * <returns>A PointF corresponding to the position given in argument once put to the correct scale (orientation).</returns>
         * <param name="position">The raw position in the document.</param>
         */
        public static System.Drawing.PointF GetSpacialPosition(CSMath.XYZ position, PointF Offset, RectangleF Dimension, Scale Scalefact)
        {

            float posx;
            float posy;
            if (Scalefact.Xscale > 0)
            {
                posx = (float)(position.X - Offset.X);
            }
            else
            {
                posx = (float)(Dimension.Height - (position.X - Offset.X));
            }

            if (Scalefact.Yscale > 0)
            {
                posy = (float)(position.Y - Offset.Y);
            }
            else
            {
                posy = (float)(Dimension.Height - (position.Y - Offset.Y));
            }

            return new System.Drawing.PointF(posx, posy);
        }


        /*_____________________________________EXPORT_____________________________________*/

        public abstract void GenerateProductionFiles(string name);

    }

}

    