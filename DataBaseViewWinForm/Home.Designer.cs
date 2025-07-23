namespace DataBaseViewWinForm
{
    partial class Home
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.OneWeek = new System.Windows.Forms.Button();
            this.OneMonth = new System.Windows.Forms.Button();
            this.OneDay = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // chart1
            // 
            chartArea2.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chart1.Legends.Add(legend2);
            this.chart1.Location = new System.Drawing.Point(38, 12);
            this.chart1.Name = "chart1";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chart1.Series.Add(series2);
            this.chart1.Size = new System.Drawing.Size(800, 300);
            this.chart1.TabIndex = 2;
            this.chart1.Text = "chart1";
            // 
            // OneWeek
            // 
            this.OneWeek.Location = new System.Drawing.Point(370, 327);
            this.OneWeek.Name = "OneWeek";
            this.OneWeek.Size = new System.Drawing.Size(61, 29);
            this.OneWeek.TabIndex = 3;
            this.OneWeek.Text = "本周";
            this.OneWeek.UseVisualStyleBackColor = true;
            this.OneWeek.Click += new System.EventHandler(this.OneWeek_Click);
            // 
            // OneMonth
            // 
            this.OneMonth.Location = new System.Drawing.Point(488, 329);
            this.OneMonth.Name = "OneMonth";
            this.OneMonth.Size = new System.Drawing.Size(61, 27);
            this.OneMonth.TabIndex = 4;
            this.OneMonth.Text = "本月";
            this.OneMonth.UseVisualStyleBackColor = true;
            this.OneMonth.Click += new System.EventHandler(this.OneMonth_Click);
            // 
            // OneDay
            // 
            this.OneDay.Location = new System.Drawing.Point(251, 328);
            this.OneDay.Name = "OneDay";
            this.OneDay.Size = new System.Drawing.Size(61, 27);
            this.OneDay.TabIndex = 5;
            this.OneDay.Text = "本日";
            this.OneDay.UseVisualStyleBackColor = true;
            this.OneDay.Click += new System.EventHandler(this.OneDay_Click);
            // 
            // Home
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1356, 741);
            this.Controls.Add(this.OneDay);
            this.Controls.Add(this.OneMonth);
            this.Controls.Add(this.OneWeek);
            this.Controls.Add(this.chart1);
            this.Name = "Home";
            this.Text = "Home";
            this.Load += new System.EventHandler(this.Home_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Button OneWeek;
        private System.Windows.Forms.Button OneMonth;
        private System.Windows.Forms.Button OneDay;
    }
}