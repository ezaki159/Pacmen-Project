using System;
using System.Collections.Generic;
using CommonInterfaces;

namespace Server {
    class ServerPuppetService : AbstractPuppetSlave, IServerService {
        private readonly IServerService _server;

        public ServerPuppetService(string pid, IServerService server) : base(pid) {
            _server = server;
            server.SetPid(pid);
        }

        public void RegisterPlayer(IClientService player) {
            CheckFreeze();
            var proxy = new PuppetClientProxy(player);
            lock (_proxies) {
                _proxies[player.GetPid()] = proxy;
            }
            _server.RegisterPlayer(proxy);
        }

        public void ReceiveInput(IClientService player, Input input) {
            CheckFreeze();
            _server.ReceiveInput(player, input);
        }

        public int GetNumberOfPlayers() {
            CheckFreeze();
            return _server.GetNumberOfPlayers();
        }

        public IEnumerable<IClientService> GetClientList() {
            CheckFreeze();
            return _server.GetClientList();
        }

        public GameState GetGameState(int roundId) {
            CheckFreeze();
            return _server.GetGameState(roundId);
        }

        public override string LocalState(int roundID) {
            return _server.GetGameState(roundID).ToString();
        }

        public void RegisterReplica(IServerService replica, String id) {
            _server.RegisterReplica(replica, id);
        }

        public Dictionary<string, IClientService> GetPlayerList() {
            return _server.GetPlayerList();
        }

        public Dictionary<String, IServerService> GetReplicaDictionary() {
            return _server.GetReplicaDictionary();
        }

        public Dictionary<string, Input> GetInputList() {
            return _server.GetInputList();
        }

        public List<GameState> GetStateList() {
            return _server.GetStateList();
        }

        public void AddReplica(IServerService replica, String id) {
            _server.AddReplica(replica, id);
        }

        public void AddPlayer(IClientService player) {
            _server.AddPlayer(player);
        }

        public void UpdateInput(string nickname, Input input) {
            _server.UpdateInput(nickname, input);
        }

        public void UpdateState(GameState state) {
            _server.UpdateState(state);
        }

        public void PrimaryChange(String id, IServerService newPrimary) {
            _server.PrimaryChange(id, newPrimary);
        }

        public int GetMsecPerRound() {
            return _server.GetMsecPerRound();
        }

        public void ReceiveImAlive() {
            _server.ReceiveImAlive();
        }

        public string GetPid() {
            CheckFreeze();
            return _server.GetPid();
        }

        public void SetPid(string pid) {
            CheckFreeze();
            _server.SetPid(pid);
        }
    }
}
