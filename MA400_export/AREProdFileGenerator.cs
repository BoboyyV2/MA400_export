using ACadSharp;
using ACadSharp.Entities;
using CSMath;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MA400_export
{
    public class AREProdFileGenerator : ProdFileGenerator
    {

        public static int AREProgramSize = 1000;
        public static int AREParameterSize = 100;//TODO : remplacer les occurence des nb magiques

        public AREProdFileGenerator(BindingList<Stud> Studs, CadObjectCollection<Entity> Entities, RectangleF Dimension,
                                    PointF Offset, Scale Scalefact, double Rotation)
                                    : base(Studs, ref Entities, Dimension, Offset, Scalefact, Rotation)
        {

        }

        /**
        * <summary>Generate the files necessary for the machine to work and save them to the \daten and \cnc folder</summary>
        * <param name="fileID">The ProgramNumber of the files to write.</param>
        * <param name="Data">The GeneratorData containing the data to write in the files</param>
        */
        public override void GenerateProductionFiles(string name, GeneratorData Data)
        {
            string arepath = Path.GetDirectoryName(name);

            try
            {
                Directory.CreateDirectory(arepath);
                Util.SetPermissions(arepath);
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
            using (StreamWriter sw = File.CreateText(name))
            {
                WriteParameters(sw);
                WriteStuds(sw);
                //WriteComments(sw);
                //Pas de commentaires dans le fichier are
            }

        }

        /**
         * <summary>Write all the parameters inside the ARE file </summary>
         * <remarks>uses the saved paramaters, not the current so make sure to save the parameters</remarks>
         */
        private void WriteParameters(StreamWriter sw)
        {
            for (int i = 0; i < PTS_300_CURRENT_PARAM.Length; i++)
            {
                sw.WriteLine(PTS_300_CURRENT_PARAM[i]);
            }
        }

        /**
         * <summary>Write the studs in the ARE file in a somewhat sorted order.</summary>
         */
        private void WriteStuds(StreamWriter sw)
        {
            List<Stud> SortedStuds = Util.SortStuds(Studs);
            int numberOfLineToFill = 900;

            foreach (Stud stud in SortedStuds)
            {
                sw.WriteLine( (stud.circle.Center.X).ToString("0.0000") );//limite le nombre de 0 quand meme
                sw.WriteLine( (stud.circle.Center.Y).ToString("0.0000") );
                numberOfLineToFill -= 2;
            }
            // Fill the remaining lines with default values if necessary
            for (int i = 0; i < numberOfLineToFill; i++)
            {
                sw.WriteLine(0);
            }
        }

        //unused
        private void WriteComments(StreamWriter sw)
        {
            for (int i = 0; i < PTS_300_CURRENT_PARAM.Length; i++)
            {
                if (String.IsNullOrEmpty(PTS_300_CURRENT_COMMENTS[i]))
                {
                    sw.WriteLine("$"+i+",1");
                    sw.WriteLine(PTS_300_CURRENT_PARAM[i]);
                }
            }
        }

        /*_____________________________________PTS_300_PARAMETERS_____________________________________*/




        /**
         * <summary>Get the default parameters for the PTS300 machine from the default file, it should come with the app on installation</summary>
         * <remarks>if the default file is not found, it will be generated from scratch</remarks>
         */
        public void GetDefaultPTS300Parameters()
        {
            string paramPath = Constants.MainPath + Constants.paramPath;
            string defaultParamPath = paramPath + @"default\PTS_300_PARAM.txt";

            //TODO make sure the file is always present in the default folder and contains the default parameters.

            if (!File.Exists(defaultParamPath))
            {
                MessageBox.Show("Le fichier de paramètres par défaut pour la machine PTS300 est manquant, création des paramètres par défaut.");
                GenerateDefaultPTS300Parameters();
            }
            //read the default file to get the default parameters
            ReadPTS300Parameters(defaultParamPath);

        }


        /**
         * <summary>generate the default parameters file from scratch.</summary>
         */
        public void GenerateDefaultPTS300Parameters()
        {
            string defaultParamPath = Constants.MainPath + Constants.paramPath + @"\default\";
            //create the folder if it doesn't exist
            try
            {
                Directory.CreateDirectory(defaultParamPath);
                Util.SetPermissions(defaultParamPath);
            }
            catch (DirectoryNotFoundException e)
            {
                MessageBox.Show("Dossier des paramètres par défaut introuvable" + e.Message);
                Application.Exit();
                return ;
            }
            catch (UnauthorizedAccessException e)
            {
                MessageBox.Show("Impossible d'acceder au dossier des paramètres par défaut " + e.Message);
                Application.Exit();
                return;
            }
            catch
            {
                MessageBox.Show("Échec de creation du fichier de paramètre par défaut");
                Application.Exit();
                return;
            }
            using (StreamWriter sw = File.CreateText(defaultParamPath + "PTS_300_PARAM.txt"))
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
         * <param name="filePath">The full path to the parameter file</param>
         */
        public void ReadPTS300Parameters(string filePath)
        {

            string[] file;
            try
            {
                file = File.ReadAllLines(filePath);
            }

            catch (Exception e)
            {
                MessageBox.Show("echec de l'ouverture des paramètres de la machine PTS300: " + e.Message + Environment.NewLine + "vous pouvez reinitialiser les paramètres");
                return;
            }

            //get les valeurs des paramètres du fichier, si une valeur n'est pas un entier valide, affiche un message d'erreur et arrête la lecture des paramètres
            for (int lineIndex = 0; lineIndex < AREParameterSize; lineIndex++)
            {
                try
                {

                    PTS_300_CURRENT_PARAM[lineIndex] = Convert.ToInt32(file[lineIndex]);
                }
                catch (Exception e)
                {
                    MessageBox.Show("echec de la lecture d'un paramètre" + e.Message + Environment.NewLine + "valeur de paramètre invalide : " + file[lineIndex]);
                    return;
                }
            }
            PTS_300_CURRENT_COMMENTS = new string[AREParameterSize];

            //get les commentaires des paramètres du fichier si il y en a
            for (int lineIndex = AREParameterSize; lineIndex < file.Length; lineIndex++)
            {
                //for each comment, get the line it belongs to
                //format =
                //$linenumer,1
                //comment
                
                int line = -1;
                string input = (file[lineIndex]);
                   
                // Remove $ and ,1 characters => isolate the value
                string cleaned = input.Replace("$", "").Split(',')[0];

                //parse the cleaned string to an integer
                try
                {
                    int.TryParse(cleaned, out line);
                }
                catch(Exception e)

                {
                    MessageBox.Show("Erreur lors de la lecture des commentaires du fichier de paramètres. " + e.Message );
                    return;
                }
                try
                {
                    PTS_300_CURRENT_COMMENTS[line] = file[++lineIndex];
                }
                catch (Exception e)
                {
                    MessageBox.Show("Numero de commentaire invalide. " + e.Message);
                }
                
            }

        }


        /**
         * <summary>Write the saved parameters to the parameters file.</summary>
         * <remarks>should be done everytime the save button is pressed and whenever a full reset occur.</remarks>
         */
        public void writePTS300Parameters()
        {
            string paramPath = Constants.MainPath + Constants.paramPath + @"PTS_300_PARAM.txt";
            using (StreamWriter sw = new StreamWriter(paramPath))
            {
                //PARAMS
                try
                {

                    for (int i = 0; i < PTS_300_SAVE_PARAM.Length; i++)
                    {
                        sw.WriteLine(PTS_300_SAVE_PARAM[i]);
                    }

                }
                catch (Exception e)
                {
                    MessageBox.Show("echec de l'écriture des paramètres de la machine PTS300: " + e.Message);
                    return;
                }

                //COMMENTS
                try
                {

                    for (int i = 0; i < PTS_300_SAVE_COMMENTS.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(PTS_300_SAVE_COMMENTS[i]))
                        {
                            sw.WriteLine("$" + (i+1).ToString() + ",1");
                            sw.WriteLine(PTS_300_SAVE_COMMENTS[i]);
                        }
                    }

                }
                catch (Exception e)
                {
                    MessageBox.Show("echec de l'écriture des paramètres de la machine PTS300: " + e.Message);
                    return;
                }
            }
        }



        /**
         * <summary>Set the values of the parameters and comments to that of the default file</summary>
         */
        public void SaveCurrentValues()
        {
            PTS_300_CURRENT_PARAM.CopyTo(PTS_300_SAVE_PARAM, 0);
            PTS_300_CURRENT_COMMENTS.CopyTo(PTS_300_SAVE_COMMENTS, 0);

        }

        /**
         * <summary>Reset the values of the parameters and comments to those of the default file, in case the user wants to discard his changes</summary>
         * <remarks>this does not change the saved values.</remarks>
         */
        public void ResetCurrentValues()
        {
            PTS_300_SAVE_PARAM.CopyTo(PTS_300_CURRENT_PARAM, 0);
            PTS_300_SAVE_COMMENTS.CopyTo(PTS_300_CURRENT_COMMENTS, 0);
        }

        internal void ReadRecievedAREProgram(object[] recieved)
        {
            if(recieved.Length != AREProgramSize)// taille d'un program
            {
                MessageBox.Show("Taille du programme reçu invalide.");
                return;
            }

            //params
            for(int i = 0 ; i < AREParameterSize ; i++)
            {
                try
                {
                    PTS_300_CURRENT_PARAM[i] = Convert.ToInt32(recieved[i]);
                }
                catch (Exception e)
                {
                    MessageBox.Show("echec de la lecture d'un paramètre" + e.Message + Environment.NewLine + "valeur de paramètre invalide : " + recieved[i]);
                    return;
                }
            }

            int nbStuds = 0;
            double X = 0, Y = 0;
            for (int i = AREParameterSize ;i < AREProgramSize ; i++)
            {
            
            
            
                //X
                try
                {
                    X = Convert.ToDouble(recieved[i++], CultureInfo.GetCultureInfo("fr-FR"));
                }
                catch (Exception e)
                {
                    MessageBox.Show("Fichier ARE mal formé, lecture de la position X d'un goujon impossible, passage au suivant" + e.Message);
                    i++;
                    continue;
                }
                //Y
                try
                {
                    Y = Convert.ToDouble(recieved[i], CultureInfo.GetCultureInfo("fr-FR"));
                }
                catch (Exception e)
                {
                    MessageBox.Show("Fichier ARE mal formé, lecture de la position Y d'un goujon impossible, passage au suivant" + e.Message);
                    i++;
                    continue;
                }

                //ajout du goujon à la liste
                if (X != 0 || Y != 0)//si les deux sont à 0 alors c'est juste des 0 pour fill le fichier
                {
                    nbStuds++;
                    Studs.Add(new Stud(new Circle() { Center = new XYZ(X, Y, 0), Radius = Constants.StudRadius3, Color = ACadSharp.Color.Green }));
                }
            }
        }



        /*____________________________________________________________________________________________________*/
    }
}
