namespace TCPServer
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;

    using Core.Connection;
    using Server.Controllers;
   
    /// <summary>
    /// TCP server.
    /// </summary>
    public class TcpServer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TCPServer.TcpServer"/> class.
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
        /// Stop listening.
        /// </summary>
        public void Stop()
        {
            m_listener.Stop();
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="TCPServer.TcpServer"/> is running.
        /// </summary>
        /// <value><c>true</c> if running; otherwise, <c>false</c>.</value>
        public bool Running
        {
            get;
            private set;
        }

        /// <summary>
        /// Starts the TCP thread.
        /// </summary>
        /// <param name="stateInfo">TCPClient Object who connected.</param>
        private void StartTcpThread(object stateInfo)
        {
            Console.Write("Client Thread opened.\n");

            var client = (TcpClient)stateInfo;
            try
            {
                var stream = client.GetStream();

                var packetIn = Packet.Receive(stream);
                if (packetIn.Content.Length <= Config.MAX_CONTENT_SIZE)
                {
                    var json = string.Empty;
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
                            Console.Write("Client Invalid Method?\n");
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
        /// The TCP listener.
        /// </summary>
        private TcpListener m_listener;
    }
}