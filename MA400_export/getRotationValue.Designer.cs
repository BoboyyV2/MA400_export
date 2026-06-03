namespace MA400_export
{
    partial class getRotationValue
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
            this.labelRotationValue = new System.Windows.Forms.Label();
            this.labelRotationDirection = new System.Windows.Forms.Label();
            this.textBoxRotation = new System.Windows.Forms.TextBox();
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelRotationValue
            // 
            this.labelRotationValue.AutoSize = true;
            this.labelRotationValue.Location = new System.Drawing.Point(12, 9);
            this.labelRotationValue.Name = "labelRotationValue";
            this.labelRotationValue.Size = new System.Drawing.Size(186, 13);
            this.labelRotationValue.TabIndex = 0;
            this.labelRotationValue.Text = "Saisissez l\'angle de rotation en degrés";
            // 
            // labelRotationDirection
            // 
            this.labelRotationDirection.AutoSize = true;
            this.labelRotationDirection.Location = new System.Drawing.Point(12, 22);
            this.labelRotationDirection.Name = "labelRotationDirection";
            this.labelRotationDirection.Size = new System.Drawing.Size(186, 13);
            this.labelRotationDirection.TabIndex = 1;
            this.labelRotationDirection.Text = "La rotation ce fait dans le sens horaire";
            // 
            // textBoxRotation
            // 
            this.textBoxRotation.Location = new System.Drawing.Point(81, 38);
            this.textBoxRotation.Name = "textBoxRotation";
            this.textBoxRotation.Size = new System.Drawing.Size(50, 20);
            this.textBoxRotation.TabIndex = 2;
            this.textBoxRotation.Text = "0";
            // 
            // buttonOk
            // 
            this.buttonOk.Location = new System.Drawing.Point(24, 75);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(84, 25);
            this.buttonOk.TabIndex = 3;
            this.buttonOk.Text = "&Ok";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonCancel.Location = new System.Drawing.Point(114, 75);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(84, 25);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "&Annuler";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // getRotationValue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(209, 111);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.textBoxRotation);
            this.Controls.Add(this.labelRotationDirection);
            this.Controls.Add(this.labelRotationValue);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "getRotationValue";
            this.Text = "Rotation";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelRotationValue;
        private System.Windows.Forms.Label labelRotationDirection;
        private System.Windows.Forms.TextBox textBoxRotation;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancel;
    }
}