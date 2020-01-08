using System;
using System.Collections.Generic;
using CommonInterfaces;
using CommonInterfaces.Pacman;
using GameState = CommonInterfaces.GameState;

namespace Server {
    public class PacmanServerService : ServerService {
        private const int PLAYER_SPEED = 5;
        private const int GHOST_SPEED = 5;
        private const int PLAYER_SPRITE_SIZE = 25;
        private const int COIN_SPRITE_SIZE = 15;
        private const int GHOST_SPRITE_SIZE = 30;
        private const int GRID_CELL_SIZE = 40;

        private CommonInterfaces.Pacman.GameState _state = new CommonInterfaces.Pacman.GameState();

        public PacmanServerService(int numberPlayers, int msecPerRound, ServerForm sf) :
            base(numberPlayers, msecPerRound, sf) {
            _state.TotalPlayers = numberPlayers;
        }

        public PacmanServerService(ServerForm sf) : base(sf) {            
        }

        protected override void ServerSetup(IClientService client) {
            var player = new Player(client.GetNickname(), PLAYER_SPEED, PLAYER_SPRITE_SIZE);
            _state.Players.Add(player);
        }

        protected override GameState CalculateInitialState() {
            _state.Board = new Board(GRID_CELL_SIZE * 9, GRID_CELL_SIZE * 8); // 9 columns, 8 lines board
 
            SetupPlayers();
            SetupCoins();
            SetupGhosts();
            SetupWalls();

            return _state;
        }

        protected override GameState CalculateState(GameState state) {
            _state = (CommonInterfaces.Pacman.GameState) state;
            GameState nextState = _state.NextState(Inputs);

            foreach (Player p in nextState.NewDeadPlayers) {
                _form.Invoke(new UpdateServerLogDelegate(_form.UpdateServerLogTB), $"Player {p.Id} died with {p.Score} coins.");
            }
            nextState.NewDeadPlayers.Clear();

            return nextState;
        }

        private void SetupPlayers() {
            for (int i = 0; i < _state.Players.Count; i++) {
                _state.Players[i].SetGridPosition(GRID_CELL_SIZE, 0, i);
            }
        }

        private void SetupCoins() {
            _state.Coins = CoinFactory.GetCoins(COIN_SPRITE_SIZE, GRID_CELL_SIZE);
            _state.TotalCoins = _state.Coins.Count;
        }

        private void SetupGhosts() {
            Ghost g = new Ghost("ghost1", Color.Orange, 0, GHOST_SPEED, GHOST_SPRITE_SIZE, GhostType.Type1);
            g.SetGridPosition(GRID_CELL_SIZE, 4, 6); // grid, every object starts in a position multiple of 40 and is positioned in the center of grid
            _state.Ghosts.Add(g);
            g = new Ghost("ghost2", Color.Red, 0, GHOST_SPEED, GHOST_SPRITE_SIZE, GhostType.Type1);
            g.SetGridPosition(GRID_CELL_SIZE, 3, 1);
            _state.Ghosts.Add(g);
            g = new Ghost("ghost3", Color.Tomato, 45, GHOST_SPEED, GHOST_SPRITE_SIZE, GhostType.Type2);
            g.SetGridPosition(GRID_CELL_SIZE, 7, 0);
            _state.Ghosts.Add(g);
        }

        private void SetupWalls() {
            Wall w = new Wall("wall1", 15, 110);
            w.SetGridPosition(GRID_CELL_SIZE, 2, 1);
            _state.Walls.Add(w);
            w = new Wall("wall2", 15, 110);
            w.SetGridPosition(GRID_CELL_SIZE, 3, 6);
            _state.Walls.Add(w);
            w = new Wall("wall3", 15, 110);
            w.SetGridPosition(GRID_CELL_SIZE, 6, 1);
            _state.Walls.Add(w);
            w = new Wall("wall4", 15, 110);
            w.SetGridPosition(GRID_CELL_SIZE, 7, 6);
            _state.Walls.Add(w);
        }

        protected override string GetWinnerId() {
            int maxScore = 0;
            string winnerId = "";

            foreach (Player p in _state.Players) {
                if (p.Score >= maxScore) {
                    maxScore = p.Score;
                    winnerId = p.Id;
                }
            }

            return winnerId;
        }

    }
}
