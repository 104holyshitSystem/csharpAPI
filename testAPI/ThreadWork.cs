using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;


namespace testAPI
{
    class ThreadWork : Config
    {
        //接收本來的變數
        Config config;
        //擴充用的變數 拿來存給thread用的port name
        private string _threadSerialPort = "";
        
        public string threadSerialPort
        {
            get { return _threadSerialPort; }
            set { _threadSerialPort = value; }
        }
        public ThreadWork(Config info)
        {
            config = info;
            postURL = config.postURL;
            version = config.version;
        }
        public bool Start(){
            bool success = false;
            SerialPort serialPort;
            try
            {

                SendCommand newCommand = new SendCommand(config);
                serialPort = new SerialPort(threadSerialPort, baudRate, Parity.None, 8, StopBits.One);
                serialPort.Open();
                while (true)
                {
                    string message = serialPort.ReadLine().Trim();
                    string[] submessage = message.Split(':');
                    config.toiletID = submessage[2];
                    switch (submessage[0])
                    {
                        case "Lock":
                            if (submessage[1]=="open")
                            {
                                newCommand.openlock(submessage[2]);      
                            }
                            if (submessage[1]=="close")
                            {
                                newCommand.closelock(submessage[2]);
                                
                            }
                            break;
                        case "Toilet":
                            if (submessage[1]=="in")
                            {
                                newCommand.occupy(submessage[2]);      
                            }
                            if (submessage[1]=="out")
                            {
                                newCommand.release(submessage[2]);
                            }
                            break;
                        case "beep":
                            newCommand.beepRFID(submessage[2],submessage[1]);
                            break;
                        case "Bath":
                            if (submessage[1] == "in")
                            {
                                newCommand.bathHOT(submessage[2]);
                            }
                            break;
                        default:
                            //Console.WriteLine("什麼都沒做。");
                            break;
                    }
                    
                }
            }
            catch (Exception)
            {
                success = false;
            }
            return success;
        }
        
    }
}
