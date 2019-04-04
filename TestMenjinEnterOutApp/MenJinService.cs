using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMenjinEnterOutApp
{
    public class MenJinService
    {
        private  System.Timers.Timer timer = null;

         public   MenJinService()
        {
            Console.WriteLine("模拟门禁进出程序启动");
            timer = new System.Timers.Timer(2000);
            timer.Elapsed += new System.Timers.ElapsedEventHandler(Timer_Elapsed);
            timer.Start();
        }

         void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                timer.Stop();
                Enter();
                System.Threading.Thread.Sleep(1000 * 60 * 2);
                Out();
                System.Threading.Thread.Sleep(1000 * 60 * 1);
            }
            catch (Exception ex)
            {
                SeatManage.SeatManageComm.WriteLog.Write(string.Format("执行遇到异常：{0}", ex.Message));
            }
            finally
            {
                timer.Start();
            }
        }

         void Out()
        {
            string sql = "INSERT INTO [menjindata].[dbo].[mjdatatoday]"+
          "  ([F2]"+
          "   ,[F3]" +
          "   ,[F4]" +
          "   ,[F5]" +
           "  ,[F6]" +
          "   ,[F7]" +
          "   ,[F8]" +
          "   ,[F9]" +
          "   ,[F10]" +
          "   ,[F11])" +
    "   VALUES" +
          "   ('1521110052'" +
        "     , '湘宁'" +
        "     , '本科生'" +
        "     , '经济学院'" +
         "    , 0" +
         "    , GETDATE()" +
         "    , '53'" +
         "    , 2" +
         "    , DATEADD(MI, 3, GETDATE())" +
         "    , '0')";
           // DBUtility.DbHelperSQL.connectionString = System.Configuration.ConfigurationManager.AppSettings[""];
            DBUtility.DbHelperSQL.ExecuteSql(sql);
            Console.WriteLine("有个人出来了");
        }

         void Enter()
        {
            string sql = "INSERT INTO [menjindata].[dbo].[mjdatatoday]" +
 "  ([F2]" +
 "   ,[F3]" +
 "   ,[F4]" +
 "   ,[F5]" +
  "  ,[F6]" +
 "   ,[F7]" +
 "   ,[F8]" +
 "   ,[F9]" +
 "   ,[F10]" +
 "   ,[F11])" +
"   VALUES" +
 "   ('1521110052'" +
"     , '湘宁'" +
"     , '本科生'" +
"     , '经济学院'" +
"    , 0" +
"    , GETDATE()" +
"    , '04'" +
"    , 1" +
"    , DATEADD(MI, 3, GETDATE())" +
"    , '0')";
            DBUtility.DbHelperSQL.ExecuteSql(sql);
            Console.WriteLine("有个人进去了");
        }
    }
}
