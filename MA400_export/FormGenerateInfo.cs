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
        DateTime Today ;
        DateTime Modified;

        bool IsNew;

        public FormGenerateInfo()
        {
            Today = DateTime.Now;
            Modified = default(DateTime);
            Data = new GeneratorData();
            IsNew = true;
            Data.ProgramNumber = GetProgramNumber();
            InitializeComponent();

            //prefill les textbox
            this.DateCreatedText.Text = Today.ToString("dd.MM.yyyy");
            this.ProgramNumberText.Text = "" + Data.ProgramNumber;
        }

        public FormGenerateInfo(GeneratorData data)
        {
            Today = DateTime.Now;
            Data = data;
            IsNew = false;

            InitializeComponent();

            //prefill les textbox
            this.CompanyText.Text = Data.Company;
            this.PartDesignationText.Text = Data.PartDesignation;
            this.PartNumberText.Text = Data.PartNumber;
            this.DrawingNumberText.Text = Data.DrawingNumber;
            this.NotesText.Text = Data.Notes;

            //static
            this.DateCreatedText.Text = Data.DateCreation;
            this.DateModifiedText.Text = Data.DateModification;
            this.ProgramNumberText.Text = "" + Data.ProgramNumber;

        }

        public int GetProgramNumber()
        {
            if(IsNew)
            {
                return Properties.Settings.Default.NewProgramNumber;
            }
            return Data.ProgramNumber;
        }
        public void ValidateProgram()
        {
            if (IsNew)
            {
                Properties.Settings.Default.NewProgramNumber++;
                Properties.Settings.Default.Save();
            }
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
                //fill the values
                Data.Company = this.CompanyText.Text.Trim();
                Data.PartNumber = this.PartNumberText.Text.Trim();
                Data.PartDesignation = this.PartDesignationText.Text.Trim();
                Data.DrawingNumber = this.DrawingNumberText.Text.Trim();
                Data.Notes = this.NotesText.Text.Trim();

                //prefilled values
                if (IsNew)
                {
                    Data.DateCreation = Today.ToString("dd.MM.yyyy");
                    Data.DateModification = "          "; //10 espaces
                }
                else
                {
                    Data.DateModification = Today.ToString("dd.MM.yyyy");
                }

                ValidateProgram();
                this.DialogResult = DialogResult.OK;
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
