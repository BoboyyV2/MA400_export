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
    public partial class UserSettings : Form
    {

        string outputPath;
        int programNumber;
        public UserSettings()
        {
            programNumber = Properties.Settings.Default.NewProgramNumber;
            outputPath = Properties.Settings.Default.OutputPath;
            InitializeComponent();
            textBoxOutputPath.Text = outputPath;
            textBoxProgramNumber.Text = "" + programNumber;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }


        private void buttonChoseOutputPath_Click(object sender, EventArgs e)
        {
            
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {

                
                dialog.SelectedPath = Properties.Settings.Default.OutputPath;
                
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    outputPath = dialog.SelectedPath;
                    textBoxOutputPath.Text = outputPath;
                }
            }
        }

        private bool applyChange()
        {
            //verif num
            //verif path

            //si l'un des deux échoue
            if ( !applyOutputPathChange() || !applyProgramNumberChange())
            {
                return false;
            }
            return true;
        }
        private bool applyOutputPathChange()
        {
            //verif path
            if (Directory.Exists(textBoxOutputPath.Text))
            {
                Properties.Settings.Default.OutputPath = textBoxOutputPath.Text;//le text car on peut le modifier sans repercuter sur le variable
                Properties.Settings.Default.Save();
                return true;
            }
            else
            {
                MessageBox.Show("Dossier de sortie invalide.");
                return false;
            }
        }

        private bool applyProgramNumberChange()
        {
            //verif num
            try 
            {
                programNumber = Int32.Parse(textBoxProgramNumber.Text);
            }
            catch(Exception ex) 
            {
                    MessageBox.Show("Numéro de programme saisi invalide : " + ex.Message);
                    return false;
            }
            if(programNumber < 1)
            {
                MessageBox.Show("Numéro de programme saisi invalide : " + "la valeur saisie doit être positive.");
            }
            Properties.Settings.Default.NewProgramNumber = programNumber;
            Properties.Settings.Default.Save();
            return true;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (applyChange())
            {
                this.DialogResult = DialogResult.OK;
            }
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            applyChange();
        }
    }
}
