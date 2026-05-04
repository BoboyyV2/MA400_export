using ACadSharp;
using ACadSharp.Entities;
using ACadSharp.IO;
using CSMath;
using Svg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace MA400_export
{

   

    public enum EditMode
    {
        Cursor,
        SelectStud,
        AddStud,
        RemoveStud
    }

    public enum Machine
    {
        MA400S,
        Other
    }

    /**
      * <summary>
      * The <c>FileSystem</c> Class is used to handle the files inputs and outputs 
      * </summary>
      */
     public class FileSystem
     {


         public CadDocument Doc = null;

        public SVGcontroller SvgControl; 
         public bool open {  get; private set; }
         public List<Circle> Studs {  get; private set; }

         private ProdFileGenerator Gen;

         //inner data
         //test data
         



         /**
          * <summary>create the file systeme with a new document</summary>
          */
    public FileSystem()
        {
            Doc = new CadDocument();
            open = false;
            Studs = new List<Circle>();
            SvgControl = new SVGcontroller();

        }

        /*_____________________________________PREVIEW_____________________________________*/

        /*void generatePreview()
        {
            if (selectDWGfile.FileName.Length > 0)
            {
                string path = selectDWGfile.FileName;
                try
                {
                    DwgPreview preview;
                    using (DwgReader reader = new DwgReader(path))
                    {
                        doc = reader.Read();
                        preview = reader.ReadPreview();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, toolName);
                }
            }
            else
            {
                MessageBox.Show("Select a file first.", toolName);
            }
        }*/

        /*_____________________________________UTIL_____________________________________*/


        /**
         * <summary>restore the FileSystem to a base state.</summary>
         */
        public void reset()
        {
            open = false;
            Doc = new CadDocument();
            Studs = new List<Circle>();
            SvgControl = new SVGcontroller();

        }


        /**
         * <summary>Apply the necessary transformation to a Circle to bring it to the origin and in the right orientation and scale.</summary>
         */
        public Circle ApplyTransform(Circle stud, PointF offset, RectangleF dimension, Scale scale)
        {
            Circle transformed = (Circle)stud.Clone();
            PointF pos = ProdFileGenerator.GetSpacialPosition(stud.Center, offset, dimension, scale);
            transformed.Center = new CSMath.XYZ(pos.X, pos.Y, 0);
            return transformed;
        }


        

        /**
        * <summary>Scan the document's entities and attempt to get Stud candidates as well as the dimentions of the document <br></br>
        * any stud candidate will be removed from the document, they would be added if we were to save the document to a file.</summary>
        * <returns> true if everything went well, false otherwise </returns>
        */
        private bool ScanEntities()
        {
            if (Doc == null)
            {
                return false;
            }
            foreach (var item in Doc.Entities)
            {
                switch (item.ObjectType)
                {
                    case ObjectType.CIRCLE:
                        {
                            Circle candidate = (Circle)item;

                            if (candidate.Radius == Constants.StudRadius3 || candidate.Radius == Constants.StudRadius4)
                            {
                                Circle stud = ApplyTransform(candidate, SvgControl.offset, SvgControl.dimension, SvgControl.scale);
                                Studs.Add(stud);
                            }
                            break;
                        }
                    default:
                        break;

                }
            }
            
            return true;

        }




        /*_____________________________________DXF_____________________________________*/



        /**
         * <summary>Open a file at the location specified by path and load it if it exist. </summary>
         * <returns>true if the file was loaded succesfully, false if an error occured</returns>
         */
        public bool OpenDxfFile(string path)
        {
            reset();

            if (!(File.Exists(path)))
            {
                return false;
            }

            using (DxfReader reader = new DxfReader(path))
            {
                //Inform about non critical error
                reader.OnNotification += NotificationHelper.LogConsoleNotification;
                Doc = reader.Read();
            }
            //first write to svg avant de scan pour chopper les paramètres
            string tmpPath = Properties.Settings.Default.OutputPath + Constants.tmpPath;
            WriteToSVG(tmpPath, @"\tmp.svg");//local tmp file
            SvgControl.OpenSVG(tmpPath, @"\tmp.svg");

            ScanEntities();


            open = true;
            //DEBUG
            //MessageBox.Show("insert point = " + Doc.ModelSpace.Layout.InsertionBasePoint.X + " ; " + Doc.ModelSpace.Layout.InsertionBasePoint.Y);
            //MessageBox.Show("origin = " + Doc.ModelSpace.Layout.Origin.X + " ; " + Doc.ModelSpace.Layout.Origin.Y);
            //MessageBox.Show("origin = " + Doc.ModelSpace.Layout. + " ; " + Doc.ModelSpace.Layout.Origin.Y);


            return true;

        }

        

        /**
        * <summary>Open a file using a stream and load it if it possible. </summary>
        * <returns>true if the file was loaded succesfully, false if an error occured</returns>
        */
        public bool OpenDxfFile(Stream stream)
        {
            reset();

            if (stream == null)
            {
                return false;
            }

            using (DxfReader reader = new DxfReader(stream))
            {
                //Inform about non critical error
                reader.OnNotification += NotificationHelper.LogConsoleNotification;
                Doc = reader.Read();
            }
            //first write to svg avant de scan pour chopper les paramètres
            string tmpPath = Properties.Settings.Default.OutputPath + Constants.tmpPath ;
            WriteToSVG(tmpPath, @"\tmp.svg");//local tmp file
            SvgControl.OpenSVG(tmpPath, @"\tmp.svg");

            ScanEntities();



            open = true;
            return true;

        }


        /*_____________________________________PRODFILE_____________________________________*/


        /**
        * <summary>attempt to open a program's file via it's program number and to import it into the application</summary>
        */
        public GeneratorData OpenProdFile(int ProgramNumber)
        {
            GeneratorData data = new GeneratorData();
            data.ProgramNumber = ProgramNumber;
            /*___________________________________*/

            //si on a les fichiers =>
            reset();


            ReadLAY(ProgramNumber);

            ReadGPH(ProgramNumber);
            ReadDAT(ProgramNumber, ref data);
            //get offset, scale & dimsension
            //scale always 1;1 ?
            //offset et dimension dans .LAY donc : 
            // ==>>
            ReadNC(ProgramNumber);

            string tmpPath = Properties.Settings.Default.OutputPath + Constants.tmpPath;

            WriteToSVG(tmpPath, @"\tmp.svg");//local tmp file


            //Doc.
            SvgControl.OpenSVG(tmpPath, @"\tmp.svg");



            open = true;
            return data;

        }


        /*_____________________________________GPH_____________________________________*/

        /**
         * <summary>parse the GPH file from num_line to num_line + nb_line_per_cmd in order to retrieve an entity and add it to the collection</summary>
         * <returns>the entitiy created or null if unable to create </returns>
         * <param name="file">the transcript of the file curently read</param>
         * <param name="num_line">the index of the current line in the file</param>
         */
        private Entity ParseGPHEntities(string[] file, ref int num_line)
        {

            //debug 
            /*
            MessageBox.Show("offset = " + SvgControl.offset.ToString() + Environment.NewLine +
                            "dim = " + SvgControl.dimension.ToString() + Environment.NewLine +
                            "scale = " + SvgControl.scale.Xscale + " ; " + SvgControl.scale.Yscale + Environment.NewLine);
            */

            int EntitieType = int.Parse(file[num_line]);
            switch (EntitieType)
            {
                //cercle
                case 1:
                    {
                        Circle c = new Circle();
                        double centerX = Double.Parse(file[++num_line], CultureInfo.InvariantCulture);
                        double centerY = Double.Parse(file[++num_line], CultureInfo.InvariantCulture);
                        c.Center = Util.AdjustPointGPH(new CSMath.XYZ(centerX, centerY, 0), SvgControl.offset, SvgControl.dimension, new Scale(true, false));

                        ++num_line;
                        double radius = Double.Parse(file[++num_line], CultureInfo.InvariantCulture);
                        c.Radius = radius;
                        num_line += 6;

                        //debug
                        /*
                        MessageBox.Show("center in : " + c.Center.ToString());
                        */

                        return c;
                    }

                //ligne
                case 4:
                    {
                        Line l = new Line();
                        double startX = Double.Parse(file[++num_line], CultureInfo.InvariantCulture);
                        double startY = Double.Parse(file[++num_line], CultureInfo.InvariantCulture);
                        l.StartPoint = Util.AdjustPointGPH(new CSMath.XYZ(startX, startY, 0), SvgControl.offset, SvgControl.dimension, new Scale(true, false) );
                        double endX = Double.Parse(file[++num_line], CultureInfo.InvariantCulture);
                        double endY = Double.Parse(file[++num_line], CultureInfo.InvariantCulture);
                        l.EndPoint = Util.AdjustPointGPH(new CSMath.XYZ(endX, endY, 0), SvgControl.offset, SvgControl.dimension, new Scale(true, false));
                        num_line += 6;

                        //debug
                        /*
                        MessageBox.Show("start in : " + l.StartPoint.ToString());
                        MessageBox.Show("end in : " + l.EndPoint.ToString());
                        */


                        return l;
                    }
                default:
                    {
                        num_line += 10;
                        return null;
                    }

            }

        }

        /**
         * <summary>read the GPH file of an already created program to retrieve all the entities informations</summary>
         */
        public void ReadGPH(int ProgramNumber)
        {
            string GPHPath = Properties.Settings.Default.OutputPath + Constants.DatenPath + ProgramNumber + ".GPH";

            string[] file = File.ReadAllLines(GPHPath);
            int cmd_line_number = 11;

            for (int num_line = 1; num_line <= (file.Length - cmd_line_number); num_line++)
            {
                Entity e = ParseGPHEntities(file, ref num_line);
                Doc.Entities.Add(e);
            }
        }


        /*_____________________________________DAT_____________________________________*/

        /**
         * <summary>read the DAT file of an already created program to retrieve all the header informations</summary>
         */
        public void ReadDAT(int ProgramNumber, ref GeneratorData data)
        {
            string DATPath = Properties.Settings.Default.OutputPath + Constants.DatenPath + ProgramNumber + ".DAT";

            string[] file = File.ReadAllLines(DATPath);

            //fill the data struct
            data.ProgramNumber = ProgramNumber;
            data.machine = Machine.MA400S;
            data.Company = file[0];
            data.PartDesignation = file[1];
            data.PartNumber = file[2];
            data.DrawingNumber = file[3];
            data.Notes = file[4];
            data.DateCreation = file[5];
            data.DateModification = file[6];

        }


        /*_____________________________________LAY_____________________________________*/

        /**
         * <summary>read the DAT file of an already created program to retrieve all the layout informations</summary>
         */
        public void ReadLAY(int ProgramNumber)
        {
            string LAYPath = Properties.Settings.Default.OutputPath + Constants.DatenPath + ProgramNumber + ".LAY";

            string[] file = File.ReadAllLines(LAYPath);


            SvgControl.offset = new PointF(float.Parse(file[0], CultureInfo.InvariantCulture),
                                float.Parse(file[1], CultureInfo.InvariantCulture));


            //TODO, verif si les deux dernier param sont les limites ou la hauteur / largeur
            //==> 0,0,210,110 =? 100,100,310,210 ou 100,100,210,110
            //probablement le second car je n'ai pas vu d'offset nul part
            //pour l'instant, marche impec si on est à l'origine
            SvgControl.dimension = new RectangleF( float.Parse(file[0], CultureInfo.InvariantCulture), 
                                        float.Parse(file[1], CultureInfo.InvariantCulture),
                                        float.Parse(file[2], CultureInfo.InvariantCulture),
                                        float.Parse(file[3], CultureInfo.InvariantCulture));

            SvgControl.scale = new Scale(true, false);//toujours 1;1 normalement et on réajuste en 1, -1
        }


        /*_____________________________________NC_____________________________________*/


        /**
         * <summary>parse the NC file from num_line to num_line + nb_line_per_cmd in order to retrieve an entity and add it to the collection</summary>
         * <returns>the entitiy created or null if unable to create </returns>
         * <param name="file">the transcript of the file curently read</param>
         * <param name="num_line">the index of the current line in the file</param>
         */
        private void ReadNcCommand(string[] file, ref int num_line)
        {
            if (file[num_line] == "PUNKT")
            {
                double X = float.Parse(file[++num_line], CultureInfo.InvariantCulture);
                double Y = float.Parse(file[++num_line], CultureInfo.InvariantCulture);
                num_line += (Constants.line_per_NC_cmd - 2);

                //create the stud
                Circle stud = new Circle();
                stud.Center = new CSMath.XYZ(X, Y, 0);
                stud.Radius = Constants.StudRadius3;
                //TODO , diamètre à récup
                stud.Color = ACadSharp.Color.Green;

                Studs.Add(stud);
            }
            else
            {
                num_line += Constants.line_per_NC_cmd;
            }
        }

        /**
         * <summary>read the NC file of an already created program to retrieve all the Studs informations</summary>
         */
        public void ReadNC(int ProgramNumber)
        {
            string NCPath = Properties.Settings.Default.OutputPath + Constants.DatenPath + ProgramNumber + ".NC";

            string[] file = File.ReadAllLines(NCPath);

            int numline = 0;
            int nb_studs = int.Parse(file[numline++]);

            for (int i = 0; i < nb_studs; i++)
            {
                ReadNcCommand(file, ref numline);
            }
        }

        /*_____________________________________SAVE_____________________________________*/


        public void SaveToFile(BindingList<Stud> Studs, string path)
        {
            //creation d'un nouveua document pour accomoder la sauvegarde
            CadDocument save = new CadDocument();

            //ajout de clones de toutes les entités presentes dans le doc dans la sauvegarde.
            foreach (Entity entity in Doc.Entities)
            {
                save.Entities.Add((Entity)entity.Clone());
            }

            //ajout de clones des goujons dans la sauvegarde.
            foreach (Stud stud in Studs)
            {
                Circle clone = (Circle)stud.circle.Clone();
                clone.Center = Util.AdjustPoint(clone.Center, SvgControl.offset, SvgControl.dimension, SvgControl.scale);
                save.Entities.Add(clone);
            }

            using (DxfWriter writer = new DxfWriter(path, save))
            {
                writer.OnNotification += NotificationHelper.LogConsoleNotification;
                writer.Write();
            }

            
            


        }


        private void WriteToSVG(string path, string filename)
        {

            Directory.CreateDirectory(path);

            string svgPath = path + filename;

            using (SvgWriter writer = new SvgWriter(svgPath, Doc))
            {
                writer.OnNotification += NotificationHelper.LogConsoleNotification;
                writer.Write();
            }
        }

        /*_____________________________________PRODUCTION_FILES_____________________________________*/


        /**
         * <summary>Generates the productoin files necessary to the driver to function</summary>
         */
        public void GenerateProdFiles(ref BindingList<Stud> Studs, RectangleF Dimension, PointF Offset, GeneratorData Data, Scale Scalefact) 
        {
            Gen = new ProdFileGenerator(ref Studs, Doc.Entities,Dimension, Offset, Data, Scalefact);
            Gen.GenerateProductionFiles(Data.ProgramNumber);
        }

        /*______________________________________________*/

    }
}
