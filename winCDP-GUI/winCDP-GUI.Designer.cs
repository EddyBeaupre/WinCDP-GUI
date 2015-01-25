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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(winCDP));
            this.adapterList = new System.Windows.Forms.ComboBox();
            this.startCapture = new System.Windows.Forms.Button();
            this.cdpCaptureWorker = new System.ComponentModel.BackgroundWorker();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.progressBarWorker = new System.ComponentModel.BackgroundWorker();
            this.AdapterAddress = new System.Windows.Forms.ListBox();
            this.showResults = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // adapterList
            // 
            this.adapterList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.adapterList.FormattingEnabled = true;
            this.adapterList.Location = new System.Drawing.Point(117, 13);
            this.adapterList.Name = "adapterList";
            this.adapterList.Size = new System.Drawing.Size(455, 21);
            this.adapterList.TabIndex = 0;
            this.adapterList.SelectedValueChanged += new System.EventHandler(this.adapterList_SelectedValueChanged);
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
            // AdapterAddress
            // 
            this.AdapterAddress.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.AdapterAddress.FormattingEnabled = true;
            this.AdapterAddress.Location = new System.Drawing.Point(13, 40);
            this.AdapterAddress.Name = "AdapterAddress";
            this.AdapterAddress.Size = new System.Drawing.Size(559, 30);
            this.AdapterAddress.Sorted = true;
            this.AdapterAddress.TabIndex = 16;
            // 
            // showResults
            // 
            this.showResults.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.showResults.Location = new System.Drawing.Point(13, 77);
            this.showResults.Name = "showResults";
            this.showResults.ReadOnly = true;
            this.showResults.Size = new System.Drawing.Size(557, 244);
            this.showResults.TabIndex = 17;
            this.showResults.Text = "";
            this.showResults.TextChanged += new System.EventHandler(this.showResults_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 13);
            this.label1.TabIndex = 18;
            this.label1.Text = "Network Interface :";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // winCDP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 362);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.showResults);
            this.Controls.Add(this.AdapterAddress);
            this.Controls.Add(this.startCapture);
            this.Controls.Add(this.adapterList);
            this.Controls.Add(this.progressBar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "winCDP";
            this.Text = "winCDP";
            this.Load += new System.EventHandler(this.winCDP_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox adapterList;
        private System.Windows.Forms.Button startCapture;
        private System.ComponentModel.BackgroundWorker cdpCaptureWorker;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.ComponentModel.BackgroundWorker progressBarWorker;
        private System.Windows.Forms.ListBox AdapterAddress;
        private System.Windows.Forms.RichTextBox showResults;
        private System.Windows.Forms.Label label1;
    }
}

