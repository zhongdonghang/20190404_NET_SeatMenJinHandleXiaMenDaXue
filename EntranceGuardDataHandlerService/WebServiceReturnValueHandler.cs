using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace EntranceGuardDataHandlerService
{
    public class WebServiceReturnValueHandler
    {
        public static HandleResultModel EnterLibHandle(string xmlValue)
        {
            try
            {
                HandleResultModel model = new HandleResultModel();
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlValue);
                XmlNode node = xmlDoc.SelectSingleNode("//ReaderInfo/ReaderNo");
                model.CardNo = node.InnerText;
                node = xmlDoc.SelectSingleNode("//ReaderInfo/NowStatus");
                model.NowStatus = node.InnerText;
                node = xmlDoc.SelectSingleNode("//ReaderInfo/BeforeStatus");
                model.BeforeStatus = node.InnerText;
                node = xmlDoc.SelectSingleNode("//ReaderInfo/Error");
                model.Error = node.InnerText;
                return model;
            }
            catch (Exception EX)
            {
                throw new Exception("解析结果失败，可能是接口程序返回了一个不正常的结果。错误信息为："+EX.Message);
            }
            //    <ReaderInfo>
            //<ReaderNo>{0}</ReaderNo>  --读者学号
            //<ReaderName>{1}</ReaderName> --读者姓名
            //<NowStatus>{2}</NowStatus>  --处理后（当前）的读者状态（详见最后附录）
            //<BeforeStatus>{3}</BeforeStatus> --处理前的状态（详见最后附录）
            //<Error>{4}</Error> --错误信息（正常为空值）
            //</ReaderInfo>
        }
    }

    /// <summary>
    /// 处理结果判断
    /// </summary>
    public class HandleResultModel
    {
        private string cardNo;
        /// <summary>
        /// 学号
        /// </summary>
        public string CardNo
        {
            get { return cardNo; }
            set { cardNo = value; }
        }
        private string readerName;
        /// <summary>
        /// 姓名
        /// </summary>
        public string ReaderName
        {
            get { return readerName; }
            set { readerName = value; }
        }
        private string nowStatus;
        /// <summary>
        /// 当前状态
        /// </summary>
        public string NowStatus
        {
            get { return nowStatus; }
            set { nowStatus = value; }
        }
        private string beforeStatus;
        /// <summary>
        /// 之前状态
        /// </summary>
        public string BeforeStatus
        {
            get { return beforeStatus; }
            set { beforeStatus = value; }
        }
        private string error;
        //错误信息
        public string Error
        {
            get { return error; }
            set { error = value; }
        }
        
    }
}
