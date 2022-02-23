using System;

namespace DatabaseLoader
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"D:\Program files\Dropbox\Projects\DatabaseLoader\input.txt",
                connectionString = @"Server=(localdb)\mssqllocaldb;Database=vocabulary;Trusted_Connection=True;";
            DbLoader dbLoader = new DbLoader(connectionString, path);
            dbLoader.ReadAndLoadWordsIntoDB();
        }
    }
}
