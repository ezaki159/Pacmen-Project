using System;

namespace CommonInterfaces.Messaging {
    [Serializable]
    public class Message {
        public IClock Clock { get; set; }
        public string Text { get; set; }
        public int Sender { get; set; }
        public string SenderNickname { get; set; }

        public Message(IClock clock, string text, int sender, string senderNickname) {
            Clock = clock ?? throw new ArgumentNullException(nameof(clock));
            Text = text ?? throw new ArgumentNullException(nameof(text));
            SenderNickname = senderNickname ?? throw new ArgumentNullException(nameof(senderNickname));
            Sender = sender;
        }
    }
}
