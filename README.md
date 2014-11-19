SimpleSqlBulkCopy
=================

Lets you efficiently bulk load a SQL Server table with data from a Typed List. This uses System.Data.SqlClient.SqlBulkCopy and just wraps it for a more developer-friendly interface

Usage
-----
```
using (var ssbc = new SimpleSqlBulkCopy("ConnectionString"))
{
    ssbc.WriteToServer("TableName", IEnumerable<T>);
}
```
where T is an object of the same Properties (and Type of the Database)