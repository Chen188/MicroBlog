using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 微博舆论
{
    public partial class 终端分类Dict : Form
    {
        List<SingleBrand> listBrands;
        public 终端分类Dict()
        {
            InitializeComponent();
        }

        private void CloseWindowBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnMinForm_Click(object sender, EventArgs e)
        {
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
        }

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

        private void btnSeria_Click(object sender, EventArgs e)
        {
            WordSetKind.Serialize();
        }
        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (listBoxSet.Items.Count > 0)
                return;

            using (FileStream fs = new FileStream("datas/WordsSetKind.dat", FileMode.Open))
            {
                BinaryFormatter binFormatter = new BinaryFormatter();
                WordSetKind.Words = (Dictionary<string, SetAttribute>)binFormatter.Deserialize(fs);
                foreach (var item in WordSetKind.Words)
                {
                    listBoxSet.Items.Add(item.Key);
                }
                fs.Close();
            }
            if (listBrands==null || listBrands.Count == 0)
            {
                using (FileStream fs = new FileStream("datas/mobileDatas.dat", FileMode.Open))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    listBrands = bf.Deserialize(fs) as List<SingleBrand>;
                    fs.Close();
                }
            }

            foreach (SingleBrand item in listBrands)
            {
                listBoxBrands.Items.Add(item.BrandName);
            }

            listBoxOS.Items.Add(OperatingSystem.Android);
            listBoxOS.Items.Add(OperatingSystem.iOS);
            listBoxOS.Items.Add(OperatingSystem.Windows);
            listBoxOS.Items.Add(OperatingSystem.WindowsPhone);
            listBoxOS.Items.Add(OperatingSystem.塞班);
            listBoxOS.Items.Add(OperatingSystem.其它);

            listBoxSetKind.Items.Add(SetKind.手机);
            listBoxSetKind.Items.Add(SetKind.平板);
            listBoxSetKind.Items.Add(SetKind.PC);
            listBoxSetKind.Items.Add(SetKind.未知);

            if (listBoxSet.Items.Count > 0)
                listBoxSet.SelectedIndex = 0;

        }

        private void btnAutoAna_Click(object sender, EventArgs e)
        {
            Thread threadAutoAna = new Thread(new ThreadStart(doAutoAna));
            threadAutoAna.IsBackground = true;
            threadAutoAna.Start();
            ProgressForm pf = new ProgressForm();
            Datas.ProgresBarStyle = ProgressBarStyle.Marquee;
            pf.ShowDialog();
            (sender as Button).Enabled = false;
        }

        void doAutoAna()
        {
            int anaCount = 0;
            if (WordSetKind.Words.Count != 0)
            {
                WordSetKind.Words.Clear();
            }
            if (Datas.sinaJSONList.Count == 0)
            {
                BlogReader br = new BlogReader(@"temp5.dat");
                Datas.sinaJSONList = br.ReadFromFile();
            }
            if (listBrands == null)
                listBrands = new List<SingleBrand>();
            if (listBrands.Count == 0)
            {
                using (FileStream fs = new FileStream("datas/mobileDatas.dat", FileMode.Open))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    listBrands = bf.Deserialize(fs) as List<SingleBrand>;
                    fs.Close();
                }
            }

            foreach (SinaJSON item in Datas.sinaJSONList)
            {
                anaCount++;
                Datas.ProgressBarState = "正在分析第" + anaCount + "个";
                string source = Regex.Match(item.Source, @">[^<]+").Value;
                source = source.Substring(1, source.Length - 1);

                SetAttribute attr = new SetAttribute();

                short position = 0;     //位置变量

                foreach (SingleBrand singleBrand in listBrands)
                {
                    string[] name = singleBrand.BrandName.Replace("）", "").Split('（');
                    if (name.Length == 2)
                    {
                        if (source.IndexOf(name[0]) != -1 || source.IndexOf(name[1]) != -1)
                        {
                            attr.BrandName = singleBrand.BrandName;
                            break;
                        }
                    }
                    else
                    {
                        if (source.IndexOf(name[0]) != -1)
                        {
                            attr.BrandName = singleBrand.BrandName;
                            break;
                        }
                    }
                    position++;
                }
                //品牌匹配成功
                if (attr.BrandName != null)
                {
                    foreach (Model model in listBrands[position].ListModels)
                    {
                        //存在型号
                        if (model.ModelName != null && model.ModelName.IndexOf(source) != -1)
                        {
                            if (model.ModelName.IndexOf("android", StringComparison.CurrentCultureIgnoreCase) != -1)
                            {
                                attr.OS = OperatingSystem.Android;
                            }
                            else
                                if (model.ModelName.IndexOf("iOS", StringComparison.CurrentCultureIgnoreCase) != -1)
                                    attr.OS = OperatingSystem.iOS;
                            if (attr.OS != OperatingSystem.未知)
                            {
                                attr.setKind = SetKind.手机;
                                break;
                            }
                        }
                    }

                    if (attr.OS == OperatingSystem.未知)
                    {
                        attr.OS = OperatingSystem.Android;
                    }
                    if (attr.setKind == SetKind.未知)
                        attr.setKind = SetKind.手机;
                }
                //品牌匹配失败
                else
                {
                    //大系统确定
                    if (source.IndexOf("android", StringComparison.CurrentCultureIgnoreCase) != -1)
                    {
                        attr.OS = OperatingSystem.Android;
                        attr.setKind = SetKind.未知;
                    }
                    else
                        if (source.IndexOf("ios", StringComparison.CurrentCultureIgnoreCase) != -1)
                        {
                            attr.OS = OperatingSystem.iOS;
                            attr.setKind = SetKind.未知;
                        }
                        else
                            if (source.IndexOf("win8", StringComparison.CurrentCultureIgnoreCase) != -1)
                            {
                                attr.OS = OperatingSystem.Windows;
                                attr.setKind = SetKind.PC;
                            }
                            else
                            {
                                attr.OS = OperatingSystem.其它;
                                attr.setKind = SetKind.未知;
                            }
                    attr.BrandName = "其它";
                }
                if (source.IndexOf("window", StringComparison.CurrentCultureIgnoreCase) != -1)
                    attr.OS = OperatingSystem.WindowsPhone;
                else
                    if (source.IndexOf("s60", StringComparison.CurrentCultureIgnoreCase) != -1
                        || source.IndexOf("s40", StringComparison.CurrentCultureIgnoreCase) != -1
                        || source.IndexOf("s30", StringComparison.CurrentCultureIgnoreCase) != -1)
                        attr.OS = OperatingSystem.塞班;

                if (source.IndexOf("phone", StringComparison.CurrentCultureIgnoreCase) != -1)
                {
                    attr.setKind = SetKind.手机;
                }
                if (source.IndexOf("iphone", StringComparison.CurrentCultureIgnoreCase) != -1)
                {
                    attr.OS = OperatingSystem.iOS;
                    attr.setKind = SetKind.手机;
                }
                else
                    if (source.IndexOf("ipad", StringComparison.CurrentCultureIgnoreCase) != -1)
                    {
                        attr.OS = OperatingSystem.iOS;
                        attr.setKind = SetKind.平板;
                    }

                if (source.IndexOf("手机", StringComparison.CurrentCultureIgnoreCase) != -1
                    || source.IndexOf("note", StringComparison.CurrentCultureIgnoreCase) != -1)
                {
                    attr.setKind = SetKind.手机;
                }
                else
                    if (source.IndexOf("平板", StringComparison.CurrentCultureIgnoreCase) != -1
                        || source.IndexOf("tab", StringComparison.CurrentCultureIgnoreCase) != -1)
                    {
                        attr.setKind = SetKind.平板;
                    }

                if (attr.OS == OperatingSystem.其它 && source.IndexOf("智能") != -1)
                {
                    attr.OS = OperatingSystem.Android;
                }

                WordSetKind.AddWord(source, attr);
            }

            Datas.ProgressBarCompelet = true;
        }

        private void listBoxSet_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetAttribute setAttr = WordSetKind.Words[listBoxSet.Items[listBoxSet.SelectedIndex].ToString()];
            int index;
            index = listBoxBrands.Items.IndexOf(setAttr.BrandName);
            listBoxBrands.SelectedIndex = index;

            index = listBoxOS.Items.IndexOf(setAttr.OS);
            listBoxOS.SelectedIndex = index;

            index = listBoxSetKind.Items.IndexOf(setAttr.setKind);
            listBoxSetKind.SelectedIndex = index;
        }

        private void listBoxBrands_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listBoxBrands.SelectedIndex == -1)
                return;
            string content;
            ChangeItemContent changeItemContentForm = new ChangeItemContent();
            changeItemContentForm.Owner = this;
            changeItemContentForm.Content = listBoxBrands.Items[listBoxBrands.SelectedIndex].ToString();
            changeItemContentForm.ShowDialog();
            content = changeItemContentForm.Content;
            if (content.Length > 0)
            {
                listBoxBrands.Items[listBoxBrands.SelectedIndex] = content;
                SetAttribute sa=WordSetKind.Words[listBoxSet.SelectedItem.ToString()];
                sa.BrandName = content;
                WordSetKind.Words[listBoxSet.SelectedItem.ToString()] = sa;
            }
        }

        private void 终端分类Dict_FormClosing(object sender, FormClosingEventArgs e)
        {
            Datas.ProgressBarCompelet = false;
        }
    }

    public enum SetKind { 未知 = 0, 手机, 平板, PC, 应用 }
    public enum OperatingSystem {未知=0, 其它, Android, iOS, WindowsPhone, 塞班, Windows }

    [Serializable]
    public struct SetAttribute
    {
        /// <summary>
        /// 品牌
        /// </summary>
        public string BrandName;
        /// <summary>
        /// 操作系统
        /// </summary>
        public OperatingSystem OS;
        /// <summary>
        /// 设备类型
        /// </summary>
        public SetKind setKind; 
    }

    public class WordSetKind
    {
        /// <summary>
        /// 分类词典
        /// </summary>
        public static Dictionary<string, SetAttribute> Words = new Dictionary<string, SetAttribute>();

        public static string path="datas/WordsSetKind.dat";
        public WordSetKind(string _path)
        {
            path = _path;
        }

        /// <summary>
        /// 添加单词
        /// </summary>
        /// <param name="word">单词</param>
        /// <param name="attrs">属性s</param>
        public static void AddWord(string word,SetAttribute attrs)
        {
            if (Words.ContainsKey(word))
            {
                SetAttr(word, attrs);
                return;
            }
            Words.Add(word, attrs);
        }
        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="word">单词</param>
        /// <param name="attrs">属性s</param>
        public static void SetAttr(string word, SetAttribute attrs)
        {
            if (Words.ContainsKey(word))
            {
                Words[word] = attrs;
            }
            else
            {
                Words.Add(word, attrs);
            }
        }
        /// <summary>
        /// 序列化数据到本地文件
        /// </summary>
        /// <param name="path">保存文件路径</param>
        public static void Serialize(string path)
        {
            if (Words.Count == 0)
                return;
            using (FileStream outputFileStream = new FileStream(path, FileMode.Create))
            {
                //序列化到文件
                BinaryFormatter binFormatter = new BinaryFormatter();
                binFormatter.Serialize(outputFileStream, Words);
                outputFileStream.Close();
            }
            MessageBox.Show("保存成功");
        }
        /// <summary>
        /// 序列化数据到本地文件
        /// </summary>
        public static void Serialize()
        {
            Serialize(path);
        }
        /// <summary>
        /// 从本地文件读取词典文件
        /// </summary>
        /// <param name="path">词典文件路径</param>
        /// <returns></returns>
        public static Dictionary<string, List<string>> Deserialize(string path)
        {
            using (FileStream inputFileStream = new FileStream(path, FileMode.Open))
            {
                //反序列化到内存
                BinaryFormatter binFormatter = new BinaryFormatter();
                Dictionary<string, List<string>> dict = (Dictionary<string, List<string>>)binFormatter.Deserialize(inputFileStream);
                inputFileStream.Close();
                return dict;
            }
        }
        /// <summary>
        /// 从本地文件读取词典文件
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, List<string>> Deserialize()
        {
            using (FileStream inputFileStream = new FileStream(path, FileMode.Open))
            {
                //反序列化到内存
                BinaryFormatter binFormatter = new BinaryFormatter();
                Dictionary<string, List<string>> dict = (Dictionary<string, List<string>>)binFormatter.Deserialize(inputFileStream);
                inputFileStream.Close();
                return dict;
            }
        }
    }
}
