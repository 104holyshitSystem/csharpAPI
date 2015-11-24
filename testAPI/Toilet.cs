using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace testAPI
{
    class Toilet
    {
        private string _value = "";
        public string version { get; set; }
        public int toiletID { get; set; }
        public string command { get; set; }
        public string value { get { return _value; } set { _value = value; } }
        public long unixtime { get; set; }
        
        
    }
}
