using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SignalRChat
{
    public partial class Login : System.Web.UI.Page
    {
        //Class Object
        ConnClass ConnC = new ConnClass();
        protected void Page_Load(object sender, EventArgs e)
        {
           
        }

        protected void btnSignIn_Click(object sender, EventArgs e)
        {
            string Query = "select * from tbl_Users where Email='" + txtEmail.Value + "' and Password='" + txtPassword.Value + "'";
            if (ConnC.IsExist(Query))
            {
                string UserName = ConnC.GetColumnVal(Query, "UserName");
                Session["UserName"] = UserName;
                string Badge = ConnC.GetColumnVal(Query, "Badge");
                Session["UserBadge"] = Badge;
                string EnrollNo = ConnC.GetColumnVal(Query, "EnrollNo");
                Session["UserEnrollNo"] = EnrollNo;
                string Department = ConnC.GetColumnVal(Query, "Department");
                Session["UserDepartment"] = Department;
               
                Session["UserEmail"] = txtEmail.Value;
                Response.Redirect("WebForm1.aspx");
            }
            else
                txtEmail.Value = "Invalid Email or Password!!";
        }
    }
}