using System;
using System.Collections.Generic;
using System.Data.Common;

namespace DataBaseViewWinForm.DataBaseConnect
{
    public interface IDatabaseConnection:IDisposable
    {
        DbConnection Connection { get; }

        bool Open();

        void Close();
        //执行非查询语句
        int Excute(string sql);

        Tuple<List<T>, int> QueryWithPaging<T>(
           string sql,
           int pageIndex,
           int pageSize) where T : new();
    }

}
