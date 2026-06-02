namespace MA400_export
{
    partial class printForm
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
            this.components = new System.ComponentModel.Container();
            this.StudListBox = new System.Windows.Forms.ListBox();
            this.fileSystemBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.pictureBoxpreview = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxpreview)).BeginInit();
            this.SuspendLayout();
            // 
            // StudListBox
            // 
            this.StudListBox.ColumnWidth = 186;
            this.StudListBox.FormattingEnabled = true;
            this.StudListBox.Location = new System.Drawing.Point(12, 668);
            this.StudListBox.MultiColumn = true;
            this.StudListBox.Name = "StudListBox";
            this.StudListBox.Size = new System.Drawing.Size(800, 381);
            this.StudListBox.TabIndex = 1;
            // 
            // pictureBoxpreview
            // 
            this.pictureBoxpreview.BackColor = System.Drawing.Color.Black;
            this.pictureBoxpreview.Location = new System.Drawing.Point(12, 12);
            this.pictureBoxpreview.Name = "pictureBoxpreview";
            this.pictureBoxpreview.Size = new System.Drawing.Size(800, 600);
            this.pictureBoxpreview.TabIndex = 2;
            this.pictureBoxpreview.TabStop = false;
            // 
            // printForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(826, 1061);
            this.Controls.Add(this.pictureBoxpreview);
            this.Controls.Add(this.StudListBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "printForm";
            this.Text = "printForm";
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxpreview)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ListBox StudListBox;
        private System.Windows.Forms.BindingSource fileSystemBindingSource;
        private System.Windows.Forms.PictureBox pictureBoxpreview;
    }
}