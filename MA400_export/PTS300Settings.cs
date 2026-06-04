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
    public partial class PTS300Settings : Form
    {
        public Form mainParent = null;

        public PTS300Settings()
        {
            InitializeComponent();
        }

        /**
         * <summary>use this constructor along with the settings & coments array so that it can populate the datagridview</summary>
         */
        public PTS300Settings( ref int[] settingsvalue, ref string[] comments)
        {
            InitializeComponent();

            for(int i = 0; i<100; i++)
            {
                this.dataGridView1.Rows.Add(settingsvalue[i], comments[i]);
            }


        }


        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var grid = sender as DataGridView;
            var rowIdx = (e.RowIndex + 1).ToString();

            var centerFormat = new StringFormat()
            {
                // right alignment might actually make more sense for numbers
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);
            e.Graphics.DrawString(rowIdx, this.Font, SystemBrushes.ControlText, headerBounds, centerFormat);
            
        }

        private void PTS300Settings_Load(object sender, EventArgs e)
        {

        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            //TODO: do some shit
            DialogResult = DialogResult.OK;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            //TODO save the settings to a file
        }

        private void buttonDefault_Click(object sender, EventArgs e)
        {
            //TODO reset the settings to default values
            MA400_export Form1 = this.mainParent as MA400_export;
            ref int[] defaultparameters =  ref Form1.fs.GetPTS300DefaultParameters();
            ref string[] defaultcomment = ref Form1.fs.GetPTS300DefaultComments();

            for (int i = 0; i < 100; i++)
            {
                dataGridView1.Rows[i].SetValues(new object[] { Form1.fs.GetPTS300Parameters()[i], Form1.fs.GetPTS300Comments()[i] });
            }
        }

        private void reset()
        {
            MA400_export Form1 = this.mainParent as MA400_export;
            for (int i = 0; i < 100; i++)
            {
                dataGridView1.Rows[i].SetValues(new object[] { Form1.fs.GetPTS300Parameters()[i], Form1.fs.GetPTS300Comments()[i] });
            }

        }
        private void buttonReset_Click(object sender, EventArgs e)
        {
            //TODO reset the settings to the values they had when the form was opened
            reset();
        }
    }

    
}
