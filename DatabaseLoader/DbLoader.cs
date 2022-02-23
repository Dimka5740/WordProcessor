using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Transactions;
using Microsoft.EntityFrameworkCore;

namespace DatabaseLoader
{
    class DbLoader
    {
        private readonly string connectionString;
        private readonly string pathToReadFile;
        private const int minWordCount = 4;

        public DbLoader(string connectionString, string pathToReadFile)
        {
            this.connectionString = connectionString;
            this.pathToReadFile = pathToReadFile;
        }
        /// <summary>
        /// Считывание данных из файла по пути pathToReadFile, фильтрация полученного множества, отправка в БД
        /// </summary>
        public void ReadAndLoadWordsIntoDB()
        {
            string textFromFile;
            using (StreamReader reader = new StreamReader(pathToReadFile, System.Text.Encoding.UTF8))
            {
                textFromFile = reader.ReadToEnd();
            }

            //Используем регулярное выражение для выборки слов, подходящих под условие задачи
            Regex regex = new Regex(@"(\b[a-z]{3,20}\b)|(\b[а-я]{3,20}\b)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            MatchCollection matches = regex.Matches(textFromFile);
            if (matches.Count > 0)
            {
                using (WordContext db = new WordContext(connectionString))
                {
                    var strategy = db.Database.CreateExecutionStrategy();

                    strategy.Execute(
                      () =>
                   {
                       using (WordContext dbContext = new WordContext(connectionString))
                       {
                           using (var tr = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.RepeatableRead))
                           {
                               var wordBuf = new Word();

                               foreach (Match match in matches)
                               {
                                   var matchBuf = match.Value.ToLower();
                                   if ((wordBuf = dbContext.Words.Find(matchBuf)) != null)//Проверяем наличие слова в контексте БД
                                       wordBuf.Count++;//Если слово есть, то инкрементируем счетчик
                                   else
                                       dbContext.Words.Add(new Word { WordText = matchBuf, Count = 1 });//Если нет, то добавляем новое слово
                               }

                               //Производим удаление слов, встретившихся меньше 4 раз
                               var words = dbContext.Words.Local.Where(w => w.Count < minWordCount);
                               dbContext.Words.RemoveRange(words);

                               dbContext.SaveChanges();

                               tr.Commit();
                           }
                       }
                   });
                }
            }
        }
    }
}
