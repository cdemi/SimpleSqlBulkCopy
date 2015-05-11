using System.Collections.Generic;
using System.Linq;

namespace System.Data.SqlClient
{
    public class SimpleSqlBulkUpdate : IDisposable
    {
        private readonly SqlConnection _connection;
        private readonly SqlTransaction _externalTransaction;
        private readonly ConnectionState _initialState = ConnectionState.Closed;

        public SimpleSqlBulkUpdate(SqlConnection connection)
        {
            _connection = connection;
            _initialState = connection.State;
        }

        public SimpleSqlBulkUpdate(SqlConnection connection,
            SqlTransaction externalTransaction)
        {
            _externalTransaction = externalTransaction;
            _connection = connection;
            _initialState = connection.State;
        }

        public SimpleSqlBulkUpdate(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
        }

        public void Dispose()
        {
            // recycle connection if was not initially open
            if (_initialState != ConnectionState.Open
                &&_connection != null)
                _connection.Dispose();
        }

        public void BulkUpdate<T>(string destinationTableName, IEnumerable<T> data,
            string columnNameToMatch,
            string[] columnNamesToUpdate)
        {
            bool wasOpen = _connection.State == ConnectionState.Open;
            if (!wasOpen)
                _connection.Open();
            string tempTablename = "#" + destinationTableName + "_" + Guid.NewGuid().ToString("N");
            CreateTempTable(destinationTableName, tempTablename);
            var dataAsArray = data as T[] ?? data.ToArray();

            using (SqlBulkCopy sbc = new SqlBulkCopy(_connection, SqlBulkCopyOptions.KeepIdentity, _externalTransaction))
            {
                sbc.DestinationTableName = tempTablename;
                DataTable dt = Common.GetDataTableFromFields(dataAsArray, sbc);
                sbc.WriteToServer(dt);
            }
            MergeTempAndDestination(destinationTableName, tempTablename, columnNameToMatch, columnNamesToUpdate);
            DropTempTable(tempTablename);

            if (!wasOpen)
                _connection.Close();
        }

        private void DropTempTable(string tempTablename)
        {
            var cmdTempTable = _connection.CreateCommand();
            cmdTempTable.CommandText = "DROP TABLE " + tempTablename;
            cmdTempTable.ExecuteNonQuery();
        }

        private void MergeTempAndDestination(string destinationTableName, string tempTablename,
            string matchingColumn,
            string[] columnNamesToUpdate)
        {
            var updateSql = "";
            for (var i = 0; i < columnNamesToUpdate.Length; i++)
            {
                updateSql += String.Format("Target.[{0}]=Source.[{0}]", columnNamesToUpdate[i]);
                if (i < columnNamesToUpdate.Length - 1)
                    updateSql += ",";
            }
            var mergeSql = "MERGE INTO " + destinationTableName + " AS Target\r\n" +
                           "USING " + tempTablename + " AS Source\r\n" +
                           "ON\r\n" +
                           "Target." + matchingColumn + " = Source." + matchingColumn + "\r\n" +
                           "WHEN MATCHED THEN\r\n" +
                           "UPDATE SET " + updateSql + ";";

            var cmdTempTable = _connection.CreateCommand();
            cmdTempTable.CommandText = mergeSql;
            cmdTempTable.ExecuteNonQuery();
        }

        private void CreateTempTable(string destinationTableName, string tempTablename)
        {
            var cmdTempTable = _connection.CreateCommand();
            cmdTempTable.CommandText = "SELECT TOP 0 * \r\n" +
                                       "INTO " + tempTablename + "\r\n" +
                                       "FROM " + destinationTableName;
            cmdTempTable.ExecuteNonQuery();
        }
    }
}