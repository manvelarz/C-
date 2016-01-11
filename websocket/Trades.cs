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
                     TradeItem tr = new TradeItem() {   price = Math.Round(Convert.ToSingle(item[0]),2),
                                                        amount = Math.Round(Convert.ToSingle(item[1]),4),
                                                        data = item[2],
                                                        type = item[3] };
                     listofTrades.data.Add(tr);
                }


            return listofTrades;

        }
    }

    public class TradesTyped
    {
        public string channel { get; set; }
        public List<TradeItem> data { get; set; }

        public TradesTyped()
        {
            channel = "ok_btccny_trades";
            data = new List<TradeItem>();
        }

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

            ret.Add(new Tuple<string, float> ("askSum", result.First().askSum));

            ret.Add(new Tuple<string, float>("bidSum", result.First().bidSum));

            return ret;

        }

        public static void StoreTrades  (List<Tuple<string, float>> msg)
        {
            string output = "";

            string mydocpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\WriteLines.txt";  //TODO pti mi tegic karda

            using (StreamWriter outputFile = new StreamWriter(mydocpath, true))
            {


                outputFile.Write ( msg[0].Item1  + "," + msg[0].Item2 + ","+ msg[1].Item1 + "," + msg[1].Item2); 
                //output += Math.Round(msg[m][i].Item2, 0).ToString() + ","; 

            }

            Console.WriteLine(output);
        }

    }

    public class TradeItem
    {
        public TradeItem(string price, string amount, string data, string type)
        {
            this.price = Convert.ToSingle(price);
            this.amount = Convert.ToSingle(amount);
            this.data = data;
            this.type = type;

        }

        public TradeItem() { } 

        public double price { get; set; }
        public double amount { get; set; }
        public string data { get; set; }
        public string type { get; set; }

    }





}

