using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace MA400_export
{

    public struct SerialData
    {
        public string COM;
        public int BaudRate;

        public int DataBits;
        public Parity ParityBit;
        public StopBits StopBit;
    }

    /**
      * <summary>
      * The <c>SerialPortSettings</c> Form is used to set and edit the settings relative to the serial port communication.
      * </summary>
      */
    public partial class SerialPortSettings : Form
    {
        public SerialData data;
        public SerialPortSettings()
        {
            InitializeComponent();
            data = new SerialData();
        }

        /**
         * <summary>check the settings input to make sure they are all valid, show an error otherwise.</summary>
         * <returns>true if all is correct, false otherwise.</returns>
         */
        private bool checkInput()
        {
            return true;
            //TODO un vrai check
        }

        /**
         * <summary>fill the value of the SerialData object with the user inputed values in the form.</summary>
         */
        private void fillDataValues()
        {
            Parity ParityBit;
            StopBits StopBit;
            int Databits;
            int BaudRate;
            string COM;

            data.COM = comboBoxActiveCOM.Text;
            //TODO gestion des COM actifs
            //détection, utilisation...

            try
            {
                BaudRate = Convert.ToInt32(comboBoxBaudRate.Text);
            }
            catch (Exception e)
            {
                MessageBox.Show("Le format de la valeur de la vitesse de transmission est invalide, un entier est attendu. " + e.Message);
                return;
            }

            try
            {
                Databits = Convert.ToInt32(comboBoxDataBits.Text);
            }
            catch (Exception e)
            {
                MessageBox.Show("Le format de la valeur du databit est invalide, un entier est attendu. " + e.Message);
                return;
            } 
            if (Databits > 8 || Databits < 5)
            {
                MessageBox.Show("Le nombre de bits de donnée doit être compris entre 5 et 8");
                return;
            }


            switch (comboBoxParity.Text)
            {
                case "aucun":
                    ParityBit = Parity.None;
                    break;
                case "pair":
                    ParityBit = Parity.Even;
                    break;
                case "impair":
                    ParityBit = Parity.Odd;
                    break;
                default:
                    MessageBox.Show("Veuillez choisir la valeur du bit de parité parmis celles proposées uniquement.");
                    return;
            }

            //data.StopBit = StopBits.
            switch (comboBoxStopBit.Text)
            {
                case "0":
                    StopBit = StopBits.None;
                    break;
                case "1":
                    StopBit = StopBits.One;
                    break;
                case "2":
                    StopBit = StopBits.Two;
                    break;
                case "1.5":
                case "1,5":
                    StopBit = StopBits.OnePointFive;
                    break;
                default:
                    MessageBox.Show("Veuillez choisir la valeur du bit de fin de trame parmis celles proposées uniquement.");
                    return;
            }

            //set les valurs une fois qu'elle sont toutes validées
            data.DataBits = Databits;
            data.StopBit = StopBit;
            data.BaudRate = BaudRate;
            data.ParityBit = ParityBit;


        }

        /**
         * <summary>handle the click bahavior for apply</summary>
         */
        private void buttonApply_Click(object sender, EventArgs e)
        {
            //do nothing if the values are not correct
            if (!checkInput())//s'occupe aussi des messages d'erreurs
            {
                return;
            }
            fillDataValues();
            
        }

        /**
         * <summary>handle the click bahavior for OK</summary>
         */
        private void buttonOK_Click(object sender, EventArgs e)
        {
            //do nothing if the values are not correct
            if (!checkInput())
            {
                return;
            }
            fillDataValues();
            DialogResult = DialogResult.OK;

        }
    }
}
