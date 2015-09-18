namespace TCPServer
{
    using System;
    using System.Threading;

    /// <summary>
    /// Main class which starts everything
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The entry point of the program, where the program control starts and ends.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        public static void Main(string[] args)
        {
            // Phase = Phases.Init;
            var world = Core.Models.World.Instance;
            var controller = Core.Controllers.Controller.Instance;

            var api = Server.Controllers.APIController.Instance;

            controller.DefinitionManagerController = new Server.Controllers.DefinitionManagerController();
            world.AccountManager = new Server.Controllers.AccountManagerController();
            controller.RegionManagerController = new Server.Controllers.RegionManagerController();

            for (int threadNr = 0; threadNr < Server.Models.ServerConstants.ACTION_THREADS; ++threadNr)
            {
                var t = new Thread(new ParameterizedThreadStart(Server.Controllers.APIController.Instance.Worker));
                t.Start(threadNr);
            }

            var server = new TcpServer();

            server.Start();
        }
    }
}