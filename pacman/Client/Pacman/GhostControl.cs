using System;
using System.Drawing;
using System.Windows.Forms;
using CommonInterfaces.Pacman;
using Color = CommonInterfaces.Pacman.Color;

namespace Client.Pacman {
    class GhostControl : EntityControl {
        public override void Init(Entity e) {
            Image img;

            switch (((Ghost) e).Color) {
                case Color.Orange:
                    img = Properties.Resources.yellow_guy;
                    break;
                case Color.Red:
                    img = Properties.Resources.red_guy;
                    break;
                case Color.Tomato:
                    img = Properties.Resources.pink_guy;
                    break;
                default:
                    throw new NotImplementedException("Unsupported Ghost color");
            }

            _pb = InitPictureBox(e, img);
            _p.Controls.Add(_pb);
        }

        public override void Update(ClientForm cf, Entity e) {
            cf.Invoke(new UpdateFormControlPositionDelegate(cf.UpdateControlPosition), _pb,
                new Point(e.x - e.hitboxRadius, e.y - e.hitboxRadius));
        }

        public GhostControl(Panel p) : base(p) {
        }
    }
}
