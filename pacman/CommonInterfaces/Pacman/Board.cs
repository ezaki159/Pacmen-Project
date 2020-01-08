using System;

namespace CommonInterfaces.Pacman {
    [Serializable]
    public class Board {
        private readonly int WIDTH; 
        private readonly int HEIGHT;

        public Board(int width, int height) {
            WIDTH = width;
            HEIGHT = height;
        }

        public int Width => WIDTH;
        public int Height => HEIGHT;

        public void CollisionCheck(Entity e) { // if an entity collides with board bounds, it get repositioned inside
            bool collided = false;
            if (e.x - e.hitboxRadius < 0) { // left side
                e.x = e.hitboxRadius;
                collided = true;
            } else if (e.x + e.hitboxRadius > WIDTH) { // right side
                e.x = WIDTH - e.hitboxRadius;
                collided = true;
            }

            if (e.y - e.hitboxRadius < 0) { // top side, (0,0) is top left
                e.y = e.hitboxRadius;
                collided = true;
            } else if (e.y + e.hitboxRadius > HEIGHT) { // bottom side
                e.y = HEIGHT - e.hitboxRadius;
                collided = true;
            }

            if (collided) e.BoardCollision();
        }

        public Board Copy() {
            return new Board(WIDTH, HEIGHT);
        }
    }
}
