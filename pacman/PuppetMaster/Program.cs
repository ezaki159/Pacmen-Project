using System;
using System.IO;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Threading;

namespace PuppetMaster {
    class Program {
        static void Main(string[] args) {
            TcpChannel channel = new TcpChannel(0);
            ChannelServices.RegisterChannel(channel, false);
            if (args.Length == 1)
                using (var input = new StreamReader(args[0]))
                    HandleInput(input);
            else {
                HandleInput(Console.In);
            }
        }

        private static void HandleInput(TextReader input) {
            PuppetMaster puppetMaster = new PuppetMaster();
            string line;

            while ((line = input.ReadLine()) != null) {
                string[] tokens = line.Split(' ');
                switch (tokens[0]) {
                    case "exit":
                           return;
                    case "StartClient":
                        if (tokens.Length - 1 == 5)
                            new Thread(() =>
                            puppetMaster.StartClient(tokens[1], tokens[2], tokens[3], Int32.Parse(tokens[4]), Int32.Parse(tokens[5]))).Start();
                        else if (tokens.Length - 1 == 6)
                            new Thread(() =>
                            puppetMaster.StartClient(tokens[1], tokens[2], tokens[3], Int32.Parse(tokens[4]), Int32.Parse(tokens[5]), tokens[6])).Start();
                        else
                            Console.WriteLine("Usage: StartClient [pid] [pcs_URL] [client_URL] [msec_per_round] [num_players] [filename=null]");
                        break;
                    case "StartServer":
                        if (tokens.Length - 1 == 5)
                            new Thread(() =>
                            puppetMaster.StartServer(tokens[1], tokens[2], tokens[3], Int32.Parse(tokens[4]), Int32.Parse(tokens[5]))).Start();
                        else
                            Console.WriteLine("Usage: StartServer [pid] [pcs_URL] [server_URL] [msec_per_round] [num_players]");
                        break;
                    case "GlobalStatus":
                        if (tokens.Length - 1 == 0)
                            new Thread(() =>
                            puppetMaster.GlobalStatus()).Start();
                        else
                            Console.WriteLine("Usage: GlobalStatus");
                        break;
                    case "Crash":
                        if (tokens.Length - 1 == 1)
                            new Thread(() =>
                            puppetMaster.Crash(tokens[1])).Start();
                        else
                            Console.WriteLine("Usage: Crash [pid]");
                        break;
                    case "Freeze":
                        if (tokens.Length - 1 == 1)
                            new Thread(() =>
                            puppetMaster.Freeze(tokens[1])).Start();
                        else
                            Console.WriteLine("Usage: Freeze [pid]");
                        break;
                    case "Unfreeze":
                        if (tokens.Length - 1 == 1)
                            new Thread(() =>
                            puppetMaster.Unfreeze(tokens[1])).Start();
                        else
                            Console.WriteLine("Usage: Unfreeze [pid]");
                        break;
                    case "InjectDelay":
                        if (tokens.Length - 1 == 2)
                            new Thread(() =>
                            puppetMaster.InjectDelay(tokens[1], tokens[2])).Start();
                        else
                            Console.WriteLine("Usage: InjectDelay [src_pid] [dst_pid]");
                        break;
                    case "LocalState":
                        if (tokens.Length - 1 == 2)
                            new Thread(() =>
                            puppetMaster.LocalState(tokens[1], Int32.Parse(tokens[2]))).Start();
                        else
                            Console.WriteLine("Usage: LocalState [pid] [round_id]");
                        break;
                    case "Wait":
                        if (tokens.Length - 1 == 1)
                            puppetMaster.Wait(Int32.Parse(tokens[1]));
                        else 
                            Console.WriteLine("Usage: Wait [ms]");                     
                        break;
                    default:
                        Console.WriteLine("Invalid Command.");
                        break;
                }
            }
        }
    }
}
