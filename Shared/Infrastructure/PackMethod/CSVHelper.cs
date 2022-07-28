using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Infrastructure.PackMethod
{
    public static class CSVHelper
    {
        public static void AddCSV(CSV_MODEL _MODEL, int upType)
        {
            string logFilePath = @"D:\MESLog";
            switch (upType)
            {
                case 1:
                    logFilePath += @"\托盘效验";
                    break;
                case 2:
                    logFilePath += @"\条码校验";
                    break;
                case 3:
                    logFilePath += @"\电芯与托盘绑定";
                    break;
                case 4:
                    logFilePath += @"\托盘解绑";
                    break;
                case 5:
                    logFilePath += @"\产品加工参数收集";
                    break;
                case 6:
                    logFilePath += @"\员工效验";
                    break;
                case 7:
                    logFilePath += @"\产品出站";
                    break;
                case 8:
                    logFilePath += @"\设备状态";
                    break;

            }
            if (!Directory.Exists(logFilePath))
                Directory.CreateDirectory(logFilePath);

            logFilePath += "/" + DateTime.Now.ToString("yyyyMMdd") + ".CSV";
            bool isExists = File.Exists(logFilePath);
            using (FileStream fs = new FileStream(logFilePath, FileMode.Append))
            {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                {
                    if (!isExists)
                        sw.WriteLine("工厂,设备,产品条码,生产类型,开始调用时间,上传参数信息," +
                            "接收MES返回信息时间,MES返回success,MES返回message,是否上传MES");
                    sw.WriteLine($"{_MODEL.SiteCode},{_MODEL.EquipNum},{_MODEL.Identification}," +
                        $"{_MODEL.ProductType},{_MODEL.StartUpTime},{_MODEL.UpData},{_MODEL.ReturnTime}," +
                        $"{_MODEL.Success},{_MODEL.ReturnMessage},{_MODEL.IsUpMES}");
                }
            }

        }
    }
    public class CSV_MODEL
    {
        /// <summary>
        /// 工厂
        /// </summary>
        public string SiteCode { get; set; }
        /// <summary>
        /// 设备编号
        /// </summary>
        public string EquipNum { get; set; }
        /// <summary>
        /// 生产类型
        /// </summary>
        public string ProductType { get; set; }
        /// <summary>
        /// 产品条码
        /// </summary>
        public string Identification { get; set; }
        /// <summary>
        /// 开始上传时间
        /// </summary>
        public string StartUpTime { get; set; }
        /// <summary>
        /// 上传内容
        /// </summary>
        public string UpData { get; set; }
        /// <summary>
        /// MES返回信息时间
        /// </summary>
        public string ReturnTime { get; set; }
        /// <summary>
        /// MES返回状态
        /// </summary>
        public string Success { get; set; }
        /// <summary>
        /// MES返回信息
        /// </summary>
        public string ReturnMessage { get; set; }
        /// <summary>
        /// 是否上传MES
        /// </summary>
        public string IsUpMES => "Y";
        /// <summary>
        /// 电池码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// MES结果
        /// </summary>
        public bool MES { get; set; }


    }
}
