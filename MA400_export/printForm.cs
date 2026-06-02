using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace MA400_export
{
    public partial class printForm : Form
    {
        private Bitmap bmp;

        private Bitmap printBitmap;

        public printForm(ref FileSystem fs)
        {
            InitializeComponent();
            StudListBox.DataSource = fs.Studs;
            StudListBox.ClearSelected();
            this.ControlBox = false;
            this.Text = String.Empty;
        }

        public void setImage(Bitmap bmp)
        {
            this.bmp = (Bitmap)bmp.Clone();
            pictureBoxpreview.Image = this.bmp;
            StudListBox.ClearSelected();

        }

        public void Print()
        {
            printDialog.ShowDialog();
        }

        public void Preview()
        {
            //ouvre l'aperçu d'impression
            using (PrintPreviewDialog preview = new PrintPreviewDialog())
            {

                //set the formposition outside the bondaries
                //Location = new System.Drawing.Point(Constants.print_position, Constants.print_position);


                //Add a Panel control.
                Panel panel = new Panel();
                this.Controls.Add(panel);

                //Create a Bitmap of size same as that of the Form.
                Graphics grp = panel.CreateGraphics();
                Size formSize = this.ClientSize;
                printBitmap = new Bitmap(formSize.Width, formSize.Height, grp);
                grp = Graphics.FromImage(printBitmap);

                //Copy screen area that that the Panel covers.
                Point panelLocation = PointToScreen(panel.Location);
                grp.CopyFromScreen(panelLocation.X, panelLocation.Y, 0, 0, formSize);

                //Show the Print Preview Dialog.
                
                printPreviewDialog.Document = printDocument;
                printPreviewDialog.PrintPreviewControl.Zoom = 1;

                this.DialogResult = DialogResult.OK;
                printPreviewDialog.ShowDialog();
                Hide();



                //Hide();

            }
        }

        private void printDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(printBitmap, 0, 0);
        }

        private void PrintButton_Click(object sender, EventArgs e)
        {
            //Add a Panel control.
            Panel panel = new Panel();
            this.Controls.Add(panel);

            //Create a Bitmap of size same as that of the Form.
            Graphics grp = panel.CreateGraphics();
            Size formSize = this.ClientSize;
            printBitmap = new Bitmap(formSize.Width, formSize.Height, grp);
            grp = Graphics.FromImage(printBitmap);

            //Copy screen area that that the Panel covers.
            Point panelLocation = PointToScreen(panel.Location);
            grp.CopyFromScreen(panelLocation.X, panelLocation.Y, 0, 0, formSize);

            //Show the Print Preview Dialog.
            printPreviewDialog.Document = printDocument;
            printPreviewDialog.PrintPreviewControl.Zoom = 1;
            printPreviewDialog.ShowDialog();
        }

        private void PreviewButton_Click(object sender, EventArgs e)
        {
            //Add a Panel control.
            Panel panel = new Panel();
            this.Controls.Add(panel);

            //Create a Bitmap of size same as that of the Form.
            Graphics grp = panel.CreateGraphics();
            Size formSize = this.ClientSize;
            printBitmap = new Bitmap(formSize.Width, formSize.Height, grp);
            this.DrawToBitmap(printBitmap, new Rectangle(0, 0, this.Width, this.Height));

            grp = Graphics.FromImage(printBitmap);

            //Show the Print Preview Dialog.
            printDialog.Document = printDocument;
            printDialog.ShowDialog();
        }
    }
    

    
}
