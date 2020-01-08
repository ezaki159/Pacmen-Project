using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Threading;
using System.Windows.Forms;
using CommonInterfaces;
using System.Collections;
using System.Runtime.Serialization.Formatters;

namespace Server {
    public partial class ServerForm : Form {
        private ServerService _gameServer;
        private TcpChannel _channel;
        public string ServerName = "Server";

        public string MsecPerRound {
            get => inputTimeTB.Text;
            set => inputTimeTB.Text = value;
        }

        public string NumberOfPlayers {
            get => nPlayersTB.Text;
            set => nPlayersTB.Text = value;
        }

        public int Port {
            get => int.Parse(portTB.Text);
            set => portTB.Text = "" + value;
        }

        public string PID { get; set; } = null;

        public string GameType {
            get => gameTypeCB.Text;
            set {
                if (gameTypeCB.Items.Contains(value))
                    gameTypeCB.Text = value;
            }
        }

        public bool CanStartPrimary() => !String.IsNullOrEmpty(nPlayersTB.Text) &&
                                         !String.IsNullOrEmpty(inputTimeTB.Text) &&
                                         !String.IsNullOrEmpty(gameTypeCB.Text) &&
                                         !String.IsNullOrEmpty(portTB.Text) && !replicaCB.Checked;

        public bool CanStartReplica() => !String.IsNullOrEmpty(primaryAddressTB.Text) &&
                                         !String.IsNullOrEmpty(primaryPortTB.Text) &&
                                         !String.IsNullOrEmpty(gameTypeCB.Text) &&
                                         !String.IsNullOrEmpty(portTB.Text) && replicaCB.Checked;

        public ServerForm() {
            InitializeComponent();
        }

        public void AddPlayer(string nick) {
            playerListTB.AppendText(nick + Environment.NewLine);
        }

        public void UpdateServerStatusLabel(string newStatus, System.Drawing.Color color) {
            serverStatLabel2.Text = newStatus;
            serverStatLabel2.ForeColor = color;
        }

        public void UpdateServerLogTB(string msg) {
            DateTime date = DateTime.Now;
            serverLogTB.Text += "[" + date.ToString("HH:mm:ss") + "]: " + msg + Environment.NewLine; // [hh:mm:ss]: msg
        }

        internal void startButton_Click(object sender, EventArgs e) {
            CreateChannel();
            String serverID;

            switch (gameTypeCB.Text) {
                case "Pacman":
                    _gameServer = new PacmanServerService(
                        Int32.Parse(nPlayersTB.Text),
                        Int32.Parse(inputTimeTB.Text),
                        this
                    );
                    break;
                default:
                    throw new NotImplementedException("Unsupported Game Type");
            }
            if (PID != null)
                _gameServer.SetPid(PID);
            serverID = RegisterServer(_gameServer);

            _gameServer.Id = serverID + Port;

            Thread t = new Thread(_gameServer.GameStart);
            t.IsBackground = true;
            t.Start();

            UpdateServerLogTB("Running " + gameTypeCB.Text + ".");
            DisableInputGUI();
        }

        private void KeyPressNumericOnly(object sender, KeyPressEventArgs e) {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) {
                e.Handled = true;
            }
        }

        private String RegisterServer(ServerService server) {
            ObjRef objRef = RemotingServices.Marshal(
                server,
                ServerName
            );
            return objRef.URI;
        }

        private void UpdatePrimaryStartButton(object sender, EventArgs e) {
            startButton.Enabled = CanStartPrimary();
        }

        private void UpdateReplicaStartButton(object sender, EventArgs e) {
            replicaStartButton.Enabled = CanStartReplica();
        }

        private void richTextBox_TextChanged(object sender, EventArgs e) {
            // set the current caret position to the end
            serverLogTB.SelectionStart = serverLogTB.Text.Length;
            // scroll it automatically
            serverLogTB.ScrollToCaret();
        }

        private void ServerForm_Load(object sender, EventArgs e) {
            if (CanStartPrimary())
                startButton.PerformClick();
        }

        private void replicaCB_CheckedChanged(object sender, EventArgs e) {
            primaryAddressTB.Enabled = replicaCB.Checked;
            primaryPortTB.Enabled = replicaCB.Checked;
            nPlayersTB.Enabled = !replicaCB.Checked;
            inputTimeTB.Enabled = !replicaCB.Checked;
        }

        private void replicaStartButton_Click(object sender, EventArgs e) {
            CreateChannel();

            switch (gameTypeCB.Text) {
                case "Pacman":
                    _gameServer = new PacmanServerService(
                        this
                    );
                    break;
                default:
                    throw new NotImplementedException("Unsupported Game Type");
            }

            if (Connect(_gameServer, primaryAddressTB.Text, int.Parse(primaryPortTB.Text))) {
                Thread t = new Thread(_gameServer.GameStart);
                t.IsBackground = true;
                t.Start();

                UpdateServerLogTB("Running " + gameTypeCB.Text + ".");
                UpdateServerLogTB("Replica of " + primaryAddressTB.Text + ":" + primaryPortTB.Text + ".");
                DisableInputGUI();
            }
        }

        private void DisableInputGUI() {
            startButton.Enabled = false;
            inputTimeTB.Enabled = false;
            nPlayersTB.Enabled = false;
            gameTypeCB.Enabled = false;
            replicaCB.Enabled = false;
            primaryAddressTB.Enabled = false;
            primaryPortTB.Enabled = false;
            replicaStartButton.Enabled = false;
            portTB.Enabled = false;
        }

        private void PortTextChanged(object sender, EventArgs e) {
            Port = Int32.Parse(portTB.Text);
        }

        private void CreateChannel() {
            BinaryServerFormatterSinkProvider provider = new BinaryServerFormatterSinkProvider();
            provider.TypeFilterLevel = TypeFilterLevel.Full;
            IDictionary props = new Hashtable();
            props["port"] = Port;
            _channel = new TcpChannel(props, null, provider);

            ChannelServices.RegisterChannel(_channel, false);
        }

        private bool Connect(ServerService replica, string serverAddr, int port) {
            bool success = false;
            string serverName = $"tcp://{serverAddr}:{port}/{ServerName}";
            try {
                String serverID;
                IServerService serverService = (IServerService)Activator.GetObject(
                    typeof(IServerService),
                    serverName);

                if (PID != null) {
                    _gameServer.SetPid(PID);
                }
                serverID = RegisterServer(_gameServer);

                replica.Id = serverID + Port;
                replica.PrimaryServer = serverService;
                serverService.RegisterReplica(replica, replica.Id);

                success = true;
            } catch (Exception e) { // TODO FIXME EXCEPTION ?!?
                throw e;
            }

            return success;
        }
    }
}
