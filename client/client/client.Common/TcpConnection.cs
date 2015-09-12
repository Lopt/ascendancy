namespace Client.Common
{
    using System;
    using System.Net;

    /// <summary>
    /// TCP connection. Provides an TCP Connection to write/read.
    /// </summary>
    public class TcpConnection
    {

        public virtual string Send(Core.Connection.MethodType methodType, string json)
        {
            throw new NotImplementedException();
        }

        public static TcpConnection Connector;
    }
}