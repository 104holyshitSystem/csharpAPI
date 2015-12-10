using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading;



namespace testAPI
{
    class Program
    {
        private static Config config = new Config();
        static void Main(string[] args)
        {
            bool open = config.testMode;
            string targetURL = config.postURL;
            Console.WriteLine("api版本 "+config.version);
            //serial use
            

            if (open)//測試模式
            {
                Console.WriteLine("測試模式。");
                Console.WriteLine("api目標為:" + targetURL);
                Console.WriteLine("廁所id ?");
                string toiletID = Console.ReadLine();
                //設定廁所id
                config.toiletID = toiletID;
                SendCommand newCommand = new SendCommand(config);
                open = testModeTick(open, newCommand);
            }
            else
            {
                Console.WriteLine("正式模式。");
                Console.WriteLine("api目標為:" + targetURL);
                //生成廁所ID
                int countToiletID = 1;
                foreach (string tioletPort in config.serialPort)
                {
                    //config.toiletID = countToiletID;
                    ThreadWork threadWork = new ThreadWork(config);
                    threadWork.threadSerialPort = tioletPort;
                    ThreadPool.QueueUserWorkItem(new WaitCallback(work), threadWork);
                    countToiletID++;
                }
            }


            
            //正式模式
            while (!config.testMode)
            {
                Console.ReadLine();
            }
            
            Console.WriteLine("任意鍵結束...");
            Console.ReadLine();
        }

        private static void work(object obj) {
            ThreadWork work = (ThreadWork)obj;
            Console.WriteLine("廁所待命接收訊息中,廁所ID:" + work.toiletID + ",位於:" + work.threadSerialPort);
            if(work.Start()){
                Console.WriteLine("廁所ID:" + work.toiletID.ToString()+" 發生異常");
            }
            

        }


        //測試用的程式
        private static bool testModeTick(bool open,SendCommand newCommand)
        {
            //testmode
            while (config.testMode && open == true)
            {
                Console.WriteLine("======================================");
                Console.WriteLine("1.發出廁所占用事件。\n2.發出廁所釋放事件。\n3.發出廁所鎖門事件。\n4.發出廁所開鎖事件。\n5.發出RFID事件。\n6.發出活體感應。");
                string command = Console.ReadLine();
                switch (command)
                {
                    case "1":
                        newCommand.occupy("test");
                        Console.WriteLine("已發送廁所占用事件。 toilet");
                        break;
                    case "2":
                        newCommand.release("test");
                        Console.WriteLine("已發送廁所釋放事件。 toilet");
                        break;
                    case "3":
                        newCommand.closelock("test");
                        Console.WriteLine("已發送廁所鎖門事件。 lock");
                        break;
                    case "4":
                        newCommand.openlock("test");
                        Console.WriteLine("已發送廁所開鎖事件。 lock");
                        break;
                    case "5":
                        newCommand.beepRFID("test","test");
                        Console.WriteLine("已發送RFID事件。 beep");
                        break;
                    case "6":
                        newCommand.bathHOT("test");
                        Console.WriteLine("已活體感應事件。 bathHOT");
                        break;
                    default:
                        open = false;
                        Console.WriteLine("不在動作清單中的命令。");
                        break;
                }
            }
            return open;
        }
    }

}
