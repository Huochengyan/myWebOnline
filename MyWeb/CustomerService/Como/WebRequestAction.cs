using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;

namespace CustomerService.Como
{
    public class WebRequestAction
    {
        /// <summary>
        /// Post请求  WebRequest
        /// </summary>
        /// <param name="postUrl"></param>
        /// <param name="paramData"></param>
        /// <param name="dataEncode"></param>
        /// <returns></returns>
        public static string PostWebRequest(string postUrl, string paramData, Encoding dataEncode)
        {
            ServicePointManager.ServerCertificateValidationCallback += RemoteCertificateValidate;


            string ret = string.Empty;
            try
            {
                byte[] byteArray = dataEncode.GetBytes(paramData); //转化
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(postUrl));
                webReq.Method = "POST";
                webReq.ContentType = "application/json";
                webReq.Timeout = 10000;
                webReq.ContentLength = byteArray.Length;
                Stream newStream = webReq.GetRequestStream();
                newStream.Write(byteArray, 0, byteArray.Length);//写入参数
                newStream.Close();

                HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();

                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8"));
                ret = sr.ReadToEnd();
                sr.Close();
                response.Close();
                newStream.Close();
            }
            catch (Exception ex)
            {
                return "";
            }
            return ret;
        }

        private static bool RemoteCertificateValidate(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors error)
        {
            //为了通过证书验证，总是返回true
            return true;
        }

        public static string GetUrl(string url)
        {
            // 设置参数
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            CookieContainer cookieContainer = new CookieContainer();
            request.CookieContainer = cookieContainer;
            request.AllowAutoRedirect = true;
            request.Method = "Get";
            request.ContentType = "text/html";
            request.Headers.Add("charset", "utf-8");

            //发送请求并获取相应回应数据
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            //直到request.GetResponse()程序才开始向目标网页发送Post请求
            Stream responseStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(responseStream, Encoding.UTF8);
            //返回结果网页（html）代码
            string content = sr.ReadToEnd();
            return content;
        }

    }
}