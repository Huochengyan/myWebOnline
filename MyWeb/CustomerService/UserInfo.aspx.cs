using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CustomerService
{
    public partial class UserInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        //[HttpGet]
        public  List<UserInfoDetails> GetUserInfos()
        {
            List<UserInfoDetails> userInfos = new List<UserInfoDetails>();
            for (int i = 0; i < 10; i++)
            {
                UserInfoDetails userInfo = new UserInfoDetails();
                userInfo.Id = i;
                userInfo.UserName = i.ToString();
                userInfos.Add(userInfo);
            }
            return userInfos;
        }
    }

    public class UserInfoDetails
    {
        private int _id = 0;
        private string _UserName = "";
        private string _UserPassword = "";

        public int Id { get => _id; set => _id = value; }
        public string UserName { get => _UserName; set => _UserName = value; }
        public string UserPassword { get => _UserPassword; set => _UserPassword = value; }
    }
}