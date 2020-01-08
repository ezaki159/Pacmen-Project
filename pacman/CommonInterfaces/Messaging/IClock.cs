using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonInterfaces.Messaging {
    public interface IClock {
        // Updates clock as required using the given clock.
        // Clock implementations must be of the same type.
        void Update(IClock otherClock);
        // Returns true if this clock comes after the parameter clock.
        // Clock implementations must be of the same type.
        bool IsSuccessor(IClock otherClock);

        void Increment();
    }
}
