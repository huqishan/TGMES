using System.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace Shared.Infrastructure.PackMethod
{
    public static class WebServicesHelper
    {
        private static HttpWebRequest _Client = null;

        public static string Send(string data, string url, string contentType = "text/xml; charset=utf-8")
        {
            byte[] bSend = Encoding.UTF8.GetBytes(data.ToString());
            if (_Client == null)
            {
                _Client = HttpWebRequest.Create(url) as HttpWebRequest;
            }
            _Client.Method = "POST";
            _Client.ContentType = contentType;
            _Client.ContentLength = bSend.Length;
            Stream requestStram = _Client.GetRequestStream();
            requestStram.Write(bSend, 0, bSend.Length);
            requestStram.Close();
            //获取返回数据
            WebResponse wr = _Client.GetResponse();
            Stream getStream = wr.GetResponseStream();
            XmlTextReader Reader = new XmlTextReader(getStream);
            Reader.MoveToContent();
            var txt = Reader.ReadInnerXml();
            _Client.Abort();
            return txt;
        }
    }
}
