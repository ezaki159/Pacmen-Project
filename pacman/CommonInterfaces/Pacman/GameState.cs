using System;
using System.Collections.Generic;
using System.Text;

namespace CommonInterfaces.Pacman {
    [Serializable]
    public class GameState : CommonInterfaces.GameState {
        public List<Player> Players { get; set; } = new List<Player>();
        public List<Coin> Coins { get; set; } = new List<Coin>(); 
        public List<Ghost> Ghosts { get; set; } = new List<Ghost>();
        public List<Wall> Walls { get; set; } = new List<Wall>();

        private Dictionary<string, Input> lastInputs = new Dictionary<string, Input>();


        public Board Board { get; set; }

        public int TotalPlayers { get; set; }
        public int TotalCoins { get; set; }

        public override CommonInterfaces.GameState NextState() {
            // position change
            foreach (var g in Ghosts)
                g.UpdatePosition();
            foreach (var p in Players)
                p.UpdatePosition(lastInputs[p.Id]);

            // collision detection
            // board collisions
            foreach (var g in Ghosts)
                Board.CollisionCheck(g);
            foreach (var p in Players)
                Board.CollisionCheck(p);

            // entities collision
            foreach (var p in Players) // player-coin collision
                foreach (var c in Coins)
                    CheckEntityCollision(p, c);
            foreach (var p in Players) // player-ghost collision
                foreach (var g in Ghosts)
                    CheckEntityCollision(p, g);
            foreach (var p in Players) // player-wall collision
                foreach (var w in Walls)
                    CheckEntityCollision(p, w);
            foreach (var g in Ghosts) // ghost-wall collision
                foreach (var w in Walls)
                    CheckEntityCollision(g, w);

            return (CommonInterfaces.GameState)this;
        }

        public void UpdateInput( Player player, Input input) {
            lock (this) {
                lastInputs[player.Id] = input;
            }
        }

        public override CommonInterfaces.GameState NextState(Dictionary<string, Input> Inputs) {
            lastInputs.Clear();
            foreach ( var input in Inputs) {
                lastInputs.Add(input.Key, input.Value);
            }

            return NextState();
        }

        private void CheckEntityCollision(Player p, Coin c) {
            if (p.Collided(c) && !c.Consumed) {
                p.Score++;
                c.Consumed = true;
                TotalCoins--;
            }
        }

        private void CheckEntityCollision(Player p, Ghost g) {
            if (p.Collided(g) && !p.Dead) {
                PlayerDead(p);
            }
        }

        private void CheckEntityCollision(Player p, Wall w) {
            if (p.CollidedW(w) && !p.Dead) {
                PlayerDead(p);
            }
        }

        private void CheckEntityCollision(Ghost g, Wall w) {
            if (g.CollidedW(w))
                g.InvertSpeed();
        }

        private void PlayerDead(Player player) {
            player.Dead = true;
            TotalPlayers--;
            NewDeadPlayers.Add(player);
        }

        public override bool GameOver() => TotalPlayers == 0 || TotalCoins == 0;

        public override string ToString() {
            var stringBuilder = new StringBuilder();
            foreach (var g in Ghosts)
                stringBuilder.AppendLine($@"M, {g.x}, {g.y}");
            for (var i = 0; i < Players.Count; ++i) {
                var p = Players[i];
                var status = p.Dead ? "L" : "P";
                stringBuilder.AppendLine($@"P{i}, {status}, {p.x}, {p.y}");
            }
            foreach (var c in Coins)
                stringBuilder.AppendLine($@"C, {c.x}, {c.y}");
            return stringBuilder.ToString();
        }

        public override CommonInterfaces.GameState Copy() {
            lock (this) {
                var gameState = new GameState();
                gameState.Players = Players.ConvertAll((p) => p.Copy());
                gameState.Coins = Coins.ConvertAll((c) => c.Copy());
                gameState.Ghosts = Ghosts.ConvertAll((g) => g.Copy());
                gameState.Walls = Walls.ConvertAll((w) => w.Copy());
                gameState.Board = Board.Copy();
                //last_inputs
                foreach (var input in lastInputs) {
                    gameState.lastInputs.Add(input.Key, input.Value);
                }
                return gameState;
            }
        } 
    }
}
