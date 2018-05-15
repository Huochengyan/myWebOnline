using CustomerService.Como;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;

namespace CustomerService
{
   
    /// <summary>
    /// C# 微信小程序接入  author:huochengyan
    /// </summary>
    public class Msg : IHttpHandler
    {
        //服务器日志类 
        Log log = new Log(AppDomain.CurrentDomain.BaseDirectory + @"/log/Log.txt");
        static string token = "myxiaochengxu123";//自定义字段(自己填写3-32个字符)
       static string corpId = "wx02140b1fd6f19044";
       static string AppSecret = "d6e4258791c9b6891541e9b0d178d97f";
       static string encodingAESKey = "rkmxahHljFYhWqq4TdNG1rzlcayFmcO3TWm6G3K98Al";


      

        public void ProcessRequest(HttpContext context)
        {
            string postString = string.Empty;
            if (HttpContext.Current.Request.HttpMethod.ToUpper() == "POST")
            {
                using (Stream stream = HttpContext.Current.Request.InputStream)
                {
                    Byte[] postBytes = new Byte[stream.Length];
                    stream.Read(postBytes, 0, (Int32)stream.Length);
                    postString = Encoding.UTF8.GetString(postBytes);
                }

                if (!string.IsNullOrEmpty(postString))
                {
                    log.log("=========================a.WeiXin 服务器验证========================= ");
                    log.log(postString);
                }
                else
                {
                    log.log("=========================b.WeiXin 服务器验证========================= ");
                    log.log(postString);
                }
            }
            else
            {
                log.log("=========================c.WeiXin 服务器验证========================= ");
                Auth(context);
            }

           
        }

        private void Auth(HttpContext context)
        {
            var signature = context.Request["signature"];
            var timestamp = context.Request["timestamp"];
            var nonce = context.Request["nonce"];
            var echostr = context.Request["echostr"];


            log.log("微信消息服务器验证传入数据" + string.Format("signature:{0},timestamp:{1},nonce:{2},echostr:{3}", signature, timestamp, nonce, echostr));

          

            //timestamp和token和nonce 字典排序
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("token", token);
            dic.Add("nonce", nonce);
            dic.Add("timestamp", timestamp);
            var list = dic.OrderBy(s => s.Value);
            var conbineStr = "";
            foreach (var s in list)
            {
                conbineStr = conbineStr + s.Value;
            }
            string data = conbineStr;
            //sha1加密
            string secret = FormsAuthentication.HashPasswordForStoringInConfigFile(conbineStr, "SHA1").ToLower();
            var success = signature == secret;
            if (success)
            {
                data = echostr;
            }
            context.Response.ContentType = "text/plain";
            context.Response.Write(data);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 回复用户消息
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="StrMessg"></param>
        private void SendMessage(string UserID, string StrMessg)
        {      
            string Access_Token = GetAccess_token();
            if (Access_Token == "")
                log.log("SendMessage 未能成功加载Access_Token");
            string paramData = @"{
           ""touser"":";
            paramData += '"' + UserID + '"';
            paramData += "," + '"' + @"msgtype"": ""text"", 
                   ""agentid"": ""5"", 
                    ""text"": {
                   ""content"":";
            paramData += '"' + StrMessg + '"';
            paramData += @"}, 
            ""safe"": ""0""
        }";
            ;
            string url = String.Format("https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token={0}", Access_Token);
            string postresult = Como.WebRequestAction.PostWebRequest(url, paramData, Encoding.UTF8);
        }


        /// <summary>
        /// 获取Access_token的值
        /// </summary>
        /// <returns></returns>
        private  string GetAccess_token()
        {    
            string url = String.Format("https://qyapi.weixin.qq.com/cgi-bin/gettoken?corpid={0}&corpsecret={1}", corpId, AppSecret);
            string access_tokenjson = WebRequestAction.GetUrl(url);
            access_tokenjson = "[" + access_tokenjson + "]";
            if (access_tokenjson != "")
            {
                JArray ja = (JArray)JsonConvert.DeserializeObject(access_tokenjson);
                string Error = "";
                try
                {
                    log.log("access_tokenjson:" + access_tokenjson);
                    if (access_tokenjson.Contains("errcode"))
                    {
                        Error = ja[0]["errcode"].ToString();
                        log.log("GetAccess_token :access_token Error" + Error);
                    }
                }
                catch (Exception ex)
                {
                    log.log("获取access_token,未获取到错误信息" + ex.Message.ToString());
                }
                string access_token = ja[0]["access_token"].ToString();
                string expires_in = ja[0]["expires_in"].ToString();
                if (expires_in == "7200")
                {
                    log.log("access_tokenjson 获取成功!");
                }
                return access_token;
            }     
            return "";
        }
    }
}