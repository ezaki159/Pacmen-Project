using System.Collections.Generic;
using System.Windows.Forms;
using Client.Pacman;
using CommonInterfaces;
using CommonInterfaces.Pacman;
using GameState = CommonInterfaces.GameState;

namespace Client {
    class PacmanClientService : ClientService, IPuppet {
        private BoardControl _boardControl;
        private Dictionary<string, GhostControl> _ghostsDic = new Dictionary<string, GhostControl>();
        private Dictionary<string, PlayerControl> _playersDic = new Dictionary<string, PlayerControl>();
        private Dictionary<string, CoinControl> _coinsDic = new Dictionary<string, CoinControl>();
        private Dictionary<string, WallControl> _wallsDic = new Dictionary<string, WallControl>();

        delegate void AddFormPanelControlDelegate(Control c);

        public PacmanClientService(string nickname, ClientForm cf, string tracefilePath = null) : base(nickname, cf, tracefilePath) {           
        }

        protected override void DrawGameState(GameState state) {
            CommonInterfaces.Pacman.GameState pstate = (CommonInterfaces.Pacman.GameState) state;
            if (_boardControl == null) {
                InitGamePanel(pstate.Board);
                InitGhosts(pstate.Ghosts);
                InitPlayers(pstate.Players);
                InitCoins(pstate.Coins);
                InitWalls(pstate.Walls);
                _cf.Invoke(new AddFormPanelControlDelegate(_cf.AddGamePanelControl), _boardControl.Panel);
            } else {
                updateGhosts(pstate.Ghosts);
                updatePlayers(pstate.Players);
                updateCoins(pstate.Coins);
            }
        }

        protected override void SimulateGameState() {
            if (_states.Count - 1 < 0) return;
            var lastGameState = (CommonInterfaces.Pacman.GameState) LastGameState;
            List<CommonInterfaces.Pacman.Player> players = lastGameState.Players;
            CommonInterfaces.Pacman.Player player = players[_playerIndex];

            lastGameState.UpdateInput(player, _input);

            ProcessGameState(lastGameState.NextState(), false);
        }

        private void InitGamePanel(Board b) {
            _boardControl = new BoardControl();
            _boardControl.Init(b);
        }

        private void InitGhosts(List<Ghost> ghostsList) {
            foreach (Ghost g in ghostsList) {
                GhostControl gc = new GhostControl(_boardControl.Panel);
                gc.Init(g);
                _ghostsDic.Add(g.Id, gc);
            }
        }

        private void InitPlayers(List<Player> playersList) {
            foreach (Player p in playersList) {
                PlayerControl pc = new PlayerControl(_boardControl.Panel, GetNickname() == p.Id, _cf);
                pc.Init(p);
                _playersDic.Add(p.Id, pc);
            }
        }

        private void InitWalls(List<Wall> wallsList) {
            foreach (Wall w in wallsList) {
                WallControl wc = new WallControl(_boardControl.Panel);
                wc.Init(w);
                _wallsDic.Add(w.Id, wc);
            }
        }

        private void InitCoins(List<Coin> coinsList) {
            foreach (Coin c in coinsList) {
                CoinControl cc = new CoinControl(_boardControl.Panel);
                cc.Init(c);
                _coinsDic.Add(c.Id, cc);
            }
        }

        private void updateGhosts(List<Ghost> ghostsList) {
            foreach (Ghost g in ghostsList) {
                _ghostsDic[g.Id].Update(_cf, g);
            }
        }

        private void updatePlayers(List<Player> playersList) {
            foreach (Player p in playersList) {
                _playersDic[p.Id].Update(_cf, p);
            }
        }

        private void updateCoins(List<Coin> coinsList) {
            foreach (Coin c in coinsList) {
                _coinsDic[c.Id].Update(_cf, c);
            }
        }
    }
}
