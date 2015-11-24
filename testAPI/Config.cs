using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace testAPI
{
    class Config
    {
        //版本
        private string _version = Properties.Settings.Default["version"].ToString();
        public string version
        {
            get { return _version; }
            set { _version = value; }
        }
        //是否為testmode
        private bool _testMode = (Boolean)Properties.Settings.Default["testMode"];
        public bool testMode {
            get { return _testMode; }
            set { _testMode = value; }
        }
        //COMPORT
        private string _serialPort = Properties.Settings.Default["serialPort"].ToString();
        public string[] serialPort {
            get {
                string[] subPort = _serialPort.Split(',');
                return subPort;
            }
            set { _serialPort = value.ToString(); }
        }
        //rate
        private int _baudRate = Convert.ToInt32(Properties.Settings.Default["baudRate"]);
        public int baudRate {
            get { return _baudRate; }
            set { _baudRate = value; }
        }


        //接收json的url
        private string _postURL = Properties.Settings.Default["postURL"].ToString();
        public string postURL {
            get { return _postURL; }
            set { _postURL = value; }
        }
        //接受第幾間廁所
        private int _toiletID = 0;
        public int toiletID {
            get { return _toiletID; }
            set { _toiletID = value; }
        }
        //command 命令
        private string _command = "none";
        public string command {
            get { return _command ;}
            set { _command = value; }
        }


        


    }
}
