using System;

namespace CommonInterfaces.Pacman {
    [Serializable] 
    public abstract class Entity {
        protected Entity(string id, int size) {
            Size = size;
            hitboxRadius = size / 2;
            Id = id;
        }

        public string Id { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int hitboxRadius { get; set; } // size of the sprite for collision detection (assuming circular hitbox and that x and y are in the center of the sprite)
        public int Size { get; set; }

        public virtual void BoardCollision() { }

        public void SetGridPosition(int gridSize, int newX, int newY) {
            x = newX * gridSize + gridSize / 2;
            y = newY * gridSize + gridSize / 2;
        }

        public bool CollidedW(Wall w) {
            int entityRight = x + hitboxRadius;
            int entityLeft = x - hitboxRadius;
            int entityBottom = y + hitboxRadius;
            int entityTop = y - hitboxRadius;

            return ((entityRight > w.x - w.Width / 2 && entityRight < w.x + w.Width / 2) ||
                    (entityLeft > w.x - w.Width / 2 && entityLeft < w.x + w.Width / 2)) &&
                   ((entityBottom > w.y - w.Height / 2 && entityBottom < w.y + w.Height / 2) ||
                    (entityTop > w.y - w.Height / 2 && entityTop < w.y + w.Height / 2));
        }
    }
}
