using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace websocket
{
    class OnRecive
    {
        public static void whatToDo(string msg)
        {
            if (msg != "{\"event\":\"pong\"}")
            {

                JArray jo = JArray.Parse(msg);

                foreach (JObject item in jo) // channels
                {
                    string channel = item["channel"].ToString();

                    if (channel == "ok_btccny_trades")
                    {

                        var model = item.ToObject<Trades>();

                        TradesTyped typed = Trades.typesFromString(model);

                        var total = typed.data.Sum(x => x.amount);

                        var TradeSumedAmount = TradesTyped.SumAmountByType(typed.data);

                        TradesTyped.StoreTrades(TradeSumedAmount , typed);


                    }

                    else if (channel == "ok_btccny_depth60")
                    {
                        var model = item.ToObject<depthRoot>();

                        depthData.TransformAndWright(model.data);

                    }

                }
            }

        }
    }
}
