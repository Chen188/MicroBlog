using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 微博舆论
{
    public partial class ImageViewForm : Form
    {
        //动画效果类型选项
        public const Int32 AW_HOR_POSITIVE = 0X00000001;//与NEGATIVE   
        public const Int32 AW_HOR_NEGATIVE = 0X00000002;//自左先右显示窗口，当使用AW-CENTER时，被忽略   
        public const Int32 AW_VER_POSITIVE = 0X00000004;//自上而下   
        public const Int32 AW_VER_NEGATIVE = 0X00000008;//与AW—VER-POSITIV   
        public const Int32 AW_CENTER_POSITIVE = 0X0000010;//若使用了AW_HIDE，窗口向内重叠，
        //未使用AW-HIDE，则向外扩展   
        // public const Int32 AW_HOR_POSITIVE = 0X00000001;   
        public const Int32 AW_HIDE = 0X00010000;
        public const Int32 AW_ACTIVATE = 0X00020000;
        public const Int32 AW_SLIDE = 0X00040000;
        public const Int32 AW_BLEND = 0X00080000;
        [DllImportAttribute("user32.dll")]//声明使用WINDOW的API函数ANIMATEWINDOW：参数说明：HWND-》目标窗口句柄，DWTIME-》动画持续时间，值越大时间越长；DWWLAGS-》动画效果类型选项   
        private static extern bool AnimateWindow(IntPtr hwnd, int dwTime, int dwFlags); 
        public ImageViewForm()
        {
            InitializeComponent();
        }

        private void ImageViewForm_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ImageViewForm_Load(object sender, EventArgs e)
        {
            AnimateWindow(this.Handle, 400, AW_ACTIVATE + AW_BLEND);
        }

        private void ImageViewForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            AnimateWindow(this.Handle, 400, AW_HIDE + AW_BLEND);//退出时结束动画
        }
        #region 移动窗体
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
    }
}
