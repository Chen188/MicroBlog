namespace 微博舆论
{
    partial class 终端分类Dict
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
            this.listBoxSet = new System.Windows.Forms.ListBox();
            this.btnSeria = new System.Windows.Forms.Button();
            this.BtnMinForm = new System.Windows.Forms.Button();
            this.CloseWindowBtn = new System.Windows.Forms.Button();
            this.labelFormTitle = new System.Windows.Forms.Label();
            this.btnLoad = new System.Windows.Forms.Button();
            this.listBoxBrands = new System.Windows.Forms.ListBox();
            this.listBoxOS = new System.Windows.Forms.ListBox();
            this.listBoxSetKind = new System.Windows.Forms.ListBox();
            this.btnAutoAna = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // listBoxSet
            // 
            this.listBoxSet.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
            this.listBoxSet.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listBoxSet.ForeColor = System.Drawing.Color.White;
            this.listBoxSet.FormattingEnabled = true;
            this.listBoxSet.ItemHeight = 12;
            this.listBoxSet.Location = new System.Drawing.Point(6, 28);
            this.listBoxSet.Name = "listBoxSet";
            this.listBoxSet.Size = new System.Drawing.Size(259, 422);
            this.listBoxSet.TabIndex = 0;
            this.listBoxSet.SelectedIndexChanged += new System.EventHandler(this.listBoxSet_SelectedIndexChanged);
            // 
            // btnSeria
            // 
            this.btnSeria.Location = new System.Drawing.Point(615, 288);
            this.btnSeria.Name = "btnSeria";
            this.btnSeria.Size = new System.Drawing.Size(75, 23);
            this.btnSeria.TabIndex = 5;
            this.btnSeria.Text = "保存";
            this.toolTip1.SetToolTip(this.btnSeria, "保存分析结果");
            this.btnSeria.UseVisualStyleBackColor = true;
            this.btnSeria.Click += new System.EventHandler(this.btnSeria_Click);
            // 
            // BtnMinForm
            // 
            this.BtnMinForm.BackColor = System.Drawing.Color.Transparent;
            this.BtnMinForm.FlatAppearance.BorderSize = 0;
            this.BtnMinForm.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.BtnMinForm.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.BtnMinForm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnMinForm.Font = new System.Drawing.Font("宋体", 11.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.BtnMinForm.ForeColor = System.Drawing.Color.Black;
            this.BtnMinForm.Location = new System.Drawing.Point(643, 0);
            this.BtnMinForm.Margin = new System.Windows.Forms.Padding(0);
            this.BtnMinForm.Name = "BtnMinForm";
            this.BtnMinForm.Size = new System.Drawing.Size(27, 24);
            this.BtnMinForm.TabIndex = 6;
            this.BtnMinForm.Text = "-";
            this.BtnMinForm.UseVisualStyleBackColor = false;
            this.BtnMinForm.Click += new System.EventHandler(this.BtnMinForm_Click);
            // 
            // CloseWindowBtn
            // 
            this.CloseWindowBtn.BackColor = System.Drawing.Color.Transparent;
            this.CloseWindowBtn.FlatAppearance.BorderSize = 0;
            this.CloseWindowBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(89)))), ((int)(((byte)(92)))));
            this.CloseWindowBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(89)))), ((int)(((byte)(92)))));
            this.CloseWindowBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CloseWindowBtn.Font = new System.Drawing.Font("宋体", 11.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CloseWindowBtn.ForeColor = System.Drawing.Color.Black;
            this.CloseWindowBtn.Location = new System.Drawing.Point(670, 0);
            this.CloseWindowBtn.Margin = new System.Windows.Forms.Padding(0);
            this.CloseWindowBtn.Name = "CloseWindowBtn";
            this.CloseWindowBtn.Size = new System.Drawing.Size(27, 24);
            this.CloseWindowBtn.TabIndex = 7;
            this.CloseWindowBtn.Text = "×";
            this.CloseWindowBtn.UseVisualStyleBackColor = false;
            this.CloseWindowBtn.Click += new System.EventHandler(this.CloseWindowBtn_Click);
            // 
            // labelFormTitle
            // 
            this.labelFormTitle.AutoSize = true;
            this.labelFormTitle.BackColor = System.Drawing.Color.Transparent;
            this.labelFormTitle.Font = new System.Drawing.Font("微软雅黑", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelFormTitle.ForeColor = System.Drawing.Color.Black;
            this.labelFormTitle.Location = new System.Drawing.Point(3, 2);
            this.labelFormTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelFormTitle.Name = "labelFormTitle";
            this.labelFormTitle.Size = new System.Drawing.Size(99, 20);
            this.labelFormTitle.TabIndex = 8;
            this.labelFormTitle.Text = "终端词典管理";
            this.labelFormTitle.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.labelFormTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
            this.labelFormTitle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseMove);
            this.labelFormTitle.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseUp);
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(615, 193);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 23);
            this.btnLoad.TabIndex = 10;
            this.btnLoad.Text = "本地词典";
            this.toolTip1.SetToolTip(this.btnLoad, "查看本地词典");
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // listBoxBrands
            // 
            this.listBoxBrands.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
            this.listBoxBrands.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listBoxBrands.ForeColor = System.Drawing.Color.White;
            this.listBoxBrands.FormattingEnabled = true;
            this.listBoxBrands.ItemHeight = 12;
            this.listBoxBrands.Location = new System.Drawing.Point(272, 28);
            this.listBoxBrands.Name = "listBoxBrands";
            this.listBoxBrands.Size = new System.Drawing.Size(135, 422);
            this.listBoxBrands.TabIndex = 14;
            this.listBoxBrands.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBoxBrands_MouseDoubleClick);
            // 
            // listBoxOS
            // 
            this.listBoxOS.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
            this.listBoxOS.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listBoxOS.ForeColor = System.Drawing.Color.White;
            this.listBoxOS.FormattingEnabled = true;
            this.listBoxOS.ItemHeight = 12;
            this.listBoxOS.Location = new System.Drawing.Point(414, 28);
            this.listBoxOS.Name = "listBoxOS";
            this.listBoxOS.Size = new System.Drawing.Size(120, 206);
            this.listBoxOS.TabIndex = 15;
            // 
            // listBoxSetKind
            // 
            this.listBoxSetKind.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
            this.listBoxSetKind.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listBoxSetKind.ForeColor = System.Drawing.Color.White;
            this.listBoxSetKind.FormattingEnabled = true;
            this.listBoxSetKind.ItemHeight = 12;
            this.listBoxSetKind.Location = new System.Drawing.Point(414, 244);
            this.listBoxSetKind.Name = "listBoxSetKind";
            this.listBoxSetKind.Size = new System.Drawing.Size(120, 206);
            this.listBoxSetKind.TabIndex = 15;
            // 
            // btnAutoAna
            // 
            this.btnAutoAna.Location = new System.Drawing.Point(615, 244);
            this.btnAutoAna.Name = "btnAutoAna";
            this.btnAutoAna.Size = new System.Drawing.Size(75, 23);
            this.btnAutoAna.TabIndex = 16;
            this.btnAutoAna.Text = "自动分析";
            this.toolTip1.SetToolTip(this.btnAutoAna, "分析本地数据文件");
            this.btnAutoAna.UseVisualStyleBackColor = true;
            this.btnAutoAna.Click += new System.EventHandler(this.btnAutoAna_Click);
            // 
            // 终端分类Dict
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.BackgroundImage = global::微博舆论.Properties.Resources.Bg;
            this.ClientSize = new System.Drawing.Size(702, 459);
            this.Controls.Add(this.btnAutoAna);
            this.Controls.Add(this.listBoxSetKind);
            this.Controls.Add(this.listBoxOS);
            this.Controls.Add(this.listBoxBrands);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.labelFormTitle);
            this.Controls.Add(this.BtnMinForm);
            this.Controls.Add(this.CloseWindowBtn);
            this.Controls.Add(this.btnSeria);
            this.Controls.Add(this.listBoxSet);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "终端分类Dict";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "中断分类Dict";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.终端分类Dict_FormClosing);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseUp);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxSet;
        private System.Windows.Forms.Button btnSeria;
        private System.Windows.Forms.Button BtnMinForm;
        private System.Windows.Forms.Button CloseWindowBtn;
        private System.Windows.Forms.Label labelFormTitle;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.ListBox listBoxBrands;
        private System.Windows.Forms.ListBox listBoxOS;
        private System.Windows.Forms.ListBox listBoxSetKind;
        private System.Windows.Forms.Button btnAutoAna;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}