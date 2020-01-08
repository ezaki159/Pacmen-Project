using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommonInterfaces.Pacman;

namespace Client.Pacman {
    class CoinControl : EntityControl {
        private bool _consumed = false;

        private delegate void AddDel(PictureBox pb);
        public override void Init(Entity e) {
            _pb = InitPictureBox(e, Properties.Resources.coin);
            _p.Controls.Add(_pb);
        }

        private void ReInit(Entity e) {
            _pb = InitPictureBox(e, Properties.Resources.coin);
            _p.Invoke(new AddDel(_p.Controls.Add), _pb);
        }

        public override void Update(ClientForm cf, Entity e) {
            Coin c = (Coin) e;
            if (c.Consumed && !_consumed) {
                cf.Invoke(new RemoveFormControlDelegate(cf.RemoveGamePanelControl), _p, _pb);
                _consumed = true;
            } else if (!c.Consumed && _consumed) {
                ReInit(e);
            }
        }

        public CoinControl(Panel p) : base(p) {
        }
    }
}
