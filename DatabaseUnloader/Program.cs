using System;

namespace DatabaseUnloader
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = @"Server=(localdb)\mssqllocaldb;Database=vocabulary;Trusted_Connection=True;";
            DbUnloader dbUnloader = new DbUnloader(connectionString);

            Console.Write("Введите слово или его часть: ");
            var inputSearchWord = Console.ReadLine();
            Console.WriteLine();

            if (inputSearchWord.Trim() != null && inputSearchWord.Trim() != "")
            {
                var resultList = dbUnloader.FindWord(inputSearchWord);
                foreach (var word in resultList)
                    Console.WriteLine(word.WordText);
            }
        }
    }
}
