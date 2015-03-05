namespace 微博舆论
{
    partial class 机型数据管理
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
            this.btnGetDataFromServer = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnSaveData = new System.Windows.Forms.Button();
            this.btnReadLocalFile = new System.Windows.Forms.Button();
            this.btnDownloadHTLML = new System.Windows.Forms.Button();
            this.btnAnaLocalHTML = new System.Windows.Forms.Button();
            this.listBoxBrand = new System.Windows.Forms.ListBox();
            this.listBoxModels = new System.Windows.Forms.ListBox();
            this.listBoxModel = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // btnGetDataFromServer
            // 
            this.btnGetDataFromServer.Location = new System.Drawing.Point(524, 104);
            this.btnGetDataFromServer.Name = "btnGetDataFromServer";
            this.btnGetDataFromServer.Size = new System.Drawing.Size(75, 24);
            this.btnGetDataFromServer.TabIndex = 0;
            this.btnGetDataFromServer.Text = "获取数据";
            this.toolTip1.SetToolTip(this.btnGetDataFromServer, "从服务器获取数据");
            this.btnGetDataFromServer.UseVisualStyleBackColor = true;
            this.btnGetDataFromServer.Click += new System.EventHandler(this.btnGetDataFromServer_Click);
            // 
            // btnSaveData
            // 
            this.btnSaveData.Location = new System.Drawing.Point(524, 133);
            this.btnSaveData.Name = "btnSaveData";
            this.btnSaveData.Size = new System.Drawing.Size(75, 24);
            this.btnSaveData.TabIndex = 0;
            this.btnSaveData.Text = "保存数据";
            this.toolTip1.SetToolTip(this.btnSaveData, "保存文件到本地");
            this.btnSaveData.UseVisualStyleBackColor = true;
            this.btnSaveData.Click += new System.EventHandler(this.btnSaveData_Click);
            // 
            // btnReadLocalFile
            // 
            this.btnReadLocalFile.Location = new System.Drawing.Point(524, 162);
            this.btnReadLocalFile.Name = "btnReadLocalFile";
            this.btnReadLocalFile.Size = new System.Drawing.Size(75, 24);
            this.btnReadLocalFile.TabIndex = 0;
            this.btnReadLocalFile.Text = "读取文件";
            this.toolTip1.SetToolTip(this.btnReadLocalFile, "读取本地文件");
            this.btnReadLocalFile.UseVisualStyleBackColor = true;
            this.btnReadLocalFile.Click += new System.EventHandler(this.btnReadLocalFile_Click);
            // 
            // btnDownloadHTLML
            // 
            this.btnDownloadHTLML.Location = new System.Drawing.Point(524, 192);
            this.btnDownloadHTLML.Name = "btnDownloadHTLML";
            this.btnDownloadHTLML.Size = new System.Drawing.Size(75, 23);
            this.btnDownloadHTLML.TabIndex = 2;
            this.btnDownloadHTLML.Text = "下载网页";
            this.toolTip1.SetToolTip(this.btnDownloadHTLML, "从服务器下载网页后进行分析");
            this.btnDownloadHTLML.UseVisualStyleBackColor = true;
            this.btnDownloadHTLML.Click += new System.EventHandler(this.btnDownloadHTML_Click);
            // 
            // btnAnaLocalHTML
            // 
            this.btnAnaLocalHTML.Location = new System.Drawing.Point(524, 221);
            this.btnAnaLocalHTML.Name = "btnAnaLocalHTML";
            this.btnAnaLocalHTML.Size = new System.Drawing.Size(75, 23);
            this.btnAnaLocalHTML.TabIndex = 2;
            this.btnAnaLocalHTML.Text = "分析网页";
            this.toolTip1.SetToolTip(this.btnAnaLocalHTML, "分析本地存放的网页");
            this.btnAnaLocalHTML.UseVisualStyleBackColor = true;
            this.btnAnaLocalHTML.Click += new System.EventHandler(this.btnAnaLocalHTML_Click);
            // 
            // listBoxBrand
            // 
            this.listBoxBrand.FormattingEnabled = true;
            this.listBoxBrand.ItemHeight = 12;
            this.listBoxBrand.Location = new System.Drawing.Point(12, 5);
            this.listBoxBrand.Name = "listBoxBrand";
            this.listBoxBrand.Size = new System.Drawing.Size(175, 316);
            this.listBoxBrand.TabIndex = 1;
            this.listBoxBrand.SelectedIndexChanged += new System.EventHandler(this.listBoxBrand_SelectedIndexChanged);
            // 
            // listBoxModels
            // 
            this.listBoxModels.FormattingEnabled = true;
            this.listBoxModels.ItemHeight = 12;
            this.listBoxModels.Location = new System.Drawing.Point(193, 5);
            this.listBoxModels.Name = "listBoxModels";
            this.listBoxModels.Size = new System.Drawing.Size(136, 316);
            this.listBoxModels.TabIndex = 1;
            this.listBoxModels.SelectedIndexChanged += new System.EventHandler(this.listBoxModels_SelectedIndexChanged);
            // 
            // listBoxModel
            // 
            this.listBoxModel.FormattingEnabled = true;
            this.listBoxModel.ItemHeight = 12;
            this.listBoxModel.Location = new System.Drawing.Point(335, 5);
            this.listBoxModel.Name = "listBoxModel";
            this.listBoxModel.Size = new System.Drawing.Size(183, 316);
            this.listBoxModel.TabIndex = 1;
            // 
            // 机型数据管理
            // 
            this.AcceptButton = this.btnGetDataFromServer;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(611, 333);
            this.Controls.Add(this.btnAnaLocalHTML);
            this.Controls.Add(this.btnDownloadHTLML);
            this.Controls.Add(this.listBoxModel);
            this.Controls.Add(this.listBoxModels);
            this.Controls.Add(this.listBoxBrand);
            this.Controls.Add(this.btnReadLocalFile);
            this.Controls.Add(this.btnSaveData);
            this.Controls.Add(this.btnGetDataFromServer);
            this.Name = "机型数据管理";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "机型数据管理";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnGetDataFromServer;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btnSaveData;
        private System.Windows.Forms.ListBox listBoxBrand;
        private System.Windows.Forms.ListBox listBoxModels;
        private System.Windows.Forms.ListBox listBoxModel;
        private System.Windows.Forms.Button btnReadLocalFile;
        private System.Windows.Forms.Button btnDownloadHTLML;
        private System.Windows.Forms.Button btnAnaLocalHTML;
    }
}