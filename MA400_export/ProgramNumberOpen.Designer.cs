namespace MA400_export
{
    partial class ProgramNumberOpen
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
            this.ProgramNumberLabel = new System.Windows.Forms.Label();
            this.ProgramNumberText = new System.Windows.Forms.TextBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ProgramNumberLabel
            // 
            this.ProgramNumberLabel.AutoSize = true;
            this.ProgramNumberLabel.Location = new System.Drawing.Point(61, 53);
            this.ProgramNumberLabel.Name = "ProgramNumberLabel";
            this.ProgramNumberLabel.Size = new System.Drawing.Size(114, 13);
            this.ProgramNumberLabel.TabIndex = 0;
            this.ProgramNumberLabel.Text = "Numéro de programme";
            // 
            // ProgramNumberText
            // 
            this.ProgramNumberText.Location = new System.Drawing.Point(181, 50);
            this.ProgramNumberText.Name = "ProgramNumberText";
            this.ProgramNumberText.Size = new System.Drawing.Size(198, 20);
            this.ProgramNumberText.TabIndex = 1;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(372, 97);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(101, 27);
            this.buttonCancel.TabIndex = 19;
            this.buttonCancel.Text = "&Annuler";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(265, 97);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(101, 27);
            this.buttonOK.TabIndex = 18;
            this.buttonOK.Text = "&OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // ProgramNumberOpen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 136);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.ProgramNumberText);
            this.Controls.Add(this.ProgramNumberLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ProgramNumberOpen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ProgramNumber";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ProgramNumberLabel;
        private System.Windows.Forms.TextBox ProgramNumberText;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
    }
}