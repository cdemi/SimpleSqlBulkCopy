SimpleSqlBulkCopy
=================

Lets you efficiently bulk load a SQL Server table with data from a Typed List. This uses [`System.Data.SqlClient.SqlBulkCopy`](http://msdn.microsoft.com/en-us/library/system.data.sqlclient.sqlbulkcopy(v=vs.110).aspx) and just wraps it for a more developer-friendly interface

Remarks
-------
**Microsoft SQL Server** includes a popular command-prompt utility named `bcp` for moving data from one table to another, whether on a single server or between servers. `The SqlBulkCopy` class lets you write managed code solutions that provide similar functionality. There are other ways to load data into a SQL Server table (`INSERT` statements, for example), but `SqlBulkCopy` offers a significant performance advantage over them.

The `SqlBulkCopy` class can be used to write data only to SQL Server tables. However, the data source is not limited to SQL Server; any data source can be used, as long as the data can be loaded to a `DataTable` instance or read with a `IDataReader` instance.

This project, in turn, makes it easier to write a list of typed objects instead of a `DataTable` or `IDataReader`.


Usage
-----
1. Install [SimpleSqlBulkCopy](https://www.nuget.org/packages/SimpleSqlBulkCopy/) Package from NuGet: [https://www.nuget.org/packages/SimpleSqlBulkCopy/](https://www.nuget.org/packages/SimpleSqlBulkCopy/)
2. Alternatively you can download this repo and build the source
3. Once you have included the library in your project you can just create an instance of `SimpleSqlBulkCopy` which takes a Connection String or an `SqlConnection` instance.
4. Call the `WriteToServer` method which takes in the destination table name and a list of objects with their properties matching the database entry table.

Example
------
```
using (var ssbc = new SimpleSqlBulkCopy("ConnectionString"))
{
    ssbc.WriteToServer("TableName", IEnumerable<T>);
}
```
where `T` is an object of the same Properties (and Type of the Database)

If the name of the column on the target database does match the name of the property
on your call you can create the mapping by using the `Column` attribute from 
`System.ComponentModel.DataAnnoations`.  An example of this can be seen in the Demo program
where the property of "Foo" on the `MyClass` class is mapped to the database column of `Bar`.