using ACadSharp.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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


        public float _Zoom = 1.4f;
        public FileSystem fs = new FileSystem();
        public GraphicsContainer gc;
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

            gc = new GraphicsContainer(e.Graphics);
            //Graphics graphics = e.Graphics;
            gc.graphics.ScaleTransform(_Zoom, _Zoom);



            gc.Paint(Studs);

        }

        public Decimal GetOffsetedCoords(float coord)
        {
            return Math.Round((Decimal)(coord / _Zoom - Constants.Origin_Coord.X), 0, MidpointRounding.AwayFromZero);
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
            XCoord_Display.Text = " X = " + GetOffsetedCoords(point.X) + " ";
            YCoord_Display.Text = " Y = " + GetOffsetedCoords(point.Y) + " ";
        }

        private void WorkZone_MouseMove(object sender, MouseEventArgs e)
        {
            UpdateCoords();
        }

        private void WorkZone_Zoom(object sender, MouseEventArgs e)
        {

            if (e.Delta > 0 && _Zoom < Constants.Max_Zoom)
            {
                if ( _Zoom * 1.1f > Constants.Max_Zoom)
                {
                    _Zoom = Constants.Max_Zoom;
                }
                else
                {
                    _Zoom *= 1.1f;
                }
            }
            else if ( _Zoom > Constants.Min_Zoom)
            {
                if ( _Zoom * 0.9f < Constants.Min_Zoom)
                {
                    _Zoom = Constants.Min_Zoom;
                }
                else
                {
                    _Zoom *= 0.9f;

                }

            }
            WorkZone.Refresh();
            UpdateCoords();

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
                try
                {
                    this.fs.OpenDxfFile(this.ouvrirToolStripMenuItem.Text.ToString());
                }
                catch (SecurityException ex)
                {
                    MessageBox.Show($"Security error.\n\nError message: {ex.Message}\n\n" +
                    $"Details:\n\n{ex.StackTrace}");
                }

                List<Circle> ToAdd =  this.fs.ScanStud();
                foreach (var circle in ToAdd) 
                {
                    AddStud(circle);
                }
                //TODO
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
                    if (!AddStudButtonOnClick_CheckInput((double)GetOffsetedCoords(p.X), (double)GetOffsetedCoords(p.Y), diam_String))
                    {
                        return;
                    }
                    double radius = (Double.Parse(diam_String))/2;
                    Circle circle = createStud((double)GetOffsetedCoords(p.X), (double)GetOffsetedCoords(p.Y), radius);
                    if (!IsPossibleToAddStud(circle) )
                    {
                        return;
                    }

                    AddStud(circle);
                    this.WorkZone.Refresh();
                    break;

                case EditMode.RemoveStud:
                    //TODO
                    System.Drawing.Point p_rm = getCoords();
                    p_rm.X = (int)GetOffsetedCoords(p_rm.X);
                    p_rm.Y = (int)GetOffsetedCoords(p_rm.Y);

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

