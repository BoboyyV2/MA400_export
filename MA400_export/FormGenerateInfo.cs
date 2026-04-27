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
    public partial class FormGenerateInfo : Form
    {

        public GeneratorData Data;
        int ProgramNumberValue = 1112;
        DateTime Today ;
        DateTime Modified; 
        public FormGenerateInfo()
        {
            Today = DateTime.Now;
            Modified = default(DateTime);
            Data = new GeneratorData();
            InitializeComponent();
        }

        public FormGenerateInfo(DateTime LastModified)
        {
            Today = DateTime.Now;
            Modified = LastModified;
            Data = new GeneratorData();
            InitializeComponent();
        }

        private void GetProgramNumber()
        {
            ProgramNumberValue = 1112;
            //TODO fichier de config ?
        }

        private bool CheckInput()
        {
            //message d'erreur eventuel
            return true;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if( CheckInput())
            {
                //filled values
                Data.Company = this.CompanyText.Text.Trim();
                Data.PartNumber = this.PartNumberText.Text.Trim();
                Data.PartDesignation = this.PartDesignationText.Text.Trim();
                Data.DrawingNumber = this.DrawingNumberText.Text.Trim();
                Data.Notes = this.NotesText.Text.Trim();

                //prefilled values
                Data.DateCreation = Today.ToString("dd.MM.yyyy");

                //configvalues
                Data.ProgramNumber = ProgramNumberValue;
                if(Modified == default(DateTime))
                {
                    Data.DateModification = "          "; //10 espaces
                }
                else
                {
                    Data.DateModification = Modified.ToString(); 
                }
                this.DialogResult = DialogResult.OK;
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
