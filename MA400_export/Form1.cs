using ACadSharp;
using ACadSharp.Entities;
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
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TreeView;

namespace MA400_export
{
    

    public partial class MA400_export : Form
    {

        //used variables


        public float _Zoom = 1.0f;
        float zoomValue = 0.2f;

        public PointF Origin_Offset = new PointF(0f, 0f);
        public PointF CursorPosition = new PointF(0f, 0f);

        public FileSystem fs = new FileSystem();
        public GraphicsContainer gc = new GraphicsContainer();

        public BindingList<Stud> Studs = new BindingList<Stud>();
        public int StudCurrentIndex = 0;


       

        public EditMode editMode = EditMode.Cursor;

        public MA400_export()
        {
            InitializeComponent();
            StudList_Display.DataSource = Studs;

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

            gc.Paint(Studs);

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
            //XCoord_Display.Text = " X = " + point.X + " ";
            //YCoord_Display.Text = " Y = " + point.Y + " ";
            XCoord_Display.Text = " X = " + CursorPosition.X + " ";
            YCoord_Display.Text = " Y = " + CursorPosition.Y + " ";

            //debug
            /*
            this.origin_offset_label.Text = "offset : X = "+Origin_Offset.X+" Y = "+Origin_Offset.Y;
            this.cursorPositionLabel.Text = "cursor position on panel : X = " + point.X + " Y = " + point.Y;
            */

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


                //depends on the zoom (can't see further away than 50 ou of the worspace)
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

        }



        /**
         * <summary>let you select the previous stud if there is one</summary>
         */
        private void buttonPreviousStud_Click(object sender, EventArgs e)
        {
            if (StudList_Display.Items.Count > 0)
            {
                if (StudList_Display.SelectedIndex > 0)
                {
                    StudList_Display.SelectedIndex = StudList_Display.SelectedIndex - 1;
                }
                else
                {
                    StudList_Display.SelectedIndex = StudList_Display.Items.Count - 1;
                }
            }
            StudList_Display.Focus();
        }

        /**
         * <summary>let you select the next stud if there is one</summary>
         */
        private void buttonNextStud_Click(object sender, EventArgs e)
        {
            if (StudList_Display.Items.Count > 0) 
            { 
                if (StudList_Display.SelectedIndex < StudList_Display.Items.Count - 1)
                {
                    StudList_Display.SelectedIndex = StudList_Display.SelectedIndex + 1;
                }
                else
                {
                    StudList_Display.SelectedIndex = 0;
                }

            }
            StudList_Display.Focus();
        }



        /**
         * <summary>Tell if the input given for the addstudbutton is valid or not and display an error message if not.</summary>
         * <returns>true if everything is correct, false otherwise.</returns>
         */
        private bool AddStudButton_CheckInput(string X_string, string Y_string, string Diam_string)
        {

            string regexp = "^[0-9]+$";
            string error = "";

            //correct form
            if (!System.Text.RegularExpressions.Regex.IsMatch(X_string, regexp))
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
            if (!System.Text.RegularExpressions.Regex.IsMatch(Y_string, regexp))
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
            if (!System.Text.RegularExpressions.Regex.IsMatch(Diam_string, regexp))
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
            int X = Int32.Parse(X_string);
            int Y = Int32.Parse(Y_string);
            int D = Int32.Parse(Diam_string);

            //Bounds
            //TODO remplacer la zone de travail par la pièce en elle même, aussi mettre une sécu si il n'y a pas de pièce
            if (X > Constants.WorkZoneLimits_Coord.X || X < 0)
            {
                error += "La coordonée X saisie n'est pas un emplacement valide pour poser un goujon.\r\n";
            }
            if (Y > Constants.WorkZoneLimits_Coord.X || Y < 0)
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

            string regexp = "[0-9]+";
            string error = "";

            //correct form
            if (!System.Text.RegularExpressions.Regex.IsMatch(Diam_string, regexp))
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

            if (X > Constants.WorkZoneLimits_Coord.X || X < 0)
            {
                error += "La coordonée X n'est pas un emplacement valide pour poser un goujon.\r\n";
            }
            if (Y > Constants.WorkZoneLimits_Coord.X || Y < 0)
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



            if (!this.IsPossibleToAddStud(Stud))
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

            WorkZone.Refresh();

        }

        private void buttonRemoveStud_Click(object sender, EventArgs e)
        {


            // Shutdown the painting of the ListBox as items are removed.
            StudList_Display.BeginUpdate();

            
            //start by getting the data
            Stud selected = StudList_Display.SelectedItem as Stud;
            if (selected != null)
            {
                //TODO multiselect
                //List<Stud> selected = (StudList_Display.SelectedItems).toList(Studs);

                //remove from the display is automatcally done by the linked datasource
                Studs.Remove(selected);
                StudList_Display.ClearSelected();


                // Allow the ListBox to repaint and display the remaining items.
            }
            StudList_Display.EndUpdate();

            //refresh l'affichage
            WorkZone.Refresh();

        }

        private void ouvrirToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                bool open = false;
                try
                {
                    open = this.fs.OpenDxfFile(this.openFileDialog1.FileName);
                }
                catch (SecurityException ex)
                {
                    MessageBox.Show($"Security error.\n\nError message: {ex.Message}\n\n" +
                    $"Details:\n\n{ex.StackTrace}");
                }

                if (open)
                {
                    gc.OpenSVG();
                }
                else
                {
                    gc.CloseSVG();
                }

                List<Circle> ToAdd =  this.fs.Studs;
                foreach (var circle in ToAdd) 
                {
                    AddStud(circle);
                }

                this.WorkZone.Invalidate();
                //TODO
                //s'assurer des dimentions
                //on va ouvrir le ficheir ici
                //donc on aura note FileSystem qui va l'ouvrir et faire des bétises avec
                //puis ce sera le display qui va ensuite l'afficher, en plus de ce que le Filesystem va détecter comme goujon et donc q'uil va falloir highlight
            }
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        public void setEditMode(EditMode mode)
        {
            switch (mode)
            {
                case (EditMode.Cursor):
                    editMode = EditMode.Cursor;
                    break;
                case (EditMode.AddStud):
                    editMode = EditMode.AddStud;
                    break;
                case (EditMode.RemoveStud):
                    editMode = EditMode.RemoveStud;
                    break;
            }
        }

        private void ButtonCursorMode_Click(object sender, EventArgs e)
        {
            setEditMode(EditMode.Cursor);
        }

        private void ButtonAddStudMode_Click(object sender, EventArgs e)
        {
            setEditMode(EditMode.AddStud);
        }

        private void ButtonremoveStudMode_Click(object sender, EventArgs e)
        {
            setEditMode(EditMode.RemoveStud);
        }

        private void WorkZone_Click(object sender, EventArgs e)
        {
            switch (editMode)
            {
                
                case EditMode.Cursor:
                    //nothing
                    break;

                case EditMode.AddStud:
                    
                    System.Drawing.Point p = getCoords();
                    string diam_String = this.comboBoxDiam.Text;
                    PointF offseted_p = GetOffsetedCoords(p);
                    if (!AddStudButtonOnClick_CheckInput((double)offseted_p.X, (double)offseted_p.Y, diam_String))
                    {
                        return;
                    }
                    double radius = (Double.Parse(diam_String))/2;
                    Circle circle = createStud((double)offseted_p.X, (double)offseted_p.Y, radius);
                    if (!IsPossibleToAddStud(circle) )
                    {
                        return;
                    }

                    AddStud(circle);
                    this.WorkZone.Refresh();
                    break;

                case EditMode.RemoveStud:
                    PointF p_rm = getCoords();
                    p_rm = GetOffsetedCoords(p_rm);

                    bool removed = false;
                    foreach (Stud stud in Studs)
                    {
                        if (getStudDistance(p_rm, stud.circle) < (stud.circle.Radius))
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
                    break;
            }
        }

       
    }
}

