using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SuperSocket.SocketBase;
using SuperWebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomerService.Websocket
{
    public class WebsocketServer
    {
        public  static  Dictionary<string, WebSocketSession> dicSession = new Dictionary<string, WebSocketSession>();
        public  static  Queue<JObject> queueMsg = new Queue<JObject>();
        public void Start()
        {
           
            WebSocketServer server = new WebSocketServer();
            server.NewSessionConnected += server_NewSessionConnected;
            server.NewMessageReceived += server_NewMessageReceived;
            server.SessionClosed += server_SessionClosed;
             try
             {
                 server.Setup("127.0.0.1",60152);//设置端口
                 server.Start();//开启监听
                 Console.WriteLine("开启监听");
             }
             catch (Exception ex)
             {
                 Console.WriteLine(ex.Message);
             }
             Console.ReadKey();
            }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="session"></param>
        /// <param name="value"></param>
        private void server_SessionClosed(WebSocketSession session, CloseReason value)
        {
            Console.WriteLine(session.Origin);

            if (dicSession.ContainsKey(GetSessionName(session).Split('/')[0]))
            {
                dicSession.Remove(GetSessionName(session).Split('/')[0]);
            }
        }
        /// <summary>
        /// 接收前台 消息
        /// </summary>
        /// <param name="session"></param>
        /// <param name="value"></param>
        private void server_NewMessageReceived(WebSocketSession session, string value)
        {
            Console.WriteLine("前台消息："+value);
            JObject job = (JObject)JsonConvert.DeserializeObject(value);
            if (job.GetValue("toUserName").ToString() == "")
                return;
            Msg.SendMessage(job.GetValue("toUserName").ToString(), job.GetValue("content").ToString());
        }

        private void server_NewSessionConnected(WebSocketSession session)
        {
            Console.WriteLine(session.Origin);
            Console.WriteLine(session.Host);

            if (!dicSession.ContainsKey(GetSessionName(session).Split('/')[0]))
            {
                dicSession.Add(GetSessionName(session).Split('/')[0], session);
            }
            Console.WriteLine("当前连接数："+dicSession.Count);
        }
        private string GetSessionName(WebSocketSession session)
        {
            //这里用Path来取Name 不太科学…… 
            Console.WriteLine(session.Path.TrimStart('/'));
            return HttpUtility.UrlDecode(session.Path.TrimStart('/'));
        }
    }
}