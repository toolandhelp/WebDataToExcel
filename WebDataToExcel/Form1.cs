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

        private int totalCount = 1, pageSize = 15, pageCount = 1,thisPageCount=1, tempCount = 0;

        private delegate void DelegateGridViewDataBand(string datas);
        private delegate void DelegateYXZT(int thispage,int thiscount);
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
            this.dataGridView1.Enabled = true;
            this.GetZXGL();
        }

        /// <summary>
        /// 导出02
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmi02_Click(object sender, EventArgs e)
        {

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

                var httpResponse =Util.WebRequestConstruct(url);
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
                            this.Invoke(new DelegateYXZT(yxztBand), new object[] { thisPageCount, tempCount });
                        }
                        else
                        {
                            GridViewDataBand(result);
                            yxztBand(thisPageCount, tempCount);
                        }
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
                    int index = this.dataGridView1.Rows.Add();

                    var model = this.XiangQing(((int)item.id).ToString());

                    this.dataGridView1.Rows[index].Cells[0].Value = item.id;
                    this.dataGridView1.Rows[index].Cells[1].Value = item.productName;
                    this.dataGridView1.Rows[index].Cells[2].Value = item.contactName;
                    this.dataGridView1.Rows[index].Cells[3].Value = item.creator;
                    this.dataGridView1.Rows[index].Cells[4].Value = item.city;
                    this.dataGridView1.Rows[index].Cells[5].Value = item.modifyTime;
                    //this.dataGridView1.Rows[index].Cells[6].Value = item.productUrl;
                    this.dataGridView1.Rows[index].Cells[6].Value = model.phone;
                    this.dataGridView1.Rows[index].Cells[7].Value = model.email;
                    this.dataGridView1.Rows[index].Cells[8].Value = model.searchLog;
                    this.dataGridView1.Rows[index].Cells[9].Value = model.productLog;

                    this.dataGridView1.FirstDisplayedScrollingRowIndex = this.dataGridView1.Rows[this.dataGridView1.Rows.Count - 1].Index;

                    //});

                    //数据写入
                    //if (this.InvokeRequired)
                    //{
                    //    this.Invoke(new DelegateYXZT(yxztBand), new object[] { thisPageCount, tempCount });
                    //}
                    //else
                    //{
                    //    yxztBand(thisPageCount, tempCount);
                    //}
                }
                //  ViewDataBind(datas);
            }
            else
            {
                MessageBox.Show(dynamicObject.success);
            }
        }


        public void dataGirdViewBind(decimal item)
        {
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                ExcelHelper.ExportToExcel(ExcelHelper.ToDataTable(this.dataGridView1));
            }
            catch (Exception ex)
            {
                MessageBox.Show("导出失败");
            }
        }

        /// <summary>
        /// 总状态绑定
        /// </summary>
        public void ztBand(int pagesize=0,int tempcount=0)
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

        public class tempModel
        {
            public string unit { get; set; }

            public string email { get; set; }
            public string phone { get; set; }
            public string productLog { get; set; }
            public string searchLog { get; set; }
        }
    }
}
