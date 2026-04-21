using ACadSharp.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MA400_export
{
    partial class MA400_export
    {

        

        /**
         * <returns>the absolute distance between the 2 studs on a 2D plan</returns>
         */
        private double getStudDistance(Circle from, Circle to)
        {
            return Math.Sqrt(Math.Pow(from.Center.X - to.Center.X, 2) + Math.Pow(from.Center.Y - to.Center.Y, 2));
        }
        /**
         * <returns>the absolute distance between the 2 studs on a 2D plan</returns>
         */
        private double getStudDistance(System.Drawing.Point from, Circle to)
        {
            return Math.Sqrt(Math.Pow(from.X - to.Center.X, 2) + Math.Pow(from.Y - to.Center.Y, 2));
        }

        private double getStudDistance(PointF from, Circle to)
        {
            return Math.Sqrt(Math.Pow(from.X - to.Center.X, 2) + Math.Pow(from.Y - to.Center.Y, 2));
        }

        /**
         * <summary>check if the stud are all further away than both there radius combined</summary>
         * <returns>true if it is possible to add the stud false otherwise</returns>
         */
        public bool IsPossibleToAddStud(Stud Stud)
        {
            foreach (Stud candidate in Studs)
            {
                if (getStudDistance(candidate.circle, Stud.circle) < (Stud.circle.Radius + candidate.circle.Radius))
                {
                    MessageBox.Show("position invalide pour poser un goujon.\r\nTrop près d'un goujon existant");
                    return false;
                }
            }
            return true;
        }
        /**
         * <summary>check if the stud are all further away than both there radius combined</summary>
         * <returns>true if it is possible to add the stud false otherwise</returns>
         */
        public bool IsPossibleToAddStud(Circle Stud)
        {
            foreach (Stud candidate in Studs)
            {
                if (getStudDistance(candidate.circle, Stud) < (Stud.Radius + candidate.circle.Radius))
                {
                    MessageBox.Show("position invalide pour poser un goujon.\r\nTrop près d'un goujon existant");
                    return false;
                }
            }
            return true;
        }

        /**
         * <summary>add a stud to the local collection</summary>
         */
        public void AddStud(Circle stud)
        {
            
            Studs.Add(new Stud(stud, StudCurrentIndex) );

            StudCurrentIndex++;
        }

        /**
         * <summary>add a stud to the local collection</summary>
         */
        public void AddStud(double x, double y, double diam)
        {
            Circle circle = new Circle();

            circle.Radius = diam / 2;
            circle.Center = new CSMath.XYZ(x, y, 0);
            AddStud(circle);
        }



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
            this.nouveauToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ouvrirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.enregistrerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enregistrersousToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.imprimerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aperçuavantimpressionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.quitterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.annulerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rétablirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.couperToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copierToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.collerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.sélectionnertoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.YCoord_Display = new System.Windows.Forms.Label();
            this.XCoord_Display = new System.Windows.Forms.Label();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.ButtonCursorMode = new System.Windows.Forms.ToolStripButton();
            this.ButtonAddStudMode = new System.Windows.Forms.ToolStripButton();
            this.ButtonremoveStudMode = new System.Windows.Forms.ToolStripButton();
            this.textBox_StudCoord_X = new System.Windows.Forms.TextBox();
            this.textBox_StudCoord_Y = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxDiam = new System.Windows.Forms.ComboBox();
            this.fileSystemBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.origin_offset_label = new System.Windows.Forms.Label();
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
            this.WorkZone.Click += new System.EventHandler(this.WorkZone_Click);
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
            this.toolStripSeparator,
            this.enregistrerToolStripMenuItem,
            this.enregistrersousToolStripMenuItem,
            this.toolStripSeparator1,
            this.imprimerToolStripMenuItem,
            this.aperçuavantimpressionToolStripMenuItem,
            this.toolStripSeparator2,
            this.quitterToolStripMenuItem});
            this.fichierToolStripMenuItem.Name = "fichierToolStripMenuItem";
            this.fichierToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.fichierToolStripMenuItem.Text = "&Fichier";
            // 
            // nouveauToolStripMenuItem
            // 
            this.nouveauToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("nouveauToolStripMenuItem.Image")));
            this.nouveauToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.nouveauToolStripMenuItem.Name = "nouveauToolStripMenuItem";
            this.nouveauToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.nouveauToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.nouveauToolStripMenuItem.Text = "&Nouveau";
            // 
            // ouvrirToolStripMenuItem
            // 
            this.ouvrirToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("ouvrirToolStripMenuItem.Image")));
            this.ouvrirToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ouvrirToolStripMenuItem.Name = "ouvrirToolStripMenuItem";
            this.ouvrirToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.ouvrirToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.ouvrirToolStripMenuItem.Text = "&Ouvrir";
            this.ouvrirToolStripMenuItem.Click += new System.EventHandler(this.ouvrirToolStripMenuItem_Click);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(202, 6);
            // 
            // enregistrerToolStripMenuItem
            // 
            this.enregistrerToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("enregistrerToolStripMenuItem.Image")));
            this.enregistrerToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.enregistrerToolStripMenuItem.Name = "enregistrerToolStripMenuItem";
            this.enregistrerToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.enregistrerToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.enregistrerToolStripMenuItem.Text = "&Enregistrer";
            // 
            // enregistrersousToolStripMenuItem
            // 
            this.enregistrersousToolStripMenuItem.Name = "enregistrersousToolStripMenuItem";
            this.enregistrersousToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.enregistrersousToolStripMenuItem.Text = "Enregistrer &sous";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(202, 6);
            // 
            // imprimerToolStripMenuItem
            // 
            this.imprimerToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("imprimerToolStripMenuItem.Image")));
            this.imprimerToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.imprimerToolStripMenuItem.Name = "imprimerToolStripMenuItem";
            this.imprimerToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.imprimerToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.imprimerToolStripMenuItem.Text = "&Imprimer";
            // 
            // aperçuavantimpressionToolStripMenuItem
            // 
            this.aperçuavantimpressionToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("aperçuavantimpressionToolStripMenuItem.Image")));
            this.aperçuavantimpressionToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.aperçuavantimpressionToolStripMenuItem.Name = "aperçuavantimpressionToolStripMenuItem";
            this.aperçuavantimpressionToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.aperçuavantimpressionToolStripMenuItem.Text = "Aperçu a&vant impression";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(202, 6);
            // 
            // quitterToolStripMenuItem
            // 
            this.quitterToolStripMenuItem.Name = "quitterToolStripMenuItem";
            this.quitterToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.quitterToolStripMenuItem.Text = "&Quitter";
            // 
            // editionToolStripMenuItem
            // 
            this.editionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.annulerToolStripMenuItem,
            this.rétablirToolStripMenuItem,
            this.toolStripSeparator3,
            this.couperToolStripMenuItem,
            this.copierToolStripMenuItem,
            this.collerToolStripMenuItem,
            this.toolStripSeparator4,
            this.sélectionnertoutToolStripMenuItem});
            this.editionToolStripMenuItem.Name = "editionToolStripMenuItem";
            this.editionToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
            this.editionToolStripMenuItem.Text = "&Edition";
            // 
            // annulerToolStripMenuItem
            // 
            this.annulerToolStripMenuItem.Name = "annulerToolStripMenuItem";
            this.annulerToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.annulerToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.annulerToolStripMenuItem.Text = "&Annuler";
            // 
            // rétablirToolStripMenuItem
            // 
            this.rétablirToolStripMenuItem.Name = "rétablirToolStripMenuItem";
            this.rétablirToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.rétablirToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.rétablirToolStripMenuItem.Text = "&Rétablir";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(161, 6);
            // 
            // couperToolStripMenuItem
            // 
            this.couperToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("couperToolStripMenuItem.Image")));
            this.couperToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.couperToolStripMenuItem.Name = "couperToolStripMenuItem";
            this.couperToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.couperToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.couperToolStripMenuItem.Text = "&Couper";
            // 
            // copierToolStripMenuItem
            // 
            this.copierToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("copierToolStripMenuItem.Image")));
            this.copierToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.copierToolStripMenuItem.Name = "copierToolStripMenuItem";
            this.copierToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.copierToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.copierToolStripMenuItem.Text = "Co&pier";
            // 
            // collerToolStripMenuItem
            // 
            this.collerToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("collerToolStripMenuItem.Image")));
            this.collerToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.collerToolStripMenuItem.Name = "collerToolStripMenuItem";
            this.collerToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.collerToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.collerToolStripMenuItem.Text = "Co&ller";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(161, 6);
            // 
            // sélectionnertoutToolStripMenuItem
            // 
            this.sélectionnertoutToolStripMenuItem.Name = "sélectionnertoutToolStripMenuItem";
            this.sélectionnertoutToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.sélectionnertoutToolStripMenuItem.Text = "Sélectio&nner tout";
            // 
            // outilsToolStripMenuItem
            // 
            this.outilsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.personnaliserToolStripMenuItem,
            this.optionsToolStripMenuItem});
            this.outilsToolStripMenuItem.Name = "outilsToolStripMenuItem";
            this.outilsToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.outilsToolStripMenuItem.Text = "&Outils";
            // 
            // personnaliserToolStripMenuItem
            // 
            this.personnaliserToolStripMenuItem.Name = "personnaliserToolStripMenuItem";
            this.personnaliserToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.personnaliserToolStripMenuItem.Text = "&Personnaliser";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.optionsToolStripMenuItem.Text = "&Options";
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
            this.StudList_Display.Location = new System.Drawing.Point(854, 80);
            this.StudList_Display.Name = "StudList_Display";
            this.StudList_Display.Size = new System.Drawing.Size(160, 95);
            this.StudList_Display.TabIndex = 2;
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
            this.buttonPreviousStud.Location = new System.Drawing.Point(1026, 87);
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
            this.buttonNextStud.Location = new System.Drawing.Point(1026, 129);
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
            this.buttonRemoveStud.Location = new System.Drawing.Point(854, 182);
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
            this.buttonAddStud.Location = new System.Drawing.Point(854, 210);
            this.buttonAddStud.Name = "buttonAddStud";
            this.buttonAddStud.Size = new System.Drawing.Size(158, 23);
            this.buttonAddStud.TabIndex = 8;
            this.buttonAddStud.Text = "Ajouter";
            this.buttonAddStud.UseVisualStyleBackColor = true;
            this.buttonAddStud.Click += new System.EventHandler(this.buttonAddStud_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "DXF files (*.dxf)|*dxf";
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
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
            this.ButtonAddStudMode,
            this.ButtonremoveStudMode});
            this.toolStrip2.Location = new System.Drawing.Point(0, 24);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(1064, 25);
            this.toolStrip2.TabIndex = 13;
            this.toolStrip2.Text = "toolStrip2";
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
            // textBox_StudCoord_X
            // 
            this.textBox_StudCoord_X.Location = new System.Drawing.Point(957, 245);
            this.textBox_StudCoord_X.Name = "textBox_StudCoord_X";
            this.textBox_StudCoord_X.Size = new System.Drawing.Size(57, 20);
            this.textBox_StudCoord_X.TabIndex = 14;
            // 
            // textBox_StudCoord_Y
            // 
            this.textBox_StudCoord_Y.Location = new System.Drawing.Point(957, 271);
            this.textBox_StudCoord_Y.Name = "textBox_StudCoord_Y";
            this.textBox_StudCoord_Y.Size = new System.Drawing.Size(57, 20);
            this.textBox_StudCoord_Y.TabIndex = 15;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label1.Location = new System.Drawing.Point(851, 248);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "Coordonnée X (mm)";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(851, 275);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 13);
            this.label2.TabIndex = 18;
            this.label2.Text = "Coordonnée Y (mm)";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(865, 302);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 13);
            this.label3.TabIndex = 19;
            this.label3.Text = "Diamètre (mm)";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // comboBoxDiam
            // 
            this.comboBoxDiam.FormattingEnabled = true;
            this.comboBoxDiam.ItemHeight = 13;
            this.comboBoxDiam.Items.AddRange(new object[] {
            "3",
            "4"});
            this.comboBoxDiam.Location = new System.Drawing.Point(957, 298);
            this.comboBoxDiam.Name = "comboBoxDiam";
            this.comboBoxDiam.Size = new System.Drawing.Size(57, 21);
            this.comboBoxDiam.TabIndex = 21;
            this.comboBoxDiam.Text = "3";
            // 
            // origin_offset_label
            // 
            this.origin_offset_label.AutoSize = true;
            this.origin_offset_label.Location = new System.Drawing.Point(864, 355);
            this.origin_offset_label.Name = "origin_offset_label";
            this.origin_offset_label.Size = new System.Drawing.Size(19, 13);
            this.origin_offset_label.TabIndex = 22;
            this.origin_offset_label.Text = "=>";
            // 
            // MA400_export
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1064, 681);
            this.Controls.Add(this.origin_offset_label);
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
        private System.Windows.Forms.ToolStripMenuItem imprimerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aperçuavantimpressionToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem quitterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem annulerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rétablirToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem couperToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copierToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem collerToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem sélectionnertoutToolStripMenuItem;
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
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
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
        private Label origin_offset_label;
    }
}

