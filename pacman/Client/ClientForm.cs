using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommonInterfaces;

namespace Client {
    public partial class ClientForm : Form {
        private ClientService _gameClient;
        private TcpChannel _channel;
        private Input _input = new Input();
        private bool _holdUp = false;
        private bool _holdDown = false;
        private bool _holdRight = false;
        private bool _holdLeft = false;
        private string ServerName = "Server"; // FIXME resource file

        public ClientForm() {
            InitializeComponent();
        }

        public string ClientName { get; set; } = "Client";
        public string PID { get; set; }

        public string TracefilePath {
            get => tracefileTB.Text;
            set {
                tracefileCB.Checked = true;
                tracefileTB.Text = value;               
            }
        }

        public string ServerUrl {
            get => serverAddressTB.Text;
            set => serverAddressTB.Text = value;
        }

        public string Nickname {
            get => nicknameTB.Text;
            set => nicknameTB.Text = value;
        }

        public string ServerPort {
            get => portTB.Text;
            set => portTB.Text = value;
        }

        public int ClientPort { get; set; } = 0;

        internal void AddMessage(string nickname, string text) {
            chatTB.Text += $"{nickname}: {text}" + Environment.NewLine;
        }

        internal bool CanConnect() => !string.IsNullOrEmpty(portTB.Text) && !string.IsNullOrEmpty(serverAddressTB.Text) &&
                                     !string.IsNullOrEmpty(nicknameTB.Text);

        private void UpdateConnectButton(object sender, EventArgs e) => connectButton.Enabled = CanConnect();

        private void KeyPressNumericOnly(object sender, KeyPressEventArgs e) {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) {
                e.Handled = true;
            }
        }

        private void chatMsgTB_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                var text = chatMsgTB.Text;
                if (!String.IsNullOrWhiteSpace(text)) {
                    AddMessage(_gameClient.GetNickname(), text);
                    new Thread(() => _gameClient.SendMessage(text)).Start();
                    chatMsgTB.Clear();
                }
                chatMsgTB.Enabled = false;
                Focus();
            }
        }

        internal void connectButton_Click(object sender, EventArgs e) {
            if (_channel == null) {
                var provider = new BinaryServerFormatterSinkProvider();
                provider.TypeFilterLevel = TypeFilterLevel.Full;
                IDictionary props = new Hashtable();
                props["port"] = ClientPort;
                _channel = new TcpChannel(props, null, provider);
                ChannelServices.RegisterChannel(_channel, false);
            }
            if (_gameClient == null) {
                if (tracefileCB.Checked)
                    _gameClient = new PacmanClientService(nicknameTB.Text, this, tracefileTB.Text);
                else
                    _gameClient = new PacmanClientService(nicknameTB.Text, this);
            } else {
                _gameClient.Nickname = nicknameTB.Text;
                if (tracefileCB.Checked)
                    _gameClient.TracefilePath = tracefileTB.Text;
                else
                    _gameClient.TracefilePath = null;
            }



            if (Connect(_gameClient, serverAddressTB.Text, Int32.Parse(portTB.Text))) {
                new Thread(_gameClient.InitTracefile).Start();
                serverAddressLabel.Visible = false;
                serverAddressTB.Visible = false;
                portLabel.Visible = false;
                portTB.Visible = false;
                nicknameLabel.Visible = false;
                nicknameTB.Visible = false;
                connectButton.Visible = false;
                tracefileLabel.Visible = false;
                tracefileTB.Visible = false;
                tracefileCB.Visible = false;
                chatTB.Location = new System.Drawing.Point(chatTB.Location.X, chatTB.Location.Y - 117);
                chatTB.Size = new System.Drawing.Size(chatTB.Size.Width, chatTB.Size.Height + 112);

                KeyDown += Keyisdown;
                KeyUp += Keyisup;
                Focus();
            }
        }

        public void ChangeWindowText(string result) {
            this.Text += " " + result;
        }

        private bool Connect(ClientService client, string serverAddr, int port) {
            bool success = false;
            string serverName = $"tcp://{serverAddr}:{port}/{ServerName}";
            try {
                IServerService serverService = (IServerService) Activator.GetObject(
                    typeof(IServerService),
                    serverName);
                if (PID != null)
                    client.SetPid(PID);
                RegisterClient(client, serverService);

                serverService.RegisterPlayer(client);
                success = true;             
            }
            catch (Exception e) { // TODO FIXME EXCEPTION ?!?
                throw e;
            }

            return success;
        }

        private void RegisterClient(ClientService client, IServerService server) {
            client.ServerService = server;
            RemotingServices.Marshal(
                client,
                ClientName
            );
        }

        private void Keyisdown(object sender, KeyEventArgs e) {
            bool send = false;      
            if (e.KeyCode == (Keys) KeyBind.Up && !_holdUp) {
                _input.Direction |= Direction.Up;          
                send = true;
                _holdUp = true;
            }
            if (e.KeyCode == (Keys) KeyBind.Down && !_holdDown) {
                _input.Direction |= Direction.Down;
                send = true;
                _holdDown = true;
            }
            if (e.KeyCode == (Keys) KeyBind.Right && !_holdRight) {
                _input.Direction |= Direction.Right;
                send = true;
                _holdRight = true;
            }
            if (e.KeyCode == (Keys) KeyBind.Left && !_holdLeft) {
                _input.Direction |= Direction.Left;
                send = true;
                _holdLeft = true;
            }
            if (e.KeyCode == Keys.Enter) {
                chatMsgTB.Enabled = true;
                chatMsgTB.Focus();
            }

            if (send)
                _gameClient.UpdateInput(_input);
        }

        private void Keyisup(object sender, KeyEventArgs e) {
            bool send = false;
            if (e.KeyCode == (Keys) KeyBind.Up && _holdUp) {
                _input.Direction &= ~Direction.Up;
                send = true;
                _holdUp = false;
            }
            if (e.KeyCode == (Keys) KeyBind.Down && _holdDown) {
                _input.Direction &= ~Direction.Down;
                send = true;
                _holdDown = false;
            }
            if (e.KeyCode == (Keys) KeyBind.Right && _holdRight) {
                _input.Direction &= ~Direction.Right;
                send = true;
                _holdRight = false;
            }
            if (e.KeyCode == (Keys) KeyBind.Left && _holdLeft) {
                _input.Direction &= ~Direction.Left;
                send = true;
                _holdLeft = false;
            }
            if (send)
                _gameClient.UpdateInput(_input);
        }

        public void AddGamePanelControl(Control control) {
            gamePanel.Controls.Add(control);
        }

        public void RemoveGamePanelControl(Control control1, Control control2) {
            control1.Controls.Remove(control2);
        }

        public void UpdateControlPosition(Control control, Point point) {
            control.Location = point;
        }

        public void UpdatePictureBoxImage(PictureBox pictureBox, Image image) {
            pictureBox.Image = image;
        }

        public void UpdatePlayerScore(int score) {
            scoreLabel2.Text = score.ToString();
        }

        private void chatTB_TextChanged(object sender, EventArgs e) {
            chatTB.SelectionStart = chatTB.TextLength;
            chatTB.ScrollToCaret();
        }

        private void ClientForm_Load(object sender, EventArgs e) {
            if (CanConnect())
                connectButton.PerformClick();
        }

        private void tracefileCB_CheckedChanged(object sender, EventArgs e) {
            tracefileTB.Enabled = tracefileCB.Checked;
        }
    }
}
