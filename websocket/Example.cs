﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using System.Threading;
namespace websocket
{
    class Example
    {
        static void Main(string[] args)
        {

            string url = "wss://real.okcoin.cn:10440/websocket/okcoinapi"; //国际站配置为"wss://real.okcoin.com:10440/websocket/okcoinapi"
            WebSocketService wss = new BuissnesServiceImpl();
            WebSocketBase wb = new WebSocketBase(url, wss);
            wb.start();

            // wb.send("{'event':'addChannel','channel':'ok_sub_spotcny_btc_depth_60'}");
            wb.send("[{'event':'addChannel','channel':'ok_sub_spotcny_btc_depth_60'},{'event':'addChannel','channel':'ok_sub_spotcny_btc_trades'}]");
            //wb.send("{'event':'addChannel','channel':'ok_btcusd_kline_1min'}");

            //发生断开重连时，需要重新订阅-- > Happened to disconnect and reconnect, you need to re - subscribe
            //while (true)
            //{
            //    if (wb.isReconnect())
            //    {

            //        wb.send("[{'event':'addChannel','channel':'ok_sub_spotcny_btc_depth_60'},{'event':'addChannel','channel':'ok_sub_spotcny_btc_trades'}]");
            //    }
            //    Thread.Sleep(1000);
            //    Console.WriteLine("RECCONECCTING.....");
            //}
            Console.ReadKey();
            Console.Title = "Puller";
            wb.stop();  //优雅的关闭，程序退出时需要关闭WebSocket连接 --> Shutdown gracefully exit the program needs to close WebSocket connection
        }
    }
}
