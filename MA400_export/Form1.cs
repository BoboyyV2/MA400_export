using ACadSharp.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Security;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace MA400_export
{


    //mode d'edition à la souris
    public enum EditMode
    {
        Cursor,
        SelectStud,
        AddStud,
        RemoveStud
    }

    public partial class MA400_export : Form
    {

        //used variables

        //Zoom variables
        public float _Zoom = 1.0f;
        float zoomValue = 0.2f;

        //position variables
        public PointF Origin_Offset = new PointF(0f, 0f);
        public PointF CursorPosition = new PointF(0f, 0f);


        public FileSystem fs = new FileSystem();
        public GraphicsContainer gc = new GraphicsContainer();


        private string savepath = string.Empty;

        //Edit variables
        public EditMode editMode = EditMode.Cursor;
        GeneratorData data = new GeneratorData();
        SerialData serialData = new SerialData();
        public bool IsNew = true;

        private double currentFramedCircleHash = -1.0;

        public Machine machine;

        //name of the opened file without the extension, used for the name of the output files
        public string fileName = "";

        //Serial
        public SerialConnection _serial = null;
        public bool ExecutingSerialCommand = false;

        public MA400_export()
        {
            InitializeComponent();
            StudList_Display.DataSource = fs.Studs;
        }

        private void Form1_Close(object sender, FormClosingEventArgs e)
        {
            if (_serial != null)
            { 
            _serial.CloseConnection();
            }
        }

        /**
         * <summary>Set some utilities on app launch</summary>
         */
        private void Form1_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.OutputPath.Length < 1)
            {
                Properties.Settings.Default.OutputPath = Constants.Outputpath;
                Properties.Settings.Default.Save();

            }

            //selection machine
            bool selected = false;
            //boucle jusqu'a ce qu'on choisisse
            while (!selected)
            {
                using (MachineSelection ms = new MachineSelection())
                {
                    if (ms.ShowDialog() == DialogResult.OK)
                    {
                        machine = ms.getMachine();
                        if (machine != Machine.None)
                        {
                            selected = true;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de la séléction de la machine, réouverture de la séléction");
                    }

                }
            }

            fs.setGenerator(machine);
            switch (machine)
            {
                case Machine.KTS850:
                    {

                        break;
                    }
                case Machine.PTS300:
                    {
                        paramètresPTS300ToolStripMenuItem.Visible = true;
                        paramètresInterfaceToolStripMenuItem.Visible = true;
                        this.buttonStart.Visible = true;
                        fs.GetParametersPTS300();

                        if (!string.IsNullOrEmpty(Properties.Settings.Default.OutputPath))
                        {
                            this.saveFileDialogARE.InitialDirectory = Properties.Settings.Default.OutputPath + Constants.ArePath;
                        }

                        if(File.Exists(Constants.MainPath + Constants.paramPath + "SerialData.json"))
                        {
                            serialData = Util.readSerialDataJson(Constants.MainPath + Constants.paramPath + "SerialData.json");
                        }
                        else
                        {
                            serialData.DataBits = 8;
                            serialData.ParityBit = System.IO.Ports.Parity.None;
                            serialData.StopBit = System.IO.Ports.StopBits.One;
                            serialData.BaudRate = 9600;
                            serialData.UpdateInterval = 502;
                        }

                        break;
                    }
                default:
                    {
                        break;
                    }
            }

        }

        /**
         * <summary>reset to a state where no part is loaded</summary>
         */
        private void reset()
        {
            data = new GeneratorData();
            IsNew = true;
            fs.reset();
            gc.reset();
        }

        /**
         * <summary>Empties the studlist</summary>
         */
        public void EmptyStuds()
        {
            fs.Studs.Clear();
        }

        /**
         * <summary>add a stud to the local collection</summary>
         */
        public void AddStud(Circle stud)
        {

            fs.Studs.Add(new Stud(stud));

        }

        /**
         * <summary>add a stud to the local collection</summary>
         */
        public void AddStud(double x, double y, double diam)
        {
            Circle circle = new Circle();

            circle.Radius = diam / 2;
            circle.Center = new CSMath.XYZ(x, y, 0);
            AddStud(circle);
        }

        /**
         * <summary>paint methode for the workzone, invoke the graphics container.</summary>
         */
        private void WorkZone_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            //draw the basic from of the workzone
            //including but not restricted to :
            //the background, the rectangular coordinate system, the scale
            //the workzone, the landmarks

            gc.graphics = e.Graphics;
            gc.graphics.ScaleTransform(_Zoom, _Zoom);
            gc.graphics.TranslateTransform(-Origin_Offset.X, -Origin_Offset.Y);

            gc.Paint(fs.Studs, getSelectedStuds(), gc.layout.offset);

        }

        /**
         * <returns> the list of selected studs in the studlist</returns>
         */
        public List<Stud> getSelectedStuds()
        {

            //start by getting the data
            List<Stud> selectedList = new List<Stud>();
            foreach (Stud selectedStud in StudList_Display.SelectedItems)
            {
                selectedList.Add(selectedStud);
            }
            return selectedList;
        }

        /**
         * <returns>the coordinates of the point in the panel after taking into account the offset and the zoom</returns>
         * <param name="p">the point where the cusror is on the panel (without prior modification)</param>
         */
        public PointF GetOffsetedCoords(PointF p)
        {
            return new PointF(Origin_Offset.X - Constants.Origin_Coord.X + (p.X / _Zoom), Origin_Offset.Y - Constants.Origin_Coord.Y + (p.Y / _Zoom));
        }


        /**
         *<returns>a point to the coordonate of the cursor in the pannel without adjusting it</returns>
         */
        public System.Drawing.Point getCoords()
        {
            return WorkZone.PointToClient(Cursor.Position);

        }

        /**
         * <summary>Update the coordinates display under the workzone</summary>
         */
        public void UpdateCoords()
        {
            System.Drawing.Point point = WorkZone.PointToClient(Cursor.Position);

            CursorPosition = GetOffsetedCoords(point);
            XCoord_Display.Text = " X = " + (CursorPosition.X).ToString("0.0") + " ";
            YCoord_Display.Text = " Y = " + (CursorPosition.Y).ToString("0.0") + " ";

        }


        /**
         * <summary>change the displayed coordinates of the hovered circle to that of the given Circle.</summary>
         */
        private void DisplayHoveredCircleCoord(Circle c)
        {
            labelHoveredCircleX.Text = " X = " + c.Center.X.ToString("0.0", CultureInfo.InvariantCulture);
            labelHoveredCircleY.Text = " Y = " + c.Center.Y.ToString("0.0", CultureInfo.InvariantCulture);
        }

        /**
         * <summary>remove the displayed coordinates of the hovered circle.</summary>
         */
        private void RemoveDisplayedHoveredCircleCoord()
        {
            labelHoveredCircleX.Text = "";
            labelHoveredCircleY.Text = "";
        }

        /**
         * <summary>If the Mouse cursor is hovering a circle, put a frame around it and update the displayed coordinates of said circle</summary>
         */
        private void TryFrame()
        {
            Circle c;
            fs.TryFrame(CursorPosition, out c);

            //security
            double hash = Util.HashCircleCenter(c.Center);

            //pas de changement
            if (hash == currentFramedCircleHash)
            {
                return;
            }

            //update
            currentFramedCircleHash = hash;

            // < 0 = pas dans un cercle
            if (hash < 0)
            {
                gc.RemoveFramedCircle();
                RemoveDisplayedHoveredCircleCoord();
            }
            else
            {
                gc.FrameCircle(c);
                DisplayHoveredCircleCoord(c);
            }
            WorkZone.Invalidate();

        }

        /**
         * <summary>Update the coordinates display whenever the mouse moves inside the workzone and if the mouse is sitting on a circle, frame it</summary>
         */
        private void WorkZone_MouseMove(object sender, MouseEventArgs e)
        {
            UpdateCoords();
            TryFrame();
        }


        /*_________________________________________ZOOM_________________________________________*/
        /**
         * <summary>function used to compute the zoom offset </summary>
         */
        private void updateOrigin_Offset(PointF predicted_offset, float offsetX_min, float offsetX_max, float offsetY_min, float offsetY_max, float zoom_delta)
        {

            //offsetX
            if (predicted_offset.X < offsetX_min)
            {
                Origin_Offset.X = offsetX_min;

            }
            else if (predicted_offset.X > offsetX_max)
            {
                Origin_Offset.X = offsetX_max;
            }
            else
            {
                Origin_Offset.X = predicted_offset.X;
            }

            //offsetY
            if (predicted_offset.Y < offsetY_min)
            {
                Origin_Offset.Y = offsetY_min;

            }
            else if (predicted_offset.Y > offsetY_max)
            {
                Origin_Offset.Y = offsetY_max;
            }
            else
            {
                Origin_Offset.Y = predicted_offset.Y;
            }

        }

        /**
         * <summary>Zoom or dezoom on the workzone whenever the mouse is scrolled.</summary>
         * <remarks>As much as the zoom is functionning it is by no mean perfect.</remarks>
         */
        private void WorkZone_Zoom(object sender, MouseEventArgs e)
        {
            //compute the zoom values
            float zoom_delta = (e.Delta > 0 ? zoomValue : -zoomValue);
            float new_Zoom = Math.Max(Math.Min(_Zoom + zoom_delta, Constants.Max_Zoom), Constants.Min_Zoom);

            //if needs be
            if (new_Zoom != _Zoom)
            {
                //update mouse position
                UpdateCoords();
                zoom_delta = new_Zoom - _Zoom;

                //check if we stay in bounds
                PointF predicted_offset = new PointF(Origin_Offset.X + CursorPosition.X / _Zoom * zoom_delta, Origin_Offset.Y + CursorPosition.Y / _Zoom * zoom_delta);
                float offsetX_min = -Constants.Origin_Coord.X - 141.6f;
                float offsetY_min = -Constants.Origin_Coord.X;


                //depends on the zoom (can't see further away than 50 out of the worspace)
                float offsetX_max = Constants.WorkZoneLimits_Coord.X + Constants.Origin_Coord.X - (WorkZone.Width / new_Zoom) + 141.6f;
                float offsetY_max = Constants.WorkZoneLimits_Coord.Y + Constants.Origin_Coord.Y - (WorkZone.Height / new_Zoom);


                updateOrigin_Offset(predicted_offset, offsetX_min, offsetX_max, offsetY_min, offsetY_max, zoom_delta);

                _Zoom = new_Zoom;

                UpdateCoords();
                //reframe a circle if needed
                TryFrame();

                WorkZone.Invalidate();
            }
        }

        


        /*___________________________________________STUDS_EDITING___________________________________________*/

        /**
         * <summary>let you select the previous stud if there is one</summary>
         */
        private void buttonPreviousStud_Click(object sender, EventArgs e)
        {
            if (StudList_Display.Items.Count > 0)
            {
                int index = StudList_Display.SelectedIndex;

                if ((StudList_Display.SelectedItems.Count == 1) && (index > 0))
                {
                    StudList_Display.SelectedItems.Clear();
                    StudList_Display.SelectedIndex = index - 1;
                }
                else
                {
                    StudList_Display.SelectedItems.Clear();
                    StudList_Display.SelectedIndex = StudList_Display.Items.Count - 1;
                }

            }

            StudList_Display.Focus();

            //refresh the graphics
            WorkZone.Invalidate();
        }

        /**
         * <summary>let you select the next stud if there is one</summary>
         */
        private void buttonNextStud_Click(object sender, EventArgs e)
        {
            if (StudList_Display.Items.Count > 0)
            {
                int index = StudList_Display.SelectedIndex;

                if ((StudList_Display.SelectedItems.Count == 1) && (index < StudList_Display.Items.Count - 1))
                {
                    StudList_Display.SelectedItems.Clear();
                    StudList_Display.SelectedIndex = index + 1;
                }
                else
                {
                    StudList_Display.SelectedItems.Clear();
                    StudList_Display.SelectedIndex = 0;
                }

            }

            StudList_Display.Focus();

            //refresh the graphics
            WorkZone.Invalidate();
        }

        /**
         * <summary>Tells if a circle can fit into the part.</summary>
         * <returns>true if it fits, false otherwise</returns>
         * <remarks>this uses a rectangle dimension which means it will accept more than it should as the part might contain non rectangular outer limits which are not fully supported</remarks>
         */
        private bool IsPointInBounds(Circle c, ref string error)
        {
            bool InBounds = true;
            //Bounds
            if ((c.Center.X > (Constants.WorkZoneLimits_Coord.X - Constants.Origin_Coord.X - c.Radius)) || //hors workzone droite
                 (c.Center.X < c.Radius) || // hors zone origin
                 (c.Center.X > (gc.layout.dimension.Width - c.Radius))) //hors pièce X
            {
                error += "La coordonée X saisie n'est pas un emplacement valide pour poser un goujon.\r\n";
                InBounds = false;
            }
            if ((c.Center.Y > (Constants.WorkZoneLimits_Coord.Y - Constants.Origin_Coord.Y - c.Radius)) || // hors workzone bas
                 (c.Center.Y < c.Radius) || // hors zone origin
                 (c.Center.Y > gc.layout.dimension.Height - c.Radius)) //hors pièce Y
            {
                error += "La coordonée Y saisie n'est pas un emplacement valide pour poser un goujon.\r\n";
                InBounds = false;
            }

            return InBounds;
        }


        /**
         * <summary>Tell if the input given for the addstudbutton is valid or not and display an error message if not.</summary>
         * <returns>true if everything is correct, false otherwise.</returns>
         */
        private bool AddStudButton_CheckInput(string X_string, string Y_string, string Diam_string)
        {
            if (!fs.open)
            {
                MessageBox.Show("Aucune pièce ouverte sur laquelle placer un goujon.");
                return false;
            }

            string regexp_int = @"^\s*[0-9]+\s*$";//int

            string regexp_nb = @"^[0-9]+(\.[0-9]+)?$";//nombre
            string error = "";

            //correct form
            if (!System.Text.RegularExpressions.Regex.IsMatch(X_string, regexp_nb))
            {
                if (X_string.Length > 0)
                {
                    error += "Veuillez ne saisir que des chiffres dans la coordonée X.\r\n";
                }
                else
                {
                    error += "Veuillez saisir une coordonnée X pour ajouter un goujon.\r\n";
                }
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(Y_string, regexp_nb))
            {
                if (Y_string.Length > 0)
                {
                    error += "Veuillez ne saisir que des chiffres dans la coordonée Y.\r\n";
                }
                else
                {
                    error += "Veuillez saisir une coordonnée Y pour ajouter un goujon.\r\n";
                }
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(Diam_string, regexp_int))
            {
                if (Diam_string.Length > 0)
                {
                    error += "Veuillez ne saisir que des chiffres pour le diamètre du goujon à ajouter.\r\n";
                }
                else
                {
                    error += "Veuillez saisir le diamète du ajouter un goujon.\r\n";
                }
            }
            if (error.Length > 0)
            {
                MessageBox.Show(error);
                return false;
            }
            //correct values
            double X = Convert.ToDouble(X_string, CultureInfo.InvariantCulture);
            double Y = Convert.ToDouble(Y_string, CultureInfo.InvariantCulture);
            Int32 D = Int32.Parse(Diam_string);

            //Diam
            if (D != 3 && D != 4)
            {
                error += "Le diamètre du goujon saisi est invalide.\r\n";
            }
            //Bounds
            Circle c = new Circle();
            c.Center = new CSMath.XYZ(X, Y, 0);
            c.Radius = (double)D / 2.0;
            string bound_error = "";
            if (!IsPointInBounds(c, ref bound_error))
            {
                error += bound_error;
            }

            //Error
            if (error.Length > 0)
            {
                MessageBox.Show(error);
                return false;
            }


            return true;
        }

        /**
         * <summary>Tell if the input given for the addstudbutton is valid or not and display an error message if not.</summary>
         * <returns>true if everything is correct, false otherwise.</returns>
         */
        private bool AddStudButtonOnClick_CheckInput(double X, double Y, string Diam_string)
        {
            if (!fs.open)
            {
                MessageBox.Show("Aucune pièce ouverte sur laquelle placer un goujon.");
                return false;
            }
            string regexp_int = @"\s*[0-9]+\s*";
            string error = "";

            //correct form
            if (!System.Text.RegularExpressions.Regex.IsMatch(Diam_string, regexp_int))
            {
                if (Diam_string.Length > 0)
                {
                    error += "Veuillez ne saisir que des chiffres pour le diamètre du goujon à ajouter.\r\n";
                }
                else
                {
                    error += "Veuillez saisir le diamète du ajouter un goujon.\r\n";
                }
            }
            if (error.Length > 0)
            {
                MessageBox.Show(error);
                return false;
            }
            //correct values
            int D = Int32.Parse(Diam_string);

            //Diam
            if (D != 3 && D != 4)
            {
                error += "Le diamètre du goujon saisi est invalide.\r\n";
            }
            //Bounds
            Circle c = new Circle();
            c.Center = new CSMath.XYZ(X, Y, 0);
            c.Radius = (double)D / 2.0;
            string bound_error = "";
            if (!IsPointInBounds(c, ref bound_error))
            {
                error += bound_error;
            }

            //Error


            if (error.Length > 0)
            {
                MessageBox.Show(error);
                return false;
            }

            return true;
        }

        /**
         * <summary>a Circle constructor wrapper</summary>
         */
        private Circle createStud(double x, double y, double radius)
        {
            Circle stud = new Circle();
            stud.Radius = radius;
            stud.Center = new CSMath.XYZ(x, y, 0);
            return stud;
        }

        /**
         * <summary>force the display to refresh whenever we change the selection in the studlist</summary>
         */
        private void StudList_Display_SelectedIndexChanged(object sender, EventArgs e)
        {
            //refresh the graphics
            WorkZone.Invalidate();
        }

        /**
         * <summary>Attempt to add a stud at the position given in the dedicated textbox when clicking the add button</summary>
         * <remarks>show an error if it is not possible</remarks>
         */
        private void buttonAddStud_Click(object sender, EventArgs e)
        {

            string X_string = this.textBox_StudCoord_X.Text;
            string Y_string = this.textBox_StudCoord_Y.Text;
            string Diam_string = this.comboBoxDiam.Text;

            //if the input is incorrect
            if (!AddStudButton_CheckInput(X_string, Y_string, Diam_string))
            {
                return;
            }



            double x = Convert.ToDouble(X_string, CultureInfo.InvariantCulture);
            double y = Convert.ToDouble(Y_string, CultureInfo.InvariantCulture);
            int diam = Int32.Parse(Diam_string);
            Circle Stud = createStud(x, y, ((double)diam) / 2);



            if (!Util.IsPossibleToAddStud(Stud, fs.Studs))
            {
                return;
            }

            // Shutdown the painting of the ListBox as items are added.
            StudList_Display.BeginUpdate();

            //Add it into the collection
            this.AddStud((double)x, (double)y, (double)diam);

            if (StudList_Display.Items.Count < 0)
            {
                StudList_Display.SetSelected(StudList_Display.Items.Count - 1, true);
            }

            // Allow the ListBox to repaint and display the new items.
            StudList_Display.EndUpdate();

            //refresh the graphics
            WorkZone.Invalidate();

        }

        /**
         * <summary>Remove all the selected Studs in the studlist when clicking the remove button</summary>
         */
        private void buttonRemoveStud_Click(object sender, EventArgs e)
        {


            // Shutdown the painting of the ListBox as items are removed.
            StudList_Display.BeginUpdate();


            //start by getting the data
            List<Stud> selectedList = getSelectedStuds();

            foreach (Stud selected in selectedList)
            {
                fs.Studs.Remove(selected);
            }


            StudList_Display.ClearSelected();

            StudList_Display.EndUpdate();

            //refresh the graphics
            WorkZone.Invalidate();

        }

        


        /**
         * <summary>Set the current mouseclick mode to mode</summary>
         */
        public void setEditMode(EditMode mode)
        {
            editMode = mode;
        }

        /**
         * <summary>Set the current mouseclick mode to Cursor</summary>
         */
        private void ButtonCursorMode_Click(object sender, EventArgs e)
        {
            setEditMode(EditMode.Cursor);
        }

        /**
         * <summary>Set the current mouseclick mode to Select</summary>
         */
        private void ButtonSelectStudMode_Click(object sender, EventArgs e)
        {
            setEditMode(EditMode.SelectStud);
        }

        /**
         * <summary>Set the current mouseclick mode to Add</summary>
         */
        private void ButtonAddStudMode_Click(object sender, EventArgs e)
        {
            setEditMode(EditMode.AddStud);
        }

        /**
         * <summary>Set the current mouseclick mode to Remove</summary>
         */
        private void ButtonremoveStudMode_Click(object sender, EventArgs e)
        {
            setEditMode(EditMode.RemoveStud);
        }


        /**
         * <summary>Set the given stud selection value to the given value in the listbox bound to the BindingList</summary>
         */
        private void setStudSelected(Stud stud, bool selectionValue)
        {
            int index = 0;
            foreach (Stud candidate in StudList_Display.Items)
            {
                if (candidate == stud)
                {
                    StudList_Display.SetSelected(index, selectionValue);
                    return;
                }
                index++;
            }
        }

        /**
         * <summary>
         * Handle the behavior when clicking somwhere on the WorkZone using the cursor mode.<br></br>
         * Select the Stud at the clicked coordinates if there is one, show an error otherwise.
         * </summary>
         */
        private void WorkZone_Click_SelectStud()
        {
            System.Drawing.Point p = getCoords();
            PointF offseted_p = GetOffsetedCoords(p);
            List<Stud> selected = getSelectedStuds();

            foreach (Stud stud in fs.Studs)
            {
                if (Util.getStudDistance(offseted_p, stud.circle) < stud.circle.Radius)
                {
                    bool newSelectionValue = true;
                    if (selected.Contains(stud))
                    {
                        newSelectionValue = false;
                    }
                    setStudSelected(stud, newSelectionValue);

                }
            }

        }


        /**
         * <summary>should only refresh
         * Handle the behavior when clicking somwhere on the WorkZone using the add mode and left click.<br></br>
         * Add a stud at the clicked coordinates if possible, show an error if it isn't.
         * </summary>
         */
        private void WorkZone_Click_AddStud()
        {
            System.Drawing.Point p = getCoords();
            string diam_String = this.comboBoxDiam.Text;
            PointF offseted_p = GetOffsetedCoords(p);
            if (!AddStudButtonOnClick_CheckInput((double)offseted_p.X, (double)offseted_p.Y, diam_String))
            {
                return;
            }
            double radius = (Double.Parse(diam_String)) / 2;
            Circle circle = createStud((double)offseted_p.X, (double)offseted_p.Y, radius);
            if (!Util.IsPossibleToAddStud(circle, fs.Studs))
            {
                return;
            }

            AddStud(circle);
            //should only refresh the pixel inside the rectangle
            this.WorkZone.Invalidate(new Rectangle((int)(offseted_p.X - radius + Constants.Origin_Coord.X), (int)(offseted_p.Y - radius + Constants.Origin_Coord.Y),
                                                    (int)(offseted_p.X + radius + Constants.Origin_Coord.X), (int)(offseted_p.Y + radius + Constants.Origin_Coord.Y)));
        }

        /**
         * <summary>
         * Handle the behavior when clicking somwhere on the WorkZone using the add mode and right click or ctrl + right click.<br></br>
         * Add a stud on the framed circle if possible, show an error otherwise.
         * </summary>
         */
        private void WorkZone_Click_Turn_Framed_Circle_Into_Stud()
        {
            Circle c = gc.GetFramedCircle();
            if (c == null)
            {
                return;
            }
            string diam_String = this.comboBoxDiam.Text;
            PointF offseted_p = new PointF((float)c.Center.X, (float)c.Center.Y);
            if (!AddStudButtonOnClick_CheckInput((double)offseted_p.X, (double)offseted_p.Y, diam_String))
            {
                return;
            }
            double radius = (Double.Parse(diam_String)) / 2;
            Circle circle = createStud((double)offseted_p.X, (double)offseted_p.Y, radius);
            if (!Util.IsPossibleToAddStud(circle, fs.Studs))
            {
                return;
            }

            AddStud(circle);
            
            //should only refresh the pixel inside the rectangle
            this.WorkZone.Invalidate(new Rectangle( (int)(offseted_p.X - radius + Constants.Origin_Coord.X), (int)(offseted_p.Y - radius + Constants.Origin_Coord.Y),
                                                    (int)(offseted_p.X + radius + Constants.Origin_Coord.X), (int)(offseted_p.Y + radius + Constants.Origin_Coord.Y) ) );
        }


        /**
         * <summary>
         * Handle the behavior when clicking somwhere on the WorkZone using the remove mode and left click.<br></br>
         * Remove the clicked stud if there is one, show a message if no stud were found.
         * </summary>
         */
        private void WorkZone_Click_RemoveStud()
        {
            PointF p_rm = getCoords();
            p_rm = GetOffsetedCoords(p_rm);

            bool removed = false;
            foreach (Stud stud in fs.Studs)
            {
                if (Util.getStudDistance(p_rm, stud.circle) < (stud.circle.Radius))
                {
                    fs.Studs.Remove(stud);
                    removed = true;
                    break;
                }

            }
            if (!removed)
            {
                MessageBox.Show("aucun goujon trouvé à cette position.");
            }
            //should only refresh the pixel inside the rectangle
            this.WorkZone.Invalidate(new Rectangle((int)(p_rm.X - Constants.StudRadius4 + Constants.Origin_Coord.X), (int)(p_rm.Y - Constants.StudRadius4 + Constants.Origin_Coord.Y),
                                                    (int)(p_rm.X + Constants.StudRadius4 + Constants.Origin_Coord.X), (int)(p_rm.Y + Constants.StudRadius4 + Constants.Origin_Coord.Y)));
        }



        /**
         * <summary>Handle the behavior when clicking somwhere on the WorkZone depending on the mode</summary>
         */
        private void WorkZone_Click(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            //check if the control key is pressed
            if ((Control.ModifierKeys & Keys.Control) != 0)
            {
                WorkZone_Click_Turn_Framed_Circle_Into_Stud();
                return;
            }

            if (e.Button == MouseButtons.Left)
            {
                switch (editMode)
                {

                    //select
                    case EditMode.SelectStud:
                        WorkZone_Click_SelectStud();
                        break;

                    //add
                    case EditMode.AddStud:
                        WorkZone_Click_AddStud();
                        break;

                    //remove
                    case EditMode.RemoveStud:
                        WorkZone_Click_RemoveStud();
                        break;

                    default:
                        //cursor
                        break;
                }
            }
            else if (e.Button == MouseButtons.Right)
            {

                switch (editMode)
                {

                    //add
                    case EditMode.AddStud:
                        WorkZone_Click_Turn_Framed_Circle_Into_Stud();
                        break;
                }
            }

        }

        /*___________________________________________GENERATE_PROD_FILES___________________________________________*/


        /**
         * <summary>Open the Form to generate the driver's files and get the data.</summary>
         * <returns>true if the data was succesfully retrived (press OK), false otherwise.</returns>
         */
        private bool GetFormData()
        {
            //depending on whether we already created the program
            if (IsNew)
            {
                using (FormGenerateInfo formInfo = new FormGenerateInfo())
                {
                    if (formInfo.ShowDialog() == DialogResult.OK)
                    {
                        data = formInfo.Data;
                        IsNew = false;
                        return true;
                    }

                }
            }
            else
            {
                using (FormGenerateInfo formInfo = new FormGenerateInfo(data))
                {
                    if (formInfo.ShowDialog() == DialogResult.OK)
                    {
                        data = formInfo.Data;
                        return true;
                    }
                }
            }
            return false;
        }


        /**
         * <summary>Attempt to generate the files.</summary>
         */
        private void GenerateOutput()
        {
            if (!fs.open)
            {
                MessageBox.Show("impossible de générer les fichiers de production :\r\nAucun fichier dxf ou programme n'est ouvert.");
                return;
            }

            switch (machine)
            {
                case Machine.KTS850:
                    {
                        //show form then gen
                        if (GetFormData())
                        {
                            fs.GenerateProdFiles(fs.Studs, fs.Doc.Entities, gc.layout.dimension, gc.layout.offset, data, gc.layout.scale, fs.rotation, data.ProgramNumber.ToString()); // en dernier, une fois que tout est bien rempli
                        }
                        break;
                    }
                case Machine.PTS300:
                    {
                        //opensave dialog
                        if(saveFileDialogARE.ShowDialog() == DialogResult.OK)
                        {
                            fs.GenerateProdFiles(fs.Studs, fs.Doc.Entities, gc.layout.dimension, gc.layout.offset, data, gc.layout.scale, fs.rotation, saveFileDialogARE.FileName);//most parameters are unused
                            MessageBox.Show("Fichiers de production générés avec succès.");
                        }
                        
                        break;
                    }
                default:
                    {
                        MessageBox.Show("impossible de générer les fichiers de production :\r\nMachine non sélectionnée.");
                        return;
                    }
            }
            

        }

        /**
         * <summary>Open the form to generate the outputfiles when clicking the generate button</summary>
         */
        private void buttonGenerer_Click(object sender, EventArgs e)
        {
            GenerateOutput();
        }

        /**
         * <summary>Open the form to generate the outputfiles when clicking the generate button in the menu</summary>
         */
        private void générerLesFichiersDeSortieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GenerateOutput();
        }

        /*___________________________________________FILE_MENU___________________________________________*/

        /**
         * <summary>Show a fileopen menu made to open dxf files</summary>
         */
        private void ouvrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialogOpen.ShowDialog();

        }

        /**
         * <summary>Open a dxf or ddxf file and import it once it has been correctly selected via a fileopen menu</summary>
         */
        private void openFileDialogOpen_FileOk(object sender, CancelEventArgs e)
        {
            reset();
            bool open = false;
            string filename = this.openFileDialogOpen.FileName;
            string extension = Path.GetExtension(filename);
            fileName = Path.GetFileNameWithoutExtension(filename);

            switch (extension)
            {
                //fichier dxf classique
                case ".dxf":
                    try
                    {
                        open = this.fs.OpenDxfFile(filename);
                    }
                    catch (SecurityException ex)
                    {
                        MessageBox.Show($"Security error.\r\nError message: {ex.Message}\r\n" +
                        $"Details:\r\n{ex.StackTrace}");
                        return;
                    }

                    if (!open)
                    {
                        MessageBox.Show("Echec de l'ouverture du fichier.");
                    }

                    gc.OpenCanvas();
                    fs.OpenDxfFileLayout(gc.layout);

                    break;

                //fichier dxf modifié par nos soins
                case ".ddxf":
                    try
                    {
                        open = this.fs.OpenDDxfFile(filename);
                    }
                    catch (SecurityException ex)
                    {
                        MessageBox.Show($"Security error.\r\nError message: {ex.Message}\r\n" +
                        $"Details:\r\n{ex.StackTrace}");
                        return;
                    }

                    if (!open)
                    {
                        MessageBox.Show("Echec de l'ouverture du fichier.");
                    }

                    gc.OpenCanvas();
                    fs.OpenDdxfFileLayout(gc.layout, filename);

                    break;

                default:
                    MessageBox.Show("Extention de fichier invalide." + Environment.NewLine +
                                    "Doit être .dxf ou .ddxf");
                    return;
            }


            DisplayWhenOpen(open);
        }

        /**
         * <summary>Open a SaveFileDialog menu to Save the part<br></br>
         * if the file has already been saved once this session, simply overwrite the existing file</summary>
         */
        private void enregistrerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (savepath.Length == 0)
            {
                //pas encore de sauvegarde, on forward au menu
                enregistrersousToolStripMenuItem_Click(sender, e);
                return;
            }
            fs.SaveToFile(fs.Studs, savepath);

        }

        /**
         * <summary>Open a SaveFileDialog menu to Save the part</summary>
         */
        private void enregistrersousToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialogSave.FileName = fileName;
            saveFileDialogSave.ShowDialog();
        }

        /**
         * <summary>Save a file at the path specified in the SaveFileDialog</summary>
         */
        private void saveFileDialogSave_FileOk(object sender, CancelEventArgs e)
        {
            savepath = saveFileDialogSave.FileName;
            fs.SaveToFile(fs.Studs, savepath);
        }


        /**
         * <summary>exit the program</summary>
         */
        private void quitterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        /**
         * <summary>Open a CNCprogram via an existing program number</summary>
         */
        private void ouvrirprogramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (ProgramNumberOpen programNumberWindow = new ProgramNumberOpen())
            {
                if (programNumberWindow.ShowDialog() == DialogResult.OK)
                {
                    int ProgramNumber = programNumberWindow.ProgramNumber;
                    reset();
                    data = fs.OpenCNCProdFile(ProgramNumber);

                    IsNew = false;

                    gc.OpenCanvas();
                    fs.OpenProdFileLayout(gc.layout);

                    DisplayWhenOpen(true);
                }

            }

        }

        /**
         * <summary>Open an are program via an program name</summary>
         */
        private void ouvrirUnProgramAreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialogARE.ShowDialog() == DialogResult.OK)
            {
                
                string filename = this.openFileDialogARE.FileName;
                if (filename == null || !File.Exists(filename) ) 
                {
                    MessageBox.Show("Fichier séléctionné invalide");
                    return;
                }

                reset();
                fs.OpenAREProdFile(filename);

                IsNew = false;

                gc.OpenCanvas();
                fs.OpenProdFileLayout(gc.layout);

                DisplayWhenOpen(true);
            }

            
            

            
        }

        /**
         * <summary>Prepare the display whenever we open a file or programm</summary>
         */
        private void DisplayWhenOpen(bool open)
        {
            if (!open)
            {
                gc.reset();
            }

            //clear the current list
            //EmptyStuds();not handled by the form anymore

            //and fill it with the correct studs
            StudList_Display.DataSource = fs.Studs;


            this.WorkZone.Invalidate(new Rectangle( (int)Constants.Origin_Coord.X, (int)Constants.Origin_Coord.Y,
                                                    (int)(Constants.Origin_Coord.X + Constants.WorkZoneLimits_Coord.X) ,
                                                    (int)(Constants.Origin_Coord.Y + Constants.WorkZoneLimits_Coord.Y) ) );
        }


        

        /*___________________________________________SETTINGS___________________________________________*/


        /**
         * <summary>Show the options form when the option button is clicked</summary>
         */
        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //ouvre les options
            using (UserSettings settingsWindow = new UserSettings())
            {
                settingsWindow.ShowDialog();
                //c'est tout
            }
        }


        /**
         * <summary>Show the PTS300 settings form when the menu item is clicked</summary>
         */
        private void paramètresPTS300ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //ouvre les options
            using (PTS300Settings settingsWindow = new PTS300Settings(ref fs.GetPTS300CurrentParameters(), ref fs.GetPTS300CurrentComments()))
            {
                settingsWindow.mainParent = this;
                settingsWindow.ShowDialog();
                //c'est tout
            }
            
        }

        /**
         * <summary>Show the serial port settings form when the menu item is clicked</summary>
         * <remarks>also open the serial connection at the end</remarks>
         */
        private void paramètresInterfaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //TODO menu de paramètres du port série
            //ouvre les options
            using (SerialPortSettings serialSettings = new SerialPortSettings(serialData) )
            {

                serialSettings.ShowDialog();
                serialData = serialSettings.data;

                if (serialSettings.tryConnect)
                {
                    //connect
                    connect_serial();
                    serialSettings.tryConnect = false;
                }
            }
            
        }

        /*___________________________________________ROTATE___________________________________________*/


        /**
         * <summary>Rotate the part when the rotate 180° whe the button is clicked</summary>
         */
        private void rotateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!fs.open)
            {
                MessageBox.Show("aucun fichier ou programme ouvert.");
                return;
            }
            fs.RotatePart180();
            gc.reset();
            gc.OpenCanvas();
            fs.OpenRotatedFileLayout(gc.layout, 180);
            DisplayWhenOpen(true);
            WorkZone.Invalidate();

        }

        /**
         * <summary>Rotate the part when the rotate 90° clockwise when the button is clicked</summary>
         */
        private void Rotate90ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!fs.open)
            {
                MessageBox.Show("aucun fichier ou programme ouvert.");
                return;
            }
            fs.RotatePart90();
            gc.reset();
            gc.OpenCanvas();
            fs.OpenRotatedFileLayout(gc.layout, 90);
            DisplayWhenOpen(true);
            WorkZone.Invalidate();
        }


        /**
         * <summary>Handle the behavior when the rotation button is clicked.<br></br>
         * Call the getRotationValue form to get the rotation angle then rotate the part.</summary>
         */
        private void rotationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            if (!fs.open)
            {
                MessageBox.Show("aucun fichier ou programme ouvert.");
                return;
            }

            using (getRotationValue rotationForm = new getRotationValue())
            {
                if (rotationForm.ShowDialog() == DialogResult.OK)
                {   
                    double rotationValue = rotationForm.Value;
                    fs.RotatePart(rotationValue);
                    gc.reset();
                    gc.OpenCanvas();
                    fs.OpenRotatedFileLayout(gc.layout, rotationValue);
                    DisplayWhenOpen(true);
                    WorkZone.Invalidate();
                }
                
            }
            
        }


        /*___________________________________________PANEL_RESIZE___________________________________________*/


        /**
         * <summary>change the Worzone size whenever we resize the form</summary>
         */
        private void Form1_Resize(object sender, EventArgs e)
        {
            WorkZone.Size = new Size(this.ClientSize.Width - Constants.Client_panel_delta_width, this.ClientSize.Height - Constants.Client_panel_delta_height);

        }


        /*___________________________________________PRINT___________________________________________*/

        /**
         * <summary>Print the document when the print menu item is clicked</summary>
         * <remarks>Uses the PrintHelper class to handle the printing logic.</remarks>
         */
        private void imprimerToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var helper = new PrintHelper(gc, fs.Studs, _Zoom, Origin_Offset);
            helper.Print(this);
        }

        /**
         * <summary>Show the print preview when the print preview menu item is clicked</summary>
         * <remarks>Uses the PrintHelper class to handle the printing logic.</remarks>
         */
        private void afficherLaperçuToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            var helper = new PrintHelper(gc, fs.Studs, _Zoom, Origin_Offset);
            helper.ShowPreview(this);//this = fenetre parente (owner)
        }


        /*___________________________________________ SERIAL PORT FUNSIES ___________________________________________*/

        /**
         * <summary>Connect to the new serial port.</summary>
         */
        private void connect_serial()
        {
            if (_serial == null)
            {
                _serial = new SerialConnection();
            }
            else
            {
                disconnect_serial();
                _serial = new SerialConnection();
            }
            if (_serial.OpenConnection(serialData))
            {
                _serial.StartListening();   //démarage de l'écoute (positions X/Y ?) donc au when ?
                MessageBox.Show($"Connecté sur {serialData.COM}");
            }
            else
            {
                MessageBox.Show("Connexion échouée. Vérifiez le port et les paramètres d'interface.");
            }
        }

        /**
         * <summary>disconnect from the serial port</summary>
         */
        private void disconnect_serial()
         {
            _serial.CloseConnection();
         }

        /**
         * <summary>handle the behavior for when the launch programme button is pressed : launch the program and provide feedback of the head position.</summary>
         */
        private void buttonExecuteProgram_Click(object sender, EventArgs e)
        {
            if (_serial == null || !_serial.IsOpen())
            {
                MessageBox.Show("Connectez-vous d'abord à la machine via le menu de paramète.");
                return;
            }

            if (ExecutingSerialCommand)
            {
                MessageBox.Show("Veuillez patientez jusqu'a ce que la commande courante ce termine avant d'en executer une nouvelle.");
                return;
            }
            ExecutingSerialCommand = true;

            //TODO : remplacer par la vraie commande
            _serial.SendString("CMD_RUN");
            //TODO : traquer la tête de soudage
            
            ExecutingSerialCommand = false; //fini
           
        }

        /**
         * <summary>handle the behavior for when the recalibrate button is pressed : recalibrate the machine head, may give feedback on the head posistion.</summary>
         */
        private void RecalibrerLaMachineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //TODO découvrir quel signal on utilise pour damander un recalibrage
            if (_serial == null || !_serial.IsOpen())
            {
                MessageBox.Show("Connectez-vous d'abord à la machine.");
                return;
            }

            if (ExecutingSerialCommand)
            {
                MessageBox.Show("Veuillez patientez jusqu'a ce que la commande courante ce termine avant d'en executer une nouvelle.");
                return;
            }
            ExecutingSerialCommand = true;

            // TODO : remplacer "CMD_REF" par la vraie commande une fois connue
            // TODO : traquer la tête de soudage ?
            _serial.SendString("CMD_REF");

            ExecutingSerialCommand = false;//fini
        }

        /**
         * <summary>handle the behavior for when read loaded programme is pressed : reciev the program, display it and set the current parameters.</summary>
         */
        private void lireLeProgrammeEnMémoireToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //TODO découvrir le signal
            //recup le programme, le lis, affiche les goujons et met les paramètres dans current
            if (_serial == null || !_serial.IsOpen())
            {
                MessageBox.Show("Connectez-vous d'abord à la machine.");
                return;
            }

            if (ExecutingSerialCommand)
            {
                MessageBox.Show("Veuillez patientez jusqu'a ce que la commande courante ce termine avant d'en executer une nouvelle.");
                return;
            }
            ExecutingSerialCommand = true;

            //recupere le programme
            string[] lines = null;
            string message = "";
            Task task = Task.Run(() =>
            {
                // TODO : adapter la commande de lecture
                lines = _serial.SendAndReceiveLines("CMD_READ_PROGRAM", expectedLines: AREProdFileGenerator.AREProgramSize);

                if (lines == null || lines.Length == 0)
                {
                    message += "Aucune donnée reçue.";
                }
                else
                {
                    message += $"Programme reçu : {lines.Length} lignes.";//toujours 1000
                }
            });

            task.Wait();
            
            if(!String.IsNullOrEmpty(message))
            { 
                MessageBox.Show(message);
                return;
            }

            fs.ReadRecivedAREProgram(lines);
            //TODO : display & param

            ExecutingSerialCommand = false;//fini

        }

        /**
         * <summary>handle the behavior for when the tranfer program button is pressed : transfer the whole are program to the machine.</summary>
         */
        private void transmettreUnProgrammeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //TODO découvrir le signal
            //on envoie notre programme 
            if (_serial == null || !_serial.IsOpen())
            {
                MessageBox.Show("Connectez-vous d'abord à la machine.");
                return;
            }

            if (ExecutingSerialCommand)
            {
                MessageBox.Show("Veuillez patientez jusqu'a ce que la commande courante ce termine avant d'en executer une nouvelle.");
                return;
            }
            ExecutingSerialCommand = true;

            // TODO menu openfile dialog pour fichier are
            if (openFileDialogARETransfer.ShowDialog() != DialogResult.OK)
            {
                MessageBox.Show("Fichier are invalide.");
                return;
            }

            

            string arepath = openFileDialogARETransfer.FileName;

            string[] areContent = File.ReadAllLines(arepath);

            //En arrière-plan pour ne pas bloquer l'UI
            Task.Run(() =>
            {
                //TODO : trouver la commande de démarrage de transfert
                _serial.SendString("CMD_SEND_PROGRAM");
                

                //envoie du contenu ligne par ligne
                foreach (string line in areContent)
                {
                    bool ok = _serial.SendString(line);
                    if (!ok) break;
                }

                this.Invoke(new Action(() => MessageBox.Show("Transfert terminé."))); //pas dans le thread d'écriture

            });

            ExecutingSerialCommand = false;//fini
        }









        /*___________________________________________|___________________________________________*/

    }
}

