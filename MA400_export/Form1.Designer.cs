using ACadSharp.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MA400_export
{
    partial class MA400_export
    {

        
       



        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MA400_export));
            this.WorkZone = new System.Windows.Forms.Panel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fichierToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ouvrirprogramToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.enregistrersousToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.quitterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.générerLesFichiersDeSortieToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.outilsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.personnaliserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aideToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sommaireToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.indexToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rechercherToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.àproposdeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.StudList_Display = new System.Windows.Forms.ListBox();
            this.StudsMenuLabel = new System.Windows.Forms.Label();
            this.buttonPreviousStud = new System.Windows.Forms.Button();
            this.buttonNextStud = new System.Windows.Forms.Button();
            this.buttonRemoveStud = new System.Windows.Forms.Button();
            this.buttonAddStud = new System.Windows.Forms.Button();
            this.openFileDialogOpen = new System.Windows.Forms.OpenFileDialog();
            this.YCoord_Display = new System.Windows.Forms.Label();
            this.XCoord_Display = new System.Windows.Forms.Label();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.textBox_StudCoord_X = new System.Windows.Forms.TextBox();
            this.textBox_StudCoord_Y = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxDiam = new System.Windows.Forms.ComboBox();
            this.buttonGenerer = new System.Windows.Forms.Button();
            this.saveFileDialogSave = new System.Windows.Forms.SaveFileDialog();
            this.ButtonCursorMode = new System.Windows.Forms.ToolStripButton();
            this.ButtonSelectStudMode = new System.Windows.Forms.ToolStripButton();
            this.ButtonAddStudMode = new System.Windows.Forms.ToolStripButton();
            this.ButtonremoveStudMode = new System.Windows.Forms.ToolStripButton();
            this.nouveauToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ouvrirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enregistrerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileSystemBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.menuStrip1.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // WorkZone
            // 
            this.WorkZone.AutoSize = true;
            this.WorkZone.Location = new System.Drawing.Point(0, 52);
            this.WorkZone.Name = "WorkZone";
            this.WorkZone.Size = new System.Drawing.Size(800, 600);
            this.WorkZone.TabIndex = 10;
            //this.WorkZone.Click += new System.EventHandler(this.WorkZone_Click);
            this.WorkZone.MouseClick += new System.Windows.Forms.MouseEventHandler(this.WorkZone_Click);
            this.WorkZone.Paint += new System.Windows.Forms.PaintEventHandler(this.WorkZone_Paint);
            this.WorkZone.MouseMove += new System.Windows.Forms.MouseEventHandler(this.WorkZone_MouseMove);
            this.WorkZone.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.WorkZone_Zoom);
            // 
            // menuStrip1
            // 
            this.menuStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fichierToolStripMenuItem,
            this.editionToolStripMenuItem,
            this.outilsToolStripMenuItem,
            this.aideToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.menuStrip1.Size = new System.Drawing.Size(1064, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fichierToolStripMenuItem
            // 
            this.fichierToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nouveauToolStripMenuItem,
            this.ouvrirToolStripMenuItem,
            this.ouvrirprogramToolStripMenuItem,
            this.toolStripSeparator,
            this.enregistrerToolStripMenuItem,
            this.enregistrersousToolStripMenuItem,
            this.toolStripSeparator1,
            this.quitterToolStripMenuItem});
            this.fichierToolStripMenuItem.Name = "fichierToolStripMenuItem";
            this.fichierToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.fichierToolStripMenuItem.Text = "&Fichier";
            // 
            // ouvrirprogramToolStripMenuItem
            // 
            this.ouvrirprogramToolStripMenuItem.Name = "ouvrirprogramToolStripMenuItem";
            this.ouvrirprogramToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.ouvrirprogramToolStripMenuItem.Text = "&Ouvrir &program";
            this.ouvrirprogramToolStripMenuItem.Click += new System.EventHandler(this.ouvrirprogramToolStripMenuItem_Click);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(167, 6);
            // 
            // enregistrersousToolStripMenuItem
            // 
            this.enregistrersousToolStripMenuItem.Name = "enregistrersousToolStripMenuItem";
            this.enregistrersousToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.enregistrersousToolStripMenuItem.Text = "Enregistrer &sous";
            this.enregistrersousToolStripMenuItem.Click += new System.EventHandler(this.enregistrersousToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(167, 6);
            // 
            // quitterToolStripMenuItem
            // 
            this.quitterToolStripMenuItem.Name = "quitterToolStripMenuItem";
            this.quitterToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.quitterToolStripMenuItem.Text = "&Quitter";
            this.quitterToolStripMenuItem.Click += new System.EventHandler(this.quitterToolStripMenuItem_Click);
            // 
            // editionToolStripMenuItem
            // 
            this.editionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.générerLesFichiersDeSortieToolStripMenuItem});
            this.editionToolStripMenuItem.Name = "editionToolStripMenuItem";
            this.editionToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.editionToolStripMenuItem.Text = "&Générer";
            // 
            // générerLesFichiersDeSortieToolStripMenuItem
            // 
            this.générerLesFichiersDeSortieToolStripMenuItem.Name = "générerLesFichiersDeSortieToolStripMenuItem";
            this.générerLesFichiersDeSortieToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.générerLesFichiersDeSortieToolStripMenuItem.Text = "&Générer les fichiers de sortie";
            this.générerLesFichiersDeSortieToolStripMenuItem.Click += new System.EventHandler(this.générerLesFichiersDeSortieToolStripMenuItem_Click);
            // 
            // outilsToolStripMenuItem
            // 
            this.outilsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.personnaliserToolStripMenuItem,
            this.optionsToolStripMenuItem});
            this.outilsToolStripMenuItem.Name = "outilsToolStripMenuItem";
            this.outilsToolStripMenuItem.Size = new System.Drawing.Size(78, 20);
            this.outilsToolStripMenuItem.Text = "&Paramètres";
            // 
            // personnaliserToolStripMenuItem
            // 
            this.personnaliserToolStripMenuItem.Name = "personnaliserToolStripMenuItem";
            this.personnaliserToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.personnaliserToolStripMenuItem.Text = "&Personnaliser";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.optionsToolStripMenuItem.Text = "&Options";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // aideToolStripMenuItem
            // 
            this.aideToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sommaireToolStripMenuItem,
            this.indexToolStripMenuItem,
            this.rechercherToolStripMenuItem,
            this.toolStripSeparator5,
            this.àproposdeToolStripMenuItem});
            this.aideToolStripMenuItem.Name = "aideToolStripMenuItem";
            this.aideToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.aideToolStripMenuItem.Text = "&Aide";
            // 
            // sommaireToolStripMenuItem
            // 
            this.sommaireToolStripMenuItem.Name = "sommaireToolStripMenuItem";
            this.sommaireToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.sommaireToolStripMenuItem.Text = "&Sommaire";
            // 
            // indexToolStripMenuItem
            // 
            this.indexToolStripMenuItem.Name = "indexToolStripMenuItem";
            this.indexToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.indexToolStripMenuItem.Text = "&Index";
            // 
            // rechercherToolStripMenuItem
            // 
            this.rechercherToolStripMenuItem.Name = "rechercherToolStripMenuItem";
            this.rechercherToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.rechercherToolStripMenuItem.Text = "&Rechercher";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(144, 6);
            // 
            // àproposdeToolStripMenuItem
            // 
            this.àproposdeToolStripMenuItem.Name = "àproposdeToolStripMenuItem";
            this.àproposdeToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.àproposdeToolStripMenuItem.Text = "À &propos de...";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.toolStrip1.Location = new System.Drawing.Point(0, 681);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.toolStrip1.Size = new System.Drawing.Size(1064, 0);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // StudList_Display
            // 
            this.StudList_Display.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.StudList_Display.FormattingEnabled = true;
            this.StudList_Display.Location = new System.Drawing.Point(830, 80);
            this.StudList_Display.Name = "StudList_Display";
            this.StudList_Display.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.StudList_Display.Size = new System.Drawing.Size(200, 95);
            this.StudList_Display.TabIndex = 2;
            this.StudList_Display.SelectedIndexChanged += new System.EventHandler(this.StudList_Display_SelectedIndexChanged);
            // 
            // StudsMenuLabel
            // 
            this.StudsMenuLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.StudsMenuLabel.AutoSize = true;
            this.StudsMenuLabel.Location = new System.Drawing.Point(895, 63);
            this.StudsMenuLabel.Name = "StudsMenuLabel";
            this.StudsMenuLabel.Size = new System.Drawing.Size(76, 13);
            this.StudsMenuLabel.TabIndex = 3;
            this.StudsMenuLabel.Text = "Menu Goujons";
            // 
            // buttonPreviousStud
            // 
            this.buttonPreviousStud.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonPreviousStud.Location = new System.Drawing.Point(1034, 87);
            this.buttonPreviousStud.Name = "buttonPreviousStud";
            this.buttonPreviousStud.Size = new System.Drawing.Size(13, 39);
            this.buttonPreviousStud.TabIndex = 4;
            this.buttonPreviousStud.Text = "↑";
            this.buttonPreviousStud.UseVisualStyleBackColor = true;
            this.buttonPreviousStud.Click += new System.EventHandler(this.buttonPreviousStud_Click);
            // 
            // buttonNextStud
            // 
            this.buttonNextStud.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonNextStud.Location = new System.Drawing.Point(1034, 129);
            this.buttonNextStud.Name = "buttonNextStud";
            this.buttonNextStud.Size = new System.Drawing.Size(13, 39);
            this.buttonNextStud.TabIndex = 5;
            this.buttonNextStud.Text = "↓";
            this.buttonNextStud.UseVisualStyleBackColor = true;
            this.buttonNextStud.Click += new System.EventHandler(this.buttonNextStud_Click);
            // 
            // buttonRemoveStud
            // 
            this.buttonRemoveStud.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRemoveStud.Location = new System.Drawing.Point(850, 182);
            this.buttonRemoveStud.Name = "buttonRemoveStud";
            this.buttonRemoveStud.Size = new System.Drawing.Size(158, 23);
            this.buttonRemoveStud.TabIndex = 7;
            this.buttonRemoveStud.Text = "Supprimer la sélection";
            this.buttonRemoveStud.UseVisualStyleBackColor = true;
            this.buttonRemoveStud.Click += new System.EventHandler(this.buttonRemoveStud_Click);
            // 
            // buttonAddStud
            // 
            this.buttonAddStud.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAddStud.Location = new System.Drawing.Point(850, 210);
            this.buttonAddStud.Name = "buttonAddStud";
            this.buttonAddStud.Size = new System.Drawing.Size(158, 23);
            this.buttonAddStud.TabIndex = 8;
            this.buttonAddStud.Text = "Ajouter";
            this.buttonAddStud.UseVisualStyleBackColor = true;
            this.buttonAddStud.Click += new System.EventHandler(this.buttonAddStud_Click);
            // 
            // openFileDialogOpen
            // 
            this.openFileDialogOpen.Filter = "DXF files (*.dxf)|*dxf";
            this.openFileDialogOpen.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialogOpen_FileOk);
            // 
            // YCoord_Display
            // 
            this.YCoord_Display.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.YCoord_Display.AutoSize = true;
            this.YCoord_Display.Location = new System.Drawing.Point(540, 659);
            this.YCoord_Display.Name = "YCoord_Display";
            this.YCoord_Display.Size = new System.Drawing.Size(71, 13);
            this.YCoord_Display.TabIndex = 11;
            this.YCoord_Display.Text = "Y = 00.00000";
            // 
            // XCoord_Display
            // 
            this.XCoord_Display.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.XCoord_Display.AutoSize = true;
            this.XCoord_Display.Location = new System.Drawing.Point(468, 659);
            this.XCoord_Display.Name = "XCoord_Display";
            this.XCoord_Display.Size = new System.Drawing.Size(71, 13);
            this.XCoord_Display.TabIndex = 12;
            this.XCoord_Display.Text = "X = 00.00000";
            // 
            // toolStrip2
            // 
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ButtonCursorMode,
            this.ButtonSelectStudMode,
            this.ButtonAddStudMode,
            this.ButtonremoveStudMode});
            this.toolStrip2.Location = new System.Drawing.Point(0, 24);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(1064, 25);
            this.toolStrip2.TabIndex = 13;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // textBox_StudCoord_X
            // 
            this.textBox_StudCoord_X.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_StudCoord_X.Location = new System.Drawing.Point(953, 245);
            this.textBox_StudCoord_X.Name = "textBox_StudCoord_X";
            this.textBox_StudCoord_X.Size = new System.Drawing.Size(57, 20);
            this.textBox_StudCoord_X.TabIndex = 14;
            // 
            // textBox_StudCoord_Y
            // 
            this.textBox_StudCoord_Y.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_StudCoord_Y.Location = new System.Drawing.Point(953, 271);
            this.textBox_StudCoord_Y.Name = "textBox_StudCoord_Y";
            this.textBox_StudCoord_Y.Size = new System.Drawing.Size(57, 20);
            this.textBox_StudCoord_Y.TabIndex = 15;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label1.Location = new System.Drawing.Point(847, 248);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "Coordonnée X (mm)";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(847, 275);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 13);
            this.label2.TabIndex = 18;
            this.label2.Text = "Coordonnée Y (mm)";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(861, 302);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 13);
            this.label3.TabIndex = 19;
            this.label3.Text = "Diamètre (mm)";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // comboBoxDiam
            // 
            this.comboBoxDiam.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxDiam.FormattingEnabled = true;
            this.comboBoxDiam.ItemHeight = 13;
            this.comboBoxDiam.Items.AddRange(new object[] {
            "3",
            "4"});
            this.comboBoxDiam.Location = new System.Drawing.Point(953, 298);
            this.comboBoxDiam.Name = "comboBoxDiam";
            this.comboBoxDiam.Size = new System.Drawing.Size(57, 21);
            this.comboBoxDiam.TabIndex = 21;
            this.comboBoxDiam.Text = "3";
            // 
            // buttonGenerer
            // 
            this.buttonGenerer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonGenerer.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F);
            this.buttonGenerer.Location = new System.Drawing.Point(850, 496);
            this.buttonGenerer.Name = "buttonGenerer";
            this.buttonGenerer.Size = new System.Drawing.Size(160, 54);
            this.buttonGenerer.TabIndex = 22;
            this.buttonGenerer.Text = "Générer";
            this.buttonGenerer.UseVisualStyleBackColor = true;
            this.buttonGenerer.Click += new System.EventHandler(this.buttonGenerer_Click);
            // 
            // saveFileDialogSave
            // 
            this.saveFileDialogSave.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileDialogSave_FileOk);
            // 
            // ButtonCursorMode
            // 
            this.ButtonCursorMode.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonCursorMode.Image = ((System.Drawing.Image)(resources.GetObject("ButtonCursorMode.Image")));
            this.ButtonCursorMode.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonCursorMode.Name = "ButtonCursorMode";
            this.ButtonCursorMode.Size = new System.Drawing.Size(23, 22);
            this.ButtonCursorMode.Text = "Curseur";
            this.ButtonCursorMode.Click += new System.EventHandler(this.ButtonCursorMode_Click);
            // 
            // ButtonSelectStudMode
            // 
            this.ButtonSelectStudMode.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonSelectStudMode.Image = ((System.Drawing.Image)(resources.GetObject("ButtonSelectStudMode.Image")));
            this.ButtonSelectStudMode.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonSelectStudMode.Name = "ButtonSelectStudMode";
            this.ButtonSelectStudMode.Size = new System.Drawing.Size(23, 22);
            this.ButtonSelectStudMode.Text = "Sélectionner un goujon";
            this.ButtonSelectStudMode.Click += new System.EventHandler(this.ButtonSelectStudMode_Click);
            // 
            // ButtonAddStudMode
            // 
            this.ButtonAddStudMode.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonAddStudMode.Image = ((System.Drawing.Image)(resources.GetObject("ButtonAddStudMode.Image")));
            this.ButtonAddStudMode.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonAddStudMode.Name = "ButtonAddStudMode";
            this.ButtonAddStudMode.Size = new System.Drawing.Size(23, 22);
            this.ButtonAddStudMode.Text = "Ajouter un goujon";
            this.ButtonAddStudMode.Click += new System.EventHandler(this.ButtonAddStudMode_Click);
            // 
            // ButtonremoveStudMode
            // 
            this.ButtonremoveStudMode.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonremoveStudMode.Image = ((System.Drawing.Image)(resources.GetObject("ButtonremoveStudMode.Image")));
            this.ButtonremoveStudMode.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonremoveStudMode.Name = "ButtonremoveStudMode";
            this.ButtonremoveStudMode.Size = new System.Drawing.Size(23, 22);
            this.ButtonremoveStudMode.Text = "Supprimer un goujon";
            this.ButtonremoveStudMode.Click += new System.EventHandler(this.ButtonremoveStudMode_Click);
            // 
            // nouveauToolStripMenuItem
            // 
            this.nouveauToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("nouveauToolStripMenuItem.Image")));
            this.nouveauToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.nouveauToolStripMenuItem.Name = "nouveauToolStripMenuItem";
            this.nouveauToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.nouveauToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.nouveauToolStripMenuItem.Text = "&Nouveau";
            // 
            // ouvrirToolStripMenuItem
            // 
            this.ouvrirToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("ouvrirToolStripMenuItem.Image")));
            this.ouvrirToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ouvrirToolStripMenuItem.Name = "ouvrirToolStripMenuItem";
            this.ouvrirToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.ouvrirToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.ouvrirToolStripMenuItem.Text = "&Ouvrir";
            this.ouvrirToolStripMenuItem.Click += new System.EventHandler(this.ouvrirToolStripMenuItem_Click);
            // 
            // enregistrerToolStripMenuItem
            // 
            this.enregistrerToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("enregistrerToolStripMenuItem.Image")));
            this.enregistrerToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.enregistrerToolStripMenuItem.Name = "enregistrerToolStripMenuItem";
            this.enregistrerToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.enregistrerToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.enregistrerToolStripMenuItem.Text = "&Enregistrer";
            this.enregistrerToolStripMenuItem.Click += new System.EventHandler(this.enregistrerToolStripMenuItem_Click);
            // 
            // MA400_export
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1064, 681);
            this.Controls.Add(this.buttonGenerer);
            this.Controls.Add(this.comboBoxDiam);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_StudCoord_Y);
            this.Controls.Add(this.textBox_StudCoord_X);
            this.Controls.Add(this.toolStrip2);
            this.Controls.Add(this.XCoord_Display);
            this.Controls.Add(this.YCoord_Display);
            this.Controls.Add(this.buttonAddStud);
            this.Controls.Add(this.buttonRemoveStud);
            this.Controls.Add(this.buttonNextStud);
            this.Controls.Add(this.buttonPreviousStud);
            this.Controls.Add(this.StudsMenuLabel);
            this.Controls.Add(this.StudList_Display);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.WorkZone);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "MA400_export";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MA400_Export";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fichierToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nouveauToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ouvrirToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem enregistrerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem enregistrersousToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem quitterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem outilsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem personnaliserToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aideToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sommaireToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem indexToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rechercherToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem àproposdeToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ListBox StudList_Display;
        private System.Windows.Forms.Label StudsMenuLabel;
        private System.Windows.Forms.Button buttonPreviousStud;
        private System.Windows.Forms.Button buttonNextStud;
        private System.Windows.Forms.Button buttonRemoveStud;
        private System.Windows.Forms.Button buttonAddStud;
        private System.Windows.Forms.OpenFileDialog openFileDialogOpen;
        private System.Windows.Forms.Panel WorkZone;
        private System.Windows.Forms.Label YCoord_Display;
        private System.Windows.Forms.Label XCoord_Display;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton ButtonCursorMode;
        private System.Windows.Forms.ToolStripButton ButtonAddStudMode;
        private System.Windows.Forms.ToolStripButton ButtonremoveStudMode;
        private System.Windows.Forms.TextBox textBox_StudCoord_X;
        private System.Windows.Forms.TextBox textBox_StudCoord_Y;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxDiam;
        private System.Windows.Forms.BindingSource fileSystemBindingSource;
        private Button buttonGenerer;
        private SaveFileDialog saveFileDialogSave;
        private ToolStripMenuItem générerLesFichiersDeSortieToolStripMenuItem;
        private ToolStripMenuItem ouvrirprogramToolStripMenuItem;
        private ToolStripButton ButtonSelectStudMode;
    }
}

