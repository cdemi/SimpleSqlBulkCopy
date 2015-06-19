using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
            var connectionString = "Database=MyDataBase;Server=(localdb)\\MSSQLLocalDB";
            for (int i = 0; i < 5000; i++)
            {
                randomNumbers.Add(rand.Next());
            }

            using (var ssbc = new SimpleSqlBulkCopy(connectionString))
            {
                ssbc.WriteToServer("TableName", randomNumbers.Select(rn => new MyClass { Score = rn }));
            }

            using (var ssbc = new SimpleSqlBulkUpdate(connectionString))
            {
                ssbc.BulkUpdate("TableName", // update in this table
                    new[] {
                        new MyClass(1, 100, "Norris"),
                        new MyClass(5, 500, "Willis")
                    }, // update using this data
                    "Id", // update when values in this column match
                    new[] { "Score", "Winner"} // columns to update
                    );
            }
        }
    }

    public class MyClass
    {
        public MyClass()
        {
            Foo = "Maps to bar!";
        }

        public MyClass(int id, int score, string winner)
        {
            Winner = winner;
            Id = id;
            Score = score;
            Foo = "Maps to bar!";
        }

        public string Winner { get; private set; }
        public int Score { get; set; }
        public int Id { get; set; }
        public bool IsFinal { get; set; }
        [Column("Bar")]
        public string Foo { get; set; }
    }
}