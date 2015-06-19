using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
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
                var columnName = GetColumnName(propertyInfo);
                dt.Columns.Add(columnName, propertyInfo.PropertyType);
                sqlBulkCopy.ColumnMappings.Add(columnName, columnName);
            }

            foreach (T value in data)
            {
                DataRow dr = dt.NewRow();
                foreach (PropertyInfo propertyInfo in listType.GetProperties())
                {
                    var columnName = GetColumnName(propertyInfo);
                    dr[columnName] = propertyInfo.GetValue(value, null);
                }
                dt.Rows.Add(dr);
            }
            
            return dt;
        }

        /// <summary>
        /// Gets the column name for the target database.  
        /// If the System.ComponentModel.DataAnnotations.ColumnAttribute is used
        /// it will attempt to use this value as the target column name.
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static string GetColumnName(PropertyInfo propertyInfo)
        {
            //check first for the DataAnnotations.ColumnAttribtue
            var columnAttribute = propertyInfo.GetCustomAttribute<ColumnAttribute>(false);

            if (columnAttribute != null) //it exists so use the attr value
                return columnAttribute.Name;

            //it doesn't exist so return the property name
            return propertyInfo.Name;
        }
    }
}