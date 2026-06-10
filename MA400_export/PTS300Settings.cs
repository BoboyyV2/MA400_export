using ACadSharp.Entities;
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
        private int[] backupParam = new int[AREProdFileGenerator.AREParameterSize];
        private string[] backupComms = new string[AREProdFileGenerator.AREParameterSize];
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

            for(int i = 0; i< AREProdFileGenerator.AREParameterSize; i++)
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
            MA400_export Form1 = this.mainParent as MA400_export;
            Form1.fs.GetPTS300CurrentParameters().CopyTo(backupParam, 0);
            Form1.fs.GetPTS300CurrentComments().CopyTo(backupComms, 0);

        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            MA400_export Form1 = this.mainParent as MA400_export;
            backupParam.CopyTo(Form1.fs.GetPTS300CurrentParameters(), 0);
            backupComms.CopyTo(Form1.fs.GetPTS300CurrentComments(), 0);
            DialogResult = DialogResult.Cancel;
        }

        private bool ValidateSettingsFormat()
        {

            string[] values = new string[AREProdFileGenerator.AREParameterSize];
            int numline = 0;
            //get all values
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                // Skip the new row placeholder
                if (!row.IsNewRow)
                {
                    try 
                    {
                        values[numline++] = (row.Cells[0].Value.ToString());
                    }catch(Exception e)
                    {
                        MessageBox.Show("Erreur lors de la verification des paramètres, " + e.Message);
                        return false;
                    }
                }
                
            }
            if(numline != AREProdFileGenerator.AREParameterSize)
            {
                MessageBox.Show("Nombre de paramètres récuperés invalide.");
                return false;
            }

            for (int i = 0 ; i < numline ; i++ )
            { 
                try
                {
                    Convert.ToInt32(values[i]);
                }
                catch (Exception e)
                {
                    MessageBox.Show("Valeur de paramètre invalide à la ligne " + (i + 1) +". " + e.Message);
                    return false;
                }
            }
            return true;
        }

        private bool SaveSettingsValues(ref int[]CurrentSettings, ref string[]CurrentComments)
        {
            string[] Comments = new string[AREProdFileGenerator.AREParameterSize];
            int[] Settings = new int[AREProdFileGenerator.AREParameterSize];

            int numline = 0;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                //fill the values
                if (!row.IsNewRow)
                {
                    try
                    {
                        Settings[numline] = Convert.ToInt32(row.Cells[0].Value.ToString());
                        string comment = "";
                        if (row.Cells[1].Value != null)
                        {
                            comment = row.Cells[1].Value.ToString();
                        }   
                        Comments[numline] = comment;
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Erreur lors de la sauvegarde, " + e.Message);
                        return false;
                    }
                    numline++;
                }

            }

            //fill values
            Array.Copy(Settings, CurrentSettings, Settings.Length);
            Array.Copy(Comments, CurrentComments, Comments.Length);

            return true;
        }

        

        private void buttonOK_Click(object sender, EventArgs e)
        {
            //make sure the values are of the correct format
            string[] values;
            if (ValidateSettingsFormat())
            {
                //save the values
                MA400_export Form1 = this.mainParent as MA400_export;
                ref int[] Savedparameters = ref Form1.fs.GetPTS300CurrentParameters();
                ref string[] Savedcomment = ref Form1.fs.GetPTS300CurrentComments();
                if (SaveSettingsValues(ref Savedparameters, ref Savedcomment))
                {
                    DialogResult = DialogResult.OK;
                }
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            //make sure the values are of the correct format
            string[] values;
            if (ValidateSettingsFormat())
            {
                //save the values
                MA400_export Form1 = this.mainParent as MA400_export;
                ref int[] Savedparameters = ref Form1.fs.GetPTS300CurrentParameters();
                ref string[] Savedcomment = ref Form1.fs.GetPTS300CurrentComments();
                if (SaveSettingsValues(ref Savedparameters, ref Savedcomment))
                {
                    Form1.fs.SavePTS300Settings();
                }
            }

        }


        /**
         * <summary>Reset the current settings to the default values</summary>
         */
        private void buttonDefault_Click(object sender, EventArgs e)
        {
            MA400_export Form1 = this.mainParent as MA400_export;
            Form1.fs.ResetPTS300ParamToDefault();
            ref int[] defaultparameters =  ref Form1.fs.GetPTS300CurrentParameters();
            ref string[] defaultcomment = ref Form1.fs.GetPTS300CurrentComments();

            for (int i = 0; i < AREProdFileGenerator.AREParameterSize; i++)
            {
                dataGridView1.Rows[i].SetValues(new object[] { defaultparameters[i], defaultcomment[i] });
            }
        }



        /**
         * <summary>Reset the settings to the values they had when the form was last saved</summary>
         */
        private void buttonReset_Click(object sender, EventArgs e)
        {
            MA400_export Form1 = this.mainParent as MA400_export;
            ref int[] Savedparameters = ref Form1.fs.GetPTS300SavedParameters();
            ref string[] Savedcomment = ref Form1.fs.GetPTS300SavedComments();
            for (int i = 0; i < 100; i++)
            {
                dataGridView1.Rows[i].SetValues(new object[] { Savedparameters[i], Savedcomment[i] });
            }
        }
    }

    
}
