using System;
using System.Drawing;
using System.Windows.Forms;
using CommonInterfaces.Pacman;

namespace Client.Pacman {
    class WallControl : EntityControl {
        public override void Init(Entity e) {
            Wall w = (Wall) e;
            _pb = new PictureBox();
            _pb.BackColor = System.Drawing.Color.MidnightBlue;
            _pb.Location = new Point(w.x - w.Width / 2, w.y - w.Height / 2);
            _pb.Name = w.Id;
            _pb.Size = new Size(w.Width, w.Height);
            _p.Controls.Add(_pb);
        }

        public override void Update(ClientForm cf, Entity e) {
        }

        public WallControl(Panel p) : base(p) {
        }
    }
}
