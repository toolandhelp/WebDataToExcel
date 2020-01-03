using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;


namespace WebDataToExcel
{
    public static class Util
    {
        private static string _cookie = "__utmc=124945049; UM_distinctid=16f55c2f36866c-0dcee694ce015d-6701b35-1fa400-16f55c2f3692f1; __auc=8794f96c16f55c436cb9eef4daa; route=efb99d11addba321a4d5b6549aabeb4a; BIOSESSIONID=29DAA632FB8E5599469DB59C65B69D9A-n1; dxy_da_cookie-id=89cffcff71e6005d4ed4ccebc8d2b8431577935194041; Hm_lvt_280a4cb2cd67890fa8c564956e88c914=1577691837,1577691920,1577935179,1577945947; Hm_lpvt_280a4cb2cd67890fa8c564956e88c914=1577945947; CNZZDATA1275464573=1014816051-1577691030-https%253A%252F%252Fwww.biomart.cn%252F%7C1577941847; __utma=124945049.751300685.1577691837.1577935179.1577945947.4; __utmz=124945049.1577945947.4.3.utmcsr=auth.dxy.cn|utmccn=(referral)|utmcmd=referral|utmcct=/; CNZZDATA1275462536=1560480999-1577688652-https%253A%252F%252Fwww.biomart.cn%252F%7C1577944905; __utmt=1; __utmb=124945049.5.9.1577945947";
        private static string _userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.88 Safari/537.36";



        public static HttpWebResponse WebRequestConstruct(string url, string method = "GET", string json = null)
        {
            //if (this.wb.Document != null && this.wb.Document.Cookie != null)
            //{
            //    _cookie = this.wb.Document.Cookie;
            //    _userAgent = thisGetDefaultUserAgent(thi.wb);
            //}

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Connection = "application/json;charset=UTF-8";
            httpWebRequest.Method = method;

            httpWebRequest.UserAgent = _userAgent;

            httpWebRequest.Headers.Add("Cookie", _cookie);

            //if (!string.IsNullOrEmpty(json))
            //{

            //    httpWebRequest.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            //    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            //    {
            //        //string json = "{\"user\":\"test\"," +
            //        //              "\"password\":\"bla\"}";
            //        streamWriter.Write(json);
            //        streamWriter.Flush();
            //        streamWriter.Close();
            //    }
            //}

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            return httpResponse;
        }

        //body是要传递的参数,格式"roleId=1&uid=2"
        //post的cotentType填写:
        //"application/x-www-form-urlencoded"
        //soap填写:"text/xml; charset=utf-8"
        public static string PostHttp(string url, string body, string contentType= "application/x-www-form-urlencoded; charset=UTF-8")
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

            httpWebRequest.ContentType = contentType;
            httpWebRequest.Method = "POST";
            httpWebRequest.Timeout = 20000;
            //CookieContainer cc = new CookieContainer();
            //cc.Add(new Uri(httpWebRequest.Host), new Cookie("", _cookie));
            //httpWebRequest.CookieContainer = cc;

            httpWebRequest.Headers.Add("Cookie", _cookie); 
          
            byte[] btBodys = Encoding.UTF8.GetBytes(body);
            httpWebRequest.ContentLength = btBodys.Length;
            httpWebRequest.GetRequestStream().Write(btBodys, 0, btBodys.Length);

            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
            {
                string responseContent = streamReader.ReadToEnd();

                httpWebResponse.Close();
                streamReader.Close();
                httpWebRequest.Abort();
                httpWebResponse.Close();

                return responseContent;
            }

          
        }
        public static string GetHttp(string url, HttpContext httpContext)
        {
            string queryString = "?";

            foreach (string key in httpContext.Request.QueryString.AllKeys)
            {
                queryString += key + "=" + httpContext.Request.QueryString[key] + "&";
            }

            queryString = queryString.Substring(0, queryString.Length - 1);

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url + queryString);

            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "GET";
            httpWebRequest.Timeout = 20000;

            //byte[] btBodys = Encoding.UTF8.GetBytes(body);
            //httpWebRequest.ContentLength = btBodys.Length;
            //httpWebRequest.GetRequestStream().Write(btBodys, 0, btBodys.Length);

            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
            string responseContent = streamReader.ReadToEnd();

            httpWebResponse.Close();
            streamReader.Close();

            return responseContent;
        }


        /// <summary>
        /// 通过 WebRequest/WebResponse 类访问远程地址并返回结果，需要Basic认证；
        /// 调用端自己处理异常
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="timeout">访问超时时间，单位毫秒；如果不设置超时时间，传入0</param>
        /// <param name="encoding">如果不知道具体的编码，传入null</param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string Request_WebRequest(string uri, int timeout, Encoding encoding, string username, string password)
        {
            string result = string.Empty;

            WebRequest request = WebRequest.Create(new Uri(uri));

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                request.Credentials = GetCredentialCache(uri, username, password);
                request.Headers.Add("Authorization", GetAuthorization(username, password));
            }

            if (timeout > 0)
                request.Timeout = timeout;

            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader sr = encoding == null ? new StreamReader(stream) : new StreamReader(stream, encoding);

            result = sr.ReadToEnd();

            sr.Close();
            stream.Close();

            return result;
        }

        #region # 生成 Http Basic 访问凭证 #

        private static CredentialCache GetCredentialCache(string uri, string username, string password)
        {
            string authorization = string.Format("{0}:{1}", username, password);

            CredentialCache credCache = new CredentialCache();
            credCache.Add(new Uri(uri), "Basic", new NetworkCredential(username, password));

            return credCache;
        }

        private static string GetAuthorization(string username, string password)
        {
            string authorization = string.Format("{0}:{1}", username, password);

            return "Basic " + Convert.ToBase64String(new ASCIIEncoding().GetBytes(authorization));
        }

        #endregion

        /// <summary>
        /// 一个很BT的获取IE默认UserAgent的方法
        /// </summary>
        public static string GetDefaultUserAgent(this WebBrowser wb)
        {
            wb.Navigate("about: blank");
            while (wb.IsBusy) Application.DoEvents();
            object window = wb.Document.Window.DomWindow;
            Type wt = window.GetType();
            object navigator = wt.InvokeMember("navigator", BindingFlags.GetProperty,
                null, window, new object[] { });
            Type nt = navigator.GetType();
            object userAgent = nt.InvokeMember("userAgent", BindingFlags.GetProperty,
                null, navigator, new object[] { });
            return userAgent.ToString();
        }
    }
}
