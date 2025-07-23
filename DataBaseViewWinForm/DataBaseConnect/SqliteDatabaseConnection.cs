using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.Security.Cryptography;
using System.Data.SqlClient;
using System.Windows.Documents;
using System.Data;

namespace DataBaseViewWinForm.DataBaseConnect
{
    public class SqliteDatabaseConnection : IDatabaseConnection
    {
        //连接字符串
        private readonly string _connectionstring;
        //连接对象
        private SQLiteConnection _connection;

        public SqliteDatabaseConnection(string connectionstring)
        {
            this._connectionstring = connectionstring;
        }

        public DbConnection Connection
        {
            get
            {
                return _connection;
            }
        }

        public bool Open()
        {
            try
            {
                _connection = new SQLiteConnection(_connectionstring);
                _connection.Open();
                return true;
            }catch(Exception ex)
            {
                MessageBox.Show("连接失败" + ex.Message);
                return false;
            }

        }

        public void Close()
        {
           
                _connection?.Close();
                _connection?.Dispose();
                _connection = null;
        }

        public void Dispose()
        {
            Close();
        }


        //执行只需要返回 成功或失败结果的语句
        public int Excute(string sql)
        {
            using (var command = new SQLiteCommand(sql, _connection))
            {
                return command.ExecuteNonQuery();

            }
        }

        public List<T> QueryAll<T>(string sql) where T : new()
        {
            

            var list = new List<T>();

            using (var command = new SQLiteCommand(sql, _connection))
            {
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var obj = new T();

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        var prop = typeof(T).GetProperty(reader.GetName(i));

                        if (prop != null && !reader.IsDBNull(i))
                        {
                            prop.SetValue(obj, reader.GetValue(i));
                        }
                    }

                    list.Add(obj);
                }

            }

            //// 添加分页参数
            //command.Parameters.AddWithValue("@PageSize", pageSize);
            //command.Parameters.AddWithValue("@Offset", offset);

            //if (parameter != null)
            //{
            //    AddParameter(command, parameter);
            //}
            //创建读取器


            return list;
        }

        // 分页查询实现
        public Tuple<List<T>, int> QueryWithPaging<T>(string sql, int pageIndex, int pageSize) where T : new()
        {
            var data = new List<T>();
            int totalRecords = 0;

            try
            {
                // 1. 计算总记录数
                string countSql = "SELECT COUNT(*) FROM (" + sql + ") AS subquery";

                using (var countCommand = new SQLiteCommand(countSql, _connection))
                {
                    totalRecords = Convert.ToInt32(countCommand.ExecuteScalar());
                }

                // 2. 构建分页SQL（直接拼接LIMIT和OFFSET）
                string pagedSql = string.Format(
                    "{0} LIMIT {1} OFFSET {2}",
                    sql,
                    pageSize,
                    (pageIndex - 1) * pageSize
                );

                // 3. 执行分页查询
                using (var command = new SQLiteCommand(pagedSql, _connection))
                using (var reader = command.ExecuteReader())
                {
                    var properties = typeof(T).GetProperties();

                    while (reader.Read())
                    {
                        var item = new T();
                        foreach (var property in properties)
                        {
                            if (HasColumn(reader, property.Name) && !reader.IsDBNull(reader.GetOrdinal(property.Name)))
                            {
                                property.SetValue(item, reader[property.Name]);
                            }
                        }
                        data.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("分页查询错误: " + ex.Message);
                throw;
            }

            return Tuple.Create(data, totalRecords);
        }

        // 辅助方法：检查DataReader是否包含指定列
        private bool HasColumn(IDataReader reader, string columnName)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetName(i).Equals(columnName, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

    }
}
