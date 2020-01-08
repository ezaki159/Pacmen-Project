using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Client {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args) {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
            var cf = new ClientForm();

            for (var i = 0; i < args.Length; ++i) {
                switch (args[i]) {
                    case "-url":
                        cf.ClientPort = int.Parse(args[++i].Split(':')[1]);
                        break;
                    case "-nplayers":
                        break;
                    case "-msec":
                        break;
                    case "-pid":
                        cf.PID = args[++i];
                        cf.Nickname = cf.PID;
                        break;
                    case "-server":
                        var address = args[++i].Split(':');
                        cf.ServerUrl = address[0];
                        cf.ServerPort = address[1];
                        break;
                    case "-trace":
                        cf.TracefilePath = args[++i];
                        break;
                }
            }

            Application.Run(cf);
        }
    }
}
