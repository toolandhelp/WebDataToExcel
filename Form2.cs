using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace WebDataToExcel
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            JieXiHTML(@"C:\Users\Administrator\Documents\WeChat Files\wxid_5grdl4vyhy1p22\FileStorage\File\2019-12\temp\丁香通_企业管理中心.html");
        }

        private void JieXiHTML(string htmlURL)
        {
            HtmlWeb webClient = new HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = webClient.Load(htmlURL);
            var rootNode = doc.GetElementbyId("j_inquiry_lst");

            HtmlAgilityPack.HtmlDocument tbody = new HtmlAgilityPack.HtmlDocument();
            tbody.LoadHtml(rootNode.InnerHtml);

            tbody.OptionOutputAsXml = true;

            var htmlNode = tbody.DocumentNode.SelectNodes(".//tr");

            DataTable dt = InitDataTable();

            DataRow dr;

            foreach (var table in htmlNode)
            {
                dr = dt.NewRow();
               

                //基本数据
                //  dr["id"] = table.SelectSingleNode("td[1]//input[@name='id']").InnerText.Trim();
                dr["id"] = table.SelectSingleNode("td[2]//a").Attributes["href"].Value.Split('=').LastOrDefault();
                dr["zx"] = table.SelectSingleNode("td[2]").InnerText.Trim();
                dr["yhm"] = table.SelectSingleNode("td[3]").InnerText.Trim();
                dr["dq"] = table.SelectSingleNode("td[4]").InnerText.Trim();
                dr["sj"] = table.SelectSingleNode("td[5]").InnerText.Trim();

                //详情数据
                var xqurl = table.SelectSingleNode("td[2]//a").Attributes["href"].Value;

                //HtmlElement btn = doc.GetElementById(btn_id);
                //btn.InvokeMember("click");







                dr["dh"] = "";
                dr["yx"] = "";
                dr["dw"] = "";
                dr["zy"] = "";
                dr["xxdz"] = "";



                dt.Rows.Add(dr);
            }



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
    }
}
