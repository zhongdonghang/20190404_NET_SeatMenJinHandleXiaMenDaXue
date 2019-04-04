using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using System.Data;
namespace PosDataMiddleTableHandler
{
    /// <summary>
    /// 中间表处理程序
    /// </summary>
    public class MiddleTableHandler_New
    {

        /// <summary>
        /// 删除指定日期之前的数据
        /// </summary>
        /// <param name="beforeDate">指定的日期</param> 
        /// <returns></returns>
        public static bool Delete(DateTime beforeDate)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("DELETE FROM [AccessSyncLog] where datediff(day,VisitTime,'{0}')>=0", beforeDate);
            try
            {
              //  DBUtility.DbHelperSQL
                int result = DBUtility.DbHelperSQL.ExecuteSql(strSql.ToString());

                if (result >= 0)
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
                throw ex;
            }
        }

        /// <summary>
        /// 获取还没处理的刷卡数据
        /// </summary>
        /// <returns></returns>
        public static List<AccessModelLog> GetModels(int effectiveTime)
        {
            List<AccessModelLog> posDataList = new List<AccessModelLog>();
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(string.Format("SELECT [ID],[VisitID],[VisitTime],[CardNo],[CardID],[VisierName],[Library],[EnterOrExit],[ChannelNo],[Flag],[HandleFlag] FROM [AccessSyncLog] where  datediff(mi,VisitTime,getdate())<{0} and HandleFlag<>1 or HandleFlag is Null", effectiveTime));
            try
            {
                DataSet ds = DBUtility.DbHelperSQL.Query(strSql.ToString());
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    AccessModelLog model = DataRowToModel(ds.Tables[0].Rows[i]);
                    if (model != null)
                    {
                        posDataList.Add(model);
                    }
                }
                return posDataList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 更新已经处理的记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool Update(AccessModelLog model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("update AccessSyncLog set HandleFlag=1 where ID={0} ", model.ID);
            try
            {
                int result = DBUtility.DbHelperSQL.ExecuteSql(strSql.ToString());
                if (result >= 0)
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
                throw ex;
            }
        }
        /// <summary>
        /// 把所有的记录都标识为已处理
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool UpdateAll()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update AccessSyncLog set HandleFlag=1 where HandleFlag=0 ");
            try
            {
                int result = DBUtility.DbHelperSQL.ExecuteSql(strSql.ToString());
                if (result >= 0)
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
                throw ex;
            }
        }

        /// <summary>
        /// DataRow转换为Model
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        //private static AccessModelLog dataRowToModel(DataRow dr)
        //{
        //    if (dr != null)
        //    {
        //        try
        //        {
        //            PosDataModel model = new PosDataModel();
        //            model.Id = dr["VisitNo"].ToString();
        //            model.CardNo = dr["ReadNum"].ToString().Trim();
        //            model.Passed = "";
        //            string tempTime = dr["CDate"].ToString().Trim();
        //            if (!string.IsNullOrEmpty(tempTime))
        //            {
        //                model.VisitTime = DateTime.Parse(dr["CDate"].ToString());
        //            }
        //            else
        //            {
        //                model.VisitTime =DateTime.Parse( "1900-1-1");
        //            }
        //            string tempDirection ="";
        //            if (!string.IsNullOrEmpty(tempDirection))
        //            {
        //                model.Direction = 0;
        //            }
        //            else
        //            {
        //                model.Direction = -1;
        //            }
        //            model.HandleFlag = dr["HandleFlag"].ToString();
        //            return model;
        //        }
        //        catch (Exception ex)
        //        {
        //            SeatManage.SeatManageComm.WriteLog.Write(string.Format("解析刷卡数据遇到异常，该条数据被忽略。数据Id:{0}  异常原因：{1}", dr["VisitNo"].ToString(), ex.Message));
        //            return null;
        //        }
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        private static Model.AccessModelLog DataRowToModel(DataRow dr)
        {
            Model.AccessModelLog model = new Model.AccessModelLog();
            if (dr["VisitID"] != null)
            {
                model.VisitID = int.Parse(dr["VisitID"].ToString());
            }
            if (dr["VisitTime"] != null)
            {
                model.VisitTime = DateTime.Parse(dr["VisitTime"].ToString());
            }
            if (dr["CardNo"] != null)
            {
                model.CardNo = dr["CardNo"].ToString();
            }
            if (dr["CardID"] != null)
            {
                model.CardID = dr["CardID"].ToString();
            }
            if (dr["VisierName"] != null)
            {
                model.VisiterName = dr["VisierName"].ToString();
            }
            if (dr["EnterOrExit"] != null)
            {
                model.Enter = (Model.EnterState)int.Parse(dr["EnterOrExit"].ToString());
            }
            if (dr["ChannelNo"] != null)
            {
                model.ChannelNo = dr["ChannelNo"].ToString();
            }
            if (dr["Library"] != null)
            {
                model.LibraryNo = dr["Library"].ToString();
            }
            if (dr["Flag"] != null)
            {
                model.Flag = int.Parse(dr["Flag"].ToString());
            }
            if (dr["ID"] != null)
            {
                model.ID = int.Parse(dr["ID"].ToString());
            }
            if (dr["HandleFlag"] != null)
            {
                model.HandleFlag = dr["HandleFlag"].ToString() == "1" ? true : false;
            }

            return model;
        }

    }
}
