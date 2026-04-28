using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MA400_export
{
    public partial class ProgramNumberOpen : Form
    {

        public int ProgramNumber = -1 ;
        public ProgramNumberOpen()
        {
            
            InitializeComponent();
        }

        /**
         * <summary>Set the ProgramNumber value using the textbox.<br></br>
         * If the input is incorrect, show an error message.</summary>
         * <returns>ture if a number was succesfully retrieved, false otherwise.</returns>
         */
        private bool getProgramNumber()
        {
            try
            {
                ProgramNumber = int.Parse(ProgramNumberText.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Numéro saisi non valide : " + ex.Message);
                return false;
            }
            if(ProgramNumber < 0)
            {
                MessageBox.Show("Numéro saisi non valide : " + "La valeur doit être positive");
                return false;
            }
            
            return true;
        }

        /**
         * <summary>Tells if a program number is valid by trying to find the associated production files</summary>
         * <returns>true if the number is valid, throw an exeption otherwise.</returns>
         */
        private bool IsValidProgNumber(int number)
        {
            string CNCPath = Properties.Settings.Default.OutputPath + @"Cnc\" + ProgramNumber + ".CNC";
            string GPHPath = Properties.Settings.Default.OutputPath + @"Daten\" + ProgramNumber + ".GPH";

            if (!File.Exists(CNCPath))
            {
                //le prog n'existe pas 
                throw new FileNotFoundException("Fichier CNC introuvable", ProgramNumber + ".CNC");
            }
            if (!File.Exists(GPHPath))
            {
                //le prog n'existe pas 
                throw new FileNotFoundException("Fichier GPH introuvable", ProgramNumber + ".GPH");
            }
            return true;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (!getProgramNumber())
            {
                return;
            }
            
            bool valid;
            string message = "";
            try
            {
                valid = IsValidProgNumber(ProgramNumber);
            }
            catch (FileNotFoundException ex)
            {
                valid = false;
                message = ex.Message + "(" + ex.FileName + ")";
            }

            if (valid)
            {
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show(message);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;

        }
    }
}
