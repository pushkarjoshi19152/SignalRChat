using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using SD = System.Drawing;
using System.Web.UI.HtmlControls;

namespace SignalRChat
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        public string UserName = "admin";
        public string UserBadge = "admin";
        public string UserEnrollNo = "admin";
        public string UserDepartment = "admin";
        public string UserImage = "/images/DP/dummy.png";
        protected string UploadFolderPath = "~/Uploads/";
      
        private string fromUser = "";
        public string UserEmail = "";

        public List<string> RegisteredUsers = new List<string>();
        ConnClass conc = new ConnClass();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            string GetRegisteredUsersQuery = "select UserName from tbl_users";
            RegisteredUsers = conc.GetAllFromColumn(GetRegisteredUsersQuery, "UserName");

            if (Session["UserName"] != null)
            {
                UserName = Session["UserName"].ToString();
                UserBadge = Session["UserBadge"].ToString();
                UserEnrollNo = Session["UserEnrollNo"].ToString();
                UserDepartment = Session["UserDepartment"].ToString();
                UserEmail = Session["UserEmail"].ToString();
                fromUser += UserName;
                // GetUserImage(UserName);

            }
            //else
            //    // Response.Redirect("Login.aspx");
            //    ;

            this.Header.DataBind();
        }
    }
}