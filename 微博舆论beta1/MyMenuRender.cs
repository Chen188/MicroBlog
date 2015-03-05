using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace 微博舆论
{
    public class MyMenuRender : ToolStripProfessionalRenderer
    {
        /// <summary>
        /// 渲染整个背景
        /// </summary>
        /// <param name="e"></param>
        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            e.ToolStrip.ForeColor = Color.White;
            //如果是下拉
            if (e.ToolStrip is ToolStripDropDown)
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(25, 25, 25)), e.AffectedBounds);
            }
            //如果是菜单项
            else if (e.ToolStrip is MenuStrip)
            { }
            else
            {
                base.OnRenderToolStripBackground(e);
            }
        }
        /// <summary>
        /// 渲染下拉左侧图标区域
        /// </summary>
        /// <param name="e"></param>
        protected override void OnRenderImageMargin(ToolStripRenderEventArgs e)
        {
            //渐变填充图像区域
            //FillLineGradient(e.Graphics, e.AffectedBounds, colorconfig.MarginStartColor, colorconfig.MarginEndColor, 0f, null);
        }
        /// <summary>
        /// 渲染菜单项的背景
        /// </summary>
        /// <param name="e"></param>
        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            e.Item.ForeColor = Color.White;
            if (e.ToolStrip is MenuStrip)
            {
                //如果被选中或被按下
                if (e.Item.Selected || e.Item.Pressed)
                {
                    DrawArc(e.Graphics, new Rectangle(0, 0, e.Item.Size.Width, e.Item.Size.Height), Color.FromArgb(27, 27, 28));
                }
                else
                {
                    base.OnRenderMenuItemBackground(e);
                }
            }
            else if (e.ToolStrip is ToolStripDropDown)
            {
                if (e.Item.Selected)
                {
                    //FillLineGradient(e.Graphics, new Rectangle(0, 0, e.Item.Size.Width, e.Item.Size.Height), colorconfig.DropDownItemStartColor, colorconfig.DropDownItemEndColor, 90f, null);
                    DrawArc(e.Graphics, new Rectangle(0, 0, e.Item.Size.Width, e.Item.Size.Height), Color.FromArgb(45, 45, 45));
                }
            }
            else
            {
                base.OnRenderMenuItemBackground(e);
            }
        }
        /// <summary>
        /// 渲染边框
        /// </summary>
        /// <param name="e"></param>
        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            if (e.ToolStrip is ToolStripDropDown)
            {
                e.Graphics.DrawRectangle(new Pen(Color.FromArgb(27, 27, 28)), new Rectangle(0, 0, e.AffectedBounds.Width - 1, e.AffectedBounds.Height - 1));
            }
            else
            {
                base.OnRenderToolStripBorder(e);
            }
        }
        /// <summary>
        /// 填充线性渐变
        /// </summary>
        /// <param name="g">画布</param>
        /// <param name="rect">填充区域</param>
        /// <param name="startcolor">开始颜色</param>
        /// <param name="endcolor">结束颜色</param>
        /// <param name="angle">角度</param>
        /// <param name="blend">对象的混合图案</param>
        private void FillLineGradient(Graphics g, Rectangle rect, Color startcolor, Color endcolor, float angle, Blend blend)
        {
            LinearGradientBrush linebrush = new LinearGradientBrush(rect, startcolor, endcolor, angle);
            if (blend != null)
            {
                linebrush.Blend = blend;
            }
            GraphicsPath path = new GraphicsPath();
            path.AddRectangle(rect);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.FillPath(linebrush, path);
        }

        private void DrawArc(Graphics g, Rectangle rect, Color color)
        {
            //GraphicsPath oPath = new GraphicsPath();
            //int x = 0;
            //int y = 0;
            //int w = rect.Width - 1;
            //int h = rect.Height - 1;
            //int a = 8;
            Pen p = new Pen(color);
            Brush brush = p.Brush;
            //oPath.AddArc(x, y, a, a, 180, 90);
            //oPath.AddArc(w - a, y, a, a, 270, 90);
            //oPath.AddArc(w - a, h - a, a, a, 0, 90);
            //oPath.AddArc(x, h - a, a, a, 90, 90);
            //oPath.CloseAllFigures();
            g.SmoothingMode = SmoothingMode.AntiAlias;
            //g.FillPath(brush, oPath);
            //g.DrawPath(p, oPath);
            g.FillRectangle(brush, rect);
        }
    }
}
