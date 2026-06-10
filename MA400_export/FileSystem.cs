using ACadSharp;
using ACadSharp.Entities;
using ACadSharp.IO;
using CSMath;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using DXFImporter;

/***
 * This app use two personnalized file extension : ddxf and sdxf
 * 
 * Both of them are dxf file
 * We use them to differenciate the input file from our save files, they might still be used in dxf viewer / editor (you might need to change the extension manually for the logi to recognize the file)
 * 
 * The ddxf stores all dxf Entities (the part form)
 * The sdxf stores all the  actual studs (that have been detected / added and not removed)
 * 
 * It is done this way for 2 major reason :
 *  1 - To separate the graphics from the studs.
 *  2 - The produced files may or may not open in certain dxf viewer / editor for unknown reason so it is better not to label them as dxf files, they might still be repaired by some CADMAN commands.
 */

namespace MA400_export
{

    public enum Machine
    {
        KTS850,
        PTS300,
        None
    }

    /**
      * <summary>
      * The <c>FileSystem</c> Class is used to handle the files inputs and outputs 
      * </summary>
      */
    public class FileSystem
    {

        //the cad document
        public CadDocument Doc = null;

        //whether a document or program is open
        public bool open { get; private set; }

        //The list of studs in the document, adjusted to the origion coordinates
        public BindingList<Stud> Studs { get; private set; }

        //a copy of the above list used as remnant
        private BindingList<Stud> StudsTMP = null;


        //generatueur de fichier de sortie dédié
        private ProdFileGenerator Gen;

        //information de layout de la pièce courante
        //attention l'accès doit se faire uniquement après initialisation (ouverture du fichier) 
        //USE WITH CAUTION
        public Layout_Info layout { get; set; }

        //rotation variable
        public double rotation{ get; private set; } = 0;
        private XYZ tl;
        private XYZ tr;
        private XYZ bl;
        private XYZ br;



        /**
         * <summary>create the file systeme with a new document</summary>
         */
        public FileSystem()
        {
            Doc = new CadDocument();
            open = false;
            Studs = new BindingList<Stud>();
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
            Studs.Clear();
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
        * <summary>Scan the document's entities and attempt to get Stud candidates.<br></br>
        * Stud candidate will not be removed from the document.</summary>
        * <returns> true if everything went well, false otherwise </returns>
        * <remarks>this function is used for dxf file</remarks>
        */
        private bool ScanDxfEntities()
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
                                Studs.Add(new Stud(stud));
                            }
                            break;
                        }
                    default:
                        break;

                }

            }

            return true;

        }

        /**
        * <summary>Scan the document's entities and attempt to get Stud candidates.<br></br>
        * Stud candidate will not be removed from the document.<br></br>
        * When using ddxf file, the studs to scan are stored in a .sdxf file with the same name.</summary>
        * <returns> true if everything went well, false otherwise </returns>
        * <remarks>this function is used for ddxf & sdxf file</remarks>
        */
        private bool ScanDdxfEntities(string path)
        {
            if (Doc == null)
            {
                return false;
            }
            //path du fichier sans l'extension
            string savepath = Path.GetDirectoryName(path) + @"\" + Path.GetFileNameWithoutExtension(path);

            if (File.Exists(savepath + ".sdxf"))
            {
                CadDocument sdxf = new CadDocument();

                using (DxfReader reader = new DxfReader(savepath + ".sdxf"))
                {
                    //Inform about non critical error
                    reader.OnNotification += NotificationHelper.LogConsoleNotification;
                    sdxf = reader.Read();

                    //cherche dans les entités du sdxf(ne contenant que des goujons normalement)
                    foreach (var item in sdxf.Entities)
                    {
                        //verif quand même
                        switch (item.ObjectType)
                        {
                            case ObjectType.CIRCLE:
                                {
                                    Circle candidate = (Circle)item;

                                    //ajoute les goujon au document courant
                                    if (candidate.Radius == Constants.StudRadius3 || candidate.Radius == Constants.StudRadius4)
                                    {
                                        Circle stud = ApplyTransform(candidate, layout.offset, layout.dimension, layout.scale);
                                        Studs.Add(new Stud(stud));
                                    }
                                    break;
                                }
                            default:
                                break;

                        }

                    }


                }

            }

            return true;

        }


        /**
         * <summary>Normalize an angle in radian for it to be between 0 ans 2PI</summary>
         * <param name="angle">angle given in radians</param>
         */
        public double NormalizeRadians(double angle)
        {
            double TwoPI = Math.PI * 2;
            return ((angle % TwoPI) + TwoPI) % TwoPI;
        }

        /**
         * <summary>Normalize an angle in degrees</summary>
         * <param name="angle">angle given in degrees</param>
         */
        private double NormalizeAngle(double angle)
        {
            return ((angle % 360) + 360) % 360;
        }

        

        /*_____________________________________FRAME_____________________________________*/

        /**
         * <summary>Tells if a point is located inside a circle</summary>
         */
        public bool IsInsideCircle(PointF point, Circle circle)
        {

            //si la distance entre le point et le centre du cercle est inférieure au rayon
            if (((circle.Center.X - point.X) * (circle.Center.X - point.X)
                + (circle.Center.Y - point.Y) * (circle.Center.Y - point.Y)) //disante au carré
                < (circle.Radius * circle.Radius)) // rayon au carré
            {
                return true;
            }
            return false;
        }

        /**
         * <summary>check if the cursor is inside a circle and set c to the circle it is in</summary>
         * <remarks>will set c to  -1 ; -1 if no circle matches.</remarks>
         */
        public void TryFrame(PointF CursorPosition, out Circle c)
        {
            c = new Circle();
            c.Center = new XYZ(-1, -1, 0);

            if (!open)
            {
                //security
                return;
            }

            foreach (var entity in Doc.Entities)
            {
                if (entity.ObjectType == ObjectType.CIRCLE)
                {
                    Circle candidate = ApplyTransform((Circle)entity);
                    if (IsInsideCircle(CursorPosition, candidate))
                    {
                        c.Center = new XYZ(candidate.Center.X, candidate.Center.Y, 0);
                        c.Radius = candidate.Radius;
                        return;
                    }

                }

            }

        }
        /*_____________________________________ROTATE_____________________________________*/
        //Due to the difference both in system and in utility the studs and the entity don't use the same coordinate system
        //this means we cannot rotate them together out of the box
        //the choice was made to rotate them both separatly using different method

        /**
         * <summary>rotate a point around the origin by angle degrees clockwise</summary>
         */
        public XYZ RotatePointAroundOrigin(XYZ point, double angle)
        {
            double x = Math.Round(
                            point.X * Math.Cos(Trigo.DegreesToRadians(angle)) - point.Y * Math.Sin(Trigo.DegreesToRadians(angle)),
                            5);
            double y = Math.Round(
                            point.Y * Math.Cos(Trigo.DegreesToRadians(angle)) + point.X * Math.Sin(Trigo.DegreesToRadians(angle)),
                            5);

            return new XYZ(x, y, 0);
        }

        /**
         * <summary>rotate a point around the center by angle degrees clockwise</summary>
         */
        public XYZ RotatePointAroundCenter(XYZ target, PointF center, double angle)
        {
            XYZ pointToOrigin = new XYZ(target.X - center.X, target.Y - center.Y, 0);
            
            pointToOrigin = RotatePointAroundOrigin(pointToOrigin, angle);
            //recalculate center

            return new XYZ(pointToOrigin.X + center.X, pointToOrigin.Y + center.Y, 0);
        }

        /**
         * <summary>rotate a point around the center by angle degrees clockwise and adjust it's position</summary>
         */
        public XYZ RotatePointAroundCenterAndAdjust(XYZ target, PointF center, double angle, PointF newCenter)
        {
            XYZ pointToOrigin = new XYZ(target.X - center.X, target.Y - center.Y, 0);

            pointToOrigin = RotatePointAroundOrigin(pointToOrigin, angle);
            //recalculate center

            return new XYZ(pointToOrigin.X + newCenter.X, pointToOrigin.Y + newCenter.Y, 0);
        }

        /**
         * <summary>Compute the new center of the part whenever it is rotated</summary>
         * <remarks>it is used to accurately place the studs on the display and not fuck over the coordinates</remarks>
         */
        private PointF computeCenterOnRotation(PointF center, double angle)
        {
            //the center is the center of the layout, we need to compute the new center after rotation to be able to rotate the studs around it
            double Xmin;
            double Xmax;
            double Ymin;
            double Ymax;


            //rotate the dimension around the center to get the new limits of the new layout
            tl = RotatePointAroundCenter(tl, center, angle);
            tr = RotatePointAroundCenter(tr, center, angle);
            bl = RotatePointAroundCenter(bl, center, angle);
            br = RotatePointAroundCenter(br, center, angle);

            //compute the new center
            Xmin = Math.Min(Math.Min(tl.X, tr.X), Math.Min(bl.X, br.X));
            Xmax = Math.Max(Math.Max(tl.X, tr.X), Math.Max(bl.X, br.X));
            Ymin = Math.Min(Math.Min(tl.Y, tr.Y), Math.Min(bl.Y, br.Y));
            Ymax = Math.Max(Math.Max(tl.Y, tr.Y), Math.Max(bl.Y, br.Y));

            double newCenterX = (Xmax - Xmin) / 2;
            double newCenterY = (Ymax - Ymin) / 2;
            return new PointF((float)newCenterX, (float)newCenterY);
        }

        /**
         * <summary>Rotate a stud by angle degrees clockwise, accounting for offset</summary>
         * <remarks>Since this function add the offset, it needs to be removed once updated.</remarks>
         */
        private void RotateStud(Stud stud, double angle, PointF center, PointF newCenter)
        {
            //the offseted position of the stud
            stud.circle.Center = RotatePointAroundCenterAndAdjust(stud.circle.Center, center, angle, newCenter);
        }

        /**
         * <summary>Bring a point closer to the origin, in way that his delta to the topleftmost point of the part is equal to the delta to the origin.</summary>
         */
        private XYZ BringToOrigin(XYZ point)
        {
            return new XYZ(point.X - layout.offset.X, point.Y - layout.offset.Y, point.Y);
        }

        /**
         * <summary>Rotate a single entity from the document</summary>
         * <param name="e">the entity to rotate</param>
         */
        private void RotateEntity(Entity e, double angle)
        {
            switch (e.ObjectType) {
                case ObjectType.LINE:
                    {
                        ACadSharp.Entities.Line l = (ACadSharp.Entities.Line)e;
                        l.StartPoint = RotatePointAroundOrigin( BringToOrigin(l.StartPoint), -angle);
                        l.EndPoint = RotatePointAroundOrigin( BringToOrigin(l.EndPoint), - angle);
                        break;
                    }
                case ObjectType.CIRCLE:
                    {
                        ACadSharp.Entities.Circle c = (ACadSharp.Entities.Circle)e;
                        c.Center = RotatePointAroundOrigin( BringToOrigin(c.Center), - angle);
                        break;
                    }
                case ObjectType.ARC:
                    {
                        ACadSharp.Entities.Arc a = (ACadSharp.Entities.Arc)e;
                        a.Center = RotatePointAroundOrigin( BringToOrigin(a.Center), - angle);
                        //angle rotation
                        //angle are in radians for calculation
                        a.StartAngle = NormalizeRadians( a.StartAngle - Trigo.DegreesToRadians(angle) ); //as we are in reverse we substract it 
                        a.EndAngle = NormalizeRadians(a.EndAngle - Trigo.DegreesToRadians(angle) );
                        break;
                    }
                default:
                    MessageBox.Show("Entité non prise en charge trouvée lors de la rotation : " + e.ToString());
                    return ;

            }
        }

        /**
         * <summary>Rotate the current part by the specified number of degrees clockwise</summary>
         * <param name="angle">the angle in degrees</param>
         */
        public void Rotate(double angle)
        {
            //clockwise because we are using a negative y scale

            //rotate the entities
            foreach(var entity in Doc.Entities)
            {
                RotateEntity(entity, angle);
            }

            //rotate the studs
            StudsTMP = new BindingList<Stud>();
            PointF center = new PointF(layout.dimension.Width / 2, layout.dimension.Height / 2);
            PointF newCenter = computeCenterOnRotation(center, angle);

            foreach(Stud stud in Studs)
            {
                RotateStud(stud, angle, center, newCenter);
            }
            //prepare for a refresh
        }

        /**
         * <summary>rotate the part by angle degrees clockwise.<br></br>
         * This function must be used along with the OpenFlipFileLayout() function </summary>
         */
        public void RotatePart(double angle)
        {
            //reset without a new document our new studlist
            open = false;
            string tmpPath = Constants.MainPath + Constants.tmpPath;

            //save dans un fichier temporaire pour l'affichage / traitement
            try
            {
                Directory.CreateDirectory(tmpPath);
                Util.SetPermissions(tmpPath);
            }
            catch (DirectoryNotFoundException e)
            {
                MessageBox.Show(e.Message);
            }
            catch (UnauthorizedAccessException e)
            {
                MessageBox.Show(e.Message);
            }
            catch
            {
                MessageBox.Show("Erreur lors de la rotation");
            }

            //rotate the whole part
            Rotate(angle);

            //save les studs dans une liste temporaire avant de reset pour l'affichage
            BindingList<Stud> oldStuds = new BindingList<Stud>();
            foreach (Stud s in Studs)
            {
                oldStuds.Add(s);
            }

            SaveToFile(tmpPath + @"\dxftmp.ddxf");

            //ré open le fichier tmp en ddxf
            OpenDDxfFile(tmpPath + @"\dxftmp.ddxf");

            //save the rotated studs => used in OpenFlipFileLayout()
            StudsTMP = oldStuds;

        }

        /**
         * <summary>rotate the part at a 180 degrees angle.<br></br>
         * This function must be used along with the OpenFlipFileLayout() function </summary>
         * <remarks>this operation can be done a second time to cancel the effects.</remarks>
         */
        public void RotatePart180()
        {
            RotatePart(180);
        }

        /**
         * <summary>rotate the part at a 180 degrees angle.<br></br>
         * This function must be used along with the OpenFlipFileLayout() function </summary>
         * <remarks>this operation can be done a second time to cancel the effects.</remarks>
         */
        public void RotatePart90()
        {
            RotatePart(90);
        }

        /**
         * <summary>Get the layout of a part when it is being rotated</summary>
         */
        public void OpenRotatedFileLayout(Layout_Info layout, double angle)
        {
            //set the new layout
            this.layout = layout;


            //no addrange defined 
            //fill the local stud list with the temporary one then clear and void the temporary list
            foreach (Stud stud in StudsTMP)
            {
                Studs.Add(stud);
            }
            StudsTMP.Clear();
            StudsTMP = null;

            //inverse la valeur 
            rotation = NormalizeAngle(rotation + angle);
        }

        /*_____________________________________FLIP_____________________________________*/

        /**
         * <summary>flip a single entity in the document on the X axis</summary>
         * <remarks>handle arcs angles aswell</remarks>
         */
        private void FlipEntityX(ref List<Entity> FlippedEntities, Entity e)
        {
            var type = e.ObjectType;
            switch (type)
            {

                case ObjectType.LINE:
                    {
                        ACadSharp.Entities.Line l = (ACadSharp.Entities.Line)e.Clone();
                        l.StartPoint = new XYZ(l.StartPoint.X, -l.StartPoint.Y, l.StartPoint.Z);
                        l.EndPoint = new XYZ(l.EndPoint.X, -l.EndPoint.Y, l.EndPoint.Z);
                        FlippedEntities.Add(l);
                        break;
                    }
                case ObjectType.CIRCLE:
                    {
                        Circle c = (Circle)e.Clone();
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
                        a.EndAngle = NormalizeRadians(TwoPI - ((Arc)e).StartAngle);

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
         * <remarks>handle arcs angles aswell</remarks>
         */
        private void FlipEntityY(ref List<Entity> FlippedEntities, Entity e)
        {
            var type = e.ObjectType;
            switch (type)
            {

                case ObjectType.LINE:
                    {
                        ACadSharp.Entities.Line l = (ACadSharp.Entities.Line)e.Clone();
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
                        a.EndAngle = NormalizeRadians(Math.PI - ((Arc)e).StartAngle);

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

        


        


        /*_____________________________________DDXF_____________________________________*/

        /**
         * <summary>Open a file at the location specified by path and load it if it exist.<br></br>
         * Should be followed by an initialization of the layout aswell as a scan</summary>
         * <returns>true if the file was loaded succesfully, false if an error occured</returns>
         */
        public bool OpenDDxfFile(string path)
        {
            reset();
            string tmpPath = Constants.MainPath + Constants.tmpPath;

            try
            {
                Directory.CreateDirectory(tmpPath);
                Util.SetPermissions(tmpPath);
            }
            catch (DirectoryNotFoundException e)
            {
                MessageBox.Show("Erreur lors de l'ouverture d'un fichier " + e.Message);
                return false;
            }
            catch (UnauthorizedAccessException e)
            {
                MessageBox.Show("Erreur lors de l'ouverture d'un fichier " + e.Message);
                return false;
            }
            catch
            {
                MessageBox.Show("Erreur lors de l'ouverture d'un fichier");
                return false;
            }

            if (!(File.Exists(path)))
            {
                return false;
            }

            //read the ddxf file for the graphics
            using (DxfReader reader = new DxfReader(path))
            {
                //Inform about non critical error
                reader.OnNotification += NotificationHelper.LogConsoleNotification;
                Doc = reader.Read();
            }




            //save dans un fichier temporaire pour l'affichage / traitement
            SaveToFile(tmpPath + @"\dxftmp.ddxf");

            open = true;

            return true;

        }

        /**
         * <summary>Get the layout of a dxf file when it is opening as well as performing a scan to get the studs</summary>
         * <param name="path">the path to the file we are openning.</param>
         */
        public void OpenDdxfFileLayout(Layout_Info layout, string path)
        {
            this.layout = layout;
            //read the sdxf for the studs
            ScanDdxfEntities(path);
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
            string tmpPath = Constants.MainPath + Constants.tmpPath;

            try
            {
                Directory.CreateDirectory(tmpPath);
                Util.SetPermissions(tmpPath);
            }
            catch (DirectoryNotFoundException e)
            {
                MessageBox.Show("Erreur lors de l'ouverture d'un fichier " + e.Message);
                return false;
            }
            catch (UnauthorizedAccessException e)
            {
                MessageBox.Show("Erreur lors de l'ouverture d'un fichier " + e.Message);
                return false;
            }
            catch
            {
                MessageBox.Show("Erreur lors de l'ouverture d'un fichier");
                return false;
            }

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

            //flip the part so that the inner part faces upward
            FlipEntitiesX();



            //save dans un fichier temporaire pour l'affichage / traitement
            SaveToFile(tmpPath + @"\dxftmp.ddxf");

            open = true;

            return true;

        }


        /**
         * <summary>Get the layout of a dxf file when it is opening as well as performing a scan to get the studs</summary>
         */
        public void OpenDxfFileLayout(Layout_Info layout)
        {
            //init the layout
            this.layout = layout;
            tl = new XYZ(0, 0, 0);
            tr = new XYZ(layout.dimension.Width, 0, 0);
            bl = new XYZ(0, layout.dimension.Height, 0);
            br = new XYZ(layout.dimension.Width, layout.dimension.Height, 0);

            ScanDxfEntities();
        }



        /*_____________________________________PRODFILE_____________________________________*/

        internal void setGenerator(Machine machine )
        {
            switch (machine)
            {
                case Machine.KTS850:
                    Gen = new CNCProdFileGenerator(Studs, Doc.Entities, layout.dimension, layout.offset, layout.scale, rotation);
                    break;
                case Machine.PTS300:
                    Gen = new AREProdFileGenerator(Studs, Doc.Entities, layout.dimension, layout.offset, layout.scale, rotation);
                    break;
                default:
                    Gen = new CNCProdFileGenerator(Studs, Doc.Entities, layout.dimension, layout.offset, layout.scale, rotation);
                    break;
            }
        }

        /*_____________________________________CNC_PROGRAM_____________________________________*/


        /**
        * <summary>attempt to open a CNC program's files via it's program number and import it into the application</summary>
        */
        public GeneratorData OpenCNCProdFile(int ProgramNumber)
        {
            string tmpPath = Constants.MainPath + Constants.tmpPath;
            try
            {
                Directory.CreateDirectory(tmpPath);
                Util.SetPermissions(tmpPath);
            }
            catch (DirectoryNotFoundException e)
            {
                MessageBox.Show(e.Message);
            }
            catch (UnauthorizedAccessException e)
            {
                MessageBox.Show(e.Message);
            }
            catch
            {
                MessageBox.Show("Erreur lors de l'ouverture d'un fichier");
            }

            GeneratorData data = new GeneratorData();
            data.ProgramNumber = ProgramNumber;
            /*___________________________________*/

            //si on a les fichiers =>
            reset();

            ReadLAY(ProgramNumber);
            ReadGPH(ProgramNumber);
            ReadDAT(ProgramNumber, ref data);
            ReadNC(ProgramNumber);




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

            int EntitieType;
            try 
            {
                EntitieType = int.Parse(file[num_line]);
            }
            catch(Exception e)
            {
                //failed, gph ill formed
                MessageBox.Show("Fichier GPH mal formé" + e.Message);
                num_line += 10;
                return null;
            }
            switch (EntitieType)
            {
                //cercle
                case 1:
                    {
                        Circle c = new Circle();
                        double centerX = Double.Parse(file[++num_line], CultureInfo.InvariantCulture);
                        double centerY = Double.Parse(file[++num_line], CultureInfo.InvariantCulture);
                        c.Center = Util.AdjustPointGPH(new CSMath.XYZ(centerX, centerY, 0), layout.offset, layout.dimension, new Scale(true, false));

                        ++num_line;
                        double radius = Double.Parse(file[++num_line], CultureInfo.InvariantCulture);
                        c.Radius = radius;
                        num_line += 6;

                        return c;
                    }

                //ligne
                case 4:
                    {
                        ACadSharp.Entities.Line l = new ACadSharp.Entities.Line();
                        double startX = Double.Parse(file[++num_line], CultureInfo.InvariantCulture);
                        double startY = Double.Parse(file[++num_line], CultureInfo.InvariantCulture);
                        l.StartPoint = Util.AdjustPointGPH(new CSMath.XYZ(startX, startY, 0), layout.offset, layout.dimension, new Scale(true, false));
                        double endX = Double.Parse(file[++num_line], CultureInfo.InvariantCulture);
                        double endY = Double.Parse(file[++num_line], CultureInfo.InvariantCulture);
                        l.EndPoint = Util.AdjustPointGPH(new CSMath.XYZ(endX, endY, 0), layout.offset, layout.dimension, new Scale(true, false));
                        num_line += 6;


                        return l;
                    }
                //arc
                case 3:
                    {
                        Arc a = new Arc();
                        double centerX = Double.Parse(file[++num_line], CultureInfo.InvariantCulture);
                        double centerY = Double.Parse(file[++num_line], CultureInfo.InvariantCulture);
                        a.Center = Util.AdjustPointGPH(new CSMath.XYZ(centerX, centerY, 0), layout.offset, layout.dimension, new Scale(true, false));

                        ++num_line;
                        double radius = Double.Parse(file[++num_line], CultureInfo.InvariantCulture);
                        a.Radius = radius;
                        a.StartAngle = (Double.Parse(file[++num_line], CultureInfo.InvariantCulture)) * Math.PI / 180;
                        a.EndAngle = (Double.Parse(file[++num_line], CultureInfo.InvariantCulture)) * Math.PI / 180; ;
                        num_line += 4;

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
            data.machine = Machine.KTS850;
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


            layout.dimension = new RectangleF(float.Parse(file[0], CultureInfo.InvariantCulture),
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
                stud.Radius = Constants.StudRadius3;//toujours 3 (default)
                stud.Color = ACadSharp.Color.Green;

                Studs.Add(new Stud(stud));
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

        /*_____________________________________ARE_PROGRAM_____________________________________*/
        /**
        * <summary>attempt to open an ARE program's file via it's name and import it into the application</summary>
        */
        public void OpenAREProdFile(string filename)
        {
            string tmpPath = Properties.Settings.Default.OutputPath + Constants.tmpPath;
            try
            {
                Directory.CreateDirectory(tmpPath);
                Util.SetPermissions(tmpPath);
            }
            catch (DirectoryNotFoundException e)
            {
                MessageBox.Show(e.Message);
            }
            catch (UnauthorizedAccessException e)
            {
                MessageBox.Show(e.Message);
            }
            catch
            {
                MessageBox.Show("Erreur lors de l'ouverture d'un fichier");
            }

            /*___________________________________*/

            //si on a les fichiers =>
            reset();

            ReadARE(filename); 

            SaveToFile(tmpPath + @"\dxftmp.ddxf");

            open = true;

        }

        /*_____________________________________ARE_____________________________________*/

        private void ReadARE(string filepath)
        {

            string[] file = File.ReadAllLines(filepath);

            int numline = 0;

            ReadAREParam(ref file, ref numline);

            ReadAREStuds(ref file, ref numline);

            //ReadAREComments(ref file, ref numline);
            //on ne lit pas les commentaires d'un fichier are, il n'y en a pas 
        }

        /**
         * <summary>Reads the parameters from the ARE file</summary>
         */
        public void ReadAREParam(ref string[] file, ref int numline)
        {

            ref int[] param = ref Gen.PTS_300_CURRENT_PARAM;
            //get les valeurs des paramètres du fichier, si une valeur n'est pas un entier valide, affiche un message d'erreur et arrête la lecture des paramètres
            while (numline < AREProdFileGenerator.AREParameterSize )
            {
                try
                {

                    param[numline] = Convert.ToInt32(file[numline]);
                }
                catch (Exception e)
                {
                    MessageBox.Show("echec de la lecture d'un paramètre" + e.Message + Environment.NewLine + "valeur de paramètre invalide : " + file[numline]);
                    return;
                }
                numline++;
            }

        }


        /**
         * <summary>Reads the stud information from the ARE file and add them to the collection</summary>
         */
        private void ReadAREStuds(ref string[] file, ref int numline)
        {
            if (numline < AREProdFileGenerator.AREParameterSize) {
                return;
            }

            int nbStuds = 0;
            double X = 0, Y = 0;
            while (numline < AREProdFileGenerator.AREProgramSize)
            {
                //X
                try
                {
                    X = Convert.ToDouble(file[numline++]);
                }
                catch (Exception e)
                {
                    MessageBox.Show("Fichier ARE mal formé, lecture de la position X d'un goujon impossible, passage au suivant" + e.Message);
                    numline += 2;
                    continue;
                }
                //Y
                try
                {
                    Y = Convert.ToDouble(file[numline++]);
                }
                catch (Exception e)
                {
                    MessageBox.Show("Fichier ARE mal formé, lecture de la position Y d'un goujon impossible, passage au suivant" + e.Message);
                    numline += 1;
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

        /**
         * <summary>read the comments from the are file.</summary>
         * <remarks>Unused</remarks>
         */
        public void ReadAREComments(ref string[] file, ref int numline)
        {
            if (numline < AREProdFileGenerator.AREProgramSize)
            {
                return;
            }

            ref string[] comms = ref Gen.PTS_300_CURRENT_COMMENTS;


            //get les commentaires des paramètres du fichier si il y en a
            while (numline < file.Length)
            {
                //for each comment, get the line it belongs to
                //format =
                //$linenumer,1
                //comment

                int line = -1;
                string input = (file[numline]);

                // Remove $ and ,1 characters => isolate the value
                string cleaned = input.Replace("$", "").Split(',')[0];

                //parse the cleaned string to an integer
                try
                {
                    int.TryParse(cleaned, out line);
                }
                catch (Exception e)

                {
                    MessageBox.Show("Erreur lors de la lecture des commentaires du fichier de paramètres. " + e.Message);
                    return;
                }
                try
                {
                    comms[line] = file[++numline];
                }
                catch (Exception e)
                {
                    MessageBox.Show("Numero de commentaire invalide. " + e.Message);
                }
                numline++;

            }

        }


        /*_____________________________________SAVE_____________________________________*/


        /**
         * <summary>Save the Document to a dxf file with the StudList and potential modifications</summary>
         */
        public void SaveToFile(BindingList<Stud> Studs, string path)
        {
            /*________________________________DXF_COPY________________________________*/

            //creation d'un nouveau document pour accomoder la sauvegarde du document (copie du document courant) 
            CadDocument ddxf = new CadDocument();
            string savepath = Path.GetDirectoryName(path) + @"\" + Path.GetFileNameWithoutExtension(path);

            //ajout de clones de toutes les entités presentes dans le doc dans la sauvegarde.
            foreach (Entity entity in Doc.Entities)
            {
                try
                {
                    ddxf.Entities.Add((Entity)entity.Clone());
                }
                catch (ArgumentException)
                {
                    //MessageBox.Show(entity.ToString());
                }
            }


            using (DxfWriter writer = new DxfWriter(savepath + ".ddxf", ddxf))
            {
                writer.OnNotification += NotificationHelper.LogConsoleNotification;
                writer.Write();
            }

            /*________________________________DXF_STUDS________________________________*/

            //creation d'un nouveau document pour accomoder la sauvegarde des goujons (goujons détectés dans de document)
            CadDocument sdxf = new CadDocument();

            //ajout de clones des goujons dans la sauvegarde.
            foreach (Stud stud in Studs)
            {
                Circle clone = (Circle)stud.circle.Clone();
                clone.Center = Util.AdjustPoint(clone.Center, layout.offset, layout.dimension, layout.scale);
                clone.Color = ACadSharp.Color.Green;
                sdxf.Entities.Add(clone);
            }

            using (DxfWriter writer = new DxfWriter(savepath + ".sdxf", sdxf))
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
                catch (ArgumentException)
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
         * <remarks>Machine related information are managed by the generator type</remarks>
         */
        public void GenerateProdFiles(BindingList<Stud> Studs, CadObjectCollection<Entity> Entities, RectangleF Dimension, PointF Offset, GeneratorData Data, Scale Scalefact, double rotation, string filename)
        {
            Gen.UpdateData( Studs, Entities, Dimension, Offset, Scalefact, rotation);
            
            Gen.GenerateProductionFiles(filename, Data);
        }

        /**
         * <summary>Generates the productoin files necessary to the driver to function</summary>
         * <remarks>Machine related information are managed by the generator type</remarks>
         */
        public void GenerateProdFiles(BindingList<Stud> Studs, CadObjectCollection<Entity> Entities, GeneratorData Data, Layout_Info layout, double rotation)
        {
            Gen.UpdateData(Studs, Entities, layout.dimension, layout.offset, layout.scale, rotation);

            Gen.GenerateProductionFiles(Data.ProgramNumber.ToString(),Data);
        }

        /*_____________________________________PTS_300_PARAMETERS_____________________________________*/

        /**
          * <summary>Retrieves the parameters for the PTS300 machine, if no parameters are found, default values are used</summary>
          */
        public void GetParametersPTS300()
        {
            string ParamPath = Constants.MainPath + Constants.paramPath;
            AREProdFileGenerator AREGen = (AREProdFileGenerator)Gen;
            if (File.Exists(ParamPath + @"PTS_300_PARAM.txt"))
            {
                AREGen.ReadPTS300Parameters(ParamPath + @"\PTS_300_PARAM.txt");
                AREGen.SaveCurrentValues();
            }
            else
            {
                AREGen.GetDefaultPTS300Parameters();
                AREGen.SaveCurrentValues();
                AREGen.writePTS300Parameters();
            }
        }

        /**
         * <summary>Retrieves the parameters for the PTS300 machine</summary>
         * <returns>A reference to the array of parameters</returns>
         */
        public ref int[] GetPTS300CurrentParameters()
        {
            return ref Gen.PTS_300_CURRENT_PARAM;
        }

        /**
         * <summary>Retrieves the comments for the PTS300 machine parameters</summary>
         * <returns>A reference to the array of comments</returns>
         */
        public ref string[] GetPTS300CurrentComments()
        {
            return ref Gen.PTS_300_CURRENT_COMMENTS;
        }

        /**
         * <summary>Retrieves the saved parameters for the PTS300 machine</summary>
         * <returns>A reference to the array of parameters</returns>
         */
        public ref int[] GetPTS300SavedParameters()
        {
            return ref Gen.PTS_300_SAVE_PARAM;
        }
        
        /**
         * <summary>Retrieves the saved comments for the PTS300 machine parameters</summary>
         * <returns>A reference to the array of comments</returns>
         */
        public ref string[] GetPTS300SavedComments()
        {
            return ref Gen.PTS_300_SAVE_COMMENTS;
        }

        /**
         * <summary>Resets the PTS300 parameters to their default values</summary>
         */
        public void ResetPTS300ParamToDefault()
        {
            (Gen as AREProdFileGenerator).GetDefaultPTS300Parameters();
        }


        /**
         * <summary>Saves the current PTS300 parameters</summary>
         */
        public void SavePTS300Settings()
        {
            (Gen as AREProdFileGenerator).SaveCurrentValues();
            (Gen as AREProdFileGenerator).writePTS300Parameters();

        }

        internal void ReadRecivedAREProgram(object[] recived )
        {

            (Gen as AREProdFileGenerator).ReadRecievedAREProgram(recived);
            
        }

        /*______________________________________________*/

    }
}
