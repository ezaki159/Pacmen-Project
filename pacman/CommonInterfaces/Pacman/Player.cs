using System;

namespace CommonInterfaces.Pacman {
    [Serializable]
    public class Player : Entity {
        private readonly int SPEED;
        private Direction _lastDir = Direction.Right;

        public int Score { get; set; } = 0;
        public bool Dead { get; set; } = false;

        public Direction LastDir {
            get { return _lastDir; }
            set { _lastDir = value; }
        }

        public Player(string clientId, int speed, int size) : base(clientId, size) {
            SPEED = speed;
        }

        public void UpdatePosition(Input input) {
            if (Dead) return;

            if (input.Direction.HasFlag(Direction.Up)) {
                y -= SPEED;
                _lastDir = Direction.Up;
            }
            if (input.Direction.HasFlag(Direction.Down)) {
                y += SPEED;
                _lastDir = Direction.Down;
            }
            if (input.Direction.HasFlag(Direction.Right)) {
                x += SPEED;
                _lastDir = Direction.Right;
            }
            if (input.Direction.HasFlag(Direction.Left)) {
                x -= SPEED;
                _lastDir = Direction.Left;
            }
        }

        public bool Collided(Entity e) {
            var d = (int) Math.Sqrt(Math.Pow(x - e.x, 2) + Math.Pow(y - e.y, 2));
            return (d < e.hitboxRadius + hitboxRadius);
        }

        public Player Copy() {
            return new Player(Id, SPEED, Size) {
                x = x,
                y = y,
                LastDir = LastDir,
                Score = Score,
                Dead = Dead
            };
        }
    }
}
