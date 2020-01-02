using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace WebDataToExcel
{
    public static class Util
    {
        private static string _cookie = "__utmc=124945049; UM_distinctid=16f55c2f36866c-0dcee694ce015d-6701b35-1fa400-16f55c2f3692f1; __auc=8794f96c16f55c436cb9eef4daa; route=efb99d11addba321a4d5b6549aabeb4a; BIOSESSIONID=29DAA632FB8E5599469DB59C65B69D9A-n1; dxy_da_cookie-id=89cffcff71e6005d4ed4ccebc8d2b8431577935194041; Hm_lvt_280a4cb2cd67890fa8c564956e88c914=1577691837,1577691920,1577935179,1577945947; Hm_lpvt_280a4cb2cd67890fa8c564956e88c914=1577945947; CNZZDATA1275464573=1014816051-1577691030-https%253A%252F%252Fwww.biomart.cn%252F%7C1577941847; __utma=124945049.751300685.1577691837.1577935179.1577945947.4; __utmz=124945049.1577945947.4.3.utmcsr=auth.dxy.cn|utmccn=(referral)|utmcmd=referral|utmcct=/; CNZZDATA1275462536=1560480999-1577688652-https%253A%252F%252Fwww.biomart.cn%252F%7C1577944905; __utmt=1; __utmb=124945049.5.9.1577945947";
        private static string _userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.88 Safari/537.36";



        public static HttpWebResponse WebRequestConstruct(string url)
        {
            //if (this.wb.Document != null && this.wb.Document.Cookie != null)
            //{
            //    _cookie = this.wb.Document.Cookie;
            //    _userAgent = thisGetDefaultUserAgent(thi.wb);
            //}

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Connection = "application/json;charset=UTF-8";
            httpWebRequest.Method = "GET";

            httpWebRequest.UserAgent = _userAgent;

            httpWebRequest.Headers.Add("Cookie", _cookie);


            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            return httpResponse;
        }


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
