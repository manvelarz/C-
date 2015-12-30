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
        public List<List<string>> strdata { get; set; }
       


    }

    public class aTrade
    {
        //public double price { get; set; }
        private float _price;

        public float price
        {
            get { return _price; }
            set { _price = Convert.ToSingle(value) ; }
        }

        //public double amount { get; set; }
        private float _amount;

        public float amount
        {
            get { return _amount; }
            set { _amount = Convert.ToSingle(value); }
        }
        
        public string data { get; set; }
        public string type { get; set; }

    }





}

