using ACadSharp;
using ACadSharp.Entities;
using ACadSharp.IO;
using Svg;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace MA400_export
{

    /**
    * <summary>
    * The <c>Constant</c> Class is used to store constants throughout the project
    * </summary>
     */
    public static class Constants
    {
        public const bool Debug = true;//TODO changer avant de partir en deploy

        public const double StudRadius3 = 1.5;
        public const double StudRadius4 = 2.0;

        public const float Max_Zoom = 5.0f;
        public const float Min_Zoom = 0.7f;

        public static PointF Origin_Coord { get; private set; } = new PointF(50.0f, 50.0f);

        public static PointF WorkZoneLimits_Coord { get; private set; } = new PointF(800.0f, 800.0f);
        public static String outputpath { get; private set; } = ""+AppDomain.CurrentDomain.BaseDirectory;//.exe loc

    }

    public enum EditMode
    {
        Cursor,
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


        /**
         * <summary>create the file systeme with a new document</summary>
         */
        public FileSystem()
        {
            Doc = new CadDocument();
            open = false;
            Studs = new List<Circle>();
        }

        private void reset()
        {
            open = false;
            Doc = new CadDocument();
            Studs = new List<Circle>();
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
            if (!(File.Exists(path)))
            {
                reset();
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
            if (stream == null)
            {
                reset();
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


        /*_____________________________________SAVE_____________________________________*/

        public void SaveToFile(string path)
        {
            using (DxfWriter writer = new DxfWriter(path, Doc))
            {
                writer.OnNotification += NotificationHelper.LogConsoleNotification;
                writer.Write();
                //TODO retour des goujons
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
            //FixSVGTransform(svgPath);
        }

        

        /*______________________________________________*/

    }
}
