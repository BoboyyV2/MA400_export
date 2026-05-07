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

         public bool open {  get; private set; }
         public List<Circle> Studs {  get; private set; }

         private ProdFileGenerator Gen;

         public Layout_Info layout { get; set;  }
         



         /**
          * <summary>create the file systeme with a new document</summary>
          */
    public FileSystem()
        {
            Doc = new CadDocument();
            open = false;
            Studs = new List<Circle>();
            layout = new Layout_Info();//must be initialized later on before scanning the entities

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
            layout = new Layout_Info();//must be initialized later on before scanning the entities

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
         * <summary>Apply the necessary transformation to a Circle to bring it to the origin and in the right orientation and scale.</summary>
         */
        public Circle ApplyTransform(Circle stud, Layout_Info layout)
        {
            Circle transformed = (Circle)stud.Clone();
            PointF pos = ProdFileGenerator.GetSpacialPosition(stud.Center, layout.offset, layout.dimension, layout.scale);
            transformed.Center = new CSMath.XYZ(pos.X, pos.Y, 0);
            return transformed;
        }

        /**
         * <summary>Apply the necessary transformation to a Circle to bring it to the origin and in the right orientation and scale.</summary>
         */
        public Circle ApplyTransform(Circle stud)
        {
            Circle transformed = (Circle)stud.Clone();
            PointF pos = ProdFileGenerator.GetSpacialPosition(stud.Center, layout.offset, layout.dimension, layout.scale);
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
            Studs.Clear();
            foreach (var item in Doc.Entities)
            {
                switch (item.ObjectType)
                {
                    case ObjectType.CIRCLE:
                        {
                            Circle candidate = (Circle)item;

                            if (candidate.Radius == Constants.StudRadius3 || candidate.Radius == Constants.StudRadius4)
                            {
                                Circle stud = ApplyTransform(candidate, layout.offset, layout.dimension, layout.scale);
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

        public double NormalizeRadians(double angle)
        {
            double TwoPI = Math.PI * 2;
            return ( (angle % TwoPI) + TwoPI) % TwoPI;
        }

        /*_____________________________________FLIP_____________________________________*/

        /**
         * <summary>flip a single entity in the document on the X axis</summary>
         */
        private void FlipEntityX(ref List<Entity> FlippedEntities, Entity e)
        {
            var type = e.ObjectType;
            switch (type)
            {
                 
                case ObjectType.LINE:
                    {
                        Line l = (Line)e.Clone();
                        l.StartPoint = new XYZ(l.StartPoint.X, -l.StartPoint.Y, l.StartPoint.Z);
                        l.EndPoint = new XYZ(l.EndPoint.X, -l.EndPoint.Y, l.EndPoint.Z);
                        FlippedEntities.Add(l);
                        break;
                    }
                case ObjectType.CIRCLE:
                    {
                        Circle c  = (Circle)e.Clone();
                        c.Center = new XYZ(c.Center.X, -c.Center.Y, c.Center.Z);
                        FlippedEntities.Add(c);
                        break;
                    }
                case ObjectType.ARC:
                    {
                        //more things to do here
                        Arc a = (Arc)e.Clone();
                        a.Center = new XYZ(a.Center.X, -a.Center.Y, a.Center.Z);

                        //les angles sont en radians !
                        double TwoPI = 2 * Math.PI;
                        //(may not be needed to normalize)
                        a.StartAngle = NormalizeRadians(TwoPI - ((Arc)e).EndAngle);
                        a.EndAngle   = NormalizeRadians(TwoPI - ((Arc)e).StartAngle);

                        FlippedEntities.Add(a);
                        break;
                    }
                    //TODO polyline
                default:
                    break;

            }
        }
        
        /**
         * <summary>flip all of the document's entities on the X axis</summary>
         */
        private void FlipEntitiesX()
        {
            List<Entity> FlippedEntities = new List<Entity>();
            foreach (Entity e in Doc.Entities)
            {
                FlipEntityX(ref FlippedEntities, e);
            }
            Doc.Entities.Clear();

            Doc.Entities.AddRange(FlippedEntities);
        }

        /**
         * <summary>flip a single entity in the document on the Y axis</summary>
         */
        private void FlipEntityY(ref List<Entity> FlippedEntities, Entity e)
        {
            var type = e.ObjectType;
            switch (type)
            {

                case ObjectType.LINE:
                    {
                        Line l = (Line)e.Clone();
                        l.StartPoint = new XYZ(-l.StartPoint.X, l.StartPoint.Y, l.StartPoint.Z);
                        l.EndPoint = new XYZ(-l.EndPoint.X, l.EndPoint.Y, l.EndPoint.Z);
                        FlippedEntities.Add(l);
                        break;
                    }
                case ObjectType.CIRCLE:
                    {
                        Circle c = (Circle)e.Clone();
                        c.Center = new XYZ(-c.Center.X, c.Center.Y, c.Center.Z);
                        FlippedEntities.Add(c);
                        break;
                    }
                case ObjectType.ARC:
                    {
                        //more things to do here
                        Arc a = (Arc)e.Clone();
                        a.Center = new XYZ(-a.Center.X, a.Center.Y, a.Center.Z);

                        //les angles sont en radians !
                        //(may not be needed to normalize)

                        //delta à 90
                        a.StartAngle = NormalizeRadians(Math.PI - ((Arc)e).EndAngle);
                        a.EndAngle   = NormalizeRadians(Math.PI - ((Arc)e).StartAngle);

                        FlippedEntities.Add(a);
                        break;
                    }
                //TODO polyline
                case ObjectType.LWPOLYLINE:
                default:
                    break;

            }
        }

        /**
         * <summary>flip all of the document's entities on the Y axis</summary>
         */
        private void FlipEntitiesY()
        {
            List<Entity> FlippedEntities = new List<Entity>();
            foreach (Entity e in Doc.Entities)
            {
                FlipEntityY(ref FlippedEntities, e);
            }
            Doc.Entities.Clear();

            Doc.Entities.AddRange(FlippedEntities);
        }

        /**
         * <summary>rotate the part at 180 degrees by fliping it back on the X axis then on the Y axis.</summary>
         * <remarks>this opération can be done a second time to cancel the effects.</remarks>
         */
        public void RotatePart180()
        {
            //reset without a new document
            //try debug
            open = false;
            Studs = new List<Circle>();
            layout = new Layout_Info();//must be initialized later on before scanning the entities

            FlipEntitiesX();
            FlipEntitiesY();
            string tmpPath = Properties.Settings.Default.OutputPath + Constants.tmpPath;

            Directory.CreateDirectory(tmpPath);
            //save dans un fichier temporaire pour l'affichage / traitement
            SaveToFile(tmpPath  + @"\dxftmp.ddxf");

            reset();
            OpenDxfFile(tmpPath + @"\dxftmp.ddxf");

        }


        /*_____________________________________DXF_____________________________________*/



        /**
         * <summary>Open a file at the location specified by path and load it if it exist.<br></br>
         * Should be followed by an initialization of the layout aswell as a scan</summary>
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

            //DEBUG
            FlipEntitiesX();



            string tmpPath = Properties.Settings.Default.OutputPath + Constants.tmpPath;

            Directory.CreateDirectory(tmpPath);
            //save dans un fichier temporaire pour l'affichage / traitement
            SaveToFile(tmpPath + @"\dxftmp.ddxf");

            open = true;

            return true;

        }

        public void OpenDxfFileLayout(Layout_Info layout)
        {
            this.layout = layout;
            ScanEntities();
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

            Directory.CreateDirectory(tmpPath);
            

            SaveToFile(tmpPath + @"\dxftmp.ddxf");

            open = true;

            return data;
        }

        public void OpenProdFileLayout(Layout_Info layout)
        {
            this.layout = layout;
            
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

            

            int EntitieType = int.Parse(file[num_line]);
            switch (EntitieType)
            {
                //cercle
                case 1:
                    {
                        Circle c = new Circle();
                        double centerX = Double.Parse(file[++num_line], CultureInfo.InvariantCulture);
                        double centerY = Double.Parse(file[++num_line], CultureInfo.InvariantCulture);
                        c.Center       = Util.AdjustPointGPH(new CSMath.XYZ(centerX, centerY, 0), layout.offset, layout.dimension, new Scale(true, false));

                        ++num_line;
                        double radius  = Double.Parse(file[++num_line], CultureInfo.InvariantCulture);
                        c.Radius       = radius;
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
                        l.StartPoint  = Util.AdjustPointGPH(new CSMath.XYZ(startX, startY, 0), layout.offset, layout.dimension, new Scale(true, false) );
                        double endX   = Double.Parse(file[++num_line], CultureInfo.InvariantCulture);
                        double endY   = Double.Parse(file[++num_line], CultureInfo.InvariantCulture);
                        l.EndPoint    = Util.AdjustPointGPH(new CSMath.XYZ(endX, endY, 0), layout.offset, layout.dimension, new Scale(true, false));
                        num_line += 6;

                        //debug
                        /*
                        MessageBox.Show("start in : " + l.StartPoint.ToString());
                        MessageBox.Show("end in : " + l.EndPoint.ToString());
                        */


                        return l;
                    }
                    //arc
                case 3:
                    {
                        Arc a = new Arc();
                        double centerX = Double.Parse(file[++num_line], CultureInfo.InvariantCulture);
                        double centerY = Double.Parse(file[++num_line], CultureInfo.InvariantCulture);
                        a.Center       = Util.AdjustPointGPH(new CSMath.XYZ(centerX, centerY, 0), layout.offset, layout.dimension, new Scale(true, false));

                        ++num_line;
                        double radius  = Double.Parse(file[++num_line], CultureInfo.InvariantCulture);
                        a.Radius       = radius;
                        a.StartAngle   = (Double.Parse(file[++num_line], CultureInfo.InvariantCulture)) * Math.PI / 180;
                        a.EndAngle     = (Double.Parse(file[++num_line], CultureInfo.InvariantCulture)) * Math.PI / 180; ;
                        num_line += 4;

                        //debug
                        /*
                        MessageBox.Show("center in : " + c.Center.ToString());
                        */

                        return a;
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


            //initialize the layout
            layout.offset = new PointF(float.Parse(file[0], CultureInfo.InvariantCulture),
                                float.Parse(file[1], CultureInfo.InvariantCulture));


            //TODO, verif si les deux dernier param sont les limites ou la hauteur / largeur
            //==> 0,0,210,110 =? 100,100,310,210 ou 100,100,210,110
            //probablement le second car je n'ai pas vu d'offset nul part
            //pour l'instant, marche impec si on est à l'origine
            layout.dimension = new RectangleF( float.Parse(file[0], CultureInfo.InvariantCulture), 
                                        float.Parse(file[1], CultureInfo.InvariantCulture),
                                        float.Parse(file[2], CultureInfo.InvariantCulture),
                                        float.Parse(file[3], CultureInfo.InvariantCulture));

            layout.scale = new Scale(true, false);//toujours 1;1 normalement et on réajuste en 1, -1
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


        /**
         * <summary>Save the Document to a dxf file with the StudList and potential modifications</summary>
         */
        public void SaveToFile(BindingList<Stud> Studs, string path)
        {
            //creation d'un nouveua document pour accomoder la sauvegarde
            CadDocument save = new CadDocument();

            //ajout de clones de toutes les entités presentes dans le doc dans la sauvegarde.
            foreach (Entity entity in Doc.Entities)
            {
                try
                {
                    save.Entities.Add((Entity)entity.Clone());
                }
                catch (ArgumentException e)
                {
                    //MessageBox.Show(entity.ToString());
                }
            }

            //ajout de clones des goujons dans la sauvegarde.
            foreach (Stud stud in Studs)
            {
                Circle clone = (Circle)stud.circle.Clone();
                clone.Center = Util.AdjustPoint(clone.Center, layout.offset, layout.dimension, layout.scale);
                save.Entities.Add(clone);
            }

            using (DxfWriter writer = new DxfWriter(path, save))
            {
                writer.OnNotification += NotificationHelper.LogConsoleNotification;
                writer.Write();
            }

        }

        /**
         * <summary>Save the Document to a dxf file without the StudList and potential modifications<br></br>
         * This is used to create a dxf file from a file number and display it using DXFImporter as it need a dxf FILE not just the document instance</summary>
         */
        public void SaveToFile(string path)
        {
            //creation d'un nouveau document pour accomoder la sauvegarde
            CadDocument save = new CadDocument();

            //ajout de clones de toutes les entités presentes dans le doc dans la sauvegarde.
            foreach (Entity entity in Doc.Entities)
            {
                try
                {
                    save.Entities.Add((Entity)entity.Clone());
                }
                catch(ArgumentException e)
                {
                    //MessageBox.Show(entity.ToString());
                }
            }

            using (DxfWriter writer = new DxfWriter(path, save))
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

        /**
         * <summary>Generates the productoin files necessary to the driver to function</summary>
         */
        public void GenerateProdFiles(ref BindingList<Stud> Studs,GeneratorData Data, Layout_Info layout)
        {
            Gen = new ProdFileGenerator(ref Studs, Doc.Entities, layout.dimension, layout.offset, Data, layout.scale);
            Gen.GenerateProductionFiles(Data.ProgramNumber);
        }

        /*______________________________________________*/

    }
}
