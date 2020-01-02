namespace WebDataToExcel
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_dc = new System.Windows.Forms.Button();
            this.wb = new System.Windows.Forms.WebBrowser();
            this.btn_jx = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.lbl_tj = new System.Windows.Forms.Label();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contactName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.creator = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.city = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.modifyTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productUrl = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.phone = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.email = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.searchLog = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productLog = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_dc
            // 
            this.btn_dc.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btn_dc.Enabled = false;
            this.btn_dc.Location = new System.Drawing.Point(572, 416);
            this.btn_dc.Name = "btn_dc";
            this.btn_dc.Size = new System.Drawing.Size(75, 23);
            this.btn_dc.TabIndex = 2;
            this.btn_dc.Text = "导出";
            this.btn_dc.UseVisualStyleBackColor = true;
            this.btn_dc.Click += new System.EventHandler(this.btn_dc_Click);
            // 
            // wb
            // 
            this.wb.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.wb.Location = new System.Drawing.Point(-1, 2);
            this.wb.MinimumSize = new System.Drawing.Size(20, 20);
            this.wb.Name = "wb";
            this.wb.Size = new System.Drawing.Size(894, 408);
            this.wb.TabIndex = 3;
            // 
            // btn_jx
            // 
            this.btn_jx.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btn_jx.Enabled = false;
            this.btn_jx.Location = new System.Drawing.Point(463, 416);
            this.btn_jx.Name = "btn_jx";
            this.btn_jx.Size = new System.Drawing.Size(75, 23);
            this.btn_jx.TabIndex = 1;
            this.btn_jx.Text = "解析";
            this.btn_jx.UseVisualStyleBackColor = true;
            this.btn_jx.Click += new System.EventHandler(this.btn_jx_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.productName,
            this.contactName,
            this.creator,
            this.city,
            this.modifyTime,
            this.productUrl,
            this.phone,
            this.email,
            this.searchLog,
            this.productLog});
            this.dataGridView1.Location = new System.Drawing.Point(-1, 2);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(894, 408);
            this.dataGridView1.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 427);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "获取统计数为：";
            // 
            // lbl_tj
            // 
            this.lbl_tj.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_tj.AutoSize = true;
            this.lbl_tj.Location = new System.Drawing.Point(98, 427);
            this.lbl_tj.Name = "lbl_tj";
            this.lbl_tj.Size = new System.Drawing.Size(11, 12);
            this.lbl_tj.TabIndex = 6;
            this.lbl_tj.Text = "0";
            // 
            // id
            // 
            this.id.HeaderText = "用户ID";
            this.id.Name = "id";
            // 
            // productName
            // 
            this.productName.HeaderText = "咨询项目名称";
            this.productName.Name = "productName";
            // 
            // contactName
            // 
            this.contactName.HeaderText = "用户姓名";
            this.contactName.Name = "contactName";
            // 
            // creator
            // 
            this.creator.HeaderText = "用户昵称";
            this.creator.Name = "creator";
            // 
            // city
            // 
            this.city.HeaderText = "地区";
            this.city.Name = "city";
            // 
            // modifyTime
            // 
            this.modifyTime.HeaderText = "时间";
            this.modifyTime.Name = "modifyTime";
            // 
            // productUrl
            // 
            this.productUrl.HeaderText = "详情地址";
            this.productUrl.Name = "productUrl";
            // 
            // phone
            // 
            this.phone.HeaderText = "手机号码";
            this.phone.Name = "phone";
            // 
            // email
            // 
            this.email.HeaderText = "邮箱";
            this.email.Name = "email";
            // 
            // searchLog
            // 
            this.searchLog.HeaderText = "搜索日志";
            this.searchLog.Name = "searchLog";
            // 
            // productLog
            // 
            this.productLog.HeaderText = "产品日志";
            this.productLog.Name = "productLog";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(895, 443);
            this.Controls.Add(this.lbl_tj);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.wb);
            this.Controls.Add(this.btn_dc);
            this.Controls.Add(this.btn_jx);
            this.Name = "Form1";
            this.Text = "数据导出";
            this.Load += new System.EventHandler(this.Form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btn_dc;
        private System.Windows.Forms.WebBrowser wb;
        private System.Windows.Forms.Button btn_jx;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbl_tj;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn productName;
        private System.Windows.Forms.DataGridViewTextBoxColumn contactName;
        private System.Windows.Forms.DataGridViewTextBoxColumn creator;
        private System.Windows.Forms.DataGridViewTextBoxColumn city;
        private System.Windows.Forms.DataGridViewTextBoxColumn modifyTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn productUrl;
        private System.Windows.Forms.DataGridViewTextBoxColumn phone;
        private System.Windows.Forms.DataGridViewTextBoxColumn email;
        private System.Windows.Forms.DataGridViewTextBoxColumn searchLog;
        private System.Windows.Forms.DataGridViewTextBoxColumn productLog;
    }
}

