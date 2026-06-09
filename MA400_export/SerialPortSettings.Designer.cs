namespace MA400_export
{
    partial class SerialPortSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ActiveCom = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.TransmissionFormat = new System.Windows.Forms.Label();
            this.comboBoxActiveCOM = new System.Windows.Forms.ComboBox();
            this.StopBitLabel = new System.Windows.Forms.Label();
            this.ParityLabel = new System.Windows.Forms.Label();
            this.DataBitsLabel = new System.Windows.Forms.Label();
            this.comboBoxStopBit = new System.Windows.Forms.ComboBox();
            this.comboBoxParity = new System.Windows.Forms.ComboBox();
            this.comboBoxDataBits = new System.Windows.Forms.ComboBox();
            this.comboBoxBaudRate = new System.Windows.Forms.ComboBox();
            this.buttonApply = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.UpdateIntervalLabel = new System.Windows.Forms.Label();
            this.numericUpDownUpdateInterval = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownUpdateInterval)).BeginInit();
            this.SuspendLayout();
            // 
            // ActiveCom
            // 
            this.ActiveCom.AutoSize = true;
            this.ActiveCom.Location = new System.Drawing.Point(35, 25);
            this.ActiveCom.Name = "ActiveCom";
            this.ActiveCom.Size = new System.Drawing.Size(51, 13);
            this.ActiveCom.TabIndex = 0;
            this.ActiveCom.Text = "Port cible";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(35, 135);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(116, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Vitesse de transmission";
            // 
            // TransmissionFormat
            // 
            this.TransmissionFormat.AutoSize = true;
            this.TransmissionFormat.Location = new System.Drawing.Point(339, 25);
            this.TransmissionFormat.Name = "TransmissionFormat";
            this.TransmissionFormat.Size = new System.Drawing.Size(114, 13);
            this.TransmissionFormat.TabIndex = 4;
            this.TransmissionFormat.Text = "Format de transmission";
            // 
            // comboBoxActiveCOM
            // 
            this.comboBoxActiveCOM.FormattingEnabled = true;
            this.comboBoxActiveCOM.Location = new System.Drawing.Point(72, 41);
            this.comboBoxActiveCOM.Name = "comboBoxActiveCOM";
            this.comboBoxActiveCOM.Size = new System.Drawing.Size(99, 21);
            this.comboBoxActiveCOM.TabIndex = 5;
            // 
            // StopBitLabel
            // 
            this.StopBitLabel.AutoSize = true;
            this.StopBitLabel.Location = new System.Drawing.Point(289, 125);
            this.StopBitLabel.Name = "StopBitLabel";
            this.StopBitLabel.Size = new System.Drawing.Size(91, 13);
            this.StopBitLabel.TabIndex = 6;
            this.StopBitLabel.Text = "bit de fin de trame";
            // 
            // ParityLabel
            // 
            this.ParityLabel.AutoSize = true;
            this.ParityLabel.Location = new System.Drawing.Point(317, 90);
            this.ParityLabel.Name = "ParityLabel";
            this.ParityLabel.Size = new System.Drawing.Size(63, 13);
            this.ParityLabel.TabIndex = 7;
            this.ParityLabel.Text = "Bit de parité";
            // 
            // DataBitsLabel
            // 
            this.DataBitsLabel.AutoSize = true;
            this.DataBitsLabel.Location = new System.Drawing.Point(297, 55);
            this.DataBitsLabel.Name = "DataBitsLabel";
            this.DataBitsLabel.Size = new System.Drawing.Size(83, 13);
            this.DataBitsLabel.TabIndex = 8;
            this.DataBitsLabel.Text = "Bits de données";
            // 
            // comboBoxStopBit
            // 
            this.comboBoxStopBit.FormattingEnabled = true;
            this.comboBoxStopBit.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "1,5"});
            this.comboBoxStopBit.Location = new System.Drawing.Point(386, 122);
            this.comboBoxStopBit.Name = "comboBoxStopBit";
            this.comboBoxStopBit.Size = new System.Drawing.Size(99, 21);
            this.comboBoxStopBit.TabIndex = 9;
            // 
            // comboBoxParity
            // 
            this.comboBoxParity.FormattingEnabled = true;
            this.comboBoxParity.Items.AddRange(new object[] {
            "aucun",
            "pair",
            "impair"});
            this.comboBoxParity.Location = new System.Drawing.Point(386, 87);
            this.comboBoxParity.Name = "comboBoxParity";
            this.comboBoxParity.Size = new System.Drawing.Size(99, 21);
            this.comboBoxParity.TabIndex = 10;
            // 
            // comboBoxDataBits
            // 
            this.comboBoxDataBits.FormattingEnabled = true;
            this.comboBoxDataBits.Items.AddRange(new object[] {
            "8",
            "7",
            "6",
            "5"});
            this.comboBoxDataBits.Location = new System.Drawing.Point(386, 52);
            this.comboBoxDataBits.Name = "comboBoxDataBits";
            this.comboBoxDataBits.Size = new System.Drawing.Size(99, 21);
            this.comboBoxDataBits.TabIndex = 11;
            // 
            // comboBoxBaudRate
            // 
            this.comboBoxBaudRate.FormattingEnabled = true;
            this.comboBoxBaudRate.Items.AddRange(new object[] {
            "9600",
            "19200",
            "38400",
            "57600",
            "115200",
            "230400"});
            this.comboBoxBaudRate.Location = new System.Drawing.Point(72, 151);
            this.comboBoxBaudRate.Name = "comboBoxBaudRate";
            this.comboBoxBaudRate.Size = new System.Drawing.Size(99, 21);
            this.comboBoxBaudRate.TabIndex = 12;
            // 
            // buttonApply
            // 
            this.buttonApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonApply.Location = new System.Drawing.Point(266, 197);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(124, 27);
            this.buttonApply.TabIndex = 21;
            this.buttonApply.Text = "&Appliquer";
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(396, 197);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(101, 27);
            this.buttonCancel.TabIndex = 20;
            this.buttonCancel.Text = "&Annuler";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.Location = new System.Drawing.Point(159, 197);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(101, 27);
            this.buttonOK.TabIndex = 19;
            this.buttonOK.Text = "&OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // UpdateIntervalLabel
            // 
            this.UpdateIntervalLabel.AutoSize = true;
            this.UpdateIntervalLabel.Location = new System.Drawing.Point(35, 80);
            this.UpdateIntervalLabel.Name = "UpdateIntervalLabel";
            this.UpdateIntervalLabel.Size = new System.Drawing.Size(132, 13);
            this.UpdateIntervalLabel.TabIndex = 22;
            this.UpdateIntervalLabel.Text = "Interval de mise à jour (ms)";
            // 
            // numericUpDownUpdateInterval
            // 
            this.numericUpDownUpdateInterval.Location = new System.Drawing.Point(72, 96);
            this.numericUpDownUpdateInterval.Maximum = new decimal(new int[] {
            2500,
            0,
            0,
            0});
            this.numericUpDownUpdateInterval.Name = "numericUpDownUpdateInterval";
            this.numericUpDownUpdateInterval.Size = new System.Drawing.Size(99, 20);
            this.numericUpDownUpdateInterval.TabIndex = 23;
            // 
            // SerialPortSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(509, 236);
            this.Controls.Add(this.numericUpDownUpdateInterval);
            this.Controls.Add(this.UpdateIntervalLabel);
            this.Controls.Add(this.buttonApply);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.comboBoxBaudRate);
            this.Controls.Add(this.comboBoxDataBits);
            this.Controls.Add(this.comboBoxParity);
            this.Controls.Add(this.comboBoxStopBit);
            this.Controls.Add(this.DataBitsLabel);
            this.Controls.Add(this.ParityLabel);
            this.Controls.Add(this.StopBitLabel);
            this.Controls.Add(this.comboBoxActiveCOM);
            this.Controls.Add(this.TransmissionFormat);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ActiveCom);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "SerialPortSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Paramètres d\'interface";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownUpdateInterval)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ActiveCom;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label TransmissionFormat;
        private System.Windows.Forms.ComboBox comboBoxActiveCOM;
        private System.Windows.Forms.Label StopBitLabel;
        private System.Windows.Forms.Label ParityLabel;
        private System.Windows.Forms.Label DataBitsLabel;
        private System.Windows.Forms.ComboBox comboBoxStopBit;
        private System.Windows.Forms.ComboBox comboBoxParity;
        private System.Windows.Forms.ComboBox comboBoxDataBits;
        private System.Windows.Forms.ComboBox comboBoxBaudRate;
        private System.Windows.Forms.Button buttonApply;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Label UpdateIntervalLabel;
        private System.Windows.Forms.NumericUpDown numericUpDownUpdateInterval;
    }
}