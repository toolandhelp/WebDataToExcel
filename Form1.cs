using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace WebDataToExcel
{
    public partial class Form1 : Form
    {
        private bool isLoadOk = false;
        private BackgroundWorker bgWorker;

        private delegate void DelegateTJ(string times);
        private delegate void DelegateGridViewDataBand(string datas);
        public Form1()
        {
            InitializeComponent();
        }

     //   private  WebKit.WebKitBrowser browser = new WebKit.WebKitBrowser();

        //private WebBrowser webBrowser = new WebBrowser();

        private string _cookie = "__utmc=124945049; UM_distinctid=16f55c2f36866c-0dcee694ce015d-6701b35-1fa400-16f55c2f3692f1; __auc=8794f96c16f55c436cb9eef4daa; route=efb99d11addba321a4d5b6549aabeb4a; BIOSESSIONID=29DAA632FB8E5599469DB59C65B69D9A-n1; dxy_da_cookie-id=89cffcff71e6005d4ed4ccebc8d2b8431577935194041; Hm_lvt_280a4cb2cd67890fa8c564956e88c914=1577691837,1577691920,1577935179,1577945947; Hm_lpvt_280a4cb2cd67890fa8c564956e88c914=1577945947; CNZZDATA1275464573=1014816051-1577691030-https%253A%252F%252Fwww.biomart.cn%252F%7C1577941847; __utma=124945049.751300685.1577691837.1577935179.1577945947.4; __utmz=124945049.1577945947.4.3.utmcsr=auth.dxy.cn|utmccn=(referral)|utmcmd=referral|utmcct=/; CNZZDATA1275462536=1560480999-1577688652-https%253A%252F%252Fwww.biomart.cn%252F%7C1577944905; __utmt=1; __utmb=124945049.5.9.1577945947";


        private int _totalCount = 1468, pageSize = 15, pageCount = 98, tjcount = 0;
       //   private dataGridView1 _dataGV;


        private void Form_Load(object sender, EventArgs e)
        {
            string url = "https://auth.dxy.cn/accounts/login?service=https%3A%2F%2Fwww.biomart.cn%2Fj_acegi_cas_security_check&qr=false&logoDqId=21327&method=1";
            WbLoad(url);
        }

        private void WbLoad(string url)
        {
            bgWorker = new BackgroundWorker();
            bgWorker.DoWork += bgWorker_DoWork;
            bgWorker.RunWorkerCompleted += bgWorker_RunWorkerCompleted;


            this.wb.ScriptErrorsSuppressed = true;
            this.wb.Navigate(url);


            //string lst = "j_inquiry_lst";
           // System.Windows.Forms.HtmlDocument doc = this.wb.Document;
            //if (doc != null)
            //{
            //    HtmlElement search = doc.GetElementById(lst);
            //}

            //string btn_id = "btn_s";

            //HtmlElement btn = doc.GetElementById(btn_id);
            //btn.InvokeMember("click");


            CookieContainer myCookieContainer = new CookieContainer();
            if (wb.Document != null && wb.Document.Cookie != null)
            {
               
                _cookie = this.wb.Document.Cookie;
                //string cookieStr = wb.Document.Cookie;
                //string[] cookstr = cookieStr.Split(';');
                //foreach (string str in cookstr)
                //{
                //    //string[] cookieNameValue = str.Split('=');
                //    //Cookie ck = new Cookie(cookieNameValue[0].Trim().ToString(), cookieNameValue[1].Trim().ToString());
                //    //ck.Domain = "www.google.com";
                //    //myCookieContainer.Add(ck);
                //}
                this.wb.Visible = false;
                MessageBox.Show("登录成功!,请根据按钮可点击操作，进行下一步操作");
            }


            bgWorker.RunWorkerAsync();

           this.btn_jx.Enabled = true;
          
        }

        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            CompWait();
        }
        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.wb.Document.Window.Error += new HtmlElementErrorEventHandler(Window_Error);
            if (this.wb.ReadyState == WebBrowserReadyState.Complete)
            {
                isLoadOk = true;
                this.btn_jx.Enabled = true;
            }
            else
            {
                isLoadOk = false;
            }
        }

        private void Window_Error(object sender, HtmlElementErrorEventArgs e)
        {
            e.Handled = true;
        }

        private void CompWait()
        {
            while (!isLoadOk)
            {
                Thread.Sleep(500);
            }
        }

        private void btn_dc_Click(object sender, EventArgs e)
        {

            try
            {
                ExcelHelper.ExportToExcel(ExcelHelper.ToDataTable(this.dataGridView1));
                //MessageBox.Show("导出完成");
            }
            catch (Exception ex)
            {
                MessageBox.Show("导出失败");
            }

        }

        private void btn_jx_Click(object sender, EventArgs e)
        {
            AjaxTest();
            this.btn_jx.Enabled = false;
            this.btn_dc.Enabled = true;
        }


        #region 注释

        private void JieXiXQHTML(string htmlURL, ref Infos info)
        {
            //Task task = Task.Run(() =>
            //{
            HtmlWeb webClient = new HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = webClient.Load(htmlURL);
            wb.Navigate(htmlURL);

            

           HtmlNode rootNode = doc.DocumentNode.SelectSingleNode("//*[@id='j_inquiry_detail']/div[2]/div[1]/div[3]");

            HtmlAgilityPack.HtmlDocument tbody = new HtmlAgilityPack.HtmlDocument();
            tbody.LoadHtml(rootNode.InnerText);

            tbody.OptionOutputAsXml = true;


            HtmlNodeCollection dlList = rootNode.SelectNodes(".//dl");


            foreach (var item in dlList)
            {
                // var daa  = item.SelectSingleNode("/dd[1]").InnerHtml.Trim();
                info.dh = item.SelectSingleNode("/dd[2]").InnerHtml.Trim();
                info.yx = item.SelectSingleNode("/dd[3]").InnerHtml.Trim();
                info.dw = item.SelectSingleNode("/dd[4]").InnerHtml.Trim();
                info.zw = item.SelectSingleNode("/dd[5]").InnerHtml.Trim();
                info.xxdz = item.SelectSingleNode("/dd[6]").InnerHtml.Trim();
            }


            //});




        }


        private void JieXiHTML(string htmlURL)
        {
            HtmlWeb webClient = new HtmlWeb();

            //var wbDoc = wb.Document;

            //string htmlt = wbDoc.Body.InnerHtml;

           var t1body =  webClient.LoadFromWebAsync(htmlURL);
            // t1body.LoadHtml(htmlt);

            //t1body.OptionOutputAsXml = true;
            //var nodes = t1body.DocumentNode.SelectSingleNode("//*[@id=\"j_inquiry_lst\"]");


            // HtmlAgilityPack.HtmlDocument doc = webClient.LoadFromBrowser(htmlURL);
            // HtmlAgilityPack.HtmlDocument doc = webClient.Load(htmlt);
            // var rootNode = doc.GetElementbyId("j_inquiry_lst");
            //doc.OptionOutputAsXml = true;

            //string html = doc.ParsedText;

            //HtmlAgilityPack.HtmlDocument a1 = new HtmlAgilityPack.HtmlDocument();
            //a1.LoadHtml(html);

            // var rootNode = doc.DocumentNode.SelectSingleNode("//*[@id=\"j_inquiry_lst\"]");

            //if (rootNode == null)
            //    return;

            //HtmlAgilityPack.HtmlDocument tbody = new HtmlAgilityPack.HtmlDocument();
            //tbody.LoadHtml(rootNode.InnerHtml);

            //tbody.OptionOutputAsXml = true;

            //var htmlNode = tbody.DocumentNode.SelectNodes(".//tr");

            //DataTable dt = InitDataTable();

            //DataRow dr;

            //foreach (var table in htmlNode)
            //{
            //    dr = dt.NewRow();


            //    //基本数据
            //    //  dr["id"] = table.SelectSingleNode("td[1]//input[@name='id']").InnerText.Trim();
            //    dr["id"] = table.SelectSingleNode("td[2]//a").Attributes["href"].Value.Split('=').LastOrDefault();
            //    dr["zx"] = table.SelectSingleNode("td[2]").InnerText.Trim();
            //    dr["yhm"] = table.SelectSingleNode("td[3]").InnerText.Trim();
            //    dr["dq"] = table.SelectSingleNode("td[4]").InnerText.Trim();
            //    dr["sj"] = table.SelectSingleNode("td[5]").InnerText.Trim();

            //    //详情数据
            //    var xqurl = table.SelectSingleNode("td[2]//a").Attributes["href"].Value;

            //    //HtmlElement btn = doc.GetElementById(btn_id);
            //    //btn.InvokeMember("click");
            //    dr["dh"] = "";
            //    dr["yx"] = "";
            //    dr["dw"] = "";
            //    dr["zy"] = "";
            //    dr["xxdz"] = "";
            //    dt.Rows.Add(dr);
            //}

            //List<Infos> infos = new List<Infos>();


            //foreach (var table in htmlNode)
            //{
            //    Infos info = new Infos();
            //    //基本数据
            //    //  dr["id"] = table.SelectSingleNode("td[1]//input[@name='id']").InnerText.Trim();
            //    info.id= table.SelectSingleNode("td[2]//a").Attributes["href"].Value.Split('=').LastOrDefault();
            //    info.zx = table.SelectSingleNode("td[2]").InnerText.Trim();
            //    info.yhm = table.SelectSingleNode("td[3]").InnerText.Trim();
            //    info.dq = table.SelectSingleNode("td[4]").InnerText.Trim();
            //    info.sj = table.SelectSingleNode("td[5]").InnerText.Trim();

            //    //详情数据
            //    var xqurl = table.SelectSingleNode("td[2]//a").Attributes["href"].Value;

            //    //HtmlElement btn = doc.GetElementById(btn_id);
            //    //btn.InvokeMember("click");



            //    JieXiXQHTML(table.SelectSingleNode("td[2]//a").Attributes["href"].Value, ref info);

            //    infos.Add(info);
            //}


        }
        private DataTable InitDataTable()
        {
            DataTable retProDt = new DataTable();

            DataColumn id = new DataColumn("id", typeof(string));
            DataColumn zx = new DataColumn("zx", typeof(string));
            DataColumn yhm = new DataColumn("yhm", typeof(string));
            DataColumn dq = new DataColumn("dq", typeof(string));
            DataColumn sj = new DataColumn("sj", typeof(string));
            DataColumn dh = new DataColumn("dh", typeof(string));
            DataColumn yx = new DataColumn("yx", typeof(string));
            DataColumn dw = new DataColumn("dw", typeof(string));
            DataColumn zy = new DataColumn("zy", typeof(string));
            DataColumn xxdz = new DataColumn("xxdz", typeof(string));


            retProDt.Columns.Add(id);
            retProDt.Columns.Add(zx);
            retProDt.Columns.Add(yhm);
            retProDt.Columns.Add(dq);
            retProDt.Columns.Add(sj);
            retProDt.Columns.Add(dh);
            retProDt.Columns.Add(yx);
            retProDt.Columns.Add(dw);
            retProDt.Columns.Add(zy);
            retProDt.Columns.Add(xxdz);

            return retProDt;
        }



        #endregion


        /// <summary>
        /// ajax
        /// </summary>
        public void AjaxTest()
        {
            this.wb.Visible = false;

            for (int i = 1; i <= pageCount; i++)
            {
                string url = $"https://www.biomart.cn/japi/agency/enquiry/list?_csrf=4c546c75-02af-4342-a3e4-4c7e4375fd9e&pageNo={i}";

                var httpResponse = WebRequestConstruct(url);
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    Console.WriteLine(result);
                    if (!string.IsNullOrEmpty(result))
                    {
                        //数据写入
                        if (this.InvokeRequired)
                        {
                            this.Invoke(new DelegateGridViewDataBand(GridViewDataBand), new object[] { result });
                        }
                        else
                        {
                            GridViewDataBand(result);
                        }
                    }
                    else
                    {
                        MessageBox.Show("没有获取到数据");
                    }

                }

                if (this.InvokeRequired)
                {
                    this.Invoke(new DelegateTJ(TJ), new object[] { (tjcount).ToString() });
                }
                else
                {
                    TJ((tjcount).ToString());
                }
            }

            this.btn_dc.Enabled = true;
            this.btn_jx.Enabled = false;
        }

        /// <summary>
        /// 详情页面
        /// </summary>
        public tempModel XiangQing(string userId)
        {
            tempModel model = new tempModel();
            string url = $"https://www.biomart.cn/japi/agency/enquiry/{userId}?_csrf=4c546c75-02af-4342-a3e4-4c7e4375fd9e&action=GetDetail&id={userId}";
            HttpWebResponse httpResponse = WebRequestConstruct(url);
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                Console.WriteLine(result);
                if (!string.IsNullOrEmpty(result))
                {
                    var dynamicObject = JsonConvert.DeserializeObject<dynamic>(result);
                    if ((bool)dynamicObject.success)
                    {
                        model.phone = dynamicObject.results.phone != null ? dynamicObject.results.phone : "";
                        model.unit = dynamicObject.results.unit != null ? dynamicObject.results.unit : "";
                        model.email = dynamicObject.results.email != null ? dynamicObject.results.email : "";
                        model.searchLog = dynamicObject.results.searchLog!=null ? dynamicObject.results.searchLog.ToString().TrimEnd(']').TrimStart('[') : "";
                        model.productLog = dynamicObject.results.productLog != null ? dynamicObject.results.productLog.ToString().TrimEnd(']').TrimStart('[') : "";

                        return model;
                    }
                    else
                    {
                        return model;
                    }
                }
                else
                {
                    //  MessageBox.Show("没有获取到数据");
                    return model;
                }

            }
        }


        private HttpWebResponse WebRequestConstruct(string url)
        {
            //if (this.wb.Document != null && this.wb.Document.Cookie != null)
            //{
            //    _cookie = this.wb.Document.Cookie;
            //}

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Connection = "application/json;charset=UTF-8";
            httpWebRequest.Method = "GET";

            httpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.88 Safari/537.36";

            httpWebRequest.Headers.Add("Cookie", _cookie);


            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            return httpResponse;
        }

        private void GridViewDataBand(string result)
        {
            var dynamicObject = JsonConvert.DeserializeObject<dynamic>(result);
            if ((bool)dynamicObject.success)
            {
                _totalCount = dynamicObject.results.pageBean.totalCount;
                var datas = dynamicObject.results.items;

                foreach (var item in datas)
                {
                    int index = this.dataGridView1.Rows.Add();
;
                    var model = this.XiangQing(((int)item.id).ToString());

                    this.dataGridView1.Rows[index].Cells[0].Value = item.id;
                    this.dataGridView1.Rows[index].Cells[1].Value = item.productName;
                    this.dataGridView1.Rows[index].Cells[2].Value = item.contactName;
                    this.dataGridView1.Rows[index].Cells[3].Value = item.creator;
                    this.dataGridView1.Rows[index].Cells[4].Value = item.city;
                    this.dataGridView1.Rows[index].Cells[5].Value = item.modifyTime;
                    this.dataGridView1.Rows[index].Cells[6].Value = item.productUrl;
                    this.dataGridView1.Rows[index].Cells[7].Value = model.phone;
                    this.dataGridView1.Rows[index].Cells[8].Value = model.email;
                    this.dataGridView1.Rows[index].Cells[9].Value = model.searchLog;
                    this.dataGridView1.Rows[index].Cells[10].Value = model.productLog;



                    this.dataGridView1.FirstDisplayedScrollingRowIndex = this.dataGridView1.Rows[this.dataGridView1.Rows.Count - 1].Index;

                    //数据写入
                    //if (this.InvokeRequired)
                    //{
                    //    this.Invoke(new DelegateTJ(TJ), new object[] { (tjcount += 1).ToString() });
                    //}
                    //else
                    //{
                    //    TJ((tjcount += 1).ToString());
                    //}
                    tjcount = tjcount += 1;
                }


                //  ViewDataBind(datas);
            }
            else
            {
                MessageBox.Show(dynamicObject.success);
            }
        }

        /// <summary>
        /// 视图数据绑定
        /// </summary>
        public void ViewDataBind(object data)
        {

            //int index = this.dataGridView1.Rows.Add();
            //this.dataGridView1.Rows[index].Cells[0].Value = data.creator;
            //this.dataGridView1.Rows[index].Cells[1].Value = "2";
            //this.dataGridView1.Rows[index].Cells[2].Value = "3";
            //this.dataGridView1.Rows[index].Cells[4].Value = "42";
            //this.dataGridView1.Rows[index].Cells[5].Value = "43";
            //this.dataGridView1.Rows[index].Cells[6].Value = "44";
            //this.dataGridView1.Rows[index].Cells[7].Value = "44";
            //this.dataGridView1.Rows[index].Cells[8].Value = "4";
            //this.dataGridView1.Rows[index].Cells[9].Value = "4";


            //this.dataGridView1.FirstDisplayedScrollingRowIndex = this.dataGridView1.Rows[this.dataGridView1.Rows.Count - 1].Index;

            //  this.dataGridView1.DataSource += data;

            //dataGridView.DataSource = data;

        }

        public void TJ(string count)
        {
            lbl_tj.Text = count;
        }


    }

    public class tempModel
    {
        public string unit { get; set; }

        public string email { get; set; }
        public string phone { get; set; }
        public string productLog { get; set; }
        public string searchLog { get; set; }
    }
}
