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
        public static Int32 DeptCount = 100; // Unmodifiable
        public static string mydocpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\WriteLines.txt";  //TODO pti mi tegic karda
       // public static string Headers = "Timestump\tUp\tDown\t10\t9\t8\t7\t6\t5\t4\t3\t2\t1\t00\t-1\t-2\t-3\t-4\t-5\t-6\t-7\t-8\t-9\t-10\tdate\tAsksum\tBidsum";

        //public static string Headers100 = "Timestump\tUp\tDown\t100\t99\t98\t97\t96\t95\t94\t93\t92\t91\t90\t89\t88\t87\t86\t85\t84\t83\t82\t81\t80\t79\t78\t77\t76\t75\t74\t73\t72\t71\t70\t69\t68\t67\t66\t65\t64\t63\t62\t61\t60\t59\t58\t57\t56\t55\t54\t53\t52\t51\t50\t49\t48\t47\t46\t45\t44\t43\t42\t41\t40\t39\t38\t37\t36\t35\t34\t33\t32\t31\t30\t29\t28\t27\t26\t25\t24\t23\t22\t21\t20\t19\t18\t17\t16\t15\t14\t13\t12\t11\t10\t09\t08\t07\t06\t05\t04\t03\t02\t01t\00\t-01\t-02\t-03\t-04\t-05\t-06\t-07\t-08\t-09\t-10\t-11\t-12\t-13\t-14\t-15\t-16\t-17\t-18\t-19\t-20\t-21\t-22\t-23\t-24\t-25\t-26\t-27\t-28\t-29\t-30\t-31\t-32\t-33\t-34\t-35\t-36\t-37\t-38\t-39\t-40\t-41\t-42\t-43\t-44\t-45\t-46\t-47\t-48\t-49\t-50\t-51\t-52\t-53\t-54\t-55\t-56\t-57\t-58\t-59\t-60\t-61\t-62\t-63\t-64\t-65\t-66\t-67\t-68\t-69\t-70\t-71\t-72\t-73\t-74\t-75\t-76\t-77\t-78\t-79\t-80\t-81\t-82\t-83\t-84\t-85\t-86\t-87\t-88\t-89\t-90\t-91\t-92\t-93\t-94\t-95\t-96\t-97\t-98\t-99\t-100\tdate\tAsksum\tBidsum";
        //                                  "Timestump\tUp\tDown\t100\t99\t98\t97\t96\t95\t94\t93\t92\t91\t90\t89\t88\t87\t86\t85\t84\t83\t82\t81\t80\t79\t78\t77\t76\t75\t74\t73\t72\t71\t70\t69\t68\t67\t66\t65\t64\t63\t62\t61\t60\t59\t58\t57\t56\t55\t54\t53\t52\t51\t50\t49\t48\t47\t46\t45\t44\t43\t42\t41\t40\t39\t38\t37\t36\t35\t34\t33\t32\t31\t30\t29\t28\t27\t26\t25\t24\t23\t22\t21\t20\t19\t18\t17\t16\t15\t14\t13\t12\t11\t10\t09\t08\t07\t06\t05\t04\t03\t02\t01\t00\t-01\t-02\t-03\t-04\t-05\t-06\t-07\t-08\t-09\t-10\t-11\t-12\t-13\t-14\t-15\t-16\t-17\t-18\t-19\t-20\t-21\t-22\t-23\t-24\t-25\t-26\t-27\t-28\t-29\t-30\t-31\t-32\t-33\t-34\t-35\t-36\t-37\t-38\t-39\t-40\t-41\t-42\t-43\t-44\t-45\t-46\t-47\t-48\t-49\t-50\t-51\t-52\t-53\t-54\t-55\t-56\t-57\t-58\t-59\t-60\t-61\t-62\t-63\t-64\t-65\t-66\t-67\t-68\t-69\t-70\t-71\t-72\t-73\t-74\t-75\t-76\t-77\t-78\t-79\t-80\t-81\t-82\t-83\t-84\t-85\t-86\t-87\t-88\t-89\t-90\t-91\t-92\t-93\t-94\t-95\t-96\t-97\t-98\t-99\t-100\tdate\tAsksum\tBidsum"   
        public static string Headers() 
        {
            string str = "Timestump\tUp\tDown";

            for (int i = 0; i < DeptCount; i++)
            {
                str += "\t" + (DeptCount - i).ToString("00");
            }
            str += "\t00";
            for (int i = 0; i < DeptCount; i++)
            {
                str += "\t-" + (i + 1).ToString("00");
            }
            str += "\tdate\tAsksum\tBidsum";
            return str;
        }

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

                            var str = depthData.FullLineOfString(model.data, Globals.DeptCount);

                            depthData.WrightToFile(str);

                            //depthData.TransformAndStore(model.data);
                            
                            //Console.WriteLine(msg); 
                        }

                    }
                } // if         "success"
            }
          // Console.WriteLine("pong");

        }
    }
}
