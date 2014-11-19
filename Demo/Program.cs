using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Demo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var rand = new Random();
            var randomNumbers = new List<int>();
            for (int i = 0; i < 5000; i++)
            {
                randomNumbers.Add(rand.Next());
            }

            using (var ssbc = new SimpleSqlBulkCopy("ConnectionString"))
            {
                ssbc.WriteToServer("TableName", randomNumbers.Select(rn => new { Score = rn }));
            }
        }
    }
}