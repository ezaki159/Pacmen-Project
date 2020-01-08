using System;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;

namespace ProcessCreationService {
    class Program {
        private static readonly int PCS_PORT = 11000;
        static void Main(string[] args) {
            TcpChannel channel = new TcpChannel(PCS_PORT);

            ProcessCreationService pcs = new ProcessCreationService();
            ChannelServices.RegisterChannel(channel, false);
            RemotingServices.Marshal(
                pcs,
                "PCS",
                typeof(ProcessCreationService)
            );
            Console.WriteLine($"PCS running on port {PCS_PORT}.");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
