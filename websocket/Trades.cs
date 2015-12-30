using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace websocket
{



    public class Trades
    {
        public string channel { get; set; }
        public List<List<string>> data { get; set; }
    }

    public class aTrade
    {
        public double price { get; set; }
        public double amount { get; set; }
        public string data { get; set; }
        public string type { get; set; }

    }





}

