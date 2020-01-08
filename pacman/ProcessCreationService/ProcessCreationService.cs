using System;
using System.Diagnostics;
using System.Security.Policy;
using System.Text.RegularExpressions;
using Client;
using Server;

namespace ProcessCreationService {
    public class ProcessCreationService : MarshalByRefObject {
        private static readonly string CLIENT_FILENAME = System.Reflection.Assembly.GetAssembly(typeof(ClientForm)).Location;
        private static readonly string SERVER_FILENAME = System.Reflection.Assembly.GetAssembly(typeof(ServerForm)).Location;
        private static readonly Regex URLREGEX = new Regex(@"tcp:\/\/(?<url>[^:]+:\d+)\/.+");

        public void StartServer(string pid, string url, int msecPerRound, int numPlayers) {
            const string game = "Pacman";
            url = URLREGEX.Match(url).Groups["url"].Value;
            var args = $"-url {url} -msec {msecPerRound} -nplayers {numPlayers} -pid {pid} -game {game}";

            Console.WriteLine($@"Starting server {pid}");
            Process.Start(SERVER_FILENAME, args);
        }

        public void StartClient(string pid, string url, string serverUrl, int msecPerRound, int numPlayers, string filename = null) {
            url = URLREGEX.Match(url).Groups["url"].Value;
            serverUrl = URLREGEX.Match(serverUrl).Groups["url"].Value;
            var args = $"-url {url} -server {serverUrl} -msec {msecPerRound} -nplayers {numPlayers} -pid {pid}";
            if (filename != null) args += $" -trace {filename}";
            Console.WriteLine($@"Starting client {pid}");
            Process.Start(CLIENT_FILENAME, args);
        }
    }
}
