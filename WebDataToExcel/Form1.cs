using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebDataToExcel
{
    public partial class Form1 : Form
    {

        private int totalCount = 1, pageSize = 15, pageCount = 1, thisPageCount = 1, tempCount = 0;

        private delegate void DelegateGridViewDataBand(string retData);
        private delegate void DelegateGridViewDataListBand(string id, string projuctName, string contactName, string creator, string modifyTime);

        private delegate void DelegateYXZT(int thispage, int thiscount);
        private delegate void DelegateGetZXGL();

        private List<QgglModel> qgglModels = new List<QgglModel>();

        public Form1()
        {
            InitializeComponent();
            ztBand();
            yxztBand();
        }

        /// <summary>
        /// 导出01
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmi01_Click(object sender, EventArgs e)
        {
            this.dataGridView1.Rows.Clear();
            this.dataGridView1.Enabled = true;

            totalCount = 1; pageSize = 15; pageCount = 1; thisPageCount = 1; tempCount = 0;

            if (this.InvokeRequired)
            {
                this.Invoke(new DelegateGetZXGL(GetZXGL), new object[] { });
            }
            else
            {
                this.GetZXGL();
            }
        }

        /// <summary>
        /// 导出02
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmi02_Click(object sender, EventArgs e)
        {
            this.dataGridView1.Rows.Clear();
            totalCount = 1; pageSize = 20; pageCount = 1; thisPageCount = 1; tempCount = 0;
            this.GetQGGL();
        }

        /// <summary>
        /// 获取资讯管理数据
        /// </summary>
        public void GetZXGL()
        {
            // this.wb.Visible = false;

            for (int i = 1; i <= pageCount; i++)
            {
                string url = $"https://www.biomart.cn/japi/agency/enquiry/list?_csrf=4c546c75-02af-4342-a3e4-4c7e4375fd9e&pageNo={i}";

                var httpResponse = Util.WebRequestConstruct(url);
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    Console.WriteLine(result);
                    if (!string.IsNullOrEmpty(result))
                    {
                        //数据写入
                        //if (this.InvokeRequired)
                        //{
                        //    this.Invoke(new DelegateGridViewDataBand(GridViewDataBand), new object[] { result });
                        //    this.Invoke(new DelegateYXZT(yxztBand), new object[] { thisPageCount, tempCount });
                        //}
                        //else
                        //{
                        GridViewDataBand(result);
                        yxztBand(thisPageCount, tempCount);
                        // }
                    }
                    else
                    {
                        MessageBox.Show("没有获取到数据");
                    }

                }

                thisPageCount++;
            }

            //this.btn_dc.Enabled = true;
            //this.btn_jx.Enabled = false;
        }


        private void GridViewDataBand(string result)
        {
            var dynamicObject = JsonConvert.DeserializeObject<dynamic>(result);
            if ((bool)dynamicObject.success)
            {
                if (totalCount == 1 && pageCount == 1)
                {
                    totalCount = dynamicObject.results.pageBean.totalCount;
                    pageSize = dynamicObject.results.pageBean.pageSize;
                    pageCount = ((int)dynamicObject.results.pageBean.totalCount / (int)dynamicObject.results.pageBean.pageSize) + (totalCount % pageSize == 0 ? 0 : 1);

                    this.ztBand(pageCount, totalCount);
                }
                var datas = dynamicObject.results.items;

                foreach (var item in datas)
                {
                    tempCount += 1;
                    //GridViewBind(this.dataGridView1);

                    //Task task = new Task(() =>
                    //{


                    //});

                    //数据写入
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new DelegateGridViewDataListBand(dataGirdViewListBind), new object[] { (String)item.id, (String)item.projuctName, (String)item.contactName, (String)item.creator, (String)item.modifyTime });
                    }
                    else
                    {
                        dataGirdViewListBind((String)item.id, (String)item.projuctName, (String)item.contactName, (String)item.creator, (String)item.modifyTime);
                    }
                }
                //  ViewDataBind(datas);
            }
            else
            {
                MessageBox.Show(dynamicObject.success);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="projuctName"></param>
        /// <param name="contactName"></param>
        /// <param name="creator"></param>
        /// <param name="modifyTime"></param>
        /// <param name="phone"></param>
        /// <param name="email"></param>
        /// <param name="searchLog"></param>
        /// <param name="productLog"></param>
        public void dataGirdViewListBind(string id, string productName, string contactName, string creator, string modifyTime)
        {
            int index = this.dataGridView1.Rows.Add();

            var model = this.XiangQing(id);

            this.dataGridView1.Rows[index].Cells[0].Value = id;
            this.dataGridView1.Rows[index].Cells[1].Value = productName;
            this.dataGridView1.Rows[index].Cells[2].Value = contactName;
            this.dataGridView1.Rows[index].Cells[3].Value = creator;
            this.dataGridView1.Rows[index].Cells[4].Value = city;
            this.dataGridView1.Rows[index].Cells[5].Value = modifyTime;
            //this.dataGridView1.Rows[index].Cells[6].Value = item.productUrl;
            this.dataGridView1.Rows[index].Cells[6].Value = model.phone;
            this.dataGridView1.Rows[index].Cells[7].Value = model.email;
            this.dataGridView1.Rows[index].Cells[8].Value = model.searchLog;
            this.dataGridView1.Rows[index].Cells[9].Value = model.productLog;

            this.dataGridView1.FirstDisplayedScrollingRowIndex = this.dataGridView1.Rows[this.dataGridView1.Rows.Count - 1].Index;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (qgglModels.Any())
                {
                    ExcelHelper.ExportToExcel(ExcelHelper.ToDataTable(qgglModels));
                }
                else
                {
                    ExcelHelper.ExportToExcel(ExcelHelper.ToDataTable(this.dataGridView1));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("导出失败");
            }
        }

        public void GetQGGL()
        {
            button1.Enabled = false;

            for (int i = 1; i <= pageCount; i++)
            {
                string url = $"https://www.biomart.cn/i/opt.do?_csrf=4c546c75-02af-4342-a3e4-4c7e4375fd9e&page_index={i}&items_per_page=20&cate=0&action=List&sortBy=&order=";

                var httpResponse = Util.WebRequestConstruct(url);
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    Console.WriteLine(result);
                    if (!string.IsNullOrEmpty(result))
                    {
                        var dynamicObject = JsonConvert.DeserializeObject<dynamic>(result);
                        if (dynamicObject != null)
                        {
                            if (totalCount == 1 && pageCount == 1)
                            {
                                totalCount = dynamicObject.data.total_items;
                                pageSize = dynamicObject.data.items_per_page;
                                pageCount = dynamicObject.data.total_pages;

                                this.ztBand(pageCount, totalCount);
                            }
                            var datas = dynamicObject.data.items;



                            foreach (var item in datas)
                            {
                                tempCount += 1;
                                //GridViewBind(this.dataGridView1);

                                //Task task = new Task(() =>
                                //{

                                string uid = GXXingQingUserId((String)item.demand_id);

                               
                                QgglModel qgglModel = new QgglModel();
                                if (!string.IsNullOrEmpty(uid))
                                {
                                    qgglModel = QGXiangQing(uid);
                                }

                                qgglModel.cate = item.cate;
                                qgglModel.city = item.city;
                                qgglModel.name = item.name;
                                qgglModel.id = item.id;
                                qgglModel.release_time = item.release_time;


                                qgglModels.Add(qgglModel);
                                //});

                                //数据写入
                                //if (this.InvokeRequired)
                                //{
                                //    this.Invoke(new DelegateGridViewDataListBand(dataGirdViewListBind), new object[] { (String)item.id, (String)item.projuctName, (String)item.contactName, (String)item.creator, (String)item.modifyTime });
                                //}
                                //else
                                //{
                                //    dataGirdViewListBind((String)item.id, (String)item.projuctName, (String)item.contactName, (String)item.creator, (String)item.modifyTime);
                                //}
                            }
                            //  ViewDataBind(datas);
                        }
                        else
                        {
                            MessageBox.Show(dynamicObject.success);
                        }
                    }
                    else
                    {
                        MessageBox.Show("没有获取到数据");
                    }

                }

                thisPageCount++;
            }

            button1.Enabled = true;
        }


        /// <summary>
        /// 总状态绑定
        /// </summary>
        public void ztBand(int pagesize = 0, int tempcount = 0)
        {
            this.toolStripStatusLabel1.Text = string.Format($"共计{pagesize}页码，{tempcount}条数据");
        }
        /// <summary>
        /// 运行状态绑定
        /// </summary>
        public void yxztBand(int thispage = 0, int thiscount = 0)
        {
            this.toolStripStatusLabel2.Text = string.Format($"当前第{thispage}页，第{thiscount}条数据");
        }

        /// <summary>
        /// 详情页面
        /// </summary>
        public tempModel XiangQing(string userId)
        {
            tempModel model = new tempModel();
            string url = $"https://www.biomart.cn/japi/agency/enquiry/{userId}?_csrf=4c546c75-02af-4342-a3e4-4c7e4375fd9e&action=GetDetail&id={userId}";
            var httpResponse = Util.WebRequestConstruct(url);
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
                        model.searchLog = dynamicObject.results.searchLog != null ? dynamicObject.results.searchLog.ToString().TrimEnd(']').TrimStart('[') : "";
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


        public string GXXingQingUserId(string demand_id)
        {
            string url = "https://www.biomart.cn/usercenter/info/infoDemand/contact";

            string json = "{\"_csrf\":\"4c546c75-02af-4342-a3e4-4c7e4375fd9e\"," +
                          "\"infoDemandId\":\"" + demand_id + "\"," +
                          "\"confirm\":\"false\"}";
            string body =$"_csrf=4c546c75-02af-4342-a3e4-4c7e4375fd9e&infoDemandId={demand_id}&confirm=false";

            string res = Util.PostHttp(url, body);

            var dynamicObject = JsonConvert.DeserializeObject<dynamic>(res);
            if ((bool)dynamicObject.success)
            {
                return ((string)dynamicObject.results.item.url).Split('=').LastOrDefault();
            }

            //var httpResponse = Util.WebRequestConstruct(url, "POST", json);
            //using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            //{
            //    var result = streamReader.ReadToEnd();
            //    Console.WriteLine(result);
            //    if (!string.IsNullOrEmpty(result))
            //    {
            //        var dynamicObject = JsonConvert.DeserializeObject<dynamic>(result);
            //        if ((bool)dynamicObject.success)
            //        {

            //        }
            //    }
            //}

            return "";
        }

        /// <summary>
        /// 详情页面
        /// </summary>
        public QgglModel QGXiangQing(string userId)
        {
            QgglModel model = new QgglModel();
            string url = $"https://www.biomart.cn/japi/agency/enquiry/{userId}?_csrf=4c546c75-02af-4342-a3e4-4c7e4375fd9e&action=GetDetail&id={userId}";
            var httpResponse = Util.WebRequestConstruct(url);
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
                        model.email = dynamicObject.results.email != null ? dynamicObject.results.email : "";
                        model.searchLog = dynamicObject.results.searchLog != null ? dynamicObject.results.searchLog.ToString().TrimEnd(']').TrimStart('[') : "";
                        model.productLog = dynamicObject.results.productLog != null ? dynamicObject.results.productLog.ToString().TrimEnd(']').TrimStart('[') : "";
                        model.fullAddress = dynamicObject.results.fullAddress != null ? dynamicObject.results.fullAddress : "";
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

        public class tempModel
        {
            public string unit { get; set; }

            public string email { get; set; }
            public string phone { get; set; }
            public string productLog { get; set; }
            public string searchLog { get; set; }
        }

        public class QgglModel
        {
            public string cate { get; set; }
            public string city { get; set; }
            public string name { get; set; }

            public string id { get; set; }

            public string release_time { get; set; }

            public string fullAddress { get; set; }

            public string phone { get; set; }
            public string email { get; set; }
            public string productLog { get; set; }
            public string searchLog { get; set; }
        }
    }
}
