using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;

namespace DataBaseViewWinForm.DataBaseConnect
{
    public class MySqlDatabaseConnection : IDatabaseConnection
    {
        private string connectionstring;
        private MySqlConnection _connection;
        public MySqlDatabaseConnection(string connectionstring)
        {
            this.connectionstring = connectionstring;
        }

        public DbConnection Connection
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        public void Close()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public int Excute(string sql)
        {
            throw new NotImplementedException();
        }

        public bool Open()
        {
            throw new NotImplementedException();
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

                using (var countCommand = new MySqlCommand(countSql, _connection))
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
                using (var command = new MySqlCommand(pagedSql, _connection))
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
