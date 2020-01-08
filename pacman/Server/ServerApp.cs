using System;
using System.Threading;
using System.Windows.Forms;

namespace Server {
    static class ServerApp {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args) {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var sf = new ServerForm {Text = Constants.WINDOW_NAME};

            // TODO PUPPET REPLICATED SERVER CREATION
            for (var i = 0; i < args.Length; ++i) {
                switch (args[i]) {
                    case "-url":
                        var port = int.Parse(args[++i].Split(':')[1]);
                        sf.Port = port;
                        break;
                    case "-nplayers":
                        sf.NumberOfPlayers = args[++i];
                        break;
                    case "-msec":
                        sf.MsecPerRound = args[++i];
                        break;
                    case "-pid":
                        sf.PID = args[++i];
                        break;
                    case "-game":
                        sf.GameType = args[++i];
                        break;
                }
            }

            Application.Run(sf);
        }
    }
}
