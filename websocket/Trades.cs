using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;

namespace websocket
{
    public class Trades 
    {
        public Trades() { }

        public string channel { get; set; }

        public List<List<string>> data { get; set; }




        public static TradesTyped typesFromString(Trades input)

        {
            TradesTyped listofTrades = new TradesTyped();

            foreach (List<string> item in input.data)
                {
                TradeItem tr = new TradeItem() {        Unixtime = Convert.ToSingle(item[0]),
                                                        price = Math.Round(float.Parse(item[1], System.Globalization.CultureInfo.InvariantCulture),2),
                                                        amount = Math.Round(float.Parse(item[2], System.Globalization.CultureInfo.InvariantCulture), 4),
                                                        data = item[3],
                                                        type = item[4] };
                     listofTrades.data.Add(tr);
                }


            return listofTrades;

        }
    }

    public class TradesTyped : List<TradesTyped>, IEnumerable<TradesTyped>
    {
        public string channel { get; set; }
        public List<TradeItem> data { get; set; }

        public TradesTyped()
        {
            channel = "ok_btccny_trades";
            data = new List<TradeItem>();
        }

        //public  List<TradeItem> Add(List<TradeItem> value1)
        //{

        //    this.AddRange(value1);

        //    return (value1);

        //}



        public static List<Tuple<string, float>> SumAmountByType(List<TradeItem> data)
        {
            var ret = new List<Tuple<string, float>>();

            var result = data.GroupBy(i => 1)
                            .Select(i => new
                            {
                                askSum = i.Where(j => j.type == "ask").Sum(k => (float)k.amount),
                                bidSum = i.Where(j => j.type == "bid").Sum(k => (float)k.amount)
                            });

            // Console.WriteLine(result.First().askSum);

            ret.Add(new Tuple<string, float>("askSum", result.First().askSum));

            ret.Add(new Tuple<string, float>("bidSum", result.First().bidSum));

            return ret;

        }

        public static void StoreTrades(List<Tuple<string, float>> msg, TradesTyped typed)
        {
            string output = "";
            string tab = "\t";

            string mydocpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\WriteLines.txt";  //TODO pti mi tegic karda

            using (StreamWriter outputFile = new StreamWriter(mydocpath, true))
            {


                outputFile.Write((Convert.ToDateTime(typed.data.First().data)) + tab + msg[0].Item2 + tab + +msg[1].Item2);  //

                output += msg[0].Item2 + tab + msg[1].Item2 + tab; // [0]-> Asksum , [1]-> Bidsum
                var diff = Math.Round(msg[1].Item2 - msg[0].Item2, 1);
                WrightInColors(diff);

            }

            //Console.WriteLine(output);
            Console.Write(output);
        }
        /// <summary>
        /// ++ It Wright in green positiv an in red negative numbers
        /// </summary>
        /// <param name="diff"></param>
        public static void WrightInColors(double diff)
        {

            if (diff > 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(diff.ToString("0.0") + "\t"); // The difference of asksum and bidsum
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(Math.Abs(diff).ToString("0.0") + "\t"); // The difference of asksum and bidsum
                Console.ResetColor();
            }


        }

    }

        public class TradeItem
    {
        public TradeItem(string Unixtime, string price, string amount, string data, string type)
        {
            this.Unixtime = Convert.ToDouble(Unixtime);
            this.price = Convert.ToDouble(price);
            this.amount = Convert.ToDouble(amount);
            this.data = data;
            this.type = type;

        }

        public TradeItem() { } 

        public double Unixtime { get; set; }
        public double price { get; set; }
        public double amount { get; set; }
        public string data { get; set; }
        public string type { get; set; }

    }





}

