using ACadSharp;
using ACadSharp.Entities;
using ACadSharp.IO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
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
    }

    public enum EditMode
    {
        Cursor,
        AddStud,
        RemoveStud
    }

    public class FileSystem
    {


        public CadDocument Doc;



        /**
         * <summary>create the file systeme with a new document</summary>
         */
        public FileSystem()
        {
            Doc = new CadDocument();
        }

        
        public List<Circle> ScanStud()
        {
            List<Circle> studs = new List<Circle>();
            if (Doc == null)
            {
                return studs;
            }
            foreach(var item in Doc.Entities)
            {
                if (item.ObjectType == ObjectType.CIRCLE)
                {
                    Circle candidate = (Circle)item;

                    if (candidate.Radius == Constants.StudRadius3 || candidate.Radius == Constants.StudRadius4)
                    {
                        Circle stud = candidate;
                        studs.Add(stud);
                    }
                }
            }
            return studs;

        }

        /**
         * <summary>Open a file at the location specified by path and load it if it exist. </summary>
         * <returns>true if the file was loaded succesfully, false if an error occured</returns>
         */
        public bool OpenDxfFile(string path)
        {
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
            return true;

        }



        

    }
}
