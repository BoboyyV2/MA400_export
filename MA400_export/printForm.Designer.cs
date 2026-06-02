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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(printForm));
            this.StudListBox = new System.Windows.Forms.ListBox();
            this.fileSystemBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.pictureBoxpreview = new System.Windows.Forms.PictureBox();
            this.printDocument = new System.Drawing.Printing.PrintDocument();
            this.printDialog = new System.Windows.Forms.PrintDialog();
            this.printPreviewDialog = new System.Windows.Forms.PrintPreviewDialog();
            this.studlist = new System.Windows.Forms.Label();
            this.PrintButton = new System.Windows.Forms.Button();
            this.PreviewButton = new System.Windows.Forms.Button();
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
            this.pictureBoxpreview.Location = new System.Drawing.Point(12, 37);
            this.pictureBoxpreview.Name = "pictureBoxpreview";
            this.pictureBoxpreview.Size = new System.Drawing.Size(800, 600);
            this.pictureBoxpreview.TabIndex = 2;
            this.pictureBoxpreview.TabStop = false;
            // 
            // printDocument
            // 
            this.printDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument_PrintPage);
            // 
            // printDialog
            // 
            this.printDialog.UseEXDialog = true;
            // 
            // printPreviewDialog
            // 
            this.printPreviewDialog.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.printPreviewDialog.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.printPreviewDialog.ClientSize = new System.Drawing.Size(400, 300);
            this.printPreviewDialog.Enabled = true;
            this.printPreviewDialog.Icon = ((System.Drawing.Icon)(resources.GetObject("printPreviewDialog.Icon")));
            this.printPreviewDialog.Name = "printPreviewDialog";
            this.printPreviewDialog.Visible = false;
            // 
            // studlist
            // 
            this.studlist.AutoSize = true;
            this.studlist.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.studlist.Location = new System.Drawing.Point(7, 640);
            this.studlist.Name = "studlist";
            this.studlist.Size = new System.Drawing.Size(193, 25);
            this.studlist.TabIndex = 3;
            this.studlist.Text = "Liste des goujons :";
            // 
            // PrintButton
            // 
            this.PrintButton.Location = new System.Drawing.Point(12, 8);
            this.PrintButton.Name = "PrintButton";
            this.PrintButton.Size = new System.Drawing.Size(75, 23);
            this.PrintButton.TabIndex = 4;
            this.PrintButton.Text = "&Imprimer";
            this.PrintButton.UseVisualStyleBackColor = true;
            this.PrintButton.Click += new System.EventHandler(this.PrintButton_Click);
            // 
            // PreviewButton
            // 
            this.PreviewButton.Location = new System.Drawing.Point(93, 8);
            this.PreviewButton.Name = "PreviewButton";
            this.PreviewButton.Size = new System.Drawing.Size(75, 23);
            this.PreviewButton.TabIndex = 5;
            this.PreviewButton.Text = "&Aperçu";
            this.PreviewButton.UseVisualStyleBackColor = true;
            this.PreviewButton.Click += new System.EventHandler(this.PreviewButton_Click);
            // 
            // printForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(826, 1061);
            this.Controls.Add(this.PreviewButton);
            this.Controls.Add(this.PrintButton);
            this.Controls.Add(this.studlist);
            this.Controls.Add(this.pictureBoxpreview);
            this.Controls.Add(this.StudListBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "printForm";
            this.Text = "printForm";
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxpreview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ListBox StudListBox;
        private System.Windows.Forms.BindingSource fileSystemBindingSource;
        private System.Windows.Forms.PictureBox pictureBoxpreview;
        private System.Drawing.Printing.PrintDocument printDocument;
        private System.Windows.Forms.PrintDialog printDialog;
        private System.Windows.Forms.PrintPreviewDialog printPreviewDialog;
        private System.Windows.Forms.Label studlist;
        private System.Windows.Forms.Button PrintButton;
        private System.Windows.Forms.Button PreviewButton;
    }
}