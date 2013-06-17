namespace winCDP_Service
{
    partial class winCDPService
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

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.cdpTimer = new System.Windows.Forms.Timer(this.components);
            this.cdpBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.cdpEventLog = new System.Diagnostics.EventLog();
            ((System.ComponentModel.ISupportInitialize)(this.cdpEventLog)).BeginInit();
            // 
            // winCDPService
            // 
            this.ServiceName = "Service1";
            ((System.ComponentModel.ISupportInitialize)(this.cdpEventLog)).EndInit();

        }

        #endregion

        private System.Windows.Forms.Timer cdpTimer;
        private System.ComponentModel.BackgroundWorker cdpBackgroundWorker;
        private System.Diagnostics.EventLog cdpEventLog;
    }
}
