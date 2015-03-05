using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.IO;

namespace 微博舆论
{
    public partial class Form1 :Form
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


        bool FlowLayoutPanelFreqVisible
        {
            set
            {
                if (flowLayoutPanelFreq.Visible != value)
                {
                    flowLayoutPanelFreq.Visible = value;
                }
            }
        }
        bool FlowLayoutPanelEmojVisible
        {
            set
            {
                if (flowLayoutPanelEmoj.Visible != value)
                {
                    flowLayoutPanelEmoj.Visible = value;
                }
            }
        }
        bool FlowLayoutPanelSetKindVisible
        {
            set
            {
                if (flowLayoutPanelSetKind.Visible != value)
                {
                    flowLayoutPanelSetKind.Visible = value;
                }
            }
        }
        /// <summary>
        /// 设置窗体的圆角矩形
        /// </summary>
        /// <param name="form">需要设置的窗体</param>
        /// <param name="rgnRadius">圆角矩形的半径</param>
        public static void SetFormRoundRectRgn(Form form, int rgnRadius)
        {
            int hRgn = 0;
            hRgn = Win32.CreateRoundRectRgn(0, 0, form.Width + 1, form.Height + 1, rgnRadius, rgnRadius);
            Win32.SetWindowRgn(form.Handle, hRgn, true);
            Win32.DeleteObject(hRgn);
        }

        #region 窗体事件
        public Form1()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.ResizeRedraw |
              ControlStyles.OptimizedDoubleBuffer |
              ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();
            SetFormRoundRectRgn(this, 3);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            //this.button1.BackColor = Color.Transparent;
            AnimateWindow(this.Handle, 400, AW_ACTIVATE + AW_BLEND);
            //AnimateWindow(this.Handle, 400, AW_SLIDE + AW_VER_NEGATIVE + AW_ACTIVATE);
            微博舆论.Segment.Init();
            微博舆论.Match.MatchOptions options = 微博舆论.Setting.PanGuSettings.Config.MatchOptions;
            微博舆论.Match.MatchParameter parameters = 微博舆论.Setting.PanGuSettings.Config.Parameters;
            menuStrip1.Renderer = new MyMenuRender();

            flowLayoutPanelEmoj.Left = flowLayoutPanelSetKind.Left = flowLayoutPanelFreq.Left;
            flowLayoutPanelEmoj.Top = flowLayoutPanelSetKind.Top = flowLayoutPanelFreq.Top;
            //RetrieveSheetnames();
        }
        private void CloseWindowBtn_MouseClick(object sender, MouseEventArgs e)
        {
            this.Close();
        }

        private void BtnMinForm_MouseClick(object sender, MouseEventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void CloseWindowBtn_MouseLeave(object sender, EventArgs e)
        {
            labelFormTitle.Focus();
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //AnimateWindow(this.Handle, 300, AW_SLIDE + AW_VER_NEGATIVE + AW_HIDE );//退出时结束动画
            AnimateWindow(this.Handle, 400, AW_HIDE + AW_BLEND);//退出时结束动画
        }
        #endregion
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
            if (pictureBoxFren.Visible)
            {
                pictureBoxFren.Visible = false;
            }
            this.Left = MousePosition.X - preMousePos.X + preLocationX;
            this.Top = MousePosition.Y - preMousePos.Y + preLocationY;

        }
        #endregion
        #region 底部按钮事件处理
        private void BtnLeft_MouseDown(object sender, MouseEventArgs e)
        {
            BtnLeft.BackColor = Color.Maroon;
            BtnCenter.BackColor = Color.White;
            BtnRight.BackColor = Color.White;

            BtnLeft.ForeColor = Color.LavenderBlush;
            BtnCenter.ForeColor = BtnRight.ForeColor = Color.FromArgb(44, 62, 80);
        }

        private void BtnCenter_MouseDown(object sender, MouseEventArgs e)
        {
            BtnLeft.BackColor = Color.White;
            BtnCenter.BackColor = Color.Maroon;
            BtnRight.BackColor = Color.White;

            BtnCenter.ForeColor = Color.LavenderBlush;
            BtnLeft.ForeColor = BtnRight.ForeColor = Color.FromArgb(44, 62, 80);
        }

        private void BtnRight_MouseDown(object sender, MouseEventArgs e)
        {
            BtnLeft.BackColor = Color.White;
            BtnCenter.BackColor = Color.White;
            BtnRight.BackColor = Color.Maroon;

            BtnRight.ForeColor = Color.LavenderBlush;
            BtnLeft.ForeColor = BtnCenter.ForeColor = Color.FromArgb(44, 62, 80);
        }

        private void BtnLeft_Click(object sender, EventArgs e)
        {
            FlowLayoutPanelSetKindVisible = false;
            FlowLayoutPanelEmojVisible = false;

            if (DisplayFrequency)
            {
                FlowLayoutPanelFreqVisible = true;
                return;
            }
            DisplayFrequency = true;
            ProgressForm progressForm = new ProgressForm();
            progressForm.ShowDialog();
            FlowLayoutPanelFreqVisible = true;
        }
        
        private void BtnCenter_Click(object sender, EventArgs e)
        {
            FlowLayoutPanelFreqVisible = false;
            FlowLayoutPanelEmojVisible = false;
            if (DisplaySetKind)
            {
                FlowLayoutPanelSetKindVisible = true;
                return;
            }
            DisplaySetKind = true;
            
            ProgressForm progressForm = new ProgressForm();
            progressForm.ShowDialog();

            FlowLayoutPanelSetKindVisible = true;
        }
        private void BtnRight_Click(object sender, EventArgs e)
        {

            FlowLayoutPanelFreqVisible = false;
            FlowLayoutPanelSetKindVisible = false;
            if (DisplayEmotion)
            {
                FlowLayoutPanelEmojVisible = true;
                return;
            }
            DisplayEmotion = true;

            ProgressForm progressForm = new ProgressForm();
            progressForm.ShowDialog();

            FlowLayoutPanelEmojVisible = true;

        }

        #endregion
        #region 工具栏
        /// <summary>
        /// 终端分类Dict
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            终端分类Dict dict = new 终端分类Dict();
            dict.ShowDialog();
        }

        /// <summary>
        /// 机型数据管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            机型数据管理 manage = new 机型数据管理();
            manage.Show();
        }
        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "dat文件|*.dat";
            ofd.Title = "打开文件";
            ofd.Multiselect = false;
            ofd.InitialDirectory = Environment.CurrentDirectory;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Datas.PublicOpinionAnaPath = ofd.FileName;
            }
        }
        #endregion

        /// <summary>
        /// 提取制定网页表情数据
        /// </summary>
        void HTML2EmoTxt()
        {
            //Ana html emo to emo.txt
            FileStream fs = new FileStream("emo.txt", FileMode.Open);
            StreamReader sr = new StreamReader(fs);

            string s = sr.ReadToEnd();
            List<string> result = new List<string>();
            MatchCollection mc = Regex.Matches(s, @"\[\w+\]");

            foreach (System.Text.RegularExpressions.Match item in mc)
            {
                if (!result.Contains(item.Value))
                    result.Add(item.Value);
            }
            sr.Close();
            fs.Close();

            fs = new FileStream("datas/emo.txt", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            foreach (string item in result)
            {
                sw.WriteLine(item);
            }
            sw.Close();
            fs.Close();
        }


        private void pictureBoxSetKind_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            PictureBox pictureBox = sender as PictureBox;
            if (pictureBox.Image == null)
                return;
            ImageViewForm form = new ImageViewForm();
            form.Height = Screen.GetWorkingArea(this).Height;
            form.Width = form.Height * pictureBox.Image.Width / pictureBox.Image.Height;
            form.BackgroundImage = pictureBox.Image;
            form.Show();
        }

        delegate void updateListboxFreq(List<ju_word>[] list);

        void doUpdateListboxFreq(List<ju_word>[] list)
        {
            //listBoxTimeCir.Items.Clear();
            //for (int i = 0; i < list.Length; i++)
            //{
            //    listBoxTimeCir.Items.Add(list[i]);
            //}
        }
        
        public void readdata()
        {
            Segment segment = new Segment();
            ICollection<WordInfo> words;
            ICollection<WordInfo> re_words;
            SinaJSON item;
            SinaJSON re_item;
            List<string> ls = new List<string>();
            string[] sp = Datas.ads_word.Split(',');
            int quan;
            int t;
            int i;
            double temp_quan;
            List<ju_word> te;//test

            Datas.auto_read.WaitOne();
            for (; Datas.now_readnum < Datas.temp_data.Count; Datas.now_readnum++)
            {
                if (Datas.now_readnum == Datas.temp_data.Count)
                    Datas.auto_read.WaitOne();
                quan = 0;
                item = Datas.temp_data.ElementAt(Datas.now_readnum);
                for (i = 0; i < sp.Length; i++)
                {
                    t = item.Text.IndexOf(sp[i]);
                    if (t != -1)
                        quan++;

                }
                if (quan < 4)
                {
                    item.Text = Regex.Replace(item.Text, @"[a-zA-Z]+[:/]*[a-zA-Z\d\./]+", "");

                    for (i = item.Text.Length - 1; i > -1; i--)
                    {
                        if (item.Text[i] == 55356 || item.Text[i] == 55357)
                        {
                            item.Text = item.Text.Remove(i);

                        }

                    }

                    words = segment.DoSegment(item.Text);
                    ls.Clear();
                    if (words.Count != 0)
                    {
                        foreach (WordInfo wordInfo in words)
                        {
                            if (!ls.Contains(wordInfo.Word))
                            {
                                ls.Add(wordInfo.Word);
                                if (Datas.frequency[6].ContainsKey(wordInfo.Word))
                                {
                                    Datas.frequency[6][wordInfo.Word].now_nu++;
                                    Datas.frequency[6][wordInfo.Word].position.Add(Datas.now_time_count);
                                }
                                else
                                {
                                    words temp = new words(wordInfo.Word, Datas.now_time_count);

                                    Datas.frequency[6].Add(wordInfo.Word, temp);
                                }
                            }
                        }
                    }
                    Datas.total_data[6].Add(item);
                    Datas.total_num++;
                    Datas.now_time_count++;
                    if (Datas.now_time_count >= Datas.now_time_max)
                    {
                        Datas.frequency[Datas.now_time_read % 6] = Datas.frequency[6];
                        Datas.total_data[Datas.now_time_read % 6] = Datas.total_data[6];
                        Datas.frequency[6] = new Dictionary<string, 微博舆论.words>();
                        Datas.total_data[6] = new List<SinaJSON>();
                        //if(Datas.now_time_read>0)   
                        //算基础权重
                        for (i = 0; i < Datas.total_data[Datas.now_time_read % 6].Count; i++)//~~
                        {
                            string max;
                            re_item = Datas.total_data[Datas.now_time_read % 6].ElementAt(i);//~~
                            re_words = segment.DoSegment(re_item.Text);
                            //max = max_ocur(re_words,Datas.frequency[Datas.now_time_read % 6],i);
                            if (re_words.Count != 0)
                            {
                                max = re_words.ElementAt(0).Word;
                                foreach (WordInfo word in re_words)
                                {
                                    if (Datas.frequency[Datas.now_time_read % 6].ContainsKey(word.Word))
                                    {
                                        if (Datas.frequency[Datas.now_time_read % 6][max].now_nu < Datas.frequency[Datas.now_time_read % 6][word.Word].now_nu)
                                            max = word.Word;
                                    }
                                }
                                foreach (WordInfo temp in re_words)
                                {
                                    temp_quan = (0.5 + 0.5 * Datas.frequency[Datas.now_time_read % 6][temp.Word].now_nu / Datas.frequency[Datas.now_time_read % 6][max].now_nu);
                                    Datas.frequency[Datas.now_time_read % 6][temp.Word].weight_quan += (temp_quan * (Datas.weight_word * re_item.Attitudes_count + (1 - Datas.weight_word) * re_item.Comments_count));

                                }
                            }
                        }
                        //算bursty  待加用户权重
                        List<string> l_s = new List<string>();
                        List<int> l_i = new List<int>();
                        List<ju_word> l_julei;

                        int j;
                        if (Datas.frequency[(Datas.now_time_read + 5) % 6].Count > 0)
                        {
                            l_s.Clear();//突发度大于0
                            l_i.Clear();//在字典中的位置
                            for (i = 0; i < Datas.frequency[Datas.now_time_read % 6].Count; i++)
                            {
                                double wehit_cou = 0;
                                if (Datas.frequency[(Datas.now_time_read + 5) % 6].ContainsKey(Datas.frequency[Datas.now_time_read % 6].ElementAt(i).Key))
                                    wehit_cou = Datas.frequency[(Datas.now_time_read + 5) % 6][Datas.frequency[Datas.now_time_read % 6].ElementAt(i).Key].weight_quan;
                                Datas.frequency[Datas.now_time_read % 6].ElementAt(i).Value.bursty =
                                    (Datas.frequency[Datas.now_time_read % 6].ElementAt(i).Value.weight_quan - wehit_cou) / 2;
                                if (Datas.frequency[Datas.now_time_read % 6].ElementAt(i).Value.bursty > 0)
                                {
                                    l_s.Add(Datas.frequency[Datas.now_time_read % 6].ElementAt(i).Key);
                                    l_i.Add(i);
                                }

                            }
                            //距离聚类

                            l_julei = new List<ju_word>();
                            for (i = 0; i < l_s.Count; i++)
                            {
                                ju_word ju_t = new ju_word(l_s.ElementAt(i), Datas.frequency[Datas.now_time_read % 6][l_s.ElementAt(i)].position);
                                l_julei.Add(ju_t);
                            }

                            double f_z = 0;
                            double ju_temp;
                            double ju_min;
                            double fengzi = 0;
                            double fenmu = 0;
                            int temp_count;//the nunmber of the same
                            int ju_x = 0;
                            int ju_y = 0;
                            List<int> l_posi = new List<int>();
                            while (f_z < 15)
                            {
                                ju_min = 10000;
                                ju_x = 0;
                                ju_y = 0;
                                List<int> l_temp_pos = new List<int>();
                                for (i = 0; i < l_julei.Count; i++)
                                {
                                    for (j = i + 1; j < l_julei.Count; j++)
                                    {
                                        //zuo = l_julei.ElementAt(i).Split(',');
                                        //you = l_julei.ElementAt(j).Split(',');

                                        for (int x = 0; x < l_julei.ElementAt(i).word.Count; x++)
                                        {
                                            for (int y = 0; y < l_julei.ElementAt(j).word.Count; y++)
                                            {
                                                fengzi += (Datas.frequency[Datas.now_time_read % 6][l_julei.ElementAt(i).word.ElementAt(x)].bursty
                                                    * Datas.frequency[Datas.now_time_read % 6][l_julei.ElementAt(j).word.ElementAt(y)].bursty);

                                            }
                                        }
                                        fengzi = fengzi / (l_julei.ElementAt(i).word.Count + l_julei.ElementAt(i).word.Count);
                                        temp_count = 0;
                                        for (int ju_i = 0; ju_i < l_julei.ElementAt(i).position.Count; ju_i++)
                                        {
                                            if (l_julei.ElementAt(j).position.Contains(l_julei.ElementAt(i).position.ElementAt(ju_i)))
                                            {
                                                temp_count++;
                                                l_temp_pos.Add(l_julei.ElementAt(i).position.ElementAt(ju_i));
                                            }
                                        }
                                        fenmu = (double)temp_count / (double)Datas.now_time_max;
                                        ju_temp = fengzi / fenmu;
                                        if (ju_temp < ju_min)
                                        {
                                            ju_min = ju_temp;
                                            ju_x = i;
                                            ju_y = j;
                                            l_posi = new List<int>(l_temp_pos);
                                        }
                                        fengzi = 0;
                                        fenmu = 0;
                                        l_temp_pos.Clear();
                                    }
                                    te = l_julei.OrderBy(ju_word => ju_word.word.Count).ToList();
                                }
                                if (ju_x == 0 && ju_y == 0)
                                    break;
                                //聚类一个
                                for (i = 0; i < l_julei.ElementAt(ju_y).word.Count; i++)
                                    l_julei.ElementAt(ju_x).word.Add(l_julei.ElementAt(ju_y).word.ElementAt(i));
                                l_julei.ElementAt(ju_x).position = new List<int>(l_posi);
                                l_julei.RemoveAt(ju_y);
                                f_z = ju_min;
                            }
                            List<ju_word> temp = new List<ju_word>();

                            for (i = 0; i < l_julei.Count; i++)
                                if (l_julei.ElementAt(i).word.Count > 1)
                                    temp.Add(l_julei.ElementAt(i));
                            Datas.ju_list[Datas.now_time_read%6] = temp.OrderByDescending(ju_word => ju_word.position.Count).ToList();

                            //this.Invoke(new updateListboxFreq(doUpdateListboxFreq), new object[] { Datas.ju_list });
                            if (!Datas.ProgressBarCompelet)
                                Datas.ProgressBarCompelet = true;
                        }

                        Datas.now_time_count = 0;
                        Datas.now_time_read++;
                    }
                }
            }
        }

        private void listBoxTimeCir_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBoxKeyWords.Items.Clear();
            listBoxWeiBlogText.Items.Clear();
            int index = listBoxTimeCir.SelectedIndex;
            if (Datas.ju_list[index].Count > 0)
            {
                List< ju_word> ju = Datas.ju_list[index];
                foreach (ju_word item in ju)
                {
                    listBoxKeyWords.Items.Add(item);
                }
            }
        }

        private void listBoxKeyWords_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBoxWeiBlogText.Items.Clear();
            if (listBoxKeyWords.Items.Count == 0)
                return;
            int index = listBoxKeyWords.SelectedIndex;
            List<int> postionList = (listBoxKeyWords.SelectedItem as ju_word).position;
            foreach (int  item in postionList)
            {
                listBoxWeiBlogText.Items.Add(Datas.total_data[listBoxTimeCir.SelectedIndex][item].Text);
            }
        }

    }
}
