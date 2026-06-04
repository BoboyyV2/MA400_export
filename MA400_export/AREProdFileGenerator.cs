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
                                    PointF Offset, GeneratorData Data, Scale Scalefact, double Rotation, ref int[] parameters)
                                    : base(Studs, Entities, Dimension, Offset, Data, Scalefact, Rotation, ref parameters)
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
            for(int i = 0; i < parameters.Length; i++)
            {
                fw.WriteLine(parameters[i]);
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
                numberOfLineToFill -=2;
            }
            // Fill the remaining lines with default values if necessary
            for (int i = 0; i < numberOfLineToFill; i++)
            {
                fw.WriteLine(0);
            }
        }
    }
}
