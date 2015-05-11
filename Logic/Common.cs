using System.Collections.Generic;
using System.Reflection;

namespace System.Data.SqlClient
{
    public class Common
    {
        public static DataTable GetDataTableFromFields<T>(IEnumerable<T> data, SqlBulkCopy sqlBulkCopy)
        {
            var dt = new DataTable();
            Type listType = typeof (T);
            foreach (PropertyInfo propertyInfo in listType.GetProperties())
            {
                dt.Columns.Add(propertyInfo.Name, propertyInfo.PropertyType);
                sqlBulkCopy.ColumnMappings.Add(propertyInfo.Name, propertyInfo.Name);
            }

            foreach (T value in data)
            {
                DataRow dr = dt.NewRow();
                foreach (PropertyInfo propertyInfo in listType.GetProperties())
                {
                    dr[propertyInfo.Name] = propertyInfo.GetValue(value, null);
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }
    }
}