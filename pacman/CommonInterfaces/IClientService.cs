using CommonInterfaces.Messaging;

namespace CommonInterfaces {
    public interface IClientService {
        string GetNickname();
        int GetPlayerIndex();
        void SetPlayerIndex(int index);
        void AnnounceClient(IClientService client);
        void ProcessGameState(GameState state, bool server);
        void ReceiveMessage(Message message);
        void EndGame(string result);
        void RegisterClient(IClientService client);
        GameState GetGameState(int roundId);
        void ChangeServer(IServerService newServer);
        string GetPid();
        void SetPid(string pid);
    }
}
