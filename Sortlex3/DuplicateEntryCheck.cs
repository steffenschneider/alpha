using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Sortlex3
{
    class DuplicateEntryCheck
    {
        public static void Run(string filename)
        {
            // input scheme
            // ##article
            // [tag]
            // content xölkasödfkjöasdkjf
            // empty line
            // solution: put all article lines into one line
            // take chars until first ']'
            // search duplicates
            
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

            var list = new List<string>();
            var listPointer = -1;

            foreach (string line in lines)
            {
                if (line.Contains("##"))
                {
                    listPointer += 1;
                    list.Add("");
                }

                list[listPointer] = list[listPointer] + line;
            }

            // cut string after first ']'
            var list2 = new List<string>();
            foreach (string item in list)
            {
                var item2 = item.Split(']')[0];
                list2.Add(item2);
            }

            var duplicateKeys = list2.GroupBy(x => x)
                        .Where(group => group.Count() > 1)
                        .Select(group => group.Key)
                        .OrderBy(x => x);

            foreach (string duplicate in duplicateKeys)
            {
                logger.Warn($"duplicate '{duplicate}' found in file '{filename}'.");
            }
        }
    }
}
