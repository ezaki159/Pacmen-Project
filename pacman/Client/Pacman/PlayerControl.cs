using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommonInterfaces;
using CommonInterfaces.Pacman;

namespace Client.Pacman {
    class PlayerControl : EntityControl {
        private delegate void UpdatePlayerScoreDelegate(int score);
        private delegate void UpdatePictureBoxImageDelegate(PictureBox pb, Image img);
        private Direction _playerDir;
        private bool _clientPlayer;
        private ClientForm _cf;
        private bool _dead;

        public PlayerControl(Panel p, bool clientPlayer, ClientForm cf) : base(p) {
            _clientPlayer = clientPlayer;
            _cf = cf;
        }

        public void UpdateScore(Player p) {
            _cf.Invoke(new UpdatePlayerScoreDelegate(_cf.UpdatePlayerScore), p.Score);
        }

        public override void Init(Entity e) {
            _pb = InitPictureBox(e, Properties.Resources.Right);
            _playerDir = Direction.Right;
            _p.Controls.Add(_pb);
            if (_clientPlayer)
                UpdateScore((Player) e);
        }

        public override void Update(ClientForm cf, Entity e) {
            Player p = (Player) e;

            if (_dead) return;

            if (p.Dead) {
                _cf.Invoke(new RemoveFormControlDelegate(_cf.RemoveGamePanelControl), _p, _pb);
                _dead = true;
                return;
            }

            _cf.Invoke(new UpdateFormControlPositionDelegate(_cf.UpdateControlPosition), _pb,
                new Point(p.x - p.hitboxRadius, p.y - p.hitboxRadius));
            Image img;
            switch (p.LastDir) {
                case Direction.Up:
                    img = Properties.Resources.Up;
                    break;
                case Direction.Down:
                    img = Properties.Resources.down;
                    break;
                case Direction.Left:
                    img = Properties.Resources.Left;
                    break;
                case Direction.Right:
                    img = Properties.Resources.Right;
                    break;
                default:
                    throw new NotImplementedException("Unsupported Player diretion");
            }
            if (p.LastDir != _playerDir) {
                _cf.Invoke(new UpdatePictureBoxImageDelegate(_cf.UpdatePictureBoxImage), _pb,
                    img);
                _playerDir = p.LastDir;
            }
            if (_clientPlayer)
                UpdateScore(p);
        }
    }
}
