using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Sortlex3
{
    class LexCleanUp
    {
        public static void Run(string myfile, string inputLineArray)
        {
            var logger = LogManager.GetCurrentClassLogger();

            // two following empty lines --> one empty line
            // \n\r\n\r\n --> \n\r\n
            string output = Regex.Replace(inputLineArray, "\n\r\n\r\n", "\n\r\n");

            // replace wrong tags
            var tagTranslator = new Dictionary<string, string>
            {
                //{ @"\[CSharp\]\[CSharp\]", "[CSharp]" },
                { @"\[IT\]\[Programmieren\]", "[Programmieren]" },
                { @"\[osi-pi\]", "[OSI-PI]" },
                { @"\[IT\]\[Hardware\]", "[Hardware]" },
                { @"\[EntityFramework\]", "[Entity-Framework]" },
                { @"\[Umweltschutz\]", "[Umwelt]" },
                { @"\[vbnet\]", "[VB.NET]" },
                { @"\[Collection\]", "[Collections]" },
                { @"\[CSharp\]\[List\]", "[CSharp][Collections][List]" },
                { @"\[CSharp\]\[String\]", "[CSharp][Type][String]" },
                { @"\[CSharp\]\[Array\]", "[CSharp][Type][Array]" },
                { @"^\[Entity-Framework\]", "[CSharp][Entity-Framework]" },
                { @"\[Tiere\]", "[Tier]" }
            };

            foreach (KeyValuePair<string, string> entry in tagTranslator)
            {
                string inputLineArrayTemp = Regex.Replace(inputLineArray, entry.Key, entry.Value);
                inputLineArray = inputLineArrayTemp;
            }

            File.WriteAllText(myfile, inputLineArray);

            // looking for wrong sorted tags
            if (myfile.Contains("lex-work") || myfile.Contains("lex-main"))
            {
                if (inputLineArray.Contains("[privat]"))
                {
                    logger.Warn("[privat] in lex-main or lex-work!");
                }
            }
        }
    }
}
