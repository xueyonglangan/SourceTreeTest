using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseViewWinForm.DataBaseConnect
{
    //数据工厂-根据类型不同创建不同的数据库连接
    public static class DatabaseFactory
    {
        public static string dataBaseWay;

        public enum DatabaseType { MySQL, SQLite }

        //数据库连接的静态方法
        public static IDatabaseConnection Creat(DatabaseType type, string connectionString)
        {
            switch (type)
            {
                case DatabaseType.MySQL:
                    return new MySqlDatabaseConnection(connectionString);  // 创建MySQL连接
                case DatabaseType.SQLite:
                    return new SqliteDatabaseConnection(connectionString);  // 创建SQLite连接
                default:
                    throw new NotSupportedException($"不支持的数据库类型: {type}");
            }
        }

        public static string DatabaseNameSwitch(string DBName)
        {
            switch (DBName)
            {
                case "secote_PartNumber":

                    //var result = @"Data Source = C:\Users\user\Desktop\SQL\secote_PartNumber.db;Version=3";
                    var result = $"Data Source =  {dataBaseWay};Version=3";
                return result;
                case "test":
                var result1 = @"Data Source = D:\Documents\vs2022\WPF\WPFFunctionPractice\SqliteStudio\SqliteStudio\DataBase\test.db;Version=3";
                return result1;

                default:
                    throw new NotSupportedException("未选择正确的数据库");
            }   
        }
    }
}
