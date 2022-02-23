using DatabaseLoader;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseUnloader
{
    class DbUnloader
    {
        public readonly string connectionString;

        public DbUnloader(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Word> FindWord(string searchWord)
        {
            SqlParameter param = new SqlParameter("@word", $"{ searchWord}%");

            string requestText = @"SELECT TOP(5)WordText, Count
                    FROM [vocabulary].[dbo].[Words] WHERE WordText LIKE @word
                    ORDER BY Count DESC, WordText";

            using (WordContext dbContext = new WordContext(connectionString))
            {
                return dbContext.Words.FromSqlRaw(requestText, param).ToList();
            }
        }
    }
}
