using System;
using System.Windows.Forms;

namespace MA400_export
{
    /**
    * The <c>FormGenerateInfo</c> class is used to retrieve various information on the part & program from the user in order to generate the program files
    */
    public partial class FormGenerateInfo : Form
    {

        public GeneratorData Data;
        DateTime Today;
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

        /**
         * <returns>the current program number to generate to.</returns>
         * <remarks>It is either the number of the already imported program if there is one<br></br>
         * or the current value of the NewProgramNumber setting variable.</remarks>
         */
        public int GetProgramNumber()
        {
            if (IsNew)
            {
                return Properties.Settings.Default.NewProgramNumber;
            }
            return Data.ProgramNumber;
        }

        /**
         * <summary>Do the necessary change once a generation is validated</summary>
         */
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


        /**
         * <summary>fill the Data information with informations given in the form's textbox and validate the form, if an existing program was imported, overwrite it, create a new one otherwise.</summary>
         */
        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (CheckInput())
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

        /**
         * <summary>Cancel the form without saving</summary>
         */
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        /**
         * <summary>force a new program number, even if an existing program was imported making sure not to overwrite existing program.</summary>
         */
        private void buttonNewProgNumber_Click(object sender, EventArgs e)
        {


            //prefilled values
            if (!IsNew)
            {
                IsNew = true;

                //since IsNew is now set, it will return a new program number
                Data.ProgramNumber = GetProgramNumber();

                //and update the 
                Data.DateCreation = Today.ToString("dd.MM.yyyy");
                Data.DateModification = "          "; //10 espaces


                this.DateCreatedText.Text = Data.DateCreation;
                this.DateModifiedText.Text = Data.DateModification;

                this.ProgramNumberText.Text = "" + Data.ProgramNumber;

            }

        }
    }
}
