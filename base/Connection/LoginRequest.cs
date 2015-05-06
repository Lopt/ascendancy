using System;

namespace @base.connection
{
    public class LoginRequest : Request
    {
        public LoginRequest(model.Position position, string username, string password)
            : base(Guid.Empty, position)
        {
            m_username = username;
            m_password = password;
        }
           
        public string Username
        {
            get { return m_username; }
            set { m_username = value; }
        }

        public string Password
        {
            get { return m_password; }
            set { m_password = value; }
        }

        string m_username;
        string m_password;
    }
}

