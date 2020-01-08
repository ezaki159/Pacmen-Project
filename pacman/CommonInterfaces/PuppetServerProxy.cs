using System;
using System.Collections.Generic;

namespace CommonInterfaces {
    public class PuppetServerProxy : PuppetProxy, IServerService {
        private readonly IServerService _server;

        public PuppetServerProxy(IServerService server) => _server = server;

        public void RegisterPlayer(IClientService player) {
            CheckDelay();
            if (player is PuppetClientProxy proxy)
                player = proxy.Client;
            _server.RegisterPlayer(player);
        }

        public void ReceiveInput(IClientService player, Input input) {
            CheckDelay();
            if (player is PuppetClientProxy proxy)
                player = proxy.Client;
            _server.ReceiveInput(player, input);
        }

        public int GetNumberOfPlayers() {
            CheckDelay();
            return _server.GetNumberOfPlayers();
        }

        public IEnumerable<IClientService> GetClientList() {
            CheckDelay();
            return _server.GetClientList();
        }

        public GameState GetGameState(int roundId) {
            CheckDelay();
            return _server.GetGameState(roundId);
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
            CheckDelay();
            return _server.GetPid();
        }

        public void SetPid(string pid) {
            CheckDelay();
            _server.SetPid(pid);
        }
    }
}
