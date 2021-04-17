using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using MySql.Data.MySqlClient;


namespace SignalRChat
{
    public class ConnClass
    {
        public MySqlCommand cmd = new MySqlCommand();
        public MySqlDataAdapter sda;
        public MySqlDataReader sdr;
        public DataSet ds = new DataSet();
        public MySqlConnection con = new MySqlConnection(@"Server=localhost;Database=temp;Uid=root;Pwd=");

        public bool IsExist(string Query)
        {
            bool check = false;
            using (cmd = new MySqlCommand(Query, con))
            {

                con.Open();
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                    check = true;
            }
            sdr.Close();
            con.Close();
            return check;

        }

        public List<List<string>> GetAllData(string Query)
        {
            List<List<string>> RetVal = new List<List<string>>();
            using (cmd = new MySqlCommand(Query, con))
            {
                con.Open();
                sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    List<string> temp = new List<string>();
                    for(int i = 0; i < sdr.FieldCount; i++)
                    {
                        temp.Add(sdr[i].ToString());
                    }
                    RetVal.Add(temp);
                }
            }
            sdr.Close();
            con.Close();
            return RetVal;
        }
        public List<List<string>> GetAllFromColumn(string Query, params string [] ColumnName)
        {
            List<List<string>> RetVal = new List<List<string>>();

           
            using (cmd = new MySqlCommand(Query, con))
            {
                con.Open();
                sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {

                    List<string> temp = new List<string>() ;
                    temp.Add(sdr[ColumnName[0]].ToString());
                    //temp.Add(sdr[ColumnName[ColumnName.Length-1]].ToString());
                    RetVal.Add(temp);
                }
                sdr.Close();
                con.Close();
            }

            return RetVal;
        }

        public bool ExecuteQuery(string Query)
        {
            int j = 0;
            using (cmd = new MySqlCommand(Query, con))
            {
                con.Open();
                j = cmd.ExecuteNonQuery();
                con.Close();
            }

            if (j > 0)
                return true;
            else
                return false;

        }

        public string GetColumnVal(string Query, string ColumnName)
        {
            string RetVal = "";
            using (cmd = new MySqlCommand(Query, con))
            {
                con.Open();
                sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    RetVal = sdr[ColumnName].ToString();
                    break;
                }
                sdr.Close();
                con.Close();
            }

            return RetVal;


        }

    }
}