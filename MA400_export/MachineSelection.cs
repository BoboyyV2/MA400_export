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
    public partial class MachineSelection : Form
    {
        private const string KTS850 = "KTS-850 CNC";
        private const string smallermachine = "Petite machine";

        private Machine machine = Machine.None;
        public MachineSelection()
        {
            

            InitializeComponent();
            this.comboBoxSelect.Items.Clear();
            this.comboBoxSelect.Items.AddRange(new object[] { KTS850, smallermachine } );
        }

        public void setMachine()
        {
            switch (this.comboBoxSelect.Text)
            {
                case KTS850:
                    machine = Machine.KTS850;
                    break;

                case smallermachine:
                    machine = Machine.Small;
                    break;

                default:
                    machine = Machine.None;
                    break;
            }
        }

        public Machine getMachine()
        {
            return machine;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            setMachine();
            if (getMachine() == Machine.None)
            {
                MessageBox.Show("La machine séléctionnée n'est pas valide");
                return;
            }

            DialogResult = DialogResult.OK;
        }
    }
}
