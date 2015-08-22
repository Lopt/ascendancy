namespace Core.Connections
{   
    using System;

    /// <summary>
    /// Request class which should be used to send actions to the server.
    /// Should be serialized before sending, will be deserialized after receiving.
    /// </summary>
    public class LoginRequest : Request
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Core.Connections.LoginRequest"/> class.
        /// </summary>
        /// <param name="position">Position where the user is standing (converted GPS information).</param>
        /// <param name="username">User Name.</param>
        /// <param name="password">Password of the user.</param>
        public LoginRequest(Core.Models.Position position, string username, string password)
            : base(Guid.Empty, position)
        {
            Username = username;
            Password = password;
        }

        /// <summary>
        /// The Name of the user.
        /// </summary>
        public string Username;

        /// <summary>
        /// The password of the user.
        /// </summary>
        public string Password;
    }
}