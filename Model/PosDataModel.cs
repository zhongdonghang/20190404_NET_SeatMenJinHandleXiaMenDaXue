using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class PosDataModel
    {
        private string id;
        /// <summary>
        /// 记录序列号从1开始递增，
        /// </summary>
        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        private string cardNo;
        /// <summary>
        /// 学工号
        /// </summary>
        public string CardNo
        {
            get { return cardNo; }
            set { cardNo = value; }
        } 
        private string passed;
        /// <summary>
        /// 中间表未使用字段，也可用作处理标识。
        /// </summary>
        public string Passed
        {
            get { return passed; }
            set { passed = value; }
        }
        private DateTime visitTime;
        /// <summary>
        /// 刷卡时间
        /// </summary>
        public DateTime VisitTime
        {
            get { return visitTime; }
            set { visitTime = value; }
        }
        private string gateno;
        /// <summary>
        /// 闸机号
        /// </summary>
        public string Gateno
        {
            get { return gateno; }
            set { gateno = value; }
        }
        private int direction;
        /// <summary>
        /// 刷卡方向（1进2出）
        /// </summary>
        public int Direction
        {
            get { return direction; }
            set { direction = value; }
        }
        private string handleFlag;
        /// <summary>
        /// 处理标识（1 已处理，其他值 未处理）
        /// </summary>
        public string HandleFlag
        {
            get { return handleFlag; }
            set { handleFlag = value; }
        }
    }
}
