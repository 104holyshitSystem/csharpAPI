using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Net;
using System.IO;
using System.Collections.Specialized;


namespace testAPI
{
    class SendCommand
    {
        //版本
        string version;
        //廁所id
        string toiletID;
        //目標
        string targetURL;
        public SendCommand(Config info){
            toiletID = info.toiletID;
            targetURL = info.postURL;
            version = info.version;
        }
        //占用事件計時器
        long takeTimeOccupy = new DateTime(1970, 1, 1).Ticks;
        //鎖門事件計時器
        long takeTimeLock = new DateTime(1970, 1, 1).Ticks;

        //占用事件
        public void occupy(string id){
            //占用時間開始
            takeTimeOccupy = DateTime.UtcNow.Ticks;
            string result = createList("occupy",id);
            if (sendMessage(result)) {
                Console.WriteLine(result + " 成功。");
            }
            else
            {
                Console.WriteLine(result + " 失敗。"); 
            }
        }

        //釋放事件
        public void release(string id) {
            //占用時間結束
            long costtime = (DateTime.UtcNow.Ticks - takeTimeOccupy) / TimeSpan.TicksPerSecond;
            string result = createList("release",id, costtime.ToString());
            if (sendMessage(result))
            {
                Console.WriteLine(result + " 成功。");
            }
            else
            {
                Console.WriteLine(result + " 失敗。");
            }
        }

        //開鎖事件
        public void openlock(string id) {
            //占用時間結束
            long costtime = (DateTime.UtcNow.Ticks - takeTimeLock) / TimeSpan.TicksPerSecond;
            string result = createList("openlock",id, costtime.ToString());
            if (sendMessage(result))
            {
                Console.WriteLine(result + " 成功。");
            }
            else
            {
                Console.WriteLine(result + " 失敗。");
            }
        
        }
        //關鎖事件
        public void closelock(string id)
        {
            //占用時間開始
            takeTimeLock = DateTime.UtcNow.Ticks;
            string result = createList("closelock",id);
            if (sendMessage(result))
            {
                Console.WriteLine(result + " 成功。");
            }
            else
            {
                Console.WriteLine(result + " 失敗。");
            }

        }

        //RFID卡事件
        public void beepRFID(string id,string value="030ac102") {
            string result = createList("beepRFID",id, value);
            if (sendMessage(result))
            {
                Console.WriteLine(result + " 成功。");
            }
            else
            {
                Console.WriteLine(result + " 失敗。");
            }
        }

        //RF HOT 偵測事件
        public void bathHOT(string id)
        {
            string result = createList("bathHOT",id);
            if (sendMessage(result))
            {
                Console.WriteLine(result + " 成功。");
            }
            else
            {
                Console.WriteLine(result + " 失敗。");
            }
        }

        //傳送事件給web
        private bool sendMessage(string message) {
            bool success = false;
            try
            {
                WebClient wc = new WebClient();
                NameValueCollection nc = new NameValueCollection();
                nc["jsondata"] = message;
                byte[] result = wc.UploadValues(targetURL,nc);
                string resultBack = Encoding.UTF8.GetString(result);
                Console.WriteLine( targetURL+" 回傳:");
                Console.WriteLine(resultBack);
                success = true;
                Console.WriteLine("======================================");
            }
            catch (Exception)
            {
                success = false;
            }
            return success;
        }



        //創造json
        private string createList(string command,string id,string value="")
        {
            //取得unix 時間
            long epochTicks = new DateTime(1970, 1, 1).Ticks;
            long unixTime = ((DateTime.UtcNow.Ticks - epochTicks) / TimeSpan.TicksPerSecond);
            //創造清單
            List<Toilet> jsonlist = new List<Toilet>();
            switch (command)
            {
                case "occupy":
                    jsonlist.Add(new Toilet { version = version, toiletID = id, command = "toilet", value = "true", unixtime = unixTime });
                    break;
                case "release":
                    jsonlist.Add(new Toilet { version = version, toiletID = id, command = "toilet", value = "false", unixtime = unixTime,taketime = Convert.ToInt32(value) });
                    break;
                case "closelock":
                    jsonlist.Add(new Toilet { version = version, toiletID = id, command = "lock", value = "true", unixtime = unixTime });
                    break;
                case "openlock":
                    jsonlist.Add(new Toilet { version = version, toiletID = id, command = "lock", value = "false", unixtime = unixTime, taketime = Convert.ToInt32(value) });
                    break;
                case "beepRFID":
                    jsonlist.Add(new Toilet { version = version, toiletID = id, command = "beep", value = value, unixtime = unixTime });
                    break;
                case "bathHOT":
                    jsonlist.Add(new Toilet { version = version, toiletID = id, command = "bathHOT", value = "true", unixtime = unixTime });
                    break;
                default:
                    break;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string result = serializer.Serialize(jsonlist);
            return result;
        }
    }
  
}
