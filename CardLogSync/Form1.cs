using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace CardLogSync
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        System.Timers.Timer t;
        private SqlConnection SourceConn = new SqlConnection();
        private SqlConnection TargetConn = new SqlConnection();
        string strSourConn = ConfigurationManager.ConnectionStrings["AccessConnectionString"].ConnectionString;
        string strTargerConn = ConfigurationManager.ConnectionStrings["SeatConnectionString"].ConnectionString;
        private void button1_Click(object sender, EventArgs e)
        {
            SourceConn.ConnectionString = strSourConn;
            TargetConn.ConnectionString = strTargerConn;
            t = new System.Timers.Timer(5000);
            t.Elapsed += new System.Timers.ElapsedEventHandler(t_Elapsed);
            t.Enabled = true;
            t.Start();
        }

        void t_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            t.Stop();
            try
            {
                string MaxID = "0";
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select max(VisitNo) from shuakajilu");
                DataSet ds = new DataSet();
                try
                {
                    TargetConn.Open();
                    SqlDataAdapter command = new SqlDataAdapter(strSql.ToString(), TargetConn);
                    command.Fill(ds, "ds");

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        MaxID = ds.Tables[0].Rows[0][0].ToString();
                    }
                }
                catch (Exception ex)
                {
                    SeatManage.SeatManageComm.WriteLog.Write("获取最大ID失败：" + ex.Message);
                }
                finally
                {
                    TargetConn.Close();
                }

                StringBuilder strSqls = new StringBuilder();
                strSqls.Append("SELECT [VisitNo],[CardId],[name],[type],[dept],[Passed],[VisitTime],[gateno],[direction] from Visit");
                strSqls.Append(" WHERE [VisitNo]>'" + MaxID + "' AND datediff(second,[VisitTime],getdate())<=7");
                DataSet ds1 = new DataSet();
                try
                {
                    SourceConn.Open();
                    SqlDataAdapter command = new SqlDataAdapter(strSqls.ToString(), SourceConn);
                    command.Fill(ds1, "ds1");
                }
                catch (Exception ex)
                {
                    SeatManage.SeatManageComm.WriteLog.Write("获取刷卡记录失败:" + ex.Message);
                }
                finally
                {
                    SourceConn.Close();
                }
                
                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    string cmdstr = "insert into shuakajilu ([VisitNo],[CardId],[name],[type],[dept],[Passed],[VisitTime],[gateno],[direction],[HandleFlag]) " +
                    "values('"
                    + ds1.Tables[0].Rows[i]["VisitNo"] + "','"
                    + ds1.Tables[0].Rows[i]["CardId"] + "','"
                    + ds1.Tables[0].Rows[i]["name"] + "','"
                    + ds1.Tables[0].Rows[i]["type"] + "','"
                    + ds1.Tables[0].Rows[i]["dept"] + "','"
                    + ds1.Tables[0].Rows[i]["Passed"] + "','"
                    + ds1.Tables[0].Rows[i]["VisitTime"] + "','"
                    + ds1.Tables[0].Rows[i]["gateno"] + "','"
                    + ds1.Tables[0].Rows[i]["direction"] + "','"
                    + 0 + "') ";
                    SqlCommand sqlcmd = new SqlCommand(cmdstr, TargetConn);
                    int count = -1;
                    try
                    {
                        TargetConn.Open();
                        count = sqlcmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        SeatManage.SeatManageComm.WriteLog.Write("记录插入失败：" + ex.Message);
                    }
                    finally
                    {
                        TargetConn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                SeatManage.SeatManageComm.WriteLog.Write(ex.Message);
            }
            finally
            {
                t.Start();
            }
        }
    }
}
