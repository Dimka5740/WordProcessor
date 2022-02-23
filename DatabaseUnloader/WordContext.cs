using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DatabaseLoader
{
    class WordContext : DbContext
    {
        public DbSet<Word> Words { get; set; } = null!;
        readonly private string connectionString;
        public WordContext(string connectionString)
        {
            this.connectionString = connectionString;
            Database.EnsureCreated();

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
