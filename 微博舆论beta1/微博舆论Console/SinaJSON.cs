using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
namespace 微博舆论
{
    #region class SinaJSON
    /// <summary>
    /// sina微博返回数据格式为JSON,
    /// 解析返回数据
    /// </summary>
    [Serializable()]
    class SinaJSON
    {
        public SinaJSON()
        {
            this.SinaUser = new SinaUser();
            this.Visiable = new Visiable();
            this.Pic_urls = new List<string>();
        }

        /// <summary>
        /// 用于微博时间转dtaetime
        /// </summary>
        /// <param name="gmt">需要转换的字符串</param>
        /// <returns></returns>
        public static DateTime GMT2Local(string gmt)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-us");
            DateTime dt = new DateTime();
            gmt=gmt.Insert(23, ":");
            dt = DateTime.ParseExact(gmt, "ddd MMM dd HH:mm:ss zzz yyyy", null);
            return dt;
        }

        /// <summary>
        /// 将json数据转换存入List<SinaJson>中
        /// </summary>
        /// <param name="Sj">ref List<SinaJSON></param>
        /// <param name="json_str">要转换的字符串</param>
        public void  ConvertStr( string json_str)
        {
            int count = Datas.sinaJSONList.Count - 1;   //当前长度
            //int countSame=0;                            //重复个数
            List<SinaJSON> tempSj = new List<SinaJSON>();

            JObject javascript = (JObject)JsonConvert.DeserializeObject(json_str);
            JArray ja = (JArray)javascript["statuses"];
            SinaJSON _Sj;
            for (int i = 0; i < ja.Count; i++)
            {
                _Sj = new SinaJSON();
                JObject obj = (JObject)ja[i];
                _Sj.Created_at = SinaJSON.GMT2Local(obj["created_at"].ToString());
                _Sj.Id = Convert.ToInt64(obj["id"].ToString());
                _Sj.Mid = obj["mid"].ToString();
                _Sj.Idstr = obj["idstr"].ToString();
                _Sj.Text = obj["text"].ToString();
                _Sj.Source = obj["source"].ToString();
                _Sj.Favorite = Convert.ToBoolean(obj["favorited"]);
                _Sj.Trunctated = Convert.ToBoolean(obj["truncated"]);
                _Sj.In_reply_to_status_id = obj["in_reply_to_status_id"].ToString();
                _Sj.In_reply_to_user_id = obj["in_reply_to_status_id"].ToString();
                JArray pic = (JArray)obj["pic_urls"];
                for (int t = 0; t < pic.Count; t++)
                {
                    JObject pi = (JObject)pic[t];
                    _Sj.Pic_urls.Add(pi["thumbnail_pic"].ToString());
                }
                JObject user = (JObject)obj["user"];
                _Sj.SinaUser.Id = Convert.ToInt64(user["id"].ToString());
                _Sj.SinaUser.Idstr = user["idstr"].ToString();
                _Sj.SinaUser.Class = Convert.ToInt16(user["class"]);
                _Sj.SinaUser.Screen_name = user["screen_name"].ToString();
                _Sj.SinaUser.Name = user["name"].ToString();
                _Sj.SinaUser.Province = user["province"].ToString();
                _Sj.SinaUser.City = user["city"].ToString();
                _Sj.SinaUser.Location = user["location"].ToString();
                _Sj.SinaUser.Description = user["description"].ToString();
                _Sj.SinaUser.Url = user["url"].ToString();
                _Sj.SinaUser.Profile_image = user["profile_image_url"].ToString();
                _Sj.SinaUser.Profile_url = user["profile_url"].ToString();
                _Sj.SinaUser.Domain = user["domain"].ToString();
                _Sj.SinaUser.Weihao = user["weihao"].ToString();
                _Sj.SinaUser.Gender = user["gender"].ToString();

                _Sj.SinaUser.Follow_count = Convert.ToInt32(user["followers_count"]);
                _Sj.SinaUser.Friends_count = Convert.ToInt32(user["friends_count"]);
                _Sj.SinaUser.Statuses_count = Convert.ToInt32(user["statuses_count"]);
                _Sj.SinaUser.Favourites_count = Convert.ToInt32(user["favourites_count"]);

                _Sj.SinaUser.Created_at = SinaJSON.GMT2Local(user["created_at"].ToString());
                _Sj.SinaUser.Following = Convert.ToBoolean(user["following"].ToString());
                _Sj.SinaUser.Allow_all_act_msg = Convert.ToBoolean(user["allow_all_act_msg"].ToString());
                _Sj.SinaUser.Geo_enable = Convert.ToBoolean(user["geo_enabled"].ToString());
                _Sj.SinaUser.Verified = Convert.ToBoolean(user["verified"].ToString());

                _Sj.SinaUser.Verified_type = Convert.ToInt16(user["verified_type"]);

                _Sj.SinaUser.Remark = user["remark"].ToString();
                _Sj.SinaUser.Ptype = Convert.ToInt16(user["ptype"]);
                _Sj.SinaUser.Allow_all_comment = Convert.ToBoolean(user["allow_all_comment"].ToString());
                _Sj.SinaUser.Avatar_large = user["avatar_large"].ToString();
                _Sj.SinaUser.Avatar_hd = user["avatar_hd"].ToString();
                _Sj.SinaUser.Verfied_reason = user["verified_reason"].ToString();
                _Sj.SinaUser.Follow_me = Convert.ToBoolean(user["follow_me"].ToString());
                _Sj.SinaUser.Online_status = Convert.ToInt16(user["online_status"]);
                _Sj.SinaUser.Bi_followers_count = Convert.ToInt32(user["bi_followers_count"]);
                _Sj.SinaUser.Lang = user["lang"].ToString();
                _Sj.SinaUser.Star = Convert.ToInt16(user["star"]);
                _Sj.SinaUser.Mbtype = Convert.ToInt16(user["mbtype"]);
                _Sj.SinaUser.Mbrank = Convert.ToInt16(user["mbrank"]);
                _Sj.SinaUser.Block_word = Convert.ToInt16(user["block_word"]);
                _Sj.Reposts_count = Convert.ToInt16(obj["reposts_count"]);
                _Sj.Comments_count = Convert.ToInt16(obj["comments_count"]);
                _Sj.Attitudes_count = Convert.ToInt16(obj["attitudes_count"]);
                _Sj.Mlevel = Convert.ToInt16(obj["mlevel"]);
                JObject vis = (JObject)obj["visible"];
                _Sj.Visiable.List_id = Convert.ToInt16(vis["list_id"]);
                _Sj.Visiable.Type = Convert.ToInt16(vis["type"]);

                tempSj.Add(_Sj);
            }

            //去重
            //所有元素与最后一个元素相比,相同跳出循环
            //经过分析,未发现重复元素,所以关闭去重函数
            //if (count > 0)
            //{
            //    foreach (SinaJSON item in tempSj)
            //    {
            //        countSame++;
            //        if (item.Equals(Datas.sinaJSONList[count]))
            //            break;
            //    }
            //    countSame = tempSj.Count - countSame;
            //    Log.WriteToAll("重复个数:" + countSame);
            //}
            for (int i = 0; i < tempSj.Count; i++)
            {
                Datas.sinaJSONList.Add(tempSj[i]);
                using (FileStream outputFileStream = new FileStream("temp"+Datas.FileName+".dat", FileMode.Append))
                {
                    //序列化到文件
                    BinaryFormatter binFormatter = new BinaryFormatter();
                    binFormatter.Serialize(outputFileStream, tempSj[i]);
                    outputFileStream.Close();
                }
            }

        }

        #region 封装字段
        private long _id;
        private string _mid;
        private string _idstr;
        private string _source;
        private int _mlevel;
        private int _reposts_count;
        private int _comments_count;
        private int _attitudes_count;
        private bool _favorite;
        private bool _trunctated;
        private string _text;
        //private string[] _geo = new string[2];
        private List<string> _pic_urls;
        private string _in_reply_to_user_id;
        private string _in_reply_to_status_id;
        private SinaUser _sinaUser;
        private Visiable _visiable;
        private DateTime _created_at;
        public long Id
        {
            get { return _id; }
            set { _id = value; }
        }
        public string Mid
        {
            get { return _mid; }
            set { _mid = value; }
        }
        public string Source
        {
            get { return _source; }
            set { _source = value; }
        }
        public string Idstr
        {
            get { return _idstr; }
            set { _idstr = value; }
        }
        public int Mlevel
        {
            get { return _mlevel; }
            set { _mlevel = value; }
        }
        public int Reposts_count
        {
            get { return _reposts_count; }
            set { _reposts_count = value; }
        }
        public int Attitudes_count
        {
            get { return _attitudes_count; }
            set { _attitudes_count = value; }
        }
        public int Comments_count
        {
            get { return _comments_count; }
            set { _comments_count = value; }
        }
        public bool Favorite
        {
            get { return _favorite; }
            set { _favorite = value; }
        }
        public bool Trunctated
        {
            get { return _trunctated; }
            set { _trunctated = value; }
        }
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }
        //public string[] Geo
        //{
        //    get { return _geo; }
        //    set { _geo = value; }
        //}
        public List<string> Pic_urls
        {
            get { return _pic_urls; }
            set { _pic_urls = value; }
        }
        public string In_reply_to_user_id
        {
            get { return _in_reply_to_user_id; }
            set { _in_reply_to_user_id = value; }
        }
        public string In_reply_to_status_id
        {
            get { return _in_reply_to_status_id; }
            set { _in_reply_to_status_id = value; }
        }
        internal SinaUser SinaUser
        {
            get { return _sinaUser; }
            set { _sinaUser = value; }
        }
        internal Visiable Visiable
        {
            get { return _visiable; }
            set { _visiable = value; }
        }
        public DateTime Created_at
        {
            get { return _created_at; }
            set { _created_at = value; }
        }
        #endregion
    }
    #endregion
    #region class SinaUser
    /// <summary>
    /// 返回数据中用户类
    /// </summary>
    [Serializable()]
    class SinaUser
    {
        #region 封装字段
        private long _id;
        private int _star;
        private int _class;
        private int _ptype;
        private int _mbtype;
        private int _mbrank;
        private int _block_word;
        private int _follow_count;
        private int _friends_count;
        private int _verified_type;
        private int _online_status;
        private int _statuses_count;
        private int _favourites_count;
        private int _bi_followers_count;
        private string _url;
        private string _name;
        private string _city;
        private string _lang;
        private string _idstr;
        private string _remark;
        private string _domain;
        private string _gender;
        private string _weihao;
        private string _location;
        private string _province;
        private string _avatar_hd;
        private string _screen_name;
        private string _description;
        private string _cover_image;
        private string _profile_url;
        private string _avatar_large;
        private string _profile_image;
        private string _verfied_reason;
        private DateTime _created_at;
        private bool _verified;
        private bool _following;
        private bool _follow_me;
        private bool _geo_enable;
        private bool _allow_all_act_msg;
        private bool _allow_all_comment;

        public long Id
        {
            get { return _id; }
            set { _id = value; }
        }
        public int Star
        {
            get { return _star; }
            set { _star = value; }
        }
        public string Idstr
        {
            get { return _idstr; }
            set { _idstr = value; }
        }
        public int Class
        {
            get { return _class; }
            set { _class = value; }
        }
        public int Ptype
        {
            get { return _ptype; }
            set { _ptype = value; }
        }
        public int Mbtype
        {
            get { return _mbtype; }
            set { _mbtype = value; }
        }
        public int Mbrank
        {
            get { return _mbrank; }
            set { _mbrank = value; }
        }
        public int Block_word
        {
            get { return _block_word; }
            set { _block_word = value; }
        }
        public int Follow_count
        {
            get { return _follow_count; }
            set { _follow_count = value; }
        }
        public int Friends_count
        {
            get { return _friends_count; }
            set { _friends_count = value; }
        }
        public int Verified_type
        {
            get { return _verified_type; }
            set { _verified_type = value; }
        }
        public int Online_status
        {
            get { return _online_status; }
            set { _online_status = value; }
        }
        public int Statuses_count
        {
            get { return _statuses_count; }
            set { _statuses_count = value; }
        }
        public int Favourites_count
        {
            get { return _favourites_count; }
            set { _favourites_count = value; }
        }
        public int Bi_followers_count
        {
            get { return _bi_followers_count; }
            set { _bi_followers_count = value; }
        }
        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public string City
        {
            get { return _city; }
            set { _city = value; }
        }
        public string Lang
        {
            get { return _lang; }
            set { _lang = value; }
        }
        public string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
        public string Domain
        {
            get { return _domain; }
            set { _domain = value; }
        }
        public string Weihao
        {
            get { return _weihao; }
            set { _weihao = value; }
        }
        public string Location
        {
            get { return _location; }
            set { _location = value; }
        }
        public string Province
        {
            get { return _province; }
            set { _province = value; }
        }
        public string Avatar_hd
        {
            get { return _avatar_hd; }
            set { _avatar_hd = value; }
        }
        public string Screen_name
        {
            get { return _screen_name; }
            set { _screen_name = value; }
        }
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
        public string Cover_image
        {
            get { return _cover_image; }
            set { _cover_image = value; }
        }
        public string Profile_url
        {
            get { return _profile_url; }
            set { _profile_url = value; }
        }
        public string Avatar_large
        {
            get { return _avatar_large; }
            set { _avatar_large = value; }
        }
        public string Profile_image
        {
            get { return _profile_image; }
            set { _profile_image = value; }
        }
        public string Verfied_reason
        {
            get { return _verfied_reason; }
            set { _verfied_reason = value; }
        }
        public string Gender
        {
            get { return _gender; }
            set { _gender = value; }
        }
        public DateTime Created_at
        {
            get { return _created_at; }
            set { _created_at = value; }
        }
        public bool Verified
        {
            get { return _verified; }
            set { _verified = value; }
        }
        public bool Following
        {
            get { return _following; }
            set { _following = value; }
        }
        public bool Geo_enable
        {
            get { return _geo_enable; }
            set { _geo_enable = value; }
        }
        public bool Allow_all_act_msg
        {
            get { return _allow_all_act_msg; }
            set { _allow_all_act_msg = value; }
        }
        public bool Allow_all_comment
        {
            get { return _allow_all_comment; }
            set { _allow_all_comment = value; }
        }
        public bool Follow_me
        {
            get { return _follow_me; }
            set { _follow_me = value; }
        }
        #endregion
    }
    #endregion
    #region class Visiable
    [Serializable()]
    class Visiable
    {
        private int _type;
        private int _list_id;
        public Visiable()
        {
            this._type = 0;
            this._list_id = 0;
        }
        public Visiable(int type, int list_id)
        {
            this._list_id = list_id;
            this._type = type;
        }
        public int Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public int List_id
        {
            get { return _list_id; }
            set { _list_id = value; }
        }
    }
    #endregion
}
