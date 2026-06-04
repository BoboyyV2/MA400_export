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
    }

    
}
