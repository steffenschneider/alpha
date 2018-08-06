using System;
using System.IO;
using System.Collections.Generic;
using Sortlex3;
using NLog;

namespace Sortlex
{
    class MainClass
    {
        // todo


        public static void Main(string[] args)
        {
            var logger = LogManager.GetCurrentClassLogger();

            var files = new List<string>()
            {
                @"C:\Users\User\Dropbox\lex-home.txt",
                @"C:\Users\User\Dropbox\lex-work.txt",
                @"C:\Users\User\Desktop\diary.txt"
            };

            foreach (string myfile in files)
            {
                logger.Info($"Processing file: {myfile}");

                var allText = File.ReadAllText(myfile);
                logger.Info("LexCleanUp started");
                LexCleanUp.Run(myfile, allText);

                var allLines = File.ReadAllLines(myfile);
                logger.Info("SortLines started");
                SortLex.SortLines(myfile, allLines);

                logger.Info("TagStats started");
                Stats.TagStats(myfile);
                Stats.ArticleStats(myfile);

                logger.Info("DuplicateEntryCheck started");
                DuplicateEntryCheck.Run(myfile);
            }

            Console.ReadKey();
        }        
    }
}
