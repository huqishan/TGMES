using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Infrastructure.PackMethod
{
    public static class WebApiHelper
    {
        static RestClient _Client;
        //application/json
        public static string? Send(string data, string url)
        {
            if (_Client == null)
                _Client = new RestClient(url);
            var request = new RestRequest();
            request.Method = Method.Post;
            if (!string.IsNullOrEmpty(data))
                request.AddBody(data);
            var result = _Client.Execute(request);
            if (!string.IsNullOrEmpty(result.ErrorException?.Message))
                return result.ErrorException.Message;
            return result.Content;
        }
    }
}
