using System;
using System.Drawing;

namespace MA400_export
{

    /**
     * <summary>
     * The <c>Constant</c> Class is used to store constants throughout the project
     * </summary>
     */
    public static class Constants
    {
        public const bool Debug = false;

        //rayon des goujons en mm
        public const double StudRadius3 = 1.5;
        public const double StudRadius4 = 2.0;

        //facteur de zoom min et max
        public const float Max_Zoom = 7.0f;
        public const float Min_Zoom = 0.7f;

        //coordonée de l'origine dans le graphics d'affichage
        public static PointF Origin_Coord { get; private set; } = new PointF(50.0f, 50.0f);

        //limite de la zone de travail dans le graphics d'affichage (750mm de coté + offset origin)
        public static PointF WorkZoneLimits_Coord { get; private set; } = new PointF(800.0f, 800.0f);

        //path auto-calculé vers le dossier d'output
        public static string Outputpath { get; private set; } = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\MA400_export\";//location dans document 

        //path auto-calculé vers le dossier daten
        public static string DatenPath { get; private set; } = @"\Daten\2\";

        //path auto-calculé vers le dossier cnc
        public static string CncPath { get; private set; } = @"\Cnc\2\";

        //path auto-calculé vers le dossier tmp
        public static string tmpPath { get; private set; } = @"\tmp\";

        //scale par defaut d'un fichier dxf (1;-1)
        public static Scale DefaultDxfScale = new Scale(true, false);

        //GHP command id
        public const int CIRCLE_CMD = 1;
        public const int ARC_CMD = 3;
        public const int LINE_CMD = 4;


        public const int line_per_NC_cmd = 21;

        public const int Client_panel_delta_width = 280;
        public const int Client_panel_delta_height = 80;


    }

}
