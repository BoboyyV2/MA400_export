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
    public partial class getRotationValue : Form
    {
        public double Value {  get; private set; }
        public getRotationValue()
        {
            InitializeComponent();
            Value = 0;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            if (RetrieveValue() != -1)
            {
                DialogResult = DialogResult.OK;
            }
        }

        public int RetrieveValue()
        {
            string regexp_nb = @"^\-?[0-9]+(\.[0-9]+)?$";//nombre
            if(!System.Text.RegularExpressions.Regex.IsMatch(textBoxRotation.Text, regexp_nb))
            {
                MessageBox.Show("Veuillez entrer un nombre valide", "Entrée invalide", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
            double RotationValue;
            try
            {
                RotationValue = Convert.ToDouble(textBoxRotation.Text);
            }
            catch (Exception e)
            {
                MessageBox.Show("Veuillez entrer un nombre valide" + e.Message, "Entrée invalide", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }

            if ( (RotationValue < 0) || (RotationValue > 360) || !System.Text.RegularExpressions.Regex.IsMatch(textBoxRotation.Text, regexp_nb))
            {
                MessageBox.Show("Veuillez entrer un valeur entre 0 et 360°", "Entrée invalide", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
            else
            {
                Value = RotationValue;
                return 1;
            }
        }

    }
}
