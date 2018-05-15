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
        public void Start()
        {
            WebSocketServer server = new WebSocketServer();
            server.NewSessionConnected += server_NewSessionConnected;
            server.NewMessageReceived += server_NewMessageReceived;
            server.SessionClosed += server_SessionClosed;
             try
             {
                 server.Setup("127.0.0.1", 60152);//设置端口
                 server.Start();//开启监听
                 Console.WriteLine("开启监听");
             }
             catch (Exception ex)
             {
                 Console.WriteLine(ex.Message);
             }
             Console.ReadKey();
            }

        private void server_SessionClosed(WebSocketSession session, CloseReason value)
        {
            Console.WriteLine(session.Origin);
        }

        private void server_NewMessageReceived(WebSocketSession session, string value)
        {
            Console.WriteLine(value);
            session.Send(value);
        }

        private void server_NewSessionConnected(WebSocketSession session)
        {
            Console.WriteLine(session.Origin);
        }
    }
}