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

    public class depthData
    {
        public List<List<double>> bids { get; set; }
        public List<List<double>> asks { get; set; }
        public string timestamp { get; set; }


        public static List<Tuple<int, double>> round(List<List<double>> input)
        {
            List<Tuple<int, double>> newdata = new List<Tuple<int, double>>();
            for (int i = 0; i < input.Count; i++)
            {
                var tup = new Tuple<int, double>(Convert.ToInt32(input[i][0]), input[i][1]);
                newdata.Insert(i, tup);
            }
            return newdata;
        }

        public static void TransformAndStore(depthData input)
        {

            List<Tuple<int, double>> asktup = depthData.round(input.asks);
            List<Tuple<int, double>> bidtup = depthData.round(input.bids);
            List<Tuple<int, double>> asklevel1sum = asktup.GroupBy(o => o.Item1).Select(i => new Tuple<int, double>(i.Key, i.Sum(x => (double)x.Item2))).ToList();
            List<Tuple<int, double>> bidlevel1sum = bidtup.GroupBy(o => o.Item1).Select(i => new Tuple<int, double>(i.Key, i.Sum(x => (double)x.Item2))).ToList();
            List<List<Tuple<int, double>>> level1sum = new List<List<Tuple<int, double>>> { asklevel1sum, bidlevel1sum };

            storeToupleToFileFirst10(level1sum, input);
        }


        /// <summary>
        /// Store First 10 int numbers of both Asks and Bids. If there are no bid or ask in the next Int number it not stored Bad for the Orden
        /// ete xosqi ask a 6612-0.1,6614-0.45... vaabshe chi save anum 6613 i masin info aranqic hanum a.
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="input"></param>
        public static void storeToupleToFileAbsolute10(List<List<Tuple<int, double>>> msg, depthData input) // 
        {
            string output = "";
            string tab = "\t";
            int orderbookDepthCount = 10;
            //Console.WriteLine("\n"+msg[0].Count+"------------" + msg[1].Count);

            string timestamp = input.timestamp.Remove(input.timestamp.Length - 3, 3); // Drop out milliseconds we dont need that


            var dt = UnixTime.UnixTimeStampToDateTime(long.Parse(timestamp)); // Mer stacac@ 3 tiv avel uni varkyanic heto


            string mydocpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\WriteLines100_.txt";

            if (!File.Exists(mydocpath))
            {
                using (StreamWriter outputFile = new StreamWriter(mydocpath, true))
                {
                    outputFile.Write("Timestump\tUp\tDown\t10\t9\t8\t7\t6\t5\t4\t3\t2\t1\t-1\t-2\t-3\t-4\t-5\t-6\t-7\t-8\t-9\t-10\tdate\tAsksum\tBidsum");
                }
            }

            using (StreamWriter outputFile = new StreamWriter(mydocpath, true))
            {


                outputFile.Write("\r\n" + timestamp + tab + input.asks.Last()[0] + tab + input.bids[0][0] + tab);

                output += "" + dt.ToString() + tab + input.asks.Last()[0] + tab + input.bids[0][0] + tab;

                for (int m = 0; m < msg.Count; m++)  //    2 hata 0 -> Ask(verevin) 1 -> Bid(nerqevi)
                {

                    if (m == 0)  // Asqeri demic Taber dnelu hamar vor misht +1 i column@ nuyn@ lini
                    {
                        for (int i = 0; i < orderbookDepthCount - msg[m].Count; i++)
                        {
                            outputFile.Write(tab);
                            output += tab;
                        }
                    }

                    for (int i = 0; i < orderbookDepthCount; i++)
                    {
                        if (i < msg[m].Count) // TOWATCH i< or i<=   ------------------------ || m == 0
                        {
                            outputFile.Write(Math.Round(msg[m][i].Item2, 0).ToString() + tab);
                            output += Math.Round(msg[m][i].Item2, 0).ToString() + tab;


                        }
                        else
                        {
                            if (m != 0)
                            {
                                outputFile.Write(tab);
                                output += tab;
                            }


                        }

                    }
                    output += "||\t";
                }



            }

            Console.WriteLine(output);
        }

        /// <summary>
        /// Store First 10 int numbers of both Asks and Bids. If there are no bid or ask in the next Int number it not stored Bad for the Orden
        /// ete xosqi ask a 6612-0.1,6614-0.45... vaabshe chi save anum 6613 i masin info aranqic hanum a.
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="input"></param>
        public static void storeToupleToFileFirst10(List<List<Tuple<int, double>>> msg, depthData input) // 
        {
            string output = "";
            string tab = "\t";
            int orderbookDepthCount = 10;
            //Console.WriteLine("\n"+msg[0].Count+"------------" + msg[1].Count);

            string timestamp = input.timestamp.Remove(input.timestamp.Length - 3, 3); // Drop out milliseconds we dont need that


            var dt = UnixTime.UnixTimeStampToDateTime(long.Parse(timestamp)); // Mer stacac@ 3 tiv avel uni varkyanic heto


            string mydocpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\WriteLines100_.txt";

            if (!File.Exists(mydocpath))
            {
                using (StreamWriter outputFile = new StreamWriter(mydocpath, true))
                {
                    outputFile.Write("Timestump\tUp\tDown\t10\t9\t8\t7\t6\t5\t4\t3\t2\t1\t-1\t-2\t-3\t-4\t-5\t-6\t-7\t-8\t-9\t-10\tdate\tAsksum\tBidsum");
                }
            }

            using (StreamWriter outputFile = new StreamWriter(mydocpath, true))
            {


                outputFile.Write("\r\n" + timestamp + tab + input.asks.Last()[0] + tab + input.bids[0][0] + tab);

                output += "" + dt.ToString() + tab + input.asks.Last()[0] + tab + input.bids[0][0] + tab;

                for (int m = 0; m < msg.Count; m++)  //    2 hata 0 -> Ask(verevin) 1 -> Bid(nerqevi)
                {

                    if (m == 0)  // Asqeri demic Taber dnelu hamar vor misht +1 i column@ nuyn@ lini
                    {
                        for (int i = 0; i < orderbookDepthCount - msg[m].Count; i++)
                        {
                            outputFile.Write(tab);
                            output += tab;
                        }
                    }

                    for (int i = 0; i < orderbookDepthCount; i++)
                    {
                        if (i < msg[m].Count) // TOWATCH i< or i<=   ------------------------ || m == 0
                        {
                            outputFile.Write(Math.Round(msg[m][i].Item2, 0).ToString() + tab); 
                            output += Math.Round(msg[m][i].Item2, 0).ToString() + tab; 


                        }
                        else
                        {
                            if (m != 0)
                            {
                                outputFile.Write(tab);
                                output += tab;
                            }

                            
                        }

                    }
                    output += "||\t";
                }



            }

            Console.WriteLine(output);
        }


    }
 }

