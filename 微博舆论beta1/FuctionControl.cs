using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;

namespace 微博舆论
{
    /// <summary>
    /// 功能控制,显示不同的窗体
    /// </summary>
    partial class Form1
    {
        #region 私有成员变量
        private bool displayEmotionAna = false;         //显示表情统计
        private bool displayFrequencyAna = false;       //显示词频分析
        private bool displaySetKindAna = false;         //显示设备分析
        #endregion

        #region 表情分析
        //表情值 结构体 
        struct EmoValue
        {
            public int female, male;
            public EmoValue(int m, int f)
            {
                female = f;
                male = m;
            }
        }

        //读取数据 显示到 datagridviewEmo
        void runDisplayEmotion()
        {
            BlogReader breader;
            //if (Datas.sinaJSONList.Count > 0)
            //    Datas.sinaJSONList.Clear();
            if (Datas.sinaJSONList.Count == 0)
            {
                breader = new BlogReader("temp5.dat");
                Datas.sinaJSONList = breader.ReadFromFile();
            }
            Datas.ProgressBarState = "读取完成,正在分析...";
            Datas.ProgresBarStyle = System.Windows.Forms.ProgressBarStyle.Marquee;
            string emo = "";
            //表情键
            List<string> emoKey = new List<string>();
            List<EmoValue> emoValue = new List<EmoValue>();
            using (FileStream fs = new FileStream("datas/emo.txt", FileMode.Open))
            {
                StreamReader sr = new StreamReader(fs);
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    if (line.Length > 0)
                    {
                        emoKey.Add(line);
                        emoValue.Add(new EmoValue());
                    }
                }
                sr.Close();
                fs.Close();
            }
            MatchCollection mCollection;
            foreach (SinaJSON item in Datas.sinaJSONList)
            {
                mCollection = Regex.Matches(item.Text, @"\[\w+\]");
                for (int i = 0; i < mCollection.Count; i++)
                {
                    emo = mCollection[i].Value;
                    //emo = emo.Substring(1, emo.Length - 2);
                    int index = emoKey.IndexOf(emo);
                    int male = 0, female = 0;
                    if (item.SinaUser.Gender.Equals("m"))
                        male++;
                    else
                        female++;
                    if (index != -1)
                        emoValue[index] = new EmoValue(emoValue[index].male + male, emoValue[index].female + female);
                }
            }


            app = new Excel.Application();
            workbooks = app.Workbooks as Excel.Workbooks;
            workbook = workbooks.Add(Type.Missing);
            app.DisplayAlerts = false;

            //删除 多余Sheet
            foreach (Worksheet ws in workbook.Worksheets)
                if (ws != app.ActiveSheet)
                {
                    ws.Delete();
                }
            foreach (Chart cht in workbook.Charts)
                cht.Delete();

            //创建一个Sheet,存数据
            //worksheet = (Worksheet)workbook.
            //    Worksheets.Add(Type.Missing, workbook.ActiveSheet,
            //    Type.Missing, Type.Missing);
            worksheet = workbook.Worksheets[1];
            worksheet.Name = "数据";


            int worksheetIndex = 0;
            for (int i = 0; i < emoKey.Count; i++)
            {
                if (emoValue[i].male > 0 || emoValue[i].female > 0)
                {
                    worksheet.Cells[i + 1, 1] = emoKey[i];
                    worksheet.Cells[i + 1, 2] = emoValue[i].male;
                    worksheet.Cells[i + 1, 3] = emoValue[i].female;
                    worksheetIndex++;
                }
            }
            // TODO: 生成一个统计图对象：
            Chart xlChart = (Chart)workbook.Charts.
                Add(Type.Missing, worksheet, Type.Missing, Type.Missing);

            // TODO: 设定数据来源
            Range cellRange = (Range)worksheet.Cells[1, 1];
            // TODO: 通过向导生成Chart
            xlChart.ChartWizard(cellRange.CurrentRegion,
                XlChartType.xl3DColumn, Type.Missing,
                XlRowCol.xlColumns, 1, 0, true,
                "表情比较", "表情", "数量");
            // TODO: 设置统计图Sheet的名称
            xlChart.Name = "统计";
            // TODO: 让12个Bar都显示不同的颜色
            ChartGroup grp = (ChartGroup)xlChart.ChartGroups(1);
            grp.GapWidth = 20;
            grp.VaryByCategories = true;
            // TODO: 让Chart的条目的显示形状变成圆柱形，并给它们显示加上数据标签
            Series s1 = (Series)grp.SeriesCollection(1);
            s1.Name = "男";
            s1.BarShape = XlBarShape.xlCylinder;
            s1.HasDataLabels = true;
            Series s = (Series)grp.SeriesCollection(2);
            s.BarShape = XlBarShape.xlCylinder;
            s.HasDataLabels = true;
            s.Name = "女";
            // TODO: 设置统计图的标题和图例的显示
            xlChart.Legend.Position = XlLegendPosition.xlLegendPositionTop;
            xlChart.ChartTitle.Font.Size = 24;
            xlChart.ChartTitle.Shadow = false;
            xlChart.ChartTitle.Border.LineStyle = XlLineStyle.xlContinuous;
            // TODO: 设置两个轴的属性，Excel.XlAxisType.xlValue对应的是Y轴，Excel.XlAxisType.xlCategory对应的是X轴
            Axis valueAxis = (Axis)xlChart.Axes(XlAxisType.xlValue, XlAxisGroup.xlPrimary);
            valueAxis.AxisTitle.Orientation = -90;
            Axis categoryAxis = (Axis)xlChart.Axes(XlAxisType.xlCategory, XlAxisGroup.xlPrimary);
            categoryAxis.AxisTitle.Font.Name = "宋体";
            //--------------------------------------------------

            //workbook.SaveAs(sPath, Type.Missing, Type.Missing,
            //            Type.Missing, Type.Missing, Type.Missing, XlSaveAsAccessMode.xlNoChange,
            //        Type.Missing, Type.Missing, Type.Missing, Type.Missing,
            //        Type.Missing);

            xlChart.CopyPicture(XlPictureAppearance.xlScreen, XlCopyPictureFormat.xlPicture);
            IntPtr hwnd = (IntPtr)app.Hwnd;
            Bitmap b = null;
            try
            {
                if (OpenClipboard(hwnd))
                {
                    IntPtr data = GetClipboardData(14); // CF_ENHMETAFILE      14 
                    if (data != IntPtr.Zero)
                    {
                        using (Metafile mf = new Metafile(data, true))
                        {
                            b = new Bitmap(mf);
                        }
                    }
                }
            }
            finally
            {
                CloseClipboard();
                //clear
                workbook.Close(Type.Missing, Type.Missing, Type.Missing);
                app.Workbooks.Close();
                app.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(app);
                workbook = null;
                app = null;
                xlChart = null;
                GC.Collect();
            }

            this.Invoke(new updateDataGridViewEmo(doUpdateDataGridViewEmo), new object[] { emoKey, emoValue, b });

        }
        delegate void updateDataGridViewEmo(List<string> emoKey, List<EmoValue> emoValue,Bitmap bitmap);
            Excel.Application app;
            Excel.Workbooks workbooks;
            Excel.Workbook workbook;
            Excel.Worksheet worksheet;

        void doUpdateDataGridViewEmo(List<string> emoKey, List<EmoValue> emoValue,Bitmap bitmap)
        {
            dataGridViewEmo.Rows.Clear();
            for (int i = 0; i < emoKey.Count; i++)
            {
                if (emoValue[i].male > 0 || emoValue[i].female > 0)
                {
                    dataGridViewEmo.Rows.Add(emoKey[i], emoValue[i].male, emoValue[i].female, emoValue[i].male + emoValue[i].female);
                }
            }
            pictureBoxEmo.Image = bitmap;
            dataGridViewEmo.Visible = true;
            Datas.ProgressBarCompelet = true;
        }

        [DllImport("User32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool OpenClipboard(IntPtr hWndNewOwner);

        [DllImport("User32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseClipboard();

        [DllImport("User32.dll")]
        public static extern IntPtr GetClipboardData(System.UInt32 uFormat); 

        public bool DisplayEmotion
        {
            set
            {
                if (value && !this.displayEmotionAna)
                {
                    this.displayEmotionAna = true;

                    Thread threadDisplayEmo = new Thread(new ThreadStart(runDisplayEmotion));
                    threadDisplayEmo.IsBackground = true;
                    threadDisplayEmo.Start();
                }
            }
            get { return this.displayEmotionAna; }
        }
        #endregion

        #region 词频分析

        delegate void updateDataGridViewFreq();
        public void runDisplayFrequency()
        {
            Datas.init();
            Thread t1 = new Thread(new ThreadStart(this.readdata));
            Thread t2 = new Thread(new ThreadStart(new BlogReader().ReadFromFile_thread));
            t1.IsBackground = true;
            t2.IsBackground = true;
            t2.Start();
            t1.Start();

            //Datas.ProgressBarState = "读取完成,正在分析...";
            Datas.ProgresBarStyle = System.Windows.Forms.ProgressBarStyle.Marquee;

            //this.Invoke(new updateDataGridViewFreq(doUpdateDataGridViewFreq));
        }
        void doUpdateDataGridViewFreq()
        {
            dataGridViewFren.Rows.Clear();

            dataGridViewFren.Visible = true;
            Datas.DataGridViewFreqVisiable = true;
            Datas.ProgressBarCompelet = true;
        }

        public bool DisplayFrequency
        {
            set
            {
                if (value && !this.displayFrequencyAna)
                {
                    this.displayFrequencyAna = true;
                    dataGridViewFren.Rows.Clear();
                    Thread threadDisplayFren = new Thread(new ThreadStart(runDisplayFrequency));
                    threadDisplayFren.IsBackground = true;
                    threadDisplayFren.Start();
                }
            }
            get { return this.displayFrequencyAna; }
        }
        #endregion

        #region 设备分析
        delegate void updateDataGridViewSetKind(List<string> key, List<int> value, List<SetKind> setKind);
        public bool DisplaySetKind
        {
            set
            {
                if (value && !this.displaySetKindAna)
                {
                    this.displaySetKindAna = true;

                    Thread threadDisplayEmo = new Thread(new ThreadStart(runDisplaySetKind));
                    threadDisplayEmo.IsBackground = true;
                    threadDisplayEmo.Start();
                }
            }
            get { return this.displaySetKindAna; }
        }

        void runDisplaySetKind()
        {

            //读取设备词典
            if (WordSetKind.Words.Count == 0)
            {
                using (FileStream fs = new FileStream("datas/WordsSetKind.dat", FileMode.Open))
                {
                    BinaryFormatter binFormatter = new BinaryFormatter();
                    WordSetKind.Words = (Dictionary<string, SetAttribute>)binFormatter.Deserialize(fs);
                    fs.Close();
                }
            }

            BlogReader breader;
            //if (Datas.sinaJSONList.Count > 0)
            //    Datas.sinaJSONList.Clear();
            if (Datas.sinaJSONList.Count == 0)
            {
                breader = new BlogReader(@"temp5.dat");//G:\11\SerializedData\temp3.dat");//("temp5.dat");//
                Datas.sinaJSONList = breader.ReadFromFile();
            }
            Datas.ProgressBarState = "读取完成,正在分析...";
            Datas.ProgresBarStyle = System.Windows.Forms.ProgressBarStyle.Marquee;
            List<string> key = new List<string>();
            List<int> value = new List<int>();
            List<SetKind> setKind = new List<SetKind>();
            foreach (SinaJSON item in Datas.sinaJSONList)
            {
                string set = Regex.Match(item.Source, @">[^<]+").Value;
                set = set.Substring(1, set.Length - 1);
                int index = key.IndexOf(set);
                //存在该条目
                if (index != -1)
                {
                    value[index] = value[index] + 1;
                }
                else
                {

                    if (WordSetKind.Words.ContainsKey(set))
                    {
                        setKind.Add(WordSetKind.Words[set].setKind);
                    }
                    else
                    {
                        setKind.Add(SetKind.未知);
                    }
                    key.Add(set);
                    value.Add(1);
                }
            }

            this.Invoke(new updateDataGridViewSetKind(doUpdateDataGridViewSetKind), new object[] { key, value, setKind});
        }
        void doUpdateDataGridViewSetKind(List<string> key, List<int> value,List<SetKind> setKind)
        {
            //FileStream fs = new FileStream("datas/手机.txt", FileMode.Create);
            //StreamWriter sw = new StreamWriter(fs);

            int totalNum = Datas.sinaJSONList.Count;
            for (int i = 0; i < key.Count; i++)
            {
                dataGridViewSetKind.Rows.Add(key[i], value[i], (float)value[i] / totalNum * 100, setKind[i]);
                //sw.WriteLine(key[i]);
            }
            //sw.Close();
            //fs.Close();
            dataGridViewSetKind.Visible = true;
            Datas.ProgressBarCompelet = true;


            //picturebox update

        }
        #endregion
    }
}