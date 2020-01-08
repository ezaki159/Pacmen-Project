using System;

namespace CommonInterfaces.Pacman {
    [Serializable]
    public class Coin : Entity {
        public bool Consumed { get; set; } = false;
        public Coin(string id, int size) : base(id, size) {
            hitboxRadius = size;
        }

        public Coin Copy() {
            return new Coin(Id, Size) {
                x = x,
                y = y,
                Consumed = Consumed
            };
        }
    }
}