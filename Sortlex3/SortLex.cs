using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sortlex3
{
    class SortLex
    {
        public static void SortLines(string myfile, string[] inputLineArray)
        {
            var articleLineCounter = 0;
            string articleName = "";
            string tagLine = "";
            List<List<String>> matrix = new List<List<String>>();
            List<String> sortedLinesList = new List<String>();
            var inputLineList = inputLineArray.ToList();

            // last line has to be empty
            if (inputLineList[inputLineList.Count - 1] != "")
                inputLineList.Add("");

            // ["#a", "[aaa]", "-aaa", "#a2", "[aaa]", "-a22", 
            for (int i = 0; i < inputLineList.Count; i++)
            {
                // find article name
                if (inputLineList[i].Length > 1)
                {
                    if (inputLineList[i][0] == '#' && inputLineList[i][1] == '#')
                    {
                        articleLineCounter = 0;
                        articleName = inputLineList[i];
                        tagLine = inputLineList[i + 1];
                    }
                }

                // save all to 4 column list
                matrix.Add(new List<String>());
                matrix[i].Add(articleName);
                matrix[i].Add(tagLine);
                matrix[i].Add(articleLineCounter.ToString());
                matrix[i].Add(inputLineList[i]);

                articleLineCounter += 1;
            }

            // sort matrix - tags - article name - article line number
            var orderedResult = matrix.OrderBy(x => x[1]).ThenBy(x => x[0]).ToList();

            // take last column with article name
            foreach (var item in orderedResult)
            {
                sortedLinesList.Add(item[3]);
            }

            File.WriteAllLines(myfile, sortedLinesList.ToArray());
        }
    }
}
