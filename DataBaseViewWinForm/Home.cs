using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using DataBaseViewWinForm.DataBaseConnect;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.WinForms;
using LiveCharts.Wpf;

namespace DataBaseViewWinForm
{
    public partial class Home : Form
    {
        public static string DBName = "secote_PartNumber";
        public static string connectString = DatabaseFactory.DatabaseNameSwitch(DBName);

        public LiveCharts.WinForms.CartesianChart CartesianChart { get; set; }
        public System.Windows.Forms.DataVisualization.Charting.SeriesCollection SeriesCollection { get; set; }
        public Home()
        {
            InitializeComponent();



        }

        private void Home_Load(object sender, EventArgs e)
        {
            OneDay_Click(sender, e);
        }

        #region 当天生产数量
        //当天按钮
        private void OneDay_Click(object sender, EventArgs e)
        {
            chart1.Width = 800;


            // 获取图表区域
            ChartArea chartArea = chart1.ChartAreas[0];

            // 隐藏X轴网格线
            chartArea.AxisX.MajorGrid.Enabled = false;

            // 隐藏Y轴网格线
            chartArea.AxisY.MajorGrid.Enabled = false;

            LoadHourlyChart();
        }
        //加载表格方法
        private void LoadHourlyChart()
        {
            DateTime today = DateTime.Today;
            var yValues = GetHourlyData(); // 返回 12 个两小时段的数据（0-23 小时）

            // 1. 清空图表
            chart1.Series.Clear();
            chart1.Titles.Clear();

            // 2. 添加柱状图系列
            System.Windows.Forms.DataVisualization.Charting.Series series = new System.Windows.Forms.DataVisualization.Charting.Series("小时数据")
            {
                ChartType = SeriesChartType.Column,
                Color = Color.FromArgb(231, 76, 60),
                BorderWidth = 1,
                IsValueShownAsLabel = true
            };
            chart1.Series.Add(series);

            // 3. 按 “两小时段” 渲染（共 12 个数据点）
            for (int slot = 0; slot < 12; slot++)
            {
                int startHour = slot * 2; // 0,2,4...22 点
                DateTime timePoint = today.AddHours(startHour);
                double value = yValues[slot]; // 直接取两小时段的数据

                series.Points.AddXY(timePoint, value);
            }

            // 4. 设置 X 轴（两小时为间隔）
            ChartArea chartArea = chart1.ChartAreas[0];
            chartArea.AxisX.LabelStyle.Format = "HH:mm";
            chartArea.AxisX.IntervalType = DateTimeIntervalType.Hours;
            chartArea.AxisX.Interval = 2; // 与统计粒度对齐
            chartArea.AxisX.LabelStyle.Angle = 45;
            chartArea.AxisX.Title = "时间（24 小时制）";

            // 5. 设置 Y 轴
            chartArea.AxisY.Title = "数值";
            if (yValues.Any())
            {
                chartArea.AxisY.Minimum = 0;
                chartArea.AxisY.Maximum = yValues.Max() + 5;
            }

            // 6. 设置标题
            chart1.Titles.Add($"24 小时数据统计 - {today:yyyy年MM月dd日}");
        }

        // 将获取24小时数据处理成列表
        private List<int> GetHourlyData()
        {
            DateTime specificDate = new DateTime(2025, 6, 4);
            var dicValue = GetDataByTimeInterval(specificDate);
            var values = GetOrderedCountList(dicValue);
            // 示例：返回24小时的随机数据

            return values;
        }

        // 获取时间段对应的整数
        private int GetTimeSlot(string timeInterval)
        {
            try
            {
                // 提取日期时间部分（忽略时区等复杂情况）
                string dateTimePart = timeInterval.Split(' ')[0] + " " +
                                     timeInterval.Split(' ')[1].Split('-')[0];

                // 解析为DateTime并提取小时
                DateTime time = DateTime.ParseExact(
                    dateTimePart, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);

                int startHour = time.Hour;
                if (startHour >= 0 && startHour < 24)
                {
                    return startHour / 2;
                }
            }
            catch
            {
                // 忽略解析异常
            }

            return -1;
        }
        //从数据库中取出数据，存放到字典中
        public Dictionary<string, int> GetDataByTimeInterval(DateTime date)
        {
            var result = new Dictionary<string, int>();

            // 手动格式化日期字符串
            string startDate = date.ToString("yyyy-MM-dd 00:00:00");
            string endDate = date.AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss");

            // 构建SQL查询（直接嵌入日期值）
            string query = $@"
            SELECT
                STRFTIME('%Y-%m-%d', StartTime) || ' ' || 
                ((STRFTIME('%H', StartTime) / 2) * 2) || ':00-' ||
                (((STRFTIME('%H', StartTime) / 2) * 2) + 2) || ':00' AS TimeInterval,
                COUNT(*) AS DataCount
            FROM CompletedLayers
            WHERE StartTime BETWEEN '{startDate}' AND '{endDate}'
            GROUP BY STRFTIME('%Y-%m-%d', StartTime), (STRFTIME('%H', StartTime) / 2)
            ORDER BY MIN(StartTime)";

            using (var connection = new SQLiteConnection(connectString))
            using (var command = new SQLiteCommand(query, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string interval = reader.GetString(0);
                        int count = reader.GetInt32(1);
                        result[interval] = count;
                    }
                }
            }

            // 填充空时间段
            FillEmptyIntervals(result, date);
            return result;
        }
        //给未读取到数值的时间段赋值
        private void FillEmptyIntervals(Dictionary<string, int> result, DateTime date)
        {
            for (int hour = 0; hour < 24; hour += 2)
            {
                string intervalKey = $"{date:yyyy-MM-dd} {hour:D2}:00-{hour + 2:D2}:00";
                if (!result.ContainsKey(intervalKey))
                {
                    result[intervalKey] = 0;
                }
            }
        }

        // 从字典中按0-24小时顺序提取数量值
        public List<int> GetOrderedCountList(Dictionary<string, int> timeData)
        {
            var result = new List<int>(Enumerable.Repeat(0, 12)); // 12个两小时段（0-24小时）

            // 解析每个时间段并填充数据
            foreach (var item in timeData)
            {
                int timeSlot = GetTimeSlot(item.Key);
                if (timeSlot >= 0 && timeSlot < 12)
                {
                    result[timeSlot] = item.Value;

                }
            }

            return result;
        }


        #endregion





        // 模拟从列表获取Y轴数据（实际应用中替换为真实数据来源）
        private List<double> GetYValuesFromList()
        {
            // 示例：返回7天的随机数据
            
            List<double> values = new List<double>()
            {
                1000,3000,5000,7000,10000,12000,15000
            };

            return values;
        }

        public void ShowWeekChart()
        {
            chart1.Series.Clear();
            chart1.Titles[0].Text = $"一周数据统计 - {DateTime.Now:yyyy年MM月}";

            // 获取本周日期范围（周一至周日）
            DateTime today = DateTime.Today;
            DateTime startDate = today.AddDays(-(int)today.DayOfWeek + (today.DayOfWeek == DayOfWeek.Sunday ? 0 : 1));
            DateTime endDate = startDate.AddDays(6);

            // 创建数据系列
            System.Windows.Forms.DataVisualization.Charting.Series series = new System.Windows.Forms.DataVisualization.Charting.Series("日数据")
            {
                ChartType = SeriesChartType.Column,
                Color = System.Drawing.Color.FromArgb(52, 152, 219)
            };
            chart1.Series.Add(series);

            // 添加每天的数据点（模拟数据，实际应从数据库获取）
            for (DateTime day = startDate; day <= endDate; day = day.AddDays(1))
            {
                double value = GetDailyValue(day); // 从数据库获取当日值
                series.Points.AddXY(day, value);
            }

            // 设置X轴为日期（天）
            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "MM-dd";
            chart1.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Days;
            chart1.ChartAreas[0].AxisX.Interval = 1;
            chart1.ChartAreas[0].AxisX.Title = "日期";

            // 设置Y轴
            chart1.ChartAreas[0].AxisY.Title = "数值";
        }
        // 显示一月数据（X轴为周）
        public void ShowMonthChart()
        {
            chart1.Series.Clear();
            chart1.Titles[0].Text = $"一月数据统计 - {DateTime.Now:yyyy年MM月}";

            // 获取当月第一天和最后一天
            DateTime today = DateTime.Today;
            DateTime firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            // 创建数据系列
            System.Windows.Forms.DataVisualization.Charting.Series series = new System.Windows.Forms.DataVisualization.Charting.Series("周数据")
            {
                ChartType = SeriesChartType.Column,
                Color = System.Drawing.Color.FromArgb(231, 76, 60)
            };
            chart1.Series.Add(series);

            // 计算当月有多少周
            int weeksInMonth = (int)Math.Ceiling((lastDayOfMonth.Day + (int)firstDayOfMonth.DayOfWeek - 1) / 7.0);

            // 添加每周的数据点
            for (int week = 0; week < weeksInMonth; week++)
            {
                // 计算本周的日期范围
                DateTime weekStart = firstDayOfMonth.AddDays(week * 7);
                DateTime weekEnd = weekStart.AddDays(6);
                if (weekEnd > lastDayOfMonth) weekEnd = lastDayOfMonth;

                // 计算本周的值（模拟数据，实际应从数据库按周汇总）
                double value = GetWeeklyValue(weekStart, weekEnd);
                series.Points.AddXY(weekStart, value);

                // 设置X轴标签为周范围
                series.Points[week].AxisLabel = $"第{week + 1}周\n{weekStart:MM-dd}~{weekEnd:MM-dd}";
            }

            // 设置X轴为周
            chart1.ChartAreas[0].AxisX.Interval = 1;
            chart1.ChartAreas[0].AxisX.Title = "周次";

            // 设置Y轴
            chart1.ChartAreas[0].AxisY.Title = "数值";
        }

        // 模拟从数据库获取每日数据（实际应用中替换为真实数据库查询）
        private double GetDailyValue(DateTime date)
        {
            // 示例：返回随机值或从数据库查询
            return new Random(date.GetHashCode()).Next(10, 100);
        }

        // 模拟从数据库获取每周数据（实际应用中替换为真实数据库查询）
        private double GetWeeklyValue(DateTime startDate, DateTime endDate)
        {
            // 示例：返回随机值或从数据库按日期范围汇总
            return new Random(startDate.GetHashCode()).Next(50, 200);
        }

        private void OneWeek_Click(object sender, EventArgs e)
        {
            ShowWeekChart();
        }

        private void OneMonth_Click(object sender, EventArgs e)
        {
            ShowMonthChart();
        }
    }
}
