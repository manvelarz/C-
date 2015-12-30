using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace websocket
{



    public class Ticker
    {
        public double buy { get; set; }
        public double high { get; set; }
        public string last { get; set; }
        public double low { get; set; }
        public double sell { get; set; }
        public string timestamp { get; set; }
        public string vol { get; set; }
    }

    public class RootObject
    {
        public string channel { get; set; }
        public Ticker data { get; set; }
    }



    public class functions

    {

        public static List<string> CustomSplit(string line, char CharToSplit)
        {

            char[] pakagic = { CharToSplit };

            List<string> LineList = line.Split(pakagic, 2, StringSplitOptions.RemoveEmptyEntries).ToList();

            return LineList;
        }

        public static void storeListToFile(List<List<double>> msg)

        {

            string mydocpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\WriteLines.txt";

            using (StreamWriter outputFile = new StreamWriter(mydocpath ,true))
            {
                foreach (List<double> line in msg)
                {

                    //outputFile.NewLine();
                    foreach (double num in line)
                    {
                        //outputFile.WriteLine(num);
                        outputFile.Write(num+',');
                    }
                }
            }
        }






        
    
    }


}


