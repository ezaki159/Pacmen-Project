using System;
using System.Linq;
using System.Threading;
using CommonInterfaces.Messaging;

namespace Client.Messaging {
    [Serializable]
    internal class VectorClock : IClock {
        public int[] Clocks { get; }
        public int OwnIndex { get; set; }

        public VectorClock(int nClocks, int ownIndex) {
            Clocks = new int[nClocks];
            OwnIndex = ownIndex;
        }

        private VectorClock(VectorClock otherClock) {
            Clocks = otherClock.Clocks.ToArray();
            OwnIndex = otherClock.OwnIndex;
        }

        public void Update(IClock otherIClock) {
            var otherClock = otherIClock as VectorClock;
            lock (this) {
                if (otherClock == null || otherClock.Clocks.Length != Clocks.Length) {
                    throw new Exception("Incompatible clocks"); // TODO specialize exception
                }

                for (var i = 0; i < Clocks.Length; ++i) {
                    Clocks[i] = Math.Max(Clocks[i], otherClock.Clocks[i]);
                }
            }
        }

        public bool IsSuccessor(IClock otherIClock) {
            var otherClock = otherIClock as VectorClock;
            lock (this) {
                if (otherClock == null || otherClock.Clocks.Length < Clocks.Length) {
                    throw new Exception("Incompatible clocks"); // TODO specialize exceptions
                }


                // FIXME document this a bit better
                if (Clocks[OwnIndex] != otherClock.Clocks[OwnIndex] + 1) {
                    return false;
                }

                return !Clocks.Where((ts, i) => i != OwnIndex && ts > otherClock.Clocks[i]).Any();
            }
        }

        public void Increment() {
            lock (this) {
                ++Clocks[OwnIndex];
            }
        }

        public VectorClock Copy() {
            return new VectorClock(this);
        }
    }
}
