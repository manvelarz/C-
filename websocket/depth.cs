using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace websocket
{

    public class depthRoot
    {
        public string channel { get; set; }
        public depthData data { get; set; }

    }

    public  class depthData
    {
        public List<List<double>> bids { get; set; }
        public List<List<double>> asks { get; set; }
        public  string timestamp { get; set; }


        public static List<Tuple<int,double>> round (List<List<double>> input)
        {
            List<Tuple<int,double>> newdata = new List<Tuple<int,double>>();
            for (int i = 0; i < input.Count; i++)
            {
                var tup = new Tuple<int, double>(Convert.ToInt32(input[i][0]), input[i][1]);
                newdata.Insert(i, tup);
            }
            return newdata;
        }

        public static void storeToupleToFile(List<List<Tuple<int, double>>> msg, depthData input)
        {
            string output = "";

            string mydocpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\WriteLines.txt";

            using (StreamWriter outputFile = new StreamWriter(mydocpath, true))
            {


                outputFile.Write("\r\n" + input.timestamp + "," + input.asks.Last()[0] + "," + input.bids[0][0] + ",");

                output += "\r\n" + input.timestamp + "," + input.asks.Last()[0] + "," + input.bids[0][0] + ",";

                for (int m = 0; m < msg.Count; m++)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        if (i < msg[m].Count)
                        {
                            outputFile.Write(Math.Round(msg[m][i].Item2, 0).ToString() + ","); // outputFile.Write(Math.Round(msg[m][i].Item2, 2).ToString("0.00") + ","); 
                            output += Math.Round(msg[m][i].Item2, 0).ToString() + ","; //msg[m][i].Item1 +":"+ 


                        }
                        else
                        {
                            outputFile.Write(",");
                            output += ",";
                        }

                    }
                    output += "--";
                }



            }

            Console.WriteLine(output);
        }

        public static void TransformAndWright(depthData input)
        {

            List<Tuple<int, double>> asktup = depthData.round(input.asks);
            List<Tuple<int, double>> bidtup = depthData.round(input.bids);
            List<Tuple<int, double>> asklevel1sum = asktup.GroupBy(o => o.Item1).Select(i => new Tuple<int, double>(i.Key, i.Sum(x => (double)x.Item2))).ToList();
            List<Tuple<int, double>> bidlevel1sum = bidtup.GroupBy(o => o.Item1).Select(i => new Tuple<int, double>(i.Key, i.Sum(x => (double)x.Item2))).ToList();
            List<List<Tuple<int, double>>> level1sum = new List<List<Tuple<int, double>>> { asklevel1sum, bidlevel1sum };

            storeToupleToFile(level1sum, input);
        }
    }
}

