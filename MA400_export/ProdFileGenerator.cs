using ACadSharp;
using ACadSharp.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
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
        //TODO gestion chaine vide 
        public string DateCreation;
        public string DateModification;

    }

    /**
      * <summary>
      * The <c>ProdFileGenerator</c> Class is an extention of the <c>FileSystem</c> class used to generate de production files
      * </summary>
      */
    internal class ProdFileGenerator
    {

        private int reoccuring_number = 233; //TODO savoir qui c'est celui là
        private BindingList<Stud> Studs;//can't use ref or pointer it 
        private CadObjectCollection<Entity> Entities;//same shee

        private GeneratorData Data;

        private RectangleF Dimension;
        private PointF Offset;
        private Scale Scalefact;



        public ProdFileGenerator(ref BindingList<Stud> Studs, CadObjectCollection<Entity> Entities, RectangleF Dimension, PointF Offset, GeneratorData Data, Scale Scalefact)//should work but C# and ref being themselves 
        {
            this.Studs = Studs;
            this.Entities = Entities;
            this.Dimension = Dimension;
            this.Offset = Offset;
            this.Data = Data;
            this.Scalefact = Scalefact;

        }


        /*_____________________________________UTIL_____________________________________*/


        /**
         * <summary>Turn a double value into a string correctly formated for output files, it take care of rounding and significant number display.</summary>
         * <returns>the formated string corresponding to value.</returns>
         */
        private string FormatValue(double value)
        {
            CultureInfo culture = System.Globalization.CultureInfo.InvariantCulture;
            
            double deci = (value % 1);
            if (deci > 0.951)
            {
                deci = 1; //arrondi pour écarter l'imprécision des float
            }

            if ( ( (deci * 100) % 10 ) >= 1)
            {
                return value.ToString("0.00", culture);
            }
            if (((deci * 10) % 10) >= 1)
            {
                return value.ToString("0.0", culture);
            }

            return value.ToString("0", culture) ;
        }


        /**
         * <summary>Write numlines empty line into the stream connected to sw.</summary>
         * <param name="sw">The StreamWriter connected to the outputf file.</param>
         * <param name="numlines">The number of empty lines to write</param>
         */
        private void WriteEmptyLine(StreamWriter sw, int numlines)
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
        private void WriteLine0(StreamWriter sw, int numlines)
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
        private void WriteLineStar(StreamWriter sw, int numlines)
        {
            for (int i = 0; i < numlines; i++)
            {
                sw.WriteLine("*" );
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
            if(Scalefact.Xscale > 0)
            {
                posx = (float)((position.X - Offset.X) * Scalefact.Xscale);
            }else
            {
                posx = (float)(Dimension.Width + ( (position.X - Offset.X) * Scalefact.Xscale ) );
            }

            if (Scalefact.Yscale > 0)
            {
                posy = (float)((position.Y - Offset.Y) * Scalefact.Yscale);
            }
            else
            {
                posy = (float)(Dimension.Height + ((position.Y - Offset.Y) * Scalefact.Yscale));
            }

            return new System.Drawing.PointF( posx, posy );
        }


        /*_____________________________________EXPORT_____________________________________*/


        /**
         * <summary>Generate the files necessary for the machine to work and save them to the \daten and \cnc folder</summary>
         * <param name="fileID">The ProgramNumber of the files to write.</param>
         */
        public void GenerateProductionFiles(int fileID)
        {
            string Daten = Properties.Settings.Default.OutputPath + Constants.DatenPath;
            string Cnc = Properties.Settings.Default.OutputPath + Constants.CncPath;

            Directory.CreateDirectory(Daten);
            Directory.CreateDirectory(Cnc);

            //Daten
            GenerateAN4(Daten + fileID);
            GenerateBOL(Daten + fileID);
            GenerateBST(Daten + fileID);
            GenerateDAT(Daten + fileID);
            GenerateDUP(Daten + fileID);
            GenerateGPH(Daten + fileID);
            GenerateLAY(Daten + fileID);
            GenerateMIN(Daten + fileID);
            GenerateNC(Daten + fileID);
            GenerateST(Daten + fileID);//pas toujours
            GenerateVER(Daten + fileID);

            //Cnc
            GenerateCNC(Cnc + fileID);
        }


        /*_____________________________________CNC_____________________________________*/


        /**
         * <summary>write the header of the CNC file into the file connected to sw.</summary>
         * <param name="sw">The StreamWriter connected to the outputf file.</param>
         */
        private void GenerateCNCHeader(StreamWriter sw)
        {
            /*
            N1 (P0151)
            N2 (Steuerung        : MA-400-S)
            N3 (Firma            : CA)
            N4 (Teilebezeichnung : CA 1150)
            N5 (Teilenummer      : )
            N6 (Zeichnungsnummer : )
            N7 (Erstellt am      : 05.09.2019)
            N8 (Geändert am      : 07.11.2019)
            N9 (Nullpunkt        : X0 Y0 Z0/400/160)
             */
            
            sw.WriteLine( "N1 (P"+ Data.ProgramNumber +")" );
            string machineName;
            switch (Data.machine)
            {
                case Machine.MA400S:
                    machineName = "MA-400-S";
                    break;
                default:
                    machineName = "other";//TODO
                    break;
            }
            sw.WriteLine( "N2 (Steuerung        : " + machineName + ")" );
            sw.WriteLine( "N3 (Firma            : " + Data.Company  + ")" );
            sw.WriteLine( "N4 (Teilebezeichnung : " + Data.PartDesignation + ")" );
            sw.WriteLine( "N5 (Teilenummer      : " + Data.PartNumber + ")" );
            sw.WriteLine( "N6 (Zeichnungsnummer : " + Data.DrawingNumber + ")" );
            sw.WriteLine( "N7 (Erstellt am      : " + Data.DateCreation + ")" );
            sw.WriteLine( "N8 (Geändert am      : " + Data.DateModification + ")" );
            string nullpoint = $"X{Dimension.X} Y{Dimension.Y} Z0/{Dimension.Width}/{Dimension.Height}";
            sw.WriteLine( "N9 (Nullpunkt        : " + nullpoint + ")" );
            //9 lignes
        }

        /**
         * <summary>write the initialization of the CNC file into the file connected to sw.</summary>
         * <param name="sw">The StreamWriter connected to the outputf file.</param>
         */
        private void GenerateCNCInit(StreamWriter sw)
        {
            /*
            N10 M20
            N10 M80
            N11 F100
            N12 (Massespanner zu)
            N13 M08
            N14 T0
            N15 G90
            N18 (Bolzenschweisszyklus ein)
             */

            sw.WriteLine("N10 M20" );
            sw.WriteLine("N10 M80" );
            sw.WriteLine("N11 F100" );
            sw.WriteLine("N12 (Massespanner zu)" );
            sw.WriteLine("N13 M08" );
            sw.WriteLine("N14 T0" );
            sw.WriteLine("N15 G90" );
            sw.WriteLine("N18 (Bolzenschweisszyklus ein)" );
            //8 lignes
            //mais fini à N18
            //on reprend a partir de N19
        }


        /**
         * <summary>write the end of the CNC file into the file connected to sw.</summary>
         * <param name="sw">The StreamWriter connected to the outputf file</param>
         * <param name="N">The Current line number</param>
         * <param name="first">if there is at least one Stud that have been placed</param>
         * <param name="firstStud">The first Stud to be placed or null if there is none</param>
         */
        private void GenerateCNCEnd(StreamWriter sw, int N, bool first, Stud firstStud)
        {
            /*
            N46 M20
            N47 G00 X38.8 Y30
            N48 (Massespanner auf)
            N49 M09
            N50 (Werkstueck entnehmen)
            N51 M30
            %
             */
            //PointF p = GetSpacialPosition(firstStud.circle.Center, Offset, Dimension, Scalefact);

            sw.WriteLine($"N{++N} M20" );
            if (first)
            {
                sw.WriteLine($"N{++N} G00 X{FormatValue(firstStud.circle.Center.X - 0.2f)} Y{FormatValue(firstStud.circle.Center.Y)}");
            }
            sw.WriteLine($"N{++N} (Massespanner auf)" );
            sw.WriteLine($"N{++N} M09" );
            sw.WriteLine($"N{++N} (Werkstueck entnehmen)" );
            sw.WriteLine($"N{++N} M30" );
            sw.WriteLine("%" );

            //on va de 1 en 1 
        }

        /**
         * <summary>write a CNC command into the CNC file connected to sw.</summary>
         * <param name="sw">The StreamWriter connected to the outputf file.</param>
         * <param name="N">The Current line number</param>
         * <param name="stud">The Stud to place</param>
         */
        private void GenerateCNCCommand(StreamWriter sw , int N, Stud stud)
        {
            /*
            N19 G00 X39 Y30
            N19 M81
             */
            //PointF p = GetSpacialPosition(stud.circle.Center, Offset, Dimension, Scalefact);

            sw.WriteLine($"N{N} G00 X{FormatValue(stud.circle.Center.X)} Y{FormatValue(stud.circle.Center.Y)}" );
            sw.WriteLine($"N{N} M81" );


        }


        /**
         * <summary>write the CNC file at the path specified by path</summary>
         * <param name="path">The path where to write the file.</param>
         */
        private void GenerateCNC(string path)
        {
            //TODO, besoin du formulaire
            using (StreamWriter sw = File.CreateText(path + ".CNC"))
            {
                GenerateCNCHeader(sw);
                GenerateCNCInit(sw);
                int N = 17; //pour start à 19 
                bool first = false;
                Stud firstStud = new Stud();
                //N19
                //commandes sur 2 lignes ( N19 blabla
                //                         N19 autre blabla)
                //                         N21 truc
                // => commandes 
                foreach (Stud stud in Studs)
                {
                    N += 2;//num de commande
                    //sauvegarde le premier pour écrire à la fin
                    if (!first)
                    {
                        firstStud = stud;
                        first = true;
                    }
                    GenerateCNCCommand(sw, N, stud);
                }

                GenerateCNCEnd(sw, N, first, firstStud);
            }
        }


        /*_____________________________________AN4_____________________________________*/


        /**
         * <summary>write the AN4 file at the path specified by path</summary>
         * <param name="path">The path where to write the file.</param>
         */
        private void GenerateAN4(string path)
        {
            int numlines = 6;//TODO, un moyen de savoir combien il y en a de manière auto
            using (StreamWriter sw = File.CreateText(path + ".AN4"))
            {
                WriteLine0(sw, 1);
                WriteEmptyLine(sw, numlines);
                WriteLine0(sw, 2);

            }
        }


        /*_____________________________________BOL_____________________________________*/


        /**
         * <summary>write the BOL file at the path specified by path</summary>
         * <param name="path">The path where to write the file.</param>
         */
        private void GenerateBOL(string path)
        {
            using (StreamWriter sw = File.CreateText(path + ".BOL"))
            {
                WriteLine0(sw, 1);
                sw.WriteLine(" " + reoccuring_number );//nombre qui apparait partout
                sw.WriteLine(Studs.Count.ToString());//le nombre de goujons 
            }
        }


        /*_____________________________________BST_____________________________________*/


        /**
         * <summary>write the BST file at the path specified by path</summary>
         * <param name="path">The path where to write the file.</param>
         */
        private void GenerateBST(string path)
        {
            using (StreamWriter sw = File.CreateText(path + ".BST"))
            {
                WriteLine0(sw, 5);
                sw.WriteLine(" " + reoccuring_number );

                int skippedlines = 5;//TODO, le nombre de ligne n'est pas fix, c'est quoi ?
                WriteEmptyLine(sw, skippedlines);

                int magicnumber = 2;
                sw.WriteLine(magicnumber );

                WriteEmptyLine(sw, skippedlines);

                WriteLine0(sw, 6);

            }
        }


        /*_____________________________________DAT_____________________________________*/


        /**
         * <summary>write the DAT file at the path specified by path</summary>
         * <param name="path">The path where to write the file.</param>
         */
        private void GenerateDAT(string path)
        {
            using (StreamWriter sw = File.CreateText(path + ".DAT"))
            {
                sw.WriteLine(Data.Company );
                sw.WriteLine(Data.PartDesignation );
                sw.WriteLine(Data.PartNumber );
                sw.WriteLine(Data.DrawingNumber );
                sw.WriteLine(Data.Notes );
                sw.WriteLine(Data.DateCreation );
                sw.WriteLine(Data.DateModification );

            }
        }


        /*_____________________________________DUP_____________________________________*/


        /**
         * <summary>write the DUP file at the path specified by path</summary>
         * <param name="path">The path where to write the file.</param>
         */
        private void GenerateDUP(string path)
        {
            using (StreamWriter sw = File.CreateText(path + ".DUP"))
            {
                WriteLine0(sw, 1);
                sw.WriteLine("1" );
                WriteLine0(sw, 4);

            }
        }


        /*_____________________________________GPH_____________________________________*/


        /**
         * <summary>Write a command for a line entity in the GPH file</summary>
         * <param name="sw">The StreamWriter linkde to the GPH file</param>
         * <param name="line">The line to turn into a command</param>
         */
        public void WriteGPH_LINE(StreamWriter sw, Line line)
        {
            PointF start = GetSpacialPosition(line.StartPoint, Offset, Dimension, Scalefact);
            PointF end = GetSpacialPosition(line.EndPoint, Offset, Dimension, Scalefact);
            if(start.X < 0 || start.Y < 0 || end.X < 0 || end.Y < 0)
            {
                //throw new ArgumentOutOfRangeException();
            }
            sw.WriteLine(Constants.LINE_CMD );
            sw.WriteLine( FormatValue(start.X) );
            sw.WriteLine( FormatValue(start.Y));
            sw.WriteLine( FormatValue(end.X));
            sw.WriteLine( FormatValue(end.Y));
            WriteLine0(sw, 6);
        }

        /**
         * <summary>Write a command for a line entity in the GPH file</summary>
         * <param name="sw">The StreamWriter linkde to the GPH file</param>
         * <param name="circle">The line to turn into a command</param>
         */
        public void WriteGPH_CIRCLE(StreamWriter sw, Circle circle)
        {
            PointF center = GetSpacialPosition(circle.Center, Offset, Dimension, Scalefact);
            if ( center.X < 0 || center.Y < 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            sw.WriteLine(Constants.CIRCLE_CMD );
            sw.WriteLine( FormatValue(center.X) );
            sw.WriteLine( FormatValue(center.Y) );
            WriteLine0(sw, 1);
            sw.WriteLine( FormatValue(circle.Radius));
            WriteLine0(sw, 6);
        }


        /**
         * <summary>Write a GPH command representing an entity</summary>
         * <param name="sw">The StreamWriter linkde to the GPH file</param>
         * <param name="entity">The Entity being turned into a command</param>
         */
        private void GenerateGPH_CMD(StreamWriter sw, Entity entity)
        {
            //all command are on 11 lines
            switch (entity.ObjectType)
            {
                case ObjectType.LINE:
                    {
                        Line line = (Line)entity;
                        WriteGPH_LINE(sw, line);
                        break;
                    }
                case ObjectType.CIRCLE:
                    {
                        Circle circle = (Circle)entity;
                        WriteGPH_CIRCLE(sw, circle);

                        break;
                    }
            }
        }

        /**
         * <summary>write the GPH file at the path specified by path</summary>
         * <param name="path">The path where to write the file.</param>
         */
        private void GenerateGPH(string path)
        {
            using (StreamWriter sw = File.CreateText(path + ".GPH"))
            {
                //TODO graphic par, commandes sur 11 lignes, c'est quoi ce 1er nombre
                int magicnumber = 6;
                sw.WriteLine(magicnumber );
                foreach (var entity in Entities)
                {
                    GenerateGPH_CMD(sw, entity);
                }

                
            }
        }


        /*_____________________________________LAY_____________________________________*/


        /**
         * <summary>write the LAY file at the path specified by path</summary>
         * <param name="path">The path where to write the file.</param>
         */
        private void GenerateLAY(string path)
        {
            using (StreamWriter sw = File.CreateText(path + ".LAY"))
            {
                sw.WriteLine(Dimension.Left );
                sw.WriteLine(Dimension.Top );
                sw.WriteLine(Dimension.Right );
                sw.WriteLine(Dimension.Bottom );
                WriteLine0(sw, 4);

                //TODO c'est quoi le reste ?, implem

            }
        }


        /*_____________________________________MIN_____________________________________*/


        /**
         * <summary>write the MIN file at the path specified by path</summary>
         * <param name="path">The path where to write the file.</param>
         */
        private void GenerateMIN(string path)
        {
            using (StreamWriter sw = File.CreateText(path + ".MIN"))
            {
                WriteLine0(sw, 8);
                //c'est tout ?

            }
        }



        /*_____________________________________NC_____________________________________*/


        /**
         * <summary>write a command in the NC file connected to sw.</summary>
         * <param name="sw">The StreamWriter connected to the outputf file.</param>
         * <param name="stud">The stud to place</param>
         */
        private void WriteNcCommand(StreamWriter sw, Stud stud)
        {
            sw.WriteLine("PUNKT" );
            //PointF RealPosition = GetSpacialPosition(stud.circle.Center, Offset, Dimension, Scalefact);
            sw.WriteLine( FormatValue(stud.circle.Center.X) );
            sw.WriteLine( FormatValue(stud.circle.Center.Y) );

            //param incertains
            sw.WriteLine("0" );
            sw.WriteLine("2" );
            sw.WriteLine("A" );

            //fill la commande avec des *
            WriteLineStar(sw, 13);

            //fin de la cmd
            sw.WriteLine(" " + reoccuring_number +"/1" );
            sw.WriteLine("1" );

        }


        /**
         * <summary>write the last command(empty) in the NC file connected to sw.</summary>
         * <param name="sw">The StreamWriter connected to the outputf file.</param>
         */
        private void WriteNcEmptyCommand(StreamWriter sw)
        {
            
            //fill la commande avec des *
            WriteLineStar(sw, 21);
        }


        /**
         * <summary>write the NC file at the path specified by path</summary>
         * <param name="path">The path where to write the file.</param>
         */
        private void GenerateNC(string path)
        {
            using (StreamWriter sw = File.CreateText(path + ".NC"))
            {
                sw.WriteLine(Studs.Count);
                foreach (Stud stud in Studs)
                {
                    WriteNcCommand(sw, stud);
                }

                WriteNcEmptyCommand(sw);
            }
        }


        /*_____________________________________ST_____________________________________*/


        /**
         * <summary>write the ST file at the path specified by path</summary>
         * <param name="path">The path where to write the file.</param>
         */
        private void GenerateST(string path)
        {
            using (StreamWriter sw = File.CreateText(path + ".ST"))
            {
                int magicnumber = 0;//TODO, comprendre c'est quoi
                sw.WriteLine(magicnumber );
            }
        }


        /*_____________________________________VER_____________________________________*/


        /**
         * <summary>write the VER file at the path specified by path</summary>
         * <param name="path">The path where to write the file.</param>
         */
        private void GenerateVER(string path)
        {
            using (StreamWriter sw = File.CreateText(path + ".VER"))
            {
                int magicnumber = 0;//TODO, comprendre c'est quoi, 4 nombre potentiellement différent, souvent des 0
                sw.WriteLine(magicnumber );
                sw.WriteLine(magicnumber );
                sw.WriteLine(magicnumber );
                sw.WriteLine(magicnumber );

            }
        }

        /*_____________________________________EXPORT_____________________________________*/


    }
}
