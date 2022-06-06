﻿using MinvoiceLib.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace InvoiceApi.IService
{
    public partial interface INopDbContext2
    {
        InvoiceDbContext GetInvoiceDb();
        string ExecuteStoreProcedure(string sql, Dictionary<string, string> parameters);
        DataTable ExecuteCmd(string sql);
        DataTable ExecuteCmd(string sql, CommandType commandType, Dictionary<string, object> parameters);
        Task<DataTable> ExecuteCmdAsync(string sql);
        Task<string> ExecuteStoreProcedureAsync(string sql, Dictionary<string, object> parameters);
        DataTable GetStoreProcedureParameters(string storeProcedure);
        DataSet GetDataSet(string sql, Dictionary<string, string> parameters);
        void ExecuteNoneQuery(string sql);
        void ExecuteNoneQuery(string sql, Dictionary<string, object> parameters);
        Task<string> ExecuteNoneQueryAsync(string sql, CommandType commandType, Dictionary<string, object> parameters);
        void SetConnect(string mst);
        DataTable GetAllColumnsOfTable(string tableName);

    }
}