namespace TCPServer
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;
    using Server.Controllers;

    using Core.Connection;
   
    /// <summary>
    /// TCP server.
    /// </summary>
    public class TcpServer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TCPServer.TCPServer"/> class.
        /// </summary>
        public TcpServer()
        {
        }

        /// <summary>
        /// Start the server.
        /// </summary>
        public void Start()
        {
            m_listener = new TcpListener(IPAddress.Parse(Config.SERVER), Config.PORT);
                    
            try
            {
                m_listener.Start();
                Console.Write("Server running on {0}:{1}.\n", Config.SERVER, Config.PORT);
            }
            catch (Exception exception)
            {
                Console.Write("{2}\nServer starting error.\nMaybe there is already an server running on {0}:{1}?\n", Config.SERVER, Config.PORT, exception);
                return;
            }


            Console.Write("Debug: Test.\n");
            Running = true;
            while (Running)
            {
                var client = m_listener.AcceptTcpClient();
                Console.Write("New Client.\n");
                ThreadPool.QueueUserWorkItem(new WaitCallback(StartTcpThread), client);
            }
            Running = false;
        }

        /// <summary>
        /// Starts the TCP thread.
        /// </summary>
        /// <param name="stateInfo">TcpClient Object who connected.</param>
        private void StartTcpThread(Object stateInfo)
        {
            Console.Write("Client Thread opened.\n");

            var client = (TcpClient)stateInfo;
            try
            {
                var stream = client.GetStream();

                var packetIn = Packet.Receive(stream);
                if (packetIn.Content.Length <= Config.MAX_CONTENT_SIZE)
                {
                    var json = "";
                    switch (packetIn.MethodType)
                    {
                        case MethodType.Login:
                            Console.Write("Client Login.\n");
                            json = JSONController.Login(packetIn.Content);
                            break;
                        case MethodType.DoActions:
                            Console.Write("Client DoActions.\n");
                            json = JSONController.DoActions(packetIn.Content);
                            break;
                        case MethodType.LoadEntities:
                            Console.Write("Client LoadRegions.\n");
                            json = JSONController.LoadRegions(packetIn.Content);
                            break;
                        default:
                            Console.Write("Client Invalid Method?.\n");
                            json = JSONController.Default(packetIn.Content);
                            break;
                    }

                    var packetOut = new Packet();
                    packetOut.MethodType = packetIn.MethodType;
                    packetOut.Content = json;
                    packetOut.Send(stream);
                }
            }
            catch (Exception exception)
            {
                Console.Write("{0}", exception);
            }
            client.Close();
        }

        /// <summary>
        /// Stop listening.
        /// </summary>
        public void Stop()
        {
            m_listener.Stop();
        }

        /// <summary>
        /// true if the listener is running
        /// </summary>
        public bool Running
        {
            private set;
            get;
        }
            
        /// <summary>
        /// The TCP listener.
        /// </summary>
        private TcpListener m_listener;
    }
}