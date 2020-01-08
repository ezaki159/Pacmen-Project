using System.Drawing;
using System.Windows.Forms;
using CommonInterfaces.Pacman;

namespace Client.Pacman {
    public abstract class EntityControl {
        protected delegate void UpdateFormControlPositionDelegate(Control c, Point p);
        protected delegate void RemoveFormControlDelegate(Control c1, Control c2);
        protected PictureBox _pb;
        protected Panel _p;

        public EntityControl(Panel p) {
            _p = p;
        }

        public abstract void Init(Entity e);
        public abstract void Update(ClientForm cf, Entity e);

        protected PictureBox InitPictureBox(Entity entity, Image img) {
            PictureBox pb = new PictureBox();
            pb.Image = img;
            pb.Size = new Size(entity.hitboxRadius * 2, entity.hitboxRadius * 2);
            pb.Location = new Point(entity.x - entity.hitboxRadius, entity.y - entity.hitboxRadius);
            pb.BackColor = System.Drawing.Color.Transparent;
            pb.SizeMode = PictureBoxSizeMode.Zoom;
            pb.Name = entity.Id;
            return pb;
        }
    }
}
