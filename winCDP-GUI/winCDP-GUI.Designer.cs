namespace winCDP_GUI
{
    partial class winCDP
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
            this.adapterList = new System.Windows.Forms.ComboBox();
            this.adapterInfo_Index = new System.Windows.Forms.ComboBox();
            this.adapterInfo_AddressLabel = new System.Windows.Forms.Label();
            this.defaultAddressControl = new System.Windows.Forms.TextBox();
            this.startCapture = new System.Windows.Forms.Button();
            this.cdpCaptureWorker = new System.ComponentModel.BackgroundWorker();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.progressBarWorker = new System.ComponentModel.BackgroundWorker();
            this.ipV4AddressControl = new TestFlexFieldControl.IPV4AddressControl();
            this.ipV6AddressControl = new TestFlexFieldControl.IPV6AddressControl();
            this.SuspendLayout();
            // 
            // adapterList
            // 
            this.adapterList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.adapterList.FormattingEnabled = true;
            this.adapterList.Location = new System.Drawing.Point(13, 13);
            this.adapterList.Name = "adapterList";
            this.adapterList.Size = new System.Drawing.Size(559, 21);
            this.adapterList.TabIndex = 0;
            this.adapterList.SelectedValueChanged += new System.EventHandler(this.adapterList_SelectedValueChanged);
            // 
            // adapterInfo_Index
            // 
            this.adapterInfo_Index.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.adapterInfo_Index.FormattingEnabled = true;
            this.adapterInfo_Index.Location = new System.Drawing.Point(13, 40);
            this.adapterInfo_Index.Name = "adapterInfo_Index";
            this.adapterInfo_Index.Size = new System.Drawing.Size(130, 21);
            this.adapterInfo_Index.TabIndex = 8;
            this.adapterInfo_Index.SelectedIndexChanged += new System.EventHandler(this.adapterInfo_Index_SelectedIndexChanged);
            // 
            // adapterInfo_AddressLabel
            // 
            this.adapterInfo_AddressLabel.AutoSize = true;
            this.adapterInfo_AddressLabel.Location = new System.Drawing.Point(149, 48);
            this.adapterInfo_AddressLabel.Name = "adapterInfo_AddressLabel";
            this.adapterInfo_AddressLabel.Size = new System.Drawing.Size(51, 13);
            this.adapterInfo_AddressLabel.TabIndex = 9;
            this.adapterInfo_AddressLabel.Text = "Address :";
            this.adapterInfo_AddressLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // defaultAddressControl
            // 
            this.defaultAddressControl.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.defaultAddressControl.Location = new System.Drawing.Point(206, 48);
            this.defaultAddressControl.Name = "defaultAddressControl";
            this.defaultAddressControl.ReadOnly = true;
            this.defaultAddressControl.Size = new System.Drawing.Size(365, 13);
            this.defaultAddressControl.TabIndex = 13;
            this.defaultAddressControl.Visible = false;
            // 
            // startCapture
            // 
            this.startCapture.Location = new System.Drawing.Point(470, 327);
            this.startCapture.Name = "startCapture";
            this.startCapture.Size = new System.Drawing.Size(100, 23);
            this.startCapture.TabIndex = 14;
            this.startCapture.Text = "Go";
            this.startCapture.UseVisualStyleBackColor = true;
            this.startCapture.Click += new System.EventHandler(this.startCapture_Click);
            // 
            // cdpCaptureWorker
            // 
            this.cdpCaptureWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.cdpCaptureWorker_DoWork);
            this.cdpCaptureWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.cdpCaptureWorker_ProgressChanged);
            this.cdpCaptureWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.cdpCaptureWorker_RunWorkerCompleted);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(470, 327);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(100, 23);
            this.progressBar.Step = 1;
            this.progressBar.TabIndex = 15;
            this.progressBar.Visible = false;
            // 
            // progressBarWorker
            // 
            this.progressBarWorker.WorkerReportsProgress = true;
            this.progressBarWorker.WorkerSupportsCancellation = true;
            this.progressBarWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.progressBarWorker_DoWork);
            this.progressBarWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.progressBarWorker_ProgressChanged);
            this.progressBarWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.progressBarWorker_RunWorkerCompleted);
            // 
            // ipV4AddressControl
            // 
            this.ipV4AddressControl.AllowInternalTab = false;
            this.ipV4AddressControl.AutoHeight = true;
            this.ipV4AddressControl.BackColor = System.Drawing.SystemColors.Window;
            this.ipV4AddressControl.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ipV4AddressControl.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.ipV4AddressControl.FieldCount = 4;
            this.ipV4AddressControl.Location = new System.Drawing.Point(206, 48);
            this.ipV4AddressControl.Name = "ipV4AddressControl";
            this.ipV4AddressControl.ReadOnly = true;
            this.ipV4AddressControl.Size = new System.Drawing.Size(87, 13);
            this.ipV4AddressControl.TabIndex = 11;
            this.ipV4AddressControl.Text = "...";
            this.ipV4AddressControl.Visible = false;
            // 
            // ipV6AddressControl
            // 
            this.ipV6AddressControl.AllowInternalTab = false;
            this.ipV6AddressControl.AutoHeight = true;
            this.ipV6AddressControl.BackColor = System.Drawing.SystemColors.Window;
            this.ipV6AddressControl.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ipV6AddressControl.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.ipV6AddressControl.FieldCount = 8;
            this.ipV6AddressControl.Location = new System.Drawing.Point(206, 48);
            this.ipV6AddressControl.Name = "ipV6AddressControl";
            this.ipV6AddressControl.ReadOnly = true;
            this.ipV6AddressControl.Size = new System.Drawing.Size(283, 13);
            this.ipV6AddressControl.TabIndex = 12;
            this.ipV6AddressControl.Text = ":::::::";
            this.ipV6AddressControl.Visible = false;
            // 
            // winCDP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 362);
            this.Controls.Add(this.startCapture);
            this.Controls.Add(this.ipV4AddressControl);
            this.Controls.Add(this.adapterInfo_AddressLabel);
            this.Controls.Add(this.adapterInfo_Index);
            this.Controls.Add(this.adapterList);
            this.Controls.Add(this.ipV6AddressControl);
            this.Controls.Add(this.defaultAddressControl);
            this.Controls.Add(this.progressBar);
            this.Name = "winCDP";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.winCDP_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox adapterList;
        private System.Windows.Forms.ComboBox adapterInfo_Index;
        private System.Windows.Forms.Label adapterInfo_AddressLabel;
        private TestFlexFieldControl.IPV4AddressControl ipV4AddressControl;
        private TestFlexFieldControl.IPV6AddressControl ipV6AddressControl;
        private System.Windows.Forms.TextBox defaultAddressControl;
        private System.Windows.Forms.Button startCapture;
        private System.ComponentModel.BackgroundWorker cdpCaptureWorker;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.ComponentModel.BackgroundWorker progressBarWorker;
    }
}

