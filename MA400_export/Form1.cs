using ACadSharp;
using ACadSharp.Entities;
using ACadSharp.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TreeView;

namespace MA400_export
{
    

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

        //Studs informations
        public BindingList<Stud> Studs = new BindingList<Stud>();
        public int StudCurrentIndex = 0;

        private string savepath = string.Empty;
       
        //Edit variables
        public EditMode editMode = EditMode.Cursor;
        GeneratorData data = new GeneratorData();
        public bool IsNew = true;


        public MA400_export()
        {
            InitializeComponent();
            StudList_Display.DataSource = Studs;

        }

        private void reset()
        {
            data = new GeneratorData();
            IsNew = true;
            fs.reset();
            gc.reset();
        }

        public void EmptyStuds()
        {
            Studs.Clear();
        }
        /**
         * <summary>add a stud to the local collection</summary>
         */
        public void AddStud(Circle stud)
        {

            Studs.Add(new Stud(stud, StudCurrentIndex));

            StudCurrentIndex++;
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

        private void WorkZone_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            //draw the basic from of the workzone
            //including but not restricted to :
            //the background, the rectangular coordinate system, the scale
            //the workzone, the landmarks

            gc.graphics = e.Graphics;
            gc.graphics.ScaleTransform(_Zoom, _Zoom);
            gc.graphics.TranslateTransform(-Origin_Offset.X, -Origin_Offset.Y);

            gc.Paint(Studs, getSelectedStuds(), gc.layout.offset );

        }

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
            return new PointF(Origin_Offset.X - Constants.Origin_Coord.X + ( p.X  / _Zoom ), Origin_Offset.Y - Constants.Origin_Coord.Y + (p.Y  / _Zoom) );
        }
        

        /**
         *<returns>a point to the coordonate of the cursor in the pannel without adjusting it</returns>
         */
        public System.Drawing.Point getCoords()
        {
            return WorkZone.PointToClient(Cursor.Position);

        }

        public void UpdateCoords()
        {
            System.Drawing.Point point = WorkZone.PointToClient(Cursor.Position);

            CursorPosition = GetOffsetedCoords(point);
            XCoord_Display.Text = " X = " + (CursorPosition.X).ToString("0.0") + " ";
            YCoord_Display.Text = " Y = " + (CursorPosition.Y).ToString("0.0") + " ";

        }

        private void WorkZone_MouseMove(object sender, MouseEventArgs e)
        {
            UpdateCoords();
        }


        private void updateOrigin_Offset(PointF predicted_offset, float offsetX_min, float offsetX_max, float offsetY_min, float offsetY_max, float zoom_delta)
        {

            //offsetX
            if(predicted_offset.X < offsetX_min)
            {
                Origin_Offset.X = offsetX_min;

            }else if (predicted_offset.X > offsetX_max)
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

        private void WorkZone_Zoom(object sender, MouseEventArgs e)
        {
            //compute the zoom values
            float zoom_delta = (e.Delta > 0 ? zoomValue : -zoomValue);
            float new_Zoom = Math.Max(Math.Min(_Zoom + zoom_delta, Constants.Max_Zoom),Constants.Min_Zoom);

            //if needs be
            if (new_Zoom != _Zoom)
            {
                //update
                UpdateCoords();
                zoom_delta = new_Zoom - _Zoom;

                //check if we stay in bounds
                PointF predicted_offset = new PointF(Origin_Offset.X + CursorPosition.X / _Zoom * zoom_delta, Origin_Offset.Y + CursorPosition.Y / _Zoom * zoom_delta);
                float offsetX_min = -Constants.Origin_Coord.X - 141.6f;
                float offsetY_min = -Constants.Origin_Coord.X ;


                //depends on the zoom (can't see further away than 50 out of the worspace)
                float offsetX_max = Constants.WorkZoneLimits_Coord.X + Constants.Origin_Coord.X - (WorkZone.Width / new_Zoom) + 141.6f;
                float offsetY_max = Constants.WorkZoneLimits_Coord.Y + Constants.Origin_Coord.Y - (WorkZone.Height / new_Zoom);
                
                
                updateOrigin_Offset(predicted_offset, offsetX_min, offsetX_max, offsetY_min, offsetY_max, zoom_delta);
                
                _Zoom = new_Zoom;

                WorkZone.Invalidate();
                UpdateCoords();
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            if(Properties.Settings.Default.OutputPath.Length < 1)
            {
                Properties.Settings.Default.OutputPath = Constants.Outputpath;
                Properties.Settings.Default.Save();

            }
        }



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

                if ( ( StudList_Display.SelectedItems.Count == 1 ) && ( index < StudList_Display.Items.Count - 1 ) )
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
         * <summary>Tell if the input given for the addstudbutton is valid or not and display an error message if not.</summary>
         * <returns>true if everything is correct, false otherwise.</returns>
         */
        private bool AddStudButton_CheckInput(string X_string, string Y_string, string Diam_string)
        {
            string regexp_int = @"^\s*[0-9]+\s*$";//int

            string regexp_nb = @"^[0-9]+(\.[0-9]+)?$";//nombre
            string error = "";

            //correct form
            if (!System.Text.RegularExpressions.Regex.IsMatch(X_string, regexp_int))
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
            if (!System.Text.RegularExpressions.Regex.IsMatch(Y_string, regexp_int))
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
            Int32 X = Int32.Parse(X_string);
            Int32 Y = Int32.Parse(Y_string);
            int D = Int32.Parse(Diam_string);

            //Bounds
            //TODO remplacer la zone de travail par la pièce en elle même, aussi mettre une sécu si il n'y a pas de pièce
            if (X > ( Constants.WorkZoneLimits_Coord.X - Constants.Origin_Coord.X ) || X < 0)
            {
                error += "La coordonée X saisie n'est pas un emplacement valide pour poser un goujon.\r\n";
            }
            if (Y > (Constants.WorkZoneLimits_Coord.Y - Constants.Origin_Coord.Y ) || Y < 0)
            {
                error += "La coordonée Y saisie n'est pas un emplacement valide pour poser un goujon.\r\n";
            }

            if (D != 3 && D != 4)
            {
                error += "Le diamètre du goujon saisi est invalide.\r\n";
            }


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

            //Bounds
            //TODO remplacer la zone de travail par la pièce en elle même, aussi mettre une sécu si il n'y a pas de pièce

            if (X > (Constants.WorkZoneLimits_Coord.X - Constants.Origin_Coord.X) || X < 0)
            {
                error += "La coordonée X n'est pas un emplacement valide pour poser un goujon.\r\n";
            }
            if (Y > (Constants.WorkZoneLimits_Coord.Y - Constants.Origin_Coord.Y) || Y < 0)
            {
                error += "La coordonée Y saisie n'est pas un emplacement valide pour poser un goujon.\r\n";
            }

            if (D != 3 && D != 4)
            {
                error += "Le diamètre du goujon saisi est invalide.\r\n";
            }


            if (error.Length > 0)
            {
                MessageBox.Show(error);
                return false;
            }

            return true;
        }


        private Circle createStud(double x, double y, double radius)
        {
            Circle stud = new Circle();
            stud.Radius = radius;
            stud.Center = new CSMath.XYZ(x, y, 0);
            return stud;
        }

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



            int x = Int32.Parse(X_string);
            int y = Int32.Parse(Y_string);
            int diam = Int32.Parse(Diam_string);
            Circle Stud = createStud((double)x, (double)y, ((double)diam) / 2);



            if (!Util.IsPossibleToAddStud(Stud, Studs))
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

        private void buttonRemoveStud_Click(object sender, EventArgs e)
        {


            // Shutdown the painting of the ListBox as items are removed.
            StudList_Display.BeginUpdate();

            
            //start by getting the data
            List<Stud> selectedList = getSelectedStuds();

            foreach (Stud selected in selectedList)
            {
                Studs.Remove(selected);
            }


            StudList_Display.ClearSelected();

            StudList_Display.EndUpdate();

            //refresh the graphics
            WorkZone.Invalidate();

        }

        /**
         * <summary>Show a fileopen menu made to open dxf files</summary>
         */
        private void ouvrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialogOpen.ShowDialog();
            
        }

        /**
         * <summary>Open a dxf file and import it once it has been correctly selected via a fileopen menu</summary>
         */
        private void openFileDialogOpen_FileOk(object sender, CancelEventArgs e)
        {
            reset();
            bool open = false;
            try
            {
                open = this.fs.OpenDxfFile(this.openFileDialogOpen.FileName);
            }
            catch (SecurityException ex)
            {
                MessageBox.Show($"Security error.\r\nError message: {ex.Message}\r\n" +
                $"Details:\r\n{ex.StackTrace}");
            }
            gc.OpenCanvas();
            fs.OpenDxfFileLayout(gc.layout);

            DisplayWhenOpen(open);
        }

        public void setEditMode(EditMode mode)
        {
            editMode = mode;
        }

        private void ButtonCursorMode_Click(object sender, EventArgs e)
        {
            setEditMode(EditMode.Cursor);
        }

        private void ButtonSelectStudMode_Click(object sender, EventArgs e)
        {
            setEditMode(EditMode.SelectStud);
        }

        private void ButtonAddStudMode_Click(object sender, EventArgs e)
        {
            setEditMode(EditMode.AddStud);
        }

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

            foreach (Stud stud in Studs)
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
         * <summary>
         * Handle the behavior when clicking somwhere on the WorkZone using the add mode.<br></br>
         * Add a stud at the clicked coordinates if possible, show an error if not.
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
            if (!Util.IsPossibleToAddStud(circle, Studs))
            {
                return;
            }

            AddStud(circle);
            this.WorkZone.Refresh();
        }


        /**
         * <summary>
         * Handle the behavior when clicking somwhere on the WorkZone using the remove mode.<br></br>
         * Remove the clicked stud if there is one, show a message if no stud were found.
         * </summary>
         */
        private void WorkZone_Click_RemoveStud()
        {
            PointF p_rm = getCoords();
            p_rm = GetOffsetedCoords(p_rm);

            bool removed = false;
            foreach (Stud stud in Studs)
            {
                if (Util.getStudDistance(p_rm, stud.circle) < (stud.circle.Radius))
                {
                    Studs.Remove(stud);
                    removed = true;
                    break;
                }

            }
            if (!removed)
            {
                MessageBox.Show("aucun goujon trouvé à cette position.");
            }
            this.WorkZone.Refresh();
        }


        /**
         * <summary>Handle the behavior when clicking somwhere on the WorkZone depending on the mode</summary>
         */
        private void WorkZone_Click(object sender, System.Windows.Forms.MouseEventArgs e)
        {
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
        }

       

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
         * <summary>Attempt to generate the files for the MA400 driver to function.</summary>
         */
        private void GenerateOutput()
        {
            if (!fs.open)
            {
                MessageBox.Show("impossible de générer les fichiers de production :\r\nAucun fichier dxf ou programme n'est ouvert.");
                return;
            }

            GetFormData();
            
            
            fs.GenerateProdFiles(ref Studs, gc.layout.dimension, gc.layout.offset, data, gc.layout.scale); // en dernier, une fois que tout est bien rempli
        }


        private void buttonGenerer_Click(object sender, EventArgs e)
        {
            GenerateOutput();
        }

        private void générerLesFichiersDeSortieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GenerateOutput();
        }

        private void enregistrerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (savepath.Length == 0)
            {
                //pas encore de sauvegarde, on ouvre un menu
                saveFileDialogSave.ShowDialog();
                return;
            }
            fs.SaveToFile(Studs, savepath);

        }

        private void enregistrersousToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialogSave.ShowDialog();
        }

        private void saveFileDialogSave_FileOk(object sender, CancelEventArgs e)
        {
            savepath = saveFileDialogSave.FileName;
            fs.SaveToFile(Studs, savepath);
        }



        private void quitterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /**
         * <summary>Open a program via an existing program number</summary>
         */
        private void ouvrirprogramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (ProgramNumberOpen programNumberWindow = new ProgramNumberOpen())
            {
                if (programNumberWindow.ShowDialog() == DialogResult.OK)
                {
                    int ProgramNumber = programNumberWindow.ProgramNumber;
                    reset();
                    data = fs.OpenProdFile(ProgramNumber);
                    
                    IsNew = false;
                    
                    gc.OpenCanvas();
                    fs.OpenProdFileLayout(gc.layout);

                    DisplayWhenOpen(true);
                }

            }

        }

        private void DisplayWhenOpen(bool open)
        {
            if (!open)
            {
                gc.reset();
            }

            List<Circle> ToAdd = this.fs.Studs;
            EmptyStuds();
            foreach (var circle in ToAdd)
            {
                AddStud(circle);
            }
            

            this.WorkZone.Invalidate();

            //TODO
            //s'assurer des dimentions
            //on va ouvrir le ficheir ici
            //donc on aura note FileSystem qui va l'ouvrir et faire des bétises avec
            //puis ce sera le display qui va ensuite l'afficher, en plus de ce que le Filesystem va détecter comme goujon et donc qu'il va falloir highlight
        }

        private void StudList_Display_SelectedIndexChanged(object sender, EventArgs e)
        {
            //refresh the graphics
            WorkZone.Invalidate();
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //ouvre les options
            using ( UserSettings settingsWindow = new UserSettings())
            {
                settingsWindow.ShowDialog();
                //c'est tout
            }
        }

        private void rotateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            fs.RotatePart180();
            gc.OpenCanvas();
            fs.OpenDxfFileLayout(gc.layout);
            DisplayWhenOpen(true);



        }



        /*___________________________________________|___________________________________________*/
    }
}

