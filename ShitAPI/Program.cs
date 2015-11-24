using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;

namespace ShitAPI
{
    class Program
    {
        /*
        static void Main(string[] args)
        {
            Socket mainSocket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
            IPEndPoint ipLocal = new IPEndPoint(IPAddress.Any,9999);
            mainSocket.Bind(ipLocal);
            mainSocket.Listen(4);
            SerialPort serialPort = new SerialPort("COM4",9600,Parity.None,8,StopBits.One);
            mainSocket.BeginAccept(new AsyncCallback(checkCM),null);
            //try
            //{
            //    serialPort.Open();
            //    while (true)
            //    {
            //        Console.WriteLine(serialPort.ReadLine());
            //    }
                

            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("open error " + ex.Message);
            //}
            //finally {
            //    serialPort.Close();
            //}
            Console.ReadLine();
        }
        static private void checkCM(IAsyncResult ar)
        {
            
            //Console.WriteLine("echo ok");
        }

        */

    }

}
