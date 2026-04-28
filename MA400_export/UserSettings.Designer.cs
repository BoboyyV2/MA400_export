namespace MA400_export
{
    partial class UserSettings
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
            this.OutputPathLabel = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.textBoxOutputPath = new System.Windows.Forms.TextBox();
            this.buttonChoseOutputPath = new System.Windows.Forms.Button();
            this.ProgramNumberLabel = new System.Windows.Forms.Label();
            this.textBoxProgramNumber = new System.Windows.Forms.TextBox();
            this.buttonApply = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // OutputPathLabel
            // 
            this.OutputPathLabel.AutoSize = true;
            this.OutputPathLabel.Location = new System.Drawing.Point(80, 48);
            this.OutputPathLabel.Name = "OutputPathLabel";
            this.OutputPathLabel.Size = new System.Drawing.Size(91, 13);
            this.OutputPathLabel.TabIndex = 0;
            this.OutputPathLabel.Text = "Dossier de sortie :";
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(673, 322);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(101, 27);
            this.buttonCancel.TabIndex = 19;
            this.buttonCancel.Text = "&Annuler";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(566, 322);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(101, 27);
            this.buttonOK.TabIndex = 18;
            this.buttonOK.Text = "&OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // textBoxOutputPath
            // 
            this.textBoxOutputPath.Location = new System.Drawing.Point(177, 44);
            this.textBoxOutputPath.Name = "textBoxOutputPath";
            this.textBoxOutputPath.Size = new System.Drawing.Size(438, 20);
            this.textBoxOutputPath.TabIndex = 20;
            // 
            // buttonChoseOutputPath
            // 
            this.buttonChoseOutputPath.Location = new System.Drawing.Point(621, 44);
            this.buttonChoseOutputPath.Name = "buttonChoseOutputPath";
            this.buttonChoseOutputPath.Size = new System.Drawing.Size(114, 21);
            this.buttonChoseOutputPath.TabIndex = 21;
            this.buttonChoseOutputPath.Text = "Choisir un dossier";
            this.buttonChoseOutputPath.UseVisualStyleBackColor = true;
            // 
            // ProgramNumberLabel
            // 
            this.ProgramNumberLabel.AutoSize = true;
            this.ProgramNumberLabel.Location = new System.Drawing.Point(19, 119);
            this.ProgramNumberLabel.Name = "ProgramNumberLabel";
            this.ProgramNumberLabel.Size = new System.Drawing.Size(152, 13);
            this.ProgramNumberLabel.TabIndex = 22;
            this.ProgramNumberLabel.Text = "Numéro de programme actuel :";
            // 
            // textBoxProgramNumber
            // 
            this.textBoxProgramNumber.Location = new System.Drawing.Point(177, 116);
            this.textBoxProgramNumber.Name = "textBoxProgramNumber";
            this.textBoxProgramNumber.Size = new System.Drawing.Size(126, 20);
            this.textBoxProgramNumber.TabIndex = 23;
            // 
            // buttonApply
            // 
            this.buttonApply.Location = new System.Drawing.Point(459, 322);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(101, 27);
            this.buttonApply.TabIndex = 24;
            this.buttonApply.Text = "&Appliquer";
            this.buttonApply.UseVisualStyleBackColor = true;
            // 
            // UserSettings
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 361);
            this.Controls.Add(this.buttonApply);
            this.Controls.Add(this.textBoxProgramNumber);
            this.Controls.Add(this.ProgramNumberLabel);
            this.Controls.Add(this.buttonChoseOutputPath);
            this.Controls.Add(this.textBoxOutputPath);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.OutputPathLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "UserSettings";
            this.Text = "UserSettings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label OutputPathLabel;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.TextBox textBoxOutputPath;
        private System.Windows.Forms.Button buttonChoseOutputPath;
        private System.Windows.Forms.Label ProgramNumberLabel;
        private System.Windows.Forms.TextBox textBoxProgramNumber;
        private System.Windows.Forms.Button buttonApply;
    }
}