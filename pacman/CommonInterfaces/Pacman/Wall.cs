using System;

namespace CommonInterfaces.Pacman {
    [Serializable]
    public class Wall : Entity { // FIXME should this really be an Entity?
        private readonly int WIDTH;
        private readonly int HEIGHT;

        public Wall(string id, int width, int height) : base(id, 0) {
            WIDTH = width;
            HEIGHT = height;
        }

        public int Width => WIDTH;
        public int Height => HEIGHT;

        public Wall Copy() {
            return new Wall(Id, WIDTH, HEIGHT) {
                x = x,
                y = y
            };
        }
    }
}
