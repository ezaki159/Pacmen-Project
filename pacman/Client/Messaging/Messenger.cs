using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommonInterfaces;
using CommonInterfaces.Messaging;

namespace Client.Messaging {
    internal class Messenger {
        private readonly VectorClock _clock;
        private readonly List<Message> _messages = new List<Message>();
        private readonly List<Message> _sentMessages = new List<Message>();
        public List<IClientService> ClientList { get; } = new List<IClientService>();
        private readonly ClientService _client;

        public Messenger(int nMessengers, ClientService client) {
            _clock = new VectorClock(nMessengers, client.GetPlayerIndex());
            _client = client;
        }

        public void ReceiveMessage(Message message) {
            ReceiveMessage(message, true);
        }

        private void ReceiveMessage(Message message, bool isNew) {
            if (message.Clock.IsSuccessor(_clock)) {
                DeliverMessage(message);
            } else if (isNew) {
                _messages.Add(message);  
            }
        }

        private void DeliverMessage(Message message) {
            _clock.Update(message.Clock);
            _client.DeliverMessage(message);
            var toSend = new List<Message>();
            foreach (var queuedMessage in _messages.ToList()) {
                if (!queuedMessage.Clock.IsSuccessor(_clock)) continue;
                toSend.Add(queuedMessage);
                _messages.Remove(queuedMessage);
            }
            foreach (var queuedMessage in toSend) {
                DeliverMessage(queuedMessage);
            }
        }

        public void SendMessage(string content) {
            _clock.Increment();
            var message = new Message(_clock.Copy(), content, _client.GetPlayerIndex(), _client.GetNickname());
            foreach (var client in ClientList) {
                if (client.GetPlayerIndex() == _client.GetPlayerIndex()) continue;
                new Thread(() => {
                    _client.CheckDelay(client.GetPid());
                    client.ReceiveMessage(message);
                }).Start();
            }
            _sentMessages.Add(message);
        }

        public void UpdateClient(IClientService client) {
            foreach (var message in _sentMessages)
            {
                client.ReceiveMessage(message);
            }
        }
    }
}
