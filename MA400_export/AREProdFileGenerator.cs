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
using System.Windows.Forms;

namespace MA400_export
{
    public class AREProdFileGenerator : ProdFileGenerator
    {
        public AREProdFileGenerator(BindingList<Stud> Studs, CadObjectCollection<Entity> Entities, RectangleF Dimension,
                                    PointF Offset, GeneratorData Data, Scale Scalefact, double Rotation)
                                    : base(Studs, Entities, Dimension, Offset, Data, Scalefact, Rotation)
        {

        }

        /**
        * <summary>Generate the files necessary for the machine to work and save them to the \daten and \cnc folder</summary>
        * <param name="fileID">The ProgramNumber of the files to write.</param>
        */
        public override void GenerateProductionFiles(string name)
        {
            string are = Properties.Settings.Default.OutputPath + Constants.ArePath;


            try
            {
                Directory.CreateDirectory(are);
                Util.SetPermissions(are);
            }
            catch (DirectoryNotFoundException e)
            {
                MessageBox.Show("Erreur lors de la generation des fichiers" + e.Message);
            }
            catch (UnauthorizedAccessException e)
            {
                MessageBox.Show("Erreur lors de la generation des fichiers" + e.Message);
            }
            catch
            {
                MessageBox.Show("Erreur lors de la generation des fichiers");
            }
            string filepath = are + name + ".are";
            using (StreamWriter sw = File.CreateText(filepath))
            {
                WriteParameters(sw);
                writeStuds(sw);
            }

        }

        private void WriteParameters(StreamWriter fw)
        {
            for (int i = 0; i < PTS_300_PARAM.Length; i++)
            {
                fw.WriteLine(PTS_300_PARAM[i]);
            }
        }

        private void writeStuds(StreamWriter fw)
        {
            List<Stud> SortedStuds = Util.SortStuds(Studs);
            int numberOfLineToFill = 900;

            foreach (Stud stud in SortedStuds)
            {
                fw.WriteLine(stud.circle.Center.X);
                fw.WriteLine(stud.circle.Center.Y);
                numberOfLineToFill -= 2;
            }
            // Fill the remaining lines with default values if necessary
            for (int i = 0; i < numberOfLineToFill; i++)
            {
                fw.WriteLine(0);
            }
        }

        /*_____________________________________PTS_300_PARAMETERS_____________________________________*/




        /**
         * <summary>Get the default parameters for the PTS300 machine from the default file, it should come with the app on installation</summary>
         * <remarks>if the default file is not found, all parameters will be set to 0 and a message will be displayed</remarks>
         */
        public void GetDefaultPTS300Parameters()
        {

            string defaultParamPath = Constants.MainPath + Constants.paramPath + @"\default\PTS_300_PARAM.txt";

            //TODO make sure the file is always present in the default folder and contains the default parameters.

            if (!File.Exists(defaultParamPath))
            {
                MessageBox.Show("Le fichier de paramètres par défaut pour la machine PTS300 est manquant, création des paramètres par défaut.");
                GenerateDefaultPTS300Parameters();
                return;
            }
            ReadPTS300Parameters(defaultParamPath + @"\PTS_300_PARAM.txt");

        }

        public void GenerateDefaultPTS300Parameters()
        {
            // Implementation for generating default parameters
            string defaultParamPath = Constants.MainPath + Constants.paramPath + @"default\PTS_300_PARAM.txt";

            using (StreamWriter sw = new StreamWriter(defaultParamPath))
            {
                writeHeaderParam(sw);
                writeMiddleParam(sw);
                writeTailParam(sw);
            }
        }

        /**
         * <summary>write the header parameters for the PTS300 machine (the 31 first lines).</summary>
         */
        private void writeHeaderParam(StreamWriter sw)
        {
            sw.WriteLine(2);    //unknown
            sw.WriteLine(12);   //unknown
            sw.WriteLine(1);    //static
            sw.WriteLine(33);   //static
            sw.WriteLine(515);  //static
            sw.WriteLine(0);    //static ?? diff tableau & fichier
            sw.WriteLine(1);    //unknown
            WriteLine0(sw, 12);
            sw.WriteLine(5000); //static
            sw.WriteLine(200);  //static
            sw.WriteLine(5000); //static
            WriteLine0(sw, 2);
            sw.WriteLine(1);   //unknown
            WriteLine0(sw, 4);
            sw.WriteLine(-20);  //static
            sw.WriteLine(20);   //static
        }

        /**
         * <summary>write the middle parameters for the PTS300 machine (supposedly 49 lines with the 0 value).</summary>
         */
        private void writeMiddleParam(StreamWriter sw)
        {
            WriteLine0(sw, 49);
        }

        /**
         * <summary>write the tail parameters for the PTS300 machine (the last 20).</summary>
         */
        private void writeTailParam(StreamWriter sw)
        {
            sw.WriteLine(109);    //unknown
            sw.WriteLine(110);    //unknown
            WriteLine0(sw, 18);
            //fin

        }

        /**
         * <summary>read the parameters of the PTS300 machine from a file and store them in the application for later use.</summary>
         */
        public void ReadPTS300Parameters(string filename)
        {
            string paramPath = Constants.MainPath + Constants.paramPath + filename;
            string[] file;
            try
            {
                file = File.ReadAllLines(paramPath);
                //TODO read the parameters from the file and store them in the application for later use
            }
            catch (Exception e)
            {
                MessageBox.Show("echec de l'ouverture des paramètres de la machine PTS300: " + e.Message + Environment.NewLine + "vous pouvez reinitialiser les paramètres");
                return;
            }

            for (int numline = 0; numline < file.Length; numline++)
            {
                try
                {
                    PTS_300_PARAM[numline] = Convert.ToInt32(file[numline]);
                }
                catch (Exception e)
                {
                    MessageBox.Show("echec de la lecture d'un paramètre" + e.Message + Environment.NewLine + "valeur de paramètre invalide : " + file[numline]);
                    return;
                }
            }

        }

        private void writePTS300Parameters()
        {
            string paramPath = Constants.MainPath + Constants.paramPath + @"PTS_300_PARAM.txt";
            try
            {
                using (StreamWriter sw = new StreamWriter(paramPath))
                {
                    for (int i = 0; i < PTS_300_PARAM.Length; i++)
                    {
                        sw.WriteLine(PTS_300_PARAM[i]);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("echec de l'écriture des paramètres de la machine PTS300: " + e.Message);
                return;
            }
        }



        /*____________________________________________________________________________________________________*/
    }
}
