using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace CardLogSync.Code
{
    public class AccessSync
    {
        private SqlConnection AccessConn = new SqlConnection();
        private SqlConnection SeatConn = new SqlConnection();
        string strAccessConn = ConfigurationManager.ConnectionStrings["AccessConnectionString"].ConnectionString;
        string strSeatConn = ConfigurationManager.ConnectionStrings["SeatConnectionString"].ConnectionString;
        string libNo = ConfigurationManager.AppSettings["LibNo"];
        string beforeTime = ConfigurationManager.AppSettings["BeforeTime"];
        /// <summary>
        /// 消息通知
        /// </summary>
        /// <param name="message"></param>
        public delegate void EventHandleSync(string message);
        /// <summary>
        /// 进度事件
        /// </summary>
        public event EventHandleSync SyncMessage;
        /// <summary>
        /// 同步信息
        /// </summary>
        public event EventHandleSync SyncProgress;

        public AccessSync()
        {
            AccessConn.ConnectionString = strAccessConn;
            SeatConn.ConnectionString = strSeatConn;
        }
        /// <summary>
        /// 开始同步
        /// </summary>
        public void Sync()
        {
            try
            {
                int allCount = 0;
                int successCount = 0;
                int failCount = 0;
                if (SyncProgress != null)
                {
                    SyncProgress("开始获取数据...");
                }
                DataSet ds = GetAccessLogList();
                allCount = ds.Tables[0].Rows.Count;
                if (SyncProgress != null)
                {
                    SyncProgress("获取记录" + allCount + "条");
                }
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Model.AccessModelLog model = DataRowToModel(dr);
                    if (!AddLog(model))
                    {
                        if (SyncProgress != null)
                        {
                            SyncProgress("添加记录失败：" + model.VisitTime + " " + (string.IsNullOrEmpty(model.CardNo) ? model.CardID : model.CardNo) + " " + model.VisiterName);
                        }
                        failCount++;
                    }
                    else
                    {
                        if (SyncProgress != null)
                        {
                            SyncProgress("添加门禁记录：" + model.VisitTime + " " + (string.IsNullOrEmpty(model.CardNo) ? model.CardID : model.CardNo) + " " + model.VisiterName);
                        }
                        successCount++;
                    }
                }
                if (SyncProgress != null)
                {
                    SyncProgress("同步完成：总计" + allCount + "条，成功" + successCount + "条，失败" + failCount + "条");
                }
            }
            catch (Exception ex)
            {
                if (SyncProgress != null)
                {
                    SyncProgress("同步失败：" + ex.Message);
                }
            }
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        private DataSet GetAccessLogList()
        {
            DataSet ds = new DataSet();
            try
            {
                //[AccessSyncLog]([VisitID],[VisitTime],[CardNo],[CardID],[VisierName],[Library],[EnterOrExit],[ChannelNo],[Flag],[HandleFlag]

                string beforesecond = "7";
                if (!string.IsNullOrEmpty(beforeTime))
                {
                    beforesecond = beforeTime;
                }
                StringBuilder strSqls = new StringBuilder();

                //string lsatDateTime = GetLastDateTime();
                //strSqls.Append("SELECT [ReadNum] as CardNo,[CDate] as VisitTime,[InPort] as ChannelNo,[ReadName] VisierName from [ReadCardBrush]");
                //strSqls.Append(" WHERE [CDate]>'" + lsatDateTime + "'AND datediff(second,[CDate],getdate())<=" + beforesecond);


                int lastID = GetLastID();
                strSqls.Append("SELECT [VisitNo] as VisitID,[CardId] as CardNo,[name] as VisierName,[type],[dept],[Passed],[VisitTime] as VisitTime,[gateno] as ChannelNo,[direction] as EnterOrExit from visit");
                strSqls.Append(" WHERE [VisitNo]>'" + lastID + "' AND datediff(second,[VisitTime],getdate())<=" + beforesecond + " AND [gateno] in ('01','02','03','04','05')");

                AccessConn.Open();
                SqlDataAdapter command = new SqlDataAdapter(strSqls.ToString(), AccessConn);
                command.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                SeatManage.SeatManageComm.WriteLog.Write("获取刷卡记录失败:" + ex.Message);
                throw ex;
            }
            finally
            {
                AccessConn.Close();
            }

            //for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            //{

        }
        //private Model.AccessModelLog DataRowToModel(DataRow dr)
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private bool AddLog(Model.AccessModelLog model)
        {
            string cmdstr = "INSERT INTO [AccessSyncLog]([VisitID],[VisitTime],[CardNo],[CardID],[VisierName],[Library],[EnterOrExit],[ChannelNo],[Flag],[HandleFlag])"
                  + "values('" + model.VisitID + "','"
                  + model.VisitTime + "','"
                  + model.CardNo + "','"
                  + model.CardID + "','"
                  + model.VisiterName + "','"
                  + model.LibraryNo + "','"
                  + (int)model.Enter + "','"
                  + model.ChannelNo + "','"
                  + model.Flag + "','"
                  + false + "')";
            //string sqlStr = string.Format(cmdstr, model.VisitID, model.VisitTime.ToString(), model.CardNo, model.CardID, model.VisiterName, model.LibraryNo, (int)model.Enter, model.ChannelNo, model.Flag, false);
            //SeatManage.SeatManageComm.WriteLog.Write(sqlStr);
            SqlCommand sqlcmd = new SqlCommand(cmdstr, SeatConn);
            int count = -1;
            try
            {
                SeatConn.Open();
                count = sqlcmd.ExecuteNonQuery();
                if (count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                SeatManage.SeatManageComm.WriteLog.Write("记录插入失败：" + ex.Message);
                throw ex;
            }
            finally
            {
                SeatConn.Close();
            }
        }
        /// <summary>
        /// 转化moel
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private Model.AccessModelLog DataRowToModel(DataRow dr)
        {
            Model.AccessModelLog model = new Model.AccessModelLog();
            if (dr.Table.Columns.Contains("VisitID"))
            {
                model.VisitID = int.Parse(dr["VisitID"].ToString());
            }
            if (dr.Table.Columns.Contains("VisitTime"))
            {
                SeatManage.SeatManageComm.WriteLog.Write(dr["VisitTime"].ToString());
                model.VisitTime = DateTime.Parse(dr["VisitTime"].ToString());
            }
            if (dr.Table.Columns.Contains("CardNo"))
            {
                model.CardNo = dr["CardNo"].ToString().Trim();
            }
            if (dr.Table.Columns.Contains("CardID"))
            {
                model.CardNo = dr["CardID"].ToString();
            }
            if (dr.Table.Columns.Contains("VisierName"))
            {
                model.VisiterName = dr["VisierName"].ToString();
            }
            if (dr.Table.Columns.Contains("EnterOrExit"))
            {
                model.Enter = (Model.EnterState)int.Parse(dr["EnterOrExit"].ToString());
            }
            if (dr.Table.Columns.Contains("ChannelNo"))
            {
                model.ChannelNo = dr["ChannelNo"].ToString();
            }
            if (dr.Table.Columns.Contains("Flag"))
            {
                model.Flag = int.Parse(dr["Flag"].ToString());
            }
            //特殊处理
            if (!string.IsNullOrEmpty(libNo))
            {
                model.LibraryNo = libNo;
            }
            return model;
        }

        /// <summary>
        /// 获取最后记录的时间
        /// </summary>
        /// <returns></returns>
        private string GetLastDateTime()
        {
            string MaxDateTime = "2015-1-1 0:00:00";
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select max(VisitTime) from AccessSyncLog");
            if (!string.IsNullOrEmpty(libNo))
            {
                strSql.Append(" Where Library='" + libNo + "'");
            }
            DataSet ds = new DataSet();
            try
            {
                SeatConn.Open();
                SqlDataAdapter command = new SqlDataAdapter(strSql.ToString(), SeatConn);
                command.Fill(ds, "ds");
                if (ds.Tables[0].Rows.Count > 0 && !string.IsNullOrEmpty(ds.Tables[0].Rows[0][0].ToString()))
                {
                    MaxDateTime = ds.Tables[0].Rows[0][0].ToString();
                }
                return MaxDateTime;
            }
            catch (Exception ex)
            {
                SeatManage.SeatManageComm.WriteLog.Write("获取最后的时间失败：" + ex.Message);
                throw ex;
            }
            finally
            {
                SeatConn.Close();
            }

        }

        /// <summary>
        /// 获取最后记录的 ID
        /// </summary>
        /// <returns></returns>
        private int GetLastID()
        {
            int MaxID = -1;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select max(VisitID) from AccessSyncLog");
            if (!string.IsNullOrEmpty(libNo))
            {
                strSql.Append(" Where Library='" + libNo + "'");
            }
            DataSet ds = new DataSet();
            try
            {
                SeatConn.Open();
                SqlDataAdapter command = new SqlDataAdapter(strSql.ToString(), SeatConn);
                command.Fill(ds, "ds");
                if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0][0] != null && !string.IsNullOrEmpty(ds.Tables[0].Rows[0][0].ToString()))
                {
                    MaxID = int.Parse(ds.Tables[0].Rows[0][0].ToString());
                }
                return MaxID;
            }
            catch (Exception ex)
            {
                SeatManage.SeatManageComm.WriteLog.Write("获取最后的ID失败：" + ex.Message);
                throw ex;
            }
            finally
            {
                SeatConn.Close();
            }

        }
    }
}
