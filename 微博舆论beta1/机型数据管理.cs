using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Text.RegularExpressions;
using System.Threading;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace 微博舆论
{

    [Serializable]
    struct Model
    {
        /// <summary>
        /// 型号名称
        /// </summary>
        public string ModelName;
        /// <summary>
        /// 操作系统
        /// </summary>
        public string OS;
        /// <summary>
        /// 屏幕尺寸
        /// </summary>
        public float ScreenSize;
        /// <summary>
        /// 上市时间
        /// </summary>
        public DateTime TimeToMarket;
        /// <summary>
        /// 分辨率
        /// </summary>
        public Size Resolution;
        /// <summary>
        /// CPU型号
        /// </summary>
        public string CPUModel;
        /// <summary>
        /// CPU核心数
        /// </summary>
        public short CPUCoreNum;
        /// <summary>
        /// CPU 频率
        /// </summary>
        public float CPUFreq;
    }
    [Serializable]
    struct SingleBrand
    {
        /// <summary>
        /// 品牌名称
        /// </summary>
        public string BrandName;
        /// <summary>
        /// 品牌链接
        /// </summary>
        public string BrandAddress;
        /// <summary>
        /// 品牌子型号链接
        /// </summary>
        public List<string> ListBrandModelsAddress;
        /// <summary>
        /// 型号属性集,对应modelsAddress
        /// </summary>
        public List<Model> ListModels;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="bra">品牌名称</param>
        /// <param name="lnk">品牌链接</param>
        /// <param name="add">品牌子型号链接</param>
        /// <param name="models">型号集合</param>
        public SingleBrand(string bra, string lnk, List<string> add, List<Model> models)
        {
            this.BrandName = bra;
            this.ListBrandModelsAddress = add;
            this.BrandAddress = lnk;
            this.ListModels = models;
        }
    }
    public partial class 机型数据管理 : Form
    {
        /// <summary>
        /// 读取数据未被打断(数据读取窗口未提前关闭)
        /// </summary>
        bool readDataComplete = false;
        private void btnGetDataFromServer_Click(object sender, EventArgs e)
        {
            //GetBrands();
            if (MessageBox.Show("获取数据需要较长时间,与您所处网络环境很大关系\n点击\"确定\"继续", "提示", MessageBoxButtons.OKCancel
                ).Equals(DialogResult.Cancel))
            {
                return;
            }
            Thread tGetBrands = new Thread(new ThreadStart(GetBrands));
            tGetBrands.IsBackground = true;
            tGetBrands.Start();
            btnGetDataFromServer.Enabled = false;
            ProgressForm pf = new ProgressForm();
            Datas.ProgresBarStyle = ProgressBarStyle.Marquee;
            pf.ShowDialog();
        }
        public 机型数据管理()
        {
            InitializeComponent();

            listBrands = new List<SingleBrand>();
        }

        WebRequest webRequest;
        HttpWebResponse httpWebResponse;

        /// <summary>
        /// 品牌集合
        /// </summary>
        List<SingleBrand> listBrands;

        public void GetBrands()
        {
            //webBrowser.DocumentCompleted += webBrowser_DocumentCompleted;
            ////获取所有品牌元素http://list.jd.com/9987-653-655-15127-0-0-0-0-0-0-1-1-1-1-1-72-4137-33.html
            //webBrowser.Navigate("http://list.jd.com/9987-653-655-0-0-0-0-0-0-0-1-1-1-1-1-72-4137-33.html");
            webRequest = WebRequest.Create("http://list.jd.com/9987-653-655-0-0-0-0-0-0-0-1-1-1-1-1-72-4137-33.html");
            webRequest.Method = "get";

            httpWebResponse = (HttpWebResponse)webRequest.GetResponse();
            Stream stream = httpWebResponse.GetResponseStream();
            StreamReader sr = new StreamReader(stream, Encoding.Default);
            List<string> content = new List<string>();
            string brand;
            string line;
            int modelCount = 0;
            while (!sr.EndOfStream)
            {
                line = sr.ReadLine();
                //处理品牌
                if (line.StartsWith("<div class='content'>"))
                {
                    sr.ReadLine();
                    brand = sr.ReadLine();
                    //<a id='15127' href='9987-653-655-15127-0-0-0-0-0-0-1-1-1-1-1-72-4137-33.html' >三星（SAMSUNG）</a>
                    MatchCollection matchBrand = Regex.Matches(brand, @"href='[^<]+");
                    for (int i = 0; i < matchBrand.Count; i++)
                    {
                        string[] var = matchBrand[i].Value.Split('\'');
                        listBrands.Add(new SingleBrand(var[2].Substring(2), var[1], new List<string>(), new List<Model>()));

                        Datas.ProgressBarState = "获取品牌:" + listBrands.Count + "个";
                        sr.Close();
                    }
                    break;
                }
            }
            httpWebResponse.Close();

            //获取所有品牌的所有型号
            for (int i = 0; i < listBrands.Count; i++)//3;i++)//listBrands.Count; i++)
            {
                webRequest = WebRequest.Create("http://list.jd.com/" + listBrands[i].BrandAddress);
                //List<string> singleBrand = new List<string>();
                webRequest.Method = "get";
                bool hasNextPage = true;
                while (hasNextPage)
                {
                    httpWebResponse = (HttpWebResponse)webRequest.GetResponse();
                    stream = httpWebResponse.GetResponseStream();
                    sr = new StreamReader(stream, Encoding.Default);
                    string adds;
                    while (!sr.EndOfStream)
                    {
                        line = sr.ReadLine();
                        //处理产品地址
                        if (line.StartsWith("<ul class=\"list-h\">"))
                        {
                            while (!line.StartsWith("</ul>"))
                            {
                                line = sr.ReadLine();
                                if (line.IndexOf("没有") != -1)
                                {
                                    break;
                                }
                                adds = Regex.Match(line, @"href='[^<]+").Value;
                                if (adds.Length > 0)
                                {
                                    listBrands[i].ListBrandModelsAddress.Add(adds.Split('\'')[1]);
                                }
                                modelCount++;
                                Datas.ProgressBarState = "获取品牌:" + listBrands.Count + "个,型号:" + modelCount + "个";
                            }

                            //处理下一页地址
                            line = sr.ReadLine();
                            while (!line.StartsWith("<div class=\"pagin fr\">"))
                            {
                                line = sr.ReadLine();
                            }
                            line = sr.ReadLine();
                            //没有下一页,提前跳转
                            if (line.StartsWith("</div>"))
                            {
                                sr.Close();
                                hasNextPage = false;
                                break;
                            }
                            while (!line.StartsWith("</div>"))
                            {
                                //得到当前页地址,然后读取下一页的地址
                                if (line.IndexOf("current") != -1)
                                {
                                    line = sr.ReadLine();
                                    adds = Regex.Match(sr.ReadLine(), @"href='[^<]+").Value;
                                    //地址为空,不存在下一页
                                    if (adds.Length == 0)
                                    {
                                        hasNextPage = false;
                                        sr.Close();
                                        break;
                                    }
                                    else
                                    {
                                        //得到下一页地址
                                        webRequest = WebRequest.Create("http://list.jd.com/" + adds.Split('\'')[1]);
                                    }
                                    //关闭流,跳出
                                    sr.Close();
                                    break;
                                }
                                else
                                {
                                    line = sr.ReadLine();
                                }
                            }

                            break;
                        }
                    }
                }
            }
            List<string> detail = new List<string>();
            Datas.ProgressBarState = "数据获取完成,正在处理中...";
            modelCount = 1;
            //GetModels
            for (int i = 0; i < listBrands.Count; i++)
            {
                for (int j = 0; j < listBrands[i].ListBrandModelsAddress.Count; j++)
                {
                    Datas.ProgressBarState = "数据获取完成,正在处理第" + modelCount++ + "个";
                    try
                    {
                        webRequest = WebRequest.Create(listBrands[i].ListBrandModelsAddress[j]);
                        webRequest.Method = "get";
                        httpWebResponse = (HttpWebResponse)webRequest.GetResponse();
                        stream = httpWebResponse.GetResponseStream();
                    }
                    catch (Exception e)
                    {
                        if (e.Message.Equals("操作超时"))
                        {
                            j--;
                            continue;
                        }
                        else
                        {
                            MessageBox.Show(e.ToString());
                        }
                    }
                    sr = new StreamReader(stream, Encoding.Default);

                    while (!sr.EndOfStream)
                    {
                        line = sr.ReadLine();
                        //商品detail-2
                        if (!line.StartsWith("			<div class=\"mc  hide\""))
                        {
                            continue;
                        }
                        MatchCollection mc = Regex.Matches(line, @">[^<]+");
                        foreach (System.Text.RegularExpressions.Match item in mc)
                        {
                            detail.Add(item.Value.Substring(1, item.Value.Length - 1));
                        }

                        Model model = new Model();
                        int index = detail.IndexOf("型号");
                        if (index != -1)
                        {
                            model.ModelName = detail[index + 1];
                            bool alreadyExit = false;
                            for (int k = 0; k < listBrands[i].ListModels.Count; k++)
                            {
                                if (listBrands[i].ListModels[k].ModelName != null && listBrands[i].ListModels[k].ModelName.Equals(model.ModelName))
                                {
                                    alreadyExit = true;
                                    break;
                                }
                            }
                            if (alreadyExit)
                                break;
                        }
                        index = detail.IndexOf("操作系统");
                        if (index != -1)
                            model.OS = detail[index + 1];
                        index = detail.IndexOf("屏幕尺寸");
                        if (index != -1)
                        {
                            try
                            {
                                model.ScreenSize = float.Parse(Regex.Match(detail[index + 1], @"\d?\.?\d").Value);
                            }
                            catch { }
                        }
                        index = detail.IndexOf("上市时间");
                        if (index != -1)
                        {
                            mc = Regex.Matches(detail[index + 1], @"\d+");
                            if (mc.Count == 2)
                            {
                                model.TimeToMarket = new DateTime(Convert.ToInt32(mc[0].Value), Convert.ToInt32(mc[1].Value), 1);
                            }
                            else if (mc.Count == 1)
                            {
                                model.TimeToMarket = new DateTime(Convert.ToInt32(mc[0].Value), 1, 1);
                            }
                        }

                        index = detail.IndexOf("分辨率");
                        if (index != -1)
                        {
                            mc = Regex.Matches(detail[index + 1], @"\d+");
                            if (mc.Count == 2)
                            {
                                model.Resolution = new Size(Convert.ToInt32(mc[0].Value), Convert.ToInt32(mc[1].Value));
                            }
                        }
                        index = detail.IndexOf("CPU型号");
                        if (index != -1)
                        {
                            model.CPUModel = detail[index + 1];
                        }
                        index = detail.IndexOf("CPU核数");
                        if (index != -1)
                        {
                            short coreNum = 0;
                            switch (detail[index + 1][0])
                            {
                                case '单':
                                    coreNum = 1;
                                    break;
                                case '双':
                                    coreNum = 2;
                                    break;
                                case '四':
                                    coreNum = 4;
                                    break;
                                case '六':
                                    coreNum = 6;
                                    break;
                                default:
                                    break;

                            }
                            model.CPUCoreNum = coreNum;
                        }
                        index = detail.IndexOf("CPU频率");
                        if (index != -1)
                        {
                            try
                            {
                                model.CPUFreq = float.Parse(Regex.Match(detail[index + 1], @"\d?\.?\d").Value);
                            }
                            catch { }
                        }

                        listBrands[i].ListModels.Add(model);
                        sr.Close();
                        break;
                    }
                    httpWebResponse.Close();
                    detail.Clear();
                }
            }

            Datas.ProgressBarCompelet = true;
            readDataComplete = true;
        }

        private void btnSaveData_Click(object sender, EventArgs e)
        {
            if (!readDataComplete)
                return;
            using (FileStream fs = new FileStream("datas/mobileDatas.dat", FileMode.Create))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, listBrands);
                fs.Close();
            }
            MessageBox.Show("保存成功");
        }

        private void btnReadLocalFile_Click(object sender, EventArgs e)
        {
            listBoxBrand.Items.Clear();
            using (FileStream fs = new FileStream("datas/mobileDatas.dat", FileMode.Open))
            {
                BinaryFormatter bf = new BinaryFormatter();
                listBrands = bf.Deserialize(fs) as List<SingleBrand>;
                fs.Close();
            }
            for (int i = 0; i < listBrands.Count; i++)
            {
                listBoxBrand.Items.Add(listBrands[i].BrandName);
            }
        }

        private void listBoxBrand_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBoxModels.Items.Clear();
            foreach (Model item in listBrands[listBoxBrand.SelectedIndex].ListModels)
            {
                if (item.ModelName != null)
                    listBoxModels.Items.Add(item.ModelName);
            }
            listBoxModels.SelectedIndex = 0;
        }

        private void listBoxModels_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBoxModel.Items.Clear();
            Model m = listBrands[listBoxBrand.SelectedIndex].ListModels[listBoxModels.SelectedIndex];
            listBoxModel.Items.Add("操作系统:" + m.OS);
            listBoxModel.Items.Add("屏幕尺寸:" + m.ScreenSize);
            listBoxModel.Items.Add("分辨率:" + m.Resolution.Width + "*" + m.Resolution.Height);
            listBoxModel.Items.Add("CPU型号:" + m.CPUModel);
            listBoxModel.Items.Add("CPU核心数:" + m.CPUCoreNum);
            listBoxModel.Items.Add("CPU频率:" + m.CPUFreq);
        }

        private void btnDownloadHTML_Click(object sender, EventArgs e)
        {
            Thread tGetBrands = new Thread(new ThreadStart(doGetHTML));
            tGetBrands.IsBackground = true;
            tGetBrands.Start();
            ProgressForm pf = new ProgressForm();
            Datas.ProgresBarStyle = ProgressBarStyle.Marquee;
            pf.ShowDialog();
        }
        void doGetHTML()
        {
            //webBrowser.DocumentCompleted += webBrowser_DocumentCompleted;
            ////获取所有品牌元素http://list.jd.com/9987-653-655-15127-0-0-0-0-0-0-1-1-1-1-1-72-4137-33.html
            //webBrowser.Navigate("http://list.jd.com/9987-653-655-0-0-0-0-0-0-0-1-1-1-1-1-72-4137-33.html");
            webRequest = WebRequest.Create("http://list.jd.com/9987-653-655-0-0-0-0-0-0-0-1-1-1-1-1-72-4137-33.html");
            webRequest.Method = "get";

            httpWebResponse = (HttpWebResponse)webRequest.GetResponse();
            Stream stream = httpWebResponse.GetResponseStream();
            StreamReader sr = new StreamReader(stream, Encoding.Default);
            List<string> content = new List<string>();
            string brand;
            string line;
            int modelCount = 0;
            while (!sr.EndOfStream)
            {
                line = sr.ReadLine();
                //处理品牌
                if (line.StartsWith("<div class='content'>"))
                {
                    sr.ReadLine();
                    brand = sr.ReadLine();
                    //<a id='15127' href='9987-653-655-15127-0-0-0-0-0-0-1-1-1-1-1-72-4137-33.html' >三星（SAMSUNG）</a>
                    MatchCollection matchBrand = Regex.Matches(brand, @"href='[^<]+");
                    for (int i = 0; i < matchBrand.Count; i++)
                    {
                        string[] var = matchBrand[i].Value.Split('\'');
                        listBrands.Add(new SingleBrand(var[2].Substring(2), var[1], new List<string>(), new List<Model>()));

                        Datas.ProgressBarState = "获取品牌:" + listBrands.Count + "个";
                        sr.Close();
                    }
                    break;
                }
            }
            httpWebResponse.Close();

            //获取所有品牌的所有型号
            for (int i = 0; i < listBrands.Count; i++)//3;i++)//listBrands.Count; i++)
            {
                webRequest = WebRequest.Create("http://list.jd.com/" + listBrands[i].BrandAddress);
                //List<string> singleBrand = new List<string>();
                webRequest.Method = "get";
                bool hasNextPage = true;
                while (hasNextPage)
                {
                    httpWebResponse = (HttpWebResponse)webRequest.GetResponse();
                    stream = httpWebResponse.GetResponseStream();
                    sr = new StreamReader(stream, Encoding.Default);
                    string adds;
                    while (!sr.EndOfStream)
                    {
                        line = sr.ReadLine();
                        //处理产品地址
                        if (line.StartsWith("<ul class=\"list-h\">"))
                        {
                            while (!line.StartsWith("</ul>"))
                            {
                                line = sr.ReadLine();
                                if (line.IndexOf("没有") != -1)
                                {
                                    break;
                                }
                                adds = Regex.Match(line, @"href='[^<]+").Value;
                                if (adds.Length > 0)
                                {
                                    listBrands[i].ListBrandModelsAddress.Add(adds.Split('\'')[1]);
                                }
                                modelCount++;
                                Datas.ProgressBarState = "获取品牌:" + listBrands.Count + "个,型号:" + modelCount + "个";
                            }

                            //处理下一页地址
                            line = sr.ReadLine();
                            while (!line.StartsWith("<div class=\"pagin fr\">"))
                            {
                                line = sr.ReadLine();
                            }
                            line = sr.ReadLine();
                            //没有下一页,提前跳转
                            if (line.StartsWith("</div>"))
                            {
                                sr.Close();
                                hasNextPage = false;
                                break;
                            }
                            while (!line.StartsWith("</div>"))
                            {
                                //得到当前页地址,然后读取下一页的地址
                                if (line.IndexOf("current") != -1)
                                {
                                    line = sr.ReadLine();
                                    adds = Regex.Match(sr.ReadLine(), @"href='[^<]+").Value;
                                    //地址为空,不存在下一页
                                    if (adds.Length == 0)
                                    {
                                        hasNextPage = false;
                                        sr.Close();
                                        break;
                                    }
                                    else
                                    {
                                        //得到下一页地址
                                        webRequest = WebRequest.Create("http://list.jd.com/" + adds.Split('\'')[1]);
                                    }
                                    //关闭流,跳出
                                    sr.Close();
                                    break;
                                }
                                else
                                {
                                    line = sr.ReadLine();
                                }
                            }

                            break;
                        }
                    }
                }
            }
            List<string> detail = new List<string>();
            Datas.ProgressBarState = "数据获取完成,正在处理中...";
            modelCount = 1;
            //GetModels
            for (int i = 0; i < listBrands.Count; i++)
            {
                Directory.CreateDirectory("datas/" + listBrands[i].BrandName);
                for (int j = 0; j < listBrands[i].ListBrandModelsAddress.Count; j++)
                {
                    Datas.ProgressBarState = "数据获取完成,正在处理第" + modelCount++ + "个";
                    try
                    {
                        webRequest = WebRequest.Create(listBrands[i].ListBrandModelsAddress[j]);
                        webRequest.Method = "get";
                        httpWebResponse = (HttpWebResponse)webRequest.GetResponse();
                        stream = httpWebResponse.GetResponseStream();
                    }
                    catch (Exception ee)
                    {
                        if (ee.Message.Equals("操作超时"))
                        {
                            j--;
                            continue;
                        }
                        else
                        {
                            MessageBox.Show(ee.ToString());
                        }
                    }
                    FileStream fs = new FileStream("datas/" + listBrands[i].BrandName + "/" + Regex.Match(listBrands[i].ListBrandModelsAddress[j], "\\d+.html").Value, FileMode.Create);
                    StreamWriter sw = new StreamWriter(fs);
                    sw.Write(new StreamReader(stream, Encoding.Default).ReadToEnd());
                    sw.Close();
                    fs.Close();
                    httpWebResponse.Close();
                    detail.Clear();
                }
            }

            Datas.ProgressBarCompelet = true;
            readDataComplete = true;
        }
        AutoResetEvent autoResetEvent = new AutoResetEvent(false);
        private void btnAnaLocalHTML_Click(object sender, EventArgs e)
        {
            listBoxBrand.Items.Clear();
            Thread tGetBrands = new Thread(new ThreadStart(doAnaLocalHtml));
            tGetBrands.IsBackground = true;
            tGetBrands.Start();
            ProgressForm pf = new ProgressForm();
            Datas.ProgresBarStyle = ProgressBarStyle.Marquee;
            pf.ShowDialog();
            autoResetEvent.WaitOne();
            for (int i = 0; i < listBrands.Count; i++)
            {
                listBoxBrand.Items.Add(listBrands[i].BrandName);
            }
        }
        void doAnaLocalHtml()
        {
            listBrands.Clear();
            string[] dirs = Directory.GetDirectories("datas/");
            foreach (string item in dirs)
            {
                listBrands.Add(new SingleBrand(item.Split('/')[1], item, new List<string>(Directory.GetFiles(item)), new List<Model>()));
            }
            Datas.ProgressBarState = "数据获取完成,正在处理中...";
            List<string> detail = new List<string>();
            string line; int modelCount = 0;
            for (int i = 0; i < listBrands.Count; i++)
            {
                for (int j = 0; j < listBrands[i].ListBrandModelsAddress.Count; j++)
                {
                    Datas.ProgressBarState = "数据获取完成,正在处理第" + modelCount++ + "个";
                    using (FileStream fs = new FileStream(listBrands[i].ListBrandModelsAddress[j], FileMode.Open))
                    {
                        StreamReader sr = new StreamReader(fs);
                        string modelname = "";
                        while (!sr.EndOfStream)
                        {
                            line = sr.ReadLine();

                            if (line.StartsWith("				<ul class=\"detail-list\">"))
                            {
                                line = sr.ReadLine();
                                modelname = Regex.Match(line, @"：[\W\w]+\d+").Value;
                                if (modelname.Length > 0)
                                    modelname = modelname.Substring(1, modelname.Length - 1).Replace(" ", "");
                                continue;
                            }

                            //商品detail-2
                            if (!line.StartsWith("			<div class=\"mc  hide\""))
                            {
                                continue;
                            }
                            MatchCollection mc = Regex.Matches(line, @">[^<]+");
                            foreach (System.Text.RegularExpressions.Match item in mc)
                            {
                                detail.Add(item.Value.Substring(1, item.Value.Length - 1));
                            }

                            Model model = new Model();
                            int index = detail.IndexOf("型号");
                            if (index != -1)
                            {
                                model.ModelName = detail[index + 1];
                            }
                            else
                            {
                                if (modelname.Length > 0)
                                    model.ModelName = modelname;
                                modelname = "";
                            }
                            index = detail.IndexOf("操作系统");
                            if (index != -1)
                                model.OS = detail[index + 1];
                            index = detail.IndexOf("屏幕尺寸");
                            if (index != -1)
                            {
                                try
                                {
                                    string temp = Regex.Match(detail[index + 1], @"\d?\.?\d").Value;
                                    if (temp.Length > 0)
                                        model.ScreenSize = float.Parse(temp);
                                }
                                catch { }
                            }
                            index = detail.IndexOf("上市时间");
                            if (index != -1)
                            {
                                mc = Regex.Matches(detail[index + 1], @"\d+");
                                if (mc.Count == 2)
                                {
                                    model.TimeToMarket = new DateTime(Convert.ToInt32(mc[0].Value), Convert.ToInt32(mc[1].Value), 1);
                                }
                                else if (mc.Count == 1)
                                {
                                    model.TimeToMarket = new DateTime(Convert.ToInt32(mc[0].Value), 1, 1);
                                }
                            }

                            index = detail.IndexOf("分辨率");
                            if (index != -1)
                            {
                                mc = Regex.Matches(detail[index + 1], @"\d+");
                                if (mc.Count == 2)
                                {
                                    model.Resolution = new Size(Convert.ToInt32(mc[0].Value), Convert.ToInt32(mc[1].Value));
                                }
                            }
                            index = detail.IndexOf("CPU型号");
                            if (index != -1)
                            {
                                model.CPUModel = detail[index + 1];
                            }
                            index = detail.IndexOf("CPU核数");
                            if (index != -1)
                            {
                                short coreNum = 0;
                                switch (detail[index + 1][0])
                                {
                                    case '单':
                                        coreNum = 1;
                                        break;
                                    case '双':
                                        coreNum = 2;
                                        break;
                                    case '四':
                                        coreNum = 4;
                                        break;
                                    case '六':
                                        coreNum = 6;
                                        break;
                                    default:
                                        break;

                                }
                                model.CPUCoreNum = coreNum;
                            }
                            index = detail.IndexOf("CPU频率");
                            if (index != -1)
                            {
                                try
                                {
                                    model.CPUFreq = float.Parse(Regex.Match(detail[index + 1], @"\d?\.?\d").Value);
                                }
                                catch { }
                            }
                            detail.Clear();
                            //判断型号 是否已经存在
                            bool alreadyExit = false;
                            for (int k = 0; k < listBrands[i].ListModels.Count; k++)
                            {
                                if (listBrands[i].ListModels[k].ModelName == model.ModelName
                                    && listBrands[i].ListModels[k].CPUCoreNum == model.CPUCoreNum
                                    && listBrands[i].ListModels[k].CPUFreq == model.CPUFreq
                                    && listBrands[i].ListModels[k].CPUModel == model.CPUModel
                                    && listBrands[i].ListModels[k].OS == model.OS
                                    && listBrands[i].ListModels[k].Resolution == model.Resolution
                                    && listBrands[i].ListModels[k].ScreenSize == model.ScreenSize
                                    && listBrands[i].ListModels[k].TimeToMarket == model.TimeToMarket)
                                {
                                    alreadyExit = true;
                                    break;
                                }
                            }
                            if (alreadyExit)
                                break;

                            listBrands[i].ListModels.Add(model);
                            break;
                        }

                        sr.Close();
                        fs.Close();
                    }
                }
            }
            autoResetEvent.Set();
            Datas.ProgressBarCompelet = true;
            readDataComplete = true;
        }
    }
}