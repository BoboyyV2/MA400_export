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

    /**
      * <summary>
      * The <c>MachineSelection</c> Form is used to selection the machine on which we are currently working.<br></br>
      * This way we are able to generate the right program format aswell as use the specific fonctionnality of each machine.
      * </summary>
      */
    public partial class MachineSelection : Form
    {
        private const string KTS850 = "KTS-850 CNC";
        private const string PTS300 = "PTS-300";

        private Machine machine = Machine.None;

        public MachineSelection()
        {
            

            InitializeComponent();
            this.comboBoxSelect.Items.Clear();
            this.comboBoxSelect.Items.AddRange(new object[] { KTS850, PTS300 } );
        }

        /**
         * <summary>Set the machine variable according to the selection of the user in the comboBox</summary>
         */
        public void setMachine()
        {
            switch (this.comboBoxSelect.Text)
            {
                case KTS850:
                    machine = Machine.KTS850;
                    break;

                case PTS300:
                    machine = Machine.PTS300;
                    break;

                default:
                    machine = Machine.None;
                    break;
            }
        }


        /**
         * <summary>Public getter for the machine variable</summary>
         */
        public Machine getMachine()
        {
            return machine;
        }

        /**
         * <summary>Handles the click event for the OK button, make sure the machine selected is valid</summary>
         */
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
