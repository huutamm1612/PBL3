using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    internal class Database
    {
        private SqlConnection con;
        private static Database _instance; 

        public static Database Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new Database();
                }
                return _instance;
            }
            private set { }
        }

        private Database()
        {
            string strCon = @"Data Source=ASUS\HUUTAM;Initial Catalog=PBL3_Database;Integrated Security=True;MultipleActiveResultSets=true;";
            con = new SqlConnection(strCon);
        }

        public void ExecuteNonQuery(string sql, params SqlParameter[] args)
        {
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddRange(args);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public DataTable ExecuteQuery(string query, params SqlParameter[] args)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(query, con);
            da.SelectCommand.Parameters.AddRange(args);
            con.Open();
            da.Fill(dt);
            con.Close(); 

            return dt;
        }

        public string MaMoi(string loaiMa)
        {
            string query = $"SELECT {loaiMa} FROM MaHienTai";
            DataTable table = ExecuteQuery(query);

            string ma = table.Rows[0][loaiMa].ToString();
            string maMoi = (long.Parse(ma) + 1).ToString("D10");

            query = $"UPDATE maHienTai SET {loaiMa} = '{maMoi}'";
            ExecuteNonQuery(query);

            return maMoi;
        }
    }
}
