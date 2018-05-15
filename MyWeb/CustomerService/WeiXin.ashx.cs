using CustomerService.Como;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Xml;
using Tencent;
using static Tencent.WXBizMsgCrypt;

namespace CustomerService
{
    /// <summary>
    /// WeiXin 的摘要说明
    /// </summary>
    public class WeiXin : IHttpHandler
    {
        Log log = new Log(AppDomain.CurrentDomain.BaseDirectory + @"/log/Log.txt");

        string corpId = "wx02140b1fd6f19044";
        string AppSecret = "d6e4258791c9b6891541e9b0d178d97f";
        string encodingAESKey = "rkmxahHljFYhWqq4TdNG1rzlcayFmcO3TWm6G3K98Al";



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
                    // Execute(postString);   postString为：" + postString
                    log.log("=========================1.WeiXin 服务器验证========================= ");
                    //MMessage(context, postString);
                    log.log("=========================a.WeiXin 服务器验证========================= ");
                    log.log(postString);
                }
                else {
                    log.log("=========================2.WeiXin 服务器验证========================= ");
                    log.log("=========================b.WeiXin 服务器验证========================= ");
                    log.log(postString);
                }
            }
            else
            {
                log.log("=========================3.WeiXin 服务器验证========================= ");
                log.log("=========================c.WeiXin 服务器验证========================= ");
                Auth();
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        #region  1. 验证并相应服务器的数据
        /// <summary>
        /// 成为开发者的第一步，验证并相应服务器的数据
        /// </summary>
        private void Auth()
        {
            string token = "myxiaochengxu123";
            string signature = HttpContext.Current.Request.QueryString["signature"]; ;
            string timestamp = HttpContext.Current.Request.QueryString["timestamp"]; ;
            string nonce = HttpContext.Current.Request.QueryString["nonce"];
            string echostr = HttpContext.Current.Request.QueryString["echoStr"];

            log.log("token:"+ token);
            log.log("signature:" + signature);
            log.log("timestamp:" + timestamp);
            log.log("nonce:" + nonce);
            log.log("echostr:" + echostr);

            List<string> list = new List<string>();
            list.Add(token);
            list.Add(timestamp);
            list.Add(nonce);
            list.Sort();

            list.Sort();

            string res = string.Join("", list.ToArray());


            Byte[] data1ToHash = Encoding.ASCII.GetBytes(res);
            byte[] hashvalue1 = ((HashAlgorithm)CryptoConfig.CreateFromName("SHA1")).ComputeHash(data1ToHash);

            StringBuilder sb = new StringBuilder();
            foreach (byte b in hashvalue1)
            {
                sb.Append(b.ToString("x2")); //转16进制小写
            }
            log.log("sb:"+sb.ToString());
            //string s = BitConverter.ToString(hashvalue1).Replace("-", string.Empty).ToLower();
            if (signature == sb.ToString())
            {
                Console.Write("OK");
                log.log("OK");
            }
            else
            {
                log.log("NO");
            }
            //企业号方式
            string decryptEchoString = "";
            if (new CorpBasicApi().CheckSignature(token, signature, timestamp, nonce, corpId, encodingAESKey, echostr, ref decryptEchoString))
            {
                if (!string.IsNullOrEmpty(decryptEchoString))
                {
                    log.log("decryptEchoString:" + decryptEchoString);
                    HttpContext.Current.Response.Write(decryptEchoString);
                    HttpContext.Current.Response.End();
                }
            }

            //new ?
            //string decryptEchoString = "";
            //if (new CorpBasicApi().CheckSignature(token, signature, timestamp, nonce, corpId, encodingAESKey, echostr, ref decryptEchoString))
            //{
            //    if (!string.IsNullOrEmpty(decryptEchoString))
            //    {
            //        log.log("decryptEchoString:"+decryptEchoString);
            //        HttpContext.Current.Response.Write(decryptEchoString);
            //        HttpContext.Current.Response.End();
            //    }
            //}



        }
        #region  企业号基础操作API实现
        /// <summary>
        /// 企业号基础操作API实现
        /// </summary>
        public class CorpBasicApi //: ICorpBasicApi
        {
            /// <summary>
            /// 验证企业号签名
            /// </summary>
            /// <param name="token">企业号配置的Token</param>
            /// <param name="signature">签名内容</param>
            /// <param name="timestamp">时间戳</param>
            /// <param name="nonce">nonce参数</param>
            /// <param name="corpId">企业号ID标识</param>
            /// <param name="encodingAESKey">加密键</param>
            /// <param name="echostr">内容字符串</param>
            /// <param name="retEchostr">返回的字符串</param>
            /// <returns></returns>
            public bool CheckSignature(string token, string signature, string timestamp, string nonce, string corpId, string encodingAESKey, string echostr, ref string retEchostr)
            {
                WXBizMsgCrypt wxcpt = new WXBizMsgCrypt(token, encodingAESKey, corpId);
                int result = wxcpt.DecryptMsg(signature, timestamp, nonce, echostr, ref retEchostr);
                if (result != 0)
                {
                    new Log(AppDomain.CurrentDomain.BaseDirectory + @"/log/Log.txt").log("CheckSignature错误  ERR: VerifyURL fail, ret: " + result);
                    return false;
                }
                return true;

                //ret==0表示验证成功，retEchostr参数表示明文，用户需要将retEchostr作为get请求的返回参数，返回给企业号。
                // HttpUtils.SetResponse(retEchostr);
            }
        }
        #endregion

        #region 2. 解密关注者消息 图片 文字 等。。。。
        public void JMMessage(HttpContext context, string PostXML)
        {
            //Como.LogUtil.WriteLog("PostXML:为：" + PostXML);
            //公众平台上开发者设置的token, corpID, EncodingAESKey
            //string sToken = ConfigurationManager.AppSettings["CorpToken"];
            //string sCorpID = ConfigurationManager.AppSettings["CorpId"];
            //string sEncodingAESKey = ConfigurationManager.AppSettings["EncodingAESKey"];

            //Tencent.WXBizMsgCrypt wxcpt = new Tencent.WXBizMsgCrypt(sToken, sEncodingAESKey, sCorpID);
            //string sReqMsgSig = HttpContext.Current.Request.QueryString["msg_signature"];
            //string sReqTimeStamp = HttpContext.Current.Request.QueryString["timestamp"];
            //string sReqNonce = HttpContext.Current.Request.QueryString["nonce"];
            //string sReqData = PostXML;
            //string sMsg = "";  // 解析之后的明文
            //int ret = wxcpt.DecryptMsg(sReqMsgSig, sReqTimeStamp, sReqNonce, sReqData, ref sMsg);
            //if (ret != 0)
            //{
            //    System.Console.WriteLine("解密: Decrypt Fail, ret: " + ret);
            //    Como.LogUtil.WriteException("解密: Decrypt Fail, ret: " + ret);
            //    return;
            //}
            //Como.LogUtil.WriteLog("解密后：XML：" + sMsg);
            //XmlDocument doc = new XmlDocument();
            //doc.LoadXml(sMsg);
            //XmlNode root = doc.FirstChild;

            //string MsgType = root["MsgType"].InnerText;
            //string FromUserName = root["FromUserName"].InnerText;
            //Como.LogUtil.WriteLog("MsgType为：" + MsgType + ",消息人为：" + FromUserName);

            //string SendMsg = "您好"; //给用户发送的消息提示内容

            ////发送的是文字：
            //if (MsgType == "text")
            //{
            //    string content = root["Content"].InnerText;
            //    Como.LogUtil.WriteLog("内容Content为：" + content);
            //    if (content.Contains("你好"))
            //    {
            //        SendMsg = "尊敬的：" + FromUserName + ":您好";
            //    }
            //    else
            //    {
            //        SendMsg = "尊敬的：" + FromUserName + ",抱歉不能理解您发送的消息！";
            //    }
            //    //SendMessageClass.SendMessage(FromUserName, SendMsg);  //回复用户消息
            //}
            ////发送的是图片
            //string imageUrl = "";           //图片服务器地址
            //string FtpPIcAddress = "";      //Ftp图片路径
            //if (MsgType == "image")
            //{
            //    Como.LogUtil.WriteLog("MsgType为：图片");
            //    try
            //    {
            //        string PicUrl = root["PicUrl"].InnerText;
            //        Como.LogUtil.WriteLog("PicUrl为：" + PicUrl);    //上传后的网络路径
            //        string path = context.Server.MapPath("~/WeinImages/");
            //        Como.Tools.CreateFolder(path);
            //        string File_Name = "MY_" + FromUserName + "_WxTP" + DateTime.Now.ToString("yyyyMMddssmmfff") + ".jpg";
            //        imageUrl = path + File_Name;
            //        //Como.LogUtil.WriteLog("服务器路径为：" + imageUrl);
            //        Como.Tools.DownloadPicture(PicUrl, imageUrl, -1);
            //        if (File.Exists(imageUrl))
            //        {
            //            Como.LogUtil.WriteLog("存在下载的文件！");
            //            Como.FtpHelper.UploadPIC(FtpHelper.FtpName, FtpHelper.FtpPassword, imageUrl, DateTime.Now.ToString("yyyy-MM-dd"), ref FtpPIcAddress);
            //            if (FtpPIcAddress != "")  //上传FTP 成功！
            //            {
            //                Como.LogUtil.WriteLog("成功上传 FTP上 ，已保存我的图片下：" + FtpPIcAddress);
            //                string name = File_Name;  //文件名称
            //                int result = BLL.MyPic.InsertPic(FromUserName, FtpPIcAddress, name, "", "", "");
            //                if (result > 0)
            //                {
            //                    Como.LogUtil.WriteLog("成功上传！到数据库");
            //                    SendMsg = "上传成功，已保存在我的图片下：" + File_Name;
            //                }
            //                else
            //                {
            //                    SendMsg = "抱歉，上传异常，请重试！";
            //                }
            //            }
            //        }
            //        else
            //        {
            //            Como.LogUtil.WriteLog("不存在下载的文件！");
            //        }

            //    }
            //    catch (Exception ex)
            //    {
            //        Como.LogUtil.WriteException("上传后的文件未能下载成功！");
            //    }
            //    SendMessageClass.SendMessage(FromUserName, SendMsg);  //回复用户消息
            //}

        }


        //3. 回复用户消息
        /// </summary>
        /// <param name="UserID">要发送的人ID</param>
        /// <param name="StrMessg">消息</param>
        //        private void SendMessage(string UserID, string StrMessg)
        //        {
        //            Como.LogUtil.WriteLog("回复用户" + UserID + "消息");
        //            string Access_Token = Como.GetAccessToken.GetAccess_token();
        //            if (Access_Token == "")
        //                Como.LogUtil.WriteException("SendMessage 未能成功加载Access_Token");
        //            string Text = @"{
        //   ""touser"":";
        //            Text += '"' + UserID + '"';
        //            Text += "," + '"' + @"msgtype"": ""text"", 
        //           ""agentid"": ""5"", 
        //            ""text"": {
        //           ""content"":";
        //            Text += '"' + StrMessg + '"';
        //            Text += @"}, 
        //    ""safe"": ""0""
        //}";
        //            ;
        //            string url = String.Format("https://qyapi.weixin.qq.com/cgi-bin/message/send?access_token={0}", Access_Token);
        //            string strResult = Como.Tools.GetPage(url, Text);

        //            JArray ja = (JArray)JsonConvert.DeserializeObject("[" + strResult + "]");
        //            string Error = "";
        //            try
        //            {
        //                if (strResult.Contains("errcode"))
        //                {
        //                    Error = ja[0]["errcode"].ToString();
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                Como.LogUtil.WriteException("获取strResult,未获取到错误信息" + ex.Message.ToString());
        //            }
        //            string errcode = ja[0]["errcode"].ToString();
        //            string errmsg = ja[0]["errmsg"].ToString();
        //            if (errcode == "0" && errmsg == "ok")
        //            {
        //                Como.LogUtil.WriteLog("回复成功！");
        //            }
        //            else
        //            {
        //                Como.LogUtil.WriteLog("回复失败！");
        //                Como.LogUtil.WriteException("回复失败：SendMessage：" + strResult);
        //            }
        //        }

        #endregion


        #endregion
    }
}