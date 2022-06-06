using InvoiceApi.Models.hoadon68;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceApi.Util
{
    public class SerializeToClass
    {
        public static List<T> CreateListFromTable<T>(DataTable tbl, DataTable ttkhacTable) where T : new()
        {
            // define return list
            List<T> lst = new List<T>();

            // go through each row
            foreach (DataRow r in tbl.Rows)
            {
                // add to the list
                lst.Add(CreateItemFromRow<T>(r, ttkhacTable));
            }

            // return the list
            return lst;
        }

        // function that creates an object from the given data row
        public static T CreateItemFromRow<T>(DataRow row, DataTable ttkhacTable) where T : new()
        {
            // create a new object
            T item = new T();
            // set the item
            SetItemFromRow(item, row, ttkhacTable);

            // return 
            return item;
        }

        public static void SetItemFromRow<T>(T item, DataRow row, DataTable ttkhacTable) where T : new()
        {
            // go through each column
            foreach (DataColumn c in row.Table.Columns)
            {
                // find the property for the column
                PropertyInfo p = item.GetType().GetProperty(c.ColumnName);

                if (ttkhacTable != null)
                {
                    if (ttkhacTable.Rows.Count > 0)
                    {
                        var ttkhacHHDVU = item.GetType().GetProperty("TTKhac");

                        if (ttkhacHHDVU != null)
                        {
                            List<TTin> tt = new List<TTin>();

                            foreach (DataRow dRow in ttkhacTable.Rows)
                            {
                                string ttruong = dRow["ttruong"].ToString();
                                string kdlieu = dRow["kdlieu"].ToString();

                                TTin ttIn = new TTin();

                                ttIn.KDLieu = kdlieu;
                                ttIn.TTruong = ttruong;

                                if (row.Table.Columns.Contains(ttruong))
                                {
                                    if (row[ttruong] != DBNull.Value)
                                    {
                                        if (kdlieu == "dateTime")
                                        {
                                            DateTime value = Convert.ToDateTime(row[ttruong]);
                                            ttIn.DLieu = string.Format("{0:yyyy-MM-ddTHH:mm:ss}", value);
                                        }
                                        else if (kdlieu == "date")
                                        {
                                            DateTime value = Convert.ToDateTime(row[ttruong]);
                                            ttIn.DLieu = string.Format("{0:yyyy-MM-dd}", value);
                                        }
                                        else if (kdlieu == "numeric")
                                        {
                                            decimal value = Convert.ToDecimal(row[ttruong]);
                                            ttIn.DLieu = value.ToString();
                                        }
                                        else
                                        {
                                            string value = row[ttruong].ToString();
                                            ttIn.DLieu = value.ToString();
                                        }
                                    }

                                }

                                tt.Add(ttIn);
                            }

                            ttkhacHHDVU.SetValue(item, tt, null);
                        }
                    }
                }


                // if exists, set the value
                if (p != null && row[c] != DBNull.Value)
                {
                    //var k = Convert.ChangeType(row[c], p.PropertyType);
                    // kiểm tra field trong class nếu là kiểu double thì convert sang
                    if (p.PropertyType == typeof(double))
                    {
                        double val = Convert.ToDouble(row[c]);
                        p.SetValue(item, val, null);
                    }
                    else
                    {
                        p.SetValue(item, row[c], null);
                    }
                }
            }
        }
    }
}
