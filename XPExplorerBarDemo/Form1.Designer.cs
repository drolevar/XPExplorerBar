using System.ComponentModel;

namespace XPExplorerBarDemo
{
    partial class DemoForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
	        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DemoForm));
	        this.systemTaskPane = new XPExplorerBar.TaskPane();
	        this.pictureTasksExpando = new XPExplorerBar.Expando();
	        this.slideShowTaskItem = new XPExplorerBar.TaskItem();
	        this.orderOnlineTaskItem = new XPExplorerBar.TaskItem();
	        this.printPicturesTaskItem = new XPExplorerBar.TaskItem();
	        this.copyToCDTaskItem = new XPExplorerBar.TaskItem();
	        this.fileAndFolderTasksExpando = new XPExplorerBar.Expando();
	        this.newFolderTaskItem = new XPExplorerBar.TaskItem();
	        this.publishToWebTaskItem = new XPExplorerBar.TaskItem();
	        this.shareFolderTaskItem = new XPExplorerBar.TaskItem();
	        this.otherPlacesExpando = new XPExplorerBar.Expando();
	        this.myDocumentsTaskItem = new XPExplorerBar.TaskItem();
	        this.myPicturesTaskItem = new XPExplorerBar.TaskItem();
	        this.myComputerTaskItem = new XPExplorerBar.TaskItem();
	        this.myNetworkPlacesTaskItem = new XPExplorerBar.TaskItem();
	        this.detailsExpando = new XPExplorerBar.Expando();
	        this.label1 = new System.Windows.Forms.Label();
	        this.label2 = new System.Windows.Forms.Label();
	        this.label3 = new System.Windows.Forms.Label();
	        this.customGroupBox = new System.Windows.Forms.GroupBox();
	        this.controlComboBox = new System.Windows.Forms.ComboBox();
	        this.customPropertyGrid = new System.Windows.Forms.PropertyGrid();
	        this.customTaskPane = new XPExplorerBar.TaskPane();
	        this.customExpando = new XPExplorerBar.Expando();
	        this.customTaskItem1 = new XPExplorerBar.TaskItem();
	        this.customTaskItem2 = new XPExplorerBar.TaskItem();
	        this.customTaskItem3 = new XPExplorerBar.TaskItem();
	        this.customButton = new System.Windows.Forms.Button();
	        this.customTextBox = new XPExplorerBar.XPTextBox();
	        this.customComboBox = new System.Windows.Forms.ComboBox();
	        this.customCheckBox = new System.Windows.Forms.CheckBox();
	        this.customRadioButton = new System.Windows.Forms.RadioButton();
	        this.menubar = new System.Windows.Forms.MainMenu(this.components);
	        this.fileMenu = new System.Windows.Forms.MenuItem();
	        this.exitMenuItem = new System.Windows.Forms.MenuItem();
	        this.viewMenu = new System.Windows.Forms.MenuItem();
	        this.animateMenuItem = new System.Windows.Forms.MenuItem();
	        this.themesMenuItem = new System.Windows.Forms.MenuItem();
	        this.classicMenuItem = new System.Windows.Forms.MenuItem();
	        this.blueMenuItem = new System.Windows.Forms.MenuItem();
	        this.xboxMenuItem = new System.Windows.Forms.MenuItem();
	        this.itunesMenuItem = new System.Windows.Forms.MenuItem();
	        this.pantherMenuItem = new System.Windows.Forms.MenuItem();
	        this.bwMenuItem = new System.Windows.Forms.MenuItem();
	        this.defaultMenuItem = new System.Windows.Forms.MenuItem();
	        this.cycleMenuItem = new System.Windows.Forms.MenuItem();
	        this.expandoDraggingMenuItem = new System.Windows.Forms.MenuItem();
	        this.separatorMenuItem1 = new System.Windows.Forms.MenuItem();
	        this.showFocusMenuItem = new System.Windows.Forms.MenuItem();
	        this.separatorMenuItem2 = new System.Windows.Forms.MenuItem();
	        this.myPicturesMenuItem = new System.Windows.Forms.MenuItem();
	        this.myComputerMenuItem = new System.Windows.Forms.MenuItem();
	        this.myNetworkMenuItem = new System.Windows.Forms.MenuItem();
	        this.serializeGroupBox = new System.Windows.Forms.GroupBox();
	        this.removeButton = new System.Windows.Forms.Button();
	        this.label5 = new System.Windows.Forms.Label();
	        this.label4 = new System.Windows.Forms.Label();
	        this.serializeMemoryButton = new System.Windows.Forms.Button();
	        this.deserializeFileButton = new System.Windows.Forms.Button();
	        this.serializeFileButton = new System.Windows.Forms.Button();
	        this.serializeTaskPane = new XPExplorerBar.TaskPane();
	        this.serializeExpando = new XPExplorerBar.Expando();
	        this.serializeTaskItem1 = new XPExplorerBar.TaskItem();
	        this.serializeTaskItem2 = new XPExplorerBar.TaskItem();
	        this.serializeTaskItem3 = new XPExplorerBar.TaskItem();
	        ((System.ComponentModel.ISupportInitialize)(this.systemTaskPane)).BeginInit();
	        this.systemTaskPane.SuspendLayout();
	        ((System.ComponentModel.ISupportInitialize)(this.pictureTasksExpando)).BeginInit();
	        this.pictureTasksExpando.SuspendLayout();
	        ((System.ComponentModel.ISupportInitialize)(this.fileAndFolderTasksExpando)).BeginInit();
	        this.fileAndFolderTasksExpando.SuspendLayout();
	        ((System.ComponentModel.ISupportInitialize)(this.otherPlacesExpando)).BeginInit();
	        this.otherPlacesExpando.SuspendLayout();
	        ((System.ComponentModel.ISupportInitialize)(this.detailsExpando)).BeginInit();
	        this.detailsExpando.SuspendLayout();
	        this.customGroupBox.SuspendLayout();
	        ((System.ComponentModel.ISupportInitialize)(this.customTaskPane)).BeginInit();
	        this.customTaskPane.SuspendLayout();
	        ((System.ComponentModel.ISupportInitialize)(this.customExpando)).BeginInit();
	        this.customExpando.SuspendLayout();
	        this.serializeGroupBox.SuspendLayout();
	        ((System.ComponentModel.ISupportInitialize)(this.serializeTaskPane)).BeginInit();
	        this.serializeTaskPane.SuspendLayout();
	        ((System.ComponentModel.ISupportInitialize)(this.serializeExpando)).BeginInit();
	        this.serializeExpando.SuspendLayout();
	        this.SuspendLayout();
	        // 
	        // systemTaskPane
	        // 
	        this.systemTaskPane.AutoScrollMargin = new System.Drawing.Size(12, 12);
	        this.systemTaskPane.Expandos.AddRange(new XPExplorerBar.Expando[] { this.pictureTasksExpando, this.fileAndFolderTasksExpando, this.otherPlacesExpando, this.detailsExpando });
	        this.systemTaskPane.Location = new System.Drawing.Point(0, 0);
	        this.systemTaskPane.Name = "systemTaskPane";
	        this.systemTaskPane.Size = new System.Drawing.Size(210, 548);
	        this.systemTaskPane.TabIndex = 0;
	        this.systemTaskPane.Text = "System TaskPane";
	        // 
	        // pictureTasksExpando
	        // 
	        this.pictureTasksExpando.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
	        this.pictureTasksExpando.AutoLayout = true;
	        this.pictureTasksExpando.ExpandedHeight = 129;
	        this.pictureTasksExpando.Font = new System.Drawing.Font("Segoe UI", 8.25F);
	        this.pictureTasksExpando.Items.AddRange(new System.Windows.Forms.Control[] { this.slideShowTaskItem, this.orderOnlineTaskItem, this.printPicturesTaskItem, this.copyToCDTaskItem });
	        this.pictureTasksExpando.Location = new System.Drawing.Point(12, 12);
	        this.pictureTasksExpando.Name = "pictureTasksExpando";
	        this.pictureTasksExpando.Size = new System.Drawing.Size(186, 129);
	        this.pictureTasksExpando.SpecialGroup = true;
	        this.pictureTasksExpando.TabIndex = 0;
	        this.pictureTasksExpando.Text = "Picture Tasks";
	        this.pictureTasksExpando.TitleImage = ((System.Drawing.Image)(resources.GetObject("pictureTasksExpando.TitleImage")));
	        // 
	        // slideShowTaskItem
	        // 
	        this.slideShowTaskItem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
	        this.slideShowTaskItem.BackColor = System.Drawing.Color.Transparent;
	        this.slideShowTaskItem.Image = ((System.Drawing.Image)(resources.GetObject("slideShowTaskItem.Image")));
	        this.slideShowTaskItem.Location = new System.Drawing.Point(12, 42);
	        this.slideShowTaskItem.Name = "slideShowTaskItem";
	        this.slideShowTaskItem.Size = new System.Drawing.Size(160, 16);
	        this.slideShowTaskItem.TabIndex = 0;
	        this.slideShowTaskItem.Text = "View as a slide show";
	        this.slideShowTaskItem.TextAlign = System.Drawing.ContentAlignment.TopLeft;
	        this.slideShowTaskItem.UseVisualStyleBackColor = false;
	        // 
	        // orderOnlineTaskItem
	        // 
	        this.orderOnlineTaskItem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
	        this.orderOnlineTaskItem.BackColor = System.Drawing.Color.Transparent;
	        this.orderOnlineTaskItem.Image = ((System.Drawing.Image)(resources.GetObject("orderOnlineTaskItem.Image")));
	        this.orderOnlineTaskItem.Location = new System.Drawing.Point(12, 62);
	        this.orderOnlineTaskItem.Name = "orderOnlineTaskItem";
	        this.orderOnlineTaskItem.Size = new System.Drawing.Size(160, 16);
	        this.orderOnlineTaskItem.TabIndex = 1;
	        this.orderOnlineTaskItem.Text = "Order prints online";
	        this.orderOnlineTaskItem.TextAlign = System.Drawing.ContentAlignment.TopLeft;
	        this.orderOnlineTaskItem.UseVisualStyleBackColor = false;
	        // 
	        // printPicturesTaskItem
	        // 
	        this.printPicturesTaskItem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
	        this.printPicturesTaskItem.BackColor = System.Drawing.Color.Transparent;
	        this.printPicturesTaskItem.Image = ((System.Drawing.Image)(resources.GetObject("printPicturesTaskItem.Image")));
	        this.printPicturesTaskItem.Location = new System.Drawing.Point(12, 82);
	        this.printPicturesTaskItem.Name = "printPicturesTaskItem";
	        this.printPicturesTaskItem.Size = new System.Drawing.Size(160, 16);
	        this.printPicturesTaskItem.TabIndex = 2;
	        this.printPicturesTaskItem.Text = "Print pictures";
	        this.printPicturesTaskItem.TextAlign = System.Drawing.ContentAlignment.TopLeft;
	        this.printPicturesTaskItem.UseVisualStyleBackColor = false;
	        // 
	        // copyToCDTaskItem
	        // 
	        this.copyToCDTaskItem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
	        this.copyToCDTaskItem.BackColor = System.Drawing.Color.Transparent;
	        this.copyToCDTaskItem.Image = ((System.Drawing.Image)(resources.GetObject("copyToCDTaskItem.Image")));
	        this.copyToCDTaskItem.Location = new System.Drawing.Point(12, 102);
	        this.copyToCDTaskItem.Name = "copyToCDTaskItem";
	        this.copyToCDTaskItem.Size = new System.Drawing.Size(160, 16);
	        this.copyToCDTaskItem.TabIndex = 3;
	        this.copyToCDTaskItem.Text = "Copy all items to CD";
	        this.copyToCDTaskItem.TextAlign = System.Drawing.ContentAlignment.TopLeft;
	        this.copyToCDTaskItem.UseVisualStyleBackColor = false;
	        // 
	        // fileAndFolderTasksExpando
	        // 
	        this.fileAndFolderTasksExpando.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
	        this.fileAndFolderTasksExpando.AutoLayout = true;
	        this.fileAndFolderTasksExpando.ExpandedHeight = 112;
	        this.fileAndFolderTasksExpando.Font = new System.Drawing.Font("Segoe UI", 8.25F);
	        this.fileAndFolderTasksExpando.Items.AddRange(new System.Windows.Forms.Control[] { this.newFolderTaskItem, this.publishToWebTaskItem, this.shareFolderTaskItem });
	        this.fileAndFolderTasksExpando.Location = new System.Drawing.Point(12, 153);
	        this.fileAndFolderTasksExpando.Name = "fileAndFolderTasksExpando";
	        this.fileAndFolderTasksExpando.Size = new System.Drawing.Size(186, 112);
	        this.fileAndFolderTasksExpando.TabIndex = 1;
	        this.fileAndFolderTasksExpando.Text = "File and Folder Tasks";
	        // 
	        // newFolderTaskItem
	        // 
	        this.newFolderTaskItem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
	        this.newFolderTaskItem.BackColor = System.Drawing.Color.Transparent;
	        this.newFolderTaskItem.Image = ((System.Drawing.Image)(resources.GetObject("newFolderTaskItem.Image")));
	        this.newFolderTaskItem.Location = new System.Drawing.Point(12, 33);
	        this.newFolderTaskItem.Name = "newFolderTaskItem";
	        this.newFolderTaskItem.Size = new System.Drawing.Size(160, 16);
	        this.newFolderTaskItem.TabIndex = 0;
	        this.newFolderTaskItem.Text = "Make a new folder";
	        this.newFolderTaskItem.TextAlign = System.Drawing.ContentAlignment.TopLeft;
	        this.newFolderTaskItem.UseVisualStyleBackColor = false;
	        // 
	        // publishToWebTaskItem
	        // 
	        this.publishToWebTaskItem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
	        this.publishToWebTaskItem.BackColor = System.Drawing.Color.Transparent;
	        this.publishToWebTaskItem.Image = ((System.Drawing.Image)(resources.GetObject("publishToWebTaskItem.Image")));
	        this.publishToWebTaskItem.Location = new System.Drawing.Point(12, 53);
	        this.publishToWebTaskItem.Name = "publishToWebTaskItem";
	        this.publishToWebTaskItem.Size = new System.Drawing.Size(160, 28);
	        this.publishToWebTaskItem.TabIndex = 1;
	        this.publishToWebTaskItem.Text = "Publish this folder to the Web";
	        this.publishToWebTaskItem.TextAlign = System.Drawing.ContentAlignment.TopLeft;
	        this.publishToWebTaskItem.UseVisualStyleBackColor = false;
	        // 
	        // shareFolderTaskItem
	        // 
	        this.shareFolderTaskItem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
	        this.shareFolderTaskItem.BackColor = System.Drawing.Color.Transparent;
	        this.shareFolderTaskItem.Image = ((System.Drawing.Image)(resources.GetObject("shareFolderTaskItem.Image")));
	        this.shareFolderTaskItem.Location = new System.Drawing.Point(12, 85);
	        this.shareFolderTaskItem.Name = "shareFolderTaskItem";
	        this.shareFolderTaskItem.Size = new System.Drawing.Size(160, 16);
	        this.shareFolderTaskItem.TabIndex = 2;
	        this.shareFolderTaskItem.Text = "Share this folder";
	        this.shareFolderTaskItem.TextAlign = System.Drawing.ContentAlignment.TopLeft;
	        this.shareFolderTaskItem.UseVisualStyleBackColor = false;
	        // 
	        // otherPlacesExpando
	        // 
	        this.otherPlacesExpando.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
	        this.otherPlacesExpando.AutoLayout = true;
	        this.otherPlacesExpando.ExpandedHeight = 120;
	        this.otherPlacesExpando.Font = new System.Drawing.Font("Segoe UI", 8.25F);
	        this.otherPlacesExpando.Items.AddRange(new System.Windows.Forms.Control[] { this.myDocumentsTaskItem, this.myPicturesTaskItem, this.myComputerTaskItem, this.myNetworkPlacesTaskItem });
	        this.otherPlacesExpando.Location = new System.Drawing.Point(12, 277);
	        this.otherPlacesExpando.Name = "otherPlacesExpando";
	        this.otherPlacesExpando.Size = new System.Drawing.Size(186, 120);
	        this.otherPlacesExpando.TabIndex = 2;
	        this.otherPlacesExpando.Text = "Other Places";
	        // 
	        // myDocumentsTaskItem
	        // 
	        this.myDocumentsTaskItem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
	        this.myDocumentsTaskItem.BackColor = System.Drawing.Color.Transparent;
	        this.myDocumentsTaskItem.Image = ((System.Drawing.Image)(resources.GetObject("myDocumentsTaskItem.Image")));
	        this.myDocumentsTaskItem.Location = new System.Drawing.Point(12, 33);
	        this.myDocumentsTaskItem.Name = "myDocumentsTaskItem";
	        this.myDocumentsTaskItem.Size = new System.Drawing.Size(160, 16);
	        this.myDocumentsTaskItem.TabIndex = 0;
	        this.myDocumentsTaskItem.Text = "My Documents";
	        this.myDocumentsTaskItem.TextAlign = System.Drawing.ContentAlignment.TopLeft;
	        this.myDocumentsTaskItem.UseVisualStyleBackColor = false;
	        // 
	        // myPicturesTaskItem
	        // 
	        this.myPicturesTaskItem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
	        this.myPicturesTaskItem.BackColor = System.Drawing.Color.Transparent;
	        this.myPicturesTaskItem.Image = ((System.Drawing.Image)(resources.GetObject("myPicturesTaskItem.Image")));
	        this.myPicturesTaskItem.Location = new System.Drawing.Point(12, 53);
	        this.myPicturesTaskItem.Name = "myPicturesTaskItem";
	        this.myPicturesTaskItem.Size = new System.Drawing.Size(160, 16);
	        this.myPicturesTaskItem.TabIndex = 1;
	        this.myPicturesTaskItem.Text = "Shared Pictures";
	        this.myPicturesTaskItem.TextAlign = System.Drawing.ContentAlignment.TopLeft;
	        this.myPicturesTaskItem.UseVisualStyleBackColor = false;
	        // 
	        // myComputerTaskItem
	        // 
	        this.myComputerTaskItem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
	        this.myComputerTaskItem.BackColor = System.Drawing.Color.Transparent;
	        this.myComputerTaskItem.Image = ((System.Drawing.Image)(resources.GetObject("myComputerTaskItem.Image")));
	        this.myComputerTaskItem.Location = new System.Drawing.Point(12, 73);
	        this.myComputerTaskItem.Name = "myComputerTaskItem";
	        this.myComputerTaskItem.Size = new System.Drawing.Size(160, 16);
	        this.myComputerTaskItem.TabIndex = 2;
	        this.myComputerTaskItem.Text = "My Computer";
	        this.myComputerTaskItem.TextAlign = System.Drawing.ContentAlignment.TopLeft;
	        this.myComputerTaskItem.UseVisualStyleBackColor = false;
	        // 
	        // myNetworkPlacesTaskItem
	        // 
	        this.myNetworkPlacesTaskItem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
	        this.myNetworkPlacesTaskItem.BackColor = System.Drawing.Color.Transparent;
	        this.myNetworkPlacesTaskItem.Image = ((System.Drawing.Image)(resources.GetObject("myNetworkPlacesTaskItem.Image")));
	        this.myNetworkPlacesTaskItem.Location = new System.Drawing.Point(12, 93);
	        this.myNetworkPlacesTaskItem.Name = "myNetworkPlacesTaskItem";
	        this.myNetworkPlacesTaskItem.Size = new System.Drawing.Size(160, 16);
	        this.myNetworkPlacesTaskItem.TabIndex = 3;
	        this.myNetworkPlacesTaskItem.Text = "My Network Places";
	        this.myNetworkPlacesTaskItem.TextAlign = System.Drawing.ContentAlignment.TopLeft;
	        this.myNetworkPlacesTaskItem.UseVisualStyleBackColor = false;
	        // 
	        // detailsExpando
	        // 
	        this.detailsExpando.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
	        this.detailsExpando.ExpandedHeight = 106;
	        this.detailsExpando.Font = new System.Drawing.Font("Segoe UI", 8.25F);
	        this.detailsExpando.Items.AddRange(new System.Windows.Forms.Control[] { this.label1, this.label2, this.label3 });
	        this.detailsExpando.Location = new System.Drawing.Point(12, 409);
	        this.detailsExpando.Name = "detailsExpando";
	        this.detailsExpando.Size = new System.Drawing.Size(186, 106);
	        this.detailsExpando.TabIndex = 3;
	        this.detailsExpando.Text = "Details";
	        // 
	        // label1
	        // 
	        this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
	        this.label1.BackColor = System.Drawing.Color.Transparent;
	        this.label1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
	        this.label1.Location = new System.Drawing.Point(12, 33);
	        this.label1.Name = "label1";
	        this.label1.Size = new System.Drawing.Size(160, 14);
	        this.label1.TabIndex = 0;
	        this.label1.Text = "My Pictures";
	        // 
	        // label2
	        // 
	        this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
	        this.label2.BackColor = System.Drawing.Color.Transparent;
	        this.label2.Location = new System.Drawing.Point(12, 47);
	        this.label2.Name = "label2";
	        this.label2.Size = new System.Drawing.Size(160, 14);
	        this.label2.TabIndex = 1;
	        this.label2.Text = "File Folder";
	        // 
	        // label3
	        // 
	        this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
	        this.label3.BackColor = System.Drawing.Color.Transparent;
	        this.label3.Location = new System.Drawing.Point(12, 67);
	        this.label3.Name = "label3";
	        this.label3.Size = new System.Drawing.Size(160, 28);
	        this.label3.TabIndex = 2;
	        this.label3.Text = "Date Modified: Friday, 15th October 2004, 10:29 PM";
	        // 
	        // customGroupBox
	        // 
	        this.customGroupBox.Controls.Add(this.controlComboBox);
	        this.customGroupBox.Controls.Add(this.customPropertyGrid);
	        this.customGroupBox.Controls.Add(this.customTaskPane);
	        this.customGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
	        this.customGroupBox.Location = new System.Drawing.Point(232, 4);
	        this.customGroupBox.Name = "customGroupBox";
	        this.customGroupBox.Size = new System.Drawing.Size(248, 536);
	        this.customGroupBox.TabIndex = 1;
	        this.customGroupBox.TabStop = false;
	        this.customGroupBox.Text = "Custom Settings";
	        // 
	        // controlComboBox
	        // 
	        this.controlComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
	        this.controlComboBox.Items.AddRange(new object[] { "TaskPane", "Expando", "TaskItem1", "TaskItem2", "TaskItem3" });
	        this.controlComboBox.Location = new System.Drawing.Point(8, 20);
	        this.controlComboBox.Name = "controlComboBox";
	        this.controlComboBox.Size = new System.Drawing.Size(232, 21);
	        this.controlComboBox.TabIndex = 1;
	        this.controlComboBox.SelectedIndexChanged += new System.EventHandler(this.controlComboBox_SelectedIndexChanged);
	        // 
	        // customPropertyGrid
	        // 
	        this.customPropertyGrid.LineColor = System.Drawing.SystemColors.ScrollBar;
	        this.customPropertyGrid.Location = new System.Drawing.Point(8, 48);
	        this.customPropertyGrid.Name = "customPropertyGrid";
	        this.customPropertyGrid.Size = new System.Drawing.Size(232, 264);
	        this.customPropertyGrid.TabIndex = 2;
	        // 
	        // customTaskPane
	        // 
	        this.customTaskPane.AutoScrollMargin = new System.Drawing.Size(12, 12);
	        this.customTaskPane.Expandos.AddRange(new XPExplorerBar.Expando[] { this.customExpando });
	        this.customTaskPane.Location = new System.Drawing.Point(8, 318);
	        this.customTaskPane.Name = "customTaskPane";
	        this.customTaskPane.Size = new System.Drawing.Size(232, 210);
	        this.customTaskPane.TabIndex = 0;
	        this.customTaskPane.Text = "Custom TaskPane";
	        // 
	        // customExpando
	        // 
	        this.customExpando.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
	        this.customExpando.ExpandedHeight = 186;
	        this.customExpando.Font = new System.Drawing.Font("Segoe UI", 8.25F);
	        this.customExpando.Items.AddRange(new System.Windows.Forms.Control[] { this.customTaskItem1, this.customTaskItem2, this.customTaskItem3, this.customButton, this.customTextBox, this.customComboBox, this.customCheckBox, this.customRadioButton });
	        this.customExpando.Location = new System.Drawing.Point(12, 12);
	        this.customExpando.Name = "customExpando";
	        this.customExpando.Size = new System.Drawing.Size(208, 186);
	        this.customExpando.TabIndex = 0;
	        this.customExpando.Text = "Expando";
	        // 
	        // customTaskItem1
	        // 
	        this.customTaskItem1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
	        this.customTaskItem1.BackColor = System.Drawing.Color.Transparent;
	        this.customTaskItem1.Image = null;
	        this.customTaskItem1.Location = new System.Drawing.Point(12, 33);
	        this.customTaskItem1.Name = "customTaskItem1";
	        this.customTaskItem1.Size = new System.Drawing.Size(182, 16);
	        this.customTaskItem1.TabIndex = 0;
	        this.customTaskItem1.Text = "TaskItem1";
	        this.customTaskItem1.TextAlign = System.Drawing.ContentAlignment.TopLeft;
	        this.customTaskItem1.UseVisualStyleBackColor = false;
	        // 
	        // customTaskItem2
	        // 
	        this.customTaskItem2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
	        this.customTaskItem2.BackColor = System.Drawing.Color.Transparent;
	        this.customTaskItem2.Image = null;
	        this.customTaskItem2.Location = new System.Drawing.Point(12, 53);
	        this.customTaskItem2.Name = "customTaskItem2";
	        this.customTaskItem2.Size = new System.Drawing.Size(182, 16);
	        this.customTaskItem2.TabIndex = 1;
	        this.customTaskItem2.Text = "TaskItem2";
	        this.customTaskItem2.TextAlign = System.Drawing.ContentAlignment.TopLeft;
	        this.customTaskItem2.UseVisualStyleBackColor = false;
	        // 
	        // customTaskItem3
	        // 
	        this.customTaskItem3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
	        this.customTaskItem3.BackColor = System.Drawing.Color.Transparent;
	        this.customTaskItem3.Image = null;
	        this.customTaskItem3.Location = new System.Drawing.Point(12, 73);
	        this.customTaskItem3.Name = "customTaskItem3";
	        this.customTaskItem3.Size = new System.Drawing.Size(182, 16);
	        this.customTaskItem3.TabIndex = 2;
	        this.customTaskItem3.Text = "TaskItem3";
	        this.customTaskItem3.TextAlign = System.Drawing.ContentAlignment.TopLeft;
	        this.customTaskItem3.UseVisualStyleBackColor = false;
	        // 
	        // customButton
	        // 
	        this.customButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
	        this.customButton.Location = new System.Drawing.Point(12, 94);
	        this.customButton.Name = "customButton";
	        this.customButton.Size = new System.Drawing.Size(88, 23);
	        this.customButton.TabIndex = 3;
	        this.customButton.Text = "Button";
	        // 
	        // customTextBox
	        // 
	        this.customTextBox.Location = new System.Drawing.Point(108, 96);
	        this.customTextBox.Name = "customTextBox";
	        this.customTextBox.Size = new System.Drawing.Size(88, 21);
	        this.customTextBox.TabIndex = 4;
	        this.customTextBox.Text = "TextBox";
	        // 
	        // customComboBox
	        // 
	        this.customComboBox.Items.AddRange(new object[] { "Item1", "Item2", "Item3", "Item4" });
	        this.customComboBox.Location = new System.Drawing.Point(48, 128);
	        this.customComboBox.Name = "customComboBox";
	        this.customComboBox.Size = new System.Drawing.Size(121, 21);
	        this.customComboBox.TabIndex = 5;
	        this.customComboBox.Text = "ComboBox";
	        // 
	        // customCheckBox
	        // 
	        this.customCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
	        this.customCheckBox.Location = new System.Drawing.Point(12, 160);
	        this.customCheckBox.Name = "customCheckBox";
	        this.customCheckBox.Size = new System.Drawing.Size(84, 16);
	        this.customCheckBox.TabIndex = 6;
	        this.customCheckBox.Text = "CheckBox";
	        // 
	        // customRadioButton
	        // 
	        this.customRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
	        this.customRadioButton.Location = new System.Drawing.Point(108, 160);
	        this.customRadioButton.Name = "customRadioButton";
	        this.customRadioButton.Size = new System.Drawing.Size(88, 16);
	        this.customRadioButton.TabIndex = 7;
	        this.customRadioButton.Text = "RadioButton";
	        // 
	        // menubar
	        // 
	        this.menubar.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] { this.fileMenu, this.viewMenu });
	        // 
	        // fileMenu
	        // 
	        this.fileMenu.Index = 0;
	        this.fileMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] { this.exitMenuItem });
	        this.fileMenu.Text = "&File";
	        // 
	        // exitMenuItem
	        // 
	        this.exitMenuItem.Index = 0;
	        this.exitMenuItem.Text = "E&xit";
	        this.exitMenuItem.Click += new System.EventHandler(this.exitMenuItem_Click);
	        // 
	        // viewMenu
	        // 
	        this.viewMenu.Index = 1;
	        this.viewMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] { this.animateMenuItem, this.themesMenuItem, this.cycleMenuItem, this.expandoDraggingMenuItem, this.separatorMenuItem1, this.showFocusMenuItem, this.separatorMenuItem2, this.myPicturesMenuItem, this.myComputerMenuItem, this.myNetworkMenuItem });
	        this.viewMenu.Text = "&View";
	        // 
	        // animateMenuItem
	        // 
	        this.animateMenuItem.Index = 0;
	        this.animateMenuItem.Text = "&Animate";
	        this.animateMenuItem.Click += new System.EventHandler(this.animateMenuItem_Click);
	        // 
	        // themesMenuItem
	        // 
	        this.themesMenuItem.Index = 1;
	        this.themesMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] { this.classicMenuItem, this.blueMenuItem, this.xboxMenuItem, this.itunesMenuItem, this.pantherMenuItem, this.bwMenuItem, this.defaultMenuItem });
	        this.themesMenuItem.Text = "&Themes";
	        // 
	        // classicMenuItem
	        // 
	        this.classicMenuItem.Index = 0;
	        this.classicMenuItem.Text = "&Classic";
	        this.classicMenuItem.Click += new System.EventHandler(this.classicMenuItem_Click);
	        // 
	        // blueMenuItem
	        // 
	        this.blueMenuItem.Index = 1;
	        this.blueMenuItem.Text = "C&ustom (Forever Blue)";
	        this.blueMenuItem.Click += new System.EventHandler(this.blueMenuItem_Click);
	        // 
	        // xboxMenuItem
	        // 
	        this.xboxMenuItem.Index = 2;
	        this.xboxMenuItem.Text = "Custom (&XBox)";
	        this.xboxMenuItem.Click += new System.EventHandler(this.xboxMenuItem_Click);
	        // 
	        // itunesMenuItem
	        // 
	        this.itunesMenuItem.Index = 3;
	        this.itunesMenuItem.Text = "Custom (&iTunes)";
	        this.itunesMenuItem.Click += new System.EventHandler(this.itunesMenuItem_Click);
	        // 
	        // pantherMenuItem
	        // 
	        this.pantherMenuItem.Index = 4;
	        this.pantherMenuItem.Text = "Custom (&Panther)";
	        this.pantherMenuItem.Click += new System.EventHandler(this.pantherMenuItem_Click);
	        // 
	        // bwMenuItem
	        // 
	        this.bwMenuItem.Index = 5;
	        this.bwMenuItem.Text = "Custom (&BW)";
	        this.bwMenuItem.Click += new System.EventHandler(this.bwMenuItem_Click);
	        // 
	        // defaultMenuItem
	        // 
	        this.defaultMenuItem.Checked = true;
	        this.defaultMenuItem.Index = 6;
	        this.defaultMenuItem.Text = "&Default";
	        this.defaultMenuItem.Click += new System.EventHandler(this.defaultMenuItem_Click);
	        // 
	        // cycleMenuItem
	        // 
	        this.cycleMenuItem.Index = 2;
	        this.cycleMenuItem.Text = "C&ycle Expandos";
	        this.cycleMenuItem.Click += new System.EventHandler(this.cycleMenuItem_Click);
	        // 
	        // expandoDraggingMenuItem
	        // 
	        this.expandoDraggingMenuItem.Index = 3;
	        this.expandoDraggingMenuItem.Text = "Enable Expando &Dragging";
	        this.expandoDraggingMenuItem.Click += new System.EventHandler(this.expandoDraggingMenuItem_Click);
	        // 
	        // separatorMenuItem1
	        // 
	        this.separatorMenuItem1.Index = 4;
	        this.separatorMenuItem1.Text = "-";
	        // 
	        // showFocusMenuItem
	        // 
	        this.showFocusMenuItem.Index = 5;
	        this.showFocusMenuItem.Text = "Show &Focus Cues";
	        this.showFocusMenuItem.Click += new System.EventHandler(this.showFocusMenuItem_Click);
	        // 
	        // separatorMenuItem2
	        // 
	        this.separatorMenuItem2.Index = 6;
	        this.separatorMenuItem2.Text = "-";
	        // 
	        // myPicturesMenuItem
	        // 
	        this.myPicturesMenuItem.Checked = true;
	        this.myPicturesMenuItem.Index = 7;
	        this.myPicturesMenuItem.Text = "Show My &Pictures";
	        this.myPicturesMenuItem.Click += new System.EventHandler(this.myPicturesMenuItem_Click);
	        // 
	        // myComputerMenuItem
	        // 
	        this.myComputerMenuItem.Checked = true;
	        this.myComputerMenuItem.Index = 8;
	        this.myComputerMenuItem.Text = "Show My &Computer";
	        this.myComputerMenuItem.Click += new System.EventHandler(this.myComputerMenuItem_Click);
	        // 
	        // myNetworkMenuItem
	        // 
	        this.myNetworkMenuItem.Checked = true;
	        this.myNetworkMenuItem.Index = 9;
	        this.myNetworkMenuItem.Text = "Show My &Network Places";
	        this.myNetworkMenuItem.Click += new System.EventHandler(this.myNetworkMenuItem_Click);
	        // 
	        // serializeGroupBox
	        // 
	        this.serializeGroupBox.Controls.Add(this.removeButton);
	        this.serializeGroupBox.Controls.Add(this.label5);
	        this.serializeGroupBox.Controls.Add(this.label4);
	        this.serializeGroupBox.Controls.Add(this.serializeMemoryButton);
	        this.serializeGroupBox.Controls.Add(this.deserializeFileButton);
	        this.serializeGroupBox.Controls.Add(this.serializeFileButton);
	        this.serializeGroupBox.Controls.Add(this.serializeTaskPane);
	        this.serializeGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
	        this.serializeGroupBox.Location = new System.Drawing.Point(504, 4);
	        this.serializeGroupBox.Name = "serializeGroupBox";
	        this.serializeGroupBox.Size = new System.Drawing.Size(226, 536);
	        this.serializeGroupBox.TabIndex = 2;
	        this.serializeGroupBox.TabStop = false;
	        this.serializeGroupBox.Text = "Serialization";
	        // 
	        // removeButton
	        // 
	        this.removeButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
	        this.removeButton.Location = new System.Drawing.Point(8, 504);
	        this.removeButton.Name = "removeButton";
	        this.removeButton.Size = new System.Drawing.Size(210, 23);
	        this.removeButton.TabIndex = 7;
	        this.removeButton.Text = "Remove";
	        this.removeButton.Click += new System.EventHandler(this.removeButton_Click);
	        // 
	        // label5
	        // 
	        this.label5.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
	        this.label5.Location = new System.Drawing.Point(8, 280);
	        this.label5.Name = "label5";
	        this.label5.Size = new System.Drawing.Size(208, 16);
	        this.label5.TabIndex = 6;
	        this.label5.Text = "Memory Serialization";
	        // 
	        // label4
	        // 
	        this.label4.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
	        this.label4.Location = new System.Drawing.Point(8, 184);
	        this.label4.Name = "label4";
	        this.label4.Size = new System.Drawing.Size(208, 16);
	        this.label4.TabIndex = 5;
	        this.label4.Text = "File Serialization";
	        // 
	        // serializeMemoryButton
	        // 
	        this.serializeMemoryButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
	        this.serializeMemoryButton.Location = new System.Drawing.Point(8, 304);
	        this.serializeMemoryButton.Name = "serializeMemoryButton";
	        this.serializeMemoryButton.Size = new System.Drawing.Size(210, 23);
	        this.serializeMemoryButton.TabIndex = 3;
	        this.serializeMemoryButton.Text = "Serialize To Memory";
	        this.serializeMemoryButton.Click += new System.EventHandler(this.serializeMemoryButton_Click);
	        // 
	        // deserializeFileButton
	        // 
	        this.deserializeFileButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
	        this.deserializeFileButton.Location = new System.Drawing.Point(8, 240);
	        this.deserializeFileButton.Name = "deserializeFileButton";
	        this.deserializeFileButton.Size = new System.Drawing.Size(210, 23);
	        this.deserializeFileButton.TabIndex = 2;
	        this.deserializeFileButton.Text = "Deserialize From File";
	        this.deserializeFileButton.Click += new System.EventHandler(this.deserializeFileButton_Click);
	        // 
	        // serializeFileButton
	        // 
	        this.serializeFileButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
	        this.serializeFileButton.Location = new System.Drawing.Point(8, 208);
	        this.serializeFileButton.Name = "serializeFileButton";
	        this.serializeFileButton.Size = new System.Drawing.Size(210, 23);
	        this.serializeFileButton.TabIndex = 1;
	        this.serializeFileButton.Text = "Serialize To File";
	        this.serializeFileButton.Click += new System.EventHandler(this.serializeFileButton_Click);
	        // 
	        // serializeTaskPane
	        // 
	        this.serializeTaskPane.AutoScrollMargin = new System.Drawing.Size(12, 12);
	        this.serializeTaskPane.CustomSettings.GradientEndColor = System.Drawing.Color.Magenta;
	        this.serializeTaskPane.CustomSettings.GradientStartColor = System.Drawing.Color.Aqua;
	        this.serializeTaskPane.CustomSettings.Watermark = ((System.Drawing.Image)(resources.GetObject("resource.Watermark")));
	        this.serializeTaskPane.CustomSettings.WatermarkAlignment = System.Drawing.ContentAlignment.BottomRight;
	        this.serializeTaskPane.Expandos.AddRange(new XPExplorerBar.Expando[] { this.serializeExpando });
	        this.serializeTaskPane.Location = new System.Drawing.Point(8, 20);
	        this.serializeTaskPane.Name = "serializeTaskPane";
	        this.serializeTaskPane.Size = new System.Drawing.Size(210, 148);
	        this.serializeTaskPane.TabIndex = 0;
	        this.serializeTaskPane.Text = "TaskPane";
	        // 
	        // serializeExpando
	        // 
	        this.serializeExpando.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
	        this.serializeExpando.Animate = true;
	        this.serializeExpando.AutoLayout = true;
	        this.serializeExpando.CustomHeaderSettings.SpecialGradientEndColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
	        this.serializeExpando.CustomHeaderSettings.SpecialGradientStartColor = System.Drawing.Color.Black;
	        this.serializeExpando.CustomHeaderSettings.TitleFont = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
	        this.serializeExpando.CustomHeaderSettings.TitleGradient = true;
	        this.serializeExpando.CustomHeaderSettings.TitleRadius = 5;
	        this.serializeExpando.CustomSettings.SpecialBackColor = System.Drawing.Color.WhiteSmoke;
	        this.serializeExpando.CustomSettings.SpecialBorderColor = System.Drawing.Color.Black;
	        this.serializeExpando.ExpandedHeight = 109;
	        this.serializeExpando.Font = new System.Drawing.Font("Segoe UI", 8.25F);
	        this.serializeExpando.Items.AddRange(new System.Windows.Forms.Control[] { this.serializeTaskItem1, this.serializeTaskItem2, this.serializeTaskItem3 });
	        this.serializeExpando.Location = new System.Drawing.Point(12, 12);
	        this.serializeExpando.Name = "serializeExpando";
	        this.serializeExpando.Size = new System.Drawing.Size(186, 109);
	        this.serializeExpando.SpecialGroup = true;
	        this.serializeExpando.TabIndex = 0;
	        this.serializeExpando.Text = "Expando";
	        this.serializeExpando.TitleImage = ((System.Drawing.Image)(resources.GetObject("serializeExpando.TitleImage")));
	        this.serializeExpando.Watermark = ((System.Drawing.Image)(resources.GetObject("serializeExpando.Watermark")));
	        // 
	        // serializeTaskItem1
	        // 
	        this.serializeTaskItem1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
	        this.serializeTaskItem1.BackColor = System.Drawing.Color.Transparent;
	        this.serializeTaskItem1.Image = ((System.Drawing.Image)(resources.GetObject("serializeTaskItem1.Image")));
	        this.serializeTaskItem1.Location = new System.Drawing.Point(12, 42);
	        this.serializeTaskItem1.Name = "serializeTaskItem1";
	        this.serializeTaskItem1.Size = new System.Drawing.Size(160, 16);
	        this.serializeTaskItem1.TabIndex = 0;
	        this.serializeTaskItem1.Text = "TaskItem1";
	        this.serializeTaskItem1.TextAlign = System.Drawing.ContentAlignment.TopLeft;
	        this.serializeTaskItem1.UseVisualStyleBackColor = false;
	        // 
	        // serializeTaskItem2
	        // 
	        this.serializeTaskItem2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
	        this.serializeTaskItem2.BackColor = System.Drawing.Color.Transparent;
	        this.serializeTaskItem2.CustomSettings.HotLinkColor = System.Drawing.Color.Red;
	        this.serializeTaskItem2.CustomSettings.LinkColor = System.Drawing.Color.Blue;
	        this.serializeTaskItem2.Image = null;
	        this.serializeTaskItem2.Location = new System.Drawing.Point(12, 62);
	        this.serializeTaskItem2.Name = "serializeTaskItem2";
	        this.serializeTaskItem2.Size = new System.Drawing.Size(160, 16);
	        this.serializeTaskItem2.TabIndex = 1;
	        this.serializeTaskItem2.Text = "TaskItem2";
	        this.serializeTaskItem2.TextAlign = System.Drawing.ContentAlignment.TopLeft;
	        this.serializeTaskItem2.UseVisualStyleBackColor = false;
	        // 
	        // serializeTaskItem3
	        // 
	        this.serializeTaskItem3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
	        this.serializeTaskItem3.BackColor = System.Drawing.Color.Transparent;
	        this.serializeTaskItem3.CustomSettings.FontDecoration = System.Drawing.FontStyle.Bold;
	        this.serializeTaskItem3.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
	        this.serializeTaskItem3.Image = null;
	        this.serializeTaskItem3.Location = new System.Drawing.Point(12, 82);
	        this.serializeTaskItem3.Name = "serializeTaskItem3";
	        this.serializeTaskItem3.Size = new System.Drawing.Size(160, 16);
	        this.serializeTaskItem3.TabIndex = 2;
	        this.serializeTaskItem3.Text = "TaskItem3";
	        this.serializeTaskItem3.TextAlign = System.Drawing.ContentAlignment.TopLeft;
	        this.serializeTaskItem3.UseVisualStyleBackColor = false;
	        // 
	        // DemoForm
	        // 
	        this.AutoScaleBaseSize = new System.Drawing.Size(5, 15);
	        this.ClientSize = new System.Drawing.Size(736, 588);
	        this.Controls.Add(this.serializeGroupBox);
	        this.Controls.Add(this.customGroupBox);
	        this.Controls.Add(this.systemTaskPane);
	        this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
	        this.Menu = this.menubar;
	        this.Name = "DemoForm";
	        this.Text = "XPEplorerBar Demo";
	        ((System.ComponentModel.ISupportInitialize)(this.systemTaskPane)).EndInit();
	        this.systemTaskPane.ResumeLayout(false);
	        ((System.ComponentModel.ISupportInitialize)(this.pictureTasksExpando)).EndInit();
	        this.pictureTasksExpando.ResumeLayout(false);
	        ((System.ComponentModel.ISupportInitialize)(this.fileAndFolderTasksExpando)).EndInit();
	        this.fileAndFolderTasksExpando.ResumeLayout(false);
	        ((System.ComponentModel.ISupportInitialize)(this.otherPlacesExpando)).EndInit();
	        this.otherPlacesExpando.ResumeLayout(false);
	        ((System.ComponentModel.ISupportInitialize)(this.detailsExpando)).EndInit();
	        this.detailsExpando.ResumeLayout(false);
	        this.customGroupBox.ResumeLayout(false);
	        ((System.ComponentModel.ISupportInitialize)(this.customTaskPane)).EndInit();
	        this.customTaskPane.ResumeLayout(false);
	        ((System.ComponentModel.ISupportInitialize)(this.customExpando)).EndInit();
	        this.customExpando.ResumeLayout(false);
	        this.customExpando.PerformLayout();
	        this.serializeGroupBox.ResumeLayout(false);
	        ((System.ComponentModel.ISupportInitialize)(this.serializeTaskPane)).EndInit();
	        this.serializeTaskPane.ResumeLayout(false);
	        ((System.ComponentModel.ISupportInitialize)(this.serializeExpando)).EndInit();
	        this.serializeExpando.ResumeLayout(false);
	        this.ResumeLayout(false);
        }

		private XPExplorerBar.TaskPane systemTaskPane;
		private XPExplorerBar.Expando pictureTasksExpando;
		private XPExplorerBar.Expando fileAndFolderTasksExpando;
		private XPExplorerBar.Expando otherPlacesExpando;
		private XPExplorerBar.Expando detailsExpando;
		private XPExplorerBar.TaskItem slideShowTaskItem;
		private XPExplorerBar.TaskItem orderOnlineTaskItem;
		private XPExplorerBar.TaskItem printPicturesTaskItem;
		private XPExplorerBar.TaskItem copyToCDTaskItem;
		private XPExplorerBar.TaskItem newFolderTaskItem;
		private XPExplorerBar.TaskItem publishToWebTaskItem;
		private XPExplorerBar.TaskItem shareFolderTaskItem;
		private XPExplorerBar.TaskItem myDocumentsTaskItem;
		private XPExplorerBar.TaskItem myPicturesTaskItem;
		private XPExplorerBar.TaskItem myComputerTaskItem;
		private XPExplorerBar.TaskItem myNetworkPlacesTaskItem;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.GroupBox customGroupBox;
		private System.Windows.Forms.ComboBox controlComboBox;
		private System.Windows.Forms.PropertyGrid customPropertyGrid;
		private XPExplorerBar.TaskPane customTaskPane;
		private XPExplorerBar.Expando customExpando;
		private XPExplorerBar.TaskItem customTaskItem1;
		private XPExplorerBar.TaskItem customTaskItem2;
		private XPExplorerBar.TaskItem customTaskItem3;
		private TaskPaneDescriptor customTaskPaneDescriptor;
		private ExpandoDescriptor customExpandoDescriptor;
		private TaskItemDescriptor customTaskItem1Descriptor;
		private TaskItemDescriptor customTaskItem2Descriptor;
		private TaskItemDescriptor customTaskItem3Descriptor;
		private System.Windows.Forms.Button customButton;
		private XPExplorerBar.XPTextBox customTextBox;
		private System.Windows.Forms.ComboBox customComboBox;
		private System.Windows.Forms.CheckBox customCheckBox;
		private System.Windows.Forms.RadioButton customRadioButton;
		private System.Windows.Forms.MainMenu menubar;
		private System.Windows.Forms.MenuItem fileMenu;
		private System.Windows.Forms.MenuItem viewMenu;
		private System.Windows.Forms.MenuItem exitMenuItem;
		private System.Windows.Forms.MenuItem animateMenuItem;
		private System.Windows.Forms.MenuItem themesMenuItem;
		private System.Windows.Forms.MenuItem classicMenuItem;
		private System.Windows.Forms.MenuItem defaultMenuItem;
		private System.Windows.Forms.MenuItem blueMenuItem;
		private System.Windows.Forms.MenuItem xboxMenuItem;
		private System.Windows.Forms.MenuItem itunesMenuItem;
		private System.Windows.Forms.MenuItem pantherMenuItem;
		private System.Windows.Forms.MenuItem bwMenuItem;
		private System.Windows.Forms.MenuItem myPicturesMenuItem;
		private System.Windows.Forms.MenuItem myComputerMenuItem;
		private System.Windows.Forms.MenuItem myNetworkMenuItem;
		private System.Windows.Forms.MenuItem cycleMenuItem;
		private System.Windows.Forms.MenuItem separatorMenuItem2;
		private System.Windows.Forms.MenuItem separatorMenuItem1;
		private System.Windows.Forms.GroupBox serializeGroupBox;
		private System.Windows.Forms.Button serializeFileButton;
		private System.Windows.Forms.Button deserializeFileButton;
		private System.Windows.Forms.Button serializeMemoryButton;
		private XPExplorerBar.Expando serializeExpando;
		private XPExplorerBar.TaskItem serializeTaskItem1;
		private XPExplorerBar.TaskItem serializeTaskItem2;
		private XPExplorerBar.TaskItem serializeTaskItem3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Button removeButton;
		private XPExplorerBar.TaskPane serializeTaskPane;
		private System.Windows.Forms.MenuItem expandoDraggingMenuItem;
		private System.Windows.Forms.MenuItem showFocusMenuItem;

		#endregion
    }
}