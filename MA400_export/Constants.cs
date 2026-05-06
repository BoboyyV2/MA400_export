using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public const float Max_Zoom = 7.0f;
        public const float Min_Zoom = 0.7f;

        public static PointF Origin_Coord { get; private set; } = new PointF(50.0f, 50.0f);


        public static PointF WorkZoneLimits_Coord { get; private set; } = new PointF(800.0f, 800.0f);
        public static string Outputpath { get; private set; } = AppDomain.CurrentDomain.BaseDirectory + @"output\";//.exe\output loc
        public static string DatenPath { get; private set; } = @"\Daten\2\";
        public static string CncPath { get; private set; } = @"\Cnc\2\";
        public static string tmpPath { get; private set; } = @"\tmp\";

        public static Scale DefaultDxfScale = new Scale(true, false);

        //GHP command id
        public const int CIRCLE_CMD = 1;
        public const int ARC_CMD = 3;
        public const int LINE_CMD = 4;


        public const int line_per_NC_cmd = 21;

    }
    
}
