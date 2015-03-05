using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Reflection;
namespace 微博舆论
{
    class ju_word
    {
        public List<string> word = new List<string>();
        public List<int> position;
        public ju_word(string w, List<int> p)
        {
            word.Add(w);
            position = new List<int>(p);
        }
        public override string ToString()
        {
            string s = "";
            foreach (string item in word)
            {
                s += item + " ";
            }
            return s;
        }
    }

    /// <summary>
    /// 分词
    /// </summary>
    class words
    {

        public words(string x, int i)
        {
            this.word_name = x;
            this.position.Add(i);
        }
        public string word_name;
        public List<int> position = new List<int>();
        public int last_nu = 1;
        public int now_nu = 1;
        public double last_change = 0;
        public double now_changge = 0;
        /// <summary>
        /// 加权值
        /// </summary>
        public double weight_quan = 0;
        public double bursty = 0;
    }
    
    #region class ConvertFile 处理文件编码
    /// <summary>
    /// 将文件编码转换为utf-8,
    /// 转码过后的文件被放置到\test目录下
    /// </summary>
    class ConvertFile
    {
        private string _allContent;
        private string path;      //文件路径
        private string filter;    //文件类型
        private int numFiles;     //文件总数
        private FileInfo[] files; //文件信息
        private FileStream fs;
        private StreamReader sr;
        private StreamWriter sw;

        public string allContent
        {
            set
            { _allContent = value; }
            get
            {
                if (_allContent.Length > 0)
                    return _allContent;
                else
                    return "";
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="path">需要转换文件所在目录</param>
        public ConvertFile(string path)
        {
            this.path = path;
            this.filter = "*.txt";
            this.files = null;
            files = new DirectoryInfo(path).GetFiles(filter);
            this.numFiles = files.Length;

            TimeSpan timeSpan = new TimeSpan();
            DateTime startTime = DateTime.Now;
            //Console.WriteLine("文件所在目录: " + path);
            Console.WriteLine("Start converting...\nStart time: " + startTime);

            foreach (FileInfo info in files)
                ToUTF_8(info);

            DateTime endTime = DateTime.Now;
            timeSpan = endTime.Subtract(startTime);
            Console.WriteLine("End time: " + endTime);
            Console.WriteLine(numFiles + "files processed in " + timeSpan.TotalSeconds + "s");
        }
        private void ToUTF_8(FileInfo fileInfo)
        {
            fs = new FileStream(fileInfo.FullName, FileMode.Open);
            sr = new StreamReader(fs);
            string content = sr.ReadToEnd();

            byte[] buffer = Encoding.UTF8.GetBytes(content);
            string text = Encoding.GetEncoding("utf-8").GetString(buffer);
            //_allContent += text;          //转码过后不会使用的数据不再计算.

            sr.Close();
            fs.Close();

            fs = new FileStream(@"G:\11\test\" + fileInfo.Name, FileMode.Create);
            sw = new StreamWriter(fs);
            sw.Write(text);
            sw.Close();
            fs.Close();
        }
    }
    #endregion

    #region class CutToMutiFile 将大文件转换若干小文件
    class CutToMutiFile
    {
        /// <summary>
        /// 将大的微博文件转换为小的便于处理的文件
        /// </summary>
        /// <param name="sor">大文件文件路径</param>
        /// <param name="des">目标文件地址</param>
        static public void cut(string sor,string des)
        {
            int nowNum = 0;         //当前转换文档数
            int sizePerFile = 1 << 23;
            char[] buffer=new char[sizePerFile];
            if (!File.Exists(sor) || des == null)
            {
                Console.WriteLine("文件路径不合法");
                return;
            }
            //FileStream fsRead = new FileStream(from,FileMode.Open);
            StreamReader sr=new StreamReader (sor);
            //FileStream fsWrite = new FileStream(des + nowNum + ".txt", FileMode.Create);
            StreamWriter sw;
            while (!sr.EndOfStream)
            {
                sw=new StreamWriter (des + nowNum + ".txt");
                sr.Read(buffer,0,sizePerFile);
                sw.Write(new string(buffer).TrimEnd());
                nowNum++;
                sw.Close();
            }
        }
    }
    #endregion

    #region class ForbiddenMessage 违禁词结构体
    /// <summary>
    /// 违禁词结构体
    /// </summary>
    public class ForbiddenMessage
    {
        private int _level;
        private string _message;

        public ForbiddenMessage(string msg)
        {
            this._message = msg;
            this._level = -1;
        }

        public ForbiddenMessage(string msg, int lev)
        {
            this._message = msg;
            this._level = lev;
        }

        public string message
        {
            get { return this._message; }
            set { this._message = message; }
        }

        public void setLevel(int lev)
        {
            this._level = lev;
        }
        public int level
        {
            get { return this.level; }
            set { this.level = value; }
        }
    }
    #endregion

    #region class ReadForbiddenFile 读取违禁词
    /// <summary>
    /// 读取违禁词
    /// </summary>
    class ReadForbiddenFile
    {
        private string path;                //违禁词数据存放目录
        private List<ForbiddenMessage> _forbiddenWords;  //违禁词数据
        public List<ForbiddenMessage> forbiddenWords     //公共接口
        {
            get { return this._forbiddenWords; }
            set { this._forbiddenWords = value; }
        }

        //构造函数
        public ReadForbiddenFile(string path)
        {
            _forbiddenWords = new List<ForbiddenMessage>();
            this.path = path;
            readFile();
        }

        //读取违禁词文件
        private void readFile()
        {
            FileStream fs = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            while (!sr.EndOfStream)
            {                               //读取一行数据进行处理
                string line = sr.ReadLine().Trim();
                //处理引号(删除后用空格作为分隔符分割数据)
                string content = line.Replace("\"", "");
                //新建Message实例,然后添加到_forbidden变量
                ForbiddenMessage message = new ForbiddenMessage(content);
                _forbiddenWords.Add(message);
            }
            sr.Close();
            fs.Close();
        }
        /// <summary>
        /// 显示违禁词
        /// </summary>
        public void display()
        {
            for (int i = 0; i < this._forbiddenWords.Count; i++)
            {
                Console.WriteLine(((ForbiddenMessage)this._forbiddenWords[i]).message);
            }
            Console.WriteLine("共" + this._forbiddenWords.Count + "个违禁词");
        }
    }
    #endregion

    #region class StoppedMessage 停用词结构体
    /// <summary>
    /// 停用词结构体
    /// </summary>
    class StoppedMessage
    {
        private string _message;
        private int _level;

        public StoppedMessage(string msg, int lev)
        {
            this._message = msg;
            this._level = lev;
        }
        public StoppedMessage(string msg)
        {
            this._message = msg;
            this._level = -1;
        }
        public string message
        {
            get { return this._message; }
            set { this._message = value; }
        }
        public int level
        {
            get { return this._level; }
            set { this._level = value; }
        }
    }
    #endregion

    #region class ReadStoppedFile 读取停用词
    class ReadStoppedFile
    {
        private string path;              //停用词数据存放目录
        private List<StoppedMessage> _stoppedWords;  //停用词数据

        //构造函数
        public ReadStoppedFile(string path)
        {
            _stoppedWords = new List<StoppedMessage>();
            this.path = path;
            readFile();
        }

        //公共接口
        public List<StoppedMessage> stoppedWords
        {
            get { return this._stoppedWords; }
            set { this._stoppedWords = value; }
        }

        //读停用词文件
        private void readFile()
        {
            FileStream fs = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            while (!sr.EndOfStream)
            {
                //读取一行数据进行处理
                string line = sr.ReadLine().Trim();
                if (line.Length > 0)
                {
                    string[] array = line.Split(' ');
                    //新建Message实例,然后添加到_forbidden变量
                    StoppedMessage message = new StoppedMessage(array[0], Convert.ToInt32(array[1]));

                    _stoppedWords.Add(message);
                }
            }
            sr.Close();
            fs.Close();
        }

        /// <summary>
        /// 显示停用词
        /// </summary>
        public void display()
        {
            for (int i = 0; i < this._stoppedWords.Count; i++)
            {
                Console.WriteLine(((StoppedMessage)this._stoppedWords[i]).message);
            }
            Console.WriteLine("共" + this._stoppedWords.Count + "个停用词");
        }
    }
    #endregion

    #region class KeywordStruct 关键词结构体
    [Serializable()]
    class KeywordStruct
    {
        private string _content;    //关键词内容
        private int times;          //出现次数
    }
    #endregion

    #region class Blogs 微博结构体
    /// <summary>
    /// 微博结构体
    /// _content 保存微博内容
    /// _dateTime 保存微博发布时间
    /// _userName 保存发布微博用户名
    /// </summary>
    [Serializable()]
    class BlogStruct
    {
        private string _content;
        private DateTime _dateTime;
        private string _userName;
        private List<KeywordStruct> _keyWords;

        //不同参数的构造函数
        /// <summary>
        /// 只有内容的参数
        /// </summary>
        /// <param name="content">微博内容</param>
        public BlogStruct(string content)
        {
            this._content = content;
            this._dateTime = new DateTime(1900, 1, 1, 1, 0, 0, 0);
            this._userName = "";
        }
        /// <summary>
        /// 包含内容和日期
        /// </summary>
        /// <param name="content">微博内容</param>
        /// <param name="date">发布时间</param>
        public BlogStruct(string content,DateTime date)
        {
            this._content = content;
            this._dateTime = date;
            this._userName = "";
        }
        /// <summary>
        /// 包含内容和用户名
        /// </summary>
        /// <param name="content">微博内容</param>
        /// <param name="userName">用户名</param>
        public BlogStruct(string content,string userName)
        {
            this._content = content;
            this._dateTime = new DateTime(1900, 1, 1, 1, 0, 0, 0);
            this._userName = userName;
        }
        /// <summary>
        /// 包含内容,时间和用户名
        /// </summary>
        /// <param name="content">微博内容</param>
        /// <param name="date">发布时间</param>
        /// <param name="userName">用户名</param>
        public BlogStruct(string content,DateTime date,string userName)
        {
            this._content = content;
            this._dateTime = date;
            this._userName = userName;
        }
        public string content
        {
            set { return; }
            get { return this._content; }
        }
        public DateTime dateTime
        {
            set { return; }
            get { return this._dateTime; }
        }
        public List<KeywordStruct> keyWords
        {
            get { return this._keyWords; }
            set { return; }
        }

    }
    #endregion 

    #region class Blogs 分时间段读取微博
    [Serializable()]
    class DealBlogs
    {
        private string path;            //文件路径
        private int numFiles;           //文件总数
        private string filter;          //文件类型
        private FileInfo[] files;       //文件信息
        List<BlogStruct>[] al;
        public DealBlogs(string path)
        {
            this.path = path;
            this.filter = "*.txt";
            this.files = null;
            files = new DirectoryInfo(path).GetFiles(filter);
            this.numFiles = files.Length;
            al = new List<BlogStruct>[4];
            for (int i = 0; i < al.Length; i++)
            {
                al[i] = new List<BlogStruct>();
            }

            int nowNum = 0;                         //目前读取文件数
            int perALNum = this.numFiles > 8        //读取8个List<BlogStruct>;
                ? this.numFiles / 8 : 8; 
            StreamReader sr;

            foreach (FileInfo fi in files)
            {
                sr = new StreamReader(fi.FullName, Encoding.Default);
                string content;
                switch (nowNum / perALNum)
                {
                    case 0:
                        while (!sr.EndOfStream)
                        {
                            content = sr.ReadLine();
                            al[0].Add(new BlogStruct(content));
                        }
                        break;
                    case 1:
                        while (!sr.EndOfStream)
                        {
                            content = sr.ReadLine();
                            al[1].Add(new BlogStruct(content));
                        }
                        break;
                    case 2:
                        while (!sr.EndOfStream)
                        {
                            content = sr.ReadLine();
                            al[2].Add(new BlogStruct(content));
                        }
                        break;
                    default:
                        while (!sr.EndOfStream)
                        {
                            content = sr.ReadLine();
                            al[3].Add(new BlogStruct(content));
                        }
                        break;
                }
                nowNum++;
                sr.Close();
            }

            //Console.WriteLine(al[0].Count);
            //Console.WriteLine(al[1].Count);
            //Console.WriteLine(al[2].Count);
            //Console.WriteLine(al[3].Count);
        }
    }
    #endregion

    #region 全局静态数据
    class Datas
    {
        public static List<SinaJSON> sinaJSONList=new List<SinaJSON>();
        public static char FileName='1';
        public static int Value = 0;

        /// <summary>
        /// 频度统计窗口可见性
        /// </summary>
        public static bool DataGridViewFreqVisiable = false;
        /// <summary>
        /// 进度条结束,可以关闭显示进度窗口
        /// </summary>
        public static bool ProgressBarCompelet = false;
        public static string ProgressBarState = "正在读取文件...";
        public static System.Windows.Forms.ProgressBarStyle ProgresBarStyle =
            System.Windows.Forms.ProgressBarStyle.Continuous;
         public static long f_read = 0;
        public static int f_change = 0;
        public static int total_num = 0;
        /// <summary>
        /// 获得的过滤数
        /// </summary>
        public static List<SinaJSON> [] total_data=new List<SinaJSON>[7];
        /// <summary>
        /// 据读文件暂时保存的
        /// </summary>
        public static List<SinaJSON> temp_data = new List<SinaJSON>();
        /// <summary>
        /// 需要读取的数据文件路径
        /// </summary>
        public static string FileToReadPath = @"h:\11\SerializedData\temp4.dat";

        public static string PublicOpinionAnaPath = @"h:\11\SerializedData\temp4.dat";

        /// <summary>
        /// 存的七个状态关键字
        /// </summary>
        public static Dictionary<string, words> [] frequency = new Dictionary<string, words>[7];
        /// <summary>
        /// 存聚类的词和位置
        /// </summary>
        public static List<ju_word>[] ju_list = new List<ju_word>[7];
        /// <summary>
        /// 表态数与回复数对权重的影响因子
        /// </summary>
        public static double weight_word = 0.5;
        /// <summary>
        /// 当前存在哪个时段
        /// </summary>
        public static int now_time_read = 0;//当前存在哪个时段
        /// <summary>
        /// 读了多少个
        /// </summary>
        public static int now_time_count =0;//读了多少个 
        /// <summary>
        /// 每个时段读的最大
        /// </summary>
        public static int now_time_max = 10000;//每个时段读的最大
        public static DateTime now_time = new DateTime(2013, 10, 30, 08, 00, 00);
        public static string ads_word = "颁奖,热线,拨打,接听,总部,中奖,现场,系统,幸运";
        //读数据的线程数据
        
        public static  int now_readnum = 0;
        public static Mutex operate_data_start = new Mutex(true);
        public static Mutex operate_data_stop = new Mutex(true);
        public static AutoResetEvent auto_read = new AutoResetEvent(false);
        //public static Semaphore s_read = new Semaphore(0, 10000);
        public static void init()//初始化
        {
            for (int i = 0; i < 7; i++)
            {
                frequency[i] = new Dictionary<string, words>();
                total_data[i] = new List<SinaJSON>();
                ju_list[i] = new List<ju_word>();
            }
        }
    }
    #endregion

    #region 数据获取器
    /// <summary>
    /// 摘要:
    ///      负责从服务器读取数据到本地
    ///      或者从本地读取数据到内存
    /// </summary>
    class BlogReader
    {
        private string _path;                           //文件保存位置
        private string _content;                        //单次数据内容
        private SinaJSON sinaJSON;                      //...
        private int requestCounter;                     //总请求次数
        public static System.Timers.Timer timer;        //定时从服务器获取数据
        private Stream streamFromServer;
        private WebRequest request;
        //private Datas data2File;
        private StreamReader streamReader;              //streamReader of streamFromServer
        private FileStream inputFileStream;             //gets data from local file
        private BinaryFormatter binFormatter;           //formates data
        private HttpWebResponse httpWebResponse;        //...
        private DateTime startTime;
        public BlogReader()
        {
            binFormatter = new BinaryFormatter();
            sinaJSON = new SinaJSON();
            _path = "temp4.dat";
            _content = "";
            requestCounter = 0;
        }
        public BlogReader(string path)
        {
            binFormatter = new BinaryFormatter();
            sinaJSON = new SinaJSON();
            _path = path;
            _content = "";
            requestCounter = 0;
        }

        public void StartRead()
        {
            startTime = DateTime.Now;
            if (timer == null)
            {
                Log.WriteToAll("=====================================start======================================\n");
                string msg = "function StartRead.开始时间:" + DateTime.Now.ToLocalTime() + "\n单次请求返回数据约为200条\n";
                Log.WriteToAll(msg);
                timer = new System.Timers.Timer(3600);   //设置1000请求/H
                timer.Elapsed += timer_Elapsed;
                timer.Enabled = true;
                timer.Start();
            }
            else
                Log.WriteToAll("function StartRead.已经在获取数据..\n");
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            ReadFromServer();
        }

        public static void ChangePeriod(int period)
        {
            if (timer != null)
            {
                timer.Stop();
                timer.Interval=period;
                timer.Start();
                string msg = "function ChangePeriod.更改时间间隔" + period
                             + "当前时间:" + DateTime.Now.ToLocalTime() + "\n";
                Log.WriteToAll(msg);
            }
        }

        public static void EndRead()
        {
            if (timer != null)
            {
                timer.Dispose();
                Log.WriteToAll("function EndRead().从服务器获取数据结束,当前时间:" + DateTime.Now.ToLocalTime()
                                + "\n");
            }
        }

        /// <summary>
        /// 从服务器读取数据
        /// 读取的每条数据经过去重,存储到本地硬盘
        /// </summary>
        private void ReadFromServer()
        {
            int min = DateTime.Now.Minute;
            int startMin = ((int)Datas.FileName - 48) * 10;
            int sleepTime = (startMin - 10 <= min && min <= startMin) ? 0 : (min < startMin - 10 ? (startMin - 10 - min) * 60000 : (50 - min + startMin) * 60000);
            if (sleepTime > 0)
            {
                Log.WriteToAll("将在" + sleepTime / 1000 + "s后继续获取数据\n");
                timer.Stop();
                Thread.Sleep(sleepTime);
                timer.Start();
            }
            try
            {
                request = WebRequest.Create(
                    "https://api.weibo.com/2/statuses/public_timeline.json?access_token=2.00NMtauC0ng6Df315e4a2aa0WkbyCC&count=200"
                    );//&count=200                                          // Create a request for the URL. 
                Log.WriteToAll("请求" + ++requestCounter + ",");
                //request.Credentials = CredentialCache.DefaultCredentials; // If required by the server, set the credentials.
                httpWebResponse = (HttpWebResponse)request.GetResponse();   // Get the response.
                //Console.WriteLine(httpWebResponse.StatusDescription);     // Display the status.
                streamFromServer = httpWebResponse.GetResponseStream();     // Get the stream containing content returned by the server.
                streamReader = new StreamReader(streamFromServer);          // Open the stream using a StreamReader for easy access.

                this._content = streamReader.ReadToEnd();                   // Read the content. 
                sinaJSON.ConvertStr(this._content);                         //convert

                streamReader.Close();
                streamFromServer.Close();
                httpWebResponse.Close();
                string msg = "当前个数:" + Datas.sinaJSONList.Count + "   当前时间为:" + DateTime.Now.ToLocalTime() + "\n";
                Log.WriteToAll(msg);
            }
            catch (Exception e)
            {
                //捕获到403...
                timer.Stop();
                Log.WriteToFile(e.ToString());
                Thread.Sleep(60000);
                timer.Start();
            }
        }

        /// <summary>
        /// 从文件读取数据
        /// </summary>
        /// <returns>List<SinaJSON></returns>
        public List<SinaJSON> ReadFromFile()
        {
            //读取数据
            inputFileStream = new FileStream(_path, FileMode.Open);
            List<SinaJSON> list = new List<SinaJSON>();
            SinaJSON item = new SinaJSON();
            binFormatter.Binder = new UBinder();

            while (inputFileStream.Position < inputFileStream.Length)
            {
                Datas.Value = (int)(Convert.ToDouble(inputFileStream.Position) / inputFileStream.Length * 100);
                item = (SinaJSON)binFormatter.Deserialize(inputFileStream);
                list.Add(item);
            }
            Datas.Value = (int)(inputFileStream.Position / inputFileStream.Length) * 100;
            inputFileStream.Close();
            return list;
        }
        public void ReadFromFile_thread()
        {
            //读取数据
            // inputFileStream = new FileStream(_path, FileMode.Open);
            byte[] bs = File.ReadAllBytes(Datas.PublicOpinionAnaPath);
            MemoryStream ms = new MemoryStream(bs);
            //StreamReader sr = new StreamReader();

            ms.Read(bs, 0, bs.Length);
            LinkedList<SinaJSON> list = new LinkedList<SinaJSON>();
            SinaJSON item = new SinaJSON();
            binFormatter.Binder = new 微博舆论.BlogReader.UBinder();
            string[] sp = Datas.ads_word.Split(',');
            ms.Position = 0;
            // while (inputFileStream.Position < inputFileStream.Length)
            while (ms.Position < ms.Length)
            {
                // item = (SinaJSON)binFormatter.Deserialize(inputFileStream);
                item = (SinaJSON)binFormatter.Deserialize(ms);
                Datas.temp_data.Add(item);
                Datas.auto_read.Set();
            }
            Datas.auto_read.Set();
            //inputFileStream.Close();
            ms.Close();

        } 
        public class UBinder : SerializationBinder
        {
            public override Type BindToType(string assemblyName, string typeName)
            {
                Assembly ass = Assembly.GetExecutingAssembly();
                return ass.GetType(typeName);
            }
        }
    }
    #endregion

    #region 从服务器获取数据的日志记录
    class Log
    {
        private static FileStream toFile = new FileStream("log"+Datas.FileName+".txt", FileMode.Append);
        private static StreamWriter sw = new StreamWriter(toFile);
        public static void WriteToScreen(string str)
        {
            Console.Write(str);
        }
        public static void WriteToFile(string str)
        {
            sw.WriteLine(str);
        }
        public static void WriteToAll(string str)
        {
            Console.Write(str);
            sw.WriteLine(str);
        }
        public static void EndRead()
        {
            BlogReader.timer.Dispose();
            sw.Close();
            toFile.Close();
        }
    }
    #endregion
}