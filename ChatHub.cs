using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;


namespace SignalRChat
{
    public class ChatHub : Hub
    {
        static List<Users> ConnectedUsers = new List<Users>();
        static List<Messages> CurrentMessage = new List<Messages>();
        public List<List<string>> RegisteredUsers = new List<List<string>>();
        ConnClass ConnC = new ConnClass();

        public void Connect(string userName, string userBadge, string userEnrollNo, string userDepartment, string userEmail)
        {
            var id = Context.ConnectionId;
            


            if (ConnectedUsers.Count(x => x.ConnectionId == id) == 0)
            {
                string UserImg = GetUserImage(userName);
                string logintime = DateTime.Now.ToString();

                ConnectedUsers.Add(new Users { ConnectionId = id, UserName = userName, UserImage = UserImg, LoginTime = logintime,Badge=userBadge,EnrollNo=userEnrollNo,Department= userDepartment,Email=userEmail});
                
               //send to caller
               Clients.Caller.onConnected(id, userName, ConnectedUsers, CurrentMessage);
                string GetRegisteredUsersQuery = "SELECT UserName,EnrollNo FROM tbl_users";
                string[] ColumnName =  {"UserName","EnrollNo" };
                RegisteredUsers = ConnC.GetAllData(GetRegisteredUsersQuery);

                Clients.Caller.loadRegisteredUsers(RegisteredUsers);

                // send to all except caller client
             //       Clients.AllExcept(id).onNewUserConnected(id, userName, UserImg, logintime);
            }
        }

        public void SendMessageToAll(string userName, string message, string time)
        {
            string UserImg = GetUserImage(userName);
            // store last 100 messages in cache
            AddMessageinCache(userName, message, time, UserImg);

            // Broad cast message
            Clients.All.messageReceived(userName, message, time, UserImg);

        }

        private void AddMessageinCache(string userName, string message, string time, string UserImg)
        {
            CurrentMessage.Add(new Messages { UserName = userName, Message = message, Time = time, UserImage = UserImg });

            if (CurrentMessage.Count > 100)
                CurrentMessage.RemoveAt(0);

        }

        // Clear Chat History
        public void clearTimeout()
        {
            CurrentMessage.Clear();
        }

        public string GetUserImage(string username)
        {
            string RetimgName = "images/dummy.png";
            try
            {
                string query = "select Photo from tbl_Users where UserName='" + username + "'";
                string ImageName = ConnC.GetColumnVal(query, "Photo");

                if (ImageName != "")
                    RetimgName = "images/DP/" + ImageName;
            }
            catch (Exception ex)
            { }
            return RetimgName;
        }

        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            var item = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            if (item != null)
            {
                ConnectedUsers.Remove(item);

                var id = Context.ConnectionId;
                Clients.All.onUserDisconnected(id, item.UserName);

            }
            return base.OnDisconnected(stopCalled);
        }

        public void CreateTableFor(string table_name,string fromUserEN,string toUserEN)
        {
            
            string CreateTableQuery = "CREATE TABLE " + table_name + "(time varchar(30), message varchar(20),c" + fromUserEN + " boolean,c" + toUserEN + " boolean)";

            ConnC.ExecuteQuery(CreateTableQuery);
            
            
        }

        public void AddMessageTo(string table_name,string message,string fromUserEN,string toUserEN)
        {
           
            try
            {
                                   
                string AddMessageQuery = "insert into " + table_name + "(time,message,c" + fromUserEN + ",c" + toUserEN + ") values('" + DateTime.Now.ToString() + "','" + message + "','" + 0 + "','" + 1 + "')";

                ConnC.ExecuteQuery(AddMessageQuery);
                
                
                
            }
            catch(Exception e)
            {
                ConnC.con.Close();
                CreateTableFor(table_name,fromUserEN,toUserEN);
                AddMessageTo(table_name,message,fromUserEN,toUserEN);
            }
        }

        public void SendPrivateMessage(string toUserId, string message)
        {

            string fromUserId = Context.ConnectionId;

            var toUser = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == toUserId);
            var fromUser = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == fromUserId);
            
            string fromUserEnrollNo = fromUser.EnrollNo;
            string toUserEnrollNo = toUser.EnrollNo;

            
         

            //tablename for this conversation
            string table_name;
            try
            {
                table_name = fromUser.TableNameFor[toUser.EnrollNo];
            }
            catch(Exception e)
            {

                string[] arr = new string[] { fromUserEnrollNo, toUserEnrollNo };
                Array.Sort(arr);
                fromUser.TableNameFor.Add(toUser.EnrollNo,"f"+arr[0]+"to"+arr[1]);
                table_name = fromUser.TableNameFor[toUser.EnrollNo];
            }
            AddMessageTo(table_name,message,fromUserEnrollNo,toUserEnrollNo);
            if (toUser != null && fromUser != null)
            {
                string CurrentDateTime = DateTime.Now.ToString();
                string UserImg = GetUserImage(fromUser.UserName);
                // send to 
                Clients.Client(toUserId).sendPrivateMessage(fromUserId, fromUser.UserName, message, UserImg, CurrentDateTime);

                // send to caller user
                Clients.Caller.sendPrivateMessage(toUserId, fromUser.UserName, message, UserImg, CurrentDateTime);
            }

        }
    }
}