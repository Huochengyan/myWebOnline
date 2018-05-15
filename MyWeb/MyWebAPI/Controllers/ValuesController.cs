using MyWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MyWebAPI.Controllers
{
    public class ValuesController : ApiController
    {
        //[HttpGet]
        //public List<string> GetListstr()
        //{
        //    List<string> list = new List<string>();
        //    for (int i = 0; i < 10; i++)
        //    {
        //        list.Add(i.ToString());
        //    }
        //    return list;
        //}

        //[HttpPost]
        //public List<string> GetListstr2()
        //{
        //    List<string> list = new List<string>();
        //    for (int i = 0; i < 10; i++)
        //    {
        //        list.Add(i.ToString());
        //    }
        //    return list;
        //}
        [HttpGet]
        public List<UserInfo> GetAllUserInfo()
        {
            List<UserInfo> userInfos = new List<UserInfo>();
            for (int i = 0; i < 100; i++)
            {
                UserInfo userInfo = new UserInfo();
                userInfo.Id = i;
                userInfo.Name = "Name_" + i.ToString();
                userInfo.Phone = "Phone_" + i.ToString();
                userInfos.Add(userInfo);
            }
            return userInfos;
        }
    }
   

}
