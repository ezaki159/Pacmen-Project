using System;
using System.Collections.Generic;

namespace CommonInterfaces {
    [Serializable]
    public abstract class GameState {
        public List<Pacman.Player> NewDeadPlayers = new List<Pacman.Player>();

        public int RoundTimestamp { get; set; }

        public abstract GameState NextState(Dictionary<string, Input> Inputs);
        public abstract GameState NextState();

        public abstract bool GameOver();

        public abstract GameState Copy();
    }
}
