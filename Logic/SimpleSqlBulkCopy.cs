using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace System.Data.SqlClient
{
    public class SimpleSqlBulkCopy : IDisposable
    {
        private SqlBulkCopy sqlBulkCopy;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Data.SqlClient.SqlBulkCopy" /> class using the specified open
        ///     instance of <see cref="T:System.Data.SqlClient.SqlConnection" />.
        /// </summary>
        /// <param name="connection">
        ///     The already open <see cref="T:System.Data.SqlClient.SqlConnection" /> instance that will be
        ///     used to perform the bulk copy operation. If your connection string does not use Integrated Security = true, you can
        ///     use <see cref="T:System.Data.SqlClient.SqlCredential" /> to pass the user ID and password more securely than by
        ///     specifying the user ID and password as text in the connection string.
        /// </param>
        public SimpleSqlBulkCopy(SqlConnection connection)
        {
            sqlBulkCopy = new SqlBulkCopy(connection);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Data.SqlClient.SqlBulkCopy" /> class using the supplied
        ///     existing open instance of <see cref="T:System.Data.SqlClient.SqlConnection" />. The
        ///     <see cref="T:System.Data.SqlClient.SqlBulkCopy" /> instance behaves according to options supplied in the
        ///     <paramref name="copyOptions" /> parameter. If a non-null <see cref="T:System.Data.SqlClient.SqlTransaction" /> is
        ///     supplied, the copy operations will be performed within that transaction.
        /// </summary>
        /// <param name="connection">
        ///     The already open <see cref="T:System.Data.SqlClient.SqlConnection" /> instance that will be
        ///     used to perform the bulk copy. If your connection string does not use Integrated Security = true, you can use
        ///     <see cref="T:System.Data.SqlClient.SqlCredential" /> to pass the user ID and password more securely than by
        ///     specifying the user ID and password as text in the connection string.
        /// </param>
        /// <param name="copyOptions">
        ///     A combination of values from the <see cref="T:System.Data.SqlClient.SqlBulkCopyOptions" />
        ///     enumeration that determines which data source rows are copied to the destination table.
        /// </param>
        /// <param name="externalTransaction">
        ///     An existing <see cref="T:System.Data.SqlClient.SqlTransaction" /> instance under
        ///     which the bulk copy will occur.
        /// </param>
        public SimpleSqlBulkCopy(SqlConnection connection, SqlBulkCopyOptions copyOptions,
            SqlTransaction externalTransaction)
        {
            sqlBulkCopy = new SqlBulkCopy(connection, copyOptions, externalTransaction);
        }

        /// <summary>
        ///     Initializes and opens a new instance of <see cref="T:System.Data.SqlClient.SqlConnection" /> based on the supplied
        ///     <paramref name="connectionString" />. The constructor uses the <see cref="T:System.Data.SqlClient.SqlConnection" />
        ///     to initialize a new instance of the <see cref="T:System.Data.SqlClient.SqlBulkCopy" /> class.
        /// </summary>
        /// <param name="connectionString">
        ///     The string defining the connection that will be opened for use by the
        ///     <see cref="T:System.Data.SqlClient.SqlBulkCopy" /> instance. If your connection string does not use Integrated
        ///     Security = true, you can use
        ///     <see cref="M:System.Data.SqlClient.SqlBulkCopy.#ctor(System.Data.SqlClient.SqlConnection)" /> or
        ///     <see
        ///         cref="M:System.Data.SqlClient.SqlBulkCopy.#ctor(System.Data.SqlClient.SqlConnection,System.Data.SqlClient.SqlBulkCopyOptions,System.Data.SqlClient.SqlTransaction)" />
        ///     and <see cref="T:System.Data.SqlClient.SqlCredential" /> to pass the user ID and password more securely than by
        ///     specifying the user ID and password as text in the connection string.
        /// </param>
        public SimpleSqlBulkCopy(string connectionString)
        {
            sqlBulkCopy = new SqlBulkCopy(connectionString);
        }

        /// <summary>
        ///     Initializes and opens a new instance of <see cref="T:System.Data.SqlClient.SqlConnection" /> based on the supplied
        ///     <paramref name="connectionString" />. The constructor uses that
        ///     <see cref="T:System.Data.SqlClient.SqlConnection" /> to initialize a new instance of the
        ///     <see cref="T:System.Data.SqlClient.SqlBulkCopy" /> class. The <see cref="T:System.Data.SqlClient.SqlConnection" />
        ///     instance behaves according to options supplied in the <paramref name="copyOptions" /> parameter.
        /// </summary>
        /// <param name="connectionString">
        ///     The string defining the connection that will be opened for use by the
        ///     <see cref="T:System.Data.SqlClient.SqlBulkCopy" /> instance. If your connection string does not use Integrated
        ///     Security = true, you can use
        ///     <see cref="M:System.Data.SqlClient.SqlBulkCopy.#ctor(System.Data.SqlClient.SqlConnection)" /> or
        ///     <see
        ///         cref="M:System.Data.SqlClient.SqlBulkCopy.#ctor(System.Data.SqlClient.SqlConnection,System.Data.SqlClient.SqlBulkCopyOptions,System.Data.SqlClient.SqlTransaction)" />
        ///     and <see cref="T:System.Data.SqlClient.SqlCredential" /> to pass the user ID and password more securely than by
        ///     specifying the user ID and password as text in the connection string.
        /// </param>
        /// <param name="copyOptions">
        ///     A combination of values from the <see cref="T:System.Data.SqlClient.SqlBulkCopyOptions" />
        ///     enumeration that determines which data source rows are copied to the destination table.
        /// </param>
        public SimpleSqlBulkCopy(string connectionString, SqlBulkCopyOptions copyOptions)
        {
            sqlBulkCopy = new SqlBulkCopy(connectionString, copyOptions);
        }

        public SqlBulkCopy SqlBulkCopy
        {
            get { return sqlBulkCopy; }
        }

        public void Dispose()
        {
            sqlBulkCopy = null;
        }


        public void WriteToServer<T>(string destinationTableName, IEnumerable<T> data)
        {
            sqlBulkCopy.DestinationTableName = destinationTableName;
            DataTable dt = getDataTableFromFields(data);

            sqlBulkCopy.WriteToServer(dt);
        }

        public async Task WriteToServerAsync<T>(string destinationTableName, IEnumerable<T> data)
        {
            sqlBulkCopy.DestinationTableName = destinationTableName;
            DataTable dt = getDataTableFromFields(data);

            await sqlBulkCopy.WriteToServerAsync(dt);
        }

        private DataTable getDataTableFromFields<T>(IEnumerable<T> data)
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