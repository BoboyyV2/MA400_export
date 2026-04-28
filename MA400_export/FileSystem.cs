using ACadSharp;
using ACadSharp.Entities;
using ACadSharp.IO;
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

   
    public class FileSystem
    {


        public CadDocument Doc;
        public bool open {  get; private set; }
        public List<Circle> Studs {  get; private set; }

        private ProdFileGenerator Gen;

        //inner data
        //test data
        public PointF offset { get; set; } = new PointF(708.35f, 71.70f);
        public RectangleF dimension { get; set; } = new RectangleF(0, 0, 210, 110);
        public Scale scale { get; set; } = new Scale(1, -1);



        /**
         * <summary>create the file systeme with a new document</summary>
         */
        public FileSystem()
        {
            Doc = new CadDocument();
            open = false;
            Studs = new List<Circle>();
            
        }

        /*_____________________________________UTIL_____________________________________*/
        
        
        /**
         * <summary>restore the FileSystem to a base state.</summary>
         */
        public void reset()
        {
            open = false;
            Doc = new CadDocument();
            Studs = new List<Circle>();

            //test data
            //TODO remove 
            PointF offset = new PointF(708.35f, 71.70f);
            RectangleF dimension = new RectangleF(0, 0, 210, 110);
            Scale scale = new Scale(1, -1);
        }

       

        

       
        /*_____________________________________DXF_____________________________________*/


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
            foreach(var item in Doc.Entities)
            {
                switch (item.ObjectType)
                {
                    case ObjectType.CIRCLE :
                    {
                        Circle candidate = (Circle)item;

                        if (candidate.Radius == Constants.StudRadius3 || candidate.Radius == Constants.StudRadius4)
                        {
                            Circle stud = candidate;
                            Studs.Add(stud);
                        }
                        break;
                    }
                    default:
                        break;
                
                }
            }
            foreach(Circle candidate in Studs) {
                Doc.Entities.Remove(candidate);
            }
            return true;

        }

       
       

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
            ScanEntities();


            WriteToSVG(Constants.outputpath + @"tmp\");//local tmp file

            open = true;
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

            ScanEntities();


            WriteToSVG(Constants.outputpath + @"tmp\");//local tmp file

            open = true;
            return true;

        }


        /*_____________________________________PRODFILE_____________________________________*/

        /**
         * <summary>parse the GPH file from num_line to num_line + nb_line_per_cmd in order to retrieve an entity and add it to the collection</summary>
         * <returns>the entitiy created or null if unable to create </returns>
         */
        private Entity ParseEntities(string[] file, ref int num_line)
        {
            int EntitieType = int.Parse(file[num_line]);
            switch (EntitieType)
            {
                //cercle
                case 1:
                    {
                        Circle c = new Circle();
                        double centerX = Double.Parse(file[++num_line], CultureInfo.InvariantCulture);
                        double centerY = Double.Parse(file[++num_line], CultureInfo.InvariantCulture);
                        c.Center = new CSMath.XYZ(centerX, centerY, 0);

                        ++num_line;
                        double radius = Double.Parse(file[++num_line], CultureInfo.InvariantCulture);
                        c.Radius = radius;
                        num_line += 6;

                        return c;
                    }

                //ligne
                case 4:
                    {
                        Line l = new Line();
                        double startX = Double.Parse(file[++num_line], CultureInfo.InvariantCulture);
                        double startY = Double.Parse(file[++num_line], CultureInfo.InvariantCulture);
                        l.StartPoint = new CSMath.XYZ(startX, startY, 0);
                        double endX = Double.Parse(file[++num_line], CultureInfo.InvariantCulture);
                        double endY = Double.Parse(file[++num_line], CultureInfo.InvariantCulture);
                        l.EndPoint = new CSMath.XYZ(endX, endY, 0);
                        num_line += 6;

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
            string GPHPath = Constants.outputpath + @"Daten\" + ProgramNumber + ".GPH";

            string[] file = File.ReadAllLines(GPHPath);
            int cmd_line_number = 11;

            for (int num_line = 1; num_line <= (file.Length - cmd_line_number); num_line++)
            {
                Entity e = ParseEntities(file, ref num_line);
                Doc.Entities.Add(e);
            }
        }


        /**
         * <summary>read the DAT file of an already created program to retrieve all the header informations</summary>
         */
        public void ReadDAT(int ProgramNumber, ref GeneratorData data)
        {
            string DATPath = Constants.outputpath + @"Daten\" + ProgramNumber + ".DAT";

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


        /**
         * <summary>read the DAT file of an already created program to retrieve all the layout informations</summary>
         */
        public void ReadLAY(int ProgramNumber)
        {
            string LAYPath = Constants.outputpath + @"Daten\" + ProgramNumber + ".LAY";

            string[] file = File.ReadAllLines(LAYPath);


            offset = new PointF(float.Parse(file[0], CultureInfo.InvariantCulture),
                                float.Parse(file[1], CultureInfo.InvariantCulture));


            //TODO, verif si les deux dernier param sont les limites ou la hauteur / largeur
            //==> 0,0,210,110 =? 100,100,310,210 ou 100,100,210,110
            //probablement le second car je n'ai pas vu d'offset nul part
            //pour l'instant, marche impec si on est à l'origine
            dimension = new RectangleF( float.Parse(file[0], CultureInfo.InvariantCulture), 
                                        float.Parse(file[1], CultureInfo.InvariantCulture),
                                        float.Parse(file[3], CultureInfo.InvariantCulture),
                                        float.Parse(file[4], CultureInfo.InvariantCulture));

            scale = new Scale(1, 1);//toujours ça normalement
        }


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

            ReadGPH(ProgramNumber);
            ReadDAT(ProgramNumber, ref data);
            //get offset, scale & dimsension
            //scale always 1;1 ?
            //offset et dimension dans .LAY donc : 
            // ==>>
            ReadLAY(ProgramNumber);

            ScanEntities();

            WriteToSVG(Constants.outputpath + @"tmp\");//local tmp file

            open = true;
            return data;
        }


        /*_____________________________________SAVE_____________________________________*/


        public void SaveToFile(BindingList<Stud> Studs, string path)
        {

            //retour des goujons sur le document à sauvegarder
            foreach (Stud stud in Studs)
            {
                stud.circle.Color = ACadSharp.Color.Green;//bien visible, bien en vert
                Doc.Entities.Add(stud.circle);
            }
            using (DxfWriter writer = new DxfWriter(path, Doc))
            {
                writer.OnNotification += NotificationHelper.LogConsoleNotification;
                writer.Write();
                
            }

            //pour des question de logique et de propriété des Circle il FAUT les retirer de la collection
            foreach (Stud stud in Studs)
            {
                Doc.Entities.Remove(stud.circle);
            }
        }


        private void WriteToSVG(string path)
        {

            Directory.CreateDirectory(path);

            string svgPath = path + @"\display.svg";

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
