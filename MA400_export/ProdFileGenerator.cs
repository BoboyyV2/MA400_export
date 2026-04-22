using ACadSharp;
using ACadSharp.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MA400_export
{

    internal class ProdFileGenerator
    {

        private int reoccuring_number = 233; //TODO savoir qui c'est celui là
        private BindingList<Stud> Studs;//can't use ref or pointer it seems

        private int ProgrammeNumber;
        private string Company;
        private string PartDesignation;
        private string PartNumber;
        private string DrawingNumber;
        private string Notes;
        private Machine machine;

        //dates toujours sur 10 char DD.MM.YYYY si non renseigné : on écrit 10 espaces.
        //TODO gestion chaine vide 
        private string DateCreation;
        private string DateModification;

        private Rectangle Dimension;
        private System.Drawing.Point Offset;



        public ProdFileGenerator(ref BindingList<Stud> Studs)//should work but C# and ref being themselves 
        {
            this.Studs = Studs; 
        }

        /*_____________________________________EXPORT_____________________________________*/


        private void WriteEmptyLine(StreamWriter sw, int numlines)
        {
            for (int i = 0; i < numlines; i++)
            {
                sw.WriteLine(Environment.NewLine);
            }
        }

        private void WriteLine0(StreamWriter sw, int numlines)
        {
            for (int i = 0; i < numlines; i++)
            {
                sw.WriteLine("0"+ Environment.NewLine);
            }
        }

        private void WriteLineStar(StreamWriter sw, int numlines)
        {
            for (int i = 0; i < numlines; i++)
            {
                sw.WriteLine("*" + Environment.NewLine);
            }
        }

        /**
         * <summary>Generate the files necessary for the machine to work and save them to the \daten and \cnc folder</summary>
         */
        public void GenerateProductionFiles(int fileID)
        {
            string Daten = Constants.outputpath + @"Daten\";
            string Cnc = Constants.outputpath + @"Cnc\";

            Directory.CreateDirectory(Daten);
            Directory.CreateDirectory(Cnc);

            //Daten
            GenerateAN4(Daten + fileID);
            GenerateBOL(Daten + fileID);
            GenerateBST(Daten + fileID);
            GenerateDAT(Daten + fileID);
            GenerateDUP(Daten + fileID);
            GenerateGHP(Daten + fileID);
            GenerateLAY(Daten + fileID);
            GenerateMIN(Daten + fileID);
            GenerateNC(Daten + fileID);
            GenerateST(Daten + fileID);//pas toujours
            GenerateVER(Daten + fileID);

            //Cnc
            GenerateCNC(Cnc + fileID);
        }

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
            
            sw.WriteLine( "N1 (P"+ProgrammeNumber+")" + Environment.NewLine);
            string machineName;
            switch (machine)
            {
                case Machine.MA400S:
                    machineName = "MA-400-S";
                    break;
                default:
                    machineName = "other";//TODO
                    break;
            }
            sw.WriteLine( "N2 (Steuerung        : " + machineName + ")" + Environment.NewLine);
            sw.WriteLine( "N3 (Firma            : " + Company + ")" + Environment.NewLine);
            sw.WriteLine( "N4 (Teilebezeichnung : " + PartDesignation + ")" + Environment.NewLine);
            sw.WriteLine( "N5 (Teilenummer      : " + PartNumber + ")" + Environment.NewLine);
            sw.WriteLine( "N6 (Zeichnungsnummer : " + DrawingNumber + ")" + Environment.NewLine);
            sw.WriteLine( "N7 (Erstellt am      : " + DateCreation + ")" + Environment.NewLine);
            sw.WriteLine( "N8 (Geändert am      : " + DateModification + ")" + Environment.NewLine);
            string nullpoint = $"X{Dimension.X}  Y{Dimension.Y} Z0/{Dimension.Width}/{Dimension.Height}";
            sw.WriteLine( "N9 (Nullpunkt        : " + nullpoint + ")" + Environment.NewLine);
            //9 lignes
        }

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

            sw.WriteLine("N10 M20" + Environment.NewLine);
            sw.WriteLine("N10 M80" + Environment.NewLine);
            sw.WriteLine("N11 F100" + Environment.NewLine);
            sw.WriteLine("N12 (Massespanner zu)" + Environment.NewLine);
            sw.WriteLine("N13 M08" + Environment.NewLine);
            sw.WriteLine("N14 T0" + Environment.NewLine);
            sw.WriteLine("N15 G90" + Environment.NewLine);
            sw.WriteLine("N18 (Bolzenschweisszyklus ein)" + Environment.NewLine);
            //8 lignes
            //mais fini à N18
            //on reprend a partir de N19
        }

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

            sw.WriteLine($"N{N} M20" + Environment.NewLine);
            sw.WriteLine($"N{++N} G00 X{firstStud.circle.Center.X.ToString("0.0")} Y{firstStud.circle.Center.Y.ToString("0.0")}" + Environment.NewLine);
            sw.WriteLine($"N{++N}(Massespanner auf)" + Environment.NewLine);
            sw.WriteLine($"N{++N} M09" + Environment.NewLine);
            sw.WriteLine($"N{++N}(Werkstueck entnehmen)" + Environment.NewLine);
            sw.WriteLine($"N{++N} M30" + Environment.NewLine);
            sw.WriteLine("%" + Environment.NewLine);

            //on va de 1 en 1 
        }

        private void GenerateCNCCommand(StreamWriter sw , int N, Stud stud)
        {
            /*
            N19 G00 X39 Y30
            N19 M81
             */
            sw.WriteLine($"N{N} G00 X{stud.circle.Center.X.ToString("0.0")} Y{stud.circle.Center.Y.ToString("0.0")}" + Environment.NewLine);
            sw.WriteLine($"N{N} M81" + Environment.NewLine);


        }

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

        private void GenerateBOL(string path)
        {
            using (StreamWriter sw = File.CreateText(path + ".AN4"))
            {
                WriteLine0(sw, 1);
                sw.WriteLine(" " + reoccuring_number + Environment.NewLine);//nombre qui apparait partout
                sw.WriteLine(Studs.Count.ToString());//le nombre de goujons 
            }
        }

        private void GenerateBST(string path)
        {
            using (StreamWriter sw = File.CreateText(path + Environment.NewLine))
            {
                WriteLine0(sw, 5);
                sw.WriteLine(" " + reoccuring_number + Environment.NewLine);

                WriteEmptyLine(sw, 5);//TODO, le nombre de ligne n'est pas fix, c'est quoi ?

                int magicnumber = 2;
                sw.WriteLine(magicnumber + Environment.NewLine);

                WriteEmptyLine(sw, 5);//TODO, le nombre de ligne n'est pas fix, c'est quoi ?

                WriteLine0(sw, 5);

            }
        }

        private void GenerateDAT(string path)
        {
            using (StreamWriter sw = File.CreateText(path + ".DAT"))
            {
                sw.WriteLine(Company + Environment.NewLine);
                sw.WriteLine(PartDesignation + Environment.NewLine);
                sw.WriteLine(PartNumber + Environment.NewLine);
                sw.WriteLine(DrawingNumber + Environment.NewLine);
                sw.WriteLine(Notes + Environment.NewLine);
                sw.WriteLine(DateCreation + Environment.NewLine);
                sw.WriteLine(DateModification + Environment.NewLine);

            }
        }

        private void GenerateDUP(string path)
        {
            using (StreamWriter sw = File.CreateText(path + ".DUP"))
            {
                WriteLine0(sw, 1);
                sw.WriteLine("1" + Environment.NewLine);
                WriteLine0(sw, 4);

            }
        }

        private void GenerateGHP(string path)
        {
            using (StreamWriter sw = File.CreateText(path + ".GHP"))
            {
                //TODO graphic par, commandes sur 11 lignes

            }
        }

        private void GenerateLAY(string path)
        {
            using (StreamWriter sw = File.CreateText(path + ".LAY"))
            {
                sw.WriteLine(Dimension.Left + Environment.NewLine);
                sw.WriteLine(Dimension.Top + Environment.NewLine);
                sw.WriteLine(Dimension.Right + Environment.NewLine);
                sw.WriteLine(Dimension.Bottom + Environment.NewLine);


                //TODO c'est quoi le reste ?, implem

            }
        }

        private void GenerateMIN(string path)
        {
            using (StreamWriter sw = File.CreateText(path + ".MIN"))
            {
                WriteLine0(sw, 8);
                //c'est tout ?

            }
        }

        private System.Drawing.Point GetSpacialPosition(CSMath.XYZ position)
        {
            return new System.Drawing.Point((int)(position.X - Offset.X), (int)(position.Y - Offset.Y) );
        }

        private void WriteNcCommand(StreamWriter sw, Stud stud)
        {
            sw.WriteLine("PUNKT" + Environment.NewLine);
            System.Drawing.Point RealPosition = GetSpacialPosition(stud.circle.Center);
            sw.WriteLine( RealPosition.X + Environment.NewLine);
            sw.WriteLine( RealPosition.Y + Environment.NewLine);

            //param incertains
            sw.WriteLine("0" + Environment.NewLine);
            sw.WriteLine("2" + Environment.NewLine);
            sw.WriteLine("A" + Environment.NewLine);

            //fill la commande avec des *
            WriteLineStar(sw, 13);

            //fin de la cmd
            sw.WriteLine(" " + reoccuring_number +"/1" + Environment.NewLine);
            sw.WriteLine("1" + Environment.NewLine);

        }

        private void WriteNcEmptyCommand(StreamWriter sw)
        {
            
            //fill la commande avec des *
            WriteLineStar(sw, 21);
        }


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

        private void GenerateST(string path)
        {
            using (StreamWriter sw = File.CreateText(path + ".NC"))
            {
                int magicnumber = 0;//TODO, comprendre c'est quoi
                sw.WriteLine(magicnumber + Environment.NewLine);
            }
        }

        private void GenerateVER(string path)
        {
            using (StreamWriter sw = File.CreateText(path + ".NC"))
            {
                int magicnumber = 0;//TODO, comprendre c'est quoi, 4 nombre potentiellement différent, souvent des 0
                sw.WriteLine(magicnumber + Environment.NewLine);
                sw.WriteLine(magicnumber + Environment.NewLine);
                sw.WriteLine(magicnumber + Environment.NewLine);
                sw.WriteLine(magicnumber + Environment.NewLine);

            }
        }

        /*_____________________________________EXPORT_____________________________________*/


    }
}
