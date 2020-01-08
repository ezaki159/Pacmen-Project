using System;
using System.Drawing;
using System.Windows.Forms;
using CommonInterfaces.Pacman;

namespace Client.Pacman {
    class BoardControl {
        private Panel _panel;

        public Panel Panel {
            get { return _panel; }
            set { _panel = value; }
        }

        public void Init(Board b) {
            Panel = new Panel();
            Panel.Location = new Point(0, 0);
            Panel.Size = new Size(b.Width, b.Height);
            Panel.BorderStyle = BorderStyle.FixedSingle;
        }
    }
}
