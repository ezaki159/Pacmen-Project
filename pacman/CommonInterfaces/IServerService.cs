using System;
using System.Collections.Generic;

namespace CommonInterfaces {
    public interface IServerService {
        void RegisterPlayer(IClientService player);
        void ReceiveInput(IClientService player, Input input);
        int GetNumberOfPlayers();
        IEnumerable<IClientService> GetClientList();

        // Primary methods for Replicas
        void RegisterReplica(IServerService replica, String id);

        Dictionary<string, IClientService> GetPlayerList();
        Dictionary<String, IServerService> GetReplicaDictionary();
        Dictionary<string, Input> GetInputList();
        List<GameState> GetStateList();

        // Replica methods
        void AddReplica(IServerService replica, String id);

        void AddPlayer(IClientService player);
        void UpdateInput(string nickname, Input input);
        void UpdateState(GameState state);
        void PrimaryChange(String id, IServerService newPrimary);

        int GetMsecPerRound();
        void ReceiveImAlive();
        string GetPid();
        void SetPid(string pid);
    }
}
