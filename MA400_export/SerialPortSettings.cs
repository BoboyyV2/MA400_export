using Newtonsoft.Json;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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

            string[] portsnames = SerialPort.GetPortNames(); 
            foreach (string port in portsnames)
            {
                comboBoxActiveCOM.Items.Add(new { Text = port, Value = port });
            }

            data = new SerialData();
            displayDataValues();
        }

        public SerialPortSettings(SerialData data)
        {
            InitializeComponent();

            string[] portsnames = SerialPort.GetPortNames();
            foreach( string port in portsnames)
            {
                comboBoxActiveCOM.Items.Add(port);
            }

            this.data = data;
            displayDataValues();
        }

        private void displayDataValues()
        {
            comboBoxActiveCOM.Text = data.COM;

            comboBoxBaudRate.Text = data.BaudRate.ToString();

            comboBoxDataBits.Text = data.DataBits.ToString();

            comboBoxParity.Text = data.ParityBit.ToString();
            switch (data.ParityBit)
            {
                case Parity.None:
                    comboBoxParity.Text = "aucun";
                    break;
                case Parity.Odd:
                    comboBoxParity.Text = "impaire";
                    break;
                case Parity.Even:
                    comboBoxParity.Text = "paire";
                    break;

                default:
                    comboBoxParity.Text = "aucun";
                    break;
            }

            switch (data.StopBit)
            {
                case StopBits.None:
                    comboBoxStopBit.Text = "0";
                    break;
                case StopBits.One:
                    comboBoxStopBit.Text = "1";
                    break;
                case StopBits.Two:
                    comboBoxStopBit.Text = "2";
                    break;
                case StopBits.OnePointFive:
                    comboBoxStopBit.Text = "1,5";
                    break;
                default:
                    comboBoxStopBit.Text = "1";
                    break;
            }
        }

        /**
         * <summary>fill the value of the SerialData object with the user's given values in the form, if the values are correct, show an error message otherwise.</summary>
         */
        private bool fillDataValues()
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
                return false;
            }

            try
            {
                Databits = Convert.ToInt32(comboBoxDataBits.Text);
            }
            catch (Exception e)
            {
                MessageBox.Show("Le format de la valeur du databit est invalide, un entier est attendu. " + e.Message);
                return false;
            } 
            if (Databits > 8 || Databits < 5)
            {
                MessageBox.Show("Le nombre de bits de donnée doit être compris entre 5 et 8");
                return false;
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
                    return false;
            }

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
                    return false;
            }

            //set les valurs une fois qu'elle sont toutes validées
            data.DataBits = Databits;
            data.StopBit = StopBit;
            data.BaudRate = BaudRate;
            data.ParityBit = ParityBit;

            return true;


        }

        

        /**
         * <summary>handle the click bahavior for apply</summary>
         */
        private void buttonApply_Click(object sender, EventArgs e)
        {
            if (fillDataValues())
            {
                Util.writeToJson(data, Constants.MainPath + Constants.paramPath + "SerialData.json");
            }
        }

        /**
         * <summary>handle the click bahavior for OK</summary>
         */
        private void buttonOK_Click(object sender, EventArgs e)
        {
           
            if (fillDataValues())
            {
                Util.writeToJson(data, Constants.MainPath + Constants.paramPath + "SerialData.json");
                DialogResult = DialogResult.OK;
            }

        }



    }
}
