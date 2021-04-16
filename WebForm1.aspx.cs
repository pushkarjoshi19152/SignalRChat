using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SignalRChat
{
    public partial class WebForm1 : System.Web.UI.Page
    {

        public List<string> RegisteredUsers = new List<string>();
        ConnClass conc = new ConnClass();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            string GetRegisteredUsersQuery = "select UserName from tbl_users";
            RegisteredUsers = conc.GetAllFromColumn(GetRegisteredUsersQuery, "UserName");
        }

    }
}