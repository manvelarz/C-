using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace websocket
{
    public static class Globals
    {
        public static List<TradeItem> PastTradesTyped = new List<TradeItem>(); // Modifiable in Code
        public const Int32 VALUE = 10; // Unmodifiable
        public static string mydocpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\WriteLines.txt";  //TODO pti mi tegic karda
    }


    class OnRecive
    {
        public static bool isLastWastrade = false;
        public static bool after = false;
        

        public static void whatToDo(string msg)
        {
            if (msg != "{\"event\":\"pong\"}")
            {
                 

                JArray jo = JArray.Parse(msg);

                if (jo.First.Last.Path != "[0].success") // to avoid the first response data conflict
                {

                    foreach (JObject item in jo) // channels
                    {
                        string channel = item["channel"].ToString();

                        //Console.WriteLine(counter.ToString());

                        //counter += 1;

                        if (channel == "ok_sub_spotcny_btc_trades"  )
                        {

                            var model = item.ToObject<Trades>();

                            TradesTyped typed = Trades.typesFromString(model);

                            var TradeSumedAmount = TradesTyped.SumAmountByType(typed.data);

                            //TradesTyped.StoreTrades(TradeSumedAmount, typed);

                            Globals.PastTradesTyped.AddRange(typed.data);

                            if (!isLastWastrade)  // for summing extra trades wich come alone , without depth info

                            {
                                
                                TradeSumedAmount = TradesTyped.SumAmountByType(Globals.PastTradesTyped);

                                TradesTyped.StoreTrades(TradeSumedAmount, typed);

                                Globals.PastTradesTyped.RemoveRange(0, Globals.PastTradesTyped.Count);

                            }

                            isLastWastrade = true;

                        }

                        else if (channel == "ok_sub_spotcny_btc_depth_60")
                        {
                            isLastWastrade = false;

                            var model = item.ToObject<depthRoot>();

                            depthData.TransformAndStore(model.data);
                            
                            //Console.WriteLine(msg); 
                        }

                    }
                } // if         "success"
            }
          // Console.WriteLine("pong");

        }
    }
}
