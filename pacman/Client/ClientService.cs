using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Client.Messaging;
using CommonInterfaces;
using CommonInterfaces.Messaging;

namespace Client {
    public abstract class ClientService : MarshalByRefObject, IClientService {
        private string _nickname;
        protected int _playerIndex;
        private string CLIENT_NAME;
        protected ClientForm _cf;
        private Messenger _messenger;
        protected Input _input = new Input();
        public string TracefilePath { set; get; }
        private TracefileReader _tracefileReader;
        private int _lastRound;
        private DateTime _lastRoundTimestamp = DateTime.Now;
        private int MSEC_PER_ROUND= -1;
        private Thread _noUpdateThread;
        private Boolean _serverSlow = false;
        protected List<GameState> _states = new List<GameState>();
        private readonly string _clientLock = "lock";
        private string _pid = "";
        private bool _frozen = false;
        private readonly ConcurrentDictionary<string, bool> _isDelayed = new ConcurrentDictionary<string, bool>(); // maps PID -> bool saying whether channel is delayed to PID
        private static readonly int DELAY = 5000;
        private IServerService _server;
        protected GameState LastGameState = null;

        internal IServerService ServerService {
            get => _server;
            set {
                _isDelayed[value.GetPid()] = false;
                _server = value;
            }
        }

        private delegate void AddMessage(string nickname, string text);

        private delegate void ChangeWindowText(string msg);

        protected ClientService(string nickname, ClientForm cf, string tracefilePath = null) {
            Nickname = nickname;
            _cf = cf;
            if (tracefilePath != null) 
                TracefilePath = tracefilePath;
            
            _noUpdateThread = new Thread(CheckRoundTimeSpan);
            _noUpdateThread.IsBackground = true;

        }

        private void CheckRoundTimeSpan()
        {
            while (true)
            {
                TimeSpan timespan = (DateTime.Now).Subtract(_lastRoundTimestamp);

                if ( timespan.TotalMilliseconds > MSEC_PER_ROUND*5) {
                    // server not responding, server may be slow
                    lock (_clientLock) {
                        _serverSlow = true;
                    }
                }
                //compute game states at same speed as server is supposed to
                if(_serverSlow )
                    SimulateGameState();

                Thread.Sleep(MSEC_PER_ROUND);
            }
        }

        public void InitTracefile() {
            if (TracefilePath != null) {
                _tracefileReader = new TracefileReader(TracefilePath);
                lock (_tracefileReader) {
                    _tracefileReader.Init();
                }
            }
        }

        public string Nickname {
            set {
                _nickname = value;
                CLIENT_NAME = "Client" + _nickname;
            }
        }

        public void UpdateInput(Input input) {
            _input = input;
        }

        private void SendInput(Input input) {
            if (_tracefileReader != null) {
                lock (_tracefileReader) {
                    if (_lastRound + 1 < _tracefileReader.Movements.Count)
                        input = _tracefileReader.Movements[++_lastRound];
                    else
                        input = new Input(); // None input after tracefile is at the end
                }
            }
            lock (_clientLock) {
                ServerService.ReceiveInput(this, input);
            }
        }

        public string GetNickname() {
            CheckFreeze();
            return _nickname;
        }

        public int GetPlayerIndex() {
            CheckFreeze();
            return _playerIndex;
        }

        public void SetPlayerIndex(int index) {
            CheckFreeze();
            _playerIndex = index;
            lock (_clientLock) {
                _messenger = new Messenger(ServerService.GetNumberOfPlayers(), this);
            }
        }

        public void AnnounceClient(IClientService client) {
            CheckFreeze();
            var pid = client.GetPid();
            _isDelayed[pid] = false;
            _messenger.ClientList.Add(client);
            CheckDelay(pid);
            client.RegisterClient(this);
            _messenger.UpdateClient(client);
        }

        protected abstract void SimulateGameState();

        public virtual void ProcessGameState(GameState state, Boolean fromServer) {
            if (fromServer)
                CheckFreeze();
            DrawGameState(state);

            if (!fromServer) return; // We just need to draw the state, no need to do anything else

            if (fromServer) {
                lock (_clientLock) {
                    _serverSlow = false;
                }      
            }

            _lastRound = state.RoundTimestamp;
            LastGameState = state.Copy();

            if( !_serverSlow ) SendInput(_input);

            if (state.RoundTimestamp >= 0) _states.Add(state);

            _lastRoundTimestamp = DateTime.Now;

            if (MSEC_PER_ROUND == -1) {
                CheckDelay(ServerService.GetPid());
                lock (_clientLock) {
                    MSEC_PER_ROUND = ServerService.GetMsecPerRound();
                }
                _noUpdateThread.Start();
            }
        }

        protected abstract void DrawGameState(GameState state);

        public void ReceiveMessage(Message message) {
            CheckFreeze();
            _messenger.ReceiveMessage(message);
        }

        public void EndGame(string result) {
            _cf.Invoke(new ChangeWindowText(_cf.ChangeWindowText), result);
        }

        public void RegisterClient(IClientService client) {
            CheckFreeze();
            var pid = client.GetPid();
            _isDelayed[pid] = false;
            _messenger.ClientList.Add(client);
        }

        public GameState GetGameState(int roundId) {
            if (roundId > _states.Count)
                throw new Exception("Round has not happened yet!");
            return _states[roundId];
        }

        public void ChangeServer(IServerService newServer) {
            CheckFreeze();
            lock (_clientLock) {
                ServerService = newServer;
            }
            _cf.Invoke(new AddMessage(_cf.AddMessage), "System", "Server changed.");
        }

        public string GetPid() => _pid;

        public void SetPid(string pid) => _pid = pid;

        internal void SendMessage(string text) {
            _messenger.SendMessage(text);
        }

        internal void DeliverMessage(Message message) {
            _cf.Invoke(new AddMessage(_cf.AddMessage), message.SenderNickname, message.Text);
        }

        // ------------------ PuppetMaster logic ---------------------

        protected void CheckFreeze() {
            lock (this) {
                while (_frozen)
                    Monitor.Wait(this);
            }
        }

        internal void CheckDelay(string pid) {
            bool delayed;
            lock (_isDelayed) {
                delayed = _isDelayed[pid];
            }
            if (delayed) Thread.Sleep(DELAY);
        }

        public void GlobalStatus() {
            var msg = "Server is " + (_serverSlow ? "dead" : "alive");
            _cf.Invoke(new AddMessage(_cf.AddMessage), "GlobalStatus", msg);
        }

        public void Crash() {
            Process.GetCurrentProcess().Kill();
        }

        public void Freeze() {
            lock (this) {
                _frozen = true;
            }
        }

        public void Unfreeze() {
            lock (this) {
                _frozen = false;
                Monitor.PulseAll(this);
            }
        }

        public void InjectDelay(string pid) {
            _isDelayed[pid] = true;
        }

        public string LocalState(int roundID) {
            return GetGameState(roundID).ToString();
        }
    }
}
