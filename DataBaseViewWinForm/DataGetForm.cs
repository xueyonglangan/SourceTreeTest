using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using DataBaseViewWinForm.DataBaseConnect;
using DataBaseViewWinForm.DataModel;
using DataBaseViewWinForm.Tool;



namespace DataBaseViewWinForm
{
    public partial class DataGetForm : Form
    {
        public string DataBaseWayFilePath = "C:\\Program Files\\DataBaseWays.ini";
        public static int PageMode = 0;
        
        // 分页相关变量
        private int _currentPage = 1;
        private int _pageSize = 22;
        private int _totalRecords = 0;
        private int _totalPages = 0;
        public DataGetForm()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            
            
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            DateTimeHourExample();
            //初始化时获取数据库保存的路径
            DatabaseFactory.dataBaseWay = CurrentDataBaseWay.Text;

            LoadFromINIFile();
            CurrentDataBaseWay.Text = DatabaseFactory.dataBaseWay;
            this.Text = "生产数据查询助手   当前数据库路径：" + DatabaseFactory.dataBaseWay;
        }

        #region 功能封装方法
        //查询时间段内对应料号的所有生产数据
        public void SearchProductDetail1()
        {
            // 清空表格
            dataGridView1.DataSource = null;
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();

            var startTime = StartTime1.Value.ToString("yyyy-MM-dd HH:mm:ss").Replace("'", "''");
            var endTime = EndTime1.Value.AddSeconds(1).ToString("yyyy-MM-dd HH:mm:ss").Replace("'", "''");
            var productName = ProductNameText.Text.Replace("'", "''"); // 转义单引号

            // 校验料号输入
            if (string.IsNullOrEmpty(productName))
            {
                MessageBox.Show("请输入料号！");
                return;
            }

            // 构建带条件的分页查询SQL
            string sql = string.Format(@"
        SELECT * FROM CompletedLayers 
        WHERE PartNumber = '{0}' 
          AND StartTime BETWEEN '{1}' AND '{2}' 
        ORDER BY StartTime ASC",
                productName, startTime, endTime
            );

            try
            {
                var DBname = CurrentDataBaseName.Text;
                var connectstring = DatabaseFactory.DatabaseNameSwitch(DBname);

                using (var connection = new SqliteDatabaseConnection(connectstring))
                {
                    connection.Open();
                    // 执行分页查询（获取当前页数据和总记录数）
                    var result = connection.QueryWithPaging<CompletedLayers>(sql, _currentPage, _pageSize);
                    var data = result.Item1;
                    _totalRecords = result.Item2;
                    _totalPages = (int)Math.Ceiling((double)_totalRecords / _pageSize);

                    if (data != null && data.Count > 0)
                    {
                        dataGridView1.AutoGenerateColumns = true;
                        dataGridView1.Columns.Clear();
                        dataGridView1.DataSource = data;

                        // 添加序号列并设置连续序号
                        AddContinuousRowNumber();

                        // 更新分页控件状态
                        UpdatePaginationControls();
                    }
                    else
                    {
                        MessageBox.Show("未查询到数据");
                        dataGridView1.DataSource = null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"错误：{ex.Message}");
            }
        }
        public void SearchProductDetail()
        {
            var startTime = StartTime1.Value.ToString("yyyy-MM-dd HH:mm:ss").Replace("'", "''");
            var endTime = EndTime1.Value.AddSeconds(1).ToString("yyyy-MM-dd HH:mm:ss").Replace("'", "''");
            var productName = ProductNameText.Text.Replace("'", "''");

            // 构建带排序的SQL（分页必须有稳定排序）
            var sql = string.Format(@"
        SELECT * FROM CompletedLayers 
        WHERE PartNumber = '{0}' AND StartTime BETWEEN '{1}' AND '{2}'
        ORDER BY StartTime ASC", // 确保排序稳定
                productName, startTime, endTime
            );

            try
            {
                var DBname = CurrentDataBaseName.Text;
                var connectstring = DatabaseFactory.DatabaseNameSwitch(DBname);

                using (var connection = new SqliteDatabaseConnection(connectstring))
                {
                    connection.Open();
                    var result = connection.QueryWithPaging<CompletedLayers>(sql, _currentPage, _pageSize);
                    var data = result.Item1;
                    _totalRecords = result.Item2;
                    _totalPages = (int)Math.Ceiling((double)_totalRecords / _pageSize);

                    if (data != null && data.Count > 0)
                    {
                        dataGridView1.AutoGenerateColumns = true;
                        dataGridView1.Columns.Clear();
                        dataGridView1.DataSource = data;

                        // 添加连续序号
                        AddContinuousRowNumber();
                    }
                    else
                    {
                        MessageBox.Show("未查询到数据");
                        dataGridView1.DataSource = null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"错误：{ex.Message}");
            }

            // 更新分页控件状态（关键：确保每次查询后都更新）
            UpdatePaginationControls();
        }

        //输出DataGridView中的数据到Excel表中
        private void ExportToCSV()
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "CSV文件 (*.csv)|*.csv";
            saveDialog.Title = "保存CSV文件";



            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter writer = new StreamWriter(saveDialog.FileName, false, Encoding.UTF8))
                {

                    // 导出表头
                    string[] chineseHeader = {"ID","开始时间","结束时间","料号","任务ID", "曝光台面",
                        "曝光模式", "曝光A面/B面", "层名称", "生产总数", "干膜类型","扫描能量","扫描速度",
                        "板厚","板宽","板高","图高","图宽","X涨缩","Y涨缩","X自由涨缩","Y自由涨缩","拼板数量","用户名"};

                    string header = "";
                    for (int i = 0; i < dataGridView1.Columns.Count; i++)
                    {

                        dataGridView1.Columns[i].HeaderText = chineseHeader[i];
                        header += dataGridView1.Columns[i].HeaderText + ",";
                    }
                    writer.WriteLine(header.TrimEnd(','));

                    // 导出数据
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        string line = "";
                        for (int j = 0; j < dataGridView1.Columns.Count; j++)
                        {
                            if (dataGridView1.Rows[i].Cells[j].Value != null)
                            {
                                // 处理包含逗号或引号的值
                                string value = dataGridView1.Rows[i].Cells[j].Value.ToString();
                                if (value.Contains(",") || value.Contains("\""))
                                {
                                    value = "\"" + value.Replace("\"", "\"\"") + "\"";
                                }
                                line += value + ",";
                            }
                            else
                            {
                                line += ",";
                            }
                        }
                        writer.WriteLine(line.TrimEnd(','));
                    }
                }

                MessageBox.Show("导出成功！");
            }
        }
        //打印时间段内的料号
        private void ExportToCSV1()
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "CSV文件 (*.csv)|*.csv";
            saveDialog.Title = "保存CSV文件";



            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter writer = new StreamWriter(saveDialog.FileName, false, Encoding.UTF8))
                {

                    // 导出表头
                    string[] chineseHeader = {"序号","用户名","料号","曝光A面总数","曝光B面总数", "拼板A面总数", "拼板B面总数", "干膜名称","干膜类型",
                        "板厚","板宽","板高","图宽","图高","x涨缩","y涨缩","扫描速度","扫描能量","开始时间" };

                    string header = "";
                    for (int i = 0; i < dataGridView1.Columns.Count; i++)
                    {

                        dataGridView1.Columns[i].HeaderText = chineseHeader[i];
                        header += dataGridView1.Columns[i].HeaderText + ",";
                    }
                    writer.WriteLine(header.TrimEnd(','));

                    // 导出数据
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        string line = "";
                        for (int j = 0; j < dataGridView1.Columns.Count; j++)
                        {
                            if (dataGridView1.Rows[i].Cells[j].Value != null)
                            {
                                // 处理包含逗号或引号的值
                                string value = dataGridView1.Rows[i].Cells[j].Value.ToString();
                                if (value.Contains(",") || value.Contains("\""))
                                {
                                    value = "\"" + value.Replace("\"", "\"\"") + "\"";
                                }
                                line += value + ",";
                            }
                            else
                            {
                                line += ",";
                            }
                        }
                        writer.WriteLine(line.TrimEnd(','));
                    }
                }

                MessageBox.Show("导出成功！");
            }

        }

        //时间控件自定义，准确到小时
        public void DateTimeHourExample()
        {
            StartTime1.Format = DateTimePickerFormat.Custom;  // 启用自定义格式
            StartTime1.CustomFormat = "yyyy-MM-dd HH:mm:ss";  // 年月日时格式
            StartTime1.ShowUpDown = true;    // 使用上下按钮选择时间


            EndTime1.Format = DateTimePickerFormat.Custom;  // 启用自定义格式
            EndTime1.CustomFormat = "yyyy-MM-dd HH:mm:ss";  // 年月日时格式
            EndTime1.ShowUpDown = true;    // 使用上下按钮选择时间
        }


        #endregion

        #region 界面主要按钮
        
        //查询时间段内料号按钮，分页查询      
        private void Search_Click(object sender, EventArgs e)
        {
            PageMode = 0;
            _currentPage = 1;
            LoadDataWithPaging();
        }
        //查看详细数据按钮
        private void SearchDetailDataBtn_Click(object sender, EventArgs e)
        {
            PageMode = 1; // 设置为具体料号查询模式
            _currentPage = 1; // 重置为第一页
            _totalRecords = 0; // 重置总记录数
            _totalPages = 0; // 重置总页数

            // 清空表格
            dataGridView1.DataSource = null;
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();

            var DBName = CurrentDataBaseName.Text;
            var partName = ProductNameText.Text;
            var connectionString = DatabaseFactory.DatabaseNameSwitch(DBName);

            // 判断料号栏是否为空
            if (string.IsNullOrWhiteSpace(ProductNameText.Text))
            {
                MessageBox.Show("请正确输入您要查询的料号！");
                return;
            }

            // 执行带分页的查询
            SearchProductDetail();

            // 强制更新分页控件状态
            UpdatePaginationControls();
        }

        //打印对应料号所有数据按钮
        private void ExportData_Click(object sender, EventArgs e)
        {
            //清空表格
            // 清空 DataGridView
            dataGridView1.DataSource = null;
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();

            var DBName = CurrentDataBaseName.Text;
            var partName = ProductNameText.Text;

            var connectionString = DatabaseFactory.DatabaseNameSwitch(DBName);

            var sql = $"SELECT * FROM CompletedLayers WHERE PartNumber = '{partName}'";
            using (var _connection = new SqliteDatabaseConnection(connectionString))
            {
                _connection.Open();
                var data = _connection.QueryAll<CompletedLayers>(sql);

                if (data.Any())
                {
                    dataGridView1.DataSource = data;
                }
                else
                {
                    MessageBox.Show("未查询到数据");
                    dataGridView1.DataSource = null; // 清空网格
                }

            }
            //
            ExportToCSV();
        }

        //打印时间段内的料号数据按钮
        private void ExportDataBtn1_Click(object sender, EventArgs e)
        {
            //清空表格
            // 清空 DataGridView
            dataGridView1.DataSource = null;
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();



            ////查询当前时间段里面的所有料号
            var startTime = StartTime1.Value.ToString("yyyy-MM-dd HH:mm:ss");
            var endTime = EndTime1.Value.AddSeconds(1).ToString("yyyy-MM-dd HH:mm:ss");

            var SearchProductNameSql = $"SELECT DISTINCT PartNumber FROM CompletedLayers WHERE DATE(StartTime) BETWEEN '{startTime}' AND '{endTime}'";



            string sqlChange = $@"
            SELECT 
                UserName,
                PartNumber,
                SUM(CASE WHEN ExpSide = 'ExposureSide_A' THEN 1 ELSE 0 END) AS ExposureACount,
                SUM(CASE WHEN ExpSide = 'ExposureSide_B' THEN 1 ELSE 0 END) AS ExposureBCount,
                SUM(CASE WHEN ExpSide = 'ExposureSide_A' THEN MakeupCount ELSE 0 END) AS MakeupACount,
                SUM(CASE WHEN ExpSide = 'ExposureSide_B' THEN MakeupCount ELSE 0 END) AS MakeupBCount,
                -- 新增字段：取第一条记录的值（假设同一料号这些值相同）
                MAX(LayerName) AS LayerName,
                MAX(SetupFilm) AS SetupFilm,
                MAX(BoardThickness) AS BoardThickness,
                MAX(BoardWidth) AS BoardWidth,
                MAX(BoardHeight) AS BoardHeight,
                MAX(ImageWidth) AS ImageWidth,
                MAX(ImageHeight) AS ImageHeight,
                MAX(ScaleFactorX) AS ScaleFactorX,
                MAX(ScaleFactorY) AS ScaleFactorY,
                MAX(ScanSpeed) AS ScanSpeed,
                MAX(SnsEnergy) AS SnsEnergy,
                MIN(StartTime) AS StartTime
            FROM 
                CompletedLayers 
            WHERE 
                StartTime BETWEEN '{startTime}' AND '{endTime}' 
            GROUP BY 
                PartNumber
            ORDER BY 
                CAST(strftime('%s', MIN(StartTime)) AS INTEGER) ASC;
        "; try
            {
                var DBname = CurrentDataBaseName.Text;
                var connectstring = DatabaseFactory.DatabaseNameSwitch(DBname);

                using (var connection = new SQLiteConnection(connectstring))
                {
                    connection.Open();
                    using (var command = new SQLiteCommand(sqlChange, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        var data = new List<MergedPartNumberInfo>();

                        while (reader.Read())
                        {
                            data.Add(new MergedPartNumberInfo
                            {
                                UserName = reader.GetString(reader.GetOrdinal("UserName")),
                                PartNumber = reader.GetString(reader.GetOrdinal("PartNumber")),
                                ExposureACount = reader.IsDBNull(reader.GetOrdinal("ExposureACount")) ? 0 : reader.GetInt32(reader.GetOrdinal("ExposureACount")),
                                ExposureBCount = reader.IsDBNull(reader.GetOrdinal("ExposureBCount")) ? 0 : reader.GetInt32(reader.GetOrdinal("ExposureBCount")),
                                MakeupACount = reader.IsDBNull(reader.GetOrdinal("MakeupACount")) ? 0 : reader.GetInt32(reader.GetOrdinal("MakeupACount")),
                                MakeupBCount = reader.IsDBNull(reader.GetOrdinal("MakeupBCount")) ? 0 : reader.GetInt32(reader.GetOrdinal("MakeupBCount")),
                                LayerName = reader.IsDBNull(reader.GetOrdinal("LayerName")) ? "" : reader.GetString(reader.GetOrdinal("LayerName")),
                                SetupFilm = reader.IsDBNull(reader.GetOrdinal("SetupFilm")) ? "" : reader.GetString(reader.GetOrdinal("SetupFilm")),
                                BoardThickness = reader.IsDBNull(reader.GetOrdinal("BoardThickness")) ? 0 : reader.GetDouble(reader.GetOrdinal("BoardThickness")),
                                BoardWidth = reader.IsDBNull(reader.GetOrdinal("BoardWidth")) ? 0 : reader.GetDouble(reader.GetOrdinal("BoardWidth")),
                                BoardHeight = reader.IsDBNull(reader.GetOrdinal("BoardHeight")) ? 0 : reader.GetDouble(reader.GetOrdinal("BoardHeight")),
                                ImageWidth = reader.IsDBNull(reader.GetOrdinal("ImageWidth")) ? 0 : reader.GetDouble(reader.GetOrdinal("ImageWidth")),
                                ImageHeight = reader.IsDBNull(reader.GetOrdinal("ImageHeight")) ? 0 : reader.GetDouble(reader.GetOrdinal("ImageHeight")),
                                ScaleFactorX = reader.IsDBNull(reader.GetOrdinal("ScaleFactorX")) ? 0 : reader.GetDouble(reader.GetOrdinal("ScaleFactorX")),
                                ScaleFactorY = reader.IsDBNull(reader.GetOrdinal("ScaleFactorY")) ? 0 : reader.GetDouble(reader.GetOrdinal("ScaleFactorY")),
                                ScanSpeed = reader.IsDBNull(reader.GetOrdinal("ScanSpeed")) ? 0 : reader.GetDouble(reader.GetOrdinal("ScanSpeed")),
                                SnsEnergy = reader.IsDBNull(reader.GetOrdinal("SnsEnergy")) ? 0 : reader.GetDouble(reader.GetOrdinal("SnsEnergy")),
                                StartTime = reader.GetString(reader.GetOrdinal("StartTime"))
                            });
                        }

                        if (data.Any())
                        {
                            // 配置DataGridView列
                            dataGridView1.AutoGenerateColumns = false;
                            dataGridView1.Columns.Clear();
                            //序号
                            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
                            {
                                Name = "RowNumber",
                                HeaderText = "序号",
                                Width = 60,
                                ReadOnly = true
                            });

                            // 2. 用户列
                            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
                            {
                                Name = "UserName",
                                HeaderText = "用户",
                                DataPropertyName = "UserName",
                                Width = 100
                            });
                            // 料号列
                            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
                            {
                                Name = "PartNumber",
                                HeaderText = "料号",
                                DataPropertyName = "PartNumber",
                                Width = 350
                            });

                            // 曝光A面数量列
                            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
                            {
                                Name = "ExposureACount",
                                HeaderText = "曝光A面数量",
                                DataPropertyName = "ExposureACount",
                                Width = 120
                            });

                            // 曝光B面数量列
                            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
                            {
                                Name = "ExposureBCount",
                                HeaderText = "曝光B面数量",
                                DataPropertyName = "ExposureBCount",
                                Width = 120
                            });

                            // 拼板A面数量列
                            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
                            {
                                Name = "MakeupACount",
                                HeaderText = "拼板A面数量",
                                DataPropertyName = "MakeupACount",
                                Width = 120
                            });

                            // 拼板B面数量列
                            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
                            {
                                Name = "MakeupBCount",
                                HeaderText = "拼板B面数量",
                                DataPropertyName = "MakeupBCount",
                                Width = 120
                            });
                            // 层名称
                            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
                            {
                                Name = "LayerName",
                                HeaderText = "层名称",
                                DataPropertyName = "LayerName",
                                Width = 120
                            });
                            // 层名称干膜类型板厚板宽板高图宽x涨缩y涨缩扫描速度扫描能量开始时间
                            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
                            {
                                Name = "SetupFilm",
                                HeaderText = "干膜类型",
                                DataPropertyName = "SetupFilm",
                                Width = 120
                            });
                            // 板厚
                            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
                            {
                                Name = "BoardThickness",
                                HeaderText = "板厚",
                                DataPropertyName = "BoardThickness",
                                Width = 120
                            });
                            // 板宽
                            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
                            {
                                Name = "BoardWidth",
                                HeaderText = "板宽",
                                DataPropertyName = "BoardWidth",
                                Width = 120
                            });
                            // 板高
                            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
                            {
                                Name = "BoardHeight",
                                HeaderText = "板高",
                                DataPropertyName = "BoardHeight",
                                Width = 120
                            });
                            // 图宽
                            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
                            {
                                Name = "ImageWidth",
                                HeaderText = "图宽",
                                DataPropertyName = "ImageWidth",
                                Width = 120
                            });
                            // 图宽
                            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
                            {
                                Name = "ImageHeight",
                                HeaderText = "图高",
                                DataPropertyName = "ImageHeight",
                                Width = 120
                            });

                            // x涨缩
                            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
                            {
                                Name = "ScaleFactorX",
                                HeaderText = "x涨缩",
                                DataPropertyName = "ScaleFactorX",
                                Width = 120
                            });
                            // y涨缩
                            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
                            {
                                Name = "ScaleFactorY",
                                HeaderText = "y涨缩",
                                DataPropertyName = "ScaleFactorY",
                                Width = 120
                            });
                            // 扫描速度
                            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
                            {
                                Name = "ScanSpeed",
                                HeaderText = "扫描速度",
                                DataPropertyName = "ScanSpeed",
                                Width = 120
                            });
                            // 扫描能量
                            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
                            {
                                Name = "SnsEnergy",
                                HeaderText = "扫描能量",
                                DataPropertyName = "SnsEnergy",
                                Width = 120
                            });

                            // 开始时间列
                            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
                            {
                                Name = "StartTime",
                                HeaderText = "开始时间",
                                DataPropertyName = "StartTime",
                                Width = 150
                            });


                            dataGridView1.DataSource = data;


                            int totalRows = dataGridView1.Rows.Count;
                            if (dataGridView1.AllowUserToAddRows)
                                totalRows--;

                            for (int i = 0; i < dataGridView1.Rows.Count; i++)
                            {
                                if (!dataGridView1.Rows[i].IsNewRow)
                                {
                                    dataGridView1.Rows[i].Cells["RowNumber"].Value = i + 1;
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("未查询到数据");
                            dataGridView1.DataSource = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"错误：{ex.Message}");
                Console.WriteLine(ex.ToString());
            }            //RefreshSearchDataGrid();


            ExportToCSV1();
        }




        #endregion

        #region  分页查询相关方法

        private void LoadDataWithPaging()
        {
            try
            {
                // 手动转义单引号，防止SQL注入
                string startTime = StartTime1.Value.ToString("yyyy-MM-dd HH:mm:ss").Replace("'", "''");
                string endTime = EndTime1.Value.AddSeconds(1).ToString("yyyy-MM-dd HH:mm:ss").Replace("'", "''");

                // 构建SQL查询（直接拼接参数值）
                string sql = string.Format(@"
                SELECT 
                    PartNumber,
                    UserName,
                    SUM(CASE WHEN ExpSide = 'ExposureSide_A' THEN 1 ELSE 0 END) AS ExposureACount,
                    SUM(CASE WHEN ExpSide = 'ExposureSide_B' THEN 1 ELSE 0 END) AS ExposureBCount,
                    SUM(CASE WHEN ExpSide = 'ExposureSide_A' THEN MakeupCount ELSE 0 END) AS MakeupACount,
                    SUM(CASE WHEN ExpSide = 'ExposureSide_B' THEN MakeupCount ELSE 0 END) AS MakeupBCount,
                    MAX(LayerName) AS LayerName,
                    MAX(SetupFilm) AS SetupFilm,
                    MAX(BoardThickness) AS BoardThickness,
                    MAX(BoardWidth) AS BoardWidth,
                    MAX(BoardHeight) AS BoardHeight,
                    MAX(ImageWidth) AS ImageWidth,
                    MAX(ImageWidth) AS ImageHeight,
                    MAX(ScaleFactorX) AS ScaleFactorX,
                    MAX(ScaleFactorY) AS ScaleFactorY,
                    MAX(ScanSpeed) AS ScanSpeed,
                    MAX(SnsEnergy) AS SnsEnergy,
                    MIN(StartTime) AS StartTime
                FROM 
                    CompletedLayers 
                WHERE 
                    StartTime BETWEEN '{0}' AND '{1}' 
                GROUP BY 
                    PartNumber
                ORDER BY 
                    CAST(strftime('%s', MIN(StartTime)) AS INTEGER) ASC",
                    startTime,
                    endTime
                );

                var DBname = CurrentDataBaseName.Text;
                var connectstring = DatabaseFactory.DatabaseNameSwitch(DBname);

                using (var connection = new SqliteDatabaseConnection(connectstring))
                {
                    connection.Open();

                    // 执行分页查询
                    var result = connection.QueryWithPaging<MergedPartNumberInfo>(sql, _currentPage, _pageSize);
                    var data = result.Item1;
                    _totalRecords = result.Item2;
                    _totalPages = (int)Math.Ceiling((double)_totalRecords / _pageSize);

                    if (data != null && data.Count > 0)
                    {
                        dataGridView1.AutoGenerateColumns = false;
                        dataGridView1.Columns.Clear();

                        // 设置列定义（保持原有配置）
                        SetupDataGridViewColumns();

                        // 设置数据源
                        dataGridView1.DataSource = data;

                        // 调用通用方法添加连续序号
                        AddContinuousRowNumber();

                        // 更新分页导航控件状态
                        UpdatePaginationControls();
                    }
                    else
                    {
                        MessageBox.Show("未查询到数据");
                        dataGridView1.DataSource = null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误: " + ex.Message);
                Console.WriteLine(ex.ToString());
            }
        }
        private void UpdatePaginationControls()
        {
            // 显示当前分页信息
            lblPaginationInfo.Text = string.Format(
                "第 {0} 页，共 {1} 页，总记录：{2} 条",
                _currentPage,
                _totalPages,
                _totalRecords
            );

            // 控制按钮状态（处理总页数为0的特殊情况）
            bool hasData = _totalRecords > 0 && _totalPages > 0;
            btnFirstPage.Enabled = hasData && _currentPage > 1;
            btnPrevPage.Enabled = hasData && _currentPage > 1;
            btnNextPage.Enabled = hasData && _currentPage < _totalPages;
            btnLastPage.Enabled = hasData && _currentPage < _totalPages;
        }
        //手动为表格添加列
        private void SetupDataGridViewColumns()
        {
            // 配置DataGridView列（保持原有配置）
            //序号
            //dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            //{
            //    Name = "RowNumber",
            //    HeaderText = "序号",
            //    Width = 60,
            //    ReadOnly = true
            //});

            // 2. 用户列
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "UserName",
                HeaderText = "用户",
                DataPropertyName = "UserName",
                Width = 100
            });
            // 料号列
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "PartNumber",
                HeaderText = "料号",
                DataPropertyName = "PartNumber",
                Width = 350
            });

            // 曝光A面数量列
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ExposureACount",
                HeaderText = "曝光A面数量",
                DataPropertyName = "ExposureACount",
                Width = 120
            });

            // 曝光B面数量列
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ExposureBCount",
                HeaderText = "曝光B面数量",
                DataPropertyName = "ExposureBCount",
                Width = 120
            });

            // 拼板A面数量列
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MakeupACount",
                HeaderText = "拼板A面数量",
                DataPropertyName = "MakeupACount",
                Width = 120
            });

            // 拼板B面数量列
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MakeupBCount",
                HeaderText = "拼板B面数量",
                DataPropertyName = "MakeupBCount",
                Width = 120
            });
            // 层名称
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "LayerName",
                HeaderText = "层名称",
                DataPropertyName = "LayerName",
                Width = 120
            });
            // 层名称干膜类型板厚板宽板高图宽x涨缩y涨缩扫描速度扫描能量开始时间
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "SetupFilm",
                HeaderText = "干膜类型",
                DataPropertyName = "SetupFilm",
                Width = 120
            });
            // 板厚
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "BoardThickness",
                HeaderText = "板厚",
                DataPropertyName = "BoardThickness",
                Width = 120
            });
            // 板宽
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "BoardWidth",
                HeaderText = "板宽",
                DataPropertyName = "BoardWidth",
                Width = 120
            });
            // 板高
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "BoardHeight",
                HeaderText = "板高",
                DataPropertyName = "BoardHeight",
                Width = 120
            });
            // 图宽
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ImageWidth",
                HeaderText = "图宽",
                DataPropertyName = "ImageWidth",
                Width = 120
            });
            // 图宽
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ImageHeight",
                HeaderText = "图高",
                DataPropertyName = "ImageHeight",
                Width = 120
            });

            // x涨缩
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ScaleFactorX",
                HeaderText = "x涨缩",
                DataPropertyName = "ScaleFactorX",
                Width = 120
            });
            // y涨缩
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ScaleFactorY",
                HeaderText = "y涨缩",
                DataPropertyName = "ScaleFactorY",
                Width = 120
            });
            // 扫描速度
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ScanSpeed",
                HeaderText = "扫描速度",
                DataPropertyName = "ScanSpeed",
                Width = 120
            });
            // 扫描能量
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "SnsEnergy",
                HeaderText = "扫描能量",
                DataPropertyName = "SnsEnergy",
                Width = 120
            });

            // 开始时间列
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "StartTime",
                HeaderText = "开始时间",
                DataPropertyName = "StartTime",
                Width = 150
            });


        }

        /// <summary>
        /// 为当前页数据添加连续序号（跨页连续）
        /// </summary>
        private void AddContinuousRowNumber()
        {
            // 检查是否已存在序号列，若存在则移除（避免重复）
            if (dataGridView1.Columns.Contains("RowNumber"))
            {
                dataGridView1.Columns.Remove("RowNumber");
            }

            // 添加序号列（作为第一列）
            DataGridViewTextBoxColumn rowNumberColumn = new DataGridViewTextBoxColumn
            {
                Name = "RowNumber",
                HeaderText = "序号",
                Width = 60,
                ReadOnly = true
            };
            dataGridView1.Columns.Insert(0, rowNumberColumn); // 插入到第一列

            // 计算连续序号（偏移量 + 行索引 + 1）
            int offset = (_currentPage - 1) * _pageSize;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (!dataGridView1.Rows[i].IsNewRow)
                {
                    dataGridView1.Rows[i].Cells["RowNumber"].Value = offset + i + 1;
                }
            }
        }
        #endregion

        #region 分页相关按钮
        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage = 1;
                if (PageMode == 0)
                {
                    LoadDataWithPaging();
                }
                else if (PageMode == 1)
                {
                    SearchProductDetail();
                }
                UpdatePaginationControls();
            }
        }

        private void btnPrevPage_Click(object sender, EventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                // 根据当前模式执行对应查询
                if (PageMode == 0)
                {
                    LoadDataWithPaging();
                }
                else if (PageMode == 1)
                {
                    SearchProductDetail();
                }
                // 强制刷新分页状态
                UpdatePaginationControls();
            }
        }

        private void btnNextPage_Click(object sender, EventArgs e)
        {
            // 先检查总页数是否有效
            if (_totalPages > 0 && _currentPage < _totalPages)
            {
                _currentPage++;
                if (PageMode == 0)
                {
                    LoadDataWithPaging();
                }
                else if (PageMode == 1)
                {
                    SearchProductDetail();
                }
                // 强制刷新分页状态
                UpdatePaginationControls();
            }
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            if (_totalPages > 0 && _currentPage < _totalPages)
            {
                _currentPage = _totalPages;
                if (PageMode == 0)
                {
                    LoadDataWithPaging();
                }
                else if (PageMode == 1)
                {
                    SearchProductDetail();
                }
                UpdatePaginationControls();
            }
        }
        #endregion

        #region 数据库路径
        //自定义数据库路径
        private void DataBaseWayBtn_Click(object sender, EventArgs e)
        {
            IniFile iniFile = new IniFile(DataBaseWayFilePath);
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                // 配置对话框属性
                openFileDialog.Filter = "所有文件(*.*)|*.*"; // 文件筛选器，可根据需求修改
                openFileDialog.Title = "选择文件";
                openFileDialog.Multiselect = false; // 禁止多选

                // 显示对话框并获取用户操作结果
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // 用户点击了"确定"按钮，保存文件路径到变量
                    string PathKey = openFileDialog.FileName;
                    iniFile.Write("Path", "DataBasePath", PathKey);
                    DatabaseFactory.dataBaseWay = PathKey;

                    //SaveToTextFile();
                    

                    // 后续可使用selectedFilePath变量进行文件操作
                    MessageBox.Show($"已选择文件：{DatabaseFactory.dataBaseWay}", "提示");
                }
                this.Text = "生产数据查询助手   当前数据库路径：" + DatabaseFactory.dataBaseWay;

            }
        }
        //查看当前数据库路径
        private void ShowDataBaseWay_Click(object sender, EventArgs e)
        {
            MessageBox.Show("当前路径为：" + DatabaseFactory.dataBaseWay);
        }

        
        //从TXT中读数据库路径
        private void LoadFromINIFile()
        {
            IniFile iniFile = new IniFile(DataBaseWayFilePath);
            try
            {
                if (File.Exists("D:\\Workspace\\secote_ProductionData.db"))
                {
                    DatabaseFactory.dataBaseWay = "D:\\Workspace\\secote_ProductionData.db";
                    iniFile.Write("Path", "DataBasePath", DatabaseFactory.dataBaseWay);
                }
                else if(File.Exists("C:\\Program Files\\SecoteSoftware\\SecoteMC\\SQL\\secote_PartNumber.db"))
                {
                    DatabaseFactory.dataBaseWay = "C:\\Program Files\\SecoteSoftware\\SecoteMC\\SQL\\secote_PartNumber.db";
                    iniFile.Write("Path", "DataBasePath", DatabaseFactory.dataBaseWay);

                }
                else 
                {
                    //加载ini文件中的路径
                    if(File.Exists("C:\\Program Files\\DataBaseWays.ini")&& iniFile.Read("Path", "DataBasePath") != "")
                    {
                            DatabaseFactory.dataBaseWay = iniFile.Read("Path", "DataBasePath");

                    }
                    else
                    {
                        MessageBox.Show("未指定数据库路径！请正确选择！");
                        using (OpenFileDialog openFileDialog = new OpenFileDialog())
                        {
                            // 配置对话框属性
                            openFileDialog.Filter = "所有文件(*.*)|*.*"; // 文件筛选器，可根据需求修改
                            openFileDialog.Title = "选择文件";
                            openFileDialog.Multiselect = false; // 禁止多选

                            DialogResult result = openFileDialog.ShowDialog();
                            // 显示对话框并获取用户操作结果
                            if (result == DialogResult.OK)
                            {
                                // 用户点击了"确定"按钮，保存文件路径到变量
                                string PathKey = openFileDialog.FileName;
                                iniFile.Write("Path", "DataBasePath", PathKey);
                                DatabaseFactory.dataBaseWay = PathKey;

                                //SaveToTextFile();


                                // 后续可使用selectedFilePath变量进行文件操作
                                MessageBox.Show($"已选择文件：{DatabaseFactory.dataBaseWay}", "提示");
                            }
                            else if (result == DialogResult.Cancel)
                            {
                                Application.Exit();
                            }

                        }
                    }

                }




            }
            catch (Exception ex)
            {
                MessageBox.Show($"读取失败: {ex.Message}");
            }
        }


        #endregion

        
    }
}
