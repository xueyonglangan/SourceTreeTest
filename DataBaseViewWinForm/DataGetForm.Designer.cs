using System.Windows.Forms;
using System;

namespace DataBaseViewWinForm
{
    partial class DataGetForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataGetForm));
            this.CurrentDataBaseType = new System.Windows.Forms.ComboBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.sqLiteCommand1 = new System.Data.SQLite.SQLiteCommand();
            this.CurrentDataBaseName = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ProductNameText = new System.Windows.Forms.TextBox();
            this.StartTime1 = new System.Windows.Forms.DateTimePicker();
            this.EndTime1 = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.EndTime213 = new System.Windows.Forms.Label();
            this.SearchDataBtn = new System.Windows.Forms.Button();
            this.ExportDataBtn = new System.Windows.Forms.Button();
            this.SearchDetailDataBtn = new System.Windows.Forms.Button();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.DataBaseWayBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.ShowDataBaseWay = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.CurrentDataBaseWay = new System.Windows.Forms.TextBox();
            this.ExportDataBtn1 = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lblPaginationInfo = new System.Windows.Forms.Label();
            this.btnLastPage = new System.Windows.Forms.Button();
            this.btnNextPage = new System.Windows.Forms.Button();
            this.btnPrevPage = new System.Windows.Forms.Button();
            this.btnFirstPage = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // CurrentDataBaseType
            // 
            this.CurrentDataBaseType.FormattingEnabled = true;
            this.CurrentDataBaseType.Items.AddRange(new object[] {
            "MySQL",
            "SQLite"});
            this.CurrentDataBaseType.Location = new System.Drawing.Point(29, 12);
            this.CurrentDataBaseType.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.CurrentDataBaseType.Name = "CurrentDataBaseType";
            this.CurrentDataBaseType.Size = new System.Drawing.Size(121, 23);
            this.CurrentDataBaseType.TabIndex = 9;
            this.CurrentDataBaseType.Text = "SQLite";
            this.CurrentDataBaseType.Visible = false;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(1501, 653);
            this.dataGridView1.TabIndex = 11;
            // 
            // sqLiteCommand1
            // 
            this.sqLiteCommand1.CommandText = null;
            // 
            // CurrentDataBaseName
            // 
            this.CurrentDataBaseName.FormattingEnabled = true;
            this.CurrentDataBaseName.Items.AddRange(new object[] {
            "secote_PartNumber",
            "secote_Product",
            "secote_LDIExp",
            "test"});
            this.CurrentDataBaseName.Location = new System.Drawing.Point(-159, -23);
            this.CurrentDataBaseName.Name = "CurrentDataBaseName";
            this.CurrentDataBaseName.Size = new System.Drawing.Size(174, 23);
            this.CurrentDataBaseName.TabIndex = 12;
            this.CurrentDataBaseName.Text = "secote_PartNumber";
            this.CurrentDataBaseName.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 123);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 15);
            this.label2.TabIndex = 17;
            this.label2.Text = "料号：";
            // 
            // ProductNameText
            // 
            this.ProductNameText.Location = new System.Drawing.Point(79, 117);
            this.ProductNameText.Name = "ProductNameText";
            this.ProductNameText.Size = new System.Drawing.Size(380, 25);
            this.ProductNameText.TabIndex = 18;
            // 
            // StartTime1
            // 
            this.StartTime1.Location = new System.Drawing.Point(586, 55);
            this.StartTime1.Name = "StartTime1";
            this.StartTime1.Size = new System.Drawing.Size(200, 25);
            this.StartTime1.TabIndex = 19;
            // 
            // EndTime1
            // 
            this.EndTime1.Location = new System.Drawing.Point(586, 116);
            this.EndTime1.Name = "EndTime1";
            this.EndTime1.Size = new System.Drawing.Size(200, 25);
            this.EndTime1.TabIndex = 20;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(505, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 15);
            this.label3.TabIndex = 21;
            this.label3.Text = "开始时间：";
            // 
            // EndTime213
            // 
            this.EndTime213.AutoSize = true;
            this.EndTime213.Location = new System.Drawing.Point(505, 123);
            this.EndTime213.Name = "EndTime213";
            this.EndTime213.Size = new System.Drawing.Size(75, 15);
            this.EndTime213.TabIndex = 22;
            this.EndTime213.Text = "结束时间:";
            // 
            // SearchDataBtn
            // 
            this.SearchDataBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SearchDataBtn.Location = new System.Drawing.Point(1049, 22);
            this.SearchDataBtn.Name = "SearchDataBtn";
            this.SearchDataBtn.Size = new System.Drawing.Size(184, 52);
            this.SearchDataBtn.TabIndex = 23;
            this.SearchDataBtn.Text = "查询料号生产信息";
            this.SearchDataBtn.UseVisualStyleBackColor = true;
            this.SearchDataBtn.Click += new System.EventHandler(this.Search_Click);
            // 
            // ExportDataBtn
            // 
            this.ExportDataBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ExportDataBtn.Location = new System.Drawing.Point(1250, 102);
            this.ExportDataBtn.Name = "ExportDataBtn";
            this.ExportDataBtn.Size = new System.Drawing.Size(132, 37);
            this.ExportDataBtn.TabIndex = 25;
            this.ExportDataBtn.Text = "导出数据";
            this.ExportDataBtn.UseVisualStyleBackColor = true;
            this.ExportDataBtn.Click += new System.EventHandler(this.ExportData_Click);
            // 
            // SearchDetailDataBtn
            // 
            this.SearchDetailDataBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SearchDetailDataBtn.Location = new System.Drawing.Point(1049, 95);
            this.SearchDetailDataBtn.Name = "SearchDetailDataBtn";
            this.SearchDetailDataBtn.Size = new System.Drawing.Size(184, 51);
            this.SearchDetailDataBtn.TabIndex = 50;
            this.SearchDetailDataBtn.Text = "查看料号详细数据";
            this.SearchDetailDataBtn.UseVisualStyleBackColor = true;
            this.SearchDetailDataBtn.Click += new System.EventHandler(this.SearchDetailDataBtn_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1507, 27);
            this.toolStrip1.TabIndex = 51;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DataBaseWayBtn,
            this.ShowDataBaseWay});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(53, 24);
            this.toolStripDropDownButton1.Text = "设置";
            // 
            // DataBaseWayBtn
            // 
            this.DataBaseWayBtn.Name = "DataBaseWayBtn";
            this.DataBaseWayBtn.Size = new System.Drawing.Size(227, 26);
            this.DataBaseWayBtn.Text = "自定义数据库路径";
            this.DataBaseWayBtn.Click += new System.EventHandler(this.DataBaseWayBtn_Click);
            // 
            // ShowDataBaseWay
            // 
            this.ShowDataBaseWay.Name = "ShowDataBaseWay";
            this.ShowDataBaseWay.Size = new System.Drawing.Size(227, 26);
            this.ShowDataBaseWay.Text = "查看当前数据库路径";
            this.ShowDataBaseWay.Click += new System.EventHandler(this.ShowDataBaseWay_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(176, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 15);
            this.label1.TabIndex = 52;
            this.label1.Text = "当前数据库路径：";
            this.label1.Visible = false;
            // 
            // CurrentDataBaseWay
            // 
            this.CurrentDataBaseWay.Location = new System.Drawing.Point(295, 10);
            this.CurrentDataBaseWay.Name = "CurrentDataBaseWay";
            this.CurrentDataBaseWay.ReadOnly = true;
            this.CurrentDataBaseWay.Size = new System.Drawing.Size(380, 25);
            this.CurrentDataBaseWay.TabIndex = 53;
            this.CurrentDataBaseWay.Visible = false;
            // 
            // ExportDataBtn1
            // 
            this.ExportDataBtn1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ExportDataBtn1.Location = new System.Drawing.Point(1248, 29);
            this.ExportDataBtn1.Name = "ExportDataBtn1";
            this.ExportDataBtn1.Size = new System.Drawing.Size(134, 39);
            this.ExportDataBtn1.TabIndex = 54;
            this.ExportDataBtn1.Text = "导出数据";
            this.ExportDataBtn1.UseVisualStyleBackColor = true;
            this.ExportDataBtn1.Click += new System.EventHandler(this.ExportDataBtn1_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel3, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 27);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 19.88636F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 71.02273F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.090909F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1507, 928);
            this.tableLayoutPanel1.TabIndex = 55;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dataGridView1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 187);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1501, 653);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.ProductNameText);
            this.panel2.Controls.Add(this.ExportDataBtn1);
            this.panel2.Controls.Add(this.ExportDataBtn);
            this.panel2.Controls.Add(this.CurrentDataBaseType);
            this.panel2.Controls.Add(this.CurrentDataBaseWay);
            this.panel2.Controls.Add(this.SearchDetailDataBtn);
            this.panel2.Controls.Add(this.SearchDataBtn);
            this.panel2.Controls.Add(this.CurrentDataBaseName);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.StartTime1);
            this.panel2.Controls.Add(this.EndTime1);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.EndTime213);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1501, 178);
            this.panel2.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.lblPaginationInfo);
            this.panel3.Controls.Add(this.btnLastPage);
            this.panel3.Controls.Add(this.btnNextPage);
            this.panel3.Controls.Add(this.btnPrevPage);
            this.panel3.Controls.Add(this.btnFirstPage);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 846);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1501, 79);
            this.panel3.TabIndex = 2;
            // 
            // lblPaginationInfo
            // 
            this.lblPaginationInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPaginationInfo.AutoSize = true;
            this.lblPaginationInfo.Location = new System.Drawing.Point(1001, 35);
            this.lblPaginationInfo.Name = "lblPaginationInfo";
            this.lblPaginationInfo.Size = new System.Drawing.Size(67, 15);
            this.lblPaginationInfo.TabIndex = 4;
            this.lblPaginationInfo.Text = "暂无数据";
            // 
            // btnLastPage
            // 
            this.btnLastPage.Location = new System.Drawing.Point(815, 28);
            this.btnLastPage.Name = "btnLastPage";
            this.btnLastPage.Size = new System.Drawing.Size(82, 29);
            this.btnLastPage.TabIndex = 3;
            this.btnLastPage.Text = "尾页";
            this.btnLastPage.UseVisualStyleBackColor = true;
            this.btnLastPage.Click += new System.EventHandler(this.btnLastPage_Click);
            // 
            // btnNextPage
            // 
            this.btnNextPage.Location = new System.Drawing.Point(684, 28);
            this.btnNextPage.Name = "btnNextPage";
            this.btnNextPage.Size = new System.Drawing.Size(87, 29);
            this.btnNextPage.TabIndex = 2;
            this.btnNextPage.Text = "后一页";
            this.btnNextPage.UseVisualStyleBackColor = true;
            this.btnNextPage.Click += new System.EventHandler(this.btnNextPage_Click);
            // 
            // btnPrevPage
            // 
            this.btnPrevPage.Location = new System.Drawing.Point(546, 28);
            this.btnPrevPage.Name = "btnPrevPage";
            this.btnPrevPage.Size = new System.Drawing.Size(93, 29);
            this.btnPrevPage.TabIndex = 1;
            this.btnPrevPage.Text = "前一页";
            this.btnPrevPage.UseVisualStyleBackColor = true;
            this.btnPrevPage.Click += new System.EventHandler(this.btnPrevPage_Click);
            // 
            // btnFirstPage
            // 
            this.btnFirstPage.Location = new System.Drawing.Point(424, 28);
            this.btnFirstPage.Name = "btnFirstPage";
            this.btnFirstPage.Size = new System.Drawing.Size(84, 29);
            this.btnFirstPage.TabIndex = 0;
            this.btnFirstPage.Text = "首页";
            this.btnFirstPage.UseVisualStyleBackColor = true;
            this.btnFirstPage.Click += new System.EventHandler(this.btnFirstPage_Click);
            // 
            // DataGetForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1507, 955);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.toolStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "DataGetForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "生产资料信息获取助手";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ComboBox CurrentDataBaseType;
        private DataGridView dataGridView1;
        private System.Data.SQLite.SQLiteCommand sqLiteCommand1;
        private ComboBox CurrentDataBaseName;
        private Label label2;
        private TextBox ProductNameText;
        private DateTimePicker StartTime1;
        private DateTimePicker EndTime1;
        private Label label3;
        private Label EndTime213;
        private Button SearchDataBtn;
        private Button ExportDataBtn;
        private Button SearchDetailDataBtn;
        private ToolStrip toolStrip1;
        private ToolStripDropDownButton toolStripDropDownButton1;
        private ToolStripMenuItem DataBaseWayBtn;
        private ToolStripMenuItem ShowDataBaseWay;
        private Label label1;
        private TextBox CurrentDataBaseWay;
        private Button ExportDataBtn1;
        private TableLayoutPanel tableLayoutPanel1;
        private Panel panel1;
        private Panel panel2;
        private Panel panel3;
        private Button btnFirstPage;
        private Button btnLastPage;
        private Button btnNextPage;
        private Button btnPrevPage;
        private Label lblPaginationInfo;
    }
}

