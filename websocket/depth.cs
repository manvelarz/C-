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

            var asktup = depthData.round(input.asks);
            var bidtup = depthData.round(input.bids);
            var asklevel1sum = asktup.GroupBy(o => o.Item1).Select(i => new Tuple<int, double>(i.Key, i.Sum(x => (double)x.Item2))).ToList();
            var bidlevel1sum = bidtup.GroupBy(o => o.Item1).Select(i => new Tuple<int, double>(i.Key, i.Sum(x => (double)x.Item2))).ToList();
            var nAsk = AbsolutOrden(asklevel1sum, Globals.DeptCount, false);
            var nBid = AbsolutOrden(bidlevel1sum, Globals.DeptCount, true);
            List<List<Tuple<int, double>>> level1sum = new List<List<Tuple<int, double>>> { nAsk, nBid };
            var st = ToArrangedString(level1sum, Globals.DeptCount);


            storeToupleToFileAbsolute10(level1sum, input);
        }

        /// <summary>
        /// It take the Dept Data and transform it to String depending in count you need
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="Dcount"> the cout of data in dept you want to convert to String</param>
        /// <returns></returns>
        public static string ToArrangedString(List<List<Tuple<int, double>>> msg, int Dcount)
        {
            var Asks = msg[0].Skip(msg[0].Count - Dcount).Take(Dcount);
            var Bids = msg[1].Take(Dcount);
            string str = "";
            foreach (var item in Asks)
            {
                str += Math.Round(item.Item2, 1).ToString() +"\t"; //item.Item1.ToString() + "|" +
            }
            str += "||" + "\t";
            foreach (var item in Bids)
            {
                str += Math.Round(item.Item2, 1).ToString() + "\t"; //item.Item1.ToString() + "|" +
            }
            return str;
        }

        /// <summary>
        /// The function take the list of Price & cuantity and return the same  filled the gaps with 0 Cuantity and 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="atleast">Thise param will guarante to have at return at least that cuantity of items in list which you introduce</param>
        /// <returns></returns>
        /// 
        public static List<Tuple<int, double>> AbsolutOrden(List<Tuple<int, double>> msg, int atleast, bool fillLastPart) // fillLastPart is for understand where to fill if the cuantity is less than 100?
        {
            
            var ListCopy = new List<Tuple<int, double>>(msg)  ;

            List<Tuple<int, double>> absOrdered = new List<Tuple<int, double>>();
            
            for (int i = msg.First().Item1+1; i-- > msg.Last().Item1;)
            {
                if ( i != ListCopy[0].Item1)
                {
                    var nextNum = new Tuple<int, double>(i, 0);// { i,0};
                    absOrdered.Add(nextNum);
                }
                else
                {
                    absOrdered.Add(ListCopy.First());
                    ListCopy.RemoveAt(0);
                }
            }
            if (fillLastPart)
            {
                while (absOrdered.Count < atleast) // to guarranty atleast count
                {
                    absOrdered.Add(new Tuple<int, double>(absOrdered.Last().Item1 - 1, 0));
                }
            }
            else
            {
                while (absOrdered.Count < atleast) // to guarranty atleast count
                {
                    absOrdered.Insert(0, (new Tuple<int, double>(absOrdered[0].Item1 + 1, 0))) ;
                }
            }
            return absOrdered;
        }


        /// <summary>
        /// 
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




            if (!File.Exists(Globals.mydocpath))
            {
                using (StreamWriter outputFile = new StreamWriter(Globals.mydocpath, true))
                {
                    outputFile.Write(Globals.Headers());
                }
            }

            using (StreamWriter outputFile = new StreamWriter(Globals.mydocpath, true))
            {

                int FirstAsk = Convert.ToInt16(input.asks.Last()[0]); // irakanum First@ chi ayl Lastn a
                int FirstBid = Convert.ToInt16(input.bids[0][0]);
                outputFile.Write("\r\n" + timestamp + tab + input.asks.Last()[0] + tab + input.bids[0][0] + tab);

                output += "" + dt.ToString() + tab + input.asks.Last()[0] + tab + input.bids[0][0] + tab;

                for (int m = 0; m < msg.Count; m++)  //    2 hata 0 -> Ask(verevin) 1 -> Bid(nerqevi)
                {

                    if (m == 0)
                    {
                        for (int i = 0; i < orderbookDepthCount - msg[m].Count; i++)// Asqeri demic Taber dnelu hamar vor misht +1 i column@ nuyn@ lini
                        {
                            outputFile.Write(tab);
                            output += tab;
                        }

                        for (int i = orderbookDepthCount; i-- > 0;)
                        {
                            var k = msg[m].Count - i - 1;  // esi nra hamar a vor hakarak kogmic ani (vrjic iteraciaov ga) vortev Asker@ tars en... 1@ hanum em vortev Count@ 1 ic a sksum isk Index@ 0 ic

                            var nextAskCuantity = Math.Round(msg[m][k].Item2, 0);
                            var nextAskPrice = msg[m][k].Item1;
                                //if (nextAsk == FirstAsk+ )
                                //{

                                //}

                                outputFile.Write(Math.Round(nextAskCuantity, 0).ToString() + tab);
                                output += Math.Round(nextAskCuantity, 0).ToString() + tab;  //output += Math.Round(msg[m][k].Item2, 0).ToString() + "<" + msg[m][k].Item1 + ">" + tab; // Version for see the prices too

                        }
                            outputFile.Write( tab);
                        }

                    else //// m = 1 Bidern en
                    {

                        for (int i = 0; i < orderbookDepthCount; i++)
                        {
                            if (i < msg[m].Count) // TOWATCH i< or i<=   ------------------------ || m == 0
                            {
                                outputFile.Write(Math.Round(msg[m][i].Item2, 0).ToString() + tab);

                                output += Math.Round(msg[m][i].Item2, 0).ToString() + tab;

                                //    output += Math.Round(msg[m][i].Item2, 0).ToString() + "<" + msg[m][i].Item1 + ">" + tab;


                            }
                            else
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


        public static void storeToupleToFileAbsolute10(List<List<Tuple<int, double>>> msg, depthData input) // 
        {
            string output = "";
            string tab = "\t";
            int orderbookDepthCount = 10;
            //Console.WriteLine("\n"+msg[0].Count+"------------" + msg[1].Count);

            string timestamp = input.timestamp.Remove(input.timestamp.Length - 3, 3); // Drop out milliseconds we dont need that


            var dt = UnixTime.UnixTimeStampToDateTime(long.Parse(timestamp)); // Mer stacac@ 3 tiv avel uni varkyanic heto




            if (!File.Exists(Globals.mydocpath))
            {
                using (StreamWriter outputFile = new StreamWriter(Globals.mydocpath, true))
                {
                    outputFile.Write(Globals.Headers());
                }
            }

            using (StreamWriter outputFile = new StreamWriter(Globals.mydocpath, true))
            {

                int FirstAsk = Convert.ToInt16(input.asks.Last()[0]); // irakanum First@ chi ayl Lastn a
                int FirstBid = Convert.ToInt16(input.bids[0][0]);
                outputFile.Write("\r\n" + timestamp + tab + input.asks.Last()[0] + tab + input.bids[0][0] + tab);

                output += "" + dt.ToString() + tab + input.asks.Last()[0] + tab + input.bids[0][0] + tab;

                for (int m = 0; m < msg.Count; m++)  //    2 hata 0 -> Ask(verevin) 1 -> Bid(nerqevi)
                {

                    if (m == 0) // m = o ASK
                    {
                        for (int i = 0; i < orderbookDepthCount - msg[m].Count; i++)// Asqeri demic Taber dnelu hamar vor misht +1 i column@ nuyn@ lini
                        {
                            outputFile.Write(tab);
                            output += tab;
                        }

                        for (int i = orderbookDepthCount; i-- > 0;)
                        {
                            if (msg[m].Count > i)
                            {
                                var k = msg[m].Count - i - 1;  // esi nra hamar a vor hakarak kogmic ani (vrjic iteraciaov ga) vortev Asker@ tars en... 1@ hanum em vortev Count@ 1 ic a sksum isk Index@ 0 ic

                                var nextAskCuantity = Math.Round(msg[m][k].Item2, 0);
                                //var nextAskPrice = msg[m][k].Item1;

                                var t = msg[m].FirstOrDefault(g => g.Item1 == FirstAsk + i); // 
                                if (t != null)
                                {
                                    outputFile.Write(Math.Round(t.Item2, 0).ToString() + tab);
                                    output += Math.Round(t.Item2, 1).ToString() + tab;  // t.Item1 +"|" +     output += Math.Round(msg[m][k].Item2, 0).ToString() + "<" + msg[m][k].Item1 + ">" + tab; // Version for see the prices too

                                }
                                else
                                {
                                    outputFile.Write("-" + tab);
                                    output += "-" + tab;
                                }
                            }

          
                        }
                        outputFile.Write("-" + tab);
                        outputFile.Write("-"+tab);
                    }

                    else //// m = 1 Bidern en
                    {
                        int cont = 0;
                        for (int i = 0; i < orderbookDepthCount; i++)
                        {
                            if ( FirstBid - cont == msg[m][i].Item1) // TOWATCH i< or i<=   ------------------------ || m == 0  i < msg[m].Count &&
                            {
                                
                                outputFile.Write(Math.Round(msg[m][cont].Item2, 0).ToString() + tab);

                                output += msg[m][i].Item1 + "|" + Math.Round(msg[m][cont].Item2, 1).ToString() + tab;

                                cont++;
                                //    output += Math.Round(msg[m][i].Item2, 0).ToString() + "<" + msg[m][i].Item1 + ">" + tab;


                            }
                            else
                            {
                                outputFile.Write("-" + tab);
                                output += "-" + tab;
                            }

                        }

                    }
                    outputFile.Write("||" + tab);
                    output += "||" + tab;
                }



            }

            Console.WriteLine(output);
        }

        public static string FullLineOfString(depthData input, int Dcount)
        {
            string output = "";
            string tab = "\t";
            string timestamp = input.timestamp.Remove(input.timestamp.Length - 3, 3); // Drop out milliseconds we dont need that
            var dt = UnixTime.UnixTimeStampToDateTime(long.Parse(timestamp)); // Mer stacac@ 3 tiv avel uni varkyanic heto

            var asktup = depthData.round(input.asks);
            var bidtup = depthData.round(input.bids);

            var asklevel1sum = asktup.GroupBy(o => o.Item1).Select(i => new Tuple<int, double>(i.Key, i.Sum(x => (double)x.Item2))).ToList();
            var bidlevel1sum = bidtup.GroupBy(o => o.Item1).Select(i => new Tuple<int, double>(i.Key, i.Sum(x => (double)x.Item2))).ToList();

            var nAsk = AbsolutOrden(asklevel1sum, Dcount, false); // False mean insert data at start
            var nBid = AbsolutOrden(bidlevel1sum, Dcount, true); // true mean add data to final

            List<List<Tuple<int, double>>> level1sum = new List<List<Tuple<int, double>>> { nAsk, nBid };

            var st = ToArrangedString(level1sum, Dcount);

            //outputFile.Write("\r\n" + timestamp + tab + input.asks.Last()[0] + tab + input.bids[0][0] + tab);

            output += dt.ToString() + tab + input.asks.Last()[0] + tab + input.bids[0][0] + tab + st;


            return output;
        }

        public static void WrightToFile(string  msg)
        {
            if (!File.Exists(Globals.mydocpath))
            {
                using (StreamWriter outputFile = new StreamWriter(Globals.mydocpath, true))
                {
                    outputFile.Write(Globals.Headers());
                }
            }

            using (StreamWriter outputFile = new StreamWriter(Globals.mydocpath, true))
            {
                outputFile.Write("\r\n" + msg);
            }



        }

        //Store

    }
 }

