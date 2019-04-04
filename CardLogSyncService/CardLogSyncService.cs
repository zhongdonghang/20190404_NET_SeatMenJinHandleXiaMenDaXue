using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace CardLogSyncService
{
    /// <summary>
    /// 定时从门禁数据库获取刷卡记录
    /// </summary>
    public class CardLogSyncService : IService.IService
    {
        private System.Timers.Timer timer = null;
        private AccessSync accessSync = null;

        string intervalTime = ConfigurationManager.AppSettings["GetDataInterval"];
        string syncName = ConfigurationManager.AppSettings["SyncName"];

        public CardLogSyncService()
        {
            accessSync = new AccessSync();
            accessSync.SyncProgress += accessSync_SyncProgress;
            timer = new System.Timers.Timer(int.Parse(intervalTime) * 1000);
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            timer.Enabled = true;
            timer.Start();

          
        }

        void accessSync_SyncProgress(string message)
        {
            Console.WriteLine(message);
            //this.Invoke(new Action(() =>
            //{
            //    Console.WriteLine(message);

            //}));
        }

        //private void Invoke(Action action)
        //{
        //    throw new NotImplementedException();
        //}

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            timer.Stop();
            try
            {
                accessSync.Sync();
            }
            catch (Exception ex)
            {
                SeatManage.SeatManageComm.WriteLog.Write(ex.Message);
            }
            finally
            {
                timer.Start();
            }
        }


        public void Start()
        {
            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
        }

        public void Dispose()
        {
            timer.Stop();
        }


        public override string ToString()
        {
            return "门禁数据同步获取程序";
        }
    }
}
