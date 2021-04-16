﻿using System;
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

        public List<MySqlDataReader> GetAllData(string Query)
        {
            List<MySqlDataReader> RetVal = new List<MySqlDataReader>();
            using (cmd = new MySqlCommand(Query, con))
            {
                con.Open();
                sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    RetVal.Add(sdr);
                }
                sdr.Close();
                con.Close();
            }

            return RetVal;
        }
        public List<string> GetAllFromColumn(string Query, string ColumnName)
        {
            List<string> RetVal = new List<string>();
            using (cmd = new MySqlCommand(Query, con))
            {
                con.Open();
                sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    RetVal.Add(sdr[ColumnName].ToString());
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