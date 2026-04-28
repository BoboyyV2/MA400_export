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
    }
}
