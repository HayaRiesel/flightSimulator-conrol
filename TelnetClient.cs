using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;



namespace FlightSimulatorApp
{
    public class TelnetClient
    {
        private string sendLine;
        private string acceptLine;
        private volatile Boolean stop;
        private volatile Boolean doAction;

        public TelnetClient()
        {
            this.stop = true;
        }

        public void Connect(string ip, int port)
        {
            //connect to the server by tcp
            var client = new TcpClient(ip, port);
            NetworkStream ns = client.GetStream();
            //timeout for gettind a answer from the simulator
            client.ReceiveTimeout = 10000;
            stop = false;

            new Thread(delegate ()
            {
                while (!stop)
                {
                    if (doAction == true)
                    {
                        try
                        {
                           //send
                            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(sendLine + "\n");
                            ns.Write(bytesToSend, 0, bytesToSend.Length);

                            //recieve
                            byte[] bytes = new byte[1024];
                            try
                            {
                                int bytesRead = ns.Read(bytes, 0, bytes.Length);
                                acceptLine = Encoding.ASCII.GetString(bytes, 0, bytesRead);
                            }
                            catch
                            {
                                acceptLine = "the simulator dont replies";
                            }
                        }
                        catch
                        {
                            acceptLine = "ERROR - there is no connection to the server.";
                        }
                        doAction = false;
                    }
                }
                //close the connection
                client.Close();

            }).Start();

        }
        public void Write(string str)
        {
            //if there is no connection
            if(stop == true)
            {
                acceptLine = "need to connect by the connect window";
                return;
            }
            sendLine = str;
            doAction = true;
            //wait that the thread will finish
            while(doAction == true) { }
        }
        public string Read()
        {
            return this.acceptLine;
        }
        public void Disconnect()
        {
            this.stop = true;
            sendLine = "";
        }

    }
}
