using System;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace blockchainApp
{
    public static class ReqestService
    {
        public static T Get<T>(string address, string action)
        {
            var url = $"http://{address}/{action}";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = WebRequestMethods.Http.Get;
            request.ContentType = "application/json; charset=utf-8";

            Stream stream = request.GetResponse().GetResponseStream();
            StreamReader reader = new StreamReader(stream);

            var requestModel = JsonConvert.DeserializeObject<T>(reader.ReadLine());
            stream.Close();

            return requestModel;
        }

        public static T Post<T>(string address, string value)
        {
            using (var client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                client.Encoding = Encoding.UTF8;

                var url = $"http://{address}?{value}";

                var result = client.UploadString(url, "POST");

                return JsonConvert.DeserializeObject<T>(result);
            }
        }
    }
}