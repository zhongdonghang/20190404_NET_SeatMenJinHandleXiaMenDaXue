using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using PosDataMiddleTableHandler;
using Model;

namespace EntranceGuardDataHandlerService
{
    public class EntranceGuardDataHandlerService : IService.IService
    {

        /// <summary>
        /// 消息通知
        /// </summary>
        /// <param name="message"></param>
        public delegate void EventHandleSync(string message);

        /// <summary>
        /// 处理事件
        /// </summary>
        public event EventHandleSync PostProgress;

        private System.Timers.Timer timer = null;
        /// <summary>
        /// 刷卡记录有效时间。
        /// </summary>
        private string validMinutes = ConfigurationManager.AppSettings["validMinutes"];
        /// <summary>
        /// 日志保留天数
        /// </summary>
        private string deleteLogDays = ConfigurationManager.AppSettings["delLogDays"];
        /// <summary>
        /// 处理时间
        /// </summary>
        private string delHandleTime = ConfigurationManager.AppSettings["delHandleTime"];
        /// <summary>
        /// 数据处理地址
        /// </summary>
        private string dataHandlerUrl = ConfigurationManager.AppSettings["dataHandlerUrl"];
        EntranceGuardInterface.SM_AccessInterface eg = new EntranceGuardInterface.SM_AccessInterface();
        public EntranceGuardDataHandlerService()
        {
            this.PostProgress += EntranceGuardDataHandlerService_PostProgress; ;


            eg.Url = dataHandlerUrl;
            timer = new System.Timers.Timer(2000);
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
        }

        private void EntranceGuardDataHandlerService_PostProgress(string message)
        {
            Console.WriteLine(message);
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                timer.Stop();
                //处理门禁的刷卡记录
                HandPosData();

                //删除过期的门禁刷卡记录
                DeletePosData();
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

        private void StartHandle()
        {
            //把所有记录标识为已处理。
            try
            {
                PosDataMiddleTableHandler.MiddleTableHandler_New.UpdateAll();
                SeatManage.SeatManageComm.WriteLog.Write(string.Format("重启时把所有记录标识为已处理"));
                PostProgress("重启时把所有记录标识为已处理");
            }
            catch (Exception ex)
            {
                SeatManage.SeatManageComm.WriteLog.Write(string.Format("重启时把所有记录标识为已处理失败:{0}", ex.Message));
                PostProgress(string.Format("重启时把所有记录标识为已处理失败:{0}", ex.Message));
            }
        }
        /// <summary>
        /// 当天 删除操作是否执行
        /// </summary>
        bool isDel = false;
        /// <summary>
        /// 删除刷卡记录
        /// </summary>
        private void DeletePosData()
        {
            try
            {
                DateTime handleTime = DateTime.Parse(DateTime.Now.ToShortDateString() + " " + delHandleTime);
                if (DateTime.Now.CompareTo(handleTime) >= 0 && !isDel)
                {
                    int days = 0;
                    if (!string.IsNullOrEmpty(deleteLogDays))
                    {
                        days = int.Parse(deleteLogDays);
                    }
                    DateTime dt = DateTime.Now.AddDays(-days);
                    PosDataMiddleTableHandler.MiddleTableHandler_New.Delete(dt);
                    isDel = true;
                    PostProgress(string.Format("删除{0}之前的记录", dt));
                    SeatManage.SeatManageComm.WriteLog.Write(string.Format("删除{0}之前的记录", dt));
                }
                else if (DateTime.Now.CompareTo(handleTime) < 0)
                {
                    isDel = false;
                }
            }
            catch (Exception ex)
            {
                SeatManage.SeatManageComm.WriteLog.Write(string.Format("删除遇到异常:{0}", ex.Message));
                PostProgress(string.Format("删除遇到异常:{0}", ex.Message));
            }
        }
        /// <summary>
        /// 刷卡数据处理函数
        /// </summary>
        private void HandPosData()
        {
            int ivalidMinutes = -1;
            if (!string.IsNullOrEmpty(validMinutes))
            {
                ivalidMinutes = int.Parse(validMinutes);
            }
            else
            {
                ivalidMinutes = int.MaxValue;
            }


            List<AccessModelLog> posDataModels = PosDataMiddleTableHandler.MiddleTableHandler_New.GetModels(ivalidMinutes);//获取还没处理的刷卡数据,参数表示有效时间（分钟）
            PostProgress("新进馆"+ posDataModels.Count+"人,开始处理");
            for (int i = 0; i < posDataModels.Count; i++)//遍历获取到的刷卡数据列表
            {
                AccessModelLog model = posDataModels[i];
                TimeSpan ts = DateTime.Now - model.VisitTime;

                //如果读者学号不存在，跳过，不做处理
                if (string.IsNullOrEmpty(model.CardNo)) continue;

                //判断是否处理，是否为入馆通道，有效时间。
                if (!model.HandleFlag && ts.TotalMinutes < ivalidMinutes)
                {
                    string strResult = "";
                    try
                    {//调用通道机的处理接口，并记录返回结果
                        if (model.Enter == EnterState.Enter)
                        {
                            strResult = eg.EnterLib(model.CardNo);
                            SeatManage.SeatManageComm.WriteLog.Write("处理读者" + model.CardNo + "进馆处理");
                            PostProgress("处理读者" + model.CardNo + "进馆处理");
                        }
                        else
                        {
                            strResult = eg.OutLib(model.CardNo);
                            SeatManage.SeatManageComm.WriteLog.Write("处理读者" + model.CardNo + "出馆处理");
                            PostProgress("处理读者" + model.CardNo + "出馆处理");
                        }
                    }
                    catch (Exception ex)
                    {
                        SeatManage.SeatManageComm.WriteLog.Write(string.Format("处理失败，记录Id:{0}  错误信息:{1}", model.ID, ex.ToString()));
                        PostProgress(string.Format("处理失败，记录Id:{0}  错误信息:{1}", model.ID, ex.Message));
                        break;
                    }
                    try
                    {
                        //解析返回结果
                        HandleResultModel result = WebServiceReturnValueHandler.EnterLibHandle(strResult);
                        if (!string.IsNullOrEmpty(result.Error))
                        {//如果不为空，则说明处理错误
                            SeatManage.SeatManageComm.WriteLog.Write(string.Format("处理错误，记录Id:{0}  错误信息:{1}", model.ID, result.Error));
                            PostProgress(string.Format("处理错误，记录Id:{0}  错误信息:{1}", model.ID, result.Error));
                        }
                        else
                        {   //否则标识已经处理完成。
                            PosDataMiddleTableHandler.MiddleTableHandler_New.Update(model);
                        }
                    }
                    catch (Exception ex)
                    {
                        SeatManage.SeatManageComm.WriteLog.Write(ex.Message);
                        PostProgress(ex.ToString());
                        break;
                    }

                }
            }
        }


        public override string ToString()
        {
            return "门禁刷卡数据处理程序";
        }

        public void Start()
        {
            //启动之前先处理未刷卡的记录。

            StartHandle();

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
    }
}
