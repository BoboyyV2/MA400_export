using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MA400_export
{
    public partial class printForm : Form
    {
        private Bitmap bmp;
        public printForm(ref FileSystem fs)
        {
            InitializeComponent();
            StudListBox.DataSource = fs.Studs;
            StudListBox.ClearSelected();
        }

        public void setImage(Bitmap bmp)
        {
            this.bmp = (Bitmap)bmp.Clone();
            pictureBoxpreview.Image = this.bmp;
            StudListBox.ClearSelected();

        }

    }

    
}
