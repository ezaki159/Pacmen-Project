using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using CommonInterfaces;

namespace Server {
    public abstract class ServerService : MarshalByRefObject, IServerService, IPuppet {
        private Dictionary<string, IClientService> _clientList = new Dictionary<string, IClientService>();
        protected Status _serviceStatus;
        protected readonly ServerForm _form; // for form communication 
        protected Dictionary<string, Input> Inputs = new Dictionary<string, Input>();
        private int NUM_PLAYERS;
        private int MSEC_PER_ROUND;
        private bool _running = true;
        private bool _acceptingInput;
        private bool _isReplica;
        private DateTime _lastPrimaryHeartbeat;
        private bool _primaryDown = false;
        private Thread _imAliveThread;
        internal IServerService PrimaryServer;
        private string _pid = "";
        private bool _frozen = false;
        private bool _heartbeated = false;
        private readonly ConcurrentDictionary<string, bool> _isDelayed = new ConcurrentDictionary<string, bool>(); // maps PID -> bool saying whether channel is delayed to PID
        protected static readonly int DELAY = 5000;
        public String Id { get; set; }

        public int RoundTimestamp { get; set; } = -1;
        public List<IClientService> ClientList => _clientList.Values.ToList();
        private List<GameState> _states = new List<GameState>();
        private Dictionary<String, IServerService> _replicas = new Dictionary<string, IServerService>(); // ID -> replica

        private delegate void UpdateStatusDelegate(string name, System.Drawing.Color color);
        private delegate void AddPlayerDelegate(string nick);
        protected delegate void UpdateServerLogDelegate(string msg);

        public enum Status {
            WaitingPlayers,
            GameStarted,
            GameEnd,
            ReplicaStandby
        }

        protected ServerService(int numberPlayers, int msecPerRound, ServerForm sf): this() {
            NUM_PLAYERS = numberPlayers;
            MSEC_PER_ROUND = msecPerRound;
            _form = sf;
            _isReplica = false;
        }

        protected ServerService(ServerForm sf) : this() {
            _form = sf;
            _isReplica = true;
        }

        private ServerService() {
            _imAliveThread = new Thread(new
                ThreadStart(CheckHeartBeats));
            _imAliveThread.IsBackground = true;
        }

        private void CheckHeartBeats() {
            if (_isReplica) {
                while (true) {
                    if (!_heartbeated) {
                        Thread.Sleep(MSEC_PER_ROUND);
                        continue; 
                    } 
                    TimeSpan diff = (DateTime.Now).Subtract(_lastPrimaryHeartbeat);
                    if (diff.TotalMilliseconds > MSEC_PER_ROUND * 3) {// didnt receive heartbeat in a long time
                        _primaryDown = true;
                    }

                    if (_primaryDown) {
                        _form.Invoke(new UpdateServerLogDelegate(_form.UpdateServerLogTB), "Primary server is down. Electing new leader");
                        _primaryDown = false;
                        if (ElectLeader()) {
                            _isReplica = false;

                            UpdateReplicasPrimary();
                            UpdateClientsServer();

                            _form.Invoke(new UpdateServerLogDelegate(_form.UpdateServerLogTB), $"I'm primary server!");

                            lock (PrimaryServer) {
                                Monitor.Pulse(PrimaryServer);
                            }
                            break;
                        }
                    }
                    Thread.Sleep(MSEC_PER_ROUND);
                }
            }

            if (!_isReplica) {//if primary
                while (true) {
                    BroadCastImAlive();
                    Thread.Sleep(MSEC_PER_ROUND/2);
                }
            }
        }

        public int GetMsecPerRound() {
            return MSEC_PER_ROUND;
        }

        private void BroadcastNewClient(IClientService newClient) {
            foreach (var client in ClientList) {
                new Thread(() => {
                    try {
                        CheckDelay(client.GetPid());
                        client.AnnounceClient(newClient);
                    } catch (Exception) { /* player may be dead */ }
                }).Start();
            }
        }

        public void RegisterPlayer(IClientService client) {
            CheckFreeze();
            var pid = client.GetPid();
            if (_isReplica) return;

            if (_serviceStatus == Status.GameStarted) {
                return; // TODO add exception
            }

            string nick = client.GetNickname();
            lock (_clientList) {
                if (_clientList.ContainsKey(nick)) {
                    return; // TODO add exception
                }
                client.SetPlayerIndex(_clientList.Count);
                BroadcastNewClient(client);
                _clientList.Add(nick, client);
                _isDelayed[pid] = false;
                ServerSetup(client);
                Monitor.Pulse(_clientList);               
            }
            UpdateReplicasPlayers(client);
            lock (Inputs) {
                Inputs[nick] = new Input();
            }

            _form.Invoke(new AddPlayerDelegate(_form.AddPlayer), client.GetNickname());
            _form.Invoke(new UpdateServerLogDelegate(_form.UpdateServerLogTB), $"Player {nick} has connected.");
        }

        protected abstract void ServerSetup(IClientService client);

        public Status GetServiceStatus() => _serviceStatus;

        internal void GameStart() { // start communication with client services to send game state
            GameState state = null;

            if (_isReplica) {
                // Get primary information and state
                lock (PrimaryServer) {
                    NUM_PLAYERS = PrimaryServer.GetNumberOfPlayers();
                    MSEC_PER_ROUND = PrimaryServer.GetMsecPerRound();                 
                    _replicas = PrimaryServer.GetReplicaDictionary();
                    _replicas.Remove(Id); // removes himself from the replicas list
                    _clientList = PrimaryServer.GetPlayerList();
                    foreach (var client in _clientList.Values) {
                        _isDelayed[client.GetPid()] = false;
                    }
                    Inputs = PrimaryServer.GetInputList();
                    _states = PrimaryServer.GetStateList();

                    // update replica UI
                    UpdateStatus(Status.ReplicaStandby);
                    foreach (string client in _clientList.Keys) {
                        _form.Invoke(new AddPlayerDelegate(_form.AddPlayer), client);
                    }
                    _imAliveThread.Start();
                    Monitor.Wait(PrimaryServer);
                }               
            } else {
                _imAliveThread.Start();
                UpdateStatus(Status.WaitingPlayers);
                lock (_clientList) {
                    while (_clientList.Count < NUM_PLAYERS) {
                        Monitor.Wait(_clientList);
                        if (!_running) return; // Someone might terminate the server while waiting for players
                    }
                }
            }

            UpdateStatus(Status.GameStarted);
            if (RoundTimestamp >= 0) { // initial state has RoundTimestamp = -1, this is in case the primary server crashes and a replica takes place
                lock (_states) {
                    state = _states[RoundTimestamp];
                }
            } else {
                state = CalculateInitialState(); // Initial state
                state.RoundTimestamp = RoundTimestamp;
                UpdateRoundTimestamp();
                UpdateReplicasState(state);
                UpdateClients(state);
            }
            _acceptingInput = true;

            while (_running && !state.GameOver()) {
                Thread.Sleep(MSEC_PER_ROUND);
                state = CalculateState(state);
                state.RoundTimestamp = RoundTimestamp;
                UpdateRoundTimestamp();
                UpdateReplicasState(state);
                UpdateClients(state);
                lock (_states) {
                    _states.Add(state.Copy());
                }
            }
            _acceptingInput = false;
            NotifyGameOver();
            UpdateStatus(Status.GameEnd);
        }

        protected abstract string GetWinnerId();

        private void NotifyGameOver() {
            string winnerId = GetWinnerId();
            foreach (var client in _clientList.Values) {
                new Thread(() => {
                    try {
                        CheckDelay(client.GetPid());
                        client.EndGame(client.GetNickname() == winnerId ? "You win!" : "You lose!");
                    } catch (Exception) { /* player may be dead */ }
                }).Start();
            }
        }

        public void ReceiveInput(IClientService player, Input input) {
            CheckFreeze();
            var pid = player.GetPid();
            CheckDelay(pid);
            if (!_acceptingInput) return;
            lock (Inputs) {             
                Inputs[player.GetNickname()] = input;
            }

            UpdateReplicasInputs(player.GetNickname(), input);
        }

        public int GetNumberOfPlayers() {
            return NUM_PLAYERS;
        }

        public IEnumerable<IClientService> GetClientList() {
            return ClientList;
        }

        private GameState GetGameState(int roundId) {
            CheckFreeze();
            lock (_states) {
                if (roundId >= _states.Count)
                    throw new Exception("Round has not happened yet"); // FIXME specialize exceptions
                return _states[roundId];
            }
        }

        protected virtual void UpdateRoundTimestamp() => ++RoundTimestamp;

        protected void UpdateClients(GameState state) {
            var threads = new List<Thread>();
            lock (_clientList) {
                foreach (var client in _clientList.Values) {
                    var thread = new Thread(() => {
                        try {
                            CheckDelay(client.GetPid());
                            client.ProcessGameState(state, true);
                        } catch (Exception) { /* player may be dead */ }
                    });
                    threads.Add(thread);
                    thread.Start();
                }
            }
            foreach (var thread in threads) {
                thread.Join();
            }
        }

        private void UpdateClientsServer() {
            var threads = new List<Thread>();
            lock (_clientList) {
                foreach (var client in _clientList.Values) {
                    var thread = new Thread(() => {
                        try {
                            CheckDelay(client.GetPid());
                            client.ChangeServer(this);
                        } catch (Exception) { /* player may be dead */ }
                    });
                    threads.Add(thread);
                    thread.Start();
                }
            }
            foreach (var thread in threads) {
                thread.Join();
            }
        }

        protected abstract GameState CalculateInitialState();
        protected abstract GameState CalculateState(GameState state);

        internal void StopServer() {
            //_running = false;
            lock (_clientList) {
                Monitor.PulseAll(_clientList); // Maybe the server is waiting for players
            }
        }

        private void UpdateStatus(Status status) {
            _serviceStatus = status;
            string name;
            string logText;
            System.Drawing.Color color;
            switch (status) {
                case Status.WaitingPlayers:
                    name = Constants.SERVER_WAITING_FOR_PLAYERS;
                    logText = Constants.SERVER_WAITING_FOR_PLAYERS_NOCAPS;
                    color = System.Drawing.Color.YellowGreen;
                    break;
                case Status.GameStarted:
                    name = Constants.SERVER_RUNNING;
                    logText = Constants.SERVER_RUNNING_NOCAPS;
                    color = System.Drawing.Color.Green;
                    break;
                case Status.GameEnd:
                    name = Constants.SERVER_NOT_RUNNING;
                    logText = Constants.SERVER_NOT_RUNNING_NOCAPS;
                    color = System.Drawing.Color.Red;
                    break;
                case Status.ReplicaStandby:
                    name = Constants.REPLICA_STANDBY;
                    logText = Constants.REPLICA_STANDBY_NOCAPS;
                    color = System.Drawing.Color.YellowGreen;
                    break;
                default:
                    throw new NotImplementedException("Unsupported Status");
            }
            _form.Invoke(new UpdateStatusDelegate(_form.UpdateServerStatusLabel), name, color);
            _form.Invoke(new UpdateServerLogDelegate(_form.UpdateServerLogTB), logText + ".");
        }

        // ----- Primary methods for Replicas -----
        public void RegisterReplica(IServerService replica, String id) {
            CheckFreeze();
            lock (_replicas) {
                foreach (IServerService rep in _replicas.Values) { // update replicas of new replicas
                    rep.AddReplica(replica, id);
                }

                _replicas[id] = replica;
            }
        }

        public int GetRoundTime() {
            return MSEC_PER_ROUND;
        }

        public Dictionary<string, IClientService> GetPlayerList() {
            CheckFreeze();
            lock (_clientList) {
                return _clientList;
            }
        }

        public Dictionary<String, IServerService> GetReplicaDictionary() {
            CheckFreeze();
            lock (_replicas) {
                return _replicas;
            }
        }

        public Dictionary<string, Input> GetInputList() {
            CheckFreeze();
            lock (Inputs) {
                return Inputs;
            }
        }

        public List<GameState> GetStateList() {
            CheckFreeze();
            lock (_states) {
                return _states;
            }
        }

        // ----- Replica methods -----
        public void ReceiveImAlive() {
            CheckFreeze();
            if (_isReplica) {
                _heartbeated = true;
                _lastPrimaryHeartbeat = DateTime.Now;
            }
        }

        public string GetPid() => _pid;

        public void SetPid(string pid) => _pid = pid;


        public void AddReplica(IServerService replica, String id) {
            CheckFreeze();
            lock (PrimaryServer) {
                _replicas[id] = replica;
            }
        }

        public void AddPlayer(IClientService player) {
            CheckFreeze();
            lock (PrimaryServer) {
                _clientList.Add(player.GetNickname(), player);
                _form.Invoke(new AddPlayerDelegate(_form.AddPlayer), player.GetNickname());
            }
        }

        public void UpdateInput(string nickname, Input input) {
            CheckFreeze();
            lock (PrimaryServer) {
                Inputs[nickname] = input;
            }
        }

        public void UpdateState(GameState state) {
            CheckFreeze();
            lock (PrimaryServer) {
                _states.Add(state);
            }
            RoundTimestamp = state.RoundTimestamp;
        }

        public void PrimaryChange(String id, IServerService newPrimary) {
            CheckFreeze();
            lock (_replicas) {
                _replicas.Remove(id);
            }
            PrimaryServer = newPrimary;
        }

        // ----- Update all Replicas methods -----

        private void BroadCastImAlive() {
            lock (_replicas) {
                foreach (IServerService replica in _replicas.Values) {
                    replica.ReceiveImAlive();
                }
            }
        }
        
        private void UpdateReplicasState(GameState state) {
            lock (_replicas) {
                foreach (IServerService replica in _replicas.Values) {
                    replica.UpdateState(state);
                }
            }
        }

        private void UpdateReplicasPlayers(IClientService player) {
            lock (_replicas) {
                foreach (IServerService replica in _replicas.Values) {
                    replica.AddPlayer(player);
                }
            }
        }

        private void UpdateReplicasInputs(string nickname, Input input) {
            lock (_replicas) {
                foreach (IServerService replica in _replicas.Values) {
                    replica.UpdateInput(nickname, input);
                }
            }
        }

        private void UpdateReplicasPrimary() {
            lock (_replicas) {
                foreach (IServerService replica in _replicas.Values) {
                    replica.PrimaryChange(Id, this);
                }
            }
        }

        private bool ElectLeader() { // return true if current replica has the lowest Id of all
            return !_replicas.Keys.ToArray().Any(repId => String.Compare(repId, Id) < 0);
        }

        // ------------------ PuppetMaster logic ---------------------

        protected void CheckFreeze() {
            lock (this) {
                while (_frozen)
                    Monitor.Wait(this);
            }
        }

        protected void CheckDelay(string pid) {
            bool delayed;
            if (!_isDelayed.ContainsKey(pid)) return;

            delayed = _isDelayed[pid];

            if (delayed) Thread.Sleep(DELAY);
        }

        public void GlobalStatus() {
            int numReplicas = _replicas.Count;
            var logString = $"Global Status: {numReplicas} replicas, ";
            logString += _isReplica ? "this is a replica" : "this is a primary node";
            _form.Invoke(new UpdateServerLogDelegate(_form.UpdateServerLogTB), logString);
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
            if (!_isDelayed.ContainsKey(pid)) return;
            _isDelayed[pid] = true;
        }

        public string LocalState(int roundID) {
            return GetGameState(roundID).ToString();
        }

    }
}
