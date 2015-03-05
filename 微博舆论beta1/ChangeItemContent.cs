using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 微博舆论
{
    public partial class ChangeItemContent : Form
    {
        /// <summary>
        /// 获取或设置文本框内容
        /// </summary>
        public string Content
        {
            set { this.textBoxContent.Text = value; }
            get { return this.textBoxContent.Text; }
        }

        /// <summary>
        /// 文本框的提示信息
        /// </summary>
        public string Message
        {
            set { this.labelMessage.Text = value; }
            get { return this.labelMessage.Text; }
        }

        public ChangeItemContent()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (textBoxContent.Text.Length == 0)
                return;
            this.Close();
        }
        
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
