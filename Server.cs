using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace EasyServer
{
    public class ServerAsync
    {
        public Task[] appserver = new Task[2];
        // Ein Server der mit der Task Libary arbeitet.

        public int port { get; set; } = 40300;
        public string IP4s { get; private set; } 
        public byte[] buffer { get; set; } = new byte[1024];
        private int byteAnzahl { get; set; }
        public Socket connectionSocket { get; set; }
        public NetworkStream networkStream;
        public string message { get; set; }

        public string informationMessage;

        IPAddress IP4;
        IPEndPoint IPE;
        TcpListener listener;
        private static string GetLocalIpAdresse()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Local IP Address Not Found!");
            
        }


        public void Start()
        {

            informationMessage = "Beginne";
            IP4s = GetLocalIpAdresse();
            IP4 = IPAddress.Parse(IP4s);
            IPE = new IPEndPoint(IP4, port);
            listener = new TcpListener(IPE);
            appserver[0] = Task.Run(async () =>
            {

                informationMessage = "Verbindungssuche.";
                try
                {
                    listener.Start();
                    connectionSocket = await listener.AcceptSocketAsync();
                    informationMessage = "Connected";
                    networkStream = new NetworkStream(connectionSocket);
                }
                catch (SocketException)
                {
                    informationMessage = "WTF";
                    listener.Stop();
                    Start();
                }


            }
            ).ContinueWith((appservice) => Receive());

        }
        public void Send(string msg)
        {
            try
            {
                connectionSocket.Send(Encoding.UTF8.GetBytes(msg));
            }

            catch
            {
                informationMessage = "Senden ist nicht möglich";
            }
        }
        public void Receive()
        {

            while (connectionSocket.Connected)
            {
                Thread.Sleep(100);
                if (networkStream.DataAvailable)
                {
                    byteAnzahl = networkStream.Read(buffer, 0, buffer.Length);
                    message = Encoding.UTF8.GetString(buffer, 0, byteAnzahl);


                }
            }


        }




    }
}
