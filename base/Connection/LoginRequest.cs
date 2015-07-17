using System;

namespace Core.Connections
{   
    /// <summary>
    /// Request class which should be used to login.
    /// Should be serialised before sending, will be deserialised after recieving.
    /// </summary>
    public class LoginRequest : Request
    {
        public LoginRequest(Core.Models.Position position, string username, string password)
            : base(Guid.Empty, position)
        {
            Username = username;
            Password = password;
        }

        public string Username;
        public string Password;
    }
}

