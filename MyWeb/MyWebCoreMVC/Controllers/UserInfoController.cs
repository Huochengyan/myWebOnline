using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyWebCoreMVC.Models;

namespace MyWebCoreMVC.Controllers
{
    public class UserInfoController : Controller
    {
        public IActionResult Index()
        {
            List<UserInfo> userInfos = new List<UserInfo>();
            for (int i = 0; i < 100; i++)
            {
                UserInfo userInfo = new UserInfo();
                userInfo.Name = "Name_" + i.ToString();
                userInfo.Sex = i;
                userInfos.Add(userInfo);
            }
            ViewData["listuserinfo"] = userInfos;
            return View();
        }
    }
}