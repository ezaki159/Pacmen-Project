using System;
using System.Windows.Forms;

namespace CommonInterfaces {
    [Flags]
    public enum Direction {
        None = 0,
        Up = 1,
        Down = 2,
        Left = 4,
        Right = 8,
    }

    public enum KeyBind {
        Up = Keys.W,
        Down = Keys.S,
        Left = Keys.A,
        Right = Keys.D
    }

    [Flags]
    public enum Action {
        None = 0
    }

    [Serializable]
    public class Input {
        public Direction Direction { get; set; } = Direction.None;
        public Action Action { get; set; } = Action.None;
    }
}
