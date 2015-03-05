using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 微博舆论
{
    public partial class ProgressForm : Form
    {
        #region 窗口移动
        private int preLocationX = 0, preLocationY = 0;
        private Point preMousePos = new Point();
        private bool isMouseDown = false;
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            preLocationX = this.Left;
            preLocationY = this.Top;
            this.preMousePos = MousePosition;
            isMouseDown = true;
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isMouseDown) return;
            this.Left = MousePosition.X - preMousePos.X + preLocationX;
            this.Top = MousePosition.Y - preMousePos.Y + preLocationY;

        }
        #endregion
        private System.Threading.Timer timerProgress;
        public ProgressBarStyle style
        {
            set
            {
                if (progressBar1.Style != value)
                    progressBar1.Style = value;
            }
        }
        public ProgressForm()
        {
            InitializeComponent();
        }

        void updateProgress()
        {
            if (Datas.ProgressBarCompelet)
            {
                timerProgress.Dispose();
                timerProgress = null;
                this.Close();
                return;
            }
            if (Datas.Value <= 100)
            {
                progressBar1.Value = Datas.Value;
                labelProgressBarState.Text = Datas.ProgressBarState;
                style =  Datas.ProgresBarStyle;
                //Application.DoEvents();       //响应操作..
            }
        }

        //委托函数,更新窗体用
        delegate void updateForm();
        void invokeUpdate(object o)
        {
            this.Invoke(new updateForm(updateProgress));
        }

        /// <summary>
        /// 窗口关闭之前,
        /// 清理资源,
        /// 恢复状态,
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProgressForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (timerProgress != null)
            {
                timerProgress.Dispose();
                timerProgress = null;
            }
            Datas.Value = 0;
            Datas.ProgressBarCompelet = false;
            Datas.ProgresBarStyle = ProgressBarStyle.Continuous;
        }

        private void ProgressForm_Load(object sender, EventArgs e)
        {
            timerProgress = new System.Threading.Timer(new TimerCallback(invokeUpdate), null, 0, 20);
        }
    }
}
