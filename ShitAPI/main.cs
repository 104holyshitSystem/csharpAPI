using System;
using System.Net;
using System.Threading;
using System.Linq;
using System.Text;
using ShitSensor;
using System.IO;
using System.IO.Ports;


class Program
{
    static void Main(string[] args)
    {
        

        WebServer ws = new WebServer(SendResponse, "http://localhost:8080/test/");
        WebServer info = new WebServer(SendInfo, "http://localhost:8080/info/");
        ws.Run();
        info.Run();
        Console.WriteLine("A ShitSensor webserver. Press a key to quit.");
        Console.ReadKey();
        ws.Stop();
        info.Stop();
    }
 
    public static string SendResponse(HttpListenerRequest request)
    {
        return string.Format("<HTML><BODY>My web page.<br>{0}</BODY></HTML>", DateTime.Now);    
    }
    public static string SendInfo(HttpListenerRequest request) {
        string context = "";
        Boolean succeeded = false;
        int count = 0;
        SerialPort serialPort = new SerialPort("COM3", 9600, Parity.None, 8, StopBits.One);
        while (!succeeded)
        {
            try
            {
                serialPort.Open();
                int cm1 = Convert.ToInt32(serialPort.ReadLine());
                int cm2 = Convert.ToInt32(serialPort.ReadLine());
                if (cm1 >= 30)
                {
                    context = "false:" + cm1.ToString();
                    Console.WriteLine(context);
                }
                else
                {
                    context = "true:" + cm1.ToString();
                    Console.WriteLine(context);
                }
                succeeded = true;
                if (cm1 != cm2)
                {
                    succeeded = false;
                    count++;
                }
                if (count>=9){
                    context = "false:outOfRange";
                    Console.WriteLine(context);
                    succeeded = true;
                }
            }
            catch (Exception ex)
            {
                context = "open error " + ex.Message;
            }
            finally
            {
                serialPort.Close();
            }
        }
        return string.Format(context);
    }


    
}