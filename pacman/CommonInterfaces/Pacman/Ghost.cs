using System;

namespace CommonInterfaces.Pacman {
    public enum Color {
        Red,
        Tomato,
        Orange
    }

    public enum GhostType {
        Type1,
        Type2
    }

    [Serializable]
    public class Ghost : Entity {
        public Color Color { get; set; }
        public int Speed { get; set; }
        public int Direction { get; set; }
        public GhostType GhostType { get; set; }

        public Ghost(string id, Color color, int direction, int speed, int size, GhostType ghostType) : base(id, size) {
            Color = color;
            Direction = direction;
            Speed = speed;
            GhostType = ghostType;
        }

        public void UpdatePosition() {
            x += (int) (Speed * Math.Cos(Direction * Math.PI / 180)); // Cos function receive radians
            y += (int) (Speed * Math.Sin(Direction * Math.PI / 180));
        }

        public override void BoardCollision() {
            InvertSpeed();
        }

        public void InvertSpeed() {
            if (GhostType == GhostType.Type1)
                Direction = (Direction + 180) % 360; // reverse direction
            else
                Direction = (Direction + 90) % 360; // reverse direction
        }

        public Ghost Copy() {
            return new Ghost(Id, Color, Direction, Speed, Size, GhostType) {
                x = x,
                y = y
            };
        }
    }
}
