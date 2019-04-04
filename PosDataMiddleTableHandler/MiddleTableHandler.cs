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
    public class MiddleTableHandler
    {

        /// <summary>
        /// 删除指定日期之前的数据
        /// </summary>
        /// <param name="beforeDate">指定的日期</param> 
        /// <returns></returns>
        public static bool Delete(DateTime beforeDate)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(" delete shuakajilu where datediff(day,VisitTime,'{0}')>=0", beforeDate);
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
        /// 获取还没处理的刷卡数据
        /// </summary>
        /// <returns></returns>
        public static List<AccessModelLog> GetModels(int effectiveTime)
        {
            List<AccessModelLog> posDataList = new List<AccessModelLog>();
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(string.Format("select VisitNo,CardId,Passed,VisitTime,direction,HandleFlag from shuakajilu where  datediff(mi,visitTime,getdate())<{0} and HandleFlag<>1 or HandleFlag is Null",effectiveTime));
            try
            {
                DataSet ds = DBUtility.DbHelperSQL.Query(strSql.ToString());
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    AccessModelLog model = dataRowToModel(ds.Tables[0].Rows[i]);
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
        public static bool Update(PosDataModel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("update shuakajilu set HandleFlag=1 where VisitNo={0} ", model.Id);
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
        public static bool UpdateAll( )
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update shuakajilu set HandleFlag=1 where HandleFlag=0 " );
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
        private static PosDataModel dataRowToModel(DataRow dr)
        {
            if (dr != null)
            {
                try
                {
                    PosDataModel model = new PosDataModel();
                    model.Id = dr["VisitNo"].ToString();
                    model.CardNo = dr["CardId"].ToString().Trim();
                    model.Passed = dr["Passed"].ToString().Trim();
                    string tempTime = dr["VisitTime"].ToString().Trim();
                    if (!string.IsNullOrEmpty(tempTime))
                    {
                        model.VisitTime = DateTime.Parse(dr["VisitTime"].ToString());
                    }
                    else
                    {
                        model.VisitTime =DateTime.Parse( "1900-1-1");
                    }
                    string tempDirection = dr["direction"].ToString();
                    if (!string.IsNullOrEmpty(tempDirection))
                    {
                        model.Direction = int.Parse(dr["direction"].ToString());
                    }
                    else
                    {
                        model.Direction = -1;
                    }
                    model.HandleFlag = dr["HandleFlag"].ToString();
                    return model;
                }
                catch (Exception ex)
                {
                    SeatManage.SeatManageComm.WriteLog.Write(string.Format("解析刷卡数据遇到异常，该条数据被忽略。数据Id:{0}  异常原因：{1}", dr["VisitNo"].ToString(), ex.Message));
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

    }
}
