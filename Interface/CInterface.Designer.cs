namespace Interface
{
    partial class CInterface
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CInterface));
            this.butImport = new System.Windows.Forms.Button();
            this.tbPath = new System.Windows.Forms.TextBox();
            this.butGet = new System.Windows.Forms.Button();
            this.cbPorts = new System.Windows.Forms.ComboBox();
            this.butConnection = new System.Windows.Forms.Button();
            this.butDisconnect = new System.Windows.Forms.Button();
            this.butBrowse = new System.Windows.Forms.Button();
            this.butExport = new System.Windows.Forms.Button();
            this.gbConnection = new System.Windows.Forms.GroupBox();
            this.butRefresh = new System.Windows.Forms.Button();
            this.gbImportExport = new System.Windows.Forms.GroupBox();
            this.butClear = new System.Windows.Forms.Button();
            this.butSend = new System.Windows.Forms.Button();
            this.gbRecords = new System.Windows.Forms.GroupBox();
            this.pbProgress = new System.Windows.Forms.ProgressBar();
            this.dgvRecords = new System.Windows.Forms.DataGridView();
            this.Record = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tmProgressBar = new System.Windows.Forms.Timer(this.components);
            this.tmCOMVerification = new System.Windows.Forms.Timer(this.components);
            this.tmSerialCommunication = new System.Windows.Forms.Timer(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pbIUT = new System.Windows.Forms.PictureBox();
            this.gbConnection.SuspendLayout();
            this.gbImportExport.SuspendLayout();
            this.gbRecords.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecords)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbIUT)).BeginInit();
            this.SuspendLayout();
            // 
            // butImport
            // 
            this.butImport.Enabled = false;
            this.butImport.Location = new System.Drawing.Point(193, 18);
            this.butImport.Name = "butImport";
            this.butImport.Size = new System.Drawing.Size(86, 22);
            this.butImport.TabIndex = 1;
            this.butImport.Text = "Import";
            this.butImport.UseVisualStyleBackColor = true;
            this.butImport.Click += new System.EventHandler(this.ImportFile);
            // 
            // tbPath
            // 
            this.tbPath.Location = new System.Drawing.Point(9, 19);
            this.tbPath.Name = "tbPath";
            this.tbPath.Size = new System.Drawing.Size(156, 20);
            this.tbPath.TabIndex = 2;
            this.tbPath.TextChanged += new System.EventHandler(this.UpdateImportButton);
            // 
            // butGet
            // 
            this.butGet.Enabled = false;
            this.butGet.Location = new System.Drawing.Point(9, 45);
            this.butGet.Name = "butGet";
            this.butGet.Size = new System.Drawing.Size(130, 23);
            this.butGet.TabIndex = 4;
            this.butGet.Text = "Import from serial";
            this.butGet.UseVisualStyleBackColor = true;
            this.butGet.Click += new System.EventHandler(this.GetData);
            // 
            // cbPorts
            // 
            this.cbPorts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPorts.FormattingEnabled = true;
            this.cbPorts.Location = new System.Drawing.Point(10, 19);
            this.cbPorts.Name = "cbPorts";
            this.cbPorts.Size = new System.Drawing.Size(237, 21);
            this.cbPorts.TabIndex = 5;
            this.cbPorts.SelectedIndexChanged += new System.EventHandler(this.ChangeCOM);
            // 
            // butConnection
            // 
            this.butConnection.Location = new System.Drawing.Point(10, 46);
            this.butConnection.Name = "butConnection";
            this.butConnection.Size = new System.Drawing.Size(130, 23);
            this.butConnection.TabIndex = 6;
            this.butConnection.Text = "Connect";
            this.butConnection.UseVisualStyleBackColor = true;
            this.butConnection.Click += new System.EventHandler(this.Connection);
            // 
            // butDisconnect
            // 
            this.butDisconnect.Enabled = false;
            this.butDisconnect.Location = new System.Drawing.Point(146, 46);
            this.butDisconnect.Name = "butDisconnect";
            this.butDisconnect.Size = new System.Drawing.Size(130, 23);
            this.butDisconnect.TabIndex = 7;
            this.butDisconnect.Text = "Disconnect";
            this.butDisconnect.UseVisualStyleBackColor = true;
            this.butDisconnect.Click += new System.EventHandler(this.Disconnect);
            // 
            // butBrowse
            // 
            this.butBrowse.Location = new System.Drawing.Point(163, 18);
            this.butBrowse.Name = "butBrowse";
            this.butBrowse.Size = new System.Drawing.Size(27, 22);
            this.butBrowse.TabIndex = 9;
            this.butBrowse.Text = "...";
            this.butBrowse.UseVisualStyleBackColor = true;
            this.butBrowse.Click += new System.EventHandler(this.Browse);
            // 
            // butExport
            // 
            this.butExport.Enabled = false;
            this.butExport.Location = new System.Drawing.Point(9, 74);
            this.butExport.Name = "butExport";
            this.butExport.Size = new System.Drawing.Size(130, 23);
            this.butExport.TabIndex = 10;
            this.butExport.Text = "Export CSV File";
            this.butExport.UseVisualStyleBackColor = true;
            this.butExport.Click += new System.EventHandler(this.Export);
            // 
            // gbConnection
            // 
            this.gbConnection.Controls.Add(this.cbPorts);
            this.gbConnection.Controls.Add(this.butRefresh);
            this.gbConnection.Controls.Add(this.butConnection);
            this.gbConnection.Controls.Add(this.butDisconnect);
            this.gbConnection.Location = new System.Drawing.Point(12, 81);
            this.gbConnection.Name = "gbConnection";
            this.gbConnection.Size = new System.Drawing.Size(286, 80);
            this.gbConnection.TabIndex = 11;
            this.gbConnection.TabStop = false;
            this.gbConnection.Text = "Connection";
            // 
            // butRefresh
            // 
            this.butRefresh.BackgroundImage = global::Interface.Properties.Resources.refresh_button_icon;
            this.butRefresh.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.butRefresh.Location = new System.Drawing.Point(253, 18);
            this.butRefresh.Name = "butRefresh";
            this.butRefresh.Size = new System.Drawing.Size(23, 23);
            this.butRefresh.TabIndex = 8;
            this.butRefresh.UseVisualStyleBackColor = true;
            this.butRefresh.Click += new System.EventHandler(this.Refresh);
            // 
            // gbImportExport
            // 
            this.gbImportExport.Controls.Add(this.butClear);
            this.gbImportExport.Controls.Add(this.butSend);
            this.gbImportExport.Controls.Add(this.tbPath);
            this.gbImportExport.Controls.Add(this.butBrowse);
            this.gbImportExport.Controls.Add(this.butGet);
            this.gbImportExport.Controls.Add(this.butExport);
            this.gbImportExport.Controls.Add(this.butImport);
            this.gbImportExport.Location = new System.Drawing.Point(12, 167);
            this.gbImportExport.Name = "gbImportExport";
            this.gbImportExport.Size = new System.Drawing.Size(288, 108);
            this.gbImportExport.TabIndex = 12;
            this.gbImportExport.TabStop = false;
            this.gbImportExport.Text = "Import and export";
            // 
            // butClear
            // 
            this.butClear.Enabled = false;
            this.butClear.Location = new System.Drawing.Point(149, 74);
            this.butClear.Name = "butClear";
            this.butClear.Size = new System.Drawing.Size(130, 23);
            this.butClear.TabIndex = 15;
            this.butClear.Text = "Clear records";
            this.butClear.UseVisualStyleBackColor = true;
            this.butClear.Click += new System.EventHandler(this.Clear);
            // 
            // butSend
            // 
            this.butSend.Enabled = false;
            this.butSend.Location = new System.Drawing.Point(149, 45);
            this.butSend.Name = "butSend";
            this.butSend.Size = new System.Drawing.Size(130, 23);
            this.butSend.TabIndex = 15;
            this.butSend.Text = "Export to serial";
            this.butSend.UseVisualStyleBackColor = true;
            this.butSend.Click += new System.EventHandler(this.SendData);
            // 
            // gbRecords
            // 
            this.gbRecords.Controls.Add(this.pbProgress);
            this.gbRecords.Controls.Add(this.dgvRecords);
            this.gbRecords.Location = new System.Drawing.Point(12, 281);
            this.gbRecords.Name = "gbRecords";
            this.gbRecords.Size = new System.Drawing.Size(288, 286);
            this.gbRecords.TabIndex = 13;
            this.gbRecords.TabStop = false;
            this.gbRecords.Text = "Records";
            // 
            // pbProgress
            // 
            this.pbProgress.Location = new System.Drawing.Point(6, 257);
            this.pbProgress.Maximum = 1;
            this.pbProgress.Name = "pbProgress";
            this.pbProgress.Size = new System.Drawing.Size(270, 23);
            this.pbProgress.TabIndex = 4;
            this.pbProgress.Visible = false;
            // 
            // dgvRecords
            // 
            this.dgvRecords.AllowUserToAddRows = false;
            this.dgvRecords.AllowUserToDeleteRows = false;
            this.dgvRecords.AllowUserToResizeColumns = false;
            this.dgvRecords.AllowUserToResizeRows = false;
            this.dgvRecords.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRecords.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Record,
            this.Time});
            this.dgvRecords.Location = new System.Drawing.Point(6, 19);
            this.dgvRecords.Name = "dgvRecords";
            this.dgvRecords.ReadOnly = true;
            this.dgvRecords.RowHeadersVisible = false;
            this.dgvRecords.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvRecords.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvRecords.Size = new System.Drawing.Size(270, 261);
            this.dgvRecords.TabIndex = 15;
            // 
            // Record
            // 
            this.Record.HeaderText = "Record";
            this.Record.Name = "Record";
            this.Record.ReadOnly = true;
            this.Record.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Record.Width = 133;
            // 
            // Time
            // 
            this.Time.HeaderText = "Time (sec)";
            this.Time.Name = "Time";
            this.Time.ReadOnly = true;
            this.Time.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Time.Width = 133;
            // 
            // tmProgressBar
            // 
            this.tmProgressBar.Interval = 1000;
            this.tmProgressBar.Tick += new System.EventHandler(this.ProgressBar);
            // 
            // tmCOMVerification
            // 
            this.tmCOMVerification.Interval = 1000;
            // 
            // tmSerialCommunication
            // 
            this.tmSerialCommunication.Interval = 5000;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.ImageLocation = "";
            this.pictureBox1.Location = new System.Drawing.Point(158, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(111, 61);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 15;
            this.pictureBox1.TabStop = false;
            // 
            // pbIUT
            // 
            this.pbIUT.Image = ((System.Drawing.Image)(resources.GetObject("pbIUT.Image")));
            this.pbIUT.ImageLocation = "";
            this.pbIUT.Location = new System.Drawing.Point(40, 12);
            this.pbIUT.Name = "pbIUT";
            this.pbIUT.Size = new System.Drawing.Size(111, 61);
            this.pbIUT.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbIUT.TabIndex = 14;
            this.pbIUT.TabStop = false;
            // 
            // CInterface
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(307, 573);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.pbIUT);
            this.Controls.Add(this.gbRecords);
            this.Controls.Add(this.gbImportExport);
            this.Controls.Add(this.gbConnection);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "CInterface";
            this.Text = "Recumbent Bike Manager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnClose);
            this.gbConnection.ResumeLayout(false);
            this.gbImportExport.ResumeLayout(false);
            this.gbImportExport.PerformLayout();
            this.gbRecords.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecords)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbIUT)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button butImport;
        public System.Windows.Forms.TextBox tbPath;
        public System.Windows.Forms.Button butGet;
        public System.Windows.Forms.ComboBox cbPorts;
        public System.Windows.Forms.Button butConnection;
        public System.Windows.Forms.Button butDisconnect;
        public System.Windows.Forms.Button butRefresh;
        private System.Windows.Forms.Button butBrowse;
        public System.Windows.Forms.Button butExport;
        private System.Windows.Forms.GroupBox gbConnection;
        private System.Windows.Forms.GroupBox gbImportExport;
        private System.Windows.Forms.GroupBox gbRecords;
        public System.Windows.Forms.Button butSend;
        public System.Windows.Forms.DataGridView dgvRecords;
        private System.Windows.Forms.DataGridViewTextBoxColumn Record;
        private System.Windows.Forms.DataGridViewTextBoxColumn Time;
        private System.Windows.Forms.ProgressBar pbProgress;
        private System.Windows.Forms.Timer tmProgressBar;
        public System.Windows.Forms.Button butClear;
        public System.Windows.Forms.Timer tmCOMVerification;
        private System.Windows.Forms.Timer tmSerialCommunication;
        private System.Windows.Forms.PictureBox pbIUT;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

