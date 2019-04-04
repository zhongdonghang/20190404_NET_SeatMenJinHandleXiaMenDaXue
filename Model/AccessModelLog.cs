using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class AccessModelLog
    {
        private int _ID = -1;
        /// <summary>
        /// ID
        /// </summary>
        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        private int _VisitID = -1;
        /// <summary>
        /// 门禁记录ID
        /// </summary>
        public int VisitID
        {
            get { return _VisitID; }
            set { _VisitID = value; }
        }

        private string _CardNo = "";
        /// <summary>
        /// 读者学号
        /// </summary>
        public string CardNo
        {
            get { return _CardNo; }
            set { _CardNo = value; }
        }

        private string _CardID = "";
        /// <summary>
        /// 读者卡号
        /// </summary>
        public string CardID
        {
            get { return _CardID; }
            set { _CardID = value; }
        }

        private DateTime _VisitTime = DateTime.Parse("2015-1-1");
        /// <summary>
        /// 访问时间
        /// </summary>
        public DateTime VisitTime
        {
            get { return _VisitTime; }
            set { _VisitTime = value; }
        }
        private string _VisiterName = "";
        /// <summary>
        /// 访问者姓名
        /// </summary>
        public string VisiterName
        {
            get { return _VisiterName; }
            set { _VisiterName = value; }
        }

        private string _LibraryNo = "";
        /// <summary>
        /// 图使馆编号
        /// </summary>
        public string LibraryNo
        {
            get { return _LibraryNo; }
            set { _LibraryNo = value; }
        }

        private EnterState _Enter = EnterState.None;
        /// <summary>
        /// 进出状态
        /// </summary>
        public EnterState Enter
        {
            get { return _Enter; }
            set { _Enter = value; }
        }
        private string _ChannelNo = "";
        /// <summary>
        /// 通道机编号
        /// </summary>
        public string ChannelNo
        {
            get { return _ChannelNo; }
            set { _ChannelNo = value; }
        }
        private int _Flag = -1;
        /// <summary>
        /// 标示符
        /// </summary>
        public int Flag
        {
            get { return _Flag; }
            set { _Flag = value; }
        }
        private bool _HandleFlag = false;
        /// <summary>
        /// 记录是否已经处理
        /// </summary>
        public bool HandleFlag
        {
            get { return _HandleFlag; }
            set { _HandleFlag = value; }
        }
    }

    public enum EnterState
    {
        /// <summary>
        /// 空值
        /// </summary>
        None=-1,
        /// <summary>
        /// 进入
        /// </summary>
        Enter=1,
        /// <summary>
        /// 离开
        /// </summary>
        Exit=2,
    }
}
