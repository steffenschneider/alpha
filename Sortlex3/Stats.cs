using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Sortlex3
{
    class Stats
    {
        public static void TagStats(string filename)
        {
            var logger = LogManager.GetCurrentClassLogger();

            string[] lines = null;

            try
            {
                lines = System.IO.File.ReadAllLines(filename);
            }
            catch
            {
                throw new Exception("File not found");
            }

            var tagList = new List<string>();

            var toBeProcessed = false;

            if (lines != null)
            {
                foreach (string line in lines)
                {
                    if (toBeProcessed == true && line.Contains("["))
                    {
                        Regex regex = new Regex(@"^\[(.*?)\]$");
                        Match match = regex.Match(line);
                        var tag = match.Value;
                        if (tag == "")
                        {
                            logger.Error("Data with empty tag.");
                        }
                        tagList.Add(tag);
                    }

                    if (line.Contains("##"))
                    {
                        toBeProcessed = true;
                    }
                    else
                    {
                        toBeProcessed = false;
                    }
                }

                // show tags with count
                var result = tagList.GroupBy(x => x)
                .ToDictionary(y => y.Key, y => y.Count())
                .OrderByDescending(z => z.Value);

                foreach (var x in result)
                {
                    try
                    {
                        Console.WriteLine(x.Key + " ".PadLeft(38 - Convert.ToInt16(x.Key.Length)
                            - Convert.ToInt16(x.Value.ToString().Length)) + x.Value);
                    }
                    catch
                    {
                        Console.WriteLine(x.Key + "    " + x.Value);
                    }
                }

                Console.WriteLine();

                // show tags without count ordered alphabetically
                var tagList2 = tagList.Select(x => x).Distinct();
                var result2 = tagList2.OrderBy(s => s);

                foreach (var item in result2)
                {
                    Console.WriteLine(item);
                }

                Console.WriteLine();
            }
        }

        public static void ArticleStats(string filename)
        {
            var logger = LogManager.GetCurrentClassLogger();

            string[] lines = null;

            try
            {
                lines = System.IO.File.ReadAllLines(filename);
            }
            catch
            {
                throw new Exception("File not found");
            }

            var articleCount = 0;

            foreach (string line in lines)
            {
                if (line.StartsWith("##"))
                {
                    articleCount += 1;
                }
            }

            File.AppendAllText(@"C:\Users\User\Dropbox\data\lex_articles.txt", $"{DateTime.Now} - {filename} - {articleCount}"
                + Environment.NewLine);
        }
    }
}
